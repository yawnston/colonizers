using Game.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Game.Entities.Colonists
{
    public class Opportunist : Colonist
    {
        public override IList<IGameAction> GetActions(BoardState boardState)
        {
            // Steal up to 2 Omnium from the target Colonist
            IList<IGameAction> actions = new List<IGameAction>();
            // Cannot steal from self
            foreach (var c in boardState.PlayableColonists.Where(c => c.Name != boardState.Players[boardState.PlayerTurn - 1].Colonist.Name))
            {
                actions.Add(new StealOmniumCommand { BoardState = boardState, Target = c.Name });
            }
            actions.Add(new DoNothingCommand { BoardState = boardState });
            return actions;
        }

        public override void PerformClassDrawAction(BoardState boardState)
        {
            // No passive ability
        }

        public Opportunist()
        {
            Name = "Opportunist";
        }
    }
}
