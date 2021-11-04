using Nest;
using Newtonsoft.Json;
using smd_es_worker.Models.CSV;

namespace smd_es_worker.Models.Elasticsearch
{
    public class DoctorHMOESModel : BaseReferenceDataESModel
    {
        [Text(Name = "title")]
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [Number(Name = "boost")]
        [JsonProperty("boost")]
        public double? Boost { get; set; }

        [Text(Name = "image")]
        [JsonProperty("image")]
        public string Image { get; set; }

        [Number(Name = "id")]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [Text(Name = "partnerCode")]
        [JsonProperty("partnerCode")]
        public string PartnerCode { get; set; }

        public DoctorHMOESModel()
        {
            
        }

        public DoctorHMOESModel(DoctorHMOCSVModel csv)
        {
            Id = csv.SQLId;
            Name = csv.Name;
            Image = csv.IllustrationImage;
            PartnerCode = csv.PartnerCode;
        }
    }
}