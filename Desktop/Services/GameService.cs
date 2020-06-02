using Game;
using Game.Players;
using System;
using System.Threading.Tasks;

namespace Desktop.Services
{
    public class GameService
    {
        private readonly StateService stateService;
        private readonly GameEngine gameEngine;
        private readonly PlayerService playerService;

        public GameService(StateService stateService, GameEngine gameEngine, PlayerService playerService)
        {
            this.stateService = stateService;
            this.gameEngine = gameEngine;
            this.playerService = playerService;
        }

        public Task<GameState> InitializeGame(string[] playerNames)
        {
            playerService.InitPlayers(playerNames);
            var gameState = gameEngine.InitializeGame();
            stateService.GameState = gameState;
            return Task.FromResult(gameState);
        }

        public async Task<GameState> ProcessAITurn()
        {
            var gameState = await gameEngine.ProcessTurn(stateService.GameState, playerService.Players);
            stateService.GameState = gameState;
            return gameState;
        }

        public async Task<GameState> ProcessPlayerTurn(int action)
        {
            GameState gameState = stateService.GameState;
            HumanPlayer player = playerService.Players[gameState.BoardState.PlayerTurn - 1] as HumanPlayer;
            if (player == null)
            {
                throw new InvalidOperationException($"Player {gameState.BoardState.PlayerTurn} is not a human player, cannot process turn as human player.");
            }

            player.NextMove = action;
            var newGameState = await gameEngine.ProcessTurn(stateService.GameState, playerService.Players);
            stateService.GameState = newGameState;
            return newGameState;
        }

        public void DisposePlayers()
        {
            playerService.DisposePlayers();
        }
    }
}
