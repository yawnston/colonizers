using System;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using Game.ActionGetters;

namespace CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
            .AddMediatR(typeof(Program).GetTypeInfo().Assembly)
            .AddScoped<IMediator, Mediator>()
            .AddScoped<IColonistPickGetter, ColonistPickGetter>()
            .AddScoped<IDrawGetter, DrawGetter>()
            .AddScoped<IDiscardGetter, DiscardGetter>()
            .AddScoped<IPowerGetter, PowerGetter>()
            .AddScoped<IBuildGetter, BuildGetter>()
            .BuildServiceProvider();
        }
    }
}
