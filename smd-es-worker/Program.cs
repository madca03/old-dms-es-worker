using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SeriousmdRabbitMQ.Util;
using smd_es_worker.Clients;
using smd_es_worker.Config;

namespace smd_es_worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog(SerilogUtil.InitiializeSerilog)
                .ConfigureServices((hostContext, services) =>
                {
                    ServiceConfigurationUtil.InitializeDefaultSettingsConfiguration(services, hostContext.Configuration);
                    
                    services.Configure<WorkerQueues>(
                        hostContext.Configuration.GetSection(nameof(WorkerQueues)));

                    services.Configure<ElasticIndices>(
                        hostContext.Configuration.GetSection(nameof(ElasticIndices)));

                    services.AddSingleton<ElasticsearchClient>();
                    services.AddHostedService<Worker>();
                });
    }
}