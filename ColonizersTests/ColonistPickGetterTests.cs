using Game;
using Game.ActionGetters;
using Game.Entities;
using Game.Entities.Colonists;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ColonizersTests
{
    public class ColonistPickGetterTests
    {
        [Fact]
        public async Task ShouldReturnActions()
        {
            BoardState boardState = new BoardState
            {
                AvailableColonists = new List<Colonist>
                {
                    new Visionary(),
                    new Visionary(),
                    new Visionary(),
                }
            };
            ColonistPickGetter getter = new ColonistPickGetter();
            GameState gameState = await getter.Process(boardState);
            Assert.Equal(3, gameState.Actions.Count);
        }
    }
}
