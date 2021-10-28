using CsvHelper.Configuration.Attributes;

namespace smd_es_worker.Models.CSV
{
    public class DoctorServicesCSVModel
    {
        [Name("ID")]
        public int Id { get; set; }

        [Name("Services")]
        public string Name { get; set; }

        [Name("Url")]
        public string URL { get; set; }

        [Name("Description")]
        public string Description { get; set; }

        [Name("Sort Order")]
        public int SortOrder { get; set; }

        [Name("UI Type")]
        public string RawUIType { get; set; }

        [Name("Illustration Link PNG")]
        public string IllustrationImagePNG { get; set; }

        [Name("Illustration Link SVG")]
        public string IllustrationImageSVG { get; set; }

        [Name("Featured on Web?")]
        public string RawOnWeb { get; set; }

        [Name("Featured on NS Svcs?")]
        public string RawOnNSServices { get; set; }
        
        [Ignore]
        public string UIType { get; set; }

        [Ignore]
        public bool OnWeb { get; set; }

        [Ignore]
        public bool OnNSServices { get; set; }
    }
}