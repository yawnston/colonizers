using System.Threading.Tasks;

namespace Game.Players
{
    public class HumanPlayer : IPlayer
    {
        /// <summary>
        /// Set this property before invoking the human player's turn to perform the given move
        /// </summary>
        public int NextMove { get; set; }

        public Task<int> GetMove(GameState gameState, Resolver resolver)
        {
            return Task.FromResult(NextMove);
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
