using System;
using System.Threading.Tasks;

namespace Game.Players
{
    public interface IPlayer : IDisposable
    {
        Task<int> GetMove(GameState gameState, Resolver resolver);

        string Name { get; set; }
    }
}
