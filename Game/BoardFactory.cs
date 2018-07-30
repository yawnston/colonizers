using Game.Entities;
using Game.Entities.Colonists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public static class BoardFactory
    {
        public static BoardState Standard()
        {
            var board = new BoardState();
            for (int i = 0; i < 4; ++i) board.Players.Add(new PlayerInfo { ID = i });

            board.PlayableColonists.Add(new Ecologist());
            board.PlayableColonists.Add(new General());
            board.PlayableColonists.Add(new Miner());
            board.PlayableColonists.Add(new Opportunist());
            board.PlayableColonists.Add(new Spy());
            board.PlayableColonists.Add(new Visionary());

            foreach (var c in board.PlayableColonists) board.AvailableColonists.Add(c);

            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Green });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 4, Type = Module.Color.Green });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 6, VictoryValue = 7, Type = Module.Color.Green });
            for (int i = 0; i < 1; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 10, Type = Module.Color.Green });

            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Blue });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 3, VictoryValue = 3, Type = Module.Color.Blue });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 4, Type = Module.Color.Blue });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 9, Type = Module.Color.Blue });

            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 1, VictoryValue = 2, Type = Module.Color.Red });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Red });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 3, VictoryValue = 3, Type = Module.Color.Red });
            for (int i = 0; i < 3; ++i) board.Deck.Add(new Module { BuildCost = 5, VictoryValue = 5, Type = Module.Color.Red });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 7, VictoryValue = 7, Type = Module.Color.Red });

            board.Deck.Shuffle();

            return board;
        }

        public static BoardState BasicFair()
        {
            var board = new BoardState();
            for (int i = 0; i < 4; ++i) board.Players.Add(new PlayerInfo { ID = i });

            board.PlayableColonists.Add(new Ecologist());
            board.PlayableColonists.Add(new General());
            board.PlayableColonists.Add(new Miner());
            board.PlayableColonists.Add(new Opportunist());
            board.PlayableColonists.Add(new Spy());
            board.PlayableColonists.Add(new Visionary());

            foreach (var c in board.PlayableColonists) board.AvailableColonists.Add(c);

            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Green });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 4, Type = Module.Color.Green });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 6, VictoryValue = 6, Type = Module.Color.Green });
            for (int i = 0; i < 1; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 8, Type = Module.Color.Green });

            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Blue });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 4, Type = Module.Color.Blue });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 6, VictoryValue = 6, Type = Module.Color.Blue });
            for (int i = 0; i < 1; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 8, Type = Module.Color.Blue });

            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Red });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 4, Type = Module.Color.Red });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 6, VictoryValue = 6, Type = Module.Color.Red });
            for (int i = 0; i < 1; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 8, Type = Module.Color.Red });

            board.Deck.Shuffle();

            return board;
        }

        public static BoardState BasicGreenAdvantage()
        {
            var board = new BoardState();
            for (int i = 0; i < 4; ++i) board.Players.Add(new PlayerInfo { ID = i });

            board.PlayableColonists.Add(new Ecologist());
            board.PlayableColonists.Add(new General());
            board.PlayableColonists.Add(new Miner());
            board.PlayableColonists.Add(new Opportunist());
            board.PlayableColonists.Add(new Spy());
            board.PlayableColonists.Add(new Visionary());

            foreach (var c in board.PlayableColonists) board.AvailableColonists.Add(c);

            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 3, Type = Module.Color.Green });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 5, Type = Module.Color.Green });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 6, VictoryValue = 7, Type = Module.Color.Green });
            for (int i = 0; i < 1; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 9, Type = Module.Color.Green });

            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Blue });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 4, Type = Module.Color.Blue });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 6, VictoryValue = 6, Type = Module.Color.Blue });
            for (int i = 0; i < 1; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 8, Type = Module.Color.Blue });

            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Red });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 4, Type = Module.Color.Red });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 6, VictoryValue = 6, Type = Module.Color.Red });
            for (int i = 0; i < 1; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 8, Type = Module.Color.Red });

            board.Deck.Shuffle();

            return board;
        }

        public static BoardState BasicGreenBlueAdvantage()
        {
            var board = new BoardState();
            for (int i = 0; i < 4; ++i) board.Players.Add(new PlayerInfo { ID = i });

            board.PlayableColonists.Add(new Ecologist());
            board.PlayableColonists.Add(new General());
            board.PlayableColonists.Add(new Miner());
            board.PlayableColonists.Add(new Opportunist());
            board.PlayableColonists.Add(new Spy());
            board.PlayableColonists.Add(new Visionary());

            foreach (var c in board.PlayableColonists) board.AvailableColonists.Add(c);

            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 3, Type = Module.Color.Green });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 5, Type = Module.Color.Green });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 6, VictoryValue = 7, Type = Module.Color.Green });
            for (int i = 0; i < 1; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 9, Type = Module.Color.Green });

            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 3, Type = Module.Color.Blue });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 5, Type = Module.Color.Blue });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 6, VictoryValue = 7, Type = Module.Color.Blue });
            for (int i = 0; i < 1; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 9, Type = Module.Color.Blue });

            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 2, VictoryValue = 2, Type = Module.Color.Red });
            for (int i = 0; i < 4; ++i) board.Deck.Add(new Module { BuildCost = 4, VictoryValue = 4, Type = Module.Color.Red });
            for (int i = 0; i < 2; ++i) board.Deck.Add(new Module { BuildCost = 6, VictoryValue = 6, Type = Module.Color.Red });
            for (int i = 0; i < 1; ++i) board.Deck.Add(new Module { BuildCost = 8, VictoryValue = 8, Type = Module.Color.Red });

            board.Deck.Shuffle();

            return board;
        }
    }
}
