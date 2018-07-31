using Game.Entities;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class TakeOmniumCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        public JObject Serialize()
        {
            var result = new JObject();

            result["Type"] = "TakeOmnium";

            return result;
        }

        public override string ToString()
        {
            return $@"Generate Omnium";
        }
    }
}
