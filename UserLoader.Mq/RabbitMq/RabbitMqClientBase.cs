using System;
using RabbitMQ.Client;

namespace UserLoader.Mq.RabbitMq
{
    public abstract class RabbitMqClientBase : IDisposable
    {
        private readonly object _lock = new object();
        protected readonly IConnection Connection;
        protected readonly string QueueName;
        protected readonly string Exchange;
        private IModel _channel;

        protected RabbitMqClientBase(IConnection connection, string queueName, string exchange)
        {
            Connection = connection;
            QueueName = queueName;
            Exchange = exchange;
            Init();
        }

        protected IModel Channel =>
            Helpers.DoubleCheckLock(
                _lock,
                () => _channel == null || _channel.IsClosed,
                () => _channel = Connection.CreateModel(), 
                () => _channel);

        protected void Init()
        {
            if (!string.IsNullOrEmpty(Exchange))
            {
                Channel.ExchangeDeclare(Exchange, ExchangeType.Fanout, true);
            }

            Channel.QueueDeclare(QueueName, true, false, false);

            if (!string.IsNullOrEmpty(Exchange))
            {
                _channel.QueueBind(QueueName, Exchange, "");
            }
        }

        public void Dispose()
        {
            Connection?.Dispose();
            _channel?.Dispose();
        }
    }
}