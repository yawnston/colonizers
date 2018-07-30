using Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class BoardState
    {
        public ICollection<PlayerInfo> Players { get; set; }
        // Colonists that are playable this game
        public ICollection<Colonist> PlayableColonists { get; set; }
        // Colonists that are available for picking
        public ICollection<Colonist> AvailableColonists { get; set; }
    }
}
