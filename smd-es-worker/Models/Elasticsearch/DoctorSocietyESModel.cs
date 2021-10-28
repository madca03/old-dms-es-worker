using Nest;
using Newtonsoft.Json;

namespace smd_es_worker.Models.Elasticsearch
{
    public class DoctorSocietyESModel
    {
        [Text(Name = "society")]
        [JsonProperty("society")]
        public string Society { get; set; }
        
        [Text(Name = "designation")]
        [JsonProperty("designation")]
        public string Designation { get; set; }
    }
}