#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TextSeparator.cs
// Version:  2023-11-11 16:27
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

// .NET Core version can be found at https://github.com/Roydl/Text
namespace SilDev
{
    /// <summary>
    ///     Provides constant <see cref="char"/> values of separator characters.
    /// </summary>
    public static class TextSeparatorChar
    {
        /// <summary>
        ///     Boundary Neutral [BN].
        /// </summary>
        public const char BoundaryNeutral = '\u200B';

        /// <summary>
        ///     Carriage Return [CR].
        /// </summary>
        public const char CarriageReturn = '\r';

        /// <summary>
        ///     Common Number Separator [CS].
        /// </summary>
        public const char CommonNumberSeparator = '\u00a0';

        /// <summary>
        ///     Form Feed [FF].
        /// </summary>
        public const char FormFeed = '\f';

        /// <summary>
        ///     Horizontal Tab [TAB].
        /// </summary>
        public const char HorizontalTab = '\t';

        /// <summary>
        ///     Line Feed [LF].
        /// </summary>
        public const char LineFeed = '\n';

        /// <summary>
        ///     Line Separator.
        /// </summary>
        public const char LineSeparator = '\u2028';

        /// <summary>
        ///     Next Line [NEL].
        /// </summary>
        public const char NextLine = '\u0085';

        /// <summary>
        ///     Paragraph Separator [B].
        /// </summary>
        public const char ParagraphSeparator = '\u2029';

        /// <summary>
        ///     Space.
        /// </summary>
        public const char Space = ' ';

        /// <summary>
        ///     Vertical Tab [VT].
        /// </summary>
        public const char VerticalTab = '\v';

        /// <summary>
        ///     Returns a sequence of all line separator characters.
        /// </summary>
        public static readonly char[] AllNewLineChars =
        {
            LineFeed,
            VerticalTab,
            FormFeed,
            CarriageReturn,
            NextLine,
            BoundaryNeutral,
            LineSeparator,
            ParagraphSeparator
        };

        /// <summary>
        ///     Returns a sequence of all whitespace characters.
        /// </summary>
        public static readonly char[] AllWhiteSpaceChars =
        {
            LineFeed,
            HorizontalTab,
            VerticalTab,
            FormFeed,
            CarriageReturn,
            Space,
            NextLine,
            CommonNumberSeparator,
            BoundaryNeutral,
            LineSeparator,
            ParagraphSeparator
        };
    }

    /// <summary>
    ///     Provides constant <see cref="string"/> values of separator characters.
    /// </summary>
    public static class TextSeparatorString
    {
        /// <summary>
        ///     Boundary Neutral [BN].
        /// </summary>
        public const string BoundaryNeutral = "\u200B";

        /// <summary>
        ///     Carriage Return [CR].
        /// </summary>
        public const string CarriageReturn = "\r";

        /// <summary>
        ///     Common Number Separator [CS].
        /// </summary>
        public const string CommonNumberSeparator = "\u00a0";

        /// <summary>
        ///     Form Feed [FF].
        /// </summary>
        public const string FormFeed = "\f";

        /// <summary>
        ///     Horizontal Tab [TAB].
        /// </summary>
        public const string HorizontalTab = "\t";

        /// <summary>
        ///     Line Feed [LF].
        /// </summary>
        public const string LineFeed = "\n";

        /// <summary>
        ///     Line Separator.
        /// </summary>
        public const string LineSeparator = "\u2028";

        /// <summary>
        ///     Next Line [NEL].
        /// </summary>
        public const string NextLine = "\u0085";

        /// <summary>
        ///     Paragraph Separator [B].
        /// </summary>
        public const string ParagraphSeparator = "\u2029";

        /// <summary>
        ///     Space.
        /// </summary>
        public const string Space = " ";

        /// <summary>
        ///     Vertical Tab [VT].
        /// </summary>
        public const string VerticalTab = "\v";

        /// <summary>
        ///     Carriage Return [CR] &amp; Line Feed [LF].
        /// </summary>
        public const string WindowsDefault = "\r\n";

        /// <summary>
        ///     Returns a sequence of all line separator strings.
        /// </summary>
        public static readonly string[] AllNewLineStrings =
        {
            LineFeed,
            VerticalTab,
            FormFeed,
            CarriageReturn,
            NextLine,
            BoundaryNeutral,
            LineSeparator,
            ParagraphSeparator
        };

        /// <summary>
        ///     Returns a sequence of all whitespace strings.
        /// </summary>
        public static readonly string[] AllWhiteSpaceStrings =
        {
            LineFeed,
            HorizontalTab,
            VerticalTab,
            FormFeed,
            CarriageReturn,
            Space,
            NextLine,
            CommonNumberSeparator,
            BoundaryNeutral,
            LineSeparator,
            ParagraphSeparator
        };
    }
}
