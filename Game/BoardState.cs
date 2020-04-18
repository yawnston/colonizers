using Game.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class BoardState
    {
        /// <summary>
        /// A single round consists of these stages
        /// </summary>
        public enum Phase { ColonistPick, Draw, Discard, Power, Build }

        /// <summary>
        /// Player information
        /// </summary>
        public IList<PlayerInfo> Players { get; set; } = new List<PlayerInfo>();

        /// <summary>
        /// All playable colonists
        /// </summary>
        public IReadOnlyList<Colonist> PlayableColonists { get; set; } = new List<Colonist>();

        /// <summary>
        /// Colonists available for selection during ColonistPick phase
        /// </summary>
        public IList<Colonist> AvailableColonists { get; set; } = new List<Colonist>();

        /// <summary>
        /// Current state of the Module deck
        /// </summary>
        public List<Module> Deck { get; set; } = new List<Module>();

        /// <summary>
        /// Module deck at game start
        /// </summary>
        public IReadOnlyList<Module> StartingDeck { get; set; } = new List<Module>();

        /// <summary>
        /// Holds modules between Draw and Discard phases
        /// </summary>
        public IList<Module> DiscardTempStorage { get; set; } = new List<Module>();

        /// <summary>
        /// Indicates which player currently has their turn
        /// </summary>
        public int PlayerTurn { get; set; } = GameConstants.PlayerIDs.Min();

        /// <summary>
        /// The current game phase
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Phase GamePhase { get; set; } = Phase.ColonistPick;

        private static readonly Random random = new Random(GameConstants.ShuffleRandomSeed);

        /// <summary>
        /// Transitions the game board into a new round
        /// </summary>
        /// <returns>True if any player reached maximum colony capacity, False otherwise</returns>
        public bool NewRound()
        {
            AvailableColonists = new List<Colonist>(PlayableColonists);
            AvailableColonists.Shuffle();
            AvailableColonists.RemoveAt(0); // Remove a random colonist at the start of each round

            foreach (var player in Players) // Reset information sets every round
            {
                player.ColonistInformation = PlayerInfo.CreateDefaultColonistInformation();
            }

            PlayerTurn = GameConstants.PlayerIDs.Min(); // Player turns are based on ID
            GamePhase = Phase.ColonistPick;

            bool anyPlayerFinished = Players.Any(p => p.Colony.Count == GameConstants.MaxColonySize);
            return anyPlayerFinished;
        }

        /// <summary>
        /// Clone this board instance and apply a given player's information set to hide information they don't possess
        /// </summary>
        /// <param name="player">The player whose information set will be applied</param>
        /// <returns>A new board state from the point of view of the given player</returns>
        public BoardState CloneWithInformationSet(int player) => new BoardState
        {
            AvailableColonists = GamePhase == Phase.ColonistPick && PlayerTurn == player
                ? AvailableColonists
                : AvailableColonists.Select(_ => Colonist.Unknown).ToList(),
            Deck = Deck.Select(_ => Module.Unknown).ToList(),
            DiscardTempStorage = DiscardTempStorage,
            GamePhase = GamePhase,
            PlayableColonists = PlayableColonists,
            Players = Players.Select(p => p.CloneWithInformationSet(player, this)).ToList(),
            PlayerTurn = PlayerTurn,
            StartingDeck = StartingDeck
        };

        /// <summary>
        /// Clone this board instance and determinize it, applying the given player's information set
        /// </summary>
        /// <param name="player">The player whose information set will be applied</param>
        /// <returns>A new determinized board state with all hidden information revealed</returns>
        public BoardState CloneAndDeterminize(int player)
        {
            var clonedBoardState = new BoardState
            {
                AvailableColonists = AvailableColonists,
                Deck = new List<Module>(Deck),
                DiscardTempStorage = DiscardTempStorage,
                GamePhase = GamePhase,
                PlayableColonists = PlayableColonists,
                Players = Players.Select(x => x.DeepClone()).ToList(),
                PlayerTurn = PlayerTurn,
                StartingDeck = StartingDeck
            };
            clonedBoardState.DeterminizeColonists(player);
            clonedBoardState.DeterminizeModules(player);
            return clonedBoardState;
        }

        /// <summary>
        /// Reveal all colonist information to all players.
        /// Used during simulation to keep the information sets consistent.
        /// </summary>
        public void RevealColonistInformation()
        {
            foreach (var playerToReveal in Players)
            {
                foreach (var player in Players)
                {
                    player.ColonistInformation[playerToReveal.ID] = new List<Colonist> { playerToReveal.Colonist };
                }
            }
        }

        private void DeterminizeColonists(int player)
        {
            var currentPlayer = Players[player - 1];
            // For each other player, pick a random colonist from the information set and set that as their colonist
            // Then remove this colonist from information sets about other players
            foreach (var otherPlayer in Players.Where(p => p.ID != player))
            {
                var possibleColonists = currentPlayer.ColonistInformation[otherPlayer.ID];
                if (possibleColonists.Count > 1)
                {
                    var chosenColonist = possibleColonists[random.Next(possibleColonists.Count)];
                    otherPlayer.Colonist = chosenColonist;
                    foreach (var playerToUpdate in Players)
                    {
                        for (int i = 1; i <= GameConstants.PlayerCount; i++)
                        {
                            playerToUpdate.ColonistInformation[i] = playerToUpdate.ColonistInformation[i].Where(x => x.Name != chosenColonist.Name).ToList();
                        }
                        playerToUpdate.ColonistInformation[otherPlayer.ID] = new List<Colonist> { chosenColonist };
                    }
                }

                // Reveal player's own colonist to others
                otherPlayer.ColonistInformation[player] = new List<Colonist> { currentPlayer.Colonist };
            }
        }

        private void DeterminizeModules(int player)
        {
            // Save other player hand sizes and shuffle them into the deck
            var handSizeDict = new Dictionary<int, int>(GameConstants.PlayerCount - 1);
            foreach (var otherPlayer in Players.Where(p => p.ID != player))
            {
                handSizeDict[otherPlayer.ID] = otherPlayer.Hand.Count;
                Deck.AddRange(otherPlayer.Hand);
                otherPlayer.Hand = new List<Module>();
            }

            // Shuffle the deck
            Deck.Shuffle();

            int i = 0;
            // Deal modules into other players' hands according to their previous hand sizes
            foreach (var entry in handSizeDict)
            {
                for (int j = 0; j < entry.Value; j++)
                {
                    Players[entry.Key - 1].Hand.Add(Deck[i]);
                    i++;
                }
            }
            Deck.RemoveRange(0, i);
        }

        public PlayerInfo GetCurrentPlayer() => Players[PlayerTurn - 1];
    }
}
