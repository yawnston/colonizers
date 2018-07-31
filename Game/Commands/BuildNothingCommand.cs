using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class BuildNothingCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        public JObject Serialize()
        {
            var result = new JObject();

            result["Type"] = "BuildNothing";

            return result;
        }

        public override string ToString()
        {
            return $@"Skip building module";
        }
    }
}
