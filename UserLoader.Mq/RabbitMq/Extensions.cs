using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UserLoader.Mq.RabbitMq
{
    public static class Extensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection collection, string hostname, string producerQueue,
            string consumerQueue, string exchange = "")
        {
            collection.AddSingleton<IMqWorker>(factory
                => new RabbitMqWorker(hostname, producerQueue, consumerQueue, exchange,
                    factory.GetRequiredService<ILoggerFactory>()));

            return collection;
        }
    }
}