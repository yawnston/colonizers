using Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class BoardState
    {
        public enum Phase { ColonistPick, Draw, Discard, Power, Build }

        public IList<PlayerInfo> Players { get; set; }
        // Colonists that are playable this game
        public ICollection<Colonist> PlayableColonists { get; set; }
        // Colonists that are available for picking
        public ICollection<Colonist> AvailableColonists { get; set; }

        public ICollection<Module> Deck { get; set; }

        // Holds modules between card draw and discard
        public IList<Module> TempStorage { get; set; }

        public int PlayerTurn { get; set; }

        public Phase GamePhase { get; set; }

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
