﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Game
{
    public class GameEndInfo
    {
        public IList<PlayerEndInfo> Players { get; set; }

        public JArray SerializeToJArray()
        {
            var serializedPlayers = new JArray();
            foreach (var p in Players)
            {
                serializedPlayers.Add(p.Serialize());
            }

            return serializedPlayers;
        }
    }
}
