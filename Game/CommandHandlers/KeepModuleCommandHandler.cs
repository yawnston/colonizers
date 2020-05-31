using Game.ActionGetters;
using Game.Commands;
using MediatR;
using System;
using System.Linq;
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

            var module = request.BoardState.DiscardTempStorage.FirstOrDefault(m => m.Name == request.Module);
            if (module is null) throw new InvalidOperationException($"Module to keep {module} is not in the temp storage.");
            if (board.Players[board.PlayerTurn - 1].Hand.Count < GameConstants.MaxHandSize)
            {
                // If player's hand is full when they attempt to keep, the kept module is removed from play
                board.Players[board.PlayerTurn - 1].Hand.Add(module);
            }
            foreach (var m in request.BoardState.DiscardTempStorage.Where(x => x != module))
            {
                board.Deck.Add(m); // add the discarded modules to the bottom of the deck
            }
            board.GamePhase = BoardState.Phase.Power;
            board.DiscardTempStorage = null;

            return powerGetter.Process(board);
        }
    }
}
