using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class DrawModulesCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        public JObject Serialize()
        {
            var result = new JObject();

            result["Type"] = "DrawModules";

            return result;
        }

        public override string ToString()
        {
            return $@"Draw modules from the deck";
        }
    }
}
