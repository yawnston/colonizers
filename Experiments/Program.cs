using Game.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Experiments
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
              .AddColonizersGame()
              .BuildServiceProvider();

            string scriptFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AICore");

            Scenarios scenarios = new Scenarios(serviceProvider, scriptFolderPath);

            string pythonExecutable = args[1];

            switch (int.Parse(args[0]))
            {
                case 1:
                    await scenarios.FourRandoms(pythonExecutable);
                    break;
                case 2:
                    await scenarios.FourHeuristics(pythonExecutable);
                    break;
                case 3:
                    await scenarios.OneOfEach(pythonExecutable);
                    break;
                case 4:
                    await scenarios.MaxnVsHeuristic(pythonExecutable);
                    break;
                case 5:
                    await scenarios.ISMCTSVsHeuristic(pythonExecutable);
                    break;

            }

            System.Console.WriteLine("Experiment run finished! Results have been written to the output JSON file.");
        }
    }
}
