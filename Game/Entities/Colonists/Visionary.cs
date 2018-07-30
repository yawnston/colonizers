using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Commands;
using MediatR;

namespace Game.Entities.Colonists
{
    class Visionary : Colonist
    {
        public override IList<IRequest<GameState>> GetActions(BoardState boardState)
        {
            // No special ability
            IList<IRequest<GameState>> actions = new List<IRequest<GameState>>();
            actions.Add(new DoNothingCommand { BoardState = boardState });
            return actions;
        }

        public override void PerformClassDrawAction(BoardState boardState)
        {
            // Visionary draws 2 extra cards at the start of his turn
            var module1 = boardState.Deck.First();
            boardState.Deck.Remove(module1);
            boardState.Players[boardState.PlayerTurn - 1].Hand.Add(module1);
            var module2 = boardState.Deck.First();
            boardState.Deck.Remove(module2);
            boardState.Players[boardState.PlayerTurn - 1].Hand.Add(module2);
        }

        public override string ToString()
        {
            return "Visionary";
        }
    }
}
