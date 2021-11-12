using System.Collections.Generic;
using Nest;
using Newtonsoft.Json;
using smd_es_worker.Models.CSV;

namespace smd_es_worker.Models.Elasticsearch
{
    public class DoctorConditionsESModel : BaseReferenceDataESModel
    {
        [Number(Name = "id")]
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [Text(Name = "title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Keyword(Name = "image")]
        [JsonProperty("image")]
        public string Image { get; set; }
        
        [Keyword(Name = "imagepng1x")]
        [JsonProperty("imagepng1x")]
        public string ImagePNG1x { get; set; }
        
        [Keyword(Name = "imagepng3x")]
        [JsonProperty("imagepng3x")]
        public string ImagePNG3x { get; set; }

        [Keyword(Name = "imagesvg")]
        [JsonProperty("imagesvg")]
        public string ImageSVG { get; set; }

        [Number(Name = "sortOrder")]
        [JsonProperty("sortOrder")]
        public int? SortOrder { get; set; }

        [Keyword(Name = "specializations")]
        [JsonProperty("specializations")]
        public List<string> Specializations { get; set; }

        [Keyword(Name = "uiType")]
        [JsonProperty("uiType")]
        public string UIType { get; set; }

        [Boolean(Name = "onWeb")]
        [JsonProperty("onWeb")]
        public bool? OnWeb { get; set; }

        public DoctorConditionsESModel()
        {
            
        }

        public DoctorConditionsESModel(DoctorConditionsCSVModel csv)
        {
            Id = csv.Id;
            Name = csv.Name;
            Title = csv.Name;
            URI = csv.URL;
            Image = csv.IllustrationImagePNG;
            ImagePNG1x = csv.IllustrationImagePNG1x;
            ImagePNG3x = csv.IllustrationImagePNG3x;
            ImageSVG = csv.IllustrationImageSVG;
            SortOrder = csv.SortOrder;
            Specializations = csv.Specializations;
            UIType = csv.UIType;
            OnWeb = csv.OnWeb;
            Meta = new ReferenceDataMetaESModel
            {
                Description = csv.Description,
                OgDescription = csv.Description,
                OgImage = csv.IllustrationImagePNG,
                OgTitle = csv.Name,
                TwitterDescription = csv.Description,
                TwitterImage = csv.IllustrationImagePNG,
                TwitterTitle = csv.Name
            };
            Blurb = new ReferenceDataBlurbESModel
            {
                Content = csv.Description,
                Image = csv.IllustrationImagePNG,
                ImageAlt = csv.Name
            };
        }
    }
}