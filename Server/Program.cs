using Game;
using Game.ActionGetters;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Server
{
    class Program
    {
        const string API_CONFIG_SECTION = "server-info";
        const string API_CONFIG_NAME_IP = "ip";
        const string API_CONFIG_NAME_PORT = "port";

        private static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            LoadConfig();

            var serviceProvider = new ServiceCollection()
           .AddMediatR(typeof(Resolver).GetTypeInfo().Assembly)
           .AddScoped<IMediator, Mediator>()
           .AddScoped<IColonistPickGetter, ColonistPickGetter>()
           .AddScoped<IDrawGetter, DrawGetter>()
           .AddScoped<IDiscardGetter, DiscardGetter>()
           .AddScoped<IPowerGetter, PowerGetter>()
           .AddScoped<IBuildGetter, BuildGetter>()
           .AddScoped<IGameEndGetter, GameEndGetter>()
           .BuildServiceProvider();

            var mediator = serviceProvider.GetService<IMediator>();

            var server = new GameServer(mediator, serviceProvider);
            server.StartUp(
                IPAddress.Parse(configuration.GetSection(API_CONFIG_SECTION)[API_CONFIG_NAME_IP]),
                int.Parse(configuration.GetSection(API_CONFIG_SECTION)[API_CONFIG_NAME_PORT]));  // start the server
        }

        private static void LoadConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();
        }
    }
}
