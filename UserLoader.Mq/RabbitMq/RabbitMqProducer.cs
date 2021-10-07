using System;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace UserLoader.Mq.RabbitMq
{
    public class RabbitMqProducer : RabbitMqClientBase, IPublisher
    {
        private readonly ILogger<RabbitMqProducer> _logger;
        private readonly IBasicProperties _properties;

        public RabbitMqProducer(IConnection connection, string queueName, string exchange, ILogger<RabbitMqProducer> logger) : base(connection, queueName, exchange)
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
                _logger.LogInformation($"{nameof(RabbitMqProducer)}.SendMessage: Sent to {QueueName} @ {Exchange}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(RabbitMqProducer)}.SendMessage");
                throw;
            }
        }
    }
}
