#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: EnumerableEx.cs
// Version:  2020-01-13 13:02
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Properties;

    /// <summary>
    ///     Provides static methods based on the <see cref="Enumerable"/> class.
    /// </summary>
    public static class EnumerableEx
    {
        /// <summary>
        ///     Performs the specified <see cref="Action{T}"/> on each element of the
        ///     <see cref="IEnumerable{T}"/> collection.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}"/> collection.
        /// </param>
        /// <param name="action">
        ///     The <see cref="Action{T}"/> delegate to perform on each element of the
        ///     <see cref="IEnumerable{T}"/> collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source or action is null.
        /// </exception>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            foreach (var item in source)
                action(item);
        }

        /// <summary>
        ///     Performs the specified <see cref="Action{T}"/> asynchronously on each
        ///     element of the <see cref="IEnumerable{T}"/> collection.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}"/> collection.
        /// </param>
        /// <param name="action">
        ///     The <see cref="Action{T}"/> delegate to perform on each element of the
        ///     <see cref="IEnumerable{T}"/> collection.
        /// </param>
        /// <param name="continueOnCapturedContext">
        ///     <see langword="true"/> to attempt to marshal the continuation back to the
        ///     original context captured; otherwise, <see langword="false"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source or action is null.
        /// </exception>
        public static async void ForEachAsync<TSource>(this IEnumerable<TSource> source, Action<TSource> action, bool continueOnCapturedContext = false)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            foreach (var item in source)
                await Task.Run(() => action(item)).ConfigureAwait(continueOnCapturedContext);
        }

        /// <summary>
        ///     Performs the specified <see cref="Action{T1, T2}"/> on each element of the
        ///     <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of values in the dictionary.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="IDictionary{TKey, TValue}"/>.
        /// </param>
        /// <param name="action">
        ///     The <see cref="Action{T1, T2}"/> delegate to perform on each element of the
        ///     <see cref="IDictionary{TKey, TValue}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source or action is null.
        /// </exception>
        public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> source, Action<TKey, TValue> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            foreach (var item in source)
                action(item.Key, item.Value);
        }

        /// <summary>
        ///     Performs the specified <see cref="Action{T1, T2}"/> asynchronously on each
        ///     element of the <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of values in the dictionary.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="IDictionary{TKey, TValue}"/>.
        /// </param>
        /// <param name="action">
        ///     The <see cref="Action{T1, T2}"/> delegate to perform on each element of the
        ///     <see cref="IDictionary{TKey, TValue}"/>.
        /// </param>
        /// <param name="continueOnCapturedContext">
        ///     <see langword="true"/> to attempt to marshal the continuation back to the
        ///     original context captured; otherwise, <see langword="false"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source or action is null.
        /// </exception>
        public static async void ForEachAsync<TKey, TValue>(this IDictionary<TKey, TValue> source, Action<TKey, TValue> action, bool continueOnCapturedContext = false)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            foreach (var item in source)
                await Task.Run(() => action(item.Key, item.Value)).ConfigureAwait(continueOnCapturedContext);
        }

        /// <summary>
        ///     Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     A sequence of values to invoke a transform function on.
        /// </param>
        /// <param name="selector">
        ///     A transform function to apply to each element.
        /// </param>
        /// <param name="timelimit">
        ///     The time limit in milliseconds.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source or selector is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The <see cref="Stack{T}"/> is empty.
        /// </exception>
        public static IEnumerable<TSource> RecursiveSelect<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> selector, long timelimit = 60000)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            var stack = new Stack<IEnumerator<TSource>>();
            var enumerator = source.GetEnumerator();
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (true)
                {
                    if (stopwatch.ElapsedMilliseconds < timelimit && (enumerator?.MoveNext() ?? false))
                    {
                        var element = enumerator.Current;
                        yield return element;
                        stack.Push(enumerator);
                        enumerator = selector(element)?.GetEnumerator();
                        continue;
                    }
                    if (stack.Count > 0)
                    {
                        enumerator?.Dispose();
                        enumerator = stack.Pop();
                        continue;
                    }
                    yield break;
                }
            }
            finally
            {
                enumerator?.Dispose();
                while (stack.Count > 0)
                {
                    enumerator = stack.Pop();
                    enumerator?.Dispose();
                }
            }
        }

        /// <summary>
        ///     Returns a element in a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <param name="indicator">
        ///     The indicator that determines which element is returned.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The indicator value is negative.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     No element satisfies the condition in predicate. -or- The source sequence
        ///     is empty.
        /// </exception>
        public static TSource Just<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default, int indicator = 0)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (indicator < 0)
                throw new ArgumentOutOfRangeException(nameof(indicator));
            if (predicate != default)
            {
                var flag = false;
                var count = 0;
                var result = default(TSource);
                foreach (var item in source.Where(item => predicate(item) && (flag = count++ >= indicator)))
                {
                    result = item;
                    break;
                }
                if (flag)
                    return result;
                throw new InvalidOperationException(ExceptionMessages.SequenceIsEmpty);
            }
            switch (source)
            {
                case IList<TSource> list:
                    if (list.Count <= indicator)
                        break;
                    return list[indicator];
                default:
                    using (var enumerator = source.GetEnumerator())
                    {
                        if (!enumerator.MoveNext())
                            break;
                        var current = enumerator.Current;
                        switch (indicator)
                        {
                            case 0:
                                return current;
                            default:
                                for (var i = 0; i < indicator; i++)
                                    enumerator.MoveNext();
                                current = enumerator.Current;
                                return current;
                        }
                    }
            }
            throw new InvalidOperationException(ExceptionMessages.SequenceIsEmpty);
        }

        /// <summary>
        ///     Returns a element in a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="indicator">
        ///     The indicator that determines which element is returned.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The indicator value is negative.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The source sequence is empty.
        /// </exception>
        public static TSource Just<TSource>(this IEnumerable<TSource> source, int indicator) =>
            source.Just(default, indicator);

        /// <summary>
        ///     Returns a element in a sequence that satisfies a specified condition or a
        ///     default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <param name="indicator">
        ///     The indicator that determines which element is returned.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The indicator value is negative.
        /// </exception>
        public static TSource JustOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default, int indicator = 0)
        {
            try
            {
                return source.Just(predicate, indicator);
            }
            catch (InvalidOperationException ex) when (ex.IsCaught())
            {
                return default;
            }
        }

        /// <summary>
        ///     Returns a element in a sequence that satisfies a specified condition or a
        ///     default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="indicator">
        ///     The indicator that determines which element is returned.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The indicator value is negative.
        /// </exception>
        public static TSource JustOrDefault<TSource>(this IEnumerable<TSource> source, int indicator) =>
            source.JustOrDefault(default, indicator);

        /// <summary>
        ///     Returns the second element in a sequence that satisfies a specified
        ///     condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The source sequence is empty.
        /// </exception>
        public static TSource Second<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.Just(predicate, 1);

        /// <summary>
        ///     Returns the second element in a sequence that satisfies a specified
        ///     condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        public static TSource SecondOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.JustOrDefault(predicate, 1);

        /// <summary>
        ///     Returns the third element in a sequence that satisfies a specified
        ///     condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The source sequence is empty.
        /// </exception>
        public static TSource Third<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.Just(predicate, 2);

        /// <summary>
        ///     Returns the third element in a sequence that satisfies a specified
        ///     condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        public static TSource ThirdOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.JustOrDefault(predicate, 2);

        /// <summary>
        ///     Returns the fourth element in a sequence that satisfies a specified
        ///     condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The source sequence is empty.
        /// </exception>
        public static TSource Fourth<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.Just(predicate, 3);

        /// <summary>
        ///     Returns the fourth element in a sequence that satisfies a specified
        ///     condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        public static TSource FourthOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.JustOrDefault(predicate, 3);

        /// <summary>
        ///     Returns the fifth element in a sequence that satisfies a specified
        ///     condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The source sequence is empty.
        /// </exception>
        public static TSource Fifth<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.Just(predicate, 4);

        /// <summary>
        ///     Returns the fifth element in a sequence that satisfies a specified
        ///     condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        public static TSource FifthOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.JustOrDefault(predicate, 4);

        /// <summary>
        ///     Returns the sixth element in a sequence that satisfies a specified
        ///     condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The source sequence is empty.
        /// </exception>
        public static TSource Sixth<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.Just(predicate, 5);

        /// <summary>
        ///     Returns the sixth element in a sequence that satisfies a specified
        ///     condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        public static TSource SixthOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.JustOrDefault(predicate, 5);

        /// <summary>
        ///     Returns the seventh element in a sequence that satisfies a specified
        ///     condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The source sequence is empty.
        /// </exception>
        public static TSource Seventh<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.Just(predicate, 6);

        /// <summary>
        ///     Returns the seventh element in a sequence that satisfies a specified
        ///     condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        public static TSource SeventhOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.JustOrDefault(predicate, 6);

        /// <summary>
        ///     Returns the eighth element in a sequence that satisfies a specified
        ///     condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The source sequence is empty.
        /// </exception>
        public static TSource Eighth<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.Just(predicate, 7);

        /// <summary>
        ///     Returns the eighth element in a sequence that satisfies a specified
        ///     condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        public static TSource EighthOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.JustOrDefault(predicate, 7);

        /// <summary>
        ///     Returns the ninth element in a sequence that satisfies a specified
        ///     condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The source sequence is empty.
        /// </exception>
        public static TSource Ninth<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.Just(predicate, 8);

        /// <summary>
        ///     Returns the ninth element in a sequence that satisfies a specified
        ///     condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        public static TSource NinthOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.JustOrDefault(predicate, 8);

        /// <summary>
        ///     Returns the tenth element in a sequence that satisfies a specified
        ///     condition.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The source sequence is empty.
        /// </exception>
        public static TSource Tenth<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.Just(predicate, 9);

        /// <summary>
        ///     Returns the tenth element in a sequence that satisfies a specified
        ///     condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to return an element from.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        public static TSource TenthOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate = default) =>
            source.JustOrDefault(predicate, 9);

        /// <summary>
        ///     Concatenates the members of a constructed <see cref="IEnumerable{T}"/>
        ///     collection of type <see cref="string"/>, using the specified separator
        ///     between each number.
        /// </summary>
        /// <param name="values">
        ///     An array that contains the elements to concatenate.
        /// </param>
        /// <param name="separator">
        ///     The string to use as a separator.
        /// </param>
        public static string Join(this IEnumerable<string> values, string separator = null)
        {
            if (values == null)
                return null;
            var s = string.Join(separator, values);
            return s;
        }

        /// <summary>
        ///     Concatenates the members of a constructed <see cref="IEnumerable{T}"/>
        ///     collection of type <see cref="string"/>, using the specified separator
        ///     between each number.
        /// </summary>
        /// <param name="values">
        ///     An array that contains the elements to concatenate.
        /// </param>
        /// <param name="separator">
        ///     The character to use as a separator.
        /// </param>
        public static string Join(this IEnumerable<string> values, char separator) =>
            values.Join(separator.ToStringDefault());

        /// <summary>
        ///     Returns a specified number of contiguous elements from the end of a
        ///     sequence.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence to return elements from.
        /// </param>
        /// <param name="count">
        ///     The number of elements to return.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The count value is negative.
        /// </exception>
        public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            var stack = new Stack<TSource>();
            using (var enumerator = source.Reverse().GetEnumerator())
                for (var i = 0; i < count; i++)
                {
                    if (!enumerator.MoveNext())
                        break;
                    stack.Push(enumerator.Current);
                }
            return stack;
        }
    }
}
