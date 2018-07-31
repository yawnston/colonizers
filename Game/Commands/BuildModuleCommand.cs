﻿using Game.Entities;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class BuildModuleCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }
        public Module Module { get; set; }

        public JObject Serialize()
        {
            var result = new JObject();

            result["Type"] = "BuildModule";
            result["Module"] = JObject.FromObject(Module);

            return result;
        }

        public override string ToString()
        {
            return $@"Build module: 
    Cost: {Module.BuildCost}
    Value: {Module.VictoryValue}
    Type: {Module.Type.ToString()}";
        }
    }
}
