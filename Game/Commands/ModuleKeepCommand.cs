using Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class ModuleKeepCommand<GameState>
    {
        public BoardState BoardState { get; set; }
        public Module Module { get; set; }
    }
}
