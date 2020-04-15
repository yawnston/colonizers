using Game.Commands;
using System;
using System.Collections.Generic;

namespace Game.Entities
{
    public abstract class Colonist
    {
        // Get any extra Omnium this colonizer earns from his passive powers
        public abstract void PerformClassDrawAction(BoardState boardState);

        // Get all actions this character can execute in the power phase
        public abstract IList<IGameAction> GetActions(BoardState boardState);

        public string Name { get; protected set; }

        /// <summary>
        /// Represents a Colonist that a player does not know about
        /// </summary>
        public static Colonist Unknown { get; } = new UnknownColonist();

        private class UnknownColonist : Colonist
        {
            public UnknownColonist()
            {
                Name = "Unknown";
            }

            public override IList<IGameAction> GetActions(BoardState boardState)
            {
                throw new InvalidOperationException("This class is not intended to be used in gameplay.");
            }

            public override void PerformClassDrawAction(BoardState boardState)
            {
                throw new InvalidOperationException("This class is not intended to be used in gameplay.");
            }
        }
    }
}
