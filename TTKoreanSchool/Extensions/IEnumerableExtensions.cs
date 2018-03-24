using System;
using System.Collections.Generic;

namespace TTKoreanSchool.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach(T item in enumeration)
            {
                action(item);
            }

            return enumeration;
        }
    }
}