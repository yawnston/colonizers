using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.ActionGetters
{
    public class GameEndGetter : IGameEndGetter
    {
        public Task<GameState> Process(BoardState boardState)
        {
            return Task.Run(() => GetActions(boardState));
        }

        private GameState GetActions(BoardState boardState)
        {
            var state = new GameState();
            state.BoardState = boardState;
            state.GameOver = true;
            state.GameEndInfo = new GameEndInfo { Players = new List<PlayerEndInfo>() };
            int points; bool firstToFull = true;
            foreach (var p in boardState.Players) // Count victory points for each player
            {
                points = 0;
                foreach (var m in p.Colony) points += m.VictoryValue;

                if (p.Colony.Count == 8) // Bonus points for full colony
                {
                    if (firstToFull)
                    {
                        points += 4; // 4 points for first person to fill colony
                    }
                    else
                    {
                        points += 2; // 2 points for others who fill their colony
                    }
                }

                state.GameEndInfo.Players.Add(new PlayerEndInfo { Player = p, VictoryPoints = points });
            }

            var sortedResults = state.GameEndInfo.Players.OrderByDescending(p => p.VictoryPoints);
            int i = 1;
            foreach (var r in sortedResults)
            {
                r.Ranking = i;
                i++;
            }

            return state;
        }
    }
}
