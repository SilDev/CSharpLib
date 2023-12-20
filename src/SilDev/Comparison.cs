#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Comparison.cs
// Version:  2023-12-20 17:58
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
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
                    case string s:
                        return !string.IsNullOrWhiteSpace(s);
                }

                using var ms = new MemoryStream();
                var bf = new BinaryFormatter();
                bf.Serialize(ms, value);
                return ms.Length > 0;
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
        ///     Determines whether the value of this object instance is between two
        ///     specified values.
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

        #region ContainsEx

        /// <summary>
        ///     Determines whether the specified strings occurs within this string. A
        ///     parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The string to seek.
        /// </param>
        public static bool ContainsEx(this string source, StringComparison comparisonType, string target0)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target0))
                return false;
            return source.IndexOf(target0, 0, comparisonType) != -1;
        }

        /// <summary>
        ///     Determines whether one of the specified strings occurs within this string.
        ///     A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        public static bool ContainsEx(this string source, StringComparison comparisonType, string target0, string target1)
        {
            if (string.IsNullOrEmpty(source) || (string.IsNullOrEmpty(target0) && string.IsNullOrEmpty(target1)))
                return false;
            return (!string.IsNullOrEmpty(target0) && source.IndexOf(target0, 0, comparisonType) != -1) ||
                   (!string.IsNullOrEmpty(target1) && source.IndexOf(target1, 0, comparisonType) != -1);
        }

        /// <summary>
        ///     Determines whether one of the specified strings occurs within this string.
        ///     A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        /// <param name="target2">
        ///     The second string to seek.
        /// </param>
        public static bool ContainsEx(this string source, StringComparison comparisonType, string target0, string target1, string target2)
        {
            if (string.IsNullOrEmpty(source) || (string.IsNullOrEmpty(target0) && string.IsNullOrEmpty(target1) && string.IsNullOrEmpty(target2)))
                return false;
            return (!string.IsNullOrEmpty(target0) && source.IndexOf(target0, 0, comparisonType) != -1) ||
                   (!string.IsNullOrEmpty(target1) && source.IndexOf(target1, 0, comparisonType) != -1) ||
                   (!string.IsNullOrEmpty(target2) && source.IndexOf(target2, 0, comparisonType) != -1);
        }

        /// <summary>
        ///     Determines whether one of the specified strings occurs within this string.
        ///     A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
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
            if (string.IsNullOrEmpty(source) || targets == null)
                return false;
            for (var i = 0; i < targets.Length; i++)
            {
                if (targets[i] == null || source.IndexOf(targets[i], 0, comparisonType) == -1)
                    continue;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether the specified string occurs within this string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        public static bool ContainsEx(this string source, string target0) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, target0);

        /// <summary>
        ///     Determines whether one of the specified strings occurs within this string.
        ///     The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for
        ///     this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        public static bool ContainsEx(this string source, string target0, string target1) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, target0, target1);

        /// <summary>
        ///     Determines whether one of the specified strings occurs within this string.
        ///     The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for
        ///     this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        /// <param name="target2">
        ///     The second string to seek.
        /// </param>
        public static bool ContainsEx(this string source, string target0, string target1, string target2) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, target0, target1, target2);

        /// <summary>
        ///     Determines whether one of the specified strings occurs within this string.
        ///     The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for
        ///     this comparison.
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
        ///     Determines whether the specified character occurs within this string. A
        ///     parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The string to seek.
        /// </param>
        public static bool ContainsEx(this string source, StringComparison comparisonType, char target0)
        {
            if (string.IsNullOrEmpty(source))
                return false;
            return source.IndexOf(target0.ToStringCurrent(), 0, comparisonType) != -1;
        }

        /// <summary>
        ///     Determines whether one of the specified characters occurs within this
        ///     string. A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        public static bool ContainsEx(this string source, StringComparison comparisonType, char target0, char target1)
        {
            if (string.IsNullOrEmpty(source))
                return false;
            return source.IndexOf(target0.ToStringCurrent(), 0, comparisonType) != -1 ||
                   source.IndexOf(target1.ToStringCurrent(), 0, comparisonType) != -1;
        }

        /// <summary>
        ///     Determines whether one of the specified characters occurs within this
        ///     string. A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        /// <param name="target2">
        ///     The second string to seek.
        /// </param>
        public static bool ContainsEx(this string source, StringComparison comparisonType, char target0, char target1, char target2)
        {
            if (string.IsNullOrEmpty(source))
                return false;
            return source.IndexOf(target0.ToStringCurrent(), 0, comparisonType) != -1 ||
                   source.IndexOf(target1.ToStringCurrent(), 0, comparisonType) != -1 ||
                   source.IndexOf(target2.ToStringCurrent(), 0, comparisonType) != -1;
        }

        /// <summary>
        ///     Determines whether one of the specified characters occurs within this
        ///     string. A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
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
            for (var i = 0; i < targets.Length; i++)
            {
                if (source.IndexOf(targets[i].ToStringDefault(), 0, comparisonType) == -1)
                    continue;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether the specified character occurs within this string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="target0">
        ///     The character to seek.
        /// </param>
        public static bool ContainsEx(this string source, char target0) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, target0);

        /// <summary>
        ///     Determines whether one of the specified characters occurs within this
        ///     string. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is
        ///     used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        public static bool ContainsEx(this string source, char target0, char target1) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, target0, target1);

        /// <summary>
        ///     Determines whether one of the specified characters occurs within this
        ///     string. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is
        ///     used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        /// <param name="target2">
        ///     The second string to seek.
        /// </param>
        public static bool ContainsEx(this string source, char target0, char target1, char target2) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, target0, target1, target2);

        /// <summary>
        ///     Determines whether one of the specified characters occurs within this
        ///     string. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is
        ///     used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to browse.
        /// </param>
        /// <param name="targets">
        ///     The sequence of characters to seek.
        /// </param>
        public static bool ContainsEx(this string source, params char[] targets) =>
            source.ContainsEx(StringComparison.OrdinalIgnoreCase, targets);

        #endregion

        #region ContainsItem

        /// <summary>
        ///     Determines whether the specified element occurs within this sequence of
        ///     elements.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="target0">
        ///     The element to seek.
        /// </param>
        public static bool ContainsItem<TElement>(this IEnumerable<TElement> source, TElement target0) where TElement : IEquatable<TElement> =>
            target0 != null && (source?.Contains(target0) ?? false);

        /// <summary>
        ///     Determines whether one of specified elements occurs within this sequence of
        ///     elements.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="target0">
        ///     The first element to seek.
        /// </param>
        /// <param name="target1">
        ///     The second element to seek.
        /// </param>
        public static bool ContainsItem<TElement>(this IEnumerable<TElement> source, TElement target0, TElement target1)
        {
            if (source == null || (target0 == null && target1 == null))
                return false;
            var elements = source.AsArray();
            return (target0 != null && elements.Contains(target0)) ||
                   (target1 != null && elements.Contains(target1));
        }

        /// <summary>
        ///     Determines whether one of specified elements occurs within this sequence of
        ///     elements.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="target0">
        ///     The first element to seek.
        /// </param>
        /// <param name="target1">
        ///     The second element to seek.
        /// </param>
        /// <param name="target2">
        ///     The third element to seek.
        /// </param>
        public static bool ContainsItem<TElement>(this IEnumerable<TElement> source, TElement target0, TElement target1, TElement target2)
        {
            if (source == null || (target0 == null && target1 == null && target2 == null))
                return false;
            var elements = source.AsArray();
            return (target0 != null && elements.Contains(target0)) ||
                   (target1 != null && elements.Contains(target1)) ||
                   (target2 != null && elements.Contains(target2));
        }

        /// <summary>
        ///     Determines whether one of specified elements occurs within this sequence of
        ///     elements.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="targets">
        ///     The sequence of elements to seek.
        /// </param>
        public static bool ContainsItem<TElement>(this IEnumerable<TElement> source, params TElement[] targets)
        {
            if (source == null || targets == null)
                return false;
            var elements = source.AsArray();
            for (var i = 0; i < targets.Length; i++)
            {
                if (targets[i] == null || !elements.Contains(targets[i]))
                    continue;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether the specified element occurs within this sequence of
        ///     elements.
        /// </summary>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The string to seek.
        /// </param>
        public static bool ContainsItem(this IEnumerable<string> source, StringComparison comparisonType, string target0)
        {
            if (source == null || target0 == null)
                return false;
            var sa = source.AsArray();
            for (var i = 0; i < sa.Length; i++)
            {
                if (sa[i] == null || !string.Equals(sa[i], target0, comparisonType))
                    continue;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether one of specified elements occurs within this sequence of
        ///     elements.
        /// </summary>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        public static bool ContainsItem(this IEnumerable<string> source, StringComparison comparisonType, string target0, string target1)
        {
            if (source == null || (target0 == null && target1 == null))
                return false;
            var sa = source.AsArray();
            for (var i = 0; i < sa.Length; i++)
            {
                if (sa[i] == null)
                    continue;
                if ((target0 != null && string.Equals(sa[i], target0, comparisonType)) ||
                    (target1 != null && string.Equals(sa[i], target1, comparisonType)))
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether one of specified elements occurs within this sequence of
        ///     elements.
        /// </summary>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        /// <param name="target2">
        ///     The third string to seek.
        /// </param>
        public static bool ContainsItem(this IEnumerable<string> source, StringComparison comparisonType, string target0, string target1, string target2)
        {
            if (source == null || (target0 == null && target1 == null && target2 == null))
                return false;
            var sa = source.AsArray();
            for (var i = 0; i < sa.Length; i++)
            {
                if (sa[i] == null)
                    continue;
                if ((target0 != null && string.Equals(sa[i], target0, comparisonType)) ||
                    (target1 != null && string.Equals(sa[i], target1, comparisonType)) ||
                    (target2 != null && string.Equals(sa[i], target2, comparisonType)))
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether one of specified elements occurs within this sequence of
        ///     elements.
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
        public static bool ContainsItem(this IEnumerable<string> source, StringComparison comparisonType, params string[] targets)
        {
            if (source == null || targets == null)
                return false;
            var sa = source.AsArray();
            for (var i = 0; i < sa.Length; i++)
            {
                if (sa[i] == null)
                    continue;
                for (var j = 0; j < targets.Length; j++)
                {
                    if (targets[j] == null || !string.Equals(sa[i], targets[j], comparisonType))
                        continue;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Determines whether the specified string occurs within this sequence of
        ///     strings. The <see cref="CultureConfig.GlobalStringComparisonIgnoreCase"/>
        ///     parameter is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="target0">
        ///     The string to seek.
        /// </param>
        public static bool ContainsItem(this IEnumerable<string> source, string target0) =>
            source.ContainsItem(CultureConfig.GlobalStringComparisonIgnoreCase, target0);

        /// <summary>
        ///     Determines whether one of the specified strings occurs within this sequence
        ///     of strings. The
        ///     <see cref="CultureConfig.GlobalStringComparisonIgnoreCase"/> parameter is
        ///     used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        public static bool ContainsItem(this IEnumerable<string> source, string target0, string target1) =>
            source.ContainsItem(CultureConfig.GlobalStringComparisonIgnoreCase, target0, target1);

        /// <summary>
        ///     Determines whether one of the specified strings occurs within this sequence
        ///     of strings. The
        ///     <see cref="CultureConfig.GlobalStringComparisonIgnoreCase"/> parameter is
        ///     used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="target0">
        ///     The first string to seek.
        /// </param>
        /// <param name="target1">
        ///     The second string to seek.
        /// </param>
        /// <param name="target2">
        ///     The third string to seek.
        /// </param>
        public static bool ContainsItem(this IEnumerable<string> source, string target0, string target1, string target2) =>
            source.ContainsItem(CultureConfig.GlobalStringComparisonIgnoreCase, target0, target1, target2);

        /// <summary>
        ///     Determines whether one of specified strings occurs within this sequence of
        ///     strings. The <see cref="CultureConfig.GlobalStringComparisonIgnoreCase"/>
        ///     parameter is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The sequence to browse.
        /// </param>
        /// <param name="targets">
        ///     The sequence of strings to seek.
        /// </param>
        public static bool ContainsItem(this IEnumerable<string> source, params string[] targets) =>
            source.ContainsItem(CultureConfig.GlobalStringComparisonIgnoreCase, targets);

        #endregion

        #region StartsWithEx

        /// <summary>
        ///     Determines whether the beginning of this string matches the specified
        ///     string. A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The string to compare.
        /// </param>
        public static bool StartsWithEx(this string source, StringComparison comparisonType, string target0)
        {
            if (string.IsNullOrEmpty(source) || target0 == null)
                return false;
            return source.StartsWith(target0, comparisonType);
        }

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified strings. A parameter specifies the culture, case, and sort rules
        ///     used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first string to compare.
        /// </param>
        /// <param name="target1">
        ///     The second string to compare.
        /// </param>
        public static bool StartsWithEx(this string source, StringComparison comparisonType, string target0, string target1)
        {
            if (string.IsNullOrEmpty(source) || (target0 == null && target1 == null))
                return false;
            return (!string.IsNullOrEmpty(target0) && source.StartsWith(target0, comparisonType)) ||
                   (!string.IsNullOrEmpty(target1) && source.StartsWith(target1, comparisonType));
        }

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified strings. A parameter specifies the culture, case, and sort rules
        ///     used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first string to compare.
        /// </param>
        /// <param name="target1">
        ///     The second string to compare.
        /// </param>
        /// <param name="target2">
        ///     The third string to compare.
        /// </param>
        public static bool StartsWithEx(this string source, StringComparison comparisonType, string target0, string target1, string target2)
        {
            if (string.IsNullOrEmpty(source) || (target0 == null && target1 == null && target2 == null))
                return false;
            return (!string.IsNullOrEmpty(target0) && source.StartsWith(target0, comparisonType)) ||
                   (!string.IsNullOrEmpty(target1) && source.StartsWith(target1, comparisonType)) ||
                   (!string.IsNullOrEmpty(target2) && source.StartsWith(target2, comparisonType));
        }

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified strings. A parameter specifies the culture, case, and sort rules
        ///     used in the comparison.
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
            for (var i = 0; i < targets.Length; i++)
            {
                if (string.IsNullOrEmpty(targets[i]) || !source.StartsWith(targets[i], comparisonType))
                    continue;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether the beginning of this string matches the specified
        ///     string. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is
        ///     used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The string to compare.
        /// </param>
        public static bool StartsWithEx(this string source, string target0) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, target0);

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified strings. The <see cref="StringComparison.OrdinalIgnoreCase"/>
        ///     parameter is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The first string to compare.
        /// </param>
        /// <param name="target1">
        ///     The second string to compare.
        /// </param>
        public static bool StartsWithEx(this string source, string target0, string target1) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, target0, target1);

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified strings. The <see cref="StringComparison.OrdinalIgnoreCase"/>
        ///     parameter is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The first string to compare.
        /// </param>
        /// <param name="target1">
        ///     The second string to compare.
        /// </param>
        /// <param name="target2">
        ///     The third string to compare.
        /// </param>
        public static bool StartsWithEx(this string source, string target0, string target1, string target2) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, target0, target1, target2);

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified strings. The <see cref="StringComparison.OrdinalIgnoreCase"/>
        ///     parameter is used for this comparison.
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
        ///     Determines whether the beginning of this string matches the specified
        ///     character. A parameter specifies the culture, case, and sort rules used in
        ///     the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The character to compare.
        /// </param>
        public static bool StartsWithEx(this string source, StringComparison comparisonType, char target0) =>
            !string.IsNullOrEmpty(source) && source.StartsWith(target0.ToStringCurrent(), comparisonType);

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified characters. A parameter specifies the culture, case, and sort
        ///     rules used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first character to compare.
        /// </param>
        /// <param name="target1">
        ///     The second character to compare.
        /// </param>
        public static bool StartsWithEx(this string source, StringComparison comparisonType, char target0, char target1)
        {
            if (string.IsNullOrEmpty(source))
                return false;
            return source.StartsWith(target0.ToStringCurrent(), comparisonType) ||
                   source.StartsWith(target1.ToStringCurrent(), comparisonType);
        }

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified characters. A parameter specifies the culture, case, and sort
        ///     rules used in the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first character to compare.
        /// </param>
        /// <param name="target1">
        ///     The second character to compare.
        /// </param>
        /// <param name="target2">
        ///     The third character to compare.
        /// </param>
        public static bool StartsWithEx(this string source, StringComparison comparisonType, char target0, char target1, char target2)
        {
            if (string.IsNullOrEmpty(source))
                return false;
            return source.StartsWith(target0.ToStringCurrent(), comparisonType) ||
                   source.StartsWith(target1.ToStringCurrent(), comparisonType) ||
                   source.StartsWith(target2.ToStringCurrent(), comparisonType);
        }

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified characters. A parameter specifies the culture, case, and sort
        ///     rules used in the comparison.
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
            for (var i = 0; i < targets.Length; i++)
            {
                if (!source.StartsWith(targets[i].ToStringCurrent(), comparisonType))
                    continue;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether the beginning of this string matches the specified
        ///     character. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter
        ///     is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The character to compare.
        /// </param>
        public static bool StartsWithEx(this string source, char target0) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, target0);

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified characters. The <see cref="StringComparison.OrdinalIgnoreCase"/>
        ///     parameter is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The first character to compare.
        /// </param>
        /// <param name="target1">
        ///     The second character to compare.
        /// </param>
        public static bool StartsWithEx(this string source, char target0, char target1) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, target0, target1);

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified characters. The <see cref="StringComparison.OrdinalIgnoreCase"/>
        ///     parameter is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The first character to compare.
        /// </param>
        /// <param name="target1">
        ///     The second character to compare.
        /// </param>
        /// <param name="target2">
        ///     The third character to compare.
        /// </param>
        public static bool StartsWithEx(this string source, char target0, char target1, char target2) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, target0, target1, target2);

        /// <summary>
        ///     Determines whether the beginning of this string matches one of the
        ///     specified characters. The <see cref="StringComparison.OrdinalIgnoreCase"/>
        ///     parameter is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="targets">
        ///     The sequence of characters to compare.
        /// </param>
        public static bool StartsWithEx(this string source, params char[] targets) =>
            source.StartsWithEx(StringComparison.OrdinalIgnoreCase, targets);

        #endregion

        #region EndsWithEx

        /// <summary>
        ///     Determines whether the end of this string matches the specified string. A
        ///     parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The string to compare.
        /// </param>
        public static bool EndsWithEx(this string source, StringComparison comparisonType, string target0)
        {
            if (string.IsNullOrEmpty(source) || target0 == null)
                return false;
            return !string.IsNullOrEmpty(target0) && source.EndsWith(target0, comparisonType);
        }

        /// <summary>
        ///     Determines whether the end of this string matches one of the specified
        ///     strings. A parameter specifies the culture, case, and sort rules used in
        ///     the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first string to compare.
        /// </param>
        /// <param name="target1">
        ///     The second string to compare.
        /// </param>
        public static bool EndsWithEx(this string source, StringComparison comparisonType, string target0, string target1)
        {
            if (string.IsNullOrEmpty(source) || (string.IsNullOrEmpty(target0) && string.IsNullOrEmpty(target1)))
                return false;
            return (!string.IsNullOrEmpty(target0) && source.EndsWith(target0, comparisonType)) ||
                   (!string.IsNullOrEmpty(target1) && source.EndsWith(target1, comparisonType));
        }

        /// <summary>
        ///     Determines whether the end of this string matches one of the specified
        ///     strings. A parameter specifies the culture, case, and sort rules used in
        ///     the comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        /// <param name="target0">
        ///     The first string to compare.
        /// </param>
        /// <param name="target1">
        ///     The second string to compare.
        /// </param>
        /// <param name="target2">
        ///     The third string to compare.
        /// </param>
        public static bool EndsWithEx(this string source, StringComparison comparisonType, string target0, string target1, string target2)
        {
            if (string.IsNullOrEmpty(source) || (string.IsNullOrEmpty(target0) && string.IsNullOrEmpty(target1) && string.IsNullOrEmpty(target2)))
                return false;
            return (!string.IsNullOrEmpty(target0) && source.EndsWith(target0, comparisonType)) ||
                   (!string.IsNullOrEmpty(target1) && source.EndsWith(target1, comparisonType)) ||
                   (!string.IsNullOrEmpty(target2) && source.EndsWith(target2, comparisonType));
        }

        /// <summary>
        ///     Determines whether the end of this string matches one of the specified
        ///     strings. A parameter specifies the culture, case, and sort rules used in
        ///     the comparison.
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
            for (var i = 0; i < targets.Length; i++)
            {
                if (string.IsNullOrEmpty(targets[i]) || !source.EndsWith(targets[i], comparisonType))
                    continue;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether the end of this string matches the specified string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The string to compare.
        /// </param>
        public static bool EndsWithEx(this string source, string target0) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, target0);

        /// <summary>
        ///     Determines whether the end of this string matches one of the specified
        ///     strings. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is
        ///     used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The first string to compare.
        /// </param>
        /// <param name="target1">
        ///     The second string to compare.
        /// </param>
        public static bool EndsWithEx(this string source, string target0, string target1) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, target0, target1);

        /// <summary>
        ///     Determines whether the end of this string matches one of the specified
        ///     strings. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is
        ///     used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The first string to compare.
        /// </param>
        /// <param name="target1">
        ///     The second string to compare.
        /// </param>
        /// <param name="target2">
        ///     The third string to compare.
        /// </param>
        public static bool EndsWithEx(this string source, string target0, string target1, string target2) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, target0, target1, target2);

        /// <summary>
        ///     Determines whether the end of this string matches one of the specified
        ///     strings. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is
        ///     used for this comparison.
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
        ///     Determines whether the end of this string matches one of the specified
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
        public static bool EndsWithEx(this string source, StringComparison comparisonType, params char[] targets)
        {
            if (string.IsNullOrEmpty(source) || targets == null)
                return false;
            for (var i = 0; i < targets.Length; i++)
            {
                if (!source.EndsWith(targets[i].ToStringDefault(), comparisonType))
                    continue;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether the end of this string matches the specified character.
        ///     The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for
        ///     this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The character to compare.
        /// </param>
        public static bool EndsWithEx(this string source, char target0) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, target0);

        /// <summary>
        ///     Determines whether the end of this string matches one of the specified
        ///     characters. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter
        ///     is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The first character to compare.
        /// </param>
        /// <param name="target1">
        ///     The second character to compare.
        /// </param>
        public static bool EndsWithEx(this string source, char target0, char target1) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, target0, target1);

        /// <summary>
        ///     Determines whether the end of this string matches one of the specified
        ///     characters. The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter
        ///     is used for this comparison.
        /// </summary>
        /// <param name="source">
        ///     The string to check.
        /// </param>
        /// <param name="target0">
        ///     The first character to compare.
        /// </param>
        /// <param name="target1">
        ///     The second character to compare.
        /// </param>
        /// <param name="target2">
        ///     The third character to compare.
        /// </param>
        public static bool EndsWithEx(this string source, char target0, char target1, char target2) =>
            source.EndsWithEx(StringComparison.OrdinalIgnoreCase, target0, target1, target2);

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

        #endregion

        #region EqualsEx

        /// <summary>
        ///     Determines whether this string is the same as one of the specified strings.
        ///     A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="comparisonType">
        ///     The comparison type that specifies the culture, case, and sort rules.
        /// </param>
        /// <param name="target0">
        ///     The string to compare with the first string.
        /// </param>
        public static bool EqualsEx(this string source, StringComparison comparisonType, string target0)
        {
            if (source == null)
                return target0 == null;
            return string.IsNullOrEmpty(source) ? string.IsNullOrEmpty(target0) : string.Equals(source, target0, comparisonType);
        }

        /// <summary>
        ///     Determines whether this string is the same as one of the specified strings.
        ///     A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="comparisonType">
        ///     The comparison type that specifies the culture, case, and sort rules.
        /// </param>
        /// <param name="target0">
        ///     One of strings to compare with the first string.
        /// </param>
        /// <param name="target1">
        ///     One of strings to compare with the first string.
        /// </param>
        public static bool EqualsEx(this string source, StringComparison comparisonType, string target0, string target1)
        {
            if (source == null)
                return target0 == null ||
                       target1 == null;
            if (string.IsNullOrEmpty(source))
                return string.IsNullOrEmpty(target0) ||
                       string.IsNullOrEmpty(target1);
            return string.Equals(source, target0, comparisonType) ||
                   string.Equals(source, target1, comparisonType);
        }

        /// <summary>
        ///     Determines whether this string is the same as one of the specified strings.
        ///     A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="comparisonType">
        ///     The comparison type that specifies the culture, case, and sort rules.
        /// </param>
        /// <param name="target0">
        ///     One of strings to compare with the first string.
        /// </param>
        /// <param name="target1">
        ///     One of strings to compare with the first string.
        /// </param>
        /// <param name="target2">
        ///     One of strings to compare with the first string.
        /// </param>
        public static bool EqualsEx(this string source, StringComparison comparisonType, string target0, string target1, string target2)
        {
            if (source == null)
                return target0 == null ||
                       target1 == null ||
                       target2 == null;
            if (string.IsNullOrEmpty(source))
                return string.IsNullOrEmpty(target0) ||
                       string.IsNullOrEmpty(target1) ||
                       string.IsNullOrEmpty(target2);
            return string.Equals(source, target0, comparisonType) ||
                   string.Equals(source, target1, comparisonType) ||
                   string.Equals(source, target2, comparisonType);
        }

        /// <summary>
        ///     Determines whether this string is the same as one of the specified strings.
        ///     A parameter specifies the culture, case, and sort rules used in the
        ///     comparison.
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
            if (source == null)
                return targets?.Any(s => s == null) ?? true;
            for (var i = 0; i < targets.Length; i++)
            {
                if (!string.Equals(source, targets[i], comparisonType))
                    continue;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether this string is the same as the specified string. The
        ///     <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for this
        ///     comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="target0">
        ///     The string to compare with the first string.
        /// </param>
        public static bool EqualsEx(this string source, string target0) =>
            source.EqualsEx(StringComparison.OrdinalIgnoreCase, target0);

        /// <summary>
        ///     Determines whether this string is the same as one of the specified strings.
        ///     The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for
        ///     this comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="target0">
        ///     One of strings to compare with the first string.
        /// </param>
        /// <param name="target1">
        ///     One of strings to compare with the first string.
        /// </param>
        public static bool EqualsEx(this string source, string target0, string target1) =>
            source.EqualsEx(StringComparison.OrdinalIgnoreCase, target0, target1);

        /// <summary>
        ///     Determines whether this string is the same as one of the specified strings.
        ///     The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for
        ///     this comparison.
        /// </summary>
        /// <param name="source">
        ///     The first string to compare.
        /// </param>
        /// <param name="target0">
        ///     One of strings to compare with the first string.
        /// </param>
        /// <param name="target1">
        ///     One of strings to compare with the first string.
        /// </param>
        /// <param name="target2">
        ///     One of strings to compare with the first string.
        /// </param>
        public static bool EqualsEx(this string source, string target0, string target1, string target2) =>
            source.EqualsEx(StringComparison.OrdinalIgnoreCase, target0, target1, target2);

        /// <summary>
        ///     Determines whether this string is the same as one of the specified strings.
        ///     The <see cref="StringComparison.OrdinalIgnoreCase"/> parameter is used for
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

        #endregion

        #region SequenceStartsWith

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches the
        ///     specified byte.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     The byte to compare.
        /// </param>
        public static bool SequenceStartsWith(this IEnumerable<byte> source, byte target0) =>
            source?.First() == target0;

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches the
        ///     specified sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     The first byte to compare.
        /// </param>
        /// <param name="target1">
        ///     The second byte to compare.
        /// </param>
        public static bool SequenceStartsWith(this IEnumerable<byte> source, byte target0, byte target1)
        {
            if (source == null)
                return false;
            var ba = source.Take(2).ToArray();
            return ba.Length == 2 && ba[0] == target0 && ba[1] == target1;
        }

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches the
        ///     specified sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     The first byte to compare.
        /// </param>
        /// <param name="target1">
        ///     The second byte to compare.
        /// </param>
        /// <param name="target2">
        ///     The third byte to compare.
        /// </param>
        public static bool SequenceStartsWith(this IEnumerable<byte> source, byte target0, byte target1, byte target2)
        {
            if (source == null)
                return false;
            var ba = source.Take(3).ToArray();
            return ba.Length == 3 && ba[0] == target0 && ba[1] == target1 && ba[2] == target2;
        }

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches the
        ///     specified sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target">
        ///     The sequence of bytes to compare with the first.
        /// </param>
        public static bool SequenceStartsWith(this IEnumerable<byte> source, params byte[] target)
        {
            if (source == null || target == null)
                return false;
            var ba = source.AsArray();
            if (ba.Length < target.Length)
                return false;
            for (var i = 0; i < target.Length; i++)
            {
                if (ba[i] == target[i])
                    continue;
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches the
        ///     specified sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target">
        ///     The sequence of bytes to compare with the first.
        /// </param>
        public static bool SequenceStartsWith(this IEnumerable<byte> source, IEnumerable<byte> target) =>
            source?.SequenceStartsWith(target?.AsArray()) ?? false;

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches one of
        ///     the specified sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        /// <param name="target1">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        public static bool SequenceStartsWith(this IEnumerable<byte> source, IEnumerable<byte> target0, IEnumerable<byte> target1)
        {
            if (source == null || target0 == null || target1 == null)
                return false;
            var bytes = source.AsArray();
            return bytes.SequenceStartsWith(target0) || bytes.SequenceStartsWith(target1);
        }

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches one of
        ///     the specified sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        /// <param name="target1">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        /// <param name="target2">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        public static bool SequenceStartsWith(this IEnumerable<byte> source, IEnumerable<byte> target0, IEnumerable<byte> target1, IEnumerable<byte> target2)
        {
            if (source == null || target0 == null || target1 == null || target2 == null)
                return false;
            var bytes = source.AsArray();
            return bytes.SequenceStartsWith(target0) || bytes.SequenceStartsWith(target1) || bytes.SequenceStartsWith(target2);
        }

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches one of
        ///     the specified sequences of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="targets">
        ///     The sequences of bytes to compare.
        /// </param>
        public static bool SequenceStartsWith(this IEnumerable<byte> source, params IEnumerable<byte>[] targets)
        {
            if (source == null || targets == null)
                return false;
            var result = false;
            var bytes = source.AsArray();
            for (var i = 0; i < targets.Length; i++)
            {
                var target = targets[i]?.AsArray();
                if (target == null || bytes.Length < target.Length)
                    continue;
                var equal = true;
                for (var j = 0; j < target.Length; j++)
                {
                    if (bytes[j] == target[j])
                        continue;
                    equal = false;
                    break;
                }
                if (!equal)
                    continue;
                result = true;
                break;
            }
            return result;
        }

        #endregion

        #region SequenceEndsWith

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches the
        ///     specified byte.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     The byte to compare.
        /// </param>
        public static bool SequenceEndsWith(this IEnumerable<byte> source, byte target0) =>
            source?.Last() == target0;

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches the
        ///     specified sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     The first byte to compare.
        /// </param>
        /// <param name="target1">
        ///     The second byte to compare.
        /// </param>
        public static bool SequenceEndsWith(this IEnumerable<byte> source, byte target0, byte target1)
        {
            if (source == null)
                return false;
            var ba = source.TakeLast(2).ToArray();
            return ba.Length == 2 && ba[0] == target0 && ba[1] == target1;
        }

        /// <summary>
        ///     Determines whether the beginning of this sequence of bytes matches the
        ///     specified sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     The first byte to compare.
        /// </param>
        /// <param name="target1">
        ///     The second byte to compare.
        /// </param>
        /// <param name="target2">
        ///     The third byte to compare.
        /// </param>
        public static bool SequenceEndsWith(this IEnumerable<byte> source, byte target0, byte target1, byte target2)
        {
            if (source == null)
                return false;
            var ba = source.TakeLast(3).ToArray();
            return ba.Length == 3 && ba[0] == target0 && ba[1] == target1 && ba[2] == target2;
        }

        /// <summary>
        ///     Determines whether the end of this sequence of bytes matches the specified
        ///     sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target">
        ///     The sequence of bytes to compare with the first.
        /// </param>
        public static bool SequenceEndsWith(this IEnumerable<byte> source, params byte[] target) =>
            source?.SequenceEndsWith((IEnumerable<byte>)target) ?? false;

        /// <summary>
        ///     Determines whether the end of this sequence of bytes matches the specified
        ///     sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target">
        ///     The sequence of bytes to compare with the first.
        /// </param>
        public static bool SequenceEndsWith(this IEnumerable<byte> source, IEnumerable<byte> target)
        {
            if (source == null || target == null)
                return false;
            var ba1 = source.AsArray();
            var ba2 = target.AsArray();
            var len1 = ba1.Length;
            var len2 = ba2.Length;
            if (len1 < len2)
                return false;
            for (var j = 1; j < len2 + 1; j++)
            {
                if (ba1[len1 - j] == ba2[len2 - j])
                    continue;
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Determines whether the end of this sequence of bytes matches one of the
        ///     specified sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        /// <param name="target1">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        public static bool SequenceEndsWith(this IEnumerable<byte> source, IEnumerable<byte> target0, IEnumerable<byte> target1)
        {
            if (source == null || target0 == null || target1 == null)
                return false;
            var bytes = source.AsArray();
            return bytes.SequenceEndsWith(target0) || bytes.SequenceEndsWith(target1);
        }

        /// <summary>
        ///     Determines whether the end of this sequence of bytes matches one of the
        ///     specified sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        /// <param name="target1">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        /// <param name="target2">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        public static bool SequenceEndsWith(this IEnumerable<byte> source, IEnumerable<byte> target0, IEnumerable<byte> target1, IEnumerable<byte> target2)
        {
            if (source == null || target0 == null || target1 == null || target2 == null)
                return false;
            var bytes = source.AsArray();
            return bytes.SequenceEndsWith(target0) || bytes.SequenceEndsWith(target1) || bytes.SequenceEndsWith(target2);
        }

        /// <summary>
        ///     Determines whether the end of this sequence of bytes matches one of the
        ///     specified sequences of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="targets">
        ///     The sequences of bytes to compare.
        /// </param>
        public static bool SequenceEndsWith(this IEnumerable<byte> source, params IEnumerable<byte>[] targets)
        {
            if (source == null || targets == null)
                return false;
            var result = false;
            var ba1 = source.AsArray();
            var len1 = ba1.Length;
            for (var i = 0; i < targets.Length; i++)
            {
                var ba2 = targets[i]?.AsArray();
                if (ba2 == null)
                    continue;
                var len2 = ba2.Length;
                if (len1 < len2)
                    continue;
                var equal = true;
                for (var j = 1; j < len2 + 1; j++)
                {
                    if (ba1[len1 - j] == ba2[len2 - j])
                        continue;
                    equal = false;
                    break;
                }
                if (!equal)
                    continue;
                result = true;
                break;
            }
            return result;
        }

        #endregion

        #region SequenceEqualEx

        /// <summary>
        ///     Determines whether this sequence of bytes is the same as the specified
        ///     sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     The sequence of bytes to compare with the first.
        /// </param>
        public static bool SequenceEqualEx(this IEnumerable<byte> source, IEnumerable<byte> target0)
        {
            if (source == null)
                return target0 == null;
            if (target0 == null)
                return false;
            var bytes = source.AsArray();
            var target = target0.AsArray();
            if (bytes.Length != target.Length)
                return false;
            for (var i = 0; i < target.Length; i++)
            {
                if (bytes[i] == target[i])
                    continue;
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Determines whether this sequence of bytes is the same as one of the
        ///     specified sequences of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        /// <param name="target1">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        public static bool SequenceEqualEx(this IEnumerable<byte> source, IEnumerable<byte> target0, IEnumerable<byte> target1)
        {
            if (source == null)
                return target0 == null || target1 == null;
            if (target0 == null && target1 == null)
                return false;
            var bytes = source.AsArray();
            return bytes.SequenceEqualEx(target0) || bytes.SequenceEqualEx(target1);
        }

        /// <summary>
        ///     Determines whether this sequence of bytes is the same as one of the
        ///     specified sequences of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="target0">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        /// <param name="target1">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        /// <param name="target2">
        ///     One of the sequences of bytes to compare with the first.
        /// </param>
        public static bool SequenceEqualEx(this IEnumerable<byte> source, IEnumerable<byte> target0, IEnumerable<byte> target1, IEnumerable<byte> target2)
        {
            if (source == null)
                return target0 == null || target1 == null;
            if (target0 == null && target1 == null)
                return false;
            var bytes = source.AsArray();
            return bytes.SequenceEqualEx(target0) || bytes.SequenceEqualEx(target1) || bytes.SequenceEqualEx(target2);
        }

        /// <summary>
        ///     Determines whether this sequence of bytes is the same as one of the
        ///     specified sequences of bytes.
        /// </summary>
        /// <param name="source">
        ///     The first sequence of bytes to compare.
        /// </param>
        /// <param name="targets">
        ///     The sequences of bytes to compare with the first.
        /// </param>
        public static bool SequenceEqualEx(this IEnumerable<byte> source, params IEnumerable<byte>[] targets)
        {
            if (source == null)
                return targets?.Any(ba => ba == null) ?? true;
            if (targets == null)
                return false;
            var result = false;
            var bytes = source.AsArray();
            for (var i = 0; i < targets.Length; i++)
            {
                var target = targets[i]?.AsArray();
                if (target == null || bytes.Length != target.Length)
                    continue;
                var equal = true;
                for (var j = 0; j < target.Length; j++)
                {
                    if (bytes[j] == target[j])
                        continue;
                    equal = false;
                    break;
                }
                if (!equal)
                    continue;
                result = true;
                break;
            }
            return result;
        }

        /// <summary>
        ///     Determines whether two sequences are equal by comparing their elements by
        ///     using a specified <see cref="IEqualityComparer{T}"/>.
        ///     <para>
        ///         Please note that this method does the same as
        ///         <see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource})"/>
        ///         , except that parameters set to <see langword="null"/> will also
        ///         compare instead of throwing an exception.
        ///     </para>
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of the input sequences.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to compare to second.
        /// </param>
        /// <param name="target">
        ///     An <see cref="IEnumerable{T}"/> to compare to the first sequence.
        /// </param>
        /// <param name="comparer">
        ///     An <see cref="IEqualityComparer{T}"/> to use to compare elements.
        /// </param>
        public static bool SequenceEqualEx<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> target, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                return target == null;
            if (target == null)
                return false;
            comparer ??= EqualityComparer<TSource>.Default;
            using var enumerator1 = source.GetEnumerator();
            using var enumerator2 = target.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                if (enumerator2.MoveNext() && comparer.Equals(enumerator1.Current, enumerator2.Current))
                    continue;
                return false;
            }
            return !enumerator2.MoveNext();
        }

        /// <summary>
        ///     Determines whether two sequences are equal by comparing their elements by
        ///     using the default equality comparer for their type.
        ///     <para>
        ///         Please note that this method does the same as
        ///         <see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>
        ///         , except that parameters set to <see langword="null"/> will also
        ///         compare instead of throwing an exception.
        ///     </para>
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the elements of the input sequences.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to compare to second.
        /// </param>
        /// <param name="target">
        ///     An <see cref="IEnumerable{T}"/> to compare to the first sequence.
        /// </param>
        public static bool SequenceEqualEx<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> target) =>
            source.SequenceEqualEx(target, null);

        #endregion

        #region Diff

        /// <summary>
        ///     Retrieves a sequence with differences between two sequences where index,
        ///     source and target elements are enumerated.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the elements of the source and target sequences.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to compare to second.
        /// </param>
        /// <param name="target">
        ///     An <see cref="IEnumerable{T}"/> to compare to the first sequence.
        /// </param>
        /// <param name="comparer">
        ///     An <see cref="IEqualityComparer{T}"/> to use to compare elements.
        /// </param>
        public static IEnumerable<(int, TElement, TElement)> Diff<TElement>(this IEnumerable<TElement> source, IEnumerable<TElement> target, IEqualityComparer<TElement> comparer)
        {
            var index = -1;
            comparer ??= EqualityComparer<TElement>.Default;
            using var enumerator1 = source.GetEnumerator();
            using var enumerator2 = target.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                index++;
                var current1 = enumerator1.Current;
                if (enumerator2.MoveNext())
                {
                    var current2 = enumerator2.Current;
                    if (!comparer.Equals(current1, current2))
                        yield return (index, current1, current2);
                    continue;
                }
                yield return (index, current1, default);
            }
            while (enumerator2.MoveNext())
            {
                index++;
                yield return (index, default, enumerator2.Current);
            }
        }

        /// <summary>
        ///     Retrieves a sequence with differences between two sequences where index,
        ///     source and target elements are enumerated.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the elements of the source and target sequences.
        /// </typeparam>
        /// <param name="source">
        ///     An <see cref="IEnumerable{T}"/> to compare to second.
        /// </param>
        /// <param name="target">
        ///     An <see cref="IEnumerable{T}"/> to compare to the first sequence.
        /// </param>
        public static IEnumerable<(int, TElement, TElement)> Diff<TElement>(this IEnumerable<TElement> source, IEnumerable<TElement> target) =>
            source.Diff(target, null);

        #endregion
    }
}
