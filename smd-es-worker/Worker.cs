using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using SeriousmdRabbitMQ.Config;
using SeriousmdRabbitMQ.Managers;
using SeriousmdRabbitMQ.RabbitMQ;
using SeriousmdRabbitMQ.Worker;
using smd_es_worker.Config;
using smd_es_worker.Managers;
using smd_es_worker.References;

namespace smd_es_worker
{
    public class Worker : BaseWorker
    {
        private readonly WorkerQueues _workerQueues;

        public Worker(
            ILogger logger,
            IConfiguration configuration,
            IOptions<WorkerQueues> workerQueues,
            IOptions<WorkerSettings> workerSettings,
            IServiceProvider serviceProvider,
            RabbitMQClient rmq) 
            : base(logger, configuration, workerSettings, rmq, serviceProvider)
        {
            _workerQueues = workerQueues.Value;
        }
        
        public override void CreateQueueManagers()
        {
            if (!workerSettings.HasActiveQueue()) return;
            var queues = _workerQueues.GetType().GetProperties().Where(x => x.PropertyType == typeof(string));
            
            foreach (var queue in queues)
            {
                var queueName = queue.GetValue(_workerQueues)?.ToString();
                var isQueueActive = workerSettings.IsQueueActive(queueName);
                if (!isQueueActive) continue;

                BaseQueueManager queueManager = queue.Name switch
                {
                    QueueReference.UpdateESReferenceDataQueue => new UpdateESReferenceDataQueueManager(logger, rmq, queueName, configuration, serviceProvider),
                    _ => null
                };

                if (queueManager == null) continue;
                queueManagers.Add(queueManager);

                for (var i = 0; i < workerSettings.ConsumersPerQueue; i++)
                {
                    queueTasks.Add(Task.Run(queueManager.DeclareConsumerQueue(cts.Token)));
                }
            }
        }
    }
}