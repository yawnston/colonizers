using MediatR;

namespace Game.Commands
{
    public interface IGameAction : IRequest<GameState>
    {
    }
}
