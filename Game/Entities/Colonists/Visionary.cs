using Game.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Game.Entities.Colonists
{
    public class Visionary : Colonist
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
            // Visionary draws 1 extra card at the start of his turn (if his hand is not full and deck is not empty)
            if (boardState.Players[boardState.PlayerTurn - 1].Hand.Count < GameConstants.MaxHandSize
                && boardState.Deck.Count > 0)
            {
                var module1 = boardState.Deck.First();
                boardState.Deck.Remove(module1);
                boardState.Players[boardState.PlayerTurn - 1].Hand.Add(module1);
            }
        }

        public Visionary()
        {
            Name = "Visionary";
        }
    }
}
