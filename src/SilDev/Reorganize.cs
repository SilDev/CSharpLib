#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Reorganize.cs
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
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using Ini;

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
    /// ReSharper disable InconsistentNaming
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
    ///     Provides static un-categorized extension methods for converting or
    ///     reorganizing of data.
    /// </summary>
    public static class Reorganize
    {
        /// <summary>
        ///     Converts this numeric value into a string that represents the number
        ///     expressed as a size value in the specified <see cref="SizeUnit"/>.
        /// </summary>
        /// <param name="value">
        ///     The value to be converted.
        /// </param>
        /// <param name="unit">
        ///     The new unit.
        /// </param>
        /// <param name="binary">
        ///     <see langword="true"/> for the binary numeral system; otherwise,
        ///     <see langword="false"/> for the decimal numeral system.
        /// </param>
        /// <param name="suffix">
        ///     <see langword="true"/> to show the size unit suffix; otherwise,
        ///     <see langword="false"/>.
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
        ///     Converts this numeric value into a string that represents the number
        ///     expressed as a size value in the specified <see cref="SizeUnit"/>.
        /// </summary>
        /// <param name="value">
        ///     The value to be converted.
        /// </param>
        /// <param name="unit">
        ///     The new unit.
        /// </param>
        /// <param name="binary">
        ///     <see langword="true"/> for the binary numeral system; otherwise,
        ///     <see langword="false"/> for the decimal numeral system.
        /// </param>
        /// <param name="sizeOptions">
        /// </param>
        public static string FormatSize(this long value, SizeUnit unit, bool binary, SizeOption sizeOptions) =>
            value.FormatSize(unit, binary, true, sizeOptions);

        /// <summary>
        ///     Converts this numeric value into a string that represents the number
        ///     expressed as a size value in the specified <see cref="SizeUnit"/>.
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
        ///     Converts this numeric value into a string that represents the number
        ///     expressed as a size value in bytes, kilobytes, megabytes, gigabytes,
        ///     terabyte, petabyte, exabyte, depending on the size.
        /// </summary>
        /// <param name="value">
        ///     The value to be converted.
        /// </param>
        /// <param name="binary">
        ///     <see langword="true"/> for the binary numeral system; otherwise,
        ///     <see langword="false"/> for the decimal numeral system.
        /// </param>
        /// <param name="suffix">
        ///     <see langword="true"/> to show the size unit suffix; otherwise,
        ///     <see langword="false"/>.
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
        ///     Converts this numeric value into a string that represents the number
        ///     expressed as a size value in bytes, kilobytes, megabytes, gigabytes,
        ///     terabyte, petabyte, exabyte, depending on the size.
        /// </summary>
        /// <param name="value">
        ///     The value to be converted.
        /// </param>
        /// <param name="binary">
        ///     <see langword="true"/> for the binary numeral system; otherwise,
        ///     <see langword="false"/> for the decimal numeral system.
        /// </param>
        /// <param name="sizeOptions">
        /// </param>
        public static string FormatSize(this long value, bool binary, SizeOption sizeOptions) =>
            value.FormatSize(SizeUnit.Byte, binary, true, sizeOptions);

        /// <summary>
        ///     Converts this numeric value into a string that represents the number
        ///     expressed as a size value in bytes, kilobytes, megabytes, gigabytes,
        ///     terabyte, petabyte, exabyte, depending on the size.
        /// </summary>
        /// <param name="value">
        ///     The value to be converted.
        /// </param>
        /// <param name="sizeOptions">
        /// </param>
        public static string FormatSize(this long value, SizeOption sizeOptions) =>
            value.FormatSize(true, true, sizeOptions);

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
                using var ms = new MemoryStream();
                var bf = new BinaryFormatter(null, new StreamingContext(state));
                bf.Serialize(ms, src);
                return ms.ToArray();
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
                using var ms = new MemoryStream();
                var bf = new BinaryFormatter(null, new StreamingContext(state));
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return (TResult)bf.Deserialize(ms);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return defValue;
            }
        }

        /// <summary>
        ///     Increments the length of a platform-specific type number with the specified
        ///     value.
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
        ///     Converts the value of this element to an equivalent <see cref="bool"/>
        ///     value.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static bool ToBoolean<TSource>(this TSource src) where TSource : IComparable, IConvertible
        {
            switch (src)
            {
                case null:
                    return default;
                case string str when bool.TryParse(str, out var result):
                    return result;
                case string str when int.TryParse(str, out var result):
                    return result > 0;
                case string str when long.TryParse(str, out var result):
                    return result > 0L;
                case string str when decimal.TryParse(str, out var result):
                    return result > 0m;
                case string:
                    return default;
                default:
                    var comparer = Comparer<TSource>.Default;
                    return comparer.Compare(src, (TSource)(object)0) > 0;
            }
        }

        /// <summary>
        ///     Converts the value of this element to an equivalent <see cref="bool"/>
        ///     value.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static bool ToBoolean<TSource>(this TSource? src) where TSource : struct, IComparable, IConvertible =>
            src is bool b ? b : src != null && ToBoolean((TSource)src);

        /// <summary>
        ///     Converts the value of this element to its equivalent Unicode character.
        ///     using the <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static char ToChar<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when char.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToChar(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to its equivalent Unicode character.
        ///     using the <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static char ToChar<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToChar((TSource)src);

        /// <summary>
        ///     Converts the value of this element to a 8-bit signed integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static sbyte ToSByte<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when sbyte.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToSByte(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to a 8-bit signed integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static sbyte ToSByte<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToSByte((TSource)src);

        /// <summary>
        ///     Converts the value of this element to a 8-bit unsigned integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static byte ToByte<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when byte.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToByte(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to a 8-bit unsigned integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static byte ToByte<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToByte((TSource)src);

        /// <summary>
        ///     Converts the value of this element to a 16-bit signed integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static short ToInt16<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when short.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToInt16(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to a 16-bit signed integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static short ToInt16<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToInt16((TSource)src);

        /// <summary>
        ///     Converts the value of this element to a 16-bit unsigned integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static ushort ToUInt16<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when ushort.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToUInt16(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to a 16-bit unsigned integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static ushort ToUInt16<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToUInt16((TSource)src);

        /// <summary>
        ///     Converts the value of this element to a 32-bit signed integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static int ToInt32<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when int.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToInt32(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to a 32-bit signed integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static int ToInt32<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToInt32((TSource)src);

        /// <summary>
        ///     Converts the value of this element to a 32-bit unsigned integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static uint ToUInt32<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when uint.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToUInt32(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to a 32-bit unsigned integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static uint ToUInt32<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToUInt32((TSource)src);

        /// <summary>
        ///     Converts the value of this element to a 64-bit signed integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static long ToInt64<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when long.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToInt64(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to a 64-bit signed integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static long ToInt64<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToInt64((TSource)src);

        /// <summary>
        ///     Converts the value of this element to a 64-bit unsigned integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static ulong ToUInt64<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when ulong.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToUInt64(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to a 64-bit unsigned integer, using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static ulong ToUInt64<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToUInt64((TSource)src);

        /// <summary>
        ///     Converts the value of this element to an single-precision floating-point
        ///     number, using the <see cref="CultureInfo.CurrentCulture"/> format
        ///     information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static float ToSingle<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when float.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToSingle(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to an single-precision floating-point
        ///     number, using the <see cref="CultureInfo.CurrentCulture"/> format
        ///     information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static float ToSingle<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToSingle((TSource)src);

        /// <summary>
        ///     Converts the value of this element to an double-precision floating-point
        ///     number, using the <see cref="CultureInfo.CurrentCulture"/> format
        ///     information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static double ToDouble<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when double.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToDouble(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to an double-precision floating-point
        ///     number, using the <see cref="CultureInfo.CurrentCulture"/> format
        ///     information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static double ToDouble<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToDouble((TSource)src);

        /// <summary>
        ///     Converts the value of this element to an equivalent decimal number, using
        ///     the <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static decimal ToDecimal<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when decimal.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToDecimal(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to an equivalent decimal number, using
        ///     the <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static decimal ToDecimal<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToDecimal((TSource)src);

        /// <summary>
        ///     Converts the value of this element to an equivalent <see cref="DateTime"/>
        ///     object, using the <see cref="CultureInfo.CurrentCulture"/> format
        ///     information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static DateTime ToDateTime<TSource>(this TSource src) where TSource : IConvertible =>
            src switch
            {
                null => default,
                string str when DateTime.TryParse(str, out var result) => result,
                string => default,
                _ => Convert.ToDateTime(src, CultureInfo.CurrentCulture)
            };

        /// <summary>
        ///     Converts the value of this element to an equivalent <see cref="DateTime"/>
        ///     object, using the <see cref="CultureInfo.CurrentCulture"/> format
        ///     information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static DateTime ToDateTime<TSource>(this TSource? src) where TSource : struct, IConvertible =>
            src == null ? default : ToDateTime((TSource)src);

        /// <summary>
        ///     Converts the value of this element to its equivalent string representation
        ///     using the <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        /// <param name="format">
        ///     The format to use.
        /// </param>
        public static string ToStringCurrent<TSource>(this TSource src, string format) where TSource : IFormattable =>
            src.ToString(format, CultureInfo.CurrentCulture);

        /// <summary>
        ///     Converts the value of this element to its equivalent string representation
        ///     using the <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static string ToStringCurrent<TSource>(this TSource src) where TSource : IFormattable =>
            src.ToString(null, CultureInfo.CurrentCulture);

        /// <summary>
        ///     Converts the value of this boolean to its equivalent string representation
        ///     using the <see cref="CultureInfo.CurrentCulture"/> format information.
        /// </summary>
        /// <param name="b">
        ///     The boolean value to convert.
        /// </param>
        public static string ToStringCurrent(this bool b) =>
            b.ToString(CultureInfo.CurrentCulture);

        /// <summary>
        ///     Converts the value of this character to its equivalent string
        ///     representation using the <see cref="CultureInfo.CurrentCulture"/> format
        ///     information.
        /// </summary>
        /// <param name="chr">
        ///     The character to convert.
        /// </param>
        public static string ToStringCurrent(this char chr) =>
            chr.ToString(CultureInfo.CurrentCulture);

        /// <summary>
        ///     Converts the value of this <see cref="DateTime"/> object to its equivalent
        ///     string representation using the <see cref="CultureInfo.CurrentCulture"/>
        ///     format information.
        /// </summary>
        /// <param name="dateTime">
        ///     The <see cref="DateTime"/> value to convert.
        /// </param>
        /// <param name="format">
        ///     A standard or custom date and time format string.
        /// </param>
        public static string ToStringCurrent(this DateTime dateTime, string format) =>
            dateTime.ToString(format, CultureInfo.CurrentCulture);

        /// <summary>
        ///     Converts the value of this <see cref="DateTime"/> object to its equivalent
        ///     string representation using the <see cref="CultureInfo.CurrentCulture"/>
        ///     format information.
        /// </summary>
        /// <param name="dateTime">
        ///     The <see cref="DateTime"/> value to convert.
        /// </param>
        public static string ToStringCurrent(this DateTime dateTime) =>
            dateTime.ToString(CultureInfo.CurrentCulture);

        /// <summary>
        ///     Converts the value of this element to its equivalent string representation
        ///     using the <see cref="CultureConfig.GlobalCultureInfo"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        /// <param name="format">
        ///     The format to use.
        /// </param>
        public static string ToStringDefault<TSource>(this TSource src, string format) where TSource : IFormattable =>
            src.ToString(format, CultureConfig.GlobalCultureInfo);

        /// <summary>
        ///     Converts the value of this element to its equivalent string representation
        ///     using the <see cref="CultureConfig.GlobalCultureInfo"/> format information.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source element.
        /// </typeparam>
        /// <param name="src">
        ///     The source to convert.
        /// </param>
        public static string ToStringDefault<TSource>(this TSource src) where TSource : IFormattable =>
            src.ToString(null, CultureConfig.GlobalCultureInfo);

        /// <summary>
        ///     Converts the value of this boolean to its equivalent string representation
        ///     using the <see cref="CultureConfig.GlobalCultureInfo"/> format information.
        /// </summary>
        /// <param name="b">
        ///     The boolean value to convert.
        /// </param>
        public static string ToStringDefault(this bool b) =>
            b.ToString(CultureConfig.GlobalCultureInfo);

        /// <summary>
        ///     Converts the value of this character to its equivalent string
        ///     representation using the <see cref="CultureConfig.GlobalCultureInfo"/>
        ///     format information.
        /// </summary>
        /// <param name="chr">
        ///     The character to convert.
        /// </param>
        public static string ToStringDefault(this char chr) =>
            chr.ToString(CultureConfig.GlobalCultureInfo);

        /// <summary>
        ///     Converts the value of this <see cref="DateTime"/> object to its equivalent
        ///     string representation using the
        ///     <see cref="CultureConfig.GlobalCultureInfo"/> format information.
        /// </summary>
        /// <param name="dateTime">
        ///     The <see cref="DateTime"/> value to convert.
        /// </param>
        /// <param name="format">
        ///     A standard or custom date and time format string.
        /// </param>
        public static string ToStringDefault(this DateTime dateTime, string format) =>
            dateTime.ToString(format, CultureConfig.GlobalCultureInfo);

        /// <summary>
        ///     Converts the value of this <see cref="DateTime"/> object to its equivalent
        ///     string representation using the
        ///     <see cref="CultureConfig.GlobalCultureInfo"/> format information.
        /// </summary>
        /// <param name="dateTime">
        ///     The <see cref="DateTime"/> value to convert.
        /// </param>
        public static string ToStringDefault(this DateTime dateTime) =>
            dateTime.ToString(CultureConfig.GlobalCultureInfo);

        /// <summary>
        ///     Converts all the characters in the specified string into a sequence of
        ///     bytes with the specified <see cref="Encoding"/> format.
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
        ///     Converts all the characters in the specified string into a sequence of
        ///     bytes with the <see cref="EncodingEx.Ansi"/> format.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static byte[] ToBytesDefault(this string str) =>
            str.ToBytes(EncodingEx.Ansi);

        /// <summary>
        ///     Converts all the characters in the specified string into a sequence of
        ///     bytes with the <see cref="EncodingEx.Utf8NoBom"/> format.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static byte[] ToBytesUtf8(this string str) =>
            str.ToBytes(EncodingEx.Utf8NoBom);

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
        ///     <see cref="EncodingEx.Ansi"/> format.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        public static string ToStringDefault(this byte[] bytes) =>
            bytes.ToString(EncodingEx.Ansi);

        /// <summary>
        ///     Converts the specified sequence of bytes into a string with the specified
        ///     <see cref="EncodingEx.Utf8NoBom"/> format.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to convert.
        /// </param>
        public static string ToStringUtf8(this byte[] bytes) =>
            bytes.ToString(EncodingEx.Utf8NoBom);

        /// <summary>
        ///     Converts the string representation of a version number to an equivalent
        ///     <see cref="Version"/> object.
        /// </summary>
        /// <param name="str">
        ///     The string to convert.
        /// </param>
        public static Version ToVersion(this string str)
        {
            if (string.IsNullOrEmpty(str) || !str.Any(char.IsDigit) || !str.Contains('.'))
                return new Version("0.0.0.0");
            var ca = EnumerableEx.Range('0', '9').ToArray();
            var sa = new string[4];
            var i = 0;
            foreach (var e in str.Split('.'))
            {
                if (i > 3)
                    break;
                if (e.Length < 1 || !e.StartsWithEx(ca))
                    continue;
                if (e.All(char.IsDigit))
                {
                    sa[i++] = e.TrimStart('0');
                    continue;
                }
                sa[i++] = new string(e.Where(char.IsDigit).ToArray()).TrimStart('0');
            }
            for (var j = 0; j < sa.Length; j++)
            {
                if (!string.IsNullOrEmpty(sa[j]))
                    continue;
                sa[j] = "0";
            }
            return new Version(sa.Join('.'));
        }

        /// <summary>
        ///     Converts the string representation of a rectangle to an equivalent
        ///     <see cref="Rectangle"/> object.
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
                return (Rectangle)ConvertStringToSpecifiedType<RectangleConverter>(item);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return Rectangle.Empty;
            }
        }

        /// <summary>
        ///     Converts the string representation of a pair of integers for x- and
        ///     y-coordinate into a corresponding <see cref="Point"/> object.
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
                return (Point)ConvertStringToSpecifiedType<PointConverter>(item);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return new Point(int.MinValue, int.MinValue);
            }
        }

        /// <summary>
        ///     Converts the string representation of a pair of integers for width and
        ///     height into a corresponding <see cref="Size"/> object.
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
                return (Size)ConvertStringToSpecifiedType<SizeConverter>(item);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return Size.Empty;
            }
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
        ///     Returns a new sequence of bytes in which all occurrences of a specified
        ///     sequence of bytes in this instance are replaced with another specified
        ///     sequence of bytes.
        /// </summary>
        /// <param name="source">
        ///     The sequence of bytes to change.
        /// </param>
        /// <param name="current">
        ///     The sequence of bytes to be replaced.
        /// </param>
        /// <param name="replacement">
        ///     The sequence of bytes to replace all occurrences of
        ///     <paramref name="current"/>.
        /// </param>
        public static byte[] Replace(this byte[] source, byte[] current, byte[] replacement)
        {
            try
            {
                if (source?.Length is null or < 1)
                    throw new ArgumentNullException(nameof(source));
                if (current?.Length is null or < 1)
                    throw new ArgumentNullException(nameof(current));
                using var ms = new MemoryStream();
                for (var i = 0; i <= source.Length - current.Length; i++)
                    if (current.SequenceEqual(source.Skip(i).Take(current.Length)))
                    {
                        if (replacement?.Length is > 0)
                            ms.WriteBytes(replacement);
                        i += current.Length - 1;
                    }
                    else
                        ms.WriteByte(source[i]);
                for (var i = source.Length - current.Length + 1; i < source.Length; i++)
                    ms.WriteByte(source[i]);
                return ms.ToArray();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return source;
            }
        }

        /// <summary>
        ///     Converts the specified string to the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="value">
        ///     The value to convert.
        /// </param>
        /// <param name="returnType">
        ///     The type to which the return value should be converted.
        /// </param>
        public static object Parse(this string value, Type returnType)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            if (returnType == null)
                return value;
            var type = returnType;
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.DBNull:
                    return null;
                case TypeCode.String:
                    return value;
                case TypeCode.Boolean:
                    return value.ToBoolean();
                case TypeCode.Char:
                    return value.ToChar();
                case TypeCode.SByte:
                    return value.ToSByte();
                case TypeCode.Byte:
                    return value.ToByte();
                case TypeCode.Int16:
                    return value.ToInt16();
                case TypeCode.UInt16:
                    return value.ToUInt16();
                case TypeCode.Int32:
                    return value.ToInt32();
                case TypeCode.UInt32:
                    return value.ToUInt32();
                case TypeCode.Int64:
                    return value.ToInt64();
                case TypeCode.UInt64:
                    return value.ToUInt64();
                case TypeCode.Single:
                    return value.ToSingle();
                case TypeCode.Double:
                    return value.ToDouble();
                case TypeCode.Decimal:
                    return value.ToDecimal();
                case TypeCode.DateTime:
                    return value.ToDateTime();
                default:
                    if (IniHelper.HasHexPrefix(value) && IniHelper.IsHex(value) && type == typeof(byte[]))
                    {
                        var start = value.IndexOf(':') + 1;
                        return value.Substring(start).Decode(BinaryToTextEncoding.Base16);
                    }
                    if (type == typeof(Rectangle) ||
                        type == typeof(Rectangle?))
                        return value.ToRectangle();
                    if (type == typeof(Point) ||
                        type == typeof(Point?))
                        return value.ToPoint();
                    if (type == typeof(Size) ||
                        type == typeof(Size?))
                        return value.ToSize();
                    if (type == typeof(Version))
                        return value.ToVersion();
                    if (type == typeof(IEnumerable<char>))
                        return value.ToCharArray();
                    if (value.Length > 4 &&
                        value.StartsWithEx("{", "[") &&
                        value.EndsWithEx("}", "]") &&
                        type.GetInterfaces()
                            .FirstOrDefault(t => (t.IsGenericType &&
                                                  t.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                                                 t == typeof(ISerializable)) != null)
                    {
                        var method = typeof(Json).GetMethod("Deserialize")?.MakeGenericMethod(type);
                        if (method != null)
                            return method.Invoke(null, new object[]
                            {
                                value,
                                null
                            });
                    }
                    var converter = TypeDescriptor.GetConverter(type);
                    return converter.CanConvertFrom(typeof(string)) ? converter.ConvertFrom(value) : null;
            }
        }

        /// <summary>
        ///     Converts the specified string to the provided type.
        /// </summary>
        /// <param name="value">
        ///     The value to convert.
        /// </param>
        /// <param name="result">
        ///     The result value.
        /// </param>
        /// <param name="returnType">
        ///     The type to which the return value should be converted.
        /// </param>
        public static bool TryParse(this string value, out object result, Type returnType)
        {
            try
            {
                result = value.Parse(returnType);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        ///     Converts the specified object to the provided type.
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
            if (value == null)
                return defValue;
            if (value is string str)
                return str.TryParse(out var result, typeof(TResult)) && result != default ? (TResult)result : defValue;
            var converter = TypeDescriptor.GetConverter(typeof(TResult));
            if (!converter.CanConvertFrom(value.GetType()))
                return defValue;
            var element = (TResult)converter.ConvertFrom(value);
            return element == null || element.Equals(default(TResult)) ? defValue : element;
        }

        /// <summary>
        ///     Converts the specified object to the provided type.
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
        public static bool TryParse<TResult>(this object value, out TResult result, TResult defValue = default)
        {
            try
            {
                result = value.Parse(defValue);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                result = defValue;
                return false;
            }
        }

        private static object ConvertStringToSpecifiedType<TConverter>(string source) where TConverter : TypeConverter
        {
            var item = source ?? throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentInvalidException(nameof(source));
            if (item.StartsWith("{", StringComparison.Ordinal) && item.EndsWith("}", StringComparison.Ordinal))
                item = new string(item.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray()).Replace(",", ";");
            var instance = (TConverter)Activator.CreateInstance(typeof(TConverter));
            var result = instance.ConvertFrom(item);
            return result ?? throw new NullReferenceException();
        }
    }
}
