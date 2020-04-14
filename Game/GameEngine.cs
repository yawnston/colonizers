using Game.Players;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// Core class for running a game instance
    /// </summary>
    public sealed class GameEngine
    {
        private readonly Resolver resolver;
        private readonly IServiceProvider serviceProvider;

        public GameEngine(Resolver resolver, IServiceProvider serviceProvider)
        {
            this.resolver = resolver;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Run a single game of Colonizers from start to end
        /// </summary>
        /// <param name="players">The list of players to use, from first to last</param>
        /// <returns>Information about game end state</returns>
        public async Task<GameEndInfo> RunGame(IReadOnlyList<IPlayer> players)
        {
            BoardState boardState = BoardFactory.Standard();
            GameState gameState = GameFactory.NewGame(boardState, serviceProvider);

            while (!gameState.GameOver)
            {
                IPlayer currentPlayer = players[boardState.PlayerTurn - 1];
                int moveId = await currentPlayer.GetMove(gameState, resolver).ConfigureAwait(false);
                var selectedMove = gameState.Actions[moveId];
                gameState = await resolver.Resolve(selectedMove).ConfigureAwait(false);
                boardState = gameState.BoardState;
            }

            // Game is over -> return the info from gameState (guaranteed to be there when GameOver is true)
            return gameState.GameEndInfo;
        }
    }
}
