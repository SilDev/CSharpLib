#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TextEx.cs
// Version:  2023-11-11 16:27
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

// Improved .NET Core version can be found
// at https://github.com/Roydl/Text
namespace SilDev
{
    using System;
    using System.Linq;

    /// <summary>
    ///     Provides static methods for converting text.
    /// </summary>
    public static class TextEx
    {
        /// <summary>
        ///     Indicates whether the this character is categorized as an ASCII character.
        /// </summary>
        /// <param name="ch">
        ///     The character to evaluate.
        /// </param>
        public static bool IsAscii(this char ch) =>
            ch <= sbyte.MaxValue;

        /// <summary>
        ///     Indicates whether the this character is categorized as an Latin-1
        ///     (ISO-8859-1) character.
        /// </summary>
        /// <param name="ch">
        ///     The character to evaluate.
        /// </param>
        public static bool IsLatin1(char ch) =>
            ch <= byte.MaxValue;

        /// <summary>
        ///     Indicates whether the this character is categorized as a line separator
        ///     character.
        /// </summary>
        /// <param name="ch">
        ///     The character to evaluate.
        /// </param>
        public static bool IsLineSeparator(this char ch)
        {
            switch (ch)
            {
                case TextSeparatorChar.LineFeed:
                case TextSeparatorChar.VerticalTab:
                case TextSeparatorChar.FormFeed:
                case TextSeparatorChar.CarriageReturn:
                case TextSeparatorChar.NextLine:
                case TextSeparatorChar.BoundaryNeutral:
                case TextSeparatorChar.LineSeparator:
                case TextSeparatorChar.ParagraphSeparator:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Strips multiple line separator characters.
        /// </summary>
        /// <param name="text">
        ///     The text to change.
        /// </param>
        public static string StripNewLine(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var current = text.Where(IsLineSeparator).Distinct().ToArray();
            var newText = text;
            foreach (var ch in current)
            {
                if (!newText.Contains(ch))
                    continue;
                newText = newText.Split(new[] { ch }, StringSplitOptions.RemoveEmptyEntries).Join(ch);
            }
            return newText;
        }

        /// <summary>
        ///     Strips multiple white-space characters.
        /// </summary>
        /// <param name="text">
        ///     The text to change.
        /// </param>
        public static string StripWhiteSpace(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var current = text.Where(char.IsWhiteSpace).Distinct().ToArray();
            var newText = text;
            foreach (var ch in current)
            {
                if (!newText.Contains(ch))
                    continue;
                newText = newText.Split(new[] { ch }, StringSplitOptions.RemoveEmptyEntries).Join(ch);
            }
            return newText;
        }

        /// <summary>
        ///     Converts the current line separator format of the specified format.
        /// </summary>
        /// <param name="text">
        ///     The text to change.
        /// </param>
        /// <param name="newLineFormat">
        ///     The new format to be applied.
        ///     <para>
        ///         For constant templates, see <see cref="TextSeparatorString"/>.
        ///     </para>
        /// </param>
        public static string FormatNewLine(string text, string newLineFormat = TextSeparatorString.WindowsDefault)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var newText = text.Replace(TextSeparatorString.WindowsDefault, TextSeparatorString.LineFeed);
            var current = text.Where(IsLineSeparator).Distinct().ToArray();
            return newText.Split(current, StringSplitOptions.None).Join(newLineFormat);
        }

        /// <summary>
        ///     Converts the current line separator format of the specified format.
        /// </summary>
        /// <param name="text">
        ///     The text to change.
        /// </param>
        /// <param name="newWhiteSpace">
        ///     The new format to be applied.
        ///     <para>
        ///         For constant templates, see <see cref="TextSeparatorString"/>.
        ///     </para>
        /// </param>
        public static string FormatWhiteSpace(string text, string newWhiteSpace = TextSeparatorString.Space)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var newText = text.Replace(TextSeparatorString.WindowsDefault, TextSeparatorString.LineFeed);
            var current = text.Where(char.IsWhiteSpace).Distinct().ToArray();
            return newText.Split(current, StringSplitOptions.None).Join(newWhiteSpace);
        }
    }
}
