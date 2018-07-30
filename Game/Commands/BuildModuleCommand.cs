using Game.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class BuildModuleCommand : IRequest<GameState>
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }
        public Module Module { get; set; }

        public override string ToString()
        {
            return $@"Build module: 
    Cost: {Module.BuildCost}
    Value: {Module.VictoryValue}
    Type: {Module.Type.ToString()}";
        }
    }
}
