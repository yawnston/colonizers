using Game.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Game.Entities.Colonists
{
    class Spy : Colonist
    {
        public override IList<IGameAction> GetActions(BoardState boardState)
        {
            IList<IGameAction> actions = new List<IGameAction>();
            // Cannot swap hands with self
            foreach (var c in boardState.PlayableColonists.Where(c => c.Name != boardState.Players[boardState.PlayerTurn - 1].Colonist.Name))
            {
                actions.Add(new SwapHandsCommand { BoardState = boardState, Target = c.Name });
            }
            actions.Add(new DoNothingCommand { BoardState = boardState });
            return actions;
        }

        public override void PerformClassDrawAction(BoardState boardState)
        {
            // No passive ability
        }

        public Spy()
        {
            Name = "Spy";
        }
    }
}
