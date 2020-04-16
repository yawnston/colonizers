using Game.Commands;
using System.Collections.Generic;
using System.Linq;

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
            int extraOmnium = boardState.Players[boardState.PlayerTurn - 1].Colony.Count(module => module.Type == Module.Color.Blue);
            boardState.Players[boardState.PlayerTurn - 1].Omnium += extraOmnium;
        }

        public Miner()
        {
            Name = "Miner";
        }
    }
}
