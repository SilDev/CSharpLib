#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Reorganize.cs
// Version:  2016-10-18 23:33
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Provides static methods for converting or reorganizing data.
    /// </summary>
    public static class Reorganize
    {
        /// <summary>
        ///     Provides enumerated values of various new line formats.
        /// </summary>
        public enum NewLineFormat
        {
            CarriageReturn = '\u000d',
            FormFeed = '\u000c',
            LineFeed = '\u000a',
            LineSeparator = '\u2028',
            NextLine = '\u0085',
            ParagraphSeparator = '\u2029',
            VerticalTab = '\u000b',

            /// <summary>
            ///     Returns -1 because Windows combines <see cref="CarriageReturn"/>
            ///     with <see cref="LineFeed"/>, which are not convertible to an
            ///     single enumerated value.
            /// </summary>
            WindowsDefault = -1
        }

        /// <summary>
        ///     Converts the current <see cref="NewLineFormat"/> to another <see cref="NewLineFormat"/>.
        /// </summary>
        /// <param name="str">
        ///     The text to change.
        /// </param>
        /// <param name="newLineFormat">
        ///     The new format to be applied.
        /// </param>
        public static string FormatNewLine(this string str, NewLineFormat newLineFormat = NewLineFormat.WindowsDefault)
        {
            try
            {
                var sa = Enum.GetValues(typeof(NewLineFormat)).Cast<NewLineFormat>().Select(c => (int)c == -1 ? null : $"{(char)c.GetHashCode()}").ToArray();
                var f = (int)newLineFormat == -1 ? Environment.NewLine : $"{(char)newLineFormat.GetHashCode()}";
                var s = str.Replace(Environment.NewLine, $"{(char)NewLineFormat.LineFeed}");
                return s.Split(sa, StringSplitOptions.None).Join(f);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return str;
            }
        }

        /// <summary>
        ///     Creates a sequence of strings based on natural (base e) logarithm of a count
        ///     of all the characters in the specified string.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        public static string[] ToLogStringArray(this string str)
        {
            try
            {
                var i = 0;
                var b = Math.Floor(Math.Log(str.Length));
                return str.ToLookup(c => Math.Floor(i++ / b)).Select(e => new string(e.ToArray())).ToArray();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
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
            try
            {
                var s = str;
                var ca = s.ToCharArray();
                Array.Reverse(ca);
                s = new string(ca);
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return str;
            }
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
            try
            {
                var s = string.Join(separator, values);
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
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
            try
            {
                var sa = strs;
                Array.Sort(sa);
                return sa;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
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
        public static string[] Split(this string str, string separator = "\r\n", StringSplitOptions splitOptions = StringSplitOptions.None)
        {
            try
            {
                var sa = str.Split(new[] { separator }, splitOptions);
                return sa;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
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
        ///     Converts the specified strings in an string to lowercase.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        /// <param name="strs">
        ///     The sequence of strings to convert.
        /// </param>
        public static string LowerText(this string str, params string[] strs)
        {
            try
            {
                var s = strs.Aggregate(str, (c, x) => Regex.Replace(c, x, x.ToLower(), RegexOptions.IgnoreCase));
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return str;
            }
        }

        /// <summary>
        ///     Converts the specified strings in an string to uppercase.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        /// <param name="strs">
        ///     The sequence of strings to convert.
        /// </param>
        public static string UpperText(this string str, params string[] strs)
        {
            try
            {
                var s = strs.Aggregate(str, (c, x) => Regex.Replace(c, x, x.ToUpper(), RegexOptions.IgnoreCase));
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return str;
            }
        }

        /// <summary>
        ///     Removes the specified characters in an string.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        /// <param name="chrs">
        ///     The sequence of characters to remove.
        /// </param>
        public static string RemoveChar(this string str, params char[] chrs)
        {
            try
            {
                var s = new string(str.Where(c => !chrs.Contains(c)).ToArray());
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return str;
            }
        }

        /// <summary>
        ///     Removes the specified strings in an string.
        /// </summary>
        /// <param name="str">
        ///     The string to change.
        /// </param>
        /// <param name="strs">
        ///     The sequence of strings to remove.
        /// </param>
        public static string RemoveText(this string str, params string[] strs)
        {
            try
            {
                var s = strs.Aggregate(str, (c, x) => c.Replace(x, string.Empty));
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return str;
            }
        }

        /// <summary>
        ///     Converts the specified string to base-2 binary sequence.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        /// <param name="separator">
        ///     true to add a single space character as a separator between every single byte;
        ///     otherwise, false.
        /// </param>
        public static string ToBinaryString(this string str, bool separator = true)
        {
            try
            {
                var ba = Encoding.UTF8.GetBytes(str);
                var s = separator ? " " : string.Empty;
                s = ba.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).Join(s);
                return s;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified base-2 binary sequence back to string.
        /// </summary>
        /// <param name="str">
        ///     The string to reconvert.
        /// </param>
        public static string FromBinaryString(this string str)
        {
            try
            {
                var s = str.RemoveChar(' ', ':', '\r', '\n');
                if (s.Count(c => !"01".Contains(c)) > 0)
                    throw new ArgumentException();
                var bl = new List<byte>();
                for (var i = 0; i < s.Length; i += 8)
                    bl.Add(Convert.ToByte(s.Substring(i, 8), 2));
                s = Encoding.UTF8.GetString(bl.ToArray());
                return s;
            }
            catch
            {
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
        public static string ToHexString(this byte[] bytes, char? separator = null, bool upper = false)
        {
            try
            {
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
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified image to hexadecimal sequence.
        /// </summary>
        /// <param name="image">
        ///     The image to convert.
        /// </param>
        /// <param name="separator">
        ///     A single space character used as a separator.
        /// </param>
        /// <param name="upper">
        ///     true to convert the result to uppercase; otherwise, false.
        /// </param>
        public static string ToHexString(this Image image, char? separator = null, bool upper = false) =>
            image.ToByteArray().ToHexString(separator, upper);

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
        public static string ToHexString(this string str, char? separator = null, bool upper = false) =>
            str.ToByteArray().ToHexString(separator, upper);

        /// <summary>
        ///     Converts the specified hexadecimal sequence back into a sequence of bytes.
        /// </summary>
        /// <param name="str">
        ///     The string to reconvert.
        /// </param>
        public static byte[] FromHexStringToByteArray(this string str)
        {
            try
            {
                var s = new string(str.Where(char.IsLetterOrDigit).ToArray()).ToUpper();
                var ba = Enumerable.Range(0, s.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(s.Substring(x, 2), 16)).ToArray();
                return ba;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Converts the specified hexadecimal sequence back to <see cref="Image"/>.
        /// </summary>
        /// <param name="str">
        ///     The string to reconvert.
        /// </param>
        public static Image FromHexStringToImage(this string str) =>
            str.FromHexStringToByteArray().FromByteArrayToImage();

        /// <summary>
        ///     Converts the specified hexadecimal sequence back to string.
        /// </summary>
        /// <param name="str">
        ///     The string to reconvert.
        /// </param>
        public static string FromHexString(this string str)
        {
            try
            {
                var ba = str.FromHexStringToByteArray();
                if (ba == null)
                    throw new ArgumentException();
                return Encoding.UTF8.GetString(ba);
            }
            catch
            {
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
                var s = str;
                if (s.StartsWith("{") && s.EndsWith("}"))
                    s = new string(s.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()).Replace(",", ";");
                var rc = new RectangleConverter();
                var obj = rc.ConvertFrom(s);
                if (obj == null)
                    throw new ArgumentNullException();
                var rect = (Rectangle)obj;
                return rect;
            }
            catch
            {
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
                var s = str;
                if (s.StartsWith("{") && s.EndsWith("}"))
                    s = new string(s.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()).Replace(",", ";");
                var pc = new PointConverter();
                var obj = pc.ConvertFrom(s);
                if (obj == null)
                    throw new ArgumentNullException();
                var point = (Point)obj;
                return point;
            }
            catch
            {
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
                    throw new ArgumentNullException();
                var s = str;
                if (s.StartsWith("{") && s.EndsWith("}"))
                    s = new string(s.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()).Replace(",", ";");
                var sc = new SizeConverter();
                var obj = sc.ConvertFrom(s);
                if (obj == null)
                    throw new ArgumentNullException();
                var size = (Size)obj;
                return size;
            }
            catch
            {
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
                bool b;
                if (!bool.TryParse(str, out b))
                    throw new ArgumentException();
                return b;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Converts all the characters in the specified string into a sequence of bytes.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static byte[] ToByteArray(this string str)
        {
            try
            {
                var ba = Encoding.UTF8.GetBytes(str);
                return ba;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Converts the specified image into a sequence of bytes.
        /// </summary>
        /// <param name="image">
        ///     The image to convert.
        /// </param>
        /// <param name="imageFormat">
        ///     An <see cref="ImageFormat"/> that specifies the format of the saved image.
        /// </param>
        public static byte[] ToByteArray(this Image image, ImageFormat imageFormat = null)
        {
            try
            {
                byte[] ba;
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, imageFormat ?? ImageFormat.Png);
                    ba = ms.ToArray();
                }
                return ba;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Converts the specified sequence of bytes into a string.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        public static string FromByteArrayToString(this byte[] bytes)
        {
            try
            {
                var s = Encoding.UTF8.GetString(bytes);
                return s;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts the specified sequence of bytes to <see cref="Image"/>.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        public static Image FromByteArrayToImage(this byte[] bytes)
        {
            try
            {
                Image img;
                using (var ms = new MemoryStream(bytes))
                    img = Image.FromStream(ms);
                return img;
            }
            catch
            {
                return null;
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
        public static byte[] ReplaceBytes(this byte[] bytes, byte[] oldValue, byte[] newValue)
        {
            try
            {
                var index = -1;
                var match = 0;
                for (var i = 0; i < bytes.Length; i++)
                    if (bytes[i] == oldValue[match])
                    {
                        if (match == oldValue.Length - 1)
                        {
                            // ReSharper disable RedundantAssignment
                            index += i - match;
                            break;
                        }
                        match++;
                    }
                    else
                        match = 0;
                index = match;
                if (index < 0)
                    throw new ArgumentNullException();
                var ba = new byte[bytes.Length - oldValue.Length + newValue.Length];
                Buffer.BlockCopy(bytes, 0, ba, 0, index);
                Buffer.BlockCopy(newValue, 0, ba, index, newValue.Length);
                Buffer.BlockCopy(bytes, index + oldValue.Length, ba, index + newValue.Length, bytes.Length - (index + oldValue.Length));
                return ba;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return bytes;
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
    }
}
