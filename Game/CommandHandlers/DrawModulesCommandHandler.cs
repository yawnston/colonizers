using Game.ActionGetters;
using Game.Commands;
using Game.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Game.CommandHandlers
{
    class DrawModulesCommandHandler : IRequestHandler<DrawModulesCommand, GameState>
    {
        private readonly IDiscardGetter discardGetter;

        public DrawModulesCommandHandler(IDiscardGetter discardGetter)
        {
            this.discardGetter = discardGetter;
        }

        public Task<GameState> Handle(DrawModulesCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.Draw) throw new InvalidOperationException(request.ToString());

            foreach (var player in board.Players) // Colonist is revealed at start of each player's turn
            {
                player.ColonistInformation[board.PlayerTurn] = new List<Colonist> { board.GetCurrentPlayer().Colonist };
            }

            board.Players[board.PlayerTurn - 1].Colonist.PerformClassDrawAction(board);

            if (board.Deck.Count < 2)
            {
                var drawState = new GameState();
                drawState.BoardState = board;
                drawState.GameOver = true;
                drawState.GameEndInfo = new GameEndInfo();
                drawState.GameEndInfo.Players = new List<PlayerEndInfo>();
                foreach (var p in board.Players)
                {
                    drawState.GameEndInfo.Players.Add(new PlayerEndInfo { Player = p, VictoryPoints = 0 });
                }
                return Task.FromResult(drawState); // end the game with a draw if the module deck is exhausted
            }

            board.GamePhase = BoardState.Phase.Discard;
            board.DiscardTempStorage = new List<Module>();
            var module1 = board.Deck.First();
            board.Deck.Remove(module1);
            board.DiscardTempStorage.Add(module1);
            var module2 = board.Deck.First();
            board.Deck.Remove(module2);
            board.DiscardTempStorage.Add(module2);

            return discardGetter.Process(board);
        }
    }
}
