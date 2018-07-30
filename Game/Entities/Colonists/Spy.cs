using System;
using System.Collections.Generic;
using System.Text;
using Game.Commands;
using MediatR;

namespace Game.Entities.Colonists
{
    class Spy : Colonist
    {
        public override ICollection<IRequest<GameState>> GetActions(BoardState boardState)
        {
            ICollection<IRequest<GameState>> actions = new List<IRequest<GameState>>();
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
