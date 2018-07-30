using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Game.ActionGetters
{
    class ColonistPickGetter : IColonistPickGetter
    {
        public Task<GameState> Process(BoardState boardState)
        {
            return Task.Run(() => GetActions(boardState));
        }

        GameState GetActions(BoardState boardState)
        {
            var state = new GameState();
            state.BoardState = boardState;
            state.Actions = new List<IRequest<GameState>>();
            foreach(var c in boardState.AvailableColonists)
            {
                state.Actions.Add(new ColonistPickCommand { BoardState = boardState, Colonist = c});
            }
            return state;
        }
    }
}
