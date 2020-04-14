﻿using Game.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class BoardState
    {
        /// <summary>
        /// A single round consists of these stages
        /// </summary>
        public enum Phase { ColonistPick, Draw, Discard, Power, Build }

        /// <summary>
        /// Player information
        /// </summary>
        public IList<PlayerInfo> Players { get; set; } = new List<PlayerInfo>();

        /// <summary>
        /// All playable colonists
        /// </summary>
        public IReadOnlyList<Colonist> PlayableColonists { get; set; } = new List<Colonist>();

        /// <summary>
        /// Colonists available for selection during ColonistPick phase
        /// </summary>
        public IList<Colonist> AvailableColonists { get; set; } = new List<Colonist>();

        /// <summary>
        /// Current state of the Module deck
        /// </summary>
        public IList<Module> Deck { get; set; } = new List<Module>();

        /// <summary>
        /// Module deck at game start
        /// </summary>
        public IReadOnlyList<Module> StartingDeck { get; set; } = new List<Module>();

        /// <summary>
        /// Holds modules between Draw and Discard phases
        /// </summary>
        public IList<Module> DiscardTempStorage { get; set; } = new List<Module>();

        /// <summary>
        /// Indicates which player currently has their turn
        /// </summary>
        public int PlayerTurn { get; set; } = GameConstants.PlayerIDs.Min();

        /// <summary>
        /// The current game phase
        /// </summary>
        public Phase GamePhase { get; set; } = Phase.ColonistPick;

        /// <summary>
        /// Transitions the game board into a new round
        /// </summary>
        /// <returns>True if any player reached maximum colony capacity, False otherwise</returns>
        public bool NewRound()
        {
            AvailableColonists = new List<Colonist>(PlayableColonists);
            AvailableColonists.Shuffle();
            AvailableColonists.RemoveAt(0); // Remove a random colonist at the start of each round

            PlayerTurn = GameConstants.PlayerIDs.Min(); // Player turns are based on ID
            GamePhase = Phase.ColonistPick;

            bool anyPlayerFinished = Players.Any(p => p.Colony.Count == GameConstants.MaxColonySize);
            return anyPlayerFinished;
        }
    }
}
