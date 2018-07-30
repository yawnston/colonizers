using System;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using Game.ActionGetters;
using Game;
using Newtonsoft.Json;

namespace CLI
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

            Console.WriteLine("Colonizers v0.1");
            Console.WriteLine("Created by Daniel Crha");

            var mediator = serviceProvider.GetService<IMediator>();
            var resolver = new Resolver(mediator);

            GameState gameState; IRequest<GameState> action;
            var boardState = new BoardState().InitRound();
            var firstGetter = serviceProvider.GetService<IColonistPickGetter>();
            gameState = firstGetter.Process(boardState).Result;

            while (true)
            {
                foreach(var a in gameState.Actions)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(a, Formatting.Indented, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }));
                }
                action = gameState.Actions[Int32.Parse(Console.ReadLine())];
                gameState = resolver.Resolve(action).Result;
            }
        }
    }
}
