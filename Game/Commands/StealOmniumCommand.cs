﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Game.GameConstants;

namespace Game.Commands
{
    class StealOmniumCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        public string Target { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Action Type { get; set; } = Action.StealOmnium;

        public override string ToString()
        {
            return $@"Steal Omnium: 
    Target: {Target.ToString()}";
        }
    }
}
