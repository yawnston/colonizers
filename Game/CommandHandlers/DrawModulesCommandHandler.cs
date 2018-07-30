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
    class DrawModulesCommandHandler : IRequestHandler<DrawModulesCommand, GameState>
    {
        readonly IDiscardGetter discardGetter;

        public DrawModulesCommandHandler(IDiscardGetter discardGetter)
        {
            this.discardGetter = discardGetter;
        }

        public Task<GameState> Handle(DrawModulesCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.Draw) throw new InvalidOperationException(request.ToString());

            board.Players[board.PlayerTurn - 1].Colonist.PerformClassDrawAction(board);

            board.GamePhase = BoardState.Phase.Discard;
            board.TempStorage = new List<Module>();
            var module1 = board.Deck.First();
            board.Deck.Remove(module1);
            board.TempStorage.Add(module1);
            var module2 = board.Deck.First();
            board.Deck.Remove(module2);
            board.TempStorage.Add(module2);

            return discardGetter.Process(board);
        }
    }
}
