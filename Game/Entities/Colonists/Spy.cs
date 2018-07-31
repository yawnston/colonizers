using System;
using System.Collections.Generic;
using System.Text;
using Game.Commands;
using MediatR;

namespace Game.Entities.Colonists
{
    class Spy : Colonist
    {
        public override IList<IGameAction> GetActions(BoardState boardState)
        {
            IList<IGameAction> actions = new List<IGameAction>();
            foreach (var c in boardState.PlayableColonists)
            {
                actions.Add(new SwapHandsCommand { BoardState = boardState, Target = c });
            }
            actions.Add(new DoNothingCommand { BoardState = boardState });
            return actions;
        }

        public override void PerformClassDrawAction(BoardState boardState)
        {
            // No passive ability
        }

        public override string ToString()
        {
            return "Spy";
        }
    }
}
