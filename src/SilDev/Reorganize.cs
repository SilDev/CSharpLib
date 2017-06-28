#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Reorganize.cs
// Version:  2017-06-28 08:51
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Provides static methods for converting or reorganizing of data.
    /// </summary>
    public static class Reorganize
    {
        /// <summary>
        ///     Provides units of digital information.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum SizeUnits
        {
            /// <summary>
            ///     Stands for byte.
            /// </summary>
            Byte = 0,

            /// <summary>
            ///     Stands for kilobyte or kibibyte.
            /// </summary>
            KB = 1,

            /// <summary>
            ///     Stands for megabyte or mebibyte.
            /// </summary>
            MB = 2,

            /// <summary>
            ///     Stands for gigabyte or gibibyte.
            /// </summary>
            GB = 3,

            /// <summary>
            ///     Stands for terabyte or tebibyte.
            /// </summary>
            TB = 4,

            /// <summary>
            ///     Stands for petabyte or pebibyte.
            /// </summary>
            PB = 5,

            /// <summary>
            ///     Stands for exabyte or exbibyte.
            /// </summary>
            EB = 6
        }

        /// <summary>
        ///     Serializes this object graph into a sequence of bytes.
        /// </summary>
        /// <param name="value">
        ///     The object graph to convert.
        /// </param>
        public static byte[] SerializeObject<T>(this T value)
        {
            try
            {
                byte[] ba;
                using (var ms = new MemoryStream())
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(ms, value);
                    ba = ms.ToArray();
                }
                return ba;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Deserializes this sequence of bytes into an object graph.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        /// <param name="defValue">
        ///     The default value.
        /// </param>
        public static T DeserializeObject<T>(this byte[] bytes, T defValue = default(T))
        {
            try
            {
                object obj;
                using (var ms = new MemoryStream())
                {
                    var bf = new BinaryFormatter();
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    obj = bf.Deserialize(ms);
                }
                return (T)obj;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return defValue;
            }
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
        public static IEnumerable<TSource> RecursiveSelect<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> selector, long timelimit = 60000)
        {
            var stack = new Stack<IEnumerator<TSource>>();
            var enumerator = source.GetEnumerator();
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (true)
                    if (stopwatch.ElapsedMilliseconds < timelimit && enumerator?.MoveNext() == true)
                    {
                        var element = enumerator.Current;
                        yield return element;
                        stack.Push(enumerator);
                        enumerator = selector(element)?.GetEnumerator();
                    }
                    else if (stack.Count > 0)
                    {
                        enumerator?.Dispose();
                        enumerator = stack.Pop();
                    }
                    else
                        yield break;
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
        ///     Converts this numeric value into a string that represents the number expressed as a size
        ///     value in the specified <see cref="SizeUnits"/>.
        /// </summary>
        /// <param name="value">
        ///     The value to be converted.
        /// </param>
        /// <param name="unit">
        ///     The new unit.
        /// </param>
        /// <param name="binary">
        ///     true for the binary numeral system; otherwise, false for the decimal numeral system.
        /// </param>
        /// <param name="suffix">
        ///     true to show the size unit suffix; otherwise, false.
        /// </param>
        /// <param name="trim">
        ///     true to remove all zeros after the comma; otherwise, false.
        /// </param>
        public static string FormatDataSize(this long value, SizeUnits unit, bool binary = true, bool suffix = true, bool trim = false)
        {
            if (value < 0)
                return "-" + FormatDataSize(Math.Abs(value), unit, binary, suffix);
            if (value == 0)
                return value.ToString(trim ? "0.##" : "0.00") + " bytes";
            var d = value / Math.Pow(binary ? 1024 : 1000, (int)unit);
            var s = d.ToString(trim ? "0.##" : "0.00") + " " + unit;
            if (unit == 0 && !Math.Abs(d).Equals(1d))
                s += "s";
            return s;
        }

        /// <summary>
        ///     Converts this numeric value into a string that represents the number expressed as a size
        ///     value in bytes, kilobytes, megabytes, gigabytes, terabyte, petabyte, exabyte, depending
        ///     on the size.
        /// </summary>
        /// <param name="value">
        ///     The value to be converted.
        /// </param>
        /// <param name="binary">
        ///     true for the binary numeral system; otherwise, false for the decimal numeral system.
        /// </param>
        /// <param name="suffix">
        ///     true to show the size unit suffix; otherwise, false.
        /// </param>
        /// <param name="trim">
        ///     true to remove all zeros after the comma; otherwise, false.
        /// </param>
        public static string FormatDataSize(this long value, bool binary = true, bool suffix = true, bool trim = false)
        {
            if (value == 0)
                return value.FormatDataSize(SizeUnits.Byte, binary, suffix, trim);
            var i = (int)Math.Log(Math.Abs(value), binary ? 1024 : 1000);
            var s = value.FormatDataSize((SizeUnits)i, binary, suffix, trim);
            return s;
        }

        /// <summary>
        ///     Increments the length of a platform-specific type number with the specified value.
        /// </summary>
        /// <param name="ptr">
        ///     The platform-specific type to change.
        /// </param>
        /// <param name="value">
        ///     The number to be incremented.
        /// </param>
        public static IntPtr Increment(this IntPtr ptr, IntPtr value)
        {
            try
            {
                return new IntPtr(IntPtr.Size == sizeof(int) ? ptr.ToInt32() + (int)value : ptr.ToInt64() + (long)value);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return new IntPtr(IntPtr.Size == sizeof(int) ? int.MaxValue : long.MaxValue);
            }
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
            var s = new string(ca);
            return s;
        }

        /// <summary>
        ///     Creates a sequence of strings based on natural (base e) logarithm of a count
        ///     of all the characters in the specified string.
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
            return str.ToLookup(c => Math.Floor(i++ / b)).Select(e => new string(e.ToArray())).ToArray();
        }

        /// <summary>
        ///     Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type
        ///     <see cref="string"/>, using the specified separator between each number.
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
        ///     Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type
        ///     <see cref="string"/>, using the specified separator between each number.
        /// </summary>
        /// <param name="values">
        ///     An array that contains the elements to concatenate.
        /// </param>
        /// <param name="separator">
        ///     The character to use as a separator.
        /// </param>
        public static string Join(this IEnumerable<string> values, char separator) =>
            values.Join(separator.ToString());

        /// <summary>
        ///     Sorts the elements in an entire string array using the <see cref="IComparable{T}"/> generic
        ///     interface implementation of each element of the string array.
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
        ///     Splits a string into substrings based on the strings in an array. You can specify whether
        ///     the substrings inlcude empty array elements.
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
        public static string[] Split(this string str, string separator = TextEx.NewLineFormats.WindowsDefault, StringSplitOptions splitOptions = StringSplitOptions.None)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            var sa = str.Split(new[] { separator }, splitOptions);
            return sa;
        }

        /// <summary>
        ///     Splits a string into substrings based on <see cref="Environment.NewLine"/>.
        /// </summary>
        /// <param name="str">
        ///     The string to split.
        /// </param>
        public static string[] SplitNewLine(this string str) =>
            str.Split(Environment.NewLine);

        /// <summary>
        ///     Converts all the characters in the specified string into a sequence of bytes.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static byte[] ToBytes(this string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentNullException(nameof(str));
                var ba = Encoding.UTF8.GetBytes(str);
                return ba;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Converts the specified sequence of bytes into a string.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        public static string ToStringEx(this byte[] bytes)
        {
            try
            {
                if (bytes == null)
                    throw new ArgumentNullException(nameof(bytes));
                var s = Encoding.UTF8.GetString(bytes);
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified string, which stores a set of four integers that represent the
        ///     location and size of a rectangle, to <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static Rectangle ToRectangle(this string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentNullException(nameof(str));
                var s = str;
                if (s.StartsWith("{") && s.EndsWith("}"))
                    s = new string(s.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()).Replace(",", ";");
                var rc = new RectangleConverter();
                var obj = rc.ConvertFrom(s);
                if (obj == null)
                    throw new ArgumentNullException(nameof(s));
                var rect = (Rectangle)obj;
                return rect;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return Rectangle.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified string, which stores an ordered pair of integer x- and y-coordinates,
        ///     to <see cref="Point"/>.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static Point ToPoint(this string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentNullException(nameof(str));
                var s = str;
                if (s.StartsWith("{") && s.EndsWith("}"))
                    s = new string(s.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()).Replace(",", ";");
                var pc = new PointConverter();
                var obj = pc.ConvertFrom(s);
                if (obj == null)
                    throw new ArgumentNullException(nameof(obj));
                var point = (Point)obj;
                return point;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return new Point(int.MinValue, int.MinValue);
            }
        }

        /// <summary>
        ///     Converts the specified string, which stores an ordered pair of integers, to <see cref="Size"/>.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static Size ToSize(this string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentNullException(nameof(str));
                var s = str;
                if (s.StartsWith("{") && s.EndsWith("}"))
                    s = new string(s.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()).Replace(",", ";");
                var sc = new SizeConverter();
                var obj = sc.ConvertFrom(s);
                if (obj == null)
                    throw new ArgumentNullException(nameof(obj));
                var size = (Size)obj;
                return size;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return Size.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified string to an equivalent Boolean value, which returns always false
        ///     for unsupported string values.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static bool ToBoolean(this string str)
        {
            try
            {
                var b = bool.Parse(str);
                return b;
            }
            catch
            {
                return false;
            }
        }

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
            var s = strs.Aggregate(str, (c, x) => Regex.Replace(c, x, x.ToLower(), RegexOptions.IgnoreCase));
            return s;
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
            var s = strs.Aggregate(str, (c, x) => Regex.Replace(c, x, x.ToUpper(), RegexOptions.IgnoreCase));
            return s;
        }

        /// <summary>
        ///     Removes the specified characters in a char array.
        /// </summary>
        /// <param name="array">
        ///     The char array to change.
        /// </param>
        /// <param name="chrs">
        ///     The sequence of characters to remove.
        /// </param>
        public static char[] RemoveChar(this char[] array, params char[] chrs)
        {
            if (array == null || chrs == null)
                return array;
            var ca = array.Where(c => !chrs.Contains(c)).ToArray();
            return ca;
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
            var s = new string(str.Where(c => !chrs.Contains(c)).ToArray());
            return s;
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
            var s = strs.Aggregate(str, (c, x) => c.Replace(x, string.Empty));
            return s;
        }

        /// <summary>
        ///     Converts the specified string to its string representation of a base-2 binary sequence.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        /// <param name="separator">
        ///     true to add a single space character as a separator between every single byte;
        ///     otherwise, false.
        /// </param>
        public static string ToBin(this string str, bool separator = true)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                    throw new ArgumentNullException(nameof(str));
                var ba = Encoding.UTF8.GetBytes(str);
                var s = separator ? " " : string.Empty;
                s = ba.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).Join(s);
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified base-2 binary sequence back to string.
        /// </summary>
        /// <param name="str">
        ///     The string to reconvert.
        /// </param>
        public static string ToStringFromBin(this string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentNullException(nameof(str));
                var s = str.RemoveChar(' ', ':', '\r', '\n');
                if (s.Any(c => !"01".Contains(c)))
                    throw new ArgumentOutOfRangeException(nameof(s));
                using (var ms = new MemoryStream())
                {
                    for (var i = 0; i < s.Length; i += 8)
                        ms.WriteByte(Convert.ToByte(s.Substring(i, 8), 2));
                    s = Encoding.UTF8.GetString(ms.ToArray());
                }
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified sequence of bytes into a hexadecimal sequence.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        /// <param name="separator">
        ///     A single space character used as a separator.
        /// </param>
        /// <param name="upper">
        ///     true to convert the result to uppercase; otherwise, false.
        /// </param>
        public static string ToHexa(this byte[] bytes, char? separator = null, bool upper = false)
        {
            try
            {
                if (bytes == null)
                    throw new ArgumentNullException(nameof(bytes));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2"));
                var s = sb.ToString();
                if (separator != null)
                {
                    var i = 0;
                    s = s.ToLookup(c => Math.Floor(i++ / 2d)).Select(e => new string(e.ToArray())).Join(separator.ToString());
                }
                if (upper)
                    s = s.ToUpper();
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified string to hexadecimal sequence.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        /// <param name="separator">
        ///     A single space character used as a separator.
        /// </param>
        /// <param name="upper">
        ///     true to convert the result to uppercase; otherwise, false.
        /// </param>
        public static string ToHexa(this string str, char? separator = null, bool upper = false) =>
            str.ToBytes().ToHexa(separator, upper);

        /// <summary>
        ///     Converts the specified hexadecimal sequence back into a sequence of bytes.
        /// </summary>
        /// <param name="str">
        ///     The string to reconvert.
        /// </param>
        public static byte[] ToBytesFormHexa(this string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                    throw new ArgumentNullException(nameof(str));
                var s = new string(str.Where(char.IsLetterOrDigit).ToArray()).ToUpper();
                var ba = Enumerable.Range(0, s.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(s.Substring(x, 2), 16)).ToArray();
                return ba;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Converts the specified hexadecimal sequence back to string.
        /// </summary>
        /// <param name="str">
        ///     The string to reconvert.
        /// </param>
        public static string ToStringFromHexa(this string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentNullException(nameof(str));
                var ba = str.ToBytesFormHexa();
                if (ba == null)
                    throw new ArgumentNullException(nameof(ba));
                return Encoding.UTF8.GetString(ba);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     Returns a new sequence of bytes in which all occurrences of a specified sequence of bytes
        ///     in this instance are replaced with another specified sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to change.
        /// </param>
        /// <param name="oldValue">
        ///     The sequence of bytes to be replaced.
        /// </param>
        /// <param name="newValue">
        ///     The sequence of bytes to replace all occurrences of oldValue.
        /// </param>
        public static byte[] Replace(this byte[] bytes, byte[] oldValue, byte[] newValue)
        {
            try
            {
                if (bytes == null || bytes.Length == 0)
                    throw new ArgumentNullException(nameof(bytes));
                if (oldValue == null || oldValue.Length == 0)
                    throw new ArgumentNullException(nameof(oldValue));
                byte[] ba;
                using (var ms = new MemoryStream())
                {
                    int i;
                    for (i = 0; i <= bytes.Length - oldValue.Length; i++)
                        if (!oldValue.Where((t, j) => bytes[i + j] != t).Any())
                        {
                            ms.Write(newValue, 0, newValue.Length);
                            i += oldValue.Length - 1;
                        }
                        else
                            ms.WriteByte(bytes[i]);
                    for (; i < bytes.Length; i++)
                        ms.WriteByte(bytes[i]);
                    ba = ms.ToArray();
                }
                return ba;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return bytes;
            }
        }

        /// <summary>
        ///     Converts the given <see cref="object"/> value to the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The value <see cref="Type"/>.
        /// </typeparam>
        /// <param name="value">
        ///     The value to convert.
        /// </param>
        /// <param name="defValue">
        ///     The default value.
        /// </param>
        public static T Parse<T>(this object value, T defValue = default(T))
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            var result = (T)converter.ConvertFrom(value);
            return Comparison.IsNotEmpty(result) ? result : defValue;
        }

        /// <summary>
        ///     Try to convert the given <see cref="object"/> value to the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The value <see cref="Type"/>.
        /// </typeparam>
        /// <param name="value">
        ///     The value to convert.
        /// </param>
        /// <param name="result">
        ///     The result value.
        /// </param>
        /// <param name="defValue">
        ///     The default value.
        /// </param>
        public static bool TryParse<T>(this object value, out dynamic result, T defValue = default(T))
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                result = (T)converter.ConvertFrom(value);
                return true;
            }
            catch
            {
                result = defValue;
                return false;
            }
        }
    }
}
