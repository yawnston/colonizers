using Game.Commands;
using System.Collections.Generic;

namespace Game
{
    public class GameState
    {
        /// <summary>
        /// Indicates whether the game is over in this state
        /// </summary>
        public bool GameOver { get; set; } = false;

        /// <summary>
        /// If the game is over, contains game results
        /// </summary>
        public GameEndInfo GameEndInfo { get; set; }

        /// <summary>
        /// The state of the game board
        /// </summary>
        public BoardState BoardState { get; set; }

        /// <summary>
        /// Things the current player can do on their turn
        /// </summary>
        public IList<IGameAction> Actions { get; set; }

        /// <summary>
        /// Clone this game instance and apply a given player's information set
        /// </summary>
        /// <param name="player">The player whose information set will be applied</param>
        /// <returns>A new game state from the point of view of the given player</returns>
        public GameState CloneWithInformationSet(int player) => new GameState
        {
            Actions = Actions,
            GameEndInfo = GameEndInfo,
            GameOver = GameOver,
            BoardState = BoardState.CloneWithInformationSet(player)
        };

        public GameState CloneAndDeterminize() => new GameState
        {
            // Possible actions don't change by determinizing since the current player is used
            Actions = Actions,
            GameEndInfo = GameEndInfo,
            GameOver = GameOver,
            BoardState = BoardState.CloneAndDeterminize(BoardState.PlayerTurn)
        };
    }
}
