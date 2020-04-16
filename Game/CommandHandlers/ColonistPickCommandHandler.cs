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
    class ColonistPickCommandHandler : IRequestHandler<ColonistPickCommand, GameState>
    {
        readonly IColonistPickGetter colonistPickGetter;
        readonly IDrawGetter drawGetter;

        public ColonistPickCommandHandler(IColonistPickGetter colonistPickGetter, IDrawGetter drawGetter)
        {
            this.colonistPickGetter = colonistPickGetter;
            this.drawGetter = drawGetter;
        }

        public Task<GameState> Handle(ColonistPickCommand request, CancellationToken cancellationToken)
        {
            var board = request.BoardState;
            var player = board.Players[board.PlayerTurn - 1];
            if (board.GamePhase != BoardState.Phase.ColonistPick) throw new InvalidOperationException(request.ToString());

            // information about previous players' colonists
            for (int i = 1; i < player.ID; i++)
            {
                player.ColonistInformation[i] = board.PlayableColonists.Where(c => !board.AvailableColonists.Any(x => x.Name == c.Name)).ToList();
            }

            // pick colonist and remove him from available
            player.Colonist = board.AvailableColonists.Single(c => c.Name == request.Colonist);
            board.AvailableColonists.Remove(player.Colonist);

            // player knows his own colonist
            player.ColonistInformation[player.ID] = new List<Colonist> { player.Colonist };
            // information about following players' colonists
            for (int i = player.ID + 1; i <= GameConstants.PlayerCount; i++)
            {
                player.ColonistInformation[i] = new List<Colonist>(board.AvailableColonists);
            }

            if (board.PlayerTurn == board.Players.Count) // Move to next phase
            {
                board.PlayerTurn = 1;
                board.GamePhase++;
                return drawGetter.Process(board);
            }
            else
            {
                board.PlayerTurn++;
                return colonistPickGetter.Process(board);
            }
        }
    }
}
