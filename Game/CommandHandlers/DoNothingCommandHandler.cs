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
    class DoNothingCommandHandler : IRequestHandler<DoNothingCommand, GameState>
    {
        readonly IBuildGetter buildGetter;

        public DoNothingCommandHandler(IBuildGetter buildGetter)
        {
            this.buildGetter = buildGetter;
        }

        public Task<GameState> Handle(DoNothingCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.Power) throw new InvalidOperationException(request.ToString());

            // Do nothing
            board.GamePhase = BoardState.Phase.Build;

            return buildGetter.Process(board);
        }
    }
}
