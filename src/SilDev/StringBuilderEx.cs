#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: StringBuilderEx.cs
// Version:  2023-12-20 12:04
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Globalization;
    using System.Text;

    public static class StringBuilderEx
    {
        /// <summary>
        ///     Appends the string returned by processing a composite format string, which
        ///     contains zero or more format items, to this <see cref="StringBuilder"/>
        ///     instance. Each format item is replaced by the string representation of a
        ///     corresponding argument in a parameter array using the
        ///     <see cref="CultureInfo.CurrentCulture"/> format provider.
        /// </summary>
        /// <param name="stringBuilder">
        ///     The <see cref="StringBuilder"/> instance to which the string should be
        ///     append.
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
        public static StringBuilder AppendFormatCurrent(this StringBuilder stringBuilder, string format, params object[] args)
        {
            if (stringBuilder == null)
                throw new ArgumentNullException(nameof(stringBuilder));
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            return stringBuilder.AppendFormat(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        ///     Appends the string returned by processing a composite format string, which
        ///     contains zero or more format items, to this <see cref="StringBuilder"/>
        ///     instance. Each format item is replaced by the string representation of a
        ///     corresponding argument in a parameter array using the
        ///     <see cref="CultureConfig.GlobalCultureInfo"/> format provider.
        /// </summary>
        /// <param name="stringBuilder">
        ///     The <see cref="StringBuilder"/> instance to which the string should be
        ///     append.
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
        public static StringBuilder AppendFormatDefault(this StringBuilder stringBuilder, string format, params object[] args)
        {
            if (stringBuilder == null)
                throw new ArgumentNullException(nameof(stringBuilder));
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            return stringBuilder.AppendFormat(CultureConfig.GlobalCultureInfo, format, args);
        }

        /// <summary>
        ///     Appends the string returned by processing a composite format string, which
        ///     contains zero or more format items followed by the default line terminator
        ///     to the end, to this <see cref="StringBuilder"/> instance. Each format item
        ///     is replaced by the string representation of a corresponding argument in a
        ///     parameter array using a specified format provider.
        /// </summary>
        /// <param name="stringBuilder">
        ///     The <see cref="StringBuilder"/> instance to which the string should be
        ///     append.
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
        ///     Appends the string returned by processing a composite format string, which
        ///     contains zero or more format items followed by the default line terminator
        ///     to the end, to this <see cref="StringBuilder"/> instance. Each format item
        ///     is replaced by the string representation of a corresponding argument in a
        ///     parameter array using the <see cref="CultureInfo.CurrentCulture"/> format
        ///     provider.
        /// </summary>
        /// <param name="stringBuilder">
        ///     The <see cref="StringBuilder"/> instance to which the string should be
        ///     append.
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
        public static StringBuilder AppendFormatLineCurrent(this StringBuilder stringBuilder, string format, params object[] args) =>
            stringBuilder.AppendFormatLine(CultureInfo.CurrentCulture, format, args);

        /// <summary>
        ///     Appends the string returned by processing a composite format string, which
        ///     contains zero or more format items followed by the default line terminator
        ///     to the end, to this <see cref="StringBuilder"/> instance. Each format item
        ///     is replaced by the string representation of a corresponding argument in a
        ///     parameter array using the <see cref="CultureConfig.GlobalCultureInfo"/>
        ///     format provider.
        /// </summary>
        /// <param name="stringBuilder">
        ///     The <see cref="StringBuilder"/> instance to which the string should be
        ///     append.
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
        public static StringBuilder AppendFormatLineDefault(this StringBuilder stringBuilder, string format, params object[] args) =>
            stringBuilder.AppendFormatLine(CultureConfig.GlobalCultureInfo, format, args);

        /// <summary>
        ///     Converts the value of this <see cref="StringBuilder"/> instance to
        ///     <see cref="string"/> and removes all characters before <see cref="string"/>
        ///     is returned.
        /// </summary>
        /// <param name="stringBuilder">
        ///     The <see cref="StringBuilder"/> instance to convert.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stringBuilder is null.
        /// </exception>
        public static string ToStringThenClear(this StringBuilder stringBuilder)
        {
            if (stringBuilder == null)
                throw new ArgumentNullException(nameof(stringBuilder));
            var sb = stringBuilder;
            var str = sb.ToString();
            sb.Clear();
            return str;
        }

        /// <summary>
        ///     Converts the value of a substring of this <see cref="StringBuilder"/>
        ///     instance to <see cref="string"/> and removes all characters before
        ///     <see cref="string"/> is returned.
        /// </summary>
        /// <param name="stringBuilder">
        ///     The <see cref="StringBuilder"/> instance to convert.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stringBuilder is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     startIndex or length is less than zero. -or- The sum of startIndex and
        ///     length is greater than the length of the current instance.
        /// </exception>
        public static string ToStringThenClear(this StringBuilder stringBuilder, int startIndex, int length)
        {
            if (stringBuilder == null)
                throw new ArgumentNullException(nameof(stringBuilder));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            var sb = stringBuilder;
            if (startIndex + length > sb.Length)
                throw new ArgumentOutOfRangeException(nameof(length));
            var str = sb.ToString(startIndex, length);
            sb.Clear();
            return str;
        }
    }
}
