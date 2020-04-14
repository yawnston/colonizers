using Game.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.ActionGetters
{
    public class ColonistPickGetter : IColonistPickGetter
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
            foreach (var c in boardState.AvailableColonists)
            {
                state.Actions.Add(new ColonistPickCommand { BoardState = boardState, Colonist = c.Name });
            }
            return state;
        }
    }
}
