using Game.ActionGetters;
using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Game.CommandHandlers
{
    class StealOmniumCommandHandler : IRequestHandler<StealOmniumCommand, GameState>
    {
        readonly IBuildGetter buildGetter;
        private object gameEndGetter;

        public StealOmniumCommandHandler(IBuildGetter buildGetter)
        {
            this.buildGetter = buildGetter;
        }

        public Task<GameState> Handle(StealOmniumCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.Power) throw new InvalidOperationException(request.ToString());

            var currentPlayer = board.Players[board.PlayerTurn - 1];
            int amountStolen = 0;
            var targetPlayer = (from p in board.Players where p.Colonist.GetType() == request.Target.GetType() select p).FirstOrDefault();
            if(targetPlayer != null)
            {
                if(targetPlayer.Omnium < request.Amount)
                {
                    amountStolen = targetPlayer.Omnium;
                    targetPlayer.Omnium = 0;
                }
                else
                {
                    amountStolen = request.Amount;
                    targetPlayer.Omnium -= amountStolen;
                }
            }
            board.GamePhase = BoardState.Phase.Build;

            return buildGetter.Process(board);
        }
    }
}
