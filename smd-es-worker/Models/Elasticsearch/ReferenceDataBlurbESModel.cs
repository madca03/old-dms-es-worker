using Nest;

namespace smd_es_worker.Models.Elasticsearch
{
    public class ReferenceDataBlurbESModel
    {
        [Text(Name = "content")]
        public string Content { get; set; }

        [Keyword(Name = "image")]
        public string Image { get; set; }

        [Keyword(Name = "imageAlt")]
        public string ImageAlt { get; set; }
    }
}