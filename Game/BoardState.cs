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

        public BoardState InitRound()
        {
            // TODO
            for (int i = 0; i < 4; ++i) Players.Add(new PlayerInfo());

            PlayableColonists.Add(new Ecologist());
            PlayableColonists.Add(new General());
            PlayableColonists.Add(new Miner());
            PlayableColonists.Add(new Opportunist());
            PlayableColonists.Add(new Spy());
            PlayableColonists.Add(new Visionary());

            foreach (var c in PlayableColonists) AvailableColonists.Add(c);

            for (int i = 0; i < 4; ++i) Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Green });
            for (int i = 0; i < 4; ++i) Deck.Add(new Module { BuildCost = 4, VictoryValue = 4, Type = Module.Color.Green });
            for (int i = 0; i < 2; ++i) Deck.Add(new Module { BuildCost = 6, VictoryValue = 7, Type = Module.Color.Green });
            for (int i = 0; i < 1; ++i) Deck.Add(new Module { BuildCost = 8, VictoryValue = 10, Type = Module.Color.Green });

            for (int i = 0; i < 2; ++i) Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Blue });
            for (int i = 0; i < 2; ++i) Deck.Add(new Module { BuildCost = 3, VictoryValue = 3, Type = Module.Color.Blue });
            for (int i = 0; i < 4; ++i) Deck.Add(new Module { BuildCost = 4, VictoryValue = 4, Type = Module.Color.Blue });
            for (int i = 0; i < 2; ++i) Deck.Add(new Module { BuildCost = 8, VictoryValue = 9, Type = Module.Color.Blue });

            for (int i = 0; i < 2; ++i) Deck.Add(new Module { BuildCost = 1, VictoryValue = 2, Type = Module.Color.Red });
            for (int i = 0; i < 2; ++i) Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Red });
            for (int i = 0; i < 4; ++i) Deck.Add(new Module { BuildCost = 3, VictoryValue = 3, Type = Module.Color.Red });
            for (int i = 0; i < 3; ++i) Deck.Add(new Module { BuildCost = 5, VictoryValue = 5, Type = Module.Color.Red });
            for (int i = 0; i < 2; ++i) Deck.Add(new Module { BuildCost = 7, VictoryValue = 7, Type = Module.Color.Red });

            return this;
        }

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
