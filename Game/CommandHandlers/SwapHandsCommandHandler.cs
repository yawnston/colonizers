using Game.ActionGetters;
using Game.Commands;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game.CommandHandlers
{
    class SwapHandsCommandHandler : IRequestHandler<SwapHandsCommand, GameState>
    {
        readonly IBuildGetter buildGetter;

        public SwapHandsCommandHandler(IBuildGetter buildGetter)
        {
            this.buildGetter = buildGetter;
        }

        public Task<GameState> Handle(SwapHandsCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.Power) throw new InvalidOperationException(request.ToString());

            var currentPlayer = board.Players[board.PlayerTurn - 1];
            var targetPlayer = board.Players.FirstOrDefault(p => p.Colonist.Name == request.Target);
            if (targetPlayer != null)
            {
                var temp = targetPlayer.Hand;
                targetPlayer.Hand = currentPlayer.Hand;
                currentPlayer.Hand = temp;
            }
            board.GamePhase = BoardState.Phase.Build;

            return buildGetter.Process(board);
        }
    }
}
