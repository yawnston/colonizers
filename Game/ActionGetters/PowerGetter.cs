using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game.ActionGetters
{
    public class PowerGetter : IPowerGetter
    {
        public Task<GameState> Process(BoardState boardState)
        {
            return Task.Run(() => GetActions(boardState));
        }

        GameState GetActions(BoardState boardState)
        {
            var state = new GameState();
            state.BoardState = boardState;
            state.Actions = boardState.Players[boardState.PlayerTurn - 1].Colonist.GetActions(boardState);

            return state;
        }
    }
}
