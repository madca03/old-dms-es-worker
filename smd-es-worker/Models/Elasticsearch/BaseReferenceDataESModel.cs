using Nest;
using Newtonsoft.Json;

namespace smd_es_worker.Models.Elasticsearch
{
    public class BaseReferenceDataESModel
    {
        [Text(Name = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [Keyword(Name = "uri")]
        [JsonProperty("uri")]
        public string URI { get; set; }
        
        [Object(Name = "meta")]
        [JsonProperty("meta")]
        public ReferenceDataMetaESModel Meta { get; set; }

        [Object(Name = "blurb")]
        [JsonProperty("blurb")]
        public ReferenceDataBlurbESModel Blurb { get; set; }
    }
}