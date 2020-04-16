using Game.ActionGetters;
using Game.Commands;
using Game.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Game.CommandHandlers
{
    class TakeOmniumCommandHandler : IRequestHandler<TakeOmniumCommand, GameState>
    {
        private readonly IPowerGetter powerGetter;

        public TakeOmniumCommandHandler(IPowerGetter powerGetter)
        {
            this.powerGetter = powerGetter;
        }

        public Task<GameState> Handle(TakeOmniumCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            var currentPlayer = board.GetCurrentPlayer();
            if (board.GamePhase != BoardState.Phase.Draw) throw new InvalidOperationException(request.ToString());

            foreach (var player in board.Players) // Colonist is revealed at start of each player's turn
            {
                player.ColonistInformation[board.PlayerTurn] = new List<Colonist> { currentPlayer.Colonist };
            }

            currentPlayer.Colonist.PerformClassDrawAction(board);
            currentPlayer.Omnium += 2;

            board.GamePhase = BoardState.Phase.Power;
            return powerGetter.Process(board);
        }
    }
}
