#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: StringEx.cs
// Version:  2023-12-20 12:04
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringEx
    {
        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified object using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="arg0">
        ///     The first object to format.
        /// </param>
        public static string FormatCurrent(this string format, object arg0) =>
            string.Format(CultureInfo.CurrentCulture, format, arg0);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified objects using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="arg0">
        ///     The first object to format.
        /// </param>
        /// <param name="arg1">
        ///     The second object to format.
        /// </param>
        public static string FormatCurrent(this string format, object arg0, object arg1) =>
            string.Format(CultureInfo.CurrentCulture, format, arg0, arg1);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified objects using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="arg0">
        ///     The first object to format.
        /// </param>
        /// <param name="arg1">
        ///     The second object to format.
        /// </param>
        /// <param name="arg2">
        ///     The third object to format.
        /// </param>
        /// <returns>
        /// </returns>
        public static string FormatCurrent(this string format, object arg0, object arg1, object arg2) =>
            string.Format(CultureInfo.CurrentCulture, format, arg0, arg1, arg2);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified objects using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="args">
        ///     The objects to format.
        /// </param>
        public static string FormatCurrent(this string format, params object[] args) =>
            string.Format(CultureInfo.CurrentCulture, format, args);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified object using the
        ///     <see cref="CultureInfo.InvariantCulture"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="arg0">
        ///     The first object to format.
        /// </param>
        public static string FormatInvariant(this string format, object arg0) =>
            string.Format(CultureInfo.InvariantCulture, format, arg0);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified objects using the
        ///     <see cref="CultureInfo.InvariantCulture"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="arg0">
        ///     The first object to format.
        /// </param>
        /// <param name="arg1">
        ///     The second object to format.
        /// </param>
        public static string FormatInvariant(this string format, object arg0, object arg1) =>
            string.Format(CultureInfo.InvariantCulture, format, arg0, arg1);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified objects using the
        ///     <see cref="CultureInfo.InvariantCulture"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="arg0">
        ///     The first object to format.
        /// </param>
        /// <param name="arg1">
        ///     The second object to format.
        /// </param>
        /// <param name="arg2">
        ///     The third object to format.
        /// </param>
        /// <returns>
        /// </returns>
        public static string FormatInvariant(this string format, object arg0, object arg1, object arg2) =>
            string.Format(CultureInfo.InvariantCulture, format, arg0, arg1, arg2);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified objects using the
        ///     <see cref="CultureInfo.InvariantCulture"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="args">
        ///     The objects to format.
        /// </param>
        public static string FormatInvariant(this string format, params object[] args) =>
            string.Format(CultureInfo.InvariantCulture, format, args);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified object using the
        ///     <see cref="CultureConfig.GlobalCultureInfo"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="arg0">
        ///     The first object to format.
        /// </param>
        public static string FormatDefault(this string format, object arg0) =>
            string.Format(CultureConfig.GlobalCultureInfo, format, arg0);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified objects using the
        ///     <see cref="CultureConfig.GlobalCultureInfo"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="arg0">
        ///     The first object to format.
        /// </param>
        /// <param name="arg1">
        ///     The second object to format.
        /// </param>
        public static string FormatDefault(this string format, object arg0, object arg1) =>
            string.Format(CultureConfig.GlobalCultureInfo, format, arg0, arg1);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified objects using the
        ///     <see cref="CultureConfig.GlobalCultureInfo"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="arg0">
        ///     The first object to format.
        /// </param>
        /// <param name="arg1">
        ///     The second object to format.
        /// </param>
        /// <param name="arg2">
        ///     The third object to format.
        /// </param>
        /// <returns>
        /// </returns>
        public static string FormatDefault(this string format, object arg0, object arg1, object arg2) =>
            string.Format(CultureConfig.GlobalCultureInfo, format, arg0, arg1, arg2);

        /// <summary>
        ///     Replaces one or more format items in this string with the string
        ///     representation of a specified objects using the
        ///     <see cref="CultureConfig.GlobalCultureInfo"/> format information.
        /// </summary>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="args">
        ///     The objects to format.
        /// </param>
        public static string FormatDefault(this string format, params object[] args) =>
            string.Format(CultureConfig.GlobalCultureInfo, format, args);

        /// <summary>
        ///     Returns a new string in which all occurrences of a specified string in the
        ///     current instance are replaced with another specified string.
        /// </summary>
        /// <param name="str">
        /// </param>
        /// The string to change.
        /// <param name="oldValue">
        ///     The string to be replaced.
        /// </param>
        /// <param name="newValue">
        ///     The string to replace all occurrences of oldValue.
        /// </param>
        /// <param name="comparisonType">
        ///     One of the enumeration values that specifies the rules for the search.
        /// </param>
        public static string Replace(this string str, string oldValue, string newValue, StringComparison comparisonType)
        {
            if (str == null)
                return null;
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(oldValue))
                return str;
            newValue ??= string.Empty;
            string s;
            var q = new Queue<string>();
            q.Enqueue(str);
            do
            {
                s = q.Dequeue();
                var i = s.IndexOf(oldValue, comparisonType);
                if (i < 0)
                {
                    q.Clear();
                    break;
                }
                s = s.Remove(i, oldValue.Length);
                s = s.Insert(i, newValue);
                q.Enqueue(s);
            }
            while (q.Count > 0);
            return s;
        }

        /// <summary>
        ///     Reduce all white space characters in a string.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        public static string ReduceWhiteSpace(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str?.Trim();
            var sb = new StringBuilder();
            var isWhiteSpace = false;
            var startFound = false;
            foreach (var c in str)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (isWhiteSpace)
                        continue;
                    isWhiteSpace = true;
                }
                else
                {
                    isWhiteSpace = false;
                    startFound = true;
                }
                if (!startFound)
                    continue;
                sb.Append(c);
            }
            if (isWhiteSpace)
                sb.Length--;
            return sb.ToStringThenClear();
        }

        /// <summary>
        ///     Reverses the sequence of all characters in a string.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        public static string Reverse(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            var ca = str.ToCharArray();
            Array.Reverse(ca);
            return new string(ca);
        }

        /// <summary>
        ///     Trim the string logical on its maximum size.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        /// <param name="font">
        ///     The font that is used to measure.
        /// </param>
        /// <param name="width">
        ///     The maximum width in pixel.
        /// </param>
        public static string Trim(this string str, Font font, int width)
        {
            var s = default(string);
            try
            {
                const string suffix = "...";
                s = str ?? throw new ArgumentNullException(nameof(str));
                using var g = Graphics.FromHwnd(IntPtr.Zero);
                var x = Math.Floor(g.MeasureString(suffix, font).Width);
                var c = g.MeasureString(s, font).Width;
                var r = width / c;
                while (r < 1.0)
                {
                    s = string.Concat(s.Substring(0, (int)Math.Floor(s.Length * r - x)), suffix);
                    c = g.MeasureString(s, font).Width;
                    r = width / c;
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return s;
        }

        /// <summary>
        ///     Creates a sequence of strings based on natural (base e) logarithm of a
        ///     count of all the characters in the specified string.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        public static string[] ToStrings(this string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length < 8)
                return new[] { str };
            var i = 0;
            var b = Math.Floor(Math.Log(str.Length));
            return str.ToLookup(_ => Math.Floor(i++ / b)).Select(e => new string(e.ToArray())).ToArray();
        }

        /// <summary>
        ///     Sorts the elements in an entire string array using the
        ///     <see cref="IComparable{T}"/> generic interface implementation of each
        ///     element of the string array.
        /// </summary>
        /// <param name="strs">
        ///     The sequence of strings to sort.
        /// </param>
        public static string[] Sort(this string[] strs)
        {
            if (strs == null || strs.All(string.IsNullOrWhiteSpace))
                return strs;
            var sa = strs;
            Array.Sort(sa);
            return sa;
        }

        /// <summary>
        ///     Splits a string into substrings based on the strings in an array. You can
        ///     specify whether the substrings include empty array elements.
        /// </summary>
        /// <param name="str">
        ///     The string to split.
        /// </param>
        /// <param name="separator">
        ///     The string to use as a separator.
        /// </param>
        /// <param name="splitOptions">
        ///     The split options.
        /// </param>
        public static string[] Split(this string str, string separator = TextSeparatorString.WindowsDefault, StringSplitOptions splitOptions = StringSplitOptions.None) =>
            string.IsNullOrEmpty(str) ? null : str.Split(new[] { separator }, splitOptions);

        /// <summary>
        ///     Splits a string into substrings based on <see cref="Environment.NewLine"/>.
        /// </summary>
        /// <param name="str">
        ///     The string to split.
        /// </param>
        /// <param name="splitOptions">
        ///     The split options.
        /// </param>
        /// <param name="trim">
        ///     <see langword="true"/> to trim each line; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static string[] SplitNewLine(this string str, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries, bool trim = false)
        {
            if (!str.Any(TextEx.IsLineSeparator))
                return new[] { str };
            var s = TextEx.FormatNewLine(str);
            var sa = s.Split(Environment.NewLine, splitOptions);
            if (!trim)
                return sa;
            var ie = sa.Select(x => x.Trim());
            if (splitOptions == StringSplitOptions.RemoveEmptyEntries)
                ie = ie.Where(Comparison.IsNotEmpty);
            sa = ie.ToArray();
            return sa;
        }

        /// <summary>
        ///     Splits a string into substrings based on <see cref="Environment.NewLine"/>.
        /// </summary>
        /// <param name="str">
        ///     The string to split.
        /// </param>
        /// <param name="trim">
        ///     <see langword="true"/> to trim each line; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static string[] SplitNewLine(this string str, bool trim) =>
            str.SplitNewLine(StringSplitOptions.RemoveEmptyEntries, trim);

        /// <summary>
        ///     Converts the specified strings in a string to lowercase.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        /// <param name="strs">
        ///     The sequence of strings to convert.
        /// </param>
        public static string LowerText(this string str, params string[] strs)
        {
            if (string.IsNullOrWhiteSpace(str) || strs == null || strs.All(string.IsNullOrWhiteSpace))
                return str;
            return strs.Aggregate(str, (c, x) => Regex.Replace(c, x, x.ToLowerInvariant(), RegexOptions.IgnoreCase));
        }

        /// <summary>
        ///     Converts the specified strings in a string to uppercase.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        /// <param name="strs">
        ///     The sequence of strings to convert.
        /// </param>
        public static string UpperText(this string str, params string[] strs)
        {
            if (string.IsNullOrWhiteSpace(str) || strs == null || strs.All(string.IsNullOrWhiteSpace))
                return str;
            return strs.Aggregate(str, (c, x) => Regex.Replace(c, x, x.ToUpperInvariant(), RegexOptions.IgnoreCase));
        }

        /// <summary>
        ///     Removes the specified characters in a string.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        /// <param name="chrs">
        ///     The sequence of characters to remove.
        /// </param>
        public static string RemoveChar(this string str, params char[] chrs)
        {
            if (string.IsNullOrEmpty(str) || chrs == null)
                return str;
            return new string(str.Where(c => !chrs.Contains(c)).ToArray());
        }

        /// <summary>
        ///     Removes the specified strings in a string.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        /// <param name="strs">
        ///     The sequence of strings to remove.
        /// </param>
        public static string RemoveText(this string str, params string[] strs)
        {
            if (string.IsNullOrEmpty(str) || strs == null || strs.All(string.IsNullOrEmpty))
                return str;
            return strs.Aggregate(str, (c, x) => c.Replace(x, string.Empty));
        }

        /// <summary>
        ///     Removes the specified strings in a string.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        /// <param name="patterns">
        ///     The sequence of regular expression patterns to match.
        /// </param>
        public static string RemoveTextIgnoreCase(this string str, params string[] patterns)
        {
            if (string.IsNullOrEmpty(str) || patterns == null || patterns.All(string.IsNullOrEmpty))
                return str;
            var p = patterns.Distinct().Join('|');
            return new Regex(p, RegexOptions.IgnoreCase).Replace(str, string.Empty);
        }
    }
}
