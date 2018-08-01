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
            foreach(var m in (from mod in request.BoardState.TempStorage where mod != request.Module select mod))
            {
                board.Deck.Add(m); // add the discarded modules to the bottom of the deck
            }
            board.GamePhase = BoardState.Phase.Power;
            board.TempStorage = null;

            return powerGetter.Process(board);
        }
    }
}
