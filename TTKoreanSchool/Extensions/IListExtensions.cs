using System;
using System.Collections.Generic;

namespace TTKoreanSchool.Extensions
{
    public static class IListExtensions
    {
        public static Random Rand { get; } = new Random();

        // Fisher–Yates shuffle
        public static void Shuffle<T>(this IList<T> list)
        {
            for(int i = 0; i < list.Count; ++i)
            {
                T temp = list[i];
                int randIdx = Rand.Next(i, list.Count);
                list[i] = list[randIdx];
                list[randIdx] = temp;
            }
        }
    }
}