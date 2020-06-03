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

        public async Task<List<ExperimentResult>> FourRandoms(string pythonExecutable)
        {
            List<IPlayer> playersFactory()
            {
                return new List<IPlayer>
            {
                new AIPlayer(Path.Combine(scriptFolderPath, "RandomIntelligence.py"), "Player1Pipe", "Random", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "RandomIntelligence.py"), "Player2Pipe", "Random", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "RandomIntelligence.py"), "Player3Pipe", "Random", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "RandomIntelligence.py"), "Player4Pipe", "Random", pythonExecutable),
            };
            }

            ExperimentRunner runner = new ExperimentRunner(serviceProvider);

            List<ExperimentResult> results = await runner.RunMultipleGames(playersFactory, 1000);
            WriteResults(results, "FourRandoms.json");
            return results;
        }

        public async Task<List<ExperimentResult>> FourHeuristics(string pythonExecutable)
        {
            List<IPlayer> playersFactory()
            {
                return new List<IPlayer>
            {
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player1Pipe", "Heuristic", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player2Pipe", "Heuristic", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player3Pipe", "Heuristic", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player4Pipe", "Heuristic", pythonExecutable),
            };
            }

            ExperimentRunner runner = new ExperimentRunner(serviceProvider);

            List<ExperimentResult> results = await runner.RunMultipleGames(playersFactory, 1000);
            WriteResults(results, "FourHeuristics.json");
            return results;
        }

        public async Task<List<ExperimentResult>> OneOfEach(string pythonExecutable)
        {
            List<IPlayer> playersFactory()
            {
                return new List<IPlayer>
            {
                new AIPlayer(Path.Combine(scriptFolderPath, "RandomIntelligence.py"), "Player1Pipe", "Random", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player2Pipe", "Heuristic", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "MaxnIntelligence.py"), "Player3Pipe", "Maxn", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "ISMCTSIntelligence.py"), "Player4Pipe", "ISMCTS", pythonExecutable),
            };
            }

            ExperimentRunner runner = new ExperimentRunner(serviceProvider);

            List<ExperimentResult> results = await runner.RunMultipleGames(playersFactory, 50);
            WriteResults(results, "OneOfEach.json");
            return results;
        }

        public async Task<List<ExperimentResult>> MaxnVsHeuristic(string pythonExecutable)
        {
            List<IPlayer> playersFactory()
            {
                return new List<IPlayer>
            {
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player1Pipe", "Heuristic", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player2Pipe", "Heuristic", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "MaxnIntelligence.py"), "Player3Pipe", "Maxn", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "MaxnIntelligence.py"), "Player4Pipe", "Maxn", pythonExecutable),
            };
            }

            ExperimentRunner runner = new ExperimentRunner(serviceProvider);

            List<ExperimentResult> results = await runner.RunMultipleGames(playersFactory, 1);
            WriteResults(results, "MaxnVsHeuristic.json");
            return results;
        }

        public async Task<List<ExperimentResult>> ISMCTSVsHeuristic(string pythonExecutable)
        {
            List<IPlayer> playersFactory()
            {
                return new List<IPlayer>
            {
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player1Pipe", "Heuristic", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "HeuristicIntelligence.py"), "Player2Pipe", "Heuristic", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "ISMCTSIntelligence.py"), "Player3Pipe", "ISMCTS", pythonExecutable),
                new AIPlayer(Path.Combine(scriptFolderPath, "ISMCTSIntelligence.py"), "Player4Pipe", "ISMCTS", pythonExecutable),
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
