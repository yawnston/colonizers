using Game;
using Game.Extensions;
using Game.Players;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PythonCLI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
              .AddColonizersGame()
              .BuildServiceProvider();

            var players = new List<IPlayer>
            {
                //new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\ISMCTSIntelligence\ISMCTSIntelligence.py", "Player4Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player4Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player1Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player3Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\HeuristicIntelligence\HeuristicIntelligence.py", "Player2Pipe"),
            };

            try
            {
                var timer = new Stopwatch();
                timer.Start();
                var gameEngine = serviceProvider.GetService<GameEngine>();
                var gameEndInfo = await gameEngine.RunGame(players);
                timer.Stop();
                Console.WriteLine(gameEndInfo.SerializeToJArray());
                Console.WriteLine($"Game took {timer.Elapsed}");
            }
            finally
            {
                foreach (var player in players) player.Dispose();
            }
        }
    }
}
