using System;
using System.Collections.Generic;

namespace Game
{
    public static class ShuffleList
    {
        private static readonly Random random = new Random(GameConstants.ShuffleRandomSeed);

        /// <summary>
        /// Randomly shuffle a list using the Fischer-Yates shuffle (https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle)
        /// </summary>
        /// <param name="listToShuffle">List to shuffle (in-place)</param>
        public static void Shuffle<T>(this IList<T> listToShuffle)
        {
            int i = listToShuffle.Count;
            while (i > 1)
            {
                i--;
                int j = random.Next(i + 1);
                T swapValue = listToShuffle[j];
                listToShuffle[j] = listToShuffle[i];
                listToShuffle[i] = swapValue;
            }
        }
    }
}
