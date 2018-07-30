using Game.ActionGetters;
using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.CommandHandlers
{
    class SwapHandsCommandHandler : IRequestHandler<SwapHandsCommand, GameState>
    {
        readonly IBuildGetter buildGetter;
        private object gameEndGetter;

        public SwapHandsCommandHandler(IBuildGetter buildGetter)
        {
            this.buildGetter = buildGetter;
        }

        public Task<GameState> Handle(SwapHandsCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.Power) throw new InvalidOperationException(request.ToString());

            var currentPlayer = board.Players[board.PlayerTurn - 1];
            var targetPlayer = (from p in board.Players where p.Colonist.GetType() == request.Target.GetType() select p).FirstOrDefault();
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
