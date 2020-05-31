using Game.Players;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Experiments
{
    public sealed class Scenarios
    {
        private readonly ServiceProvider serviceProvider;
        private readonly string scriptFolderPath;

        public Scenarios(ServiceProvider serviceProvider, string scriptFolderPath)
        {
            this.serviceProvider = serviceProvider;
            this.scriptFolderPath = scriptFolderPath;
        }

        public async Task<List<ExperimentResult>> FourRandoms()
        {
            List<IPlayer> playersFactory()
            {
                return new List<IPlayer>
            {
                new AIPlayer(Path.Combine(scriptFolderPath, "RandomIntelligence.py"), "Player1Pipe", "Random"),
                new AIPlayer(Path.Combine(scriptFolderPath, "RandomIntelligence.py"), "Player2Pipe", "Random"),
                new AIPlayer(Path.Combine(scriptFolderPath, "RandomIntelligence.py"), "Player3Pipe", "Random"),
                new AIPlayer(Path.Combine(scriptFolderPath, "RandomIntelligence.py"), "Player4Pipe", "Random"),
            };
            }

            ExperimentRunner runner = new ExperimentRunner(serviceProvider);

            List<ExperimentResult> results = await runner.RunMultipleGames(playersFactory, 1000);
            WriteResults(results, "FourRandoms.json");
            return results;
        }

        public async Task<List<ExperimentResult>> FourHeuristics()
        {
            List<IPlayer> playersFactory()
            {
                return new List<IPlayer>
            {
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player1Pipe", "Heuristic"),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player2Pipe", "Heuristic"),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player3Pipe", "Heuristic"),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player4Pipe", "Heuristic"),
            };
            }

            ExperimentRunner runner = new ExperimentRunner(serviceProvider);

            List<ExperimentResult> results = await runner.RunMultipleGames(playersFactory, 1000);
            WriteResults(results, "FourHeuristics.json");
            return results;
        }

        public async Task<List<ExperimentResult>> OneOfEach()
        {
            List<IPlayer> playersFactory()
            {
                return new List<IPlayer>
            {
                new AIPlayer(Path.Combine(scriptFolderPath, "RandomIntelligence.py"), "Player1Pipe", "Random"),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player2Pipe", "Heuristic"),
                new AIPlayer(Path.Combine(scriptFolderPath, "MaxnIntelligence.py"), "Player3Pipe", "Maxn"),
                new AIPlayer(Path.Combine(scriptFolderPath, "ISMCTSIntelligence.py"), "Player4Pipe", "ISMCTS"),
            };
            }

            ExperimentRunner runner = new ExperimentRunner(serviceProvider);

            List<ExperimentResult> results = await runner.RunMultipleGames(playersFactory, 1);
            WriteResults(results, "OneOfEach.json");
            return results;
        }

        public async Task<List<ExperimentResult>> MaxnVsHeuristic()
        {
            List<IPlayer> playersFactory()
            {
                return new List<IPlayer>
            {
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player1Pipe", "Heuristic"),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player2Pipe", "Heuristic"),
                new AIPlayer(Path.Combine(scriptFolderPath, "MaxnIntelligence.py"), "Player3Pipe", "Maxn"),
                new AIPlayer(Path.Combine(scriptFolderPath, "MaxnIntelligence.py"), "Player4Pipe", "Maxn"),
            };
            }

            ExperimentRunner runner = new ExperimentRunner(serviceProvider);

            List<ExperimentResult> results = await runner.RunMultipleGames(playersFactory, 50);
            WriteResults(results, "MaxnVsHeuristic.json");
            return results;
        }

        public async Task<List<ExperimentResult>> ISMCTSVsHeuristic()
        {
            List<IPlayer> playersFactory()
            {
                return new List<IPlayer>
            {
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player1Pipe", "Heuristic"),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player2Pipe", "Heuristic"),
                new AIPlayer(Path.Combine(scriptFolderPath, "ISMCTSIntelligence.py"), "Player3Pipe", "ISMCTS"),
                new AIPlayer(Path.Combine(scriptFolderPath, "ISMCTSIntelligence.py"), "Player4Pipe", "ISMCTS"),
            };
            }

            ExperimentRunner runner = new ExperimentRunner(serviceProvider);

            List<ExperimentResult> results = await runner.RunMultipleGames(playersFactory, 50);
            WriteResults(results, "ISMCTSVsHeuristic.json");
            return results;
        }

        private void WriteResults(List<ExperimentResult> results, string fileName)
        {
            string serializedResults = JsonConvert.SerializeObject(results, Formatting.Indented);

            File.WriteAllText(fileName, serializedResults);
        }
    }
}
