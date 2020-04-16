using Game.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            if (boardState.Players[boardState.PlayerTurn - 1].Hand.Count < GameConstants.MaxHandSize)
            {
                state.Actions.Add(new DrawModulesCommand { BoardState = boardState });
            }

            return state;
        }
    }
}
