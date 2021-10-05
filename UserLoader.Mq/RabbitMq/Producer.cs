using System;
using System.Collections.Specialized;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace UserLoader.Mq.RabbitMq
{
    public class Producer : RabbitMqClientBase
    {
        private readonly ILogger<Producer> _logger;
        private readonly IBasicProperties _properties;

        public Producer(IConnection connection, string queueName, string exchange, ILogger<Producer> logger) : base(connection, queueName, exchange)
        {
            _logger = logger;
            _properties = Channel.CreateBasicProperties();
            _properties.ContentType = "text/plain";
            _properties.DeliveryMode = 2;
        }

        public void SendMessage(string message)
        {
            try
            {
                Channel.BasicPublish(Exchange, QueueName, false, _properties,
                    new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(message)));
                _logger.LogInformation($"{nameof(Producer)}.SendMessage: Sent to {QueueName} @ {Exchange}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Producer)}.SendMessage");
                throw;
            }
        }
    }
}
