using System;
using System.Collections.Generic;
using System.Linq;

namespace TTKSCore
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }

            return source;
        }

        /// <summary>
        /// Splits the sequence into two sequences, containing the elements 
        /// for which the given predicate returns <c>true</c> and <c>false</c> 
        /// respectively.
        /// </summary>
        public static void Partition<T>(
            this IEnumerable<T> source,
            Func<T, bool> predicate,
            out IEnumerable<T> trueSequence,
            out IEnumerable<T> falseSequence)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            IEnumerable<T> fs = null;
            IEnumerable<T> ts = null;

            foreach (var g in source.GroupBy(predicate))
            {
                if (g.Key)
                {
                    ts = g;
                }
                else
                {
                    fs = g;
                }
            }

            trueSequence = ts ?? Enumerable.Empty<T>();
            falseSequence = fs ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<TResult> ZipAll<TSource, TSecond, TResult>(
            this IEnumerable<TSource> source,
            IEnumerable<TSecond> other,
            Func<TSource, TSecond, TResult> projection)
        {
            using (var firstIterator = source.GetEnumerator())
            using (var secondIterator = other.GetEnumerator())
            {
                while (true)
                {
                    bool hasFirst = firstIterator.MoveNext();
                    bool hasSecond = secondIterator.MoveNext();

                    TSource first = hasFirst ? firstIterator.Current : default(TSource);
                    TSecond second = hasSecond ? secondIterator.Current : default(TSecond);

                    if (hasFirst || hasSecond)
                    {
                        yield return projection(first, second);
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }
    }
}
