using Desktop.Services;
using Game;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Desktop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {

        private readonly ILogger<GameController> logger;
        private readonly GameService gameService;

        public GameController(GameService playerService, ILogger<GameController> logger)
        {
            this.logger = logger;
            this.gameService = playerService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartGame()
        {
            logger.LogInformation("Starting game.");
            var gameState = await gameService.InitializeGame();

            return Ok(gameState);
        }

        [HttpPost("aiturn")]
        public async Task<IActionResult> ProcessAITurn()
        {
            return Ok(await gameService.ProcessAITurn());
        }

        [HttpPost("playerturn")]
        public async Task<IActionResult> ProcessPlayerTurn([FromBody] int move)
        {
            return Ok(await gameService.ProcessPlayerTurn(move));
        }

        [HttpPost("dispose")]
        public IActionResult DisposeGame()
        {
            gameService.DisposePlayers();
            return Ok();
        }
    }
}
