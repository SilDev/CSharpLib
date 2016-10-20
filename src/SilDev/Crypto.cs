#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Crypto.cs
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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static class Crypto
    {
        #region Base64

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64"/> class.
        /// </summary>
        public class Base64
        {
            /// <summary>
            ///     The length of lines.
            /// </summary>
            public uint LineLength { get; set; }

            /// <summary>
            ///     The prefix mark.
            /// </summary>
            public virtual string PrefixMark { get; set; }

            /// <summary>
            ///     The suffix mark.
            /// </summary>
            public virtual string SuffixMark { get; set; }

            /// <summary>
            ///     Gets the encoded result of the last object.
            /// </summary>
            public string LastEncodedResult { get; protected set; }

            /// <summary>
            ///     Gets the decoded result of the last object.
            /// </summary>
            public byte[] LastDecodedResult { get; protected set; }

            protected string EncodeFilters(string input, string prefixMark, string suffixMark, uint lineLength)
            {
                var s = input;
                if (!string.IsNullOrEmpty(prefixMark) && !string.IsNullOrEmpty(prefixMark))
                {
                    var prefix = prefixMark;
                    var suffix = suffixMark;
                    if (lineLength > 0)
                    {
                        prefix = $"{prefix}{Environment.NewLine}";
                        suffix = $"{Environment.NewLine}{suffix}";
                    }
                    s = $"{prefix}{s}{suffix}";
                }
                if (!((lineLength > 1) & (s.Length > lineLength)))
                    return s;
                var i = 0;
                s = string.Join(Environment.NewLine, s.ToLookup(c => Math.Floor(i++ / (double)lineLength)).Select(e => new string(e.ToArray())));
                return s;
            }

            protected string DecodeFilters(string input, string prefixMark, string suffixMark)
            {
                var s = input;
                if (!string.IsNullOrEmpty(prefixMark) && !string.IsNullOrEmpty(suffixMark))
                {
                    if (s.StartsWith(prefixMark))
                        s = s.Substring(prefixMark.Length);
                    if (s.EndsWith(suffixMark))
                        s = s.Substring(0, s.Length - suffixMark.Length);
                }
                if (s.Contains('\r') || s.Contains('\n'))
                    s = string.Concat(s.ToCharArray().Where(c => c != '\r' && c != '\n').ToArray());
                return s;
            }

            /// <summary>
            ///     Encodes the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encode.
            /// </param>
            /// <param name="prefixMark">
            ///     The prefix mark.
            /// </param>
            /// <param name="suffixMark">
            ///     The suffix mark.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            public virtual string EncodeByteArray(byte[] bytes, string prefixMark = null, string suffixMark = null, uint lineLength = 0)
            {
                try
                {
                    if (!string.IsNullOrEmpty(prefixMark) && !string.IsNullOrEmpty(suffixMark))
                    {
                        PrefixMark = prefixMark;
                        SuffixMark = suffixMark;
                    }
                    if (lineLength > 0)
                        LineLength = lineLength;
                    LastEncodedResult = Convert.ToBase64String(bytes);
                    LastEncodedResult = EncodeFilters(LastEncodedResult, null, null, LineLength);
                    LastEncodedResult = EncodeFilters(LastEncodedResult, PrefixMark, SuffixMark, (uint)(LineLength > 1 ? 1 : 0));
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    LastEncodedResult = string.Empty;
                }
                return LastEncodedResult;
            }

            /// <summary>
            ///     Decodes the specified sequence of bytes.
            /// </summary>
            /// <param name="code">
            ///     The string to decode.
            /// </param>
            /// <param name="prefixMark">
            ///     The prefix mark.
            /// </param>
            /// <param name="suffixMark">
            ///     The suffix mark.
            /// </param>
            public virtual byte[] DecodeByteArray(string code, string prefixMark = null, string suffixMark = null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(prefixMark) && !string.IsNullOrEmpty(suffixMark))
                    {
                        PrefixMark = prefixMark;
                        SuffixMark = suffixMark;
                    }
                    code = DecodeFilters(code, PrefixMark, SuffixMark);
                    LastDecodedResult = Convert.FromBase64String(code);
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    LastDecodedResult = null;
                }
                return LastDecodedResult;
            }

            /// <summary>
            ///     Encodes the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encode.
            /// </param>
            /// <param name="prefixMark">
            ///     The prefix mark.
            /// </param>
            /// <param name="suffixMark">
            ///     The suffix mark.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            public string EncodeString(string text, string prefixMark = null, string suffixMark = null, uint lineLength = 0)
            {
                try
                {
                    var ba = text.ToByteArray();
                    return EncodeByteArray(ba, prefixMark, suffixMark, lineLength) ?? string.Empty;
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Encodes the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encode.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            public string EncodeString(string text, uint lineLength) =>
                EncodeString(text, null, null, lineLength);

            /// <summary>
            ///     Decodes the specified string.
            /// </summary>
            /// <param name="code">
            ///     The string to decode.
            /// </param>
            /// <param name="prefixMark">
            ///     The prefix mark.
            /// </param>
            /// <param name="suffixMark">
            ///     The suffix mark.
            /// </param>
            public string DecodeString(string code, string prefixMark = null, string suffixMark = null)
            {
                try
                {
                    var ba = DecodeByteArray(code, prefixMark, suffixMark);
                    return Encoding.UTF8.GetString(ba);
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Encodes the specified file.
            /// </summary>
            /// <param name="path">
            ///     The full path of the file to encode (environment variables are accepted).
            /// </param>
            /// <param name="prefixMark">
            ///     The prefix mark.
            /// </param>
            /// <param name="suffixMark">
            ///     The suffix mark.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            public string EncodeFile(string path, string prefixMark = null, string suffixMark = null, uint lineLength = 0)
            {
                try
                {
                    var s = PathEx.Combine(path);
                    if (!File.Exists(s))
                        throw new FileNotFoundException();
                    byte[] ba;
                    using (var fs = new FileStream(s, FileMode.Open))
                    {
                        ba = new byte[fs.Length];
                        fs.Read(ba, 0, (int)fs.Length);
                    }
                    return EncodeByteArray(ba, prefixMark, suffixMark, lineLength);
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Encodes the specified file.
            /// </summary>
            /// <param name="path">
            ///     The full path of the file to encode (environment variables are accepted).
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            public string EncodeFile(string path, uint lineLength) =>
                EncodeFile(path, null, null, lineLength);

            /// <summary>
            ///     Decodes the specified sequence of bytes which presents a file.
            /// </summary>
            /// <param name="code">
            ///     The string to decode.
            /// </param>
            /// <param name="prefixMark">
            ///     The prefix mark.
            /// </param>
            /// <param name="suffixMark">
            ///     The suffix mark.
            /// </param>
            public byte[] DecodeFile(string code, string prefixMark = null, string suffixMark = null) =>
                DecodeByteArray(code, prefixMark, suffixMark);
        }

        /// <summary>
        ///     Encodes this sequence of bytes to a sequence of base-64 digits.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="lineLength">
        ///     The length of lines.
        /// </param>
        public static string EncodeToBase64(this byte[] bytes, string prefixMark = null, string suffixMark = null, uint lineLength = 0) =>
            new Base64().EncodeByteArray(bytes, prefixMark, suffixMark, lineLength);

        /// <summary>
        ///     Decodes this sequence of base-64 digits back to a sequence of bytes.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        public static byte[] DecodeByteArrayFromBase64(this string code, string prefixMark = null, string suffixMark = null) =>
            new Base64().DecodeByteArray(code, prefixMark, suffixMark);

        /// <summary>
        ///     Encodes this string to a sequence of base-64 digits.
        /// </summary>
        /// <param name="text">
        ///     The string to encode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="lineLength">
        ///     The length of lines.
        /// </param>
        public static string EncodeToBase64(this string text, string prefixMark = null, string suffixMark = null, uint lineLength = 0) =>
            new Base64().EncodeString(text, prefixMark, suffixMark, lineLength);

        /// <summary>
        ///     Decodes this sequence of base-64 digits back to string.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        public static string DecodeStringFromBase64(this string code, string prefixMark = null, string suffixMark = null) =>
            new Base64().DecodeString(code, prefixMark, suffixMark);

        /// <summary>
        ///     Encodes the specifed file to a sequence of base-64 digits.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encode (environment variables are accepted).
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="lineLength">
        ///     The length of lines.
        /// </param>
        public static string EncodeFileToBase64(string path, string prefixMark = null, string suffixMark = null, uint lineLength = 0) =>
            new Base64().EncodeFile(path, prefixMark, suffixMark, lineLength);

        /// <summary>
        ///     Decodes the specifed sequence of base-64 digits back to a sequence of bytes which presents a file.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        public static byte[] DecodeFileFromBase64(string code, string prefixMark = null, string suffixMark = null) =>
            new Base64().DecodeFile(code, prefixMark, suffixMark);

        #endregion

        #region Base85

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base85"/> class.
        /// </summary>
        public class Base85 : Base64
        {
            private static readonly uint[] P85 =
            {
                85 * 85 * 85 * 85,
                85 * 85 * 85,
                85 * 85,
                85,
                1
            };

            private static readonly byte[] EncodeBlock = new byte[5], DecodeBlock = new byte[4];

            /// <summary>
            ///     The prefix mark.
            /// </summary>
            public override string PrefixMark { get; set; } = "<~";

            /// <summary>
            ///     The suffix mark.
            /// </summary>
            public override string SuffixMark { get; set; } = "~>";

            /// <summary>
            ///     Encodes the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encode.
            /// </param>
            /// <param name="prefixMark">
            ///     The prefix mark.
            /// </param>
            /// <param name="suffixMark">
            ///     The suffix mark.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            [SuppressMessage("ReSharper", "OptionalParameterHierarchyMismatch")]
            public override string EncodeByteArray(byte[] bytes, string prefixMark = "<~", string suffixMark = "~>", uint lineLength = 0)
            {
                try
                {
                    if (prefixMark != "<~" && suffixMark != "~>")
                    {
                        PrefixMark = prefixMark;
                        SuffixMark = suffixMark;
                    }
                    if (lineLength > 0)
                        LineLength = lineLength;
                    var sb = new StringBuilder();
                    uint t = 0;
                    var n = 0;
                    foreach (var b in bytes)
                    {
                        if (n + 1 < DecodeBlock.Length)
                        {
                            t |= (uint)(b << (24 - n * 8));
                            n++;
                            continue;
                        }
                        t |= b;
                        if (t == 0)
                            sb.Append((char)0x7a);
                        else
                        {
                            for (var i = EncodeBlock.Length - 1; i >= 0; i--)
                            {
                                EncodeBlock[i] = (byte)(t % 85 + 33);
                                t /= 85;
                            }
                            foreach (var eb in EncodeBlock)
                                sb.Append((char)eb);
                        }
                        t = 0;
                        n = 0;
                    }
                    if (n > 0)
                    {
                        for (var i = EncodeBlock.Length - 1; i >= 0; i--)
                        {
                            EncodeBlock[i] = (byte)(t % 85 + 33);
                            t /= 85;
                        }
                        for (var i = 0; i <= n; i++)
                            sb.Append((char)EncodeBlock[i]);
                    }
                    LastEncodedResult = sb.ToString();
                    LastEncodedResult = EncodeFilters(LastEncodedResult, null, null, LineLength);
                    LastEncodedResult = EncodeFilters(LastEncodedResult, PrefixMark, SuffixMark, (uint)(LineLength > 1 ? 1 : 0));
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    LastEncodedResult = string.Empty;
                }
                return LastEncodedResult;
            }

            /// <summary>
            ///     Decodes the specified sequence of bytes.
            /// </summary>
            /// <param name="code">
            ///     The string to decode.
            /// </param>
            /// <param name="prefixMark">
            ///     The prefix mark.
            /// </param>
            /// <param name="suffixMark">
            ///     The suffix mark.
            /// </param>
            [SuppressMessage("ReSharper", "OptionalParameterHierarchyMismatch")]
            public override byte[] DecodeByteArray(string code, string prefixMark = "<~", string suffixMark = "~>")
            {
                try
                {
                    if (!string.IsNullOrEmpty(prefixMark) && !string.IsNullOrEmpty(suffixMark))
                    {
                        PrefixMark = prefixMark;
                        SuffixMark = suffixMark;
                    }
                    code = DecodeFilters(code, PrefixMark, SuffixMark);
                    using (var ms = new MemoryStream())
                    {
                        char[] ca =
                        {
                            (char)0x0, (char)0x8, (char)0x9,
                            (char)0xa, (char)0xc, (char)0xd
                        };
                        var n = 0;
                        uint t = 0;
                        foreach (var c in code)
                        {
                            if (c == (char)0x7a)
                            {
                                if (n != 0)
                                    throw new ArgumentException();
                                for (var i = 0; i < 4; i++)
                                    DecodeBlock[i] = 0;
                                ms.Write(DecodeBlock, 0, DecodeBlock.Length);
                                continue;
                            }
                            if (ca.Contains(c))
                                continue;
                            if (c < (char)0x21 || c > (char)0x75)
                                throw new ArgumentOutOfRangeException();
                            t += (uint)((c - 33) * P85[n]);
                            n++;
                            if (n != EncodeBlock.Length)
                                continue;
                            for (var i = 0; i < DecodeBlock.Length; i++)
                                DecodeBlock[i] = (byte)(t >> (24 - i * 8));
                            ms.Write(DecodeBlock, 0, DecodeBlock.Length);
                            t = 0;
                            n = 0;
                        }
                        if (n != 0)
                        {
                            if (n == 1)
                                throw new NotSupportedException();
                            n--;
                            t += P85[n];
                            for (var i = 0; i < n; i++)
                                DecodeBlock[i] = (byte)(t >> (24 - i * 8));
                            for (var i = 0; i < n; i++)
                                ms.WriteByte(DecodeBlock[i]);
                        }
                        LastDecodedResult = ms.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    LastDecodedResult = null;
                }
                return LastDecodedResult;
            }
        }

        /// <summary>
        ///     Encodes this sequence of bytes to a sequence of base-85 digits.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="lineLength">
        ///     The length of lines.
        /// </param>
        public static string EncodeToBase85(this byte[] bytes, string prefixMark = "<~", string suffixMark = "~>", uint lineLength = 0) =>
            new Base85().EncodeByteArray(bytes, prefixMark, suffixMark, lineLength);

        /// <summary>
        ///     Decodes this sequence of base-85 digits back to a sequence of bytes.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        public static byte[] DecodeByteArrayFromBase85(this string code, string prefixMark = "<~", string suffixMark = "~>") =>
            new Base85().DecodeByteArray(code, prefixMark, suffixMark);

        /// <summary>
        ///     Encodes this string to a sequence of base-85 digits.
        /// </summary>
        /// <param name="text">
        ///     The string to encode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="lineLength">
        ///     The length of lines.
        /// </param>
        public static string EncodeToBase85(this string text, string prefixMark = "<~", string suffixMark = "~>", uint lineLength = 0) =>
            new Base85().EncodeString(text, prefixMark, suffixMark, lineLength);

        /// <summary>
        ///     Decodes this sequence of base-85 digits back to string.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        public static string DecodeStringFromBase85(this string code, string prefixMark = "<~", string suffixMark = "~>") =>
            new Base85().DecodeString(code, prefixMark, suffixMark);

        /// <summary>
        ///     Encodes the specifed file to a sequence of base-85 digits.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encode (environment variables are accepted).
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="lineLength">
        ///     The length of lines.
        /// </param>
        public static string EncodeFileToBase85(string path, string prefixMark = "<~", string suffixMark = "~>", uint lineLength = 0) =>
            new Base85().EncodeFile(path, prefixMark, suffixMark, lineLength);

        /// <summary>
        ///     Decodes the specifed sequence of base-85 digits back to a sequence of bytes which presents a file.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        public static byte[] DecodeFileFromBase85(string code, string prefixMark = "<~", string suffixMark = "~>") =>
            new Base85().DecodeFile(code, prefixMark, suffixMark);

        #endregion

        #region Base91

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base91"/> class.
        /// </summary>
        public class Base91 : Base64
        {
            private static readonly uint[] A91 =
            {
                0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49,
                0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, 0x50, 0x51, 0x52,
                0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x61,
                0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a,
                0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 0x70, 0x71, 0x72, 0x73,
                0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x30, 0x31,
                0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x21,
                0x23, 0x24, 0x25, 0x26, 0x28, 0x29, 0x2a, 0x2b, 0x2c,
                0x2d, 0x2e, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f, 0x40,
                0x5b, 0x5d, 0x5e, 0x5f, 0x60, 0x7b, 0x7c, 0x7d, 0x7e,
                0x22
            };

            private Dictionary<byte, int> _decodeTable;
            private char[] _defaultEncodeTable, _encodeTable;

            /// <summary>
            ///     Gets the default encode table.
            /// </summary>
            public char[] DefaultEncodeTable
            {
                get
                {
                    if (_defaultEncodeTable != null)
                        return _defaultEncodeTable;
                    var sb = new StringBuilder();
                    foreach (var i in A91)
                        sb.Append((char)i);
                    _defaultEncodeTable = sb.ToString().ToCharArray();
                    return _defaultEncodeTable;
                }
            }

            /// <summary>
            ///     Gets or sets the encode table.
            /// </summary>
            public char[] EncodeTable
            {
                get { return _encodeTable ?? (_encodeTable = DefaultEncodeTable); }
                set
                {
                    try
                    {
                        value = value.Distinct().ToArray();
                        if (value.Length < 91)
                            throw new ArgumentException();
                        if (value.Length > 91)
                            value = new List<char>(value).GetRange(0, 91).ToArray();
                        _encodeTable = value;
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                        _encodeTable = DefaultEncodeTable;
                    }
                }
            }

            private void InitializeTables()
            {
                if (_encodeTable == null)
                    _encodeTable = EncodeTable;
                _decodeTable = new Dictionary<byte, int>();
                for (var i = 0; i < 255; i++)
                    _decodeTable[(byte)i] = -1;
                for (var i = 0; i < _encodeTable.Length; i++)
                    _decodeTable[(byte)_encodeTable[i]] = i;
            }

            /// <summary>
            ///     Encodes the specified sequence of bytes to a sequence of base-91 digits.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encode.
            /// </param>
            /// <param name="prefixMark">
            ///     The prefix mark.
            /// </param>
            /// <param name="suffixMark">
            ///     The suffix mark.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            public override string EncodeByteArray(byte[] bytes, string prefixMark = null, string suffixMark = null, uint lineLength = 0)
            {
                try
                {
                    if (!string.IsNullOrEmpty(prefixMark) && !string.IsNullOrEmpty(suffixMark))
                    {
                        PrefixMark = prefixMark;
                        SuffixMark = suffixMark;
                    }
                    if (lineLength > 0)
                        LineLength = lineLength;
                    InitializeTables();
                    var sb = new StringBuilder();
                    int[] ia = { 0, 0, 0 };
                    foreach (var b in bytes)
                    {
                        ia[0] |= b << ia[1];
                        ia[1] += 8;
                        if (ia[1] < 14)
                            continue;
                        ia[2] = ia[0] & 8191;
                        if (ia[2] > 88)
                        {
                            ia[1] -= 13;
                            ia[0] >>= 13;
                        }
                        else
                        {
                            ia[2] = ia[0] & 16383;
                            ia[1] -= 14;
                            ia[0] >>= 14;
                        }
                        sb.Append(_encodeTable[ia[2] % 91]);
                        sb.Append(_encodeTable[ia[2] / 91]);
                    }
                    if (ia[1] != 0)
                    {
                        sb.Append(_encodeTable[ia[0] % 91]);
                        if (ia[1] >= 8 || ia[0] >= 91)
                            sb.Append(_encodeTable[ia[0] / 91]);
                    }
                    LastEncodedResult = sb.ToString();
                    LastEncodedResult = EncodeFilters(LastEncodedResult, null, null, LineLength);
                    LastEncodedResult = EncodeFilters(LastEncodedResult, PrefixMark, SuffixMark, (uint)(LineLength > 1 ? 1 : 0));
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    LastEncodedResult = string.Empty;
                }
                return LastEncodedResult;
            }

            /// <summary>
            ///     Decodes the specified sequence of base-91 digits back to a sequence of bytes.
            /// </summary>
            /// <param name="code">
            ///     The string to decode.
            /// </param>
            /// <param name="prefixMark">
            ///     The prefix mark.
            /// </param>
            /// <param name="suffixMark">
            ///     The suffix mark.
            /// </param>
            public override byte[] DecodeByteArray(string code, string prefixMark = null, string suffixMark = null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(prefixMark) && !string.IsNullOrEmpty(suffixMark))
                    {
                        PrefixMark = prefixMark;
                        SuffixMark = suffixMark;
                    }
                    code = DecodeFilters(code, PrefixMark, SuffixMark);
                    InitializeTables();
                    using (var ms = new MemoryStream())
                    {
                        int[] ia = { 0, -1, 0, 0 };
                        foreach (var c in code)
                        {
                            if (_encodeTable.Count(e => e == (byte)c) == 0)
                                throw new ArgumentOutOfRangeException();
                            ia[0] = _decodeTable[(byte)c];
                            if (ia[0] == -1)
                                continue;
                            if (ia[1] < 0)
                            {
                                ia[1] = ia[0];
                                continue;
                            }
                            ia[1] += ia[0] * 91;
                            ia[2] |= ia[1] << ia[3];
                            ia[3] += (ia[1] & 8191) > 88 ? 13 : 14;
                            do
                            {
                                ms.WriteByte((byte)(ia[2] & 255));
                                ia[2] >>= 8;
                                ia[3] -= 8;
                            }
                            while (ia[3] > 7);
                            ia[1] = -1;
                        }
                        if (ia[1] != -1)
                            ms.WriteByte((byte)((ia[2] | (ia[1] << ia[3])) & 255));
                        LastDecodedResult = ms.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    LastDecodedResult = null;
                }
                return LastDecodedResult;
            }
        }

        /// <summary>
        ///     Encodes this sequence of bytes to a sequence of base-91 digits.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="lineLength">
        ///     The length of lines.
        /// </param>
        public static string EncodeToBase91(this byte[] bytes, string prefixMark = null, string suffixMark = null, uint lineLength = 0) =>
            new Base91().EncodeByteArray(bytes, prefixMark, suffixMark, lineLength);

        /// <summary>
        ///     Decodes this sequence of base-91 digits back to a sequence of bytes.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        public static byte[] DecodeByteArrayFromBase91(this string code, string prefixMark = null, string suffixMark = null) =>
            new Base91().DecodeByteArray(code, prefixMark, suffixMark);

        /// <summary>
        ///     Encodes this string to a sequence of base-91 digits.
        /// </summary>
        /// <param name="text">
        ///     The string to encode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="lineLength">
        ///     The length of lines.
        /// </param>
        public static string EncodeToBase91(this string text, string prefixMark = null, string suffixMark = null, uint lineLength = 0) =>
            new Base91().EncodeString(text, prefixMark, suffixMark, lineLength);

        /// <summary>
        ///     Decodes this sequence of base-91 digits back to string.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        public static string DecodeStringFromBase91(this string code, string prefixMark = null, string suffixMark = null) =>
            new Base91().DecodeString(code, prefixMark, suffixMark);

        /// <summary>
        ///     Encodes the specifed file to a sequence of base-91 digits.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encode (environment variables are accepted).
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="lineLength">
        ///     The length of lines.
        /// </param>
        public static string EncodeFileToBase91(string path, string prefixMark = null, string suffixMark = null, uint lineLength = 0) =>
            new Base91().EncodeFile(path, prefixMark, suffixMark, lineLength);

        /// <summary>
        ///     Decodes the specifed sequence of base-91 digits back to a sequence of bytes which presents a file.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        public static byte[] DecodeFileFromBase91(string code, string prefixMark = null, string suffixMark = null) =>
            new Base91().DecodeFile(code, prefixMark, suffixMark);

        #endregion

        #region Advanced Encryption Standard

        /// <summary>
        ///     Initializes a new instance of the <see cref="Aes"/> class.
        /// </summary>
        public static class Aes
        {
            /// <summary>
            ///     Provides enumerated bits of the key size.
            /// </summary>
            public enum KeySize
            {
                /// <summary>
                ///     128 bits.
                /// </summary>
                Aes128 = 128,

                /// <summary>
                ///     192 bits.
                /// </summary>
                Aes192 = 192,

                /// <summary>
                ///     256 bits.
                /// </summary>
                Aes256 = 256
            }

            /// <summary>
            ///     Encrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt.
            /// </param>
            /// <param name="password">
            ///     The sequence of bytes which is used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which is used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] EncryptByteArray(byte[] bytes, byte[] password, byte[] salt = null, KeySize keySize = KeySize.Aes256)
            {
                try
                {
                    byte[] ba;
                    using (var rm = new RijndaelManaged())
                    {
                        rm.BlockSize = 128;
                        rm.KeySize = (int)keySize;
                        using (var db = new Rfc2898DeriveBytes(password, salt ?? password.EncryptToSha512().ToByteArray(), 1000))
                        {
                            rm.Key = db.GetBytes(rm.KeySize / 8);
                            rm.IV = db.GetBytes(rm.BlockSize / 8);
                        }
                        rm.Mode = CipherMode.CBC;
                        var ms = new MemoryStream();
                        using (var cs = new CryptoStream(ms, rm.CreateEncryptor(), CryptoStreamMode.Write))
                            cs.Write(bytes, 0, bytes.Length);
                        ba = ms.ToArray();
                    }
                    return ba;
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return null;
                }
            }

            /// <summary>
            ///     Encrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt.
            /// </param>
            /// <param name="password">
            ///     The string which is used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which is used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] EncryptByteArray(byte[] bytes, string password, byte[] salt, KeySize keySize = KeySize.Aes256) =>
                EncryptByteArray(bytes, password.ToByteArray(), salt, keySize);

            /// <summary>
            ///     Encrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt.
            /// </param>
            /// <param name="password">
            ///     The string which is used as password.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] EncryptByteArray(byte[] bytes, string password, KeySize keySize = KeySize.Aes256) =>
                EncryptByteArray(bytes, password, null, keySize);

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            /// <param name="password">
            ///     The sequence of bytes which is used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which is used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] EncryptString(string text, byte[] password, byte[] salt = null, KeySize keySize = KeySize.Aes256) =>
                EncryptByteArray(text.ToByteArray(), password, salt, keySize);

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            /// <param name="password">
            ///     The string which is used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which is used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] EncryptString(string text, string password, byte[] salt, KeySize keySize = KeySize.Aes256) =>
                EncryptString(text, password.ToByteArray(), salt, keySize);

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            /// <param name="password">
            ///     The string which is used as password.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] EncryptString(string text, string password, KeySize keySize = KeySize.Aes256) =>
                EncryptString(text, password.ToByteArray(), null, keySize);

            /// <summary>
            ///     Encrypts the specified file.
            /// </summary>
            /// <param name="path">
            ///     The full path of the file to encrypt.
            /// </param>
            /// <param name="password">
            ///     The sequence of bytes which is used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which is used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] EncryptFile(string path, byte[] password, byte[] salt = null, KeySize keySize = KeySize.Aes256)
            {
                try
                {
                    var s = PathEx.Combine(path);
                    if (!File.Exists(s))
                        throw new FileNotFoundException();
                    var ba = File.ReadAllBytes(s);
                    return EncryptByteArray(ba, password, salt, keySize);
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return null;
                }
            }

            /// <summary>
            ///     Encrypts the specified file.
            /// </summary>
            /// <param name="path">
            ///     The full path of the file.
            /// </param>
            /// <param name="password">
            ///     The string which is used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which is used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] EncryptFile(string path, string password, byte[] salt, KeySize keySize = KeySize.Aes256) =>
                EncryptFile(path, password.ToByteArray(), salt, keySize);

            /// <summary>
            ///     Encrypts the specified file.
            /// </summary>
            /// <param name="path">
            ///     The full path of the file.
            /// </param>
            /// <param name="password">
            ///     The sequence of bytes which is used as password.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] EncryptFile(string path, string password, KeySize keySize = KeySize.Aes256) =>
                EncryptFile(path, password.ToByteArray(), null, keySize);

            /// <summary>
            ///     Decrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="code">
            ///     The sequence of bytes to decrypt.
            /// </param>
            /// <param name="password">
            ///     The sequence of bytes which was used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which was used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] DecryptByteArray(byte[] code, byte[] password, byte[] salt = null, KeySize keySize = KeySize.Aes256)
            {
                try
                {
                    byte[] ba;
                    using (var rm = new RijndaelManaged())
                    {
                        rm.BlockSize = 128;
                        rm.KeySize = (int)keySize;
                        using (var db = new Rfc2898DeriveBytes(password, salt ?? password.EncryptToSha512().ToByteArray(), 1000))
                        {
                            rm.Key = db.GetBytes(rm.KeySize / 8);
                            rm.IV = db.GetBytes(rm.BlockSize / 8);
                        }
                        rm.Mode = CipherMode.CBC;
                        var ms = new MemoryStream();
                        using (var cs = new CryptoStream(ms, rm.CreateDecryptor(), CryptoStreamMode.Write))
                            cs.Write(code, 0, code.Length);
                        ba = ms.ToArray();
                    }
                    return ba;
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return null;
                }
            }

            /// <summary>
            ///     Decrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="code">
            ///     The sequence of bytes to decrypt.
            /// </param>
            /// <param name="password">
            ///     The string which was used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which was used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] DecryptByteArray(byte[] code, string password, byte[] salt, KeySize keySize = KeySize.Aes256) =>
                DecryptByteArray(code, password.ToByteArray(), salt, keySize);

            /// <summary>
            ///     Decrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="code">
            ///     The sequence of bytes to decrypt.
            /// </param>
            /// <param name="password">
            ///     The string which was used as password.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] DecryptByteArray(byte[] code, string password, KeySize keySize = KeySize.Aes256) =>
                DecryptByteArray(code, password, null, keySize);

            /// <summary>
            ///     Decrypts the specified hexadecimal sequence which represents a enctrypted sequence of bytes.
            /// </summary>
            /// <param name="code">
            ///     The hexadecimal sequence to decrypt.
            /// </param>
            /// <param name="password">
            ///     The sequence of bytes which was used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which was used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] DecryptString(string code, byte[] password, byte[] salt = null, KeySize keySize = KeySize.Aes256) =>
                DecryptByteArray(code.FromHexStringToByteArray(), password, salt, keySize);

            /// <summary>
            ///     Decrypts the specified hexadecimal sequence which represents a enctrypted sequence of bytes.
            /// </summary>
            /// <param name="code">
            ///     The hexadecimal sequence to decrypt.
            /// </param>
            /// <param name="password">
            ///     The string which was used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which was used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] DecryptString(string code, string password, byte[] salt, KeySize keySize = KeySize.Aes256) =>
                DecryptString(code, password.ToByteArray(), salt, keySize);

            /// <summary>
            ///     Decrypts the specified hexadecimal sequence which represents a enctrypted sequence of bytes.
            /// </summary>
            /// <param name="code">
            ///     The hexadecimal sequence to decrypt.
            /// </param>
            /// <param name="password">
            ///     The string which was used as password.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] DecryptString(string code, string password, KeySize keySize = KeySize.Aes256) =>
                DecryptString(code, password.ToByteArray(), null, keySize);

            /// <summary>
            ///     Decrypts the specified file.
            /// </summary>
            /// <param name="path">
            ///     The full path of the file to decrypt.
            /// </param>
            /// <param name="password">
            ///     The sequence of bytes which was used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which was used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] DecryptFile(string path, byte[] password, byte[] salt = null, KeySize keySize = KeySize.Aes256)
            {
                try
                {
                    var s = PathEx.Combine(path);
                    if (!File.Exists(s))
                        throw new FileNotFoundException();
                    var ba = File.ReadAllBytes(s);
                    return DecryptByteArray(ba, password, salt, keySize);
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return null;
                }
            }

            /// <summary>
            ///     Decrypts the specified file.
            /// </summary>
            /// <param name="path">
            ///     The full path of the file to decrypt.
            /// </param>
            /// <param name="password">
            ///     The string which was used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which was used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] DecryptFile(string path, string password, byte[] salt, KeySize keySize = KeySize.Aes256) =>
                DecryptFile(path, password.ToByteArray(), salt, keySize);

            /// <summary>
            ///     Decrypts the specified file.
            /// </summary>
            /// <param name="path">
            ///     The full path of the file to decrypt.
            /// </param>
            /// <param name="password">
            ///     The string which was used as password.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] DecryptFile(string path, string password, KeySize keySize = KeySize.Aes256) =>
                DecryptFile(path, password.ToByteArray(), null, keySize);
        }

        /// <summary>
        ///     Encrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] EncryptToAes128(this byte[] bytes, string password) =>
            Aes.EncryptByteArray(bytes, password, Aes.KeySize.Aes128);

        /// <summary>
        ///     Encrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to encrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] EncryptToAes128(this string text, string password) =>
            Aes.EncryptString(text, password, Aes.KeySize.Aes128);

        /// <summary>
        ///     Encrypts this file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] EncryptFileToAes128(this string path, string password) =>
            Aes.EncryptFile(path, password, Aes.KeySize.Aes128);

        /// <summary>
        ///     Decrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] DecryptFromAes128(this byte[] bytes, string password) =>
            Aes.DecryptByteArray(bytes, password, Aes.KeySize.Aes128);

        /// <summary>
        ///     Decrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] DecryptStringFromAes128(this string text, string password) =>
            Aes.DecryptString(text, password, Aes.KeySize.Aes128);

        /// <summary>
        ///     Decrypts this file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] DecryptFileFromAes128(this string path, string password) =>
            Aes.DecryptFile(path, password, Aes.KeySize.Aes128);

        /// <summary>
        ///     Encrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] EncryptToAes192(this byte[] bytes, string password) =>
            Aes.EncryptByteArray(bytes, password, Aes.KeySize.Aes192);

        /// <summary>
        ///     Encrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to encrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] EncryptToAes192(this string text, string password) =>
            Aes.EncryptString(text, password, Aes.KeySize.Aes192);

        /// <summary>
        ///     Encrypts this file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] EncryptFileToAes192(this string path, string password) =>
            Aes.EncryptFile(path, password, Aes.KeySize.Aes192);

        /// <summary>
        ///     Decrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] DecryptFromAes192(this byte[] bytes, string password) =>
            Aes.DecryptByteArray(bytes, password, Aes.KeySize.Aes192);

        /// <summary>
        ///     Decrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] DecryptStringFromAes192(this string text, string password) =>
            Aes.DecryptString(text, password, Aes.KeySize.Aes192);

        /// <summary>
        ///     Decrypts this file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] DecryptFileFromAes192(this string path, string password) =>
            Aes.DecryptFile(path, password, Aes.KeySize.Aes192);

        /// <summary>
        ///     Encrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] EncryptToAes256(this byte[] bytes, string password) =>
            Aes.EncryptByteArray(bytes, password);

        /// <summary>
        ///     Encrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to encrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] EncryptToAes256(this string text, string password) =>
            Aes.EncryptString(text, password);

        /// <summary>
        ///     Encrypts this file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] EncryptFileToAes256(this string path, string password) =>
            Aes.EncryptFile(path, password);

        /// <summary>
        ///     Decrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] DecryptFromAes256(this byte[] bytes, string password) =>
            Aes.DecryptByteArray(bytes, password);

        /// <summary>
        ///     Decrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] DecryptStringFromAes256(this string text, string password) =>
            Aes.DecryptString(text, password);

        /// <summary>
        ///     Decrypts this file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        public static byte[] DecryptFileFromAes256(this string path, string password) =>
            Aes.DecryptFile(path, password);

        #endregion

        #region Message-Digest 5

        /// <summary>
        ///     Initializes a new instance of the <see cref="Md5"/> class.
        /// </summary>
        public class Md5
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public virtual int HashLength => 32;

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public virtual string EncryptStream(Stream stream)
            {
                try
                {
                    byte[] ba;
                    using (var csp = new MD5CryptoServiceProvider())
                        ba = csp.ComputeHash(stream);
                    return ba.ToHexString();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Encrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt.
            /// </param>
            public string EncryptByteArray(byte[] bytes)
            {
                try
                {
                    string s;
                    using (var ms = new MemoryStream())
                    {
                        ms.Read(bytes, 0, bytes.Length);
                        s = EncryptStream(ms);
                    }
                    return s;
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public virtual string EncryptString(string text)
            {
                try
                {
                    var ba = text.ToByteArray();
                    using (var csp = MD5.Create())
                        ba = csp.ComputeHash(ba);
                    var s = BitConverter.ToString(ba);
                    return s.RemoveChar('-').ToLower();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Encrypts the specified file.
            /// </summary>
            /// <param name="path">
            ///     The full path of the file to encrypt.
            /// </param>
            public string EncryptFile(string path)
            {
                try
                {
                    var s = PathEx.Combine(path);
                    using (var fs = File.OpenRead(s))
                        s = EncryptStream(fs);
                    return s;
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///     Encrypts this stream.
        /// </summary>
        /// <param name="stream">
        ///     The stream to encrypt.
        /// </param>
        public static string EncryptToMd5(this Stream stream) =>
            new Md5().EncryptStream(stream);

        /// <summary>
        ///     Encrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encrypt.
        /// </param>
        public static string EncryptToMd5(this byte[] bytes) =>
            new Md5().EncryptByteArray(bytes);

        /// <summary>
        ///     Encrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to encrypt.
        /// </param>
        public static string EncryptToMd5(this string text) =>
            new Md5().EncryptString(text);

        /// <summary>
        ///     Encrypts this file.
        /// </summary>
        /// <param name="file">
        ///     The file to encrypt.
        /// </param>
        public static string EncryptFileToMd5(this FileInfo file) =>
            new Md5().EncryptFile(file.FullName);

        /// <summary>
        ///     Encrypts the specified file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encrypt (environment variables are accepted).
        /// </param>
        public static string EncryptFileToMd5(string path) =>
            new Md5().EncryptFile(path);

        #endregion

        #region Secure Hash Algorithm 1

        /// <summary>
        ///     Initializes a new instance of the <see cref="Sha1"/> class.
        /// </summary>
        public class Sha1 : Md5
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public override int HashLength => 40;

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override string EncryptStream(Stream stream)
            {
                try
                {
                    byte[] ba;
                    using (var csp = new SHA1CryptoServiceProvider())
                        ba = csp.ComputeHash(stream);
                    return ba.ToHexString();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text)
            {
                try
                {
                    var ba = text.ToByteArray();
                    using (var csp = SHA1.Create())
                        ba = csp.ComputeHash(ba);
                    var s = BitConverter.ToString(ba);
                    return s.RemoveChar('-').ToLower();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///     Encrypts this stream.
        /// </summary>
        /// <param name="stream">
        ///     The stream to encrypt.
        /// </param>
        public static string EncryptToSha1(this Stream stream) =>
            new Sha1().EncryptStream(stream);

        /// <summary>
        ///     Encrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encrypt.
        /// </param>
        public static string EncryptToSha1(this byte[] bytes) =>
            new Sha1().EncryptByteArray(bytes);

        /// <summary>
        ///     Encrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to encrypt.
        /// </param>
        public static string EncryptToSha1(this string text) =>
            new Sha1().EncryptString(text);

        /// <summary>
        ///     Encrypts this file.
        /// </summary>
        /// <param name="file">
        ///     The file to encrypt.
        /// </param>
        public static string EncryptFileToSha1(this FileInfo file) =>
            new Sha1().EncryptFile(file.FullName);

        /// <summary>
        ///     Encrypts the specified file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encrypt (environment variables are accepted).
        /// </param>
        public static string EncryptFileToSha1(string path) =>
            new Sha1().EncryptFile(path);

        #endregion

        #region Secure Hash Algorithm 2

        /// <summary>
        ///     Initializes a new instance of the <see cref="Sha256"/> class.
        /// </summary>
        public class Sha256 : Md5
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public override int HashLength => 64;

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override string EncryptStream(Stream stream)
            {
                try
                {
                    byte[] ba;
                    using (var csp = new SHA256CryptoServiceProvider())
                        ba = csp.ComputeHash(stream);
                    return ba.ToHexString();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text)
            {
                try
                {
                    var ba = text.ToByteArray();
                    using (var csp = SHA256.Create())
                        ba = csp.ComputeHash(ba);
                    var s = BitConverter.ToString(ba);
                    return s.RemoveChar('-').ToLower();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///     Encrypts this stream.
        /// </summary>
        /// <param name="stream">
        ///     The stream to encrypt.
        /// </param>
        public static string EncryptToSha256(this Stream stream) =>
            new Sha256().EncryptStream(stream);

        /// <summary>
        ///     Encrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encrypt.
        /// </param>
        public static string EncryptToSha256(this byte[] bytes) =>
            new Sha256().EncryptByteArray(bytes);

        /// <summary>
        ///     Encrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to encrypt.
        /// </param>
        public static string EncryptToSha256(this string text) =>
            new Sha256().EncryptString(text);

        /// <summary>
        ///     Encrypts this file.
        /// </summary>
        /// <param name="file">
        ///     The file to encrypt.
        /// </param>
        public static string EncryptFileToSha256(this FileInfo file) =>
            new Sha256().EncryptFile(file.FullName);

        /// <summary>
        ///     Encrypts the specified file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encrypt (environment variables are accepted).
        /// </param>
        public static string EncryptFileToSha256(string path) =>
            new Sha256().EncryptFile(path);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Sha384"/> class.
        /// </summary>
        public class Sha384 : Md5
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public override int HashLength => 96;

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override string EncryptStream(Stream stream)
            {
                try
                {
                    byte[] ba;
                    using (var csp = new SHA384CryptoServiceProvider())
                        ba = csp.ComputeHash(stream);
                    return ba.ToHexString();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text)
            {
                try
                {
                    var ba = text.ToByteArray();
                    using (var csp = SHA384.Create())
                        ba = csp.ComputeHash(ba);
                    var s = BitConverter.ToString(ba);
                    return s.RemoveChar('-').ToLower();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///     Encrypts this stream.
        /// </summary>
        /// <param name="stream">
        ///     The stream to encrypt.
        /// </param>
        public static string EncryptToSha384(this Stream stream) =>
            new Sha384().EncryptStream(stream);

        /// <summary>
        ///     Encrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encrypt.
        /// </param>
        public static string EncryptToSha384(this byte[] bytes) =>
            new Sha384().EncryptByteArray(bytes);

        /// <summary>
        ///     Encrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to encrypt.
        /// </param>
        public static string EncryptToSha384(this string text) =>
            new Sha384().EncryptString(text);

        /// <summary>
        ///     Encrypts this file.
        /// </summary>
        /// <param name="file">
        ///     The file to encrypt.
        /// </param>
        public static string EncryptFileToSha384(this FileInfo file) =>
            new Sha384().EncryptFile(file.FullName);

        /// <summary>
        ///     Encrypts the specified file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encrypt (environment variables are accepted).
        /// </param>
        public static string EncryptFileToSha384(string path) =>
            new Sha384().EncryptFile(path);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Sha512"/> class.
        /// </summary>
        public class Sha512 : Md5
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public override int HashLength => 128;

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override string EncryptStream(Stream stream)
            {
                try
                {
                    byte[] ba;
                    using (var csp = new SHA512CryptoServiceProvider())
                        ba = csp.ComputeHash(stream);
                    return ba.ToHexString();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text)
            {
                try
                {
                    var ba = text.ToByteArray();
                    using (var csp = SHA512.Create())
                        ba = csp.ComputeHash(ba);
                    var s = BitConverter.ToString(ba);
                    return s.RemoveChar('-').ToLower();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///     Encrypts this stream.
        /// </summary>
        /// <param name="stream">
        ///     The stream to encrypt.
        /// </param>
        public static string EncryptToSha512(this Stream stream) =>
            new Sha512().EncryptStream(stream);

        /// <summary>
        ///     Encrypts this sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encrypt.
        /// </param>
        public static string EncryptToSha512(this byte[] bytes) =>
            new Sha512().EncryptByteArray(bytes);

        /// <summary>
        ///     Encrypts this string.
        /// </summary>
        /// <param name="text">
        ///     The string to encrypt.
        /// </param>
        public static string EncryptToSha512(this string text) =>
            new Sha512().EncryptString(text);

        /// <summary>
        ///     Encrypts this file.
        /// </summary>
        /// <param name="file">
        ///     The file to encrypt.
        /// </param>
        public static string EncryptFileToSha512(this FileInfo file) =>
            new Sha512().EncryptFile(file.FullName);

        /// <summary>
        ///     Encrypts the specified file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encrypt (environment variables are accepted).
        /// </param>
        public static string EncryptFileToSha512(string path) =>
            new Sha512().EncryptFile(path);

        #endregion
    }
}
