using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

using System;

using UserLoader.Common;
using UserLoader.DbModel;
using UserLoader.DbModel.Models;

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

            container.AddAutoMapper(configuration =>
            {
                configuration.CreateMap<UserEntity, UserModel>();
                configuration.CreateMap<UserModel, UserEntity>();
                configuration.CreateMap<DateTime, DateTimeOffset>().ConvertUsing(dateTime => new DateTimeOffset(dateTime, TimeSpan.Zero));
                configuration.CreateMap<DateTimeOffset, DateTime>().ConvertUsing(offset => offset.DateTime);
            });

            container.AddSingleton<ISerializer, JsonSerializer>();

            container.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}