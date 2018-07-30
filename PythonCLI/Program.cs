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

namespace PythonCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length != 1)
            //{
            //    Console.WriteLine("Invalid argument count. Please invoke with a single argument containing the python script.");
            //    return;
            //}
            //var scriptReadTask = File.ReadAllTextAsync(args[0]);

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

            GameState gameState; PlayerInfo currentPlayer;
            var boardState = BoardFactory.Standard(); string script;
            gameState = GameFactory.NewGame(boardState, serviceProvider);

            //try
            //{
            //    script = scriptReadTask.Result;
            //}
            //catch (IOException)
            //{
            //    Console.WriteLine("Could not read python script.");
            //    return;
            //}

            var engine = Python.CreateEngine();
            var scope = engine.CreateScope();
            string pyFunc = @"
def processState(gs): 
    for attr in dir(gs):
        print(""gs.% s = % r"" % (attr, getattr(gs, attr)))
    return 1";
            var source = engine.CreateScriptSourceFromString(pyFunc, SourceCodeKind.Statements);
            var compiled = source.Compile();

            while (true)
            {
                compiled.Execute(scope);
                Func<GameState, int> processState = scope.GetVariable<Func<GameState, int>>("processState");
                Console.WriteLine(processState(gameState));
            }

            Console.WriteLine("Results:");
            Console.WriteLine();
            foreach (var p in gameState.GameEndInfo.Players.OrderBy(pi => pi.VictoryPoints))
            {

                Console.WriteLine($"Player {p.Player.ID}: {p.VictoryPoints} points");
            }

            /*
            var engine = Python.CreateEngine();
            var theScript = @"def PrintMessage():
    print 'This is a message!'

PrintMessage()
";
            dynamic scope = engine.CreateScope();
            scope.Add = new Func<int, int, int>((x, y) => x + y);
            engine.Execute(theScript);
            engine.Execute(@"print Add(2, 3)", scope);
            */
        }
    }
}
