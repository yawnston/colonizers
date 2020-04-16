using Game.ActionGetters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Game
{
    public static class GameFactory
    {
        public static GameState NewGame(BoardState boardState, IServiceProvider serviceProvider)
        {
            var firstGetter = serviceProvider.GetService<IColonistPickGetter>();
            return firstGetter.Process(boardState).Result;
        }
    }
}
