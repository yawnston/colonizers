﻿using Game.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Game.Entities.Colonists
{
    public class Ecologist : Colonist
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
            int extraOmnium = boardState.Players[boardState.PlayerTurn - 1].Colony.Count(module => module.Type == Module.Color.Green);
            boardState.Players[boardState.PlayerTurn - 1].Omnium += extraOmnium;
        }

        public Ecologist()
        {
            Name = "Ecologist";
        }
    }
}
