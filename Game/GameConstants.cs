using Game.Entities;
using Game.Entities.Colonists;
using System.Collections.Generic;

namespace Game
{
    public static class GameConstants
    {
        /// <summary>
        /// Number of players in the game
        /// </summary>
        public const int PlayerCount = 4;

        /// <summary>
        /// Max size of a player's colony - indicates the end of the game
        /// </summary>
        public const int MaxColonySize = 8;

        /// <summary>
        /// Max amout of Modules a player can hold in their hand. If they reach this limit, they cannot draw more Modules.
        /// </summary>
        public const int MaxHandSize = 5;

        /// <summary>
        /// All types of actions possible in the game
        /// </summary>
        public enum Action
        {
            ColonistPick,
            TakeOmnium,
            DrawModules,
            KeepModule,
            StealOmnium,
            SwapHands,
            DoNothing,
            BuildModule,
            BuildNothing
        }

        /// <summary>
        /// List of IDs assigned to the players. Turn order is from smallest ID to largest.
        /// </summary>
        public static IReadOnlyList<int> PlayerIDs { get; } = new List<int> { 1, 2, 3, 4 };

        /// <summary>
        /// All colonists available in the game
        /// </summary>
        public static IReadOnlyList<Colonist> Colonists { get; }

        /// <summary>
        /// Starting Module deck
        /// </summary>
        public static IReadOnlyList<Module> Modules { get; }

        /// <summary>
        /// Seed for random generator which shuffles decks. Not a constant because it is expected to change.
        /// </summary>
        public static int ShuffleRandomSeed { get; } = 42;


        static GameConstants()
        {
            Colonists = InitColonists();
            Modules = InitModules();
        }

        private static List<Colonist> InitColonists()
        {
            var colonists = new List<Colonist>(PlayerCount + 2)
            {
                new Ecologist(),
                new General(),
                new Miner(),
                new Opportunist(),
                new Spy(),
                new Visionary()
            };

            return colonists;
        }

        private static List<Module> InitModules()
        {
            var modules = new List<Module>(52);

            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Oxygen Generator", BuildCost = 4, VictoryValue = 4, Type = Module.Color.Green });
            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Water Reservoir", BuildCost = 5, VictoryValue = 6, Type = Module.Color.Green });
            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Hydroponics Facility", BuildCost = 6, VictoryValue = 8, Type = Module.Color.Green });
            for (int i = 0; i < 1; ++i) modules.Add(new Module { Name = "Eco-Dome", BuildCost = 8, VictoryValue = 11, Type = Module.Color.Green });

            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Marketplace", BuildCost = 2, VictoryValue = 2, Type = Module.Color.Blue });
            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Warehouse", BuildCost = 3, VictoryValue = 3, Type = Module.Color.Blue });
            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Quarry", BuildCost = 5, VictoryValue = 6, Type = Module.Color.Blue });
            for (int i = 0; i < 1; ++i) modules.Add(new Module { Name = "Omnium Purification Plant", BuildCost = 8, VictoryValue = 10, Type = Module.Color.Blue });

            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Garrison", BuildCost = 1, VictoryValue = 1, Type = Module.Color.Red });
            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Barracks", BuildCost = 2, VictoryValue = 2, Type = Module.Color.Red });
            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Military Academy", BuildCost = 3, VictoryValue = 3, Type = Module.Color.Red });
            for (int i = 0; i < 1; ++i) modules.Add(new Module { Name = "Planetary Defense System", BuildCost = 6, VictoryValue = 7, Type = Module.Color.Red });

            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Housing Unit", BuildCost = 1, VictoryValue = 1, Type = Module.Color.None });
            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Spaceport", BuildCost = 4, VictoryValue = 5, Type = Module.Color.None });
            for (int i = 0; i < 4; ++i) modules.Add(new Module { Name = "Research Lab", BuildCost = 6, VictoryValue = 8, Type = Module.Color.None });
            for (int i = 0; i < 1; ++i) modules.Add(new Module { Name = "Mass Relay", BuildCost = 12, VictoryValue = 16, Type = Module.Color.None });

            return modules;
        }
    }
}
