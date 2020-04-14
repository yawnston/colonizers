using Game.Entities;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Game.Serialization
{
    /// <summary>
    /// Serialize BoardState from the point of view of the active player (hides other players' secret information)
    /// </summary>
    public static class BoardStateJsonSerializer
    {
        public static JObject Serialize(BoardState boardState)
        {
            var result = new JObject();

            result["CurrentPlayer"] = SerializeCurrentPlayer(boardState.Players[boardState.PlayerTurn - 1]);
            var otherPlayers = new JArray();
            foreach (var p in (from pl in boardState.Players where pl.ID != boardState.PlayerTurn select pl))
            {
                otherPlayers.Add(SerializeOtherPlayer(p));
            }
            result["Players"] = otherPlayers;

            var playableColonists = new JArray();
            foreach (var c in boardState.PlayableColonists)
            {
                playableColonists.Add(c.ToString());
            }
            result["PlayableColonists"] = playableColonists;

            result["PlayerTurn"] = new JValue(boardState.PlayerTurn);
            result["GamePhase"] = new JValue(boardState.GamePhase.ToString());

            return result;
        }

        private static JObject SerializeCurrentPlayer(PlayerInfo playerInfo)
        {
            var player = new JObject();
            player["Colonist"] = new JValue(playerInfo.Colonist?.ToString() ?? "");
            player["Omnium"] = new JValue(playerInfo.Omnium);
            var hand = new JArray();
            foreach (var m in playerInfo.Hand)
            {
                hand.Add(JObject.FromObject(m));
            }
            player["Hand"] = hand;
            var colony = new JArray();
            foreach (var m in playerInfo.Colony)
            {
                colony.Add(JObject.FromObject(m));
            }
            player["Colony"] = colony;
            player["Number"] = playerInfo.ID;
            return player;
        }

        private static JObject SerializeOtherPlayer(PlayerInfo playerInfo)
        {
            var player = new JObject();
            player["Omnium"] = new JValue(playerInfo.Omnium);
            player["Handsize"] = new JValue(playerInfo.Hand.Count);
            var colony = new JArray();
            foreach (var m in playerInfo.Colony)
            {
                colony.Add(JObject.FromObject(m));
            }
            player["Colony"] = colony;
            player["Number"] = playerInfo.ID;
            return player;
        }
    }
}
