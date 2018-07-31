using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game.ActionGetters
{
    public class DiscardGetter : IDiscardGetter
    {
        public Task<GameState> Process(BoardState boardState)
        {
            return Task.Run(() => GetActions(boardState));
        }

        GameState GetActions(BoardState boardState)
        {
            var state = new GameState();
            state.BoardState = boardState;
            state.Actions = new List<IGameAction>();
            state.Actions.Add(new KeepModuleCommand { BoardState = boardState, Module = boardState.TempStorage[0] });
            state.Actions.Add(new KeepModuleCommand { BoardState = boardState, Module = boardState.TempStorage[1] });
            return state;
        }
    }
}
