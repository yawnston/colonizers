using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MediatR;
using Game.Commands;

namespace Game.Entities.Colonists
{
    class Miner : Colonist
    {
        public override IList<IGameAction> GetActions(BoardState boardState)
        {
            // No special ability
            IList<IGameAction> actions = new List<IGameAction>();
            actions.Add(new DoNothingCommand { BoardState = boardState });
            return actions;
        }

        public override void PerformClassDrawAction(BoardState boardState)
        {
            int extraOmnium = 0;
            foreach (var m in (from module in boardState.Players[boardState.PlayerTurn - 1].Colony where module.Type == Module.Color.Blue select module)) extraOmnium++;
            boardState.Players[boardState.PlayerTurn - 1].Omnium += extraOmnium;
        }

        public override string ToString()
        {
            return "Miner";
        }
    }
}
