using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    public interface IGameAction : IRequest<GameState>
    {
        JObject Serialize();
    }
}
