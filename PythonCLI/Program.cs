using Game;
using Game.ActionGetters;
using Game.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using IronPython.Hosting;
using Microsoft.Scripting.Runtime;
using System.IO;
using Microsoft.Scripting;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Scripting.Hosting;

namespace PythonCLI
{
    class Program
    {
        static void Main(string[] args)
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
