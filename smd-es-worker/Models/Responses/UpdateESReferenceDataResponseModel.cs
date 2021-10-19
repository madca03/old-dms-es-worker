using System.Collections.Generic;
using SeriousmdRabbitMQ.Models;

namespace smd_es_worker.Models.Responses
{
    public class UpdateESReferenceDataResponseModel<T> : BaseRpcResponseModel
    {
        public List<T> UpdatedDocs { get; set; }
    }
}