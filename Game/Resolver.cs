﻿using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Resolver
    {
        readonly IMediator mediator;

        public Resolver(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<GameState> Resolve(IGameAction command)
        {
            return mediator.Send(command);
        }
    }
}
