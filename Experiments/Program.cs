using Game;
using Game.Extensions;
using Game.Players;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Experiments
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
              .AddColonizersGame()
              .BuildServiceProvider();

            string scriptFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AICore");

            var scenarios = new Scenarios(serviceProvider, scriptFolderPath);

            string pythonExecutable = args[2];

            switch (Int32.Parse(args[1]))
            {
                case 1:
                    await scenarios.FourRandoms();
                    break;
                case 2:
                    await scenarios.FourHeuristics();
                    break;
                case 3:
                    await scenarios.OneOfEach();
                    break;
                case 4:
                    await scenarios.MaxnVsHeuristic();
                    break;
                case 5:
                    await scenarios.ISMCTSVsHeuristic();
                    break;

            }

            // TODO: configure python path
        }
    }
}
