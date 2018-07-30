using Game.ActionGetters;
using Game.Commands;
using Game.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.CommandHandlers
{
    class KeepModuleCommandHandler : IRequestHandler<KeepModuleCommand, GameState>
    {
        readonly IPowerGetter powerGetter;

        public KeepModuleCommandHandler(IPowerGetter powerGetter)
        {
            this.powerGetter = powerGetter;
        }

        public Task<GameState> Handle(KeepModuleCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.Discard) throw new InvalidOperationException(request.ToString());

            board.Players[board.PlayerTurn - 1].Hand.Add(request.Module);
            board.GamePhase = BoardState.Phase.Power;
            board.TempStorage = null;

            return powerGetter.Process(board);
        }
    }
}
