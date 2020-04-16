using System.Collections.Generic;
using System.Linq;

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
        public Dictionary<int, List<Colonist>> ColonistInformation { get; set; } = CreateDefaultColonistInformation();

        public int ID { get; set; }

        public PlayerInfo CloneWithInformationSet(int player, BoardState boardState) => new PlayerInfo
        {
            // Player's colonist is revealed to others at the start of his turn
            Colonist = ID == player || (boardState.GamePhase > BoardState.Phase.ColonistPick && boardState.PlayerTurn >= ID)
                ? Colonist
                : Colonist.Unknown,
            Omnium = Omnium,
            Colony = Colony,
            Hand = ID == player
                ? Hand
                : Hand.Select(_ => Module.Unknown).ToList(),
            ID = ID,
            ColonistInformation = ID == player
                ? ColonistInformation
                : CreateDefaultColonistInformation()
        };

        public static Dictionary<int, List<Colonist>> CreateDefaultColonistInformation()
        {
            var dict = new Dictionary<int, List<Colonist>>();
            foreach (int i in GameConstants.PlayerIDs)
            {
                dict.Add(i, new List<Colonist>());
            }
            return dict;
        }
    }
}
