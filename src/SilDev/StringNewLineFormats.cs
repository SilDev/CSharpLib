#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: StringNewLineFormats.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    /// <summary>
    ///     Provides <see cref="string"/> values of line separator characters.
    /// </summary>
    public static class StringNewLineFormats
    {
        /// <summary>
        ///     Carriage Return.
        /// </summary>
        public const string CarriageReturn = "\u000d";

        /// <summary>
        ///     Form Feed.
        /// </summary>
        public const string FormFeed = "\u000c";

        /// <summary>
        ///     Line Feed.
        /// </summary>
        public const string LineFeed = "\u000a";

        /// <summary>
        ///     Line Separator.
        /// </summary>
        public const string LineSeparator = "\u2028";

        /// <summary>
        ///     Next Line.
        /// </summary>
        public const string NextLine = "\u0085";

        /// <summary>
        ///     Paragraph Separator.
        /// </summary>
        public const string ParagraphSeparator = "\u2029";

        /// <summary>
        ///     Vertical Tab.
        /// </summary>
        public const string VerticalTab = "\u000b";

        /// <summary>
        ///     Carriage Return &amp; Line Feed.
        /// </summary>
        public const string WindowsDefault = "\u000d\u000a";

        /// <summary>
        ///     Returns a sequence of all line separator characters.
        /// </summary>
        public static readonly char[] All =
        {
            '\u000d',
            '\u000c',
            '\u000a',
            '\u2028',
            '\u0085',
            '\u2029',
            '\u000b'
        };
    }
}
