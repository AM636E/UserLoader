using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

using System;
using System.Threading.Tasks;

using UserLoader.Composition;
using UserLoader.Mq.RabbitMq;
using UserLoader.Operations;

namespace UserLoader.LoaderWorker
{
    class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    Bootstrap.Configure(services, configuration);

                    services.AddRabbitMq(
                       configuration["RabbitMq:hostname"],
                       configuration["RabbitMq:senderQueue"],
                       configuration["RabbitMq:receiverQueue"]
                    );

                    services.AddTransient<IUserWriter, UserOperations>();
                    services.AddHostedService<UserLoader>();
                });

        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .Enrich.FromLogContext()
               .WriteTo.Console()
               .CreateLogger();

            using var host = CreateHostBuilder(args).UseSerilog().Build();
            await host.StartAsync();

            Console.ReadLine();
        }
    }
}
