using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Game.GameConstants;

namespace Game.Commands
{
    class BuildNothingCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Action Type { get; set; } = Action.BuildNothing;

        public override string ToString()
        {
            return $@"Skip building module";
        }
    }
}
