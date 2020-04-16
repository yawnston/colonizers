﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Game.GameConstants;

namespace Game.Commands
{
    class KeepModuleCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        public string Module { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Action Type { get; set; } = Action.KeepModule;

        public override string ToString()
        {
            return $"Keep module: {Module}";
        }
    }
}
