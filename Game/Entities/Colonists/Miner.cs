using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MediatR;

namespace Game.Entities.Colonists
{
    class Miner : Colonist
    {
        public override ICollection<IRequest<GameState>> GetActions(BoardState boardState)
        {
            // No special ability
            return new List<IRequest<BoardState>>();
        }

        public override void PerformClassDrawAction(BoardState boardState)
        {
            int extraOmnium = 0;
            foreach (var m in (from module in boardState.Players[boardState.PlayerTurn - 1].Colony where module.Type == Module.Color.Blue select module)) extraOmnium++;
            boardState.Players[boardState.PlayerTurn - 1].Omnium += extraOmnium;
        }
    }
}
