using Game.Entities;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Commands
{
    class ColonistPickCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }
        public Colonist Colonist { get; set; }

        public JObject Serialize()
        {
            var result = new JObject();

            result["Type"] = "ColonistPick";
            result["Colonist"] = Colonist.ToString();

            return result;
        }

        public override string ToString()
        {
            return $@"Choose colonist: 
    Type: {Colonist.ToString()}";
        }
    }
}
