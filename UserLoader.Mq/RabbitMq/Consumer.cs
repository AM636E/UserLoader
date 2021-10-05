using System;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UserLoader.Mq.RabbitMq
{
    public class Consumer : RabbitMqClientBase
    {
        private readonly object _lock = new object();
        private readonly ILogger<Consumer> _logger;
        public event EventHandler<MqMessage> OnMessage;
        private EventingBasicConsumer _consumer;
        public Consumer(IConnection connection, string queue, string exchange, ILogger<Consumer> logger) : base(connection, queue, exchange)
        {
            _logger = logger;
        }

        private EventingBasicConsumer MqConsumer =>
            Helpers.DoubleCheckLock(_lock, () => _consumer == null || _consumer.Model.IsClosed,
                () => _consumer = new EventingBasicConsumer(Channel), () => _consumer);

        public void Start()
        {
            try
            {
                MqConsumer.Received += MqConsumer_Received;
                Channel.BasicConsume(QueueName, true, MqConsumer);
                _logger.LogInformation($"{nameof(Consumer)}.Start: Listening to {QueueName} @ {Exchange}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Consumer)}.Start");
                throw;
            }
        }

        private void MqConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            OnMessage?.Invoke(this, new MqMessage(Encoding.UTF8.GetString(e.Body.ToArray())));
        }
    }
}
