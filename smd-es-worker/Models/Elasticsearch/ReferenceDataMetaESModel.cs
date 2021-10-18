using Nest;

namespace smd_es_worker.Models.Elasticsearch
{
    public class ReferenceDataMetaESModel
    {
        [Text(Name = "description")]
        public string Description { get; set; }

        [Text(Name = "ogDescription")]
        public string OgDescription { get; set; }

        [Keyword(Name = "ogImage")]
        public string OgImage { get; set; }

        [Keyword(Name = "ogTitle")]
        public string OgTitle { get; set; }

        [Text(Name = "twitterDescription")]
        public string TwitterDescription { get; set; }

        [Keyword(Name = "twitterImage")]
        public string TwitterImage { get; set; }

        [Keyword(Name = "twitterTitle")]
        public string TwitterTitle { get; set; }
    }
}