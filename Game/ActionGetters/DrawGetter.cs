using Game.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Game.ActionGetters
{
    public class DrawGetter : IDrawGetter
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
            state.Actions.Add(new TakeOmniumCommand { BoardState = boardState });
            state.Actions.Add(new DrawModulesCommand { BoardState = boardState });
            return state;
        }
    }
}
