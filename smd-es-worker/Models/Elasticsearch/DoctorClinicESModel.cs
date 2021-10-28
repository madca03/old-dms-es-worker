using System.Collections.Generic;
using Nest;
using Newtonsoft.Json;

namespace smd_es_worker.Models.Elasticsearch
{
    public class DoctorClinicESModel
    {
        [Number(Name = "clinicid")] 
        [JsonProperty("clinicid")]
        public int? ClinicId { get; set; }
        
        [Boolean(Name = "visible")]
        [JsonProperty("visible")]
        public bool? Visible { get; set; }
        
        [Text(Name = "clinicImage")]
        [JsonProperty("clinicImage")]
        public string ClinicImage { get; set; }
        
        [Text(Name = "address")]
        [JsonProperty("address")]
        public string Address { get; set; }
        
        [Text(Name = "city")]
        [JsonProperty("city")]
        public string City { get; set; }

        [Number(Name = "fee")]
        [JsonProperty("fee")]
        public double? Fee { get; set; }
        
        [Number(Name = "OnlineNewPxBookingFee")]
        [JsonProperty("onlineNewPxBookingFee")]
        public long? OnlineNewPxBookingFee { get; set; }

        [Number(Name = "OnlineOldPxBookingFee")]
        [JsonProperty("onlineOldPxBookingFee")]
        public long? OnlineOldPxBookingFee { get; set; }

        [Number(Name = "OnlinePaywallEnabled")]
        [JsonProperty("onlinePaywallEnabled")]
        public long? OnlinePaywallEnabled { get; set; }

        [Object(Name = "schedule")]
        [JsonProperty("schedules")]
        public List<DoctorClinicScheduleESModel> Schedules { get; set; }
    }
}