using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    public class PlayerInfo
    {
        // The player's character (characters are called colonists)
        public Colonist Colonist { get; set; }
        // The amount of currency at the player's disposal (called Omnium)
        public int Omnium { get; set; }
        // Modules in the player's hand
        public ICollection<Module> Hand { get; set; }
        // Modules the player has played and are out on the field
        public ICollection<Module> Colony { get; set; }
    }
}
