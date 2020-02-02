#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: EncodingEx.cs
// Version:  2020-01-29 17:36
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    ///     Provides static functions based on the <see cref="Encoding"/> class.
    /// </summary>
    public static class EncodingEx
    {
        private static volatile Encoding _ansi;
        private static volatile Encoding _utf8NoBom;

        /// <summary>
        ///     Gets the ANSI (Windows-1252) character encoding.
        /// </summary>
        public static Encoding Ansi => _ansi ??= Encoding.GetEncoding(1252);

        /// <summary>
        ///     Gets the UTF-8 character encoding without BOM.
        /// </summary>
        public static Encoding Utf8NoBom => _utf8NoBom ??= new UTF8Encoding(false);

        /// <summary>
        ///     Gets the character encoding of the specified file.
        /// </summary>
        /// <param name="file">
        ///     The file to check.
        /// </param>
        /// <param name="defEncoding">
        ///     The default character encoding, which is returned if no character encoding
        ///     was found. If <see langword="null"/>, <see cref="Ansi"/> is
        ///     used.
        /// </param>
        public static Encoding GetEncoding(string file, Encoding defEncoding = default)
        {
            var path = PathEx.Combine(file);
            var encoding = defEncoding ?? Ansi;
            if (!File.Exists(path))
                return encoding;
            using var sr = new StreamReader(file, true);
            sr.Peek();
            return sr.CurrentEncoding;
        }

        /// <summary>
        ///     Changes the character encoding of the specified file. This function
        ///     supports big files as well.
        /// </summary>
        /// <param name="file">
        ///     The file to change.
        /// </param>
        /// <param name="encoding">
        ///     The new character encoding. If <see langword="null"/>,
        ///     <see cref="Ansi"/> is used.
        /// </param>
        public static bool ChangeEncoding(string file, Encoding encoding = default)
        {
            if (string.IsNullOrEmpty(file))
                return false;
            var srcFile = PathEx.Combine(file);
            if (!File.Exists(srcFile))
                return false;
            encoding ??= Ansi;
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
