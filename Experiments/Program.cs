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

            await scenarios.FourRandoms();
            await scenarios.FourHeuristics();
            //await scenarios.OneOfEach();
            //await scenarios.MaxnVsHeuristic();
            //await scenarios.ISMCTSVsHeuristic();
        }
    }
}
