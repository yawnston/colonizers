using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Game.GameConstants;

namespace Game.Commands
{
    class DrawModulesCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Action Type { get; set; } = Action.DrawModules;

        public override string ToString()
        {
            return $@"Draw modules from the deck";
        }
    }
}
