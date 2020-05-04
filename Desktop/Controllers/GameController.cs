using Game;
using Game.Players;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desktop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {

        private static GameEngine gameEngine;
        private readonly ILogger<GameController> logger;

        public GameController(GameEngine gameEngine, ILogger<GameController> logger)
        {
            GameController.gameEngine = gameEngine;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var players = new List<IPlayer>
            {
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player1Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\HeuristicIntelligence\HeuristicIntelligence.py", "Player2Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player3Pipe"),
                new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\RandomIntelligence\RandomIntelligence.py", "Player4Pipe"),
                //new AIPlayer(@"C:\Users\danie\Desktop\Skola\Colonizers\ISMCTSIntelligence\ISMCTSIntelligence.py", "Player4Pipe"),
            };

            logger.LogInformation("Starting game.");
            var gameState = gameEngine.InitializeGame();

            gameState = await gameEngine.ProcessTurn(gameState, players);

            return Ok(gameState);
        }
    }
}
