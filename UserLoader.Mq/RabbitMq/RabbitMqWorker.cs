using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace UserLoader.Mq.RabbitMq
{
    public class RabbitMqWorker : IMqWorker
    {
        private readonly object _lock = new object();
        private readonly string _producerQueue;
        private readonly string _consumerQueue;
        private readonly string _exchange;
        private readonly ILogger<RabbitMqWorker> _logger;
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private Producer _producer;
        private Consumer _consumer;
        private readonly ILoggerFactory _loggerFactory;

        public event EventHandler<MqMessage> OnMessage;

        public RabbitMqWorker(string hostname, string producerQueue, string consumerQueue, string exchange,
            ILoggerFactory factory)
        {
            _producerQueue = producerQueue;
            _consumerQueue = consumerQueue;
            _exchange = exchange;
            _loggerFactory = factory;
            _logger = factory.CreateLogger<RabbitMqWorker>();
            _connectionFactory = new ConnectionFactory { HostName = hostname };
            _connection = Connection;
        }

        public void SendMessage(string message) 
        {
            if(string.IsNullOrEmpty(_producerQueue))
            {
                throw new InvalidOperationException($"Producer queue is empty. Please, add it in configuration.");
            }

            _producer.SendMessage(message);
        }

        private IConnection Connection =>
            Helpers.DoubleCheckLock(_lock, () => _connection == null || !_connection.IsOpen,
            ProduceConnection, () => _connection);

        private IConnection ProduceConnection()
        {
            _logger.LogInformation($"(Re)Creating Connection. Consumer: {_consumerQueue}. Producer: {_producerQueue}");

            if (_connection != null)
            {
                _connection.Dispose();
                _connection.ConnectionShutdown -= _connection_ConnectionShutdown;
            }

            if (_consumer != null)
            {
                _consumer.OnMessage -= _consumer_OnMessage;
            }

            _connection = _connectionFactory.CreateConnection();
            _connection.ConnectionShutdown += _connection_ConnectionShutdown;

            if (!string.IsNullOrEmpty(_producerQueue))
            {
                _producer = new Producer(Connection, _producerQueue, _exchange, _loggerFactory.CreateLogger<Producer>());
            }

            if (!string.IsNullOrEmpty(_consumerQueue))
            {
                _consumer = new Consumer(Connection, _consumerQueue, _exchange, _loggerFactory.CreateLogger<Consumer>());
                _consumer.OnMessage += _consumer_OnMessage;
                _consumer.Start();
            }

            return _connection;
        }

        private void _consumer_OnMessage(object sender, MqMessage e)
        {
            OnMessage?.Invoke(this, e);
        }

        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"{nameof(RabbitMqWorker)}.ConnectionShutdown. {e.ReplyCode}|{e.ReplyText}.");
            ProduceConnection();
        }
    }
}
