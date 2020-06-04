using Game;
using Game.ActionGetters;
using Game.Entities;
using Game.Entities.Colonists;
using Game.Players;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ColonizersTests
{
    public class DrawGetterTests
    {
        [Fact]
        public async Task ShouldReturn()
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
                }
            };

            var drawGetter = new DrawGetter();
            var gameState = await drawGetter.Process(boardState);
            Assert.Equal(2, gameState.Actions.Count);
        }
    }
}
