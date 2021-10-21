using System.Collections.Generic;
using Nest;
using smd_es_worker.Models.CSV;

namespace smd_es_worker.Models.Elasticsearch
{
    public class DoctorConditionsESModel : BaseReferenceDataESModel
    {
        [Keyword(Name = "title")]
        public string Title { get; set; }

        [Keyword(Name = "image")]
        public string Image { get; set; }

        [Number(Name = "sortOrder")]
        public int SortOrder { get; set; }

        [Number(Name = "doctorCount")]
        public string DoctorCount { get; set; }

        [Keyword(Name = "specializations")]
        public List<string> Specializations { get; set; }

        [Keyword(Name = "uiType")]
        public string UIType { get; set; }

        [Boolean(Name = "onWeb")]
        public bool OnWeb { get; set; }

        public DoctorConditionsESModel()
        {
            
        }

        public DoctorConditionsESModel(DoctorConditionsCSVModel csv)
        {
            Name = csv.Name;
            Title = csv.Name;
            URI = csv.URL;
            Image = csv.IllustrationImage;
            SortOrder = csv.SortOrder;
            Specializations = csv.Specializations;
            UIType = csv.UIType;
            OnWeb = csv.OnWeb;
            Meta = new ReferenceDataMetaESModel
            {
                Description = csv.Description,
                OgDescription = csv.Description,
                OgImage = csv.IllustrationImage,
                OgTitle = csv.Name,
                TwitterDescription = csv.Description,
                TwitterImage = csv.IllustrationImage,
                TwitterTitle = csv.Name
            };
            Blurb = new ReferenceDataBlurbESModel
            {
                Content = csv.Description,
                Image = csv.IllustrationImage,
                ImageAlt = csv.Name
            };
        }
    }
}