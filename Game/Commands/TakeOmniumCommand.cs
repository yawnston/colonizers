using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Game.GameConstants;

namespace Game.Commands
{
    class TakeOmniumCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Action Type { get; set; } = Action.TakeOmnium;

        public override string ToString()
        {
            return $@"Generate Omnium";
        }
    }
}
