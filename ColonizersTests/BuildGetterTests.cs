using Game;
using Game.ActionGetters;
using Game.Entities;
using Game.Entities.Colonists;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColonizersTests
{
    public class BuildGetterTests
    {
        [Fact]
        public async Task ShouldGet()
        {
            BoardState boardState = new BoardState
            {
                Deck = new List<Module>
                {
                    new Module(),
                    new Module(),
                    new Module(),
                    new Module(),
                    new Module(),
                },
                PlayerTurn = 1,
                Players = new List<PlayerInfo>
                {
                    new PlayerInfo
                    {
                        ID = 1,
                        Omnium = 4,
                        Colonist = new Miner(),
                        Hand = new List<Module>
                        {
                            new Module { BuildCost = 5 },
                            new Module { BuildCost = 3 },
                            new Module { BuildCost = 1 }
                        },
                        Colony = new List<Module>(),
                    }
                },
                DiscardTempStorage = new List<Module> { new Module(), new Module() }
            };

            BuildGetter buildGetter = new BuildGetter();
            GameState gameState = await buildGetter.Process(boardState);
            Assert.Equal(3, gameState.Actions.Count);
        }
    }
}
