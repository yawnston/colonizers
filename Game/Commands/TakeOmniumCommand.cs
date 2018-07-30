using Game.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class TakeOmniumCommand : IRequest<GameState>
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        public override string ToString()
        {
            return $@"Generate Omnium";
        }
    }
}
