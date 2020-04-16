using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Game.Serialization
{
    public static class GameStateJsonSerializer
    {
        public static string Serialize(GameState gameState)
        {
            var result = new JObject();

            result["Board"] = BoardStateJsonSerializer.Serialize(gameState.BoardState);

            var actions = new JArray();
            foreach (var a in gameState.Actions)
            {
                actions.Add(JsonConvert.SerializeObject(a));
            }
            result["Actions"] = actions;

            result["GameOver"] = false;

            return JsonConvert.SerializeObject(result);
        }

        public static string SerializeGameOver(GameState gameState)
        {
            var result = new JObject();

            result["Board"] = BoardStateJsonSerializer.Serialize(gameState.BoardState);

            result["GameOver"] = true;

            result["GameEndInfo"] = gameState.GameEndInfo.SerializeToJArray();

            return JsonConvert.SerializeObject(result);
        }
    }
}
