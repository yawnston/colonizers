using Desktop.Services;
using Game;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Desktop.Controllers
{
    /// <summary>
    /// Controller responsible for interacting with the game engine
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {

        private readonly ILogger<GameController> logger;
        private readonly GameService gameService;

        public GameController(GameService gameService, ILogger<GameController> logger)
        {
            this.logger = logger;
            this.gameService = gameService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartGame([FromBody] string[] playerNames)
        {
            logger.LogInformation($"Starting game with players: {string.Join(", ", playerNames)}.");
            try
            {
                GameState gameState = await gameService.InitializeGame(playerNames);
                return Ok(gameState);
            }
            catch (InvalidOperationException e)
            {
                // Exceptions of this type are thrown when an AI is not found
                return BadRequest(e.Message);
            }
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
