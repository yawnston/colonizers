using System;
using System.Collections.Generic;
using System.Text;
using Game.Commands;
using MediatR;

namespace Game.Entities.Colonists
{
    class Spy : Colonist
    {
        public override IList<IRequest<GameState>> GetActions(BoardState boardState)
        {
            IList<IRequest<GameState>> actions = new List<IRequest<GameState>>();
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
    }
}
