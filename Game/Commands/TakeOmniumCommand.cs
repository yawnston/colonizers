using Game.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class TakeOmniumCommand : IRequest<GameState>
    {
        public BoardState BoardState { get; set; }
    }
}
