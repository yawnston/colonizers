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
    class BuildModuleCommandHandler : IRequestHandler<BuildModuleCommand, GameState>
    {
        readonly IColonistPickGetter colonistPickGetter;
        readonly IDrawGetter drawGetter;
        readonly IGameEndGetter gameEndGetter;

        public BuildModuleCommandHandler(IColonistPickGetter colonistPickGetter, IDrawGetter drawGetter, IGameEndGetter gameEndGetter)
        {
            this.colonistPickGetter = colonistPickGetter;
            this.drawGetter = drawGetter;
            this.gameEndGetter = gameEndGetter;
        }

        public Task<GameState> Handle(BuildModuleCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            if (board.GamePhase != BoardState.Phase.Build) throw new InvalidOperationException(request.ToString());

            var player = board.Players[board.PlayerTurn - 1];
            if (player.Omnium < request.Module.BuildCost) throw new InvalidOperationException($"Not enough omnium to build module: {request}");
            player.Omnium -= request.Module.BuildCost;
            player.Colony.Add(request.Module);

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
