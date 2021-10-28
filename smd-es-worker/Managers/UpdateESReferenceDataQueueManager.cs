using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using SeriousmdRabbitMQ.Managers;
using SeriousmdRabbitMQ.Models;
using SeriousmdRabbitMQ.RabbitMQ;
using SeriousmdRabbitMQ.References;
using smd_es_worker.Clients;
using smd_es_worker.Config;
using smd_es_worker.Models.CSV;
using smd_es_worker.Models.Elasticsearch;
using smd_es_worker.Models.Requests;
using smd_es_worker.Models.Responses;
using smd_es_worker.References;

namespace smd_es_worker.Managers
{
    public class UpdateESReferenceDataQueueManager : BaseQueueManager
    {
        protected override string InputEvent => RMQEventTypes.UpdateESReferenceData;
        protected override string OutputEventSuccess => RMQEventTypes.UpdateESReferenceDataSuccess;
        protected override string OutputEventFailure => RMQEventTypes.UpdateESReferenceDataFailed;

        private readonly ElasticIndices esIndices;
        private readonly IElasticClient esClient;
        private readonly List<string> validRefDataTypes;
        
        public UpdateESReferenceDataQueueManager(
            ILogger logger, 
            RabbitMQClient rmq, 
            string queueName, 
            IConfiguration Configuration, 
            IServiceProvider serviceProvider) 
            : base(logger, rmq, queueName, Configuration, serviceProvider)
        {
            esIndices = serviceProvider.GetService<IOptions<ElasticIndices>>()?.Value;
            esClient = serviceProvider.GetService<ElasticsearchClient>()?.Client;
            validRefDataTypes = typeof(ReferenceDataTypes).GetFields()
                .Where(x => x.IsLiteral && x.FieldType == typeof(string))
                .Select(x => (string)x.GetValue(null))
                .ToList();
        }

        protected override async Task ProcessMessage(string message, IBasicConsumer model, BasicDeliverEventArgs ea)
        {
            var (req, success) = DeserializeRequestMessage<UpdateESReferenceDataRequestModel>(message, ea);
            if (!success) return;

            switch (req.Type)
            {
                case ReferenceDataTypes.CONDITIONS:
                    var (conditionDocs, conditionDocsBulkDescriptor) = await GetUpdatedDoctorConditionsDocs(req);
                    await DoBulkUpdate(conditionDocs, conditionDocsBulkDescriptor, req, ea);
                    break;
                
                case ReferenceDataTypes.SERVICES:
                    var (servicesDocs, servicesDocsBulkDescriptor) = await GetUpdatedDoctorServicesDocs(req);
                    await DoBulkUpdate(servicesDocs, servicesDocsBulkDescriptor, req, ea);
                    break;
                
                default:
                    HandleFailure(ea, new BaseErrorRpcResponseModel
                    {
                        Error = RMQResponseStatus.UNKNOWN_REF_DATA_TYPE.Message,
                        Request = req,
                        Status = RMQResponseStatus.UNKNOWN_REF_DATA_TYPE.StatusCode
                    });
                    break;
            }
        }

        protected override bool IsValid<T>(T req)
        {
            if (req is not UpdateESReferenceDataRequestModel request) return false;
            if (string.IsNullOrEmpty(request.Type) || string.IsNullOrEmpty(request.CSVS3Link)) return false;
            if (!validRefDataTypes.Contains(request.Type)) return false;
            return true;
        }

        private async Task DoBulkUpdate<T>(List<T> docs, BulkDescriptor bulkDescriptor, UpdateESReferenceDataRequestModel req, BasicDeliverEventArgs ea)
        {
            var bulkResponse = await esClient.BulkAsync(bulkDescriptor);
            if (!bulkResponse.IsValid)
            {
                HandleFailure(ea, new BaseErrorRpcResponseModel
                {
                    Error = bulkResponse.OriginalException,
                    Request = req,
                    Status = RMQResponseStatus.BULK_UPDATE_FAILED.StatusCode
                });
                return;
            }

            HandleSuccess(ea, new UpdateESReferenceDataResponseModel<T> { UpdatedDocs = docs });
        }

        private async Task<(List<DoctorConditionsESModel>, BulkDescriptor)> GetUpdatedDoctorConditionsDocs(UpdateESReferenceDataRequestModel req)
        {
            var csv = await ReadCSVFromUrl<DoctorConditionsCSVModel>(req.CSVS3Link);
            var bulkDescriptor = new BulkDescriptor();
            var docs = new List<DoctorConditionsESModel>();
            
            foreach (var row in csv)
            {
                row.Specializations = row.RawSpecializations.Split("\n").Select(x => x.Trim()).ToList();
                row.OnWeb = row.RawOnWeb == "Yes";
                row.UIType = row.RawUIType == "Illustrated" ? UITypes.ILLUSTRATED : UITypes.TEXT;
                var doc = new DoctorConditionsESModel(row);
                docs.Add(doc);
                
                var countDescriptor = new CountDescriptor<DoctorESModel>()
                    .Index(esIndices.Doctors)
                    .Query(q => q.Terms(t => t
                        .Field(f => f.Professional.Specialty.Suffix("keyword"))
                        .Terms(doc.Specializations)));

                var countResponse = await esClient.CountAsync<DoctorESModel>(_ => countDescriptor);
                if (!countResponse.IsValid) throw countResponse.OriginalException;

                doc.DoctorCount = (int) countResponse.Count;

                bulkDescriptor.Update<DoctorConditionsESModel>(u => u
                    .Index(esIndices.Conditions)
                    .Id(row.Id)
                    .Doc(doc)
                    .DocAsUpsert());
            }

            return (docs, bulkDescriptor);
        }
        
        private async Task<(List<DoctorServicesESModel>, BulkDescriptor)> GetUpdatedDoctorServicesDocs(UpdateESReferenceDataRequestModel req)
        {
            var csv = await ReadCSVFromUrl<DoctorServicesCSVModel>(req.CSVS3Link);
            var bulkDescriptor = new BulkDescriptor();
            var docs = new List<DoctorServicesESModel>();
            
            foreach (var row in csv)
            {
                row.OnWeb = row.RawOnWeb == "Yes";
                row.OnNSServices = row.RawOnNSServices == "Yes";
                row.UIType = row.RawUIType == "Illustrated" ? UITypes.ILLUSTRATED : UITypes.TEXT;
                var doc = new DoctorServicesESModel(row);
                docs.Add(doc);

                var countDescriptor = new CountDescriptor<DoctorESModel>()
                    .Index(esIndices.Doctors)
                    .Query(q => q.Term(t => t.Field(f => f.Services).Value(doc.Id)));

                var countResponse = await esClient.CountAsync<DoctorESModel>(_ => countDescriptor);
                if (!countResponse.IsValid) throw countResponse.OriginalException;

                doc.DoctorCount = (int) countResponse.Count;

                bulkDescriptor.Update<DoctorServicesESModel>(u => u
                    .Index(esIndices.Services)
                    .Id(row.Id)
                    .Doc(doc)
                    .DocAsUpsert());
            }

            return (docs, bulkDescriptor);
        }
        
        private async Task<IEnumerable<T>> ReadCSVFromUrl<T>(string url)
        {
            var client = new HttpClient();
            var stream = await client.GetStreamAsync(url);
            using var reader = new StreamReader(stream);

            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<T>().ToList();
            return records;
        }
    }
}