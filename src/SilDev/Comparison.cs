#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Comparison.cs
// Version:  2020-01-03 12:52
// 
// Copyright (c) 2020, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    ///     Provides static methods and base classes used for the comparison of two or
    ///     more objects.
    /// </summary>
    public static class Comparison
    {
        /// <summary>
        ///     Determines whether the specified object is not empty.
        /// </summary>
        /// <param name="value">
        ///     The object to check.
        /// </param>
        public static bool IsNotEmpty(object value)
        {
            try
            {
                switch (value)
                {
                    case null:
                        return false;
                    case string asStr:
                        return !string.IsNullOrWhiteSpace(asStr);
                }
                bool r;
                using (var ms = new MemoryStream())
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(ms, value);
                    r = ms.Length > 0;
                }
                return r;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified value is nullable.
        /// </summary>
        /// <param name="value">
        ///     The value to check.
        /// </param>
        public static bool IsNullable<TSource>(this TSource value)
        {
            if (value == null)
                return true;
            var type = typeof(TSource);
            if (!type.IsValueType)
                return true;
            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        ///     Determines whether the value of this object instance is between two specified
        ///     values.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source.
        /// </typeparam>
        /// <param name="source">
        ///     The value to compare.
        /// </param>
        /// <param name="start">
        ///     The start index value.
        /// </param>
        /// <param name="end">
        ///     The end index value.
        /// </param>
        public static bool IsBetween<TSource>(this TSource source, TSource start, TSource end) where TSource : IComparable, IComparable<TSource>
        {
            try
            {
                var c = Comparer<TSource>.Default;
                var a = c.Compare(source, start);
                var b = c.Compare(source, end);
                return a >= 0 && b <= 0;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether a specified substring occurs within this string. A
        ///     parameter specifies the culture, case, and sort rules used in the comparison.
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
            if (string.IsNullOrEmpty(source) || targets == null || targets.All(string.IsNullOrEmpty))
                return false;
            return targets.Any(s => source.IndexOf(s, 0, comparisonType) != -1);
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
        ///     Determines whether a specified characters occurs within this string. A
        ///     parameter specifies the culture, case, and sort rules used in the comparison.
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
            if (string.IsNullOrEmpty(source) || targets == null)
                return false;
            return targets.Any(s => source.IndexOf(s.ToString(CultureConfig.GlobalCultureInfo), 0, comparisonType) != -1);
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
        ///     Determines whether a specified character occurs within this string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="target">
        ///     The character to seek.
        /// </param>
        public static bool ContainsEx(this string source, char target) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, target);

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
        ///     The sequence of strings to compare.
        /// </param>
        public static bool StartsWithEx(this string source, StringComparison comparisonType, params string[] targets)
        {
            if (string.IsNullOrEmpty(source) || targets == null)
                return false;
            return targets.Any(s => source.StartsWith(s, comparisonType));
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
        ///     The sequence of strings to compare.
        /// </param>
        public static bool StartsWithEx(this string source, params string[] targets) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the specified
        ///     characters. A parameter specifies the culture, case, and sort rules used in
        ///     the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="targets">
        ///     The sequence of characters to compare.
        /// </param>
        public static bool StartsWithEx(this string source, StringComparison comparisonType, params char[] targets)
        {
            if (string.IsNullOrEmpty(source) || targets == null)
                return false;
            return targets.Any(s => source.StartsWith(s.ToString(CultureConfig.GlobalCultureInfo), comparisonType));
        }

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the specified
        ///     characters. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is
        ///     used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="targets">
        ///     The sequence of characters to compare.
        /// </param>
        public static bool StartsWithEx(this string source, params char[] targets) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Determines whether the beginning of this string matches the specified
        ///     characters. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter
        ///     is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target">
        ///     The character to compare.
        /// </param>
        public static bool StartsWithEx(this string source, char target) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, target);

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
        ///     The sequence of strings to compare.
        /// </param>
        public static bool EndsWithEx(this string source, StringComparison comparisonType, params string[] targets)
        {
            if (string.IsNullOrEmpty(source) || targets == null)
                return false;
            return targets.Any(s => source.EndsWith(s, comparisonType));
        }

        /// <summary>
        ///     Determines whether the end of this string matches a string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to compare.
        /// </param>
        public static bool EndsWithEx(this string source, params string[] targets) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Determines whether the end of this string matches the specified string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target">
        ///     The string to compare.
        /// </param>
        public static bool EndsWithEx(this string source, string target) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, target);

        /// <summary>
        ///     Determines whether the end of this string matches one of the specified
        ///     characters. A parameter specifies the culture, case, and sort rules used
        ///     in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="targets">
        ///     The sequence of characters to compare.
        /// </param>
        public static bool EndsWithEx(this string source, StringComparison comparisonType, params char[] targets)
        {
            if (string.IsNullOrEmpty(source) || targets == null)
                return false;
            return targets.Any(s => source.EndsWith(s.ToString(CultureConfig.GlobalCultureInfo), comparisonType));
        }

        /// <summary>
        ///     Determines whether the end of this string matches one of the specified
        ///     characters. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter
        ///     is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="targets">
        ///     The sequence of characters to compare.
        /// </param>
        public static bool EndsWithEx(this string source, params char[] targets) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Determines whether the end of this string matches the specified characters.
        ///     The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for
        ///     this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target">
        ///     The character to compare.
        /// </param>
        public static bool EndsWithEx(this string source, char target) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, target);

        /// <summary>
        ///     Determines whether this string is the same as one of the string of the
        ///     specified sequence of strings. A parameter specifies the culture, case,
        ///     and sort rules used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="comparisonType">
        ///     The comparison type that specifies the culture, case, and sort rules.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to compare with the first string.
        /// </param>
        public static bool EqualsEx(this string source, StringComparison comparisonType, params string[] targets)
        {
            if (source == null && (targets == null || targets.All(s => s == null)) || string.IsNullOrEmpty(source) && targets.All(string.IsNullOrEmpty))
                return true;
            if (string.IsNullOrEmpty(source) || targets == null || targets.All(string.IsNullOrEmpty))
                return false;
            return targets.Any(s => string.Equals(source, s, comparisonType));
        }

        /// <summary>
        ///     Determines whether this string is the same as one of the string of the
        ///     specified sequence of strings. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for
        ///     this comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to compare with the first string.
        /// </param>
        public static bool EqualsEx(this string source, params string[] targets) =>
            source.EqualsEx(StringComparison.OrdinalIgnoreCase, targets);

        /// <summary>
        ///     Determines whether this string is the same as the specified string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for
        ///     this comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="target">
        ///     The second string to compare.
        /// </param>
        public static bool EqualsEx(this string source, string target) =>
            string.Equals(source, target, StringComparison.OrdinalIgnoreCase);
    }
}
