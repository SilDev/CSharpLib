#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Crypto.cs
// Version:  2018-06-07 09:32
// 
// Copyright (c) 2018, Si13n7 Developments (r)
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

    /// <summary>
    ///     Specifies enumerated constants used to encode and decode data.
    /// </summary>
    public enum EncodingAlgorithms
    {
        /// <summary>
        ///     Base64.
        /// </summary>
        Base64,

        /// <summary>
        ///     Base85, also called Ascii85.
        /// </summary>
        Base85,

        /// <summary>
        ///     basE91.
        /// </summary>
        Base91,

        /// <summary>
        ///     Base-2 binary.
        /// </summary>
        Binary,

        /// <summary>
        ///     Hexadecimal.
        /// </summary>
        Hex
    }

    /// <summary>
    ///     Specifies enumerated constants used to encrypt data.
    /// </summary>
    public enum ChecksumAlgorithms
    {
        /// <summary>
        ///     Message-Digest 5.
        /// </summary>
        Md5,

        /// <summary>
        ///     Secure Hash Algorithm 1.
        /// </summary>
        Sha1,

        /// <summary>
        ///     Secure Hash Algorithm 2 (SHA-256).
        /// </summary>
        Sha256,

        /// <summary>
        ///     Secure Hash Algorithm 2 (SHA-384).
        /// </summary>
        Sha384,

        /// <summary>
        ///     Secure Hash Algorithm 2 (SHA-512).
        /// </summary>
        Sha512
    }

    /// <summary>
    ///     Specifies enumerated constants used to encrypt and decrypt data.
    /// </summary>
    public enum RijndaelAlgorithms
    {
        /// <summary>
        ///     Advanced Encryption Standard (AES-128).
        /// </summary>
        Aes128,

        /// <summary>
        ///     Advanced Encryption Standard (AES-192).
        /// </summary>
        Aes192,

        /// <summary>
        ///     Advanced Encryption Standard (AES-256).
        /// </summary>
        Aes256
    }

    /// <summary>
    ///     Provides functionality for data encryption and decryption.
    /// </summary>
    public static class Crypto
    {
        /// <summary>
        ///     Encodes this sequence of bytes with the specified algorithm.
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
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encode(this byte[] bytes, string prefixMark, string suffixMark, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().EncodeBytes(bytes, prefixMark, suffixMark);
                case EncodingAlgorithms.Base91:
                    return new Base91().EncodeBytes(bytes, prefixMark, suffixMark);
                case EncodingAlgorithms.Binary:
                case EncodingAlgorithms.Hex:
                    return string.Concat(prefixMark, bytes.Encode(algorithm), suffixMark);
                default:
                    return new Base64().EncodeBytes(bytes, prefixMark, suffixMark);
            }
        }

        /// <summary>
        ///     Encodes this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encode(this byte[] bytes, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().EncodeBytes(bytes);
                case EncodingAlgorithms.Base91:
                    return new Base91().EncodeBytes(bytes);
                case EncodingAlgorithms.Binary:
                    return bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).Join(' ');
                case EncodingAlgorithms.Hex:
                    return bytes.Select(b => b.ToString("x2").PadLeft(2, '0')).Join(' ');
                default:
                    return new Base64().EncodeBytes(bytes);
            }
        }

        /// <summary>
        ///     Encodes this sequence of bytes with the specified algorithm.
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
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encode(this string text, string prefixMark, string suffixMark, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().EncodeString(text, prefixMark, suffixMark);
                case EncodingAlgorithms.Base91:
                    return new Base91().EncodeString(text, prefixMark, suffixMark);
                case EncodingAlgorithms.Binary:
                case EncodingAlgorithms.Hex:
                    return Encode(text.ToBytes(), prefixMark, suffixMark, algorithm);
                default:
                    return new Base64().EncodeString(text, prefixMark, suffixMark);
            }
        }

        /// <summary>
        ///     Encodes this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="text">
        ///     The string to encode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encode(this string text, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().EncodeString(text);
                case EncodingAlgorithms.Base91:
                    return new Base91().EncodeString(text);
                case EncodingAlgorithms.Binary:
                case EncodingAlgorithms.Hex:
                    return Encode(text.ToBytes(), algorithm);
                default:
                    return new Base64().EncodeString(text);
            }
        }

        /// <summary>
        ///     Encodes this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string EncodeFile(this string path, string prefixMark, string suffixMark, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().EncodeFile(path, prefixMark, suffixMark);
                case EncodingAlgorithms.Base91:
                    return new Base91().EncodeFile(path, prefixMark, suffixMark);
                case EncodingAlgorithms.Binary:
                case EncodingAlgorithms.Hex:
                    return Encode(FileEx.ReadAllBytes(path), prefixMark, suffixMark, algorithm);
                default:
                    return new Base64().EncodeFile(path, prefixMark, suffixMark);
            }
        }

        /// <summary>
        ///     Decodes this sequence of bytes with the specified algorithm.
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
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] Decode(this string code, string prefixMark, string suffixMark, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().DecodeBytes(code, prefixMark, suffixMark);
                case EncodingAlgorithms.Base91:
                    return new Base91().DecodeBytes(code, prefixMark, suffixMark);
                case EncodingAlgorithms.Binary:
                case EncodingAlgorithms.Hex:
                {
                    var s = code;
                    if (s.StartsWith(prefixMark))
                        s = s.Substring(prefixMark.Length);
                    if (s.EndsWith(suffixMark))
                        s = s.Substring(0, s.Length - suffixMark.Length);
                    return Decode(s, algorithm);
                }
                default:
                    return new Base64().DecodeBytes(code, prefixMark, suffixMark);
            }
        }

        /// <summary>
        ///     Decodes this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] Decode(this string code, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().DecodeBytes(code);
                case EncodingAlgorithms.Base91:
                    return new Base91().DecodeBytes(code);
                case EncodingAlgorithms.Binary:
                {
                    var ba = default(byte[]);
                    try
                    {
                        var s = code.RemoveChar(' ', ':', '\r', '\n');
                        if (s.Any(c => !"01".Contains(c)))
                            throw new InvalidOperationException();
                        using (var ms = new MemoryStream())
                        {
                            for (var i = 0; i < s.Length; i += 8)
                                ms.WriteByte(Convert.ToByte(s.Substring(i, 8), 2));
                            ba = Encoding.UTF8.GetString(ms.ToArray()).ToBytes();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                    }
                    return ba;
                }
                case EncodingAlgorithms.Hex:
                {
                    var ba = default(byte[]);
                    try
                    {
                        var s = new string(code.Where(char.IsLetterOrDigit).ToArray()).ToUpper();
                        if (s.Any(c => !"0123456789ABCDEF".Contains(c)))
                            throw new InvalidOperationException();
                        ba = Enumerable.Range(0, s.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(s.Substring(x, 2), 16)).ToArray();
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                    }
                    return ba;
                }
                default:
                    return new Base64().DecodeBytes(code);
            }
        }

        /// <summary>
        ///     Decodes this sequence of bytes with the specified algorithm.
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
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string DecodeString(this string code, string prefixMark, string suffixMark, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().DecodeString(code, prefixMark, suffixMark);
                case EncodingAlgorithms.Base91:
                    return new Base91().DecodeString(code, prefixMark, suffixMark);
                default:
                    return new Base64().DecodeString(code, prefixMark, suffixMark);
            }
        }

        /// <summary>
        ///     Decodes this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string DecodeString(this string code, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().DecodeString(code);
                case EncodingAlgorithms.Base91:
                    return new Base91().DecodeString(code);
                case EncodingAlgorithms.Binary:
                case EncodingAlgorithms.Hex:
                    return Encoding.UTF8.GetString(Decode(code, algorithm));
                default:
                    return new Base64().DecodeString(code);
            }
        }

        /// <summary>
        ///     Decodes this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to decode.
        /// </param>
        /// <param name="prefixMark">
        ///     The prefix mark.
        /// </param>
        /// <param name="suffixMark">
        ///     The suffix mark.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] DecodeFile(this string path, string prefixMark, string suffixMark, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().DecodeFile(path, prefixMark, suffixMark);
                case EncodingAlgorithms.Base91:
                    return new Base91().DecodeFile(path, prefixMark, suffixMark);
                case EncodingAlgorithms.Binary:
                case EncodingAlgorithms.Hex:
                    return Decode(FileEx.ReadAllText(path), prefixMark, suffixMark, algorithm);
                default:
                    return new Base64().DecodeFile(path, prefixMark, suffixMark);
            }
        }

        /// <summary>
        ///     Decodes this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to decode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] DecodeFile(this string path, EncodingAlgorithms algorithm = EncodingAlgorithms.Base64)
        {
            switch (algorithm)
            {
                case EncodingAlgorithms.Base85:
                    return new Base85().DecodeFile(path);
                case EncodingAlgorithms.Base91:
                    return new Base91().DecodeFile(path);
                case EncodingAlgorithms.Binary:
                case EncodingAlgorithms.Hex:
                    return Decode(FileEx.ReadAllText(path), algorithm);
                default:
                    return new Base64().DecodeFile(path);
            }
        }

        /// <summary>
        ///     Encrypts this stream with the specified algorithm.
        /// </summary>
        /// <param name="stream">
        ///     The stream to encrypt.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encrypt(this Stream stream, ChecksumAlgorithms algorithm = ChecksumAlgorithms.Md5)
        {
            switch (algorithm)
            {
                case ChecksumAlgorithms.Sha1:
                    return new Sha1().EncryptStream(stream);
                case ChecksumAlgorithms.Sha256:
                    return new Sha256().EncryptStream(stream);
                case ChecksumAlgorithms.Sha384:
                    return new Sha384().EncryptStream(stream);
                case ChecksumAlgorithms.Sha512:
                    return new Sha512().EncryptStream(stream);
                default:
                    return new Md5().EncryptStream(stream);
            }
        }

        /// <summary>
        ///     Encrypts this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encrypt.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encrypt(this byte[] bytes, ChecksumAlgorithms algorithm = ChecksumAlgorithms.Md5)
        {
            switch (algorithm)
            {
                case ChecksumAlgorithms.Sha1:
                    return new Sha1().EncryptBytes(bytes);
                case ChecksumAlgorithms.Sha256:
                    return new Sha256().EncryptBytes(bytes);
                case ChecksumAlgorithms.Sha384:
                    return new Sha384().EncryptBytes(bytes);
                case ChecksumAlgorithms.Sha512:
                    return new Sha512().EncryptBytes(bytes);
                default:
                    return new Md5().EncryptBytes(bytes);
            }
        }

        /// <summary>
        ///     Encrypts this string with the specified algorithm.
        /// </summary>
        /// <param name="text">
        ///     The string to encrypt.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encrypt(this string text, ChecksumAlgorithms algorithm = ChecksumAlgorithms.Md5)
        {
            switch (algorithm)
            {
                case ChecksumAlgorithms.Sha1:
                    return new Sha1().EncryptString(text);
                case ChecksumAlgorithms.Sha256:
                    return new Sha256().EncryptString(text);
                case ChecksumAlgorithms.Sha384:
                    return new Sha384().EncryptString(text);
                case ChecksumAlgorithms.Sha512:
                    return new Sha512().EncryptString(text);
                default:
                    return new Md5().EncryptString(text);
            }
        }

        /// <summary>
        ///     Encrypts this file with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encrypt.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string EncryptFile(this string path, ChecksumAlgorithms algorithm = ChecksumAlgorithms.Md5)
        {
            switch (algorithm)
            {
                case ChecksumAlgorithms.Sha1:
                    return new Sha1().EncryptFile(path);
                case ChecksumAlgorithms.Sha256:
                    return new Sha256().EncryptFile(path);
                case ChecksumAlgorithms.Sha384:
                    return new Sha384().EncryptFile(path);
                case ChecksumAlgorithms.Sha512:
                    return new Sha512().EncryptFile(path);
                default:
                    return new Md5().EncryptFile(path);
            }
        }

        /// <summary>
        ///     Encrypts this sequence of bytes with the specified algorithm.
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
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] Encrypt(this byte[] bytes, byte[] password, byte[] salt = null, RijndaelAlgorithms algorithm = RijndaelAlgorithms.Aes256)
        {
            switch (algorithm)
            {
                case RijndaelAlgorithms.Aes128:
                    return Aes.EncryptBytes(bytes, password, salt, Aes.KeySize.Aes128);
                case RijndaelAlgorithms.Aes192:
                    return Aes.EncryptBytes(bytes, password, salt, Aes.KeySize.Aes192);
                default:
                    return Aes.EncryptBytes(bytes, password, salt);
            }
        }

        /// <summary>
        ///     Encrypts this sequence of bytes with the specified algorithm.
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
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] Encrypt(this byte[] bytes, string password, byte[] salt = null, RijndaelAlgorithms algorithm = RijndaelAlgorithms.Aes256)
        {
            switch (algorithm)
            {
                case RijndaelAlgorithms.Aes128:
                    return Aes.EncryptBytes(bytes, password.ToBytes(), salt, Aes.KeySize.Aes128);
                case RijndaelAlgorithms.Aes192:
                    return Aes.EncryptBytes(bytes, password.ToBytes(), salt, Aes.KeySize.Aes192);
                default:
                    return Aes.EncryptBytes(bytes, password.ToBytes(), salt);
            }
        }

        /// <summary>
        ///     Encrypts this string with the specified algorithm.
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
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] Encrypt(this string text, byte[] password, byte[] salt = null, RijndaelAlgorithms algorithm = RijndaelAlgorithms.Aes256)
        {
            switch (algorithm)
            {
                case RijndaelAlgorithms.Aes128:
                    return Aes.EncryptBytes(text.ToBytes(), password, salt, Aes.KeySize.Aes128);
                case RijndaelAlgorithms.Aes192:
                    return Aes.EncryptBytes(text.ToBytes(), password, salt, Aes.KeySize.Aes192);
                default:
                    return Aes.EncryptBytes(text.ToBytes(), password, salt);
            }
        }

        /// <summary>
        ///     Encrypts this string with the specified algorithm.
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
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] Encrypt(this string text, string password, byte[] salt = null, RijndaelAlgorithms algorithm = RijndaelAlgorithms.Aes256)
        {
            switch (algorithm)
            {
                case RijndaelAlgorithms.Aes128:
                    return Aes.EncryptBytes(text.ToBytes(), password.ToBytes(), salt, Aes.KeySize.Aes128);
                case RijndaelAlgorithms.Aes192:
                    return Aes.EncryptBytes(text.ToBytes(), password.ToBytes(), salt, Aes.KeySize.Aes192);
                default:
                    return Aes.EncryptBytes(text.ToBytes(), password.ToBytes(), salt);
            }
        }

        /// <summary>
        ///     Encrypts this file with the specified algorithm.
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
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] EncryptFile(this string path, byte[] password, byte[] salt = null, RijndaelAlgorithms algorithm = RijndaelAlgorithms.Aes256)
        {
            switch (algorithm)
            {
                case RijndaelAlgorithms.Aes128:
                    return Aes.EncryptFile(path, password, salt, Aes.KeySize.Aes128);
                case RijndaelAlgorithms.Aes192:
                    return Aes.EncryptFile(path, password, salt, Aes.KeySize.Aes192);
                default:
                    return Aes.EncryptFile(path, password, salt);
            }
        }

        /// <summary>
        ///     Encrypts this file with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        /// <param name="salt">
        ///     The sequence of bytes which is used as salt.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] EncryptFile(this string path, string password, byte[] salt = null, RijndaelAlgorithms algorithm = RijndaelAlgorithms.Aes256)
        {
            switch (algorithm)
            {
                case RijndaelAlgorithms.Aes128:
                    return Aes.EncryptFile(path, password.ToBytes(), salt, Aes.KeySize.Aes128);
                case RijndaelAlgorithms.Aes192:
                    return Aes.EncryptFile(path, password.ToBytes(), salt, Aes.KeySize.Aes192);
                default:
                    return Aes.EncryptFile(path, password.ToBytes(), salt);
            }
        }

        /// <summary>
        ///     Decrypts this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to decrypt.
        /// </param>
        /// <param name="password">
        ///     The sequence of bytes which is used as password.
        /// </param>
        /// <param name="salt">
        ///     The sequence of bytes which is used as salt.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] Decrypt(this byte[] bytes, byte[] password, byte[] salt = null, RijndaelAlgorithms algorithm = RijndaelAlgorithms.Aes256)
        {
            switch (algorithm)
            {
                case RijndaelAlgorithms.Aes128:
                    return Aes.DecryptBytes(bytes, password, salt, Aes.KeySize.Aes128);
                case RijndaelAlgorithms.Aes192:
                    return Aes.DecryptBytes(bytes, password, salt, Aes.KeySize.Aes192);
                default:
                    return Aes.DecryptBytes(bytes, password, salt);
            }
        }

        /// <summary>
        ///     Decrypts this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        /// <param name="salt">
        ///     The sequence of bytes which is used as salt.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] Decrypt(this byte[] bytes, string password, byte[] salt = null, RijndaelAlgorithms algorithm = RijndaelAlgorithms.Aes256)
        {
            switch (algorithm)
            {
                case RijndaelAlgorithms.Aes128:
                    return Aes.DecryptBytes(bytes, password.ToBytes(), salt, Aes.KeySize.Aes128);
                case RijndaelAlgorithms.Aes192:
                    return Aes.DecryptBytes(bytes, password.ToBytes(), salt, Aes.KeySize.Aes192);
                default:
                    return Aes.DecryptBytes(bytes, password.ToBytes(), salt);
            }
        }

        /// <summary>
        ///     Decrypts this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to decrypt.
        /// </param>
        /// <param name="password">
        ///     The sequence of bytes which is used as password.
        /// </param>
        /// <param name="salt">
        ///     The sequence of bytes which is used as salt.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] DecryptFile(this string path, byte[] password, byte[] salt = null, RijndaelAlgorithms algorithm = RijndaelAlgorithms.Aes256)
        {
            switch (algorithm)
            {
                case RijndaelAlgorithms.Aes128:
                    return Aes.DecryptFile(path, password, salt, Aes.KeySize.Aes128);
                case RijndaelAlgorithms.Aes192:
                    return Aes.DecryptFile(path, password, salt, Aes.KeySize.Aes192);
                default:
                    return Aes.DecryptFile(path, password, salt);
            }
        }

        /// <summary>
        ///     Decrypts this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to decrypt.
        /// </param>
        /// <param name="password">
        ///     The string which is used as password.
        /// </param>
        /// <param name="salt">
        ///     The sequence of bytes which is used as salt.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] DecryptFile(this string path, string password, byte[] salt = null, RijndaelAlgorithms algorithm = RijndaelAlgorithms.Aes256)
        {
            switch (algorithm)
            {
                case RijndaelAlgorithms.Aes128:
                    return Aes.DecryptFile(path, password.ToBytes(), salt, Aes.KeySize.Aes128);
                case RijndaelAlgorithms.Aes192:
                    return Aes.DecryptFile(path, password.ToBytes(), salt, Aes.KeySize.Aes192);
                default:
                    return Aes.DecryptFile(path, password.ToBytes(), salt);
            }
        }

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

