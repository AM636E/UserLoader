using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

using UserLoader.Common;
using UserLoader.DbModel.Models;
using UserLoader.Mq;
using UserLoader.Operations;

namespace UserLoader.LoaderWorker
{
    public class UserLoader : IHostedService
    {
        private readonly IMqWorker _mqWorker;
        private readonly IUserWriter _userWriter;
        private readonly ISerializer _serializer;
        private readonly ILogger<UserLoader> _logger;

        public UserLoader(IMqWorker mqWorker, IUserWriter userWriter, ISerializer serializer, ILogger<UserLoader> logger)
        {
            _mqWorker = mqWorker;
            _userWriter = userWriter;
            _serializer = serializer;
            _logger = logger;
        }

        public void Start()
        {
            _mqWorker.OnMessage += _mqWorker_OnMessage;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting User Loader");
            Start();

            return Task.FromResult(0);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping User Loader");
            _mqWorker.OnMessage -= _mqWorker_OnMessage;

            return Task.FromResult(0);
        }

        private void _mqWorker_OnMessage(object sender, MqMessage e)
        {
            _logger.LogInformation("Message received");
            _userWriter.Insert(_serializer.Deserialize<UserModel>(e.Data))
                .Match(
                _ => _logger.LogInformation("Inserted user"), 
                ex => _logger.LogError(ex, "Failed to insert user")
                );
        }
    }
}
