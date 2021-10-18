using System.Collections.Generic;
using SeriousmdRabbitMQ.Models;
using smd_es_worker.Models.Elasticsearch;

namespace smd_es_worker.Models.Responses
{
    public class UpdateESReferenceDataResponseModel : BaseRpcResponseModel
    {
        public List<DoctorConditionsESModel> UpdatedDocs { get; set; }
    }
}