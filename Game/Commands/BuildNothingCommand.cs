﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class BuildNothingCommand : IRequest<GameState>
    {
        public BoardState BoardState { get; set; }
    }
}
