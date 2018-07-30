using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    public abstract class Colonist
    {
        // Get all actions this character can execute in the power phase
        public abstract IEnumerable<IRequest<BoardState>> GetActions(BoardState boardState);
    }
}
