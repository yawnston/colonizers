using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    class GameState
    {
        public bool GameOver { get; set; } = false;
        public BoardState BoardState { get; set; }
        // Things the player can do on their turn
        public ICollection<IRequest> Actions { get; set; }
    }
}
