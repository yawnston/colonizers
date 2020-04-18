using ColonizersUI.Models;
using Game;
using Game.Players;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ColonizersUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly GameEngine gameEngine;

        public HomeController(ILogger<HomeController> logger, GameEngine gameEngine)
        {
            this.logger = logger;
            this.gameEngine = gameEngine;
        }

        public async Task<IActionResult> Game()
        {
            var players = new List<IPlayer>
            {
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player1Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\HeuristicIntelligence\HeuristicIntelligence.py", "Player2Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player3Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player4Pipe"),
                //new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\ISMCTSIntelligence\ISMCTSIntelligence.py", "Player4Pipe"),
            };

            try
            {
                logger.LogInformation("Starting game.");
                var timer = new Stopwatch();
                timer.Start();
                var gameEndInfo = await gameEngine.RunGame(players);
                timer.Stop();
                logger.LogInformation(gameEndInfo.SerializeToJArray().ToString());
                logger.LogInformation($"Game took {timer.Elapsed}");
                return View(new GameStateViewModel { GameEndInfo = gameEndInfo });
            }
            finally
            {
                foreach (var player in players) player.Dispose();
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
