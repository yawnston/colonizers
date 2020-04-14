using Game.ActionGetters;
using Game.Commands;
using MediatR;
using System;
using System.Linq;
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
            var module = player.Hand.FirstOrDefault(m => m.Name == request.Module);
            if (module is null) throw new InvalidOperationException($"The given module is not in the player's hand: {request}");
            if (player.Omnium < module.BuildCost) throw new InvalidOperationException($"Not enough omnium to build module: {request}");

            player.Omnium -= module.BuildCost;
            player.Hand.Remove(module);
            player.Colony.Add(module);

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
