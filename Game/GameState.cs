using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class GameState
    {
        public bool GameOver { get; set; } = false;
        
        public GameEndInfo GameEndInfo { get; set; }
        public BoardState BoardState { get; set; }
        // Things the player can do on their turn
        public IList<IRequest<GameState>> Actions { get; set; }
    }
}
