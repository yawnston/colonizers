using Game.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.ActionGetters
{
    public class BuildGetter : IBuildGetter
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
            foreach (var m in boardState.Players[boardState.PlayerTurn - 1].Hand)
            {
                if (m.BuildCost <= boardState.Players[boardState.PlayerTurn - 1].Omnium)
                {
                    state.Actions.Add(new BuildModuleCommand { BoardState = boardState, Module = m.Name });
                }
            }
            state.Actions.Add(new BuildNothingCommand { BoardState = boardState });
            return state;
        }
    }
}
