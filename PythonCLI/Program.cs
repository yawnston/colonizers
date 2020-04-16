using Game;
using Game.ActionGetters;
using Game.Players;
using IronPython.Hosting;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace PythonCLI
{
    public static class Program
    {
        public static async Task Main(string[] args)
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
            var resolver = new Resolver(mediator);

            var players = new List<IPlayer>
            {
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player1Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\HeuristicIntelligence\HeuristicIntelligence.py", "Player2Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player3Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\ISMCTSIntelligence\ISMCTSIntelligence.py", "Player4Pipe"),
            };

            try
            {
                var gameEngine = new GameEngine(resolver, serviceProvider);
                var gameEndInfo = await gameEngine.RunGame(players);
                Console.WriteLine(gameEndInfo.SerializeToJArray());
            }
            finally
            {
                foreach (var player in players) player.Dispose();
            }
        }

        static void Main2(string[] args)
        {
            if (args.Length != 4 && args.Length != 5)
            {
                Console.WriteLine("Invalid argument count. Please invoke with 4 arguments containing the python scripts and and optional 5th argument with the path to Python standard libraries for IronPython to use.");
                return;
            }

            var scriptReadTasks = ScriptLoader.LoadScripts(args);

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
            var resolver = new Resolver(mediator);

            var processor = new GameProcessor(resolver);
            var boardState = BoardFactory.Standard();
            var gameState = GameFactory.NewGame(boardState, serviceProvider);

            var engine = Python.CreateEngine();
            if (args.Length == 5)
            {
                var paths = engine.GetSearchPaths();
                paths.Add(args[4]);
                engine.SetSearchPaths(paths);
            }

            IList<CompiledCode> scripts;
            try
            {
                scripts = ScriptLoader.CreateScripts(scriptReadTasks, engine);
            }
            catch (IOException)
            {
                Console.WriteLine("Could not read python script.");
                return;
            }

            processor.Run(gameState, scripts, engine);
        }
    }
}
