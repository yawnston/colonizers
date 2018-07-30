using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game.ActionGetters
{
    class GameEndGetter : IGameEndGetter
    {
        public Task<GameState> Process(BoardState boardState)
        {
            return Task.Run(() => GetActions(boardState));
        }

        GameState GetActions(BoardState boardState)
        {
            var state = new GameState();
            state.BoardState = boardState;
            state.GameOver = true;
            state.GameEndInfo = new GameEndInfo { Players = new List<PlayerEndInfo>() };
            int points; bool firstToFull = true;
            foreach(var p in boardState.Players) // Count victory points for each player
            {
                points = 0;
                foreach (var m in p.Colony) points += m.VictoryValue;

                if(p.Colony.Count == 8) // Bonus points for full colony
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
            return state;
        }
    }
}
