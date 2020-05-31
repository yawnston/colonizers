using Game;
using Game.Players;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Experiments
{
    public sealed class ExperimentRunner
    {
        private readonly ServiceProvider serviceProvider;

        public ExperimentRunner(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<ExperimentResult> RunSingleGame(List<IPlayer> players)
        {
            List<IPlayer> shuffledPlayers = new List<IPlayer>(players);
            shuffledPlayers.Shuffle(); // Note that this also uses the game's random generator

            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                GameEngine gameEngine = serviceProvider.GetService<GameEngine>();
                GameEndInfo gameEndInfo = await gameEngine.RunGame(shuffledPlayers);
                timer.Stop();
                Console.WriteLine($"Game took {timer.Elapsed}");

                ExperimentResult result = new ExperimentResult
                {
                    Duration = timer.Elapsed,
                };

                foreach (PlayerEndInfo p in gameEndInfo.Players)
                {
                    result.Players.Add(new PlayerExperimentResult
                    {
                        PlayerEndInfo = p,
                        Name = shuffledPlayers[p.Player.ID - 1].Name,
                    });
                }
                return result;
            }
            finally
            {
                foreach (IPlayer player in players)
                {
                    player.Dispose();
                }
            }
        }

        public async Task<List<ExperimentResult>> RunMultipleGames(Func<List<IPlayer>> playersFactory, int iterations)
        {
            var rng = new Random(42); // For generating seeds for shuffles
            List<ExperimentResult> result = new List<ExperimentResult>();
            for (int i = 0; i < iterations; i++)
            {
                GameConstants.ShuffleRandomSeed = rng.Next(0, 99);
                var players = playersFactory();
                result.Add(await RunSingleGame(players));
            }
            return result;
        }
    }
}
