using Game.ActionGetters;
using Game.Commands;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game.CommandHandlers
{
    class ColonistPickCommandHandler : IRequestHandler<ColonistPickCommand, GameState>
    {
        readonly IColonistPickGetter colonistPickGetter;
        readonly IDrawGetter drawGetter;

        public ColonistPickCommandHandler(IColonistPickGetter colonistPickGetter, IDrawGetter drawGetter)
        {
            this.colonistPickGetter = colonistPickGetter;
            this.drawGetter = drawGetter;
        }

        public Task<GameState> Handle(ColonistPickCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.ColonistPick) throw new InvalidOperationException(request.ToString());

            board.Players[board.PlayerTurn - 1].Colonist = board.PlayableColonists.Single(c => c.Name == request.Colonist);
            board.AvailableColonists.Remove(board.Players[board.PlayerTurn - 1].Colonist);

            if (board.PlayerTurn == board.Players.Count) // Move to next phase
            {
                board.PlayerTurn = 1;
                board.GamePhase++;
                return drawGetter.Process(board);
            }
            else
            {
                board.PlayerTurn++;
                return colonistPickGetter.Process(board);
            }
        }
    }
}
