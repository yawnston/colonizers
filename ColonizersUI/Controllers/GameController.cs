using ColonizersUI.Models;
using ElectronNET.API;
using Game;
using Game.Players;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ColonizersUI.Controllers
{
    public class GameController : Controller
    {
        private const string GameStatusChannel = "game-status-async";
        private const string GetGameStatusChannel = "get-game-status-async";
        private const string DisposeGameChannel = "dispose-game-async";

        private readonly GameEngine gameEngine;
        private readonly ILogger<GameController> logger;

        public GameController(GameEngine gameEngine, ILogger<GameController> logger)
        {
            this.gameEngine = gameEngine;
            this.logger = logger;
        }

        public IActionResult Index()
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

            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.RemoveAllListeners(GetGameStatusChannel);
                Electron.IpcMain.On(GetGameStatusChannel, async (args) =>
                {
                    gameState = await gameEngine.ProcessTurn(gameState, players);
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();
                    Electron.IpcMain.Send(mainWindow, GameStatusChannel, JsonConvert.SerializeObject(gameState));
                });
                Electron.IpcMain.RemoveAllListeners(DisposeGameChannel);
                Electron.IpcMain.On(DisposeGameChannel, (args) =>
                {
                    logger.LogInformation("Disposing players from callback.");
                    foreach (var player in players) player.Dispose();
                });
            }
            else
            {
                logger.LogError("Electron is not active, game IPC messaging will not work.");
            }

            return View(new GameStateViewModel { GameState = gameState });
        }
    }
}