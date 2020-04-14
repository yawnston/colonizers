using System.Collections.Generic;

namespace Game.Entities
{
    public class PlayerInfo
    {
        /// <summary>
        /// The player's character (characters are called colonists)
        /// </summary>
        public Colonist Colonist { get; set; } = null;

        /// <summary>
        /// The amount of currency at the player's disposal (called Omnium)
        /// </summary>
        public int Omnium { get; set; } = 0;

        /// <summary>
        /// Modules in the player's hand
        /// </summary>
        public IList<Module> Hand { get; set; } = new List<Module>();

        /// <summary>
        /// Modules the player has played and are out on the field
        /// </summary>
        public IList<Module> Colony { get; set; } = new List<Module>();

        /// <summary>
        /// Information the player has about other players' colonists
        /// </summary>
        public Dictionary<int, List<Colonist>> ColonistInformation { get; set; } = new Dictionary<int, List<Colonist>>();

        public int ID { get; set; }

        public PlayerInfo()
        {
            foreach (int i in GameConstants.PlayerIDs)
            {
                ColonistInformation.Add(i, new List<Colonist>());
            }
        }
    }
}
