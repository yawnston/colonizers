using Game;
using Game.Entities;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PythonCLI
{
    class GameProcessor
    {
        private readonly Resolver resolver;

        public GameProcessor(Resolver resolver)
        {
            this.resolver = resolver;
        }

        public void Run(GameState beginningState, IList<CompiledCode> scripts, ScriptEngine engine)
        {
            GameState gameState = beginningState;
            PlayerInfo currentPlayer;
            ScriptScope scope;

            while (true)
            {
                currentPlayer = gameState.BoardState.Players[gameState.BoardState.PlayerTurn - 1];

                scope = engine.CreateScope();
                scripts[currentPlayer.ID].Execute(scope);
                Func<string, int> processState = scope.GetVariable<Func<string, int>>("processState");

                Console.WriteLine(processState(GameStateJsonSerializer.Serialize(gameState)));

                Console.ReadLine();
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
