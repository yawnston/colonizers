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
    }
}
