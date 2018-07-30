using Game.ActionGetters;
using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.CommandHandlers
{
    class TakeOmniumCommandHandler : IRequestHandler<TakeOmniumCommand, GameState>
    {
        readonly IPowerGetter powerGetter;

        public TakeOmniumCommandHandler(IPowerGetter powerGetter)
        {
            this.powerGetter = powerGetter;
        }

        public Task<GameState> Handle(TakeOmniumCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.Draw) throw new InvalidOperationException(request.ToString());

            board.Players[board.PlayerTurn - 1].Omnium += 2;

            board.GamePhase = BoardState.Phase.Power;
            return powerGetter.Process(board);
        }
    }
}
