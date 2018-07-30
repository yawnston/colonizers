using Game.ActionGetters;
using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.CommandHandlers
{
    class BuildNothingCommandHandler : IRequestHandler<BuildNothingCommand, GameState>
    {
        readonly IColonistPickGetter colonistPickGetter;
        readonly IDrawGetter drawGetter;
        readonly IGameEndGetter gameEndGetter;

        public BuildNothingCommandHandler(IColonistPickGetter colonistPickGetter, IDrawGetter drawGetter, IGameEndGetter gameEndGetter)
        {
            this.colonistPickGetter = colonistPickGetter;
            this.drawGetter = drawGetter;
            this.gameEndGetter = gameEndGetter;
        }

        public Task<GameState> Handle(BuildNothingCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.ColonistPick) throw new InvalidOperationException(request.ToString());

            if (board.PlayerTurn == board.Players.Count)
            {
                var gameEnd = board.NewRound();
                if (gameEnd)
                {
                    return gameEndGetter.Process(board);
                }
                else
                {
                    return colonistPickGetter.Process(board);
                }
            }
            else
            {
                board.PlayerTurn++;
                board.GamePhase = BoardState.Phase.Draw;
                return drawGetter.Process(board);
            }
        }
    }
}

