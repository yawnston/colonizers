using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Commands;
using MediatR;

namespace Game.Entities.Colonists
{
    class General : Colonist
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
            foreach (var m in (from module in boardState.Players[boardState.PlayerTurn - 1].Colony where module.Type == Module.Color.Red select module)) extraOmnium++;
            boardState.Players[boardState.PlayerTurn - 1].Omnium += extraOmnium;
        }

        public override string ToString()
        {
            return "General";
        }
    }
}
