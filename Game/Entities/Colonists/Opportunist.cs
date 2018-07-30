using System;
using System.Collections.Generic;
using System.Text;
using Game.Commands;
using MediatR;

namespace Game.Entities.Colonists
{
    public class Opportunist : Colonist
    {
        public override ICollection<IRequest<GameState>> GetActions(BoardState boardState)
        {
            // Steal up to 2 Omnium from the target Colonist
            ICollection<IRequest<GameState>> actions = new List<IRequest<GameState>>();
            foreach(var c in boardState.PlayableColonists)
            {
                actions.Add(new StealOmniumCommand { BoardState = boardState, Target = c, Amount = 2});
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
