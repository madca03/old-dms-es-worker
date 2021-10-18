using SeriousmdRabbitMQ.References;

namespace smd_es_worker.References
{
    public class RMQResponseStatus : BaseRmqResponseStatus
    {
        public static readonly RMQResponse BULK_UPDATE_FAILED = new() {StatusCode = -100, Message = "Bulk update failed"};
    }
}