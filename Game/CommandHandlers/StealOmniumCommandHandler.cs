using Game.ActionGetters;
using Game.Commands;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game.CommandHandlers
{
    class StealOmniumCommandHandler : IRequestHandler<StealOmniumCommand, GameState>
    {
        readonly IBuildGetter buildGetter;

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
            var targetPlayer = board.Players.FirstOrDefault(p => p.Colonist.Name == request.Target);
            if (targetPlayer != null)
            {
                if (targetPlayer.Omnium < 2)
                {
                    amountStolen = targetPlayer.Omnium;
                    targetPlayer.Omnium = 0;
                }
                else
                {
                    amountStolen = 2;
                    targetPlayer.Omnium -= amountStolen;
                }
                currentPlayer.Omnium += amountStolen;
            }
            board.GamePhase = BoardState.Phase.Build;

            return buildGetter.Process(board);
        }
    }
}
