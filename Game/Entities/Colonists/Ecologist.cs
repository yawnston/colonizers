using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediatR;

namespace Game.Entities.Colonists
{
    class Ecologist : Colonist
    {
        public override ICollection<IRequest<GameState>> GetActions(BoardState boardState)
        {
            // No special ability
            return new List<IRequest<BoardState>>();
        }

        public override void PerformClassDrawAction(BoardState boardState)
        {
            int extraOmnium = 0;
            foreach (var m in (from module in boardState.Players[boardState.PlayerTurn - 1].Colony where module.Type == Module.Color.Green select module)) extraOmnium++;
            boardState.Players[boardState.PlayerTurn - 1].Omnium += extraOmnium;
        }
    }
}
