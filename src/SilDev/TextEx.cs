#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TextEx.cs
// Version:  2017-05-18 14:21
// 
// Copyright (c) 2017, Si13n7 Developments (r)
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
        ///     Provides <see cref="string"/> values of line separator characters.
        /// </summary>
        public static class NewLineFormats
        {
            public const string CarriageReturn = "\u000d";
            public const string FormFeed = "\u000c";
            public const string LineFeed = "\u000a";
            public const string LineSeparator = "\u2028";
            public const string NextLine = "\u0085";
            public const string ParagraphSeparator = "\u2029";
            public const string VerticalTab = "\u000b";
            public const string WindowsDefault = "\u000d\u000a";
        }

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
            var current = text.Where(IsLineSeparator).Distinct().ToArray();
            var newText = text.Replace(NewLineFormats.WindowsDefault, NewLineFormats.LineFeed);
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
                var srcHash = new Crypto.Md5().EncryptFile(srcFile);
                var newFile = srcFile + ".new";
                File.Create(newFile).Close();
                Data.SetAttributes(newFile, FileAttributes.Hidden);
                using (var sr = new StreamReader(srcFile))
                    using (var sw = new StreamWriter(newFile, true, encoding))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                            sw.WriteLine(line);
                    }
                if (srcHash.Equals(new Crypto.Md5().EncryptFile(newFile)))
                {
                    File.Delete(newFile);
                    return true;
                }
                File.Delete(srcFile);
                File.Move(newFile, srcFile);
                Data.SetAttributes(srcFile, FileAttributes.Normal);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }
    }
}
