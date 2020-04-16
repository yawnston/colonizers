using Game.Entities;
using Newtonsoft.Json.Linq;

namespace Game
{
    public class PlayerEndInfo
    {
        public PlayerInfo Player { get; set; }

        public int VictoryPoints { get; set; }

        public int Ranking { get; set; }

        public JObject Serialize()
        {
            var result = new JObject();

            result["Player"] = new JValue(Player.ID);
            result["Points"] = new JValue(VictoryPoints);

            return result;
        }
    }
}
