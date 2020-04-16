using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Game.GameConstants;

namespace Game.Commands
{
    class SwapHandsCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        public string Target { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Action Type { get; set; } = Action.SwapHands;

        public override string ToString()
        {
            return $@"Swap hands: 
    Target: {Target.ToString()}";
        }
    }
}
