using Game.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class ColonistPickCommand : IRequest<GameState>
    {
        public BoardState BoardState { get; set; }
        public Colonist Colonist { get; set; }
    }
}
