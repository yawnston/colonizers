using Game;
using Game.ActionGetters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
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
            server.StartUp(IPAddress.Loopback, 4141);  // start the echo server
        }
    }
}
