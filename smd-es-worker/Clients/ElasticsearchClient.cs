using System;
using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;
using SeriousmdRabbitMQ.Config;

namespace smd_es_worker.Clients
{
    public class ElasticsearchClient
    {
        public IElasticClient Client { get; }

        public ElasticsearchClient(IOptions<ElasticsearchConnectionSettings> esSettings)
        {
            var config = esSettings.Value;
            var settings = new ConnectionSettings(new Uri(config.Uri))
                .ApiKeyAuthentication(new ApiKeyAuthenticationCredentials(config.ApiKey));

            Client = new ElasticClient(settings);
        }
    }
}