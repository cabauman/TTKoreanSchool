using System;
using System.Collections.Generic;
using TTKoreanSchool.Utils;

namespace TTKoreanSchool.Extensions
{
    public static class IListExtensions
    {
        // Fisher–Yates shuffle
        public static void Shuffle<T>(this IList<T> list)
        {
            for(int i = 0; i < list.Count; ++i)
            {
                T temp = list[i];
                int randIdx = RandomUtil.Rand.Next(i, list.Count);
                list[i] = list[randIdx];
                list[randIdx] = temp;
            }
        }
    }
}