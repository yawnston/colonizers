using Game;
using Game.ActionGetters;
using Game.Entities;
using Game.Entities.Colonists;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColonizersTests
{
    public class DiscardGetterTests
    {
        [Fact]
        public async Task ShouldGet()
        {
            var boardState = new BoardState
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
                },
                DiscardTempStorage = new List<Module> { new Module(), new Module() }
            };

            var discardGetter = new DiscardGetter();
            var gameState = await discardGetter.Process(boardState);
            Assert.Equal(2, gameState.Actions.Count);
        }
    }
}
