using Game.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class KeepModuleCommand : IRequest<GameState>
    {
        public BoardState BoardState { get; set; }
        public Module Module { get; set; }
    }
}
