using Newtonsoft.Json;

namespace smd_es_worker.Models.Requests
{
    public class UpdateESReferenceDataRequestModel
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("csvS3Link")]
        public string CSVS3Link { get; set; }
    }
}