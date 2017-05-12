#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Comparison.cs
// Version:  2017-05-12 11:39
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Provides static methods and base classes used for the comparison of two or more objects.
    /// </summary>
    public static class Comparison
    {
        /// <summary>
        ///     Searches for the specified one-dimensional array and returns the index of its first
        ///     occurrence in another one-dimensional array.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the elements of the array.
        /// </typeparam>
        /// <param name="source">
        ///     The one-dimensional array to search.
        /// </param>
        /// <param name="target">
        ///     The one-dimensional array to locate.
        /// </param>
        public static IEnumerable<int> IndexOf<T>(T[] source, T[] target)
        {
            if (target == null || source.Length < target.Length)
                yield break;
            for (var i = 0; i < source.Length - target.Length + 1; i++)
                if (!target.Where((data, index) => !source[i + index].Equals(data)).Any())
                    yield return i;
        }

        /// <summary>
        ///     Determines whether the value of this object instance is between two specified values.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        /// </typeparam>
        /// <param name="item">
        ///     The object value to compare.
        /// </param>
        /// <param name="start">
        ///     The start index value.
        /// </param>
        /// <param name="end">
        ///     The end index value.
        /// </param>
        public static bool IsBetween<T>(this T item, T start, T end) where T : IComparable, IComparable<T>
        {
            try
            {
                var c = Comparer<T>.Default;
                var a = c.Compare(item, start);
                var b = c.Compare(item, end);
                return a >= 0 && b <= 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether a specified sequence of elements occurs within this sequence of
        ///     elements.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the elements of the array.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence of elements to browse.
        /// </param>
        /// <param name="targets">
        ///     The sequence of elements to seek.
        /// </param>
        public static bool ContainsEx<T>(this IEnumerable<T> source, params T[][] targets)
        {
            try
            {
                var r = targets.Any(x => IndexOf(source.ToArray(), x).Any(y => y >= 0));
                return r;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether a specified string occurs within this sequence of strings. A
        ///     parameter specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to seek.
        /// </param>
        public static bool ContainsEx(this IEnumerable<string> source, StringComparison comparisonType, params string[] targets)
        {
            try
            {
                var r = source.Any(x => targets.Any(y => string.Equals(x, y, comparisonType)));
                return r;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether a specified string occurs within this sequence of strings. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to seek.
        /// </param>
        public static bool ContainsEx(this IEnumerable<string> source, params string[] targets) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Determines whether a specified substring occurs within this string. A parameter
        ///     specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to seek.
        /// </param>
        public static bool ContainsEx(this string source, StringComparison comparisonType, params string[] targets)
        {
            try
            {
                var r = targets.Any(x => source.IndexOf(x, 0, comparisonType) != -1);
                return r;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether a specified substring occurs within this string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to seek.
        /// </param>
        public static bool ContainsEx(this string source, params string[] targets) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Determines whether a specified characters occurs within this string. A parameter
        ///     specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="targets">
        ///     The sequence of characters to seek.
        /// </param>
        public static bool ContainsEx(this string source, StringComparison comparisonType, params char[] targets)
        {
            try
            {
                var r = targets.Any(x => source.IndexOf(x.ToString(), 0, comparisonType) != -1);
                return r;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether a specified characters occurs within this string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="targets">
        ///     The sequence of characters to seek.
        /// </param>
        public static bool ContainsEx(this string source, params char[] targets) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Determines whether the beginning of this string matches a string. A parameter
        ///     specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to compare seek.
        /// </param>
        public static bool StartsWithEx(this string source, StringComparison comparisonType, params string[] targets)
        {
            try
            {
                var r = targets.Any(b => source.StartsWith(b, comparisonType));
                return r;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the beginning of this string matches a string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to compare seek.
        /// </param>
        public static bool StartsWithEx(this string source, params string[] targets) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Determines whether the end of this string matches a string. A parameter
        ///     specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to compare seek.
        /// </param>
        public static bool EndsWithEx(this string source, StringComparison comparisonType, params string[] targets)
        {
            try
            {
                var r = targets.Any(b => source.EndsWith(b, comparisonType));
                return r;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the end of this string matches a string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to compare seek.
        /// </param>
        public static bool EndsWithEx(this string source, params string[] targets) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Determines whether this string instance is the same as a string of the specified string
        ///     array. A parameter specifies the culture, case, and sort rules used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to compare with the first.
        /// </param>
        public static bool EqualsEx(this string source, StringComparison comparisonType, params string[] targets)
        {
            try
            {
                var r = targets.Any(b => string.Equals(source, b, comparisonType));
                return r;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether this string instance is the same as a string of the specified string
        ///     array. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to compare with the first.
        /// </param>
        public static bool EqualsEx(this string source, params string[] targets) =>
            source.EqualsEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Provides a base class for comparison.
        /// </summary>
        public class AlphanumericComparer : IComparer<object>
        {
            private readonly bool _d;

            /// <summary>
            ///     Initilazies a new instance of the <see cref="AlphanumericComparer"/> class. A
            ///     parameter specifies whether the order is descended.
            /// </summary>
            /// <param name="descendent">
            ///     true to enable the descending order; otherwise, false.
            /// </param>
            public AlphanumericComparer(bool descendent = false)
            {
                _d = descendent;
            }

            /// <summary>
            ///     Compare two specified objects and returns an integer that indicates their relative
            ///     position in the sort order.
            /// </summary>
            /// <param name="a">
            ///     The first object to compare.
            /// </param>
            /// <param name="b">
            ///     The second object to compare.
            /// </param>
            public int Compare(object a, object b)
            {
                var s1 = !_d ? a as string : b as string;
                if (s1 == null)
                    return 0;
                var s2 = !_d ? b as string : a as string;
                if (s2 == null)
                    return 0;
                try
                {
                    var i1 = 0;
                    var i2 = 0;
                    while (i1 < s1.Length && i2 < s2.Length)
                    {
                        var c1 = s1[i1];
                        var ca1 = new char[s1.Length];
                        var l1 = 0;
                        do
                        {
                            ca1[l1++] = c1;
                            i1++;
                            if (i1 >= s1.Length)
                                break;
                            c1 = s1[i1];
                        }
                        while (char.IsDigit(c1) == char.IsDigit(ca1[0]));
                        var c2 = s2[i2];
                        var ca2 = new char[s2.Length];
                        var l2 = 0;
                        do
                        {
                            ca2[l2++] = c2;
                            i2++;
                            if (i2 >= s2.Length)
                                break;
                            c2 = s2[i2];
                        }
                        while (char.IsDigit(c2) == char.IsDigit(ca2[0]));
                        var str1 = new string(ca1);
                        var str2 = new string(ca2);
                        int r;
                        if (char.IsDigit(ca1[0]) && char.IsDigit(ca2[0]))
                        {
                            var ch1 = int.Parse(str1);
                            var ch2 = int.Parse(str2);
                            r = ch1.CompareTo(ch2);
                        }
                        else
                            r = string.Compare(str1, str2, StringComparison.InvariantCulture);
                        if (r != 0)
                            return r;
                    }
                    return s1.Length - s2.Length;
                }
                catch
                {
                    return string.Compare(s1, s2, StringComparison.InvariantCulture);
                }
            }
        }
    }
}
