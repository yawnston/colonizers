using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Game.GameConstants;

namespace Game.Commands
{
    class ColonistPickCommand : IGameAction
    {
        [JsonIgnore]
        public BoardState BoardState { get; set; }

        public string Colonist { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Action Type { get; set; } = Action.ColonistPick;

        public override string ToString()
        {
            return $@"Choose colonist: 
    Type: {Colonist.ToString()}";
        }
    }
}
