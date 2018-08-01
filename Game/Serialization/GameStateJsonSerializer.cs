using System;
using System.Collections.Generic;
using System.Text;
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
            foreach(var a in gameState.Actions)
            {
                actions.Add(a.Serialize());
            }
            result["Actions"] = actions;

            result["GameOver"] = false;

            return result.ToString();
        }

        public static string SerializeGameOver(GameState gameState)
        {
            return "placeholder";
        }
    }
}
