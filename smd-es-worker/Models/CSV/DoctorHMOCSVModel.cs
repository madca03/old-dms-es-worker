using CsvHelper.Configuration.Attributes;

namespace smd_es_worker.Models.CSV
{
    public class DoctorHMOCSVModel
    {
        [Name("ID")]
        public string Id { get; set; }

        [Name("HMO")]
        public string Name { get; set; }

        [Name("Url")]
        public string Url { get; set; }

        [Name("Illustration Link")]
        public string IllustrationImage { get; set; }

        [Name("Partner Code")]
        public string PartnerCode { get; set; }

        [Name("SQL ID")]
        public int? SQLId { get; set; }
    }
}