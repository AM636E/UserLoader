using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

using UserLoader.DbModel;

namespace UserLoader.Composition
{
    public class Bootstrap
    {
        public static void Configure(IServiceCollection container, IConfiguration configuration)
        {
            container.AddScoped(factory => new MongoClient(configuration["DatabaseSettings:ConnectionString"]));

            container.AddScoped(factory =>
            {
                var client = (MongoClient)factory.GetService(typeof(MongoClient));
                return client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            });

            container.AddAutoMapper(typeof(UserEntity));

            container.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}