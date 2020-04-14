using Game.Commands;
using System.Collections.Generic;
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
            state.Actions.Add(new KeepModuleCommand { BoardState = boardState, Module = boardState.DiscardTempStorage[0].Name });
            state.Actions.Add(new KeepModuleCommand { BoardState = boardState, Module = boardState.DiscardTempStorage[1].Name });
            return state;
        }
    }
}
