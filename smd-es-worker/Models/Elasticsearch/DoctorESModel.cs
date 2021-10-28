using System;
using System.Collections.Generic;
using Nest;
using Newtonsoft.Json;

namespace smd_es_worker.Models.Elasticsearch
{
    public class DoctorESModel
    {
        [Text(Name = "prcLicenseType")]
        [JsonProperty("prcLicenseType")]
        public string PrcLicenseType { get; set; }
        
        [Text(Name = "image")]
        [JsonProperty("image")]
        public string Image { get; set; }
        
        [Text(Name = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [Number(Name = "doctorid")]
        [JsonProperty("doctorId")]
        public int DoctorId { get; set; }
        
        [Object(Name = "societies")]
        [JsonProperty("societies")]
        public List<DoctorSocietyESModel> Societies { get; set; }
        
        [Nested(Name = "clinics")]
        [JsonProperty("clinics")]
        public List<DoctorClinicESModel> Clinics { get; set; }
        
        [Date(Name = "ExpireTime")]
        [JsonProperty("expireTime")]
        public DateTime ExpireTime { get; set; }

        [Number(Name = "profilePageScore")]
        [JsonProperty("profilePageScore")]
        public double? ProfilePageScore { get; set; }

        [Number(Name = "pageviews")]
        [JsonProperty("pageViews")]
        public long? PageViews { get; set; }

        [Number(Name = "LatestBookingTimeNoSlotMinutes")]
        [JsonProperty("latestBookingTimeNoSlotMinutes")]
        public long? LatestBookingTimeNoSlotMinutes { get; set; }

        [Text(Name = "LatestBookingTimeNoSlotReference")]
        [JsonProperty("latestBookingTimeNoSlotReference")]
        public string LatestBookingTimeNoSlotReference { get; set; }

        [Object(Name = "professional")]
        [JsonProperty("professional")]
        public DoctorSpecialtyInfoESModel Professional { get; set; }

        [Number(Name = "services")]
        [JsonProperty("services")]
        public List<int> Services { get; set; }
    }
}