using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    public abstract class Colonist
    {
        // Get any extra Omnium this colonizer earns from his passive powers
        public abstract void PerformClassDrawAction(BoardState boardState);

        // Get all actions this character can execute in the power phase
        public abstract IList<IGameAction> GetActions(BoardState boardState);
    }
}
