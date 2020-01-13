#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TextEx.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
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
    using QuickWmi;

    /// <summary>
    ///     Provides static methods for converting or reorganizing of data.
    /// </summary>
    public static class TextEx
    {
        private static object _defaultEncoding;

        /// <summary>
        ///     Gets the character encoding of the operating system.
        /// </summary>
        public static Encoding DefaultEncoding
        {
            get
            {
                if (_defaultEncoding != default)
                    return (Encoding)_defaultEncoding;
                if (!int.TryParse(Win32_OperatingSystem.CodeSet, out var codePage))
                    codePage = 1252;
                _defaultEncoding = Encoding.GetEncoding(codePage);
                return (Encoding)_defaultEncoding;
            }
        }

        /// <summary>
        ///     Indicates whether the specified character is categorized as a line
        ///     separator character.
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
        ///     Indicates whether the specified character is categorized as an ASCII
        ///     character.
        /// </summary>
        /// <param name="ch">
        ///     The character to evaluate.
        /// </param>
        public static bool IsAscii(this char ch) =>
            ch <= sbyte.MaxValue;

        /// <summary>
        ///     Converts the current <see cref="StringNewLineFormats"/> of the specified
        ///     <see cref="string"/> to another format.
        /// </summary>
        /// <param name="text">
        ///     The text to change.
        /// </param>
        /// <param name="newLineFormat">
        ///     The new format to be applied.
        /// </param>
        public static string FormatNewLine(string text, string newLineFormat = StringNewLineFormats.WindowsDefault)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var newText = text.Replace(StringNewLineFormats.WindowsDefault, StringNewLineFormats.LineFeed);
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
        ///     was found. If the value is NULL it returns the Windows-1252
        ///     <see cref="Encoding"/> format.
        /// </param>
        [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
        public static Encoding GetEncoding(string file, Encoding defEncoding = default)
        {
            var path = PathEx.Combine(file);
            var encoding = defEncoding ?? DefaultEncoding;
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
        ///     Changes the character encoding of the specified file. This function
        ///     supports big files as well.
        /// </summary>
        /// <param name="file">
        ///     The file to change.
        /// </param>
        /// <param name="encoding">
        ///     The new character encoding. If the value is NULL it uses the Windows-1252
        ///     <see cref="Encoding"/> format.
        /// </param>
        public static bool ChangeEncoding(string file, Encoding encoding = default)
        {
            if (string.IsNullOrEmpty(file))
                return false;
            var srcFile = PathEx.Combine(file);
            if (!File.Exists(srcFile))
                return false;
            if (encoding == null)
                encoding = DefaultEncoding;
            if (encoding.Equals(GetEncoding(srcFile)))
                return true;
            try
            {
                var srcDir = Path.GetDirectoryName(srcFile);
                var newFile = PathEx.Combine(srcDir, Path.GetRandomFileName());
                File.Create(newFile).Close();
                using (var sr = new StreamReader(srcFile))
                {
                    using var sw = new StreamWriter(newFile, true, encoding);
                    int i;
                    var ca = new char[4096];
                    while ((i = sr.Read(ca, 0, ca.Length)) > 0)
                        sw.Write(ca, 0, i);
                }
                if (!FileEx.ContentIsEqual(srcFile, newFile))
                    return FileEx.Move(newFile, srcFile, true);
                FileEx.TryDelete(newFile);
                return false;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }
    }
}
