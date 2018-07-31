using Game.Entities;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class SwapHandsCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }
        public Colonist Target { get; set; }

        public JObject Serialize()
        {
            var result = new JObject();

            result["Type"] = "SwapHands";
            result["Target"] = Target.ToString();

            return result;
        }

        public override string ToString()
        {
            return $@"Swap hands: 
    Target: {Target.ToString()}";
        }
    }
}
