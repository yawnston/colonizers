using Game.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class BuildModuleCommand : IRequest<GameState>
    {
        public GameState GameState { get; set; }
        public Module Module { get; set; }
    }
}
