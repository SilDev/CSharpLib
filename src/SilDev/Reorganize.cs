#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Reorganize.cs
// Version:  2018-06-07 09:32
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Provides size format options.
    /// </summary>
    public enum SizeOptions
    {
        /// <summary>
        ///     Determines that the format is not changed.
        /// </summary>
        None,

        /// <summary>
        ///     Determines that all zeros are removed after the comma.
        /// </summary>
        Trim,

        /// <summary>
        ///     Determines that the value is rounded to the nearest integral value.
        /// </summary>
        Round
    }

    /// <summary>
    ///     Provides labels for size units.
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
    ///     Provides static methods for converting or reorganizing of data.
    /// </summary>
    public static class Reorganize
    {
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
        /// <param name="sizeOptions">
        /// </param>
        public static string FormatSize(this long value, SizeUnits unit, bool binary = true, bool suffix = true, SizeOptions sizeOptions = SizeOptions.None)
        {
            if (value < 0)
                return $"-{Math.Abs(value).FormatSize(unit, binary, suffix, sizeOptions)}";
            var f = sizeOptions != SizeOptions.None ? "0.##" : "0.00";
            if (value == 0)
                return $"{value.ToString(f)} bytes";
            var d = value / Math.Pow(binary ? 1024 : 1000, (int)unit);
            if (sizeOptions == SizeOptions.Round)
                d = Math.Round(d);
            var s = d.ToString(f);
            if (!suffix)
                return s;
            s = $"{s} {unit}";
            if (unit == 0 && !Math.Abs(d).Equals(1d))
                s += "s";
            return s;
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
        /// <param name="sizeOptions">
        /// </param>
        public static string FormatSize(this long value, SizeUnits unit, bool binary, SizeOptions sizeOptions) =>
            value.FormatSize(unit, binary, true, sizeOptions);

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
        /// <param name="sizeOptions">
        /// </param>
        public static string FormatSize(this long value, SizeUnits unit, SizeOptions sizeOptions) =>
            value.FormatSize(unit, true, true, sizeOptions);

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
        /// <param name="sizeOptions">
        /// </param>
        public static string FormatSize(this long value, bool binary = true, bool suffix = true, SizeOptions sizeOptions = SizeOptions.None)
        {
            if (value == 0)
                return value.FormatSize(SizeUnits.Byte, binary, suffix, sizeOptions);
            var i = (int)Math.Floor(Math.Log(Math.Abs(value), binary ? 1024 : 1000));
            var s = value.FormatSize((SizeUnits)i, binary, suffix, sizeOptions);
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
        /// <param name="sizeOptions">
        /// </param>
        public static string FormatSize(this long value, bool binary, SizeOptions sizeOptions) =>
            value.FormatSize(SizeUnits.Byte, binary, true, sizeOptions);

        /// <summary>
        ///     Converts this numeric value into a string that represents the number expressed as a size
        ///     value in bytes, kilobytes, megabytes, gigabytes, terabyte, petabyte, exabyte, depending
        ///     on the size.
        /// </summary>
        /// <param name="value">
        ///     The value to be converted.
        /// </param>
        /// <param name="sizeOptions">
        /// </param>
        public static string FormatSize(this long value, SizeOptions sizeOptions) =>
            value.FormatSize(true, true, sizeOptions);

        /// <summary>
        ///     Reads the bytes from the specified stream and writes them to another stream.
        /// </summary>
        /// <param name="src">
        ///     The <see cref="Stream"/> to copy.
        /// </param>
        /// <param name="dest">
        ///     The <see cref="Stream"/> to override.
        /// </param>
        /// <param name="buffer">
        ///     The maximum number of bytes to buffer.
        /// </param>
        public static void CopyTo(this Stream src, Stream dest, int buffer = 4096)
        {
            try
            {
                var ba = new byte[buffer];
                int i;
                while ((i = src.Read(ba, 0, ba.Length)) > 0)
                    dest.Write(ba, 0, i);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Serializes this object graph into a sequence of bytes.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source.
        /// </typeparam>
        /// <param name="src">
        ///     The object graph to convert.
        /// </param>
        public static byte[] SerializeObject<TSource>(this TSource src)
        {
            try
            {
                byte[] ba;
                using (var ms = new MemoryStream())
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(ms, src);
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
        /// <typeparam name="TResult">
        ///     The type of the result.
        /// </typeparam>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        /// <param name="defValue">
        ///     The default value.
        /// </param>
        public static TResult DeserializeObject<TResult>(this byte[] bytes, TResult defValue = default(TResult))
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
                return (TResult)obj;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return defValue;
            }
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
        ///     Returns a new string in which all occurrences of a specified string in the current
        ///     instance are replaced with another specified string.
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
            var s = str;
            Replace:
            var i = s.IndexOf(oldValue, comparisonType);
            if (i < 0)
                return s;
            s = s.Remove(i, oldValue.Length);
            s = s.Insert(i, newValue);
            goto Replace;
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
            var s = str;
            try
            {
                const string suffix = "...";
                using (var g = Graphics.FromHwnd(IntPtr.Zero))
                {
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
            }
            catch (ArgumentOutOfRangeException)
            {
                // ignored
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
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
        /// <param name="splitOptions">
        ///     The split options.
        /// </param>
        /// <param name="trim">
        ///     true to trim each line; otherwise, false.
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
        ///     true to trim each line; otherwise, false.
        /// </param>
        public static string[] SplitNewLine(this string str, bool trim) =>
            str.SplitNewLine(StringSplitOptions.RemoveEmptyEntries, trim);

        /// <summary>
        ///     Converts all the characters in the specified string into a sequence of bytes with the
        ///     specified <see cref="Encoding"/> format.
        /// </summary>
        /// <typeparam name="TEncoding">
        ///     The type of encoding.
        /// </typeparam>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        /// <param name="encoding">
        ///     The <see cref="Encoding"/> format.
        /// </param>
        public static byte[] ToBytes<TEncoding>(this string str, TEncoding encoding) where TEncoding : Encoding
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentNullException(nameof(str));
                return encoding.GetBytes(str);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Converts all the characters in the specified string into a sequence of bytes with the
        ///     Windows-1252 <see cref="Encoding"/> format.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static byte[] ToBytes(this string str) =>
            str.ToBytes(Encoding.GetEncoding(1252));

        /// <summary>
        ///     Converts all the characters in the specified string into a sequence of bytes with the
        ///     Windows-1252 <see cref="Encoding"/> format.
        ///     <para>
        ///         This method is equal to <see cref="ToBytes"/>.
        ///     </para>
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static byte[] ToBytesDefault(this string str) =>
            str.ToBytes();

        /// <summary>
        ///     Converts the specified sequence of bytes into a string with the specified
        ///     <see cref="Encoding"/> format.
        /// </summary>
        /// <typeparam name="TEncoding">
        ///     The type of encoding.
        /// </typeparam>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        /// <param name="encoding">
        ///     The <see cref="Encoding"/> format.
        /// </param>
        public static string ToString<TEncoding>(this byte[] bytes, TEncoding encoding) where TEncoding : Encoding
        {
            try
            {
                if (bytes == null)
                    throw new ArgumentNullException(nameof(bytes));
                return encoding.GetString(bytes);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified sequence of bytes into a string with the specified
        ///     Windows-1252 <see cref="Encoding"/> format.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        public static string ToStringDefault(this byte[] bytes) =>
            bytes.ToString(Encoding.GetEncoding(1252));

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
        /// <typeparam name="TResult">
        ///     The type of the result.
        /// </typeparam>
        /// <param name="value">
        ///     The value to convert.
        /// </param>
        /// <param name="defValue">
        ///     The default value.
        /// </param>
        public static TResult Parse<TResult>(this object value, TResult defValue = default(TResult))
        {
            var converter = TypeDescriptor.GetConverter(typeof(TResult));
            var result = (TResult)converter.ConvertFrom(value);
            return Comparison.IsNotEmpty(result) ? result : defValue;
        }

        /// <summary>
        ///     Try to convert the given <see cref="object"/> value to the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type of the result.
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
        public static bool TryParse<TResult>(this object value, out dynamic result, TResult defValue = default(TResult))
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(TResult));
                result = (TResult)converter.ConvertFrom(value);
                return true;
            }
            catch
            {
                result = defValue;
                return false;
            }
        }

        /// <summary>
        ///     Updates an element with the provided key and value of the specified dictionary.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of the keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of the values in the dictionary.
        /// </typeparam>
        /// <param name="source">
        ///     The generic collection of key/value pairs.
        /// </param>
        /// <param name="key">
        ///     The key of the element to update.
        /// </param>
        /// <param name="value">
        ///     The new value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source or key is null.
        /// </exception>
        public static void Update<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value) where TValue : IComparable, IComparable<TValue>
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (source.ContainsKey(key))
            {
                if (value == null)
                {
                    source.Remove(key);
                    return;
                }
                source[key] = value;
                return;
            }
            source.Add(key, value);
        }
    }
}
