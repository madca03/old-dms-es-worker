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
        }

        protected override async Task ProcessMessage(string message, IBasicConsumer model, BasicDeliverEventArgs ea)
        {
            var (req, success) = DeserializeRequestMessage<UpdateESReferenceDataRequestModel>(message, ea);
            if (!success) return;

            var csv = await ReadCSVFromUrl<DoctorConditionsCSVModel>(req.CSVS3Link);
            var bulkDescriptor = new BulkDescriptor();
            var docs = new List<DoctorConditionsESModel>();
            
            foreach (var row in csv)
            {
                row.Specializations = row.RawSpecializations.Split("\n").Select(x => x.Trim()).ToList();
                row.OnWeb = row.RawOnWeb == "Yes";
                var doc = new DoctorConditionsESModel(row);
                docs.Add(doc);

                bulkDescriptor.Update<DoctorConditionsESModel>(u => u
                    .Index(esIndices.Conditions)
                    .Id(row.Id)
                    .Doc(doc)
                    .DocAsUpsert());
            }

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

            HandleSuccess(ea, new UpdateESReferenceDataResponseModel { UpdatedDocs = docs });
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