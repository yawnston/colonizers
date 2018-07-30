using Game.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class StealOmniumCommand : IRequest<GameState>
    {
        public BoardState BoardState { get; set; }
        public Colonist Target { get; set; }
        public int Amount { get; set; }
    }
}
