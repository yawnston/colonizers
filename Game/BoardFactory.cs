using Game.Entities;
using System.Collections.Generic;

namespace Game
{
    public static class BoardFactory
    {
        /// <summary>
        /// Create the standard initial board state
        /// </summary>
        public static BoardState Standard()
        {
            var board = new BoardState();
            foreach (int i in GameConstants.PlayerIDs) board.Players.Add(new PlayerInfo { ID = i });

            board.PlayableColonists = GameConstants.Colonists;

            board.AvailableColonists = new List<Colonist>(board.PlayableColonists);
            board.AvailableColonists.Shuffle();
            board.AvailableColonists.RemoveAt(0); // Random colonist is removed from play every round

            board.StartingDeck = GameConstants.Modules;
            board.Deck = new List<Module>(board.StartingDeck);
            board.Deck.Shuffle();

            return board;
        }
    }
}
