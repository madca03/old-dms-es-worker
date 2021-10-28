using System;
using Nest;
using Newtonsoft.Json;

namespace smd_es_worker.Models.Elasticsearch
{
    public class DoctorClinicScheduleESModel
    {
        [Number(Name = "dayofweek")]
        [JsonProperty("dayofweek")]
        public long? DayOfWeek { get; set; }

        [Date(Name = "endtime")]
        [JsonProperty("endTime")]
        public DateTime? EndTime { get; set; }

        [Text(Name = "notes")]
        [JsonProperty("notes")]
        public string Notes { get; set; }

        [Text(Name = "scheduletype")]
        [JsonProperty("scheduleType")]
        public string ScheduleType { get; set; }

        [Date(Name = "starttime")]
        [JsonProperty("startTime")]
        public DateTime? StartTime { get; set; }

        [Text(Name = "status")]
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}