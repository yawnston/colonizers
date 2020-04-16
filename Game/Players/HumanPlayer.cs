using System.Threading.Tasks;

namespace Game.Players
{
    public class HumanPlayer : IPlayer
    {
        public Task<int> GetMove(GameState gameState, Resolver resolver)
        {
            // TODO
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
