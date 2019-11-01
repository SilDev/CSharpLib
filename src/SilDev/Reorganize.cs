#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Reorganize.cs
// Version:  2019-10-31 22:01
// 
// Copyright (c) 2019, Si13n7 Developments (r)
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
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Provides size format options.
    /// </summary>
    public enum SizeOption
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
    public enum SizeUnit
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
    ///     Provides static un-categorized extension methods for converting or reorganizing of data.
    /// </summary>
    public static class Reorganize
    {
        /// <summary>
        ///     Appends the string returned by processing a composite format string, which contains
        ///     zero or more format items followed by the default line terminator to the end, to this
        ///     <see cref="StringBuilder"/> instance. Each format item is replaced by the string
        ///     representation of a corresponding argument in a parameter array using a specified
        ///     format provider.
        /// </summary>
        /// <param name="stringBuilder">
        ///     The <see cref="StringBuilder"/> instance to which the string should be append.
        /// </param>
        /// <param name="provider">
        ///     An object that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="args">
        ///     An array of objects to format.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stringBuilder, format or args is null.
        /// </exception>
        public static StringBuilder AppendFormatLine(this StringBuilder stringBuilder, IFormatProvider provider, string format, params object[] args)
        {
            if (stringBuilder == null)
                throw new ArgumentNullException(nameof(stringBuilder));
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            var sb = stringBuilder;
            sb.AppendFormat(provider, format, args);
            sb.AppendLine();
            return sb;
        }

        /// <summary>
        ///     Appends the string returned by processing a composite format string, which contains
        ///     zero or more format items followed by the default line terminator to the end, to this
        ///     <see cref="StringBuilder"/> instance. Each format item is replaced by the string
        ///     representation of a corresponding argument in a parameter array.
        /// </summary>
        /// <param name="stringBuilder">
        ///     The <see cref="StringBuilder"/> instance to which the string should be append.
        /// </param>
        /// <param name="format">
        ///     A composite format string.
        /// </param>
        /// <param name="args">
        ///     An array of objects to format.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stringBuilder, format or args is null.
        /// </exception>
        public static StringBuilder AppendFormatLine(this StringBuilder stringBuilder, string format, params object[] args)
        {
            if (stringBuilder == null)
                throw new ArgumentNullException(nameof(stringBuilder));
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            var sb = stringBuilder;
            sb.AppendFormat(CultureConfig.GlobalCultureInfo, format, args);
            sb.AppendLine();
            return sb;
        }

        /// <summary>
        ///     Converts this numeric value into a string that represents the number expressed as a size
        ///     value in the specified <see cref="SizeUnit"/>.
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
        public static string FormatSize(this long value, SizeUnit unit, bool binary = true, bool suffix = true, SizeOption sizeOptions = SizeOption.None)
        {
            if (value < 0)
                return $"-{Math.Abs(value).FormatSize(unit, binary, suffix, sizeOptions)}";
            var f = sizeOptions != SizeOption.None ? "0.##" : "0.00";
            if (value == 0)
                return $"{value.ToString(f, CultureConfig.GlobalCultureInfo)} bytes";
            var d = value / Math.Pow(binary ? 1024 : 1000, (int)unit);
            if (sizeOptions == SizeOption.Round)
                d = Math.Round(d);
            var s = d.ToString(f, CultureConfig.GlobalCultureInfo);
            if (!suffix)
                return s;
            s = $"{s} {unit}";
            if (unit == 0 && !Math.Abs(d).Equals(1d))
                s += "s";
            return s;
        }

        /// <summary>
        ///     Converts this numeric value into a string that represents the number expressed as a size
        ///     value in the specified <see cref="SizeUnit"/>.
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
        public static string FormatSize(this long value, SizeUnit unit, bool binary, SizeOption sizeOptions) =>
            value.FormatSize(unit, binary, true, sizeOptions);

        /// <summary>
        ///     Converts this numeric value into a string that represents the number expressed as a size
        ///     value in the specified <see cref="SizeUnit"/>.
        /// </summary>
        /// <param name="value">
        ///     The value to be converted.
        /// </param>
        /// <param name="unit">
        ///     The new unit.
        /// </param>
        /// <param name="sizeOptions">
        /// </param>
        public static string FormatSize(this long value, SizeUnit unit, SizeOption sizeOptions) =>
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
        public static string FormatSize(this long value, bool binary = true, bool suffix = true, SizeOption sizeOptions = SizeOption.None)
        {
            if (value == 0)
                return value.FormatSize(SizeUnit.Byte, binary, suffix, sizeOptions);
            var i = (int)Math.Floor(Math.Log(Math.Abs(value), binary ? 1024 : 1000));
            return value.FormatSize((SizeUnit)i, binary, suffix, sizeOptions);
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
        public static string FormatSize(this long value, bool binary, SizeOption sizeOptions) =>
            value.FormatSize(SizeUnit.Byte, binary, true, sizeOptions);

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
        public static string FormatSize(this long value, SizeOption sizeOptions) =>
            value.FormatSize(true, true, sizeOptions);

        /// <summary>
        ///     Reads the bytes from the specified stream and writes them to another stream.
        /// </summary>
        /// <param name="srcStream">
        ///     The <see cref="Stream"/> to copy.
        /// </param>
        /// <param name="destStream">
        ///     The <see cref="Stream"/> to override.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stream or bytes is null.
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O error occured, such as the specified file cannot be found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The stream does not support writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     stream was closed while the bytes were being written.
        /// </exception>
        public static void CopyTo(this Stream srcStream, Stream destStream)
        {
            if (srcStream == null)
                throw new ArgumentNullException(nameof(srcStream));
            if (destStream == null)
                throw new ArgumentNullException(nameof(destStream));
            int i;
            var ba = new byte[4096];
            while ((i = srcStream.Read(ba, 0, ba.Length)) > 0)
                destStream.Write(ba, 0, i);
        }

        /// <summary>
        ///     Writes a sequence of bytes to the this stream and advances the current position within
        ///     this stream by the number of bytes written.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="Stream"/> to write.
        /// </param>
        /// <param name="buffer">
        ///     An array of bytes to write.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stream or bytes is null.
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O error occured, such as the specified file cannot be found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The stream does not support writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     stream was closed while the bytes were being written.
        /// </exception>
        public static void WriteBytes(this Stream stream, byte[] buffer)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            var ba = buffer;
            stream.Write(ba, 0, ba.Length);
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
        /// <param name="state">
        ///     Specifies the destination context for the stream during serialization.
        /// </param>
        public static byte[] SerializeObject<TSource>(this TSource src, StreamingContextStates state = StreamingContextStates.All)
        {
            try
            {
                byte[] ba;
                using (var ms = new MemoryStream())
                {
                    var bf = new BinaryFormatter(null, new StreamingContext(state));
                    bf.Serialize(ms, src);
                    ba = ms.ToArray();
                }
                return ba;
            }
            catch (Exception ex) when (ex.IsCaught())
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
        /// <param name="state">
        ///     Specifies the source context for the stream during serialization.
        /// </param>
        public static TResult DeserializeObject<TResult>(this byte[] bytes, TResult defValue = default, StreamingContextStates state = StreamingContextStates.All)
        {
            try
            {
                if (bytes == null)
                    throw new ArgumentNullException(nameof(bytes));
                object obj;
                using (var ms = new MemoryStream())
                {
                    var bf = new BinaryFormatter(null, new StreamingContext(state));
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    obj = bf.Deserialize(ms);
                }
                return (TResult)obj;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return defValue;
            }
        }

        /// <summary>
        ///     Increments the length of a platform-specific type number with the specified value.
        /// </summary>
        /// <param name="intPointer">
        ///     The platform-specific type to change.
        /// </param>
        /// <param name="value">
        ///     The number to be incremented.
        /// </param>
        public static IntPtr Increment(this IntPtr intPointer, IntPtr value)
        {
            try
            {
                return new IntPtr(IntPtr.Size == sizeof(int) ? intPointer.ToInt32() + (int)value : intPointer.ToInt64() + (long)value);
            }
            catch (Exception ex) when (ex.IsCaught())
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
            if (str == null)
                return null;
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(oldValue))
                return str;
            if (newValue == null)
                newValue = string.Empty;
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
            while (q.Any());
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
            return sb.ToString();
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
        ///     the substrings include empty array elements.
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
        public static string[] Split(this string str, string separator = TextEx.NewLineFormats.WindowsDefault, StringSplitOptions splitOptions = StringSplitOptions.None) =>
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
                if (str == null)
                    throw new ArgumentNullException(nameof(str));
                if (string.IsNullOrEmpty(str))
                    throw new ArgumentInvalidException(nameof(str));
                if (encoding == null)
                    throw new ArgumentNullException(nameof(encoding));
                return encoding.GetBytes(str);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Converts all the characters in the specified string into a sequence of bytes with the
        ///     <see cref="TextEx.DefaultEncoding"/> format.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static byte[] ToBytes(this string str) =>
            str.ToBytes(TextEx.DefaultEncoding);

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
                if (encoding == null)
                    throw new ArgumentNullException(nameof(encoding));
                return encoding.GetString(bytes);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified sequence of bytes into a string with the specified
        ///     <see cref="TextEx.DefaultEncoding"/> format.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        public static string ToStringDefault(this byte[] bytes) =>
            bytes.ToString(TextEx.DefaultEncoding);

        private static object ConvertToSpecifiedType<TConverter>(string source) where TConverter : TypeConverter
        {
            var item = source ?? throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentInvalidException(nameof(source));
            if (item.StartsWith("{", StringComparison.Ordinal) && item.EndsWith("}", StringComparison.Ordinal))
                item = new string(item.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()).Replace(",", ";");
            var instance = (TConverter)Activator.CreateInstance(typeof(TConverter));
            var result = instance.ConvertFrom(item);
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            return result;
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
                var item = str ?? throw new ArgumentNullException(nameof(str));
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentInvalidException(nameof(str));
                return (Rectangle)ConvertToSpecifiedType<RectangleConverter>(item);
            }
            catch (Exception ex) when (ex.IsCaught())
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
                var item = str ?? throw new ArgumentNullException(nameof(str));
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentInvalidException(nameof(str));
                return (Point)ConvertToSpecifiedType<PointConverter>(item);
            }
            catch (Exception ex) when (ex.IsCaught())
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
                var item = str ?? throw new ArgumentNullException(nameof(str));
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentInvalidException(nameof(str));
                return (Size)ConvertToSpecifiedType<SizeConverter>(item);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return Size.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified <see cref="string"/> to an equivalent <see cref="bool"/>
        ///     value, which returns always false for unsupported <see cref="string"/> values.
        /// </summary>
        /// <param name="str">
        ///     The <see cref="string"/> to convert.
        /// </param>
        public static bool ToBoolean(this string str)
        {
            try
            {
                return bool.Parse(str);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                return false;
            }
        }

        /// <summary>
        ///     Converts the specified value to an equivalent <see cref="bool"/> value.
        /// </summary>
        /// <param name="src">
        ///     The value to convert.
        /// </param>
        public static bool ToBoolean<TSource>(this TSource src) where TSource : struct, IComparable<TSource>
        {
            switch (src)
            {
                case bool b:
                    return b;
                default:
                    try
                    {
                        var c = Comparer<TSource>.Default;
                        return c.Compare(src, (TSource)(object)default(int)) > 0;
                    }
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        return false;
                    }
            }
        }

        /// <summary>
        ///     Converts the specified <see cref="Nullable"/> value to an equivalent
        ///     <see cref="bool"/> value, which returns always false for null.
        /// </summary>
        /// <param name="src">
        ///     The <see cref="Nullable"/> value to convert.
        /// </param>
        public static bool ToBoolean<TSource>(this TSource? src) where TSource : struct, IComparable<TSource>
        {
            switch (src)
            {
                case null:
                    return false;
                case bool b:
                    return b;
                default:
                    try
                    {
                        var c = Comparer<TSource>.Default;
                        return c.Compare((TSource)src, (TSource)(object)0) > 0;
                    }
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        return false;
                    }
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
            return array.Where(c => !chrs.Contains(c)).ToArray();
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
                            ms.WriteBytes(newValue);
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
            catch (Exception ex) when (ex.IsCaught())
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
        public static TResult Parse<TResult>(this object value, TResult defValue = default)
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
        public static bool TryParse<TResult>(this object value, out dynamic result, TResult defValue = default)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(TResult));
                result = (TResult)converter.ConvertFrom(value);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
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
