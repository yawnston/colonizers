using Game.ActionGetters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Game
{
    public static class GameFactory
    {
        public static GameState NewGame(BoardState boardState, IServiceProvider serviceProvider)
        {
            var firstGetter = serviceProvider.GetService<IColonistPickGetter>();
            return firstGetter.Process(boardState).Result;
        }

        public static GameState NewGame(BoardState boardState, object serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
