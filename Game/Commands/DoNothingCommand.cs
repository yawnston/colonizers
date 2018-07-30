using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class DoNothingCommand : IRequest<GameState>
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        public override string ToString()
        {
            return $@"Skip colonist ability";
        }
    }
}
