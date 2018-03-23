#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TextEx.cs
// Version:  2018-03-23 22:28
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///     Provides static methods for converting or reorganizing of data.
    /// </summary>
    public static class TextEx
    {
        /// <summary>
        ///     Indicates whether the specified character is categorized as a line separator
        ///     character.
        /// </summary>
        /// <param name="ch">
        ///     The character to evaluate.
        /// </param>
        public static bool IsLineSeparator(this char ch)
        {
            switch (ch)
            {
                case '\u000d': // CarriageReturn
                case '\u000c': // FormFeed
                case '\u000a': // LineFeed
                case '\u2028': // LineSeparator
                case '\u0085': // NextLine
                case '\u2029': // ParagraphSeparator
                case '\u000b': // VerticalTab
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Indicates whether the specified character is categorized as an ASCII character.
        /// </summary>
        /// <param name="ch">
        ///     The character to evaluate.
        /// </param>
        public static bool IsAscii(this char ch) =>
            ch <= sbyte.MaxValue;

        /// <summary>
        ///     Converts the current <see cref="NewLineFormats"/> of the specified
        ///     <see cref="string"/> to another format.
        /// </summary>
        /// <param name="text">
        ///     The text to change.
        /// </param>
        /// <param name="newLineFormat">
        ///     The new format to be applied.
        /// </param>
        public static string FormatNewLine(string text, string newLineFormat = NewLineFormats.WindowsDefault)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var newText = text.Replace(NewLineFormats.WindowsDefault, NewLineFormats.LineFeed);
            var current = text.Where(IsLineSeparator).Distinct().ToArray();
            newText = newText.Split(current, StringSplitOptions.None).Join(newLineFormat);
            return newText;
        }

        /// <summary>
        ///     Gets the character encoding of the specified file.
        /// </summary>
        /// <param name="file">
        ///     The file to check.
        /// </param>
        /// <param name="defEncoding">
        ///     The default character encoding, which is returned if no character encoding
        ///     was found. If the value is NULL it returns <see cref="Encoding.Default"/>.
        /// </param>
        [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public static Encoding GetEncoding(string file, Encoding defEncoding = default(Encoding))
        {
            var path = PathEx.Combine(file);
            var encoding = defEncoding ?? Encoding.Default;
            if (!File.Exists(path))
                return encoding;
            using (var sr = new StreamReader(file, true))
            {
                sr.Peek();
                encoding = sr.CurrentEncoding;
            }
            return encoding;
        }

        /// <summary>
        ///     Changes the character encoding of the specified file. This function supports
        ///     big files as well.
        /// </summary>
        /// <param name="file">
        ///     The file to change.
        /// </param>
        /// <param name="encoding">
        ///     The new character encoding.
        /// </param>
        public static bool ChangeEncoding(string file, Encoding encoding)
        {
            if (string.IsNullOrEmpty(file) || encoding == null)
                return false;
            var srcFile = PathEx.Combine(file);
            if (!File.Exists(srcFile))
                return false;
            if (encoding.Equals(GetEncoding(srcFile)))
                return true;
            try
            {
                var srcDir = Path.GetDirectoryName(srcFile);
                var newFile = PathEx.Combine(srcDir, Path.GetRandomFileName());
                File.Create(newFile).Close();
                FileEx.SetAttributes(newFile, FileAttributes.Hidden);
                using (var sr = new StreamReader(srcFile))
                {
                    var ca = new char[4096];
                    using (var sw = new StreamWriter(newFile, true, encoding))
                    {
                        int i;
                        while ((i = sr.Read(ca, 0, ca.Length)) > 0)
                            sw.Write(ca, 0, i);
                    }
                }
                var srcHash = new Crypto.Md5().EncryptFile(srcFile);
                var newHash = new Crypto.Md5().EncryptFile(newFile);
                if (srcHash.Equals(newHash))
                {
                    File.Delete(newFile);
                    return true;
                }
                File.Delete(srcFile);
                File.Move(newFile, srcFile);
                FileEx.SetAttributes(srcFile, FileAttributes.Normal);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Provides <see cref="string"/> values of line separator characters.
        /// </summary>
        public static class NewLineFormats
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
}
