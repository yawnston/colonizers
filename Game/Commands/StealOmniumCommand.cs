using Game.Entities;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class StealOmniumCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }
        public Colonist Target { get; set; }
        public int Amount { get; set; }

        public JObject Serialize()
        {
            var result = new JObject();

            result["Type"] = "StealOmnium";
            result["Target"] = Target.ToString();

            return result;
        }

        public override string ToString()
        {
            return $@"Steal Omnium: 
    Target: {Target.ToString()}";
        }
    }
}
