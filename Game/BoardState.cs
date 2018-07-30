using Game.Entities;
using Game.Entities.Colonists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class BoardState
    {
        public enum Phase { ColonistPick, Draw, Discard, Power, Build }

        public IList<PlayerInfo> Players { get; set; } = new List<PlayerInfo>();
        // Colonists that are playable this game
        public ICollection<Colonist> PlayableColonists { get; set; } = new List<Colonist>();
        // Colonists that are available for picking
        public ICollection<Colonist> AvailableColonists { get; set; } = new List<Colonist>();

        public ICollection<Module> Deck { get; set; } = new List<Module>();

        // Holds modules between card draw and discard
        public IList<Module> TempStorage { get; set; } = new List<Module>();

        public int PlayerTurn { get; set; } = 1;

        public Phase GamePhase { get; set; } = Phase.ColonistPick;

        public bool NewRound()
        {
            AvailableColonists = new List<Colonist>();
            foreach (var c in PlayableColonists) AvailableColonists.Add(c);
            PlayerTurn = 1;
            GamePhase = Phase.ColonistPick;

            bool anyPlayerFinished = (from p in Players where p.Colony.Count == 8 select p.Colony.Count).Any();
            return anyPlayerFinished;
        }
    }
}
