using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace smd_es_worker.Models.CSV
{
    public class DoctorConditionsCSVModel
    {
        [Name("ID")]
        public int Id { get; set; }

        [Name("Conditions")]
        public string Name { get; set; }

        [Name("Url")]
        public string URL { get; set; }
	
        [Name("Description")]
        public string Description { get; set; }
	
        [Name("Associated Specializations")]
        public string RawSpecializations { get; set; }
	
        [Name("Sort Order")]
        public int SortOrder { get; set; }
	
        [Name("UI Type")]
        public string UIType { get; set; }
	
        [Name("Illustration File Name")]
        public string IllustrationImage { get; set; }
	
        [Name("On Web")]
        public string RawOnWeb { get; set; }
	
        [Ignore]
        public bool OnWeb { get; set; }
	
        [Ignore]
        public List<string> Specializations { get; set; }
    }
}