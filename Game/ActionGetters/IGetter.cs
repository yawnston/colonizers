using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game.ActionGetters
{
    public interface IGetter
    {
        Task<GameState> Process(BoardState boardState);
    }
}
