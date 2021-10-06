using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

using UserLoader.Common;
using UserLoader.Composition;
using UserLoader.Mq;
using UserLoader.Mq.RabbitMq;
using UserLoader.Operations;

namespace UserLoader.LoaderWorker
{
    class Program
    {
        public static IServiceProvider BuildServiceProvider()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();
            Bootstrap.Configure(services, configuration);

            services.AddRabbitMq(
               configuration["RabbitMq:hostname"],
               configuration["RabbitMq:senderQueue"],
               configuration["RabbitMq:receiverQueue"]
            );

            services.AddTransient<IUserWriter, UserOperations>();

            return services.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();
            var mqWorker = serviceProvider.GetService<IMqWorker>();
            var userWriter = serviceProvider.GetService<IUserWriter>();
            var serializer = serviceProvider.GetService<ISerializer>();

            var userLoader = new UserLoader(mqWorker, userWriter, serializer);

            userLoader.Start();

            Console.ReadKey();
        }
    }
}
