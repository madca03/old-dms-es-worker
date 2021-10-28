using System.Collections.Generic;
using Nest;
using Newtonsoft.Json;

namespace smd_es_worker.Models.Elasticsearch
{
    public class DoctorSpecialtyInfoESModel
    {
        [Text(Name = "specialty")]
        [JsonProperty("specialties")]
        public List<string> Specialty { get; set; }

        [Text(Name = "subspecialty")]
        [JsonProperty("subspecialties")]
        public List<string> SubSpecialty { get; set; }

        [Number(Name = "practicingSince")]
        [JsonProperty("practicingSince")]
        public int? PracticingSince { get; set; }

        [Text(Name = "mainspecialty")]
        [JsonProperty("mainSpecialty")]
        public string MainSpecialty { get; set; }
    }
}