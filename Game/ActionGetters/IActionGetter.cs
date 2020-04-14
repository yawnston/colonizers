using System.Threading.Tasks;

namespace Game.ActionGetters
{
    public interface IActionGetter
    {
        Task<GameState> Process(BoardState boardState);
    }
}