#pragma warning disable 1591
            protected string EncodeFilters(string input, string prefixMark, string suffixMark, uint lineLength)
#pragma warning restore 1591
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
#pragma warning disable 1591
            protected string DecodeFilters(string input, string prefixMark, string suffixMark)
#pragma warning restore 1591
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
            public virtual string EncodeBytes(byte[] bytes, string prefixMark = null, string suffixMark = null, uint lineLength = 0)
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
            public virtual byte[] DecodeBytes(string code, string prefixMark = null, string suffixMark = null)
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
                    var ba = text.ToBytes();
                    return EncodeBytes(ba, prefixMark, suffixMark, lineLength) ?? string.Empty;
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
                    var ba = DecodeBytes(code, prefixMark, suffixMark);
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
            ///     The full path of the file to encode.
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
                        throw new PathNotFoundException(s);
                    byte[] ba;
                    using (var fs = new FileStream(s, FileMode.Open))
                    {
                        ba = new byte[fs.Length];
                        fs.Read(ba, 0, (int)fs.Length);
                    }
                    return EncodeBytes(ba, prefixMark, suffixMark, lineLength);
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
            ///     The full path of the file to encode.
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
                DecodeBytes(code, prefixMark, suffixMark);
        }

        #endregion

        #region Base85

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base85"/> class.
        /// </summary>
        public class Base85 : Base64
        {
            private static readonly byte[] EncodeBlock = new byte[5], DecodeBlock = new byte[4];

            private static readonly uint[] P85 =
            {
                85 * 85 * 85 * 85,
                85 * 85 * 85,
                85 * 85,
                85,
                1
            };

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
            public override string EncodeBytes(byte[] bytes, string prefixMark = "<~", string suffixMark = "~>", uint lineLength = 0)
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
            public override byte[] DecodeBytes(string code, string prefixMark = "<~", string suffixMark = "~>")
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
                                    throw new EncoderFallbackException();
                                for (var i = 0; i < 4; i++)
                                    DecodeBlock[i] = 0;
                                ms.Write(DecodeBlock, 0, DecodeBlock.Length);
                                continue;
                            }
                            if (ca.Contains(c))
                                continue;
                            if (c < (char)0x21 || c > (char)0x75)
                                throw new EncoderFallbackException();
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
                                throw new EncoderFallbackException();
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
                get => _encodeTable ?? (_encodeTable = DefaultEncodeTable);
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
                for (var i = 0; i < byte.MaxValue; i++)
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
            public override string EncodeBytes(byte[] bytes, string prefixMark = null, string suffixMark = null, uint lineLength = 0)
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
            public override byte[] DecodeBytes(string code, string prefixMark = null, string suffixMark = null)
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
                                throw new EncoderFallbackException();
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
                                ms.WriteByte((byte)(ia[2] & byte.MaxValue));
                                ia[2] >>= 8;
                                ia[3] -= 8;
                            }
                            while (ia[3] > 7);
                            ia[1] = -1;
                        }
                        if (ia[1] != -1)
                            ms.WriteByte((byte)((ia[2] | (ia[1] << ia[3])) & byte.MaxValue));
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
            ///     Encrypts the specified stream with the specified <see cref="HashAlgorithm"/>.
            /// </summary>
            /// <typeparam name="THashAlgorithm">
            ///     The type of the algorithm.
            /// </typeparam>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            /// <param name="algorithm">
            ///     The algorithm to encrypt.
            /// </param>
            protected string EncryptStream<THashAlgorithm>(Stream stream, THashAlgorithm algorithm) where THashAlgorithm : HashAlgorithm
            {
                byte[] ba;
                using (var csp = algorithm)
                    ba = csp.ComputeHash(stream);
                var sb = new StringBuilder(ba.Length * 2);
                foreach (var b in ba)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public virtual string EncryptStream(Stream stream) =>
                EncryptStream(stream, new MD5CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt.
            /// </param>
            public string EncryptBytes(byte[] bytes)
            {
                if (bytes == null || !bytes.Any())
                    return string.Empty;
                string s;
                using (var ms = new MemoryStream())
                {
                    ms.Read(bytes, 0, bytes.Length);
                    s = EncryptStream(ms);
                }
                return s;
            }

            /// <summary>
            ///     Encrypts the specified string with the specified <see cref="HashAlgorithm"/>.
            /// </summary>
            /// <typeparam name="THashAlgorithm">
            ///     The type of the algorithm.
            /// </typeparam>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            /// <param name="algorithm">
            ///     The algorithm to encrypt.
            /// </param>
            protected string EncryptString<THashAlgorithm>(string text, THashAlgorithm algorithm) where THashAlgorithm : HashAlgorithm
            {
                if (string.IsNullOrEmpty(text))
                    return string.Empty;
                var ba = text.ToBytes();
                using (var csp = algorithm)
                    ba = csp.ComputeHash(ba);
                var s = BitConverter.ToString(ba);
                return s.RemoveChar('-').ToLower();
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public virtual string EncryptString(string text) =>
                EncryptString(text, MD5.Create());

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
            public override string EncryptStream(Stream stream) =>
                EncryptStream(stream, new SHA1CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text) =>
                EncryptString(text, SHA1.Create());
        }

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
            public override string EncryptStream(Stream stream) =>
                EncryptStream(stream, new SHA256CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text) =>
                EncryptString(text, SHA256.Create());
        }

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
            public override string EncryptStream(Stream stream) =>
                EncryptStream(stream, new SHA384CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text) =>
                EncryptString(text, SHA384.Create());
        }

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
            public override string EncryptStream(Stream stream) =>
                EncryptStream(stream, new SHA512CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text) =>
                EncryptString(text, SHA512.Create());
        }

        #endregion

        #region Advanced Encryption Standard

        /// <summary>
        ///     Provides static methods to handle AES encryption and decryption.
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
            public static byte[] EncryptBytes(byte[] bytes, byte[] password, byte[] salt = null, KeySize keySize = KeySize.Aes256)
            {
                try
                {
                    byte[] ba;
                    using (var rm = new RijndaelManaged())
                    {
                        rm.BlockSize = 128;
                        rm.KeySize = (int)keySize;
                        using (var db = new Rfc2898DeriveBytes(password, salt ?? password.Encrypt(ChecksumAlgorithms.Sha512).ToBytes(), 1000))
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
                        throw new PathNotFoundException(s);
                    var ba = File.ReadAllBytes(s);
                    return EncryptBytes(ba, password, salt, keySize);
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
            ///     The sequence of bytes which was used as password.
            /// </param>
            /// <param name="salt">
            ///     The sequence of bytes which was used as salt.
            /// </param>
            /// <param name="keySize">
            ///     The size of the secret key.
            /// </param>
            public static byte[] DecryptBytes(byte[] code, byte[] password, byte[] salt = null, KeySize keySize = KeySize.Aes256)
            {
                try
                {
                    byte[] ba;
                    using (var rm = new RijndaelManaged())
                    {
                        rm.BlockSize = 128;
                        rm.KeySize = (int)keySize;
                        using (var db = new Rfc2898DeriveBytes(password, salt ?? password.Encrypt(ChecksumAlgorithms.Sha512).ToBytes(), 1000))
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
                        throw new PathNotFoundException(s);
                    var ba = File.ReadAllBytes(s);
                    return DecryptBytes(ba, password, salt, keySize);
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return null;
                }
            }
        }

        #endregion
    }
}
