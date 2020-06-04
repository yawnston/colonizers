using Game;
using Game.ActionGetters;
using Game.Entities;
using Game.Entities.Colonists;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColonizersTests
{
    public class PowerGetterTests
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
                        Omnium = 2,
                        Colonist = new Miner(),
                        Hand = new List<Module>
                        {
                            new Module(),
                        },
                        Colony = new List<Module>(),
                    }
                }
            };

            PowerGetter powerGetter = new PowerGetter();
            GameState gameState = await powerGetter.Process(boardState);
            Assert.Equal(1, gameState.Actions.Count);
        }
    }
}
