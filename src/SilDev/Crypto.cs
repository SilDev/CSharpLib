#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Crypto.cs
// Version:  2019-10-22 15:35
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Properties;

    /// <summary>
    ///     Specifies enumerated constants used to encrypt and decrypt data.
    /// </summary>
    public enum SymmetricKeyAlgorithm
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
    ///     Specifies enumerated constants used to encode and decode data.
    /// </summary>
    public enum BinaryToTextEncoding
    {
        /// <summary>
        ///     Binary.
        /// </summary>
        Base2,

        /// <summary>
        ///     Octal.
        /// </summary>
        Base8,

        /// <summary>
        ///     Decimal.
        /// </summary>
        Base10,

        /// <summary>
        ///     Hexadecimal.
        /// </summary>
        Base16,

        /// <summary>
        ///     Base32.
        /// </summary>
        Base32,

        /// <summary>
        ///     Base64.
        /// </summary>
        Base64,

        /// <summary>
        ///     Base85 (Ascii85).
        /// </summary>
        Base85,

        /// <summary>
        ///     Base91 (basE91).
        /// </summary>
        Base91
    }

    /// <summary>
    ///     Specifies enumerated constants used to encrypt data.
    /// </summary>
    public enum ChecksumAlgorithm
    {
        /// <summary>
        ///     Adler-32.
        /// </summary>
        Adler32,

        /// <summary>
        ///     Cyclic Redundancy Check (CRC-16).
        /// </summary>
        Crc16,

        /// <summary>
        ///     Cyclic Redundancy Check (CRC-32).
        /// </summary>
        Crc32,

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
    ///     Provides functionality for data encryption and decryption.
    /// </summary>
    public static class Crypto
    {
        private static void CombineHashes(StringBuilder builder, string hash1, string hash2, bool braces)
        {
            if (braces)
                builder.Append('{');
            var first = hash1 ?? new string('0', 8);
            if (first.Length < 8)
                first = first.PadLeft(8, '0');
            if (first.Length > 8)
                first = first.Substring(0, 8);
            builder.Append(first);
            var second = hash2 ?? new string('0', 12);
            if (second.Length < 12)
                second = first.PadRight(12, '0');
            for (var i = 0; i < 3; i++)
            {
                builder.Append('-');
                builder.Append(second.Substring(i * 4, 4));
            }
            builder.Append('-');
            builder.Append(second.Substring(second.Length - 12));
            if (braces)
                builder.Append('}');
        }

        /// <summary>
        ///     Encrypts this sequence of bytes with the specified <see cref="ChecksumAlgorithm"/>
        ///     and combines both hashes into a unique GUID.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encrypt.
        /// </param>
        /// <param name="braces">
        ///     true to place the GUID between braces; otherwise, false.
        /// </param>
        /// <param name="algorithm1">
        ///     The first algorithm to use.
        /// </param>
        /// <param name="algorithm2">
        ///     The second algorithm to use.
        /// </param>
        public static string GetGuid(this byte[] bytes, bool braces = false, ChecksumAlgorithm algorithm1 = ChecksumAlgorithm.Crc32, ChecksumAlgorithm algorithm2 = ChecksumAlgorithm.Sha1)
        {
            var guid = new StringBuilder(braces ? 38 : 36);
            CombineHashes(guid, bytes?.Encrypt(algorithm1), bytes?.Encrypt(algorithm2), braces);
            return guid.ToString();
        }

        /// <summary>
        ///     Encrypts this string with the specified <see cref="ChecksumAlgorithm"/> and
        ///     combines both hashes into a unique GUID.
        /// </summary>
        /// <param name="text">
        ///     The string to encrypt.
        /// </param>
        /// <param name="braces">
        ///     true to place the GUID between braces; otherwise, false.
        /// </param>
        /// <param name="algorithm1">
        ///     The first algorithm to use.
        /// </param>
        /// <param name="algorithm2">
        ///     The second algorithm to use.
        /// </param>
        public static string GetGuid(this string text, bool braces = false, ChecksumAlgorithm algorithm1 = ChecksumAlgorithm.Crc32, ChecksumAlgorithm algorithm2 = ChecksumAlgorithm.Sha1)
        {
            var guid = new StringBuilder(braces ? 38 : 36);
            CombineHashes(guid, text?.Encrypt(algorithm1), text?.Encrypt(algorithm2), braces);
            return guid.ToString();
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
        public static byte[] Encrypt(this byte[] bytes, byte[] password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256)
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithm.Aes128:
                    return Aes.EncryptBytes(bytes, password, salt, Aes.KeySize.Aes128);
                case SymmetricKeyAlgorithm.Aes192:
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
        public static byte[] Encrypt(this byte[] bytes, string password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256)
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithm.Aes128:
                    return Aes.EncryptBytes(bytes, password.ToBytes(), salt, Aes.KeySize.Aes128);
                case SymmetricKeyAlgorithm.Aes192:
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
        public static byte[] Encrypt(this string text, byte[] password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256)
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithm.Aes128:
                    return Aes.EncryptBytes(text.ToBytes(), password, salt, Aes.KeySize.Aes128);
                case SymmetricKeyAlgorithm.Aes192:
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
        public static byte[] Encrypt(this string text, string password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256)
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithm.Aes128:
                    return Aes.EncryptBytes(text.ToBytes(), password.ToBytes(), salt, Aes.KeySize.Aes128);
                case SymmetricKeyAlgorithm.Aes192:
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
        public static byte[] EncryptFile(this string path, byte[] password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256)
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithm.Aes128:
                    return Aes.EncryptFile(path, password, salt, Aes.KeySize.Aes128);
                case SymmetricKeyAlgorithm.Aes192:
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
        public static byte[] EncryptFile(this string path, string password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256)
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithm.Aes128:
                    return Aes.EncryptFile(path, password.ToBytes(), salt, Aes.KeySize.Aes128);
                case SymmetricKeyAlgorithm.Aes192:
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
        public static byte[] Decrypt(this byte[] bytes, byte[] password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256)
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithm.Aes128:
                    return Aes.DecryptBytes(bytes, password, salt, Aes.KeySize.Aes128);
                case SymmetricKeyAlgorithm.Aes192:
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
        public static byte[] Decrypt(this byte[] bytes, string password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256)
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithm.Aes128:
                    return Aes.DecryptBytes(bytes, password.ToBytes(), salt, Aes.KeySize.Aes128);
                case SymmetricKeyAlgorithm.Aes192:
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
        public static byte[] DecryptFile(this string path, byte[] password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256)
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithm.Aes128:
                    return Aes.DecryptFile(path, password, salt, Aes.KeySize.Aes128);
                case SymmetricKeyAlgorithm.Aes192:
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
        public static byte[] DecryptFile(this string path, string password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256)
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithm.Aes128:
                    return Aes.DecryptFile(path, password.ToBytes(), salt, Aes.KeySize.Aes128);
                case SymmetricKeyAlgorithm.Aes192:
                    return Aes.DecryptFile(path, password.ToBytes(), salt, Aes.KeySize.Aes192);
                default:
                    return Aes.DecryptFile(path, password.ToBytes(), salt);
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
        public static string Encode(this byte[] bytes, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64)
        {
            switch (algorithm)
            {
                case BinaryToTextEncoding.Base2:
                    return new Base2().EncodeBytes(bytes);
                case BinaryToTextEncoding.Base8:
                    return new Base8().EncodeBytes(bytes);
                case BinaryToTextEncoding.Base10:
                    return new Base10().EncodeBytes(bytes);
                case BinaryToTextEncoding.Base16:
                    return new Base16().EncodeBytes(bytes);
                case BinaryToTextEncoding.Base32:
                    return new Base32().EncodeBytes(bytes);
                case BinaryToTextEncoding.Base85:
                    return new Base85().EncodeBytes(bytes);
                case BinaryToTextEncoding.Base91:
                    return new Base91().EncodeBytes(bytes);
                default:
                    return new Base64().EncodeBytes(bytes);
            }
        }

        /// <summary>
        ///     Encodes this string with the specified algorithm.
        /// </summary>
        /// <param name="text">
        ///     The string to encode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encode(this string text, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64)
        {
            switch (algorithm)
            {
                case BinaryToTextEncoding.Base2:
                    return new Base2().EncodeString(text);
                case BinaryToTextEncoding.Base8:
                    return new Base8().EncodeString(text);
                case BinaryToTextEncoding.Base10:
                    return new Base10().EncodeString(text);
                case BinaryToTextEncoding.Base16:
                    return new Base16().EncodeString(text);
                case BinaryToTextEncoding.Base32:
                    return new Base32().EncodeString(text);
                case BinaryToTextEncoding.Base85:
                    return new Base85().EncodeString(text);
                case BinaryToTextEncoding.Base91:
                    return new Base91().EncodeString(text);
                default:
                    return new Base64().EncodeString(text);
            }
        }

        /// <summary>
        ///     Encodes this file into a sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string EncodeFile(this string path, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64)
        {
            switch (algorithm)
            {
                case BinaryToTextEncoding.Base2:
                    return new Base2().EncodeFile(path);
                case BinaryToTextEncoding.Base8:
                    return new Base8().EncodeFile(path);
                case BinaryToTextEncoding.Base10:
                    return new Base10().EncodeFile(path);
                case BinaryToTextEncoding.Base16:
                    return new Base16().EncodeFile(path);
                case BinaryToTextEncoding.Base32:
                    return new Base32().EncodeFile(path);
                case BinaryToTextEncoding.Base85:
                    return new Base85().EncodeFile(path);
                case BinaryToTextEncoding.Base91:
                    return new Base91().EncodeFile(path);
                default:
                    return new Base64().EncodeFile(path);
            }
        }

        /// <summary>
        ///     Decodes this string into a sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] Decode(this string code, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64)
        {
            switch (algorithm)
            {
                case BinaryToTextEncoding.Base2:
                    return new Base2().DecodeBytes(code);
                case BinaryToTextEncoding.Base8:
                    return new Base8().DecodeBytes(code);
                case BinaryToTextEncoding.Base10:
                    return new Base10().DecodeBytes(code);
                case BinaryToTextEncoding.Base16:
                    return new Base85().DecodeBytes(code);
                case BinaryToTextEncoding.Base32:
                    return new Base32().DecodeBytes(code);
                case BinaryToTextEncoding.Base85:
                    return new Base85().DecodeBytes(code);
                case BinaryToTextEncoding.Base91:
                    return new Base91().DecodeBytes(code);
                default:
                    return new Base64().DecodeBytes(code);
            }
        }

        /// <summary>
        ///     Decodes this string into a sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string DecodeString(this string code, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64)
        {
            switch (algorithm)
            {
                case BinaryToTextEncoding.Base2:
                    return new Base2().DecodeString(code);
                case BinaryToTextEncoding.Base8:
                    return new Base8().DecodeString(code);
                case BinaryToTextEncoding.Base10:
                    return new Base10().DecodeString(code);
                case BinaryToTextEncoding.Base16:
                    return new Base16().DecodeString(code);
                case BinaryToTextEncoding.Base32:
                    return new Base32().DecodeString(code);
                case BinaryToTextEncoding.Base85:
                    return new Base85().DecodeString(code);
                case BinaryToTextEncoding.Base91:
                    return new Base91().DecodeString(code);
                default:
                    return new Base64().DecodeString(code);
            }
        }

        /// <summary>
        ///     Decodes this file into a sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to decode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] DecodeFile(this string path, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64)
        {
            switch (algorithm)
            {
                case BinaryToTextEncoding.Base2:
                    return new Base2().DecodeFile(path);
                case BinaryToTextEncoding.Base8:
                    return new Base8().DecodeFile(path);
                case BinaryToTextEncoding.Base10:
                    return new Base10().DecodeFile(path);
                case BinaryToTextEncoding.Base16:
                    return new Base16().DecodeFile(path);
                case BinaryToTextEncoding.Base32:
                    return new Base32().DecodeFile(path);
                case BinaryToTextEncoding.Base85:
                    return new Base85().DecodeFile(path);
                case BinaryToTextEncoding.Base91:
                    return new Base91().DecodeFile(path);
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
        public static string Encrypt(this Stream stream, ChecksumAlgorithm algorithm = ChecksumAlgorithm.Md5)
        {
            switch (algorithm)
            {
                case ChecksumAlgorithm.Adler32:
                    return new Adler32().EncryptStream(stream);
                case ChecksumAlgorithm.Crc16:
                    return new Crc16().EncryptStream(stream);
                case ChecksumAlgorithm.Crc32:
                    return new Crc32().EncryptStream(stream);
                case ChecksumAlgorithm.Sha1:
                    return new Sha1().EncryptStream(stream);
                case ChecksumAlgorithm.Sha256:
                    return new Sha256().EncryptStream(stream);
                case ChecksumAlgorithm.Sha384:
                    return new Sha384().EncryptStream(stream);
                case ChecksumAlgorithm.Sha512:
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
        public static string Encrypt(this byte[] bytes, ChecksumAlgorithm algorithm = ChecksumAlgorithm.Md5)
        {
            switch (algorithm)
            {
                case ChecksumAlgorithm.Adler32:
                    return new Adler32().EncryptBytes(bytes);
                case ChecksumAlgorithm.Crc16:
                    return new Crc16().EncryptBytes(bytes);
                case ChecksumAlgorithm.Crc32:
                    return new Crc32().EncryptBytes(bytes);
                case ChecksumAlgorithm.Sha1:
                    return new Sha1().EncryptBytes(bytes);
                case ChecksumAlgorithm.Sha256:
                    return new Sha256().EncryptBytes(bytes);
                case ChecksumAlgorithm.Sha384:
                    return new Sha384().EncryptBytes(bytes);
                case ChecksumAlgorithm.Sha512:
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
        public static string Encrypt(this string text, ChecksumAlgorithm algorithm = ChecksumAlgorithm.Md5)
        {
            switch (algorithm)
            {
                case ChecksumAlgorithm.Adler32:
                    return new Adler32().EncryptString(text);
                case ChecksumAlgorithm.Crc16:
                    return new Crc16().EncryptString(text);
                case ChecksumAlgorithm.Crc32:
                    return new Crc32().EncryptString(text);
                case ChecksumAlgorithm.Sha1:
                    return new Sha1().EncryptString(text);
                case ChecksumAlgorithm.Sha256:
                    return new Sha256().EncryptString(text);
                case ChecksumAlgorithm.Sha384:
                    return new Sha384().EncryptString(text);
                case ChecksumAlgorithm.Sha512:
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
        public static string EncryptFile(this string path, ChecksumAlgorithm algorithm = ChecksumAlgorithm.Md5)
        {
            switch (algorithm)
            {
                case ChecksumAlgorithm.Adler32:
                    return new Adler32().EncryptFile(path);
                case ChecksumAlgorithm.Crc16:
                    return new Crc16().EncryptFile(path);
                case ChecksumAlgorithm.Crc32:
                    return new Crc32().EncryptFile(path);
                case ChecksumAlgorithm.Sha1:
                    return new Sha1().EncryptFile(path);
                case ChecksumAlgorithm.Sha256:
                    return new Sha256().EncryptFile(path);
                case ChecksumAlgorithm.Sha384:
                    return new Sha384().EncryptFile(path);
                case ChecksumAlgorithm.Sha512:
                    return new Sha512().EncryptFile(path);
                default:
                    return new Md5().EncryptFile(path);
            }
        }

        #region Asymmetric-key Algorithms

        /// <summary>
        ///     Provides static methods to handle RSA encryption and decryption.
        /// </summary>
        public static class Rsa
        {
            /// <summary>
            ///     Creates a public and private key pair at the specified location.
            /// </summary>
            /// <param name="dirPath">
            ///     The directory path.
            /// </param>
            /// <param name="keySize">
            ///     The size of the key in bits.
            /// </param>
            public static void CreateKeyFiles(string dirPath, int keySize = 4096)
            {
                var csp = new RSACryptoServiceProvider(keySize);
                try
                {
                    var keyStamp = $"rsa-{keySize}-{DateTime.Now:yyyyMMdd}";
                    var privPath = PathEx.Combine(dirPath, $"{keyStamp}-private.xml");
                    Xml.SerializeToFile(privPath, csp.ExportParameters(true));
                    var pubPath = PathEx.Combine(dirPath, $"{keyStamp}-public.xml");
                    Xml.SerializeToFile(pubPath, csp.ExportParameters(false));
                }
                finally
                {
                    csp.PersistKeyInCsp = false;
                    csp.Dispose();
                }
            }

            /// <summary>
            ///     Encrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="publicKeyPath">
            ///     The path to the public key file.
            /// </param>
            /// <param name="bytes">
            ///     The data to encrypt.
            /// </param>
            public static string EncryptBytes(string publicKeyPath, byte[] bytes)
            {
                var csp = new RSACryptoServiceProvider();
                var text = default(string);
                try
                {
                    var pubKey = Xml.DeserializeFile<RSAParameters>(publicKeyPath);
                    csp.ImportParameters(pubKey);
                    var cypher = csp.Encrypt(bytes, true);
                    text = Convert.ToBase64String(cypher);
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                finally
                {
                    csp.PersistKeyInCsp = false;
                    csp.Dispose();
                }
                return text;
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="publicKeyPath">
            ///     The path to the public key file.
            /// </param>
            /// <param name="text">
            ///     The text to encrypt.
            /// </param>
            public static string EncryptString(string publicKeyPath, string text) =>
                EncryptBytes(publicKeyPath, text?.ToBytesDefault());

            /// <summary>
            ///     Decrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="privateKeyPath">
            ///     The path to the private key file.
            /// </param>
            /// <param name="code">
            ///     The cypher to decrypt.
            /// </param>
            public static byte[] DecryptBytes(string privateKeyPath, string code)
            {
                var csp = new RSACryptoServiceProvider();
                var data = default(byte[]);
                try
                {
                    var privKey = Xml.DeserializeFile<RSAParameters>(privateKeyPath);
                    csp.ImportParameters(privKey);
                    var cypher = Convert.FromBase64String(code);
                    data = csp.Decrypt(cypher, true);
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                finally
                {
                    csp.PersistKeyInCsp = false;
                    csp.Dispose();
                }
                return data;
            }

            /// <summary>
            ///     Decrypts the specified string.
            /// </summary>
            /// <param name="privateKeyPath">
            ///     The path to the private key file.
            /// </param>
            /// <param name="code">
            ///     The cypher to decrypt.
            /// </param>
            public static string DecryptString(string privateKeyPath, string code) =>
                DecryptBytes(privateKeyPath, code)?.ToStringDefault();
        }

        #endregion

        #region Symmetric-key Algorithms

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
                        using (var db = new Rfc2898DeriveBytes(password, salt ?? password.Encrypt(ChecksumAlgorithm.Sha512).ToBytes(), 1000))
                        {
                            rm.Key = db.GetBytes(rm.KeySize / 8);
                            rm.IV = db.GetBytes(rm.BlockSize / 8);
                        }
                        rm.Mode = CipherMode.CBC;
                        var ms = new MemoryStream();
                        using (var cs = new CryptoStream(ms, rm.CreateEncryptor(), CryptoStreamMode.Write))
                            cs.WriteBytes(bytes);
                        ba = ms.ToArray();
                    }
                    return ba;
                }
                catch (Exception ex) when (ex.IsCaught())
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
                catch (Exception ex) when (ex.IsCaught())
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
                        using (var db = new Rfc2898DeriveBytes(password, salt ?? password.Encrypt(ChecksumAlgorithm.Sha512).ToBytes(), 1000))
                        {
                            rm.Key = db.GetBytes(rm.KeySize / 8);
                            rm.IV = db.GetBytes(rm.BlockSize / 8);
                        }
                        rm.Mode = CipherMode.CBC;
                        var ms = new MemoryStream();
                        using (var cs = new CryptoStream(ms, rm.CreateDecryptor(), CryptoStreamMode.Write))
                            cs.WriteBytes(code);
                        ba = ms.ToArray();
                    }
                    return ba;
                }
                catch (Exception ex) when (ex.IsCaught())
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
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                    return null;
                }
            }
        }

        #endregion

        #region Binary-To-Text Encodings

        /// <summary>
        ///     Represents the base class from which all implementations of binary-to-text encoding
        ///     algorithms must derive.
        /// </summary>
        public class BinaryToText
        {
            /// <summary>
            ///     Gets the separator.
            /// </summary>
            protected static readonly byte[] Separator =
            {
                0xd,
                0xa
            };

            /// <summary>
            ///     Encodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to encode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for encoding.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="NotSupportedException">
            ///     The current method has no functionality.
            /// </exception>
            public virtual void EncodeStream(Stream inputStream, Stream outputStream, int lineLength = 0, bool dispose = false) =>
                throw new NotSupportedException();

            /// <summary>
            ///     Encodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to encode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for encoding.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="NotSupportedException">
            ///     <see cref="EncodeStream(Stream, Stream, int, bool)"/> method has no functionality.
            /// </exception>
            public void EncodeStream(Stream inputStream, Stream outputStream, bool dispose) =>
                EncodeStream(inputStream, outputStream, 0, dispose);

            /// <summary>
            ///     Encodes the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encode.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            public string EncodeBytes(byte[] bytes, int lineLength = 0)
            {
                try
                {
                    if (bytes == null)
                        throw new ArgumentNullException(nameof(bytes));
                    string s;
                    using (var msi = new MemoryStream(bytes))
                    {
                        byte[] ba;
                        using (var mso = new MemoryStream())
                        {
                            EncodeStream(msi, mso, lineLength);
                            ba = mso.ToArray();
                        }
                        s = ba.ToStringDefault();
                    }
                    return s;
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                return null;
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
            public string EncodeString(string text, int lineLength = 0) =>
                EncodeBytes(text?.ToBytes(), lineLength);

            /// <summary>
            ///     Encodes the specified source file to the specified destination file.
            /// </summary>
            /// <param name="srcPath">
            ///     The source file to encode.
            /// </param>
            /// <param name="destPath">
            ///     The destination file to create.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="overwrite">
            ///     true to allow an existing file to be overwritten; otherwise, false.
            /// </param>
            public bool EncodeFile(string srcPath, string destPath, int lineLength = 0, bool overwrite = true)
            {
                try
                {
                    if (string.IsNullOrEmpty(srcPath))
                        throw new ArgumentNullException(nameof(srcPath));
                    if (string.IsNullOrEmpty(destPath))
                        throw new ArgumentNullException(nameof(destPath));
                    var src = PathEx.Combine(srcPath);
                    if (!File.Exists(src))
                        throw new PathNotFoundException(srcPath);
                    var dest = PathEx.Combine(destPath);
                    if (!PathEx.IsValidPath(dest))
                        throw new ArgumentException(ExceptionMessages.DestPathNotValid);
                    using (var fsi = new FileStream(src, FileMode.Open, FileAccess.Read))
                        using (var fso = new FileStream(dest, overwrite ? FileMode.Create : FileMode.CreateNew))
                            EncodeStream(fsi, fso, lineLength);
                    return true;
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                return false;
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
            public string EncodeFile(string path, int lineLength = 0)
            {
                try
                {
                    if (string.IsNullOrEmpty(path))
                        throw new ArgumentNullException(nameof(path));
                    var file = PathEx.Combine(path);
                    if (!File.Exists(file))
                        throw new PathNotFoundException(path);
                    string s;
                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        byte[] ba;
                        using (var ms = new MemoryStream())
                        {
                            EncodeStream(fs, ms, lineLength);
                            ba = ms.ToArray();
                        }
                        s = ba.ToStringDefault();
                    }
                    return s;
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                return null;
            }

            /// <summary>
            ///     Decodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to decode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for decoding.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="NotSupportedException">
            ///     The current method has no functionality.
            /// </exception>
            public virtual void DecodeStream(Stream inputStream, Stream outputStream, bool dispose = false) =>
                throw new NotSupportedException();

            /// <summary>
            ///     Decodes the specified string into a sequence of bytes.
            /// </summary>
            /// <param name="code">
            ///     The string to decode.
            /// </param>
            public byte[] DecodeBytes(string code)
            {
                try
                {
                    if (string.IsNullOrEmpty(code))
                        throw new ArgumentNullException(nameof(code));
                    using (var msi = new MemoryStream(code.ToBytes()))
                    {
                        byte[] ba;
                        using (var mso = new MemoryStream())
                        {
                            DecodeStream(msi, mso);
                            ba = mso.ToArray();
                        }
                        return ba;
                    }
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                return null;
            }

            /// <summary>
            ///     Decodes the specified string into a string.
            /// </summary>
            /// <param name="code">
            ///     The string to decode.
            /// </param>
            public string DecodeString(string code) =>
                DecodeBytes(code)?.ToStringDefault();

            /// <summary>
            ///     Decodes the specified source file to the specified destination file.
            /// </summary>
            /// <param name="srcPath">
            ///     The source file to encode.
            /// </param>
            /// <param name="destPath">
            ///     The destination file to create.
            /// </param>
            /// <param name="overwrite">
            ///     true to allow an existing file to be overwritten; otherwise, false.
            /// </param>
            public bool DecodeFile(string srcPath, string destPath, bool overwrite = true)
            {
                try
                {
                    if (string.IsNullOrEmpty(srcPath))
                        throw new ArgumentNullException(nameof(srcPath));
                    if (string.IsNullOrEmpty(destPath))
                        throw new ArgumentNullException(nameof(destPath));
                    var src = PathEx.Combine(srcPath);
                    if (!File.Exists(src))
                        throw new PathNotFoundException(srcPath);
                    var dest = PathEx.Combine(destPath);
                    if (!PathEx.IsValidPath(dest))
                        throw new ArgumentException(ExceptionMessages.DestPathNotValid);
                    using (var fsi = new FileStream(src, FileMode.Open, FileAccess.Read))
                        using (var fso = new FileStream(dest, overwrite ? FileMode.Create : FileMode.CreateNew))
                            DecodeStream(fsi, fso);
                    return true;
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                return false;
            }

            /// <summary>
            ///     Decodes the specified string into a sequence of bytes containing a small file.
            /// </summary>
            /// <param name="code">
            ///     The string to decode.
            /// </param>
            public byte[] DecodeFile(string code) =>
                DecodeBytes(code);

            /// <summary>
            ///     Write the specified byte into the stream and add a line separator depending
            ///     on the specified line length.
            /// </summary>
            /// <param name="stream">
            ///     The stream in which to write the single byte.
            /// </param>
            /// <param name="singleByte">
            ///     The single byte.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="linePos">
            ///     The position in the line.
            /// </param>
            protected static void WriteLine(Stream stream, byte singleByte, int lineLength, ref int linePos)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                stream.WriteByte(singleByte);
                if (lineLength < 1 || lineLength > ++linePos)
                    return;
                linePos = 0;
                stream.WriteBytes(Separator);
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base2"/> class.
        /// </summary>
        public class Base2 : BinaryToText
        {
            /// <summary>
            ///     Encodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to encode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for encoding.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void EncodeStream(Stream inputStream, Stream outputStream, int lineLength = 0, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int i;
                    var p = 0;
                    while ((i = si.ReadByte()) != -1)
                        foreach (var b in Convert.ToString(i, 2).PadLeft(8, '0').ToBytes())
                            WriteLine(so, b, lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }

            /// <summary>
            ///     Decodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to decode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for decoding.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="DecoderFallbackException">
            ///     inputStream contains invalid characters.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void DecodeStream(Stream inputStream, Stream outputStream, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int i;
                    var cl = new List<char>();
                    while ((i = si.ReadByte()) != -1)
                    {
                        if (i <= 0 || i == 0x20 || Separator.Contains((byte)i))
                            continue;
                        if (i != 0x30 && i != 0x31)
                            throw new DecoderFallbackException(ExceptionMessages.CharsInStreamAreInvalid);
                        cl.Add((char)i);
                        if (cl.Count % 8 != 0)
                            continue;
                        so.WriteByte(Convert.ToByte(new string(cl.ToArray()), 2));
                        cl.Clear();
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base8"/> class.
        /// </summary>
        public class Base8 : BinaryToText
        {
            /// <summary>
            ///     Encodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to encode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for encoding.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void EncodeStream(Stream inputStream, Stream outputStream, int lineLength = 0, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int i;
                    var p = 0;
                    while ((i = si.ReadByte()) != -1)
                        foreach (var b in Convert.ToString(i, 8).PadLeft(3, '0').ToBytes())
                            WriteLine(so, b, lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }

            /// <summary>
            ///     Decodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to decode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for decoding.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="DecoderFallbackException">
            ///     inputStream contains invalid characters.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void DecodeStream(Stream inputStream, Stream outputStream, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int i;
                    var cl = new List<char>();
                    while ((i = si.ReadByte()) != -1)
                    {
                        if (i <= 0 || i == 0x20 || Separator.Contains((byte)i))
                            continue;
                        if (!i.IsBetween(0x30, 0x39))
                            throw new DecoderFallbackException(ExceptionMessages.CharsInStreamAreInvalid);
                        cl.Add((char)i);
                        if (cl.Count % 3 != 0)
                            continue;
                        so.WriteByte(Convert.ToByte(new string(cl.ToArray()), 8));
                        cl.Clear();
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base10"/> class.
        /// </summary>
        public class Base10 : BinaryToText
        {
            /// <summary>
            ///     Encodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to encode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for encoding.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void EncodeStream(Stream inputStream, Stream outputStream, int lineLength = 0, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int i;
                    var p = 0;
                    while ((i = si.ReadByte()) != -1)
                        foreach (var b in Convert.ToString(i, 10).PadLeft(3, '0').ToBytes())
                            WriteLine(so, b, lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }

            /// <summary>
            ///     Decodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to decode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for decoding.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="DecoderFallbackException">
            ///     inputStream contains invalid characters.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void DecodeStream(Stream inputStream, Stream outputStream, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int i;
                    var cl = new List<char>();
                    while ((i = si.ReadByte()) != -1)
                    {
                        if (i <= 0 || i == 0x20 || Separator.Contains((byte)i))
                            continue;
                        if (!i.IsBetween(0x30, 0x39))
                            throw new DecoderFallbackException(ExceptionMessages.CharsInStreamAreInvalid);
                        cl.Add((char)i);
                        if (cl.Count % 3 != 0)
                            continue;
                        so.WriteByte(Convert.ToByte(new string(cl.ToArray()), 10));
                        cl.Clear();
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base16"/> class.
        /// </summary>
        public class Base16 : BinaryToText
        {
            /// <summary>
            ///     Encodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to encode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for encoding.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void EncodeStream(Stream inputStream, Stream outputStream, int lineLength = 0, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int i;
                    var p = 0;
                    while ((i = si.ReadByte()) != -1)
                        foreach (var b in i.ToString("x2", CultureInfo.InvariantCulture).PadLeft(2, '0').ToBytes())
                            WriteLine(so, b, lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }

            /// <summary>
            ///     Decodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to decode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for decoding.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="DecoderFallbackException">
            ///     inputStream contains invalid characters.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void DecodeStream(Stream inputStream, Stream outputStream, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int i;
                    var cl = new List<char>();
                    while ((i = si.ReadByte()) != -1)
                    {
                        if (i <= 0 || i == 0x20 || Separator.Contains((byte)i))
                            continue;
                        if (!i.IsBetween(0x30, 0x39) && !i.IsBetween(0x41, 0x46) && !i.IsBetween(0x61, 0x66))
                            throw new DecoderFallbackException(ExceptionMessages.CharsInStreamAreInvalid);
                        cl.Add((char)i);
                        if (cl.Count % 2 != 0)
                            continue;
                        so.WriteByte(Convert.ToByte(new string(cl.ToArray()), 16));
                        cl.Clear();
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base32"/> class.
        /// </summary>
        public class Base32 : BinaryToText
        {
            private static readonly byte[] CharacterTable32 =
            {
                0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48,
                0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, 0x50,
                0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58,
                0x59, 0x5a, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37
            };

            /// <summary>
            ///     Encodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to encode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for encoding.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">
            ///     inputStream is larger than 128 MB.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void EncodeStream(Stream inputStream, Stream outputStream, int lineLength = 0, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                if (inputStream.Length > 0x8000000)
                    throw new ArgumentOutOfRangeException(nameof(inputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int i;
                    var ba = new byte[si.Length];
                    var p = 0;
                    while ((i = si.Read(ba, 0, ba.Length)) > 0)
                    {
                        var len = (i > ba.Length ? Math.Pow(ba.Length, Math.Max(Math.Floor((double)i / ba.Length), 1)) : i) * 8;
                        for (var j = 0; j < len; j += 5)
                        {
                            var c = ba[j / 8] << 8;
                            if (j / 8 + 1 < ba.Length)
                                c |= ba[j / 8 + 1];
                            c = 31 & (c >> (16 - j % 8 - 5));
                            WriteLine(so, CharacterTable32[c], lineLength, ref p);
                        }
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }

            /// <summary>
            ///     Decodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to decode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for decoding.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">
            ///     inputStream is larger than 128 MB.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="DecoderFallbackException">
            ///     inputStream contains invalid characters.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void DecodeStream(Stream inputStream, Stream outputStream, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                if (inputStream.Length > 0x8000000)
                    throw new ArgumentOutOfRangeException(nameof(inputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    var ba1 = new byte[si.Length];
                    var a32 = Encoding.ASCII.GetString(CharacterTable32);
                    while (si.Read(ba1, 0, ba1.Length) > 0)
                    {
                        var ba2 = ba1.Where(b => b > 0 && !Separator.Contains(b)).ToArray();
                        if (ba2.Any(x => !CharacterTable32.Contains(x)))
                            throw new DecoderFallbackException(ExceptionMessages.CharsInStreamAreInvalid);
                        var len = ba2.Length * 5;
                        for (var i = 0; i < len; i += 8)
                        {
                            var b = ba2[i / 5];
                            var c = a32.IndexOf((char)b) << 10;
                            if (i / 5 + 1 < ba2.Length)
                            {
                                b = ba2[i / 5 + 1];
                                c |= a32.IndexOf((char)b) << 5;
                            }
                            if (i / 5 + 2 < ba2.Length)
                            {
                                b = ba2[i / 5 + 2];
                                c |= a32.IndexOf((char)b);
                            }
                            c = 255 & (c >> (15 - i % 5 - 8));
                            if (i + 5 > len && c <= 0)
                                break;
                            so.WriteByte((byte)c);
                        }
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base64"/> class.
        /// </summary>
        public class Base64 : BinaryToText
        {
            /// <summary>
            ///     Encodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to encode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for encoding.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void EncodeStream(Stream inputStream, Stream outputStream, int lineLength = 0, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int i;
                    var cs = new CryptoStream(si, new ToBase64Transform(), CryptoStreamMode.Read);
                    var ba = new byte[lineLength < 1 ? 4096 : lineLength];
                    while ((i = cs.Read(ba, 0, ba.Length)) > 0)
                    {
                        so.Write(ba, 0, i);
                        if (lineLength < 1 || i < ba.Length)
                            continue;
                        so.WriteBytes(Separator);
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }

            /// <summary>
            ///     Decodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to decode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for decoding.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void DecodeStream(Stream inputStream, Stream outputStream, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    var bai = new byte[4096];
                    var bao = new byte[bai.Length];
                    using (var fbt = new FromBase64Transform())
                    {
                        int i;
                        while ((i = si.Read(bai, 0, bai.Length)) > 0)
                        {
                            i = fbt.TransformBlock(bai, 0, i, bao, 0);
                            so.Write(bao, 0, i);
                        }
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base85"/> class.
        /// </summary>
        public class Base85 : BinaryToText
        {
            private static readonly byte[] EncodeBlock = new byte[5],
                                           DecodeBlock = new byte[4];

            private static readonly uint[] Pow85 =
            {
                85 * 85 * 85 * 85,
                85 * 85 * 85,
                85 * 85,
                85,
                1
            };

            /// <summary>
            ///     Encodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to encode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for encoding.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void EncodeStream(Stream inputStream, Stream outputStream, int lineLength = 0, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int b;
                    var n = 0;
                    var t = 0u;
                    var p = 0;
                    while ((b = si.ReadByte()) != -1)
                    {
                        if (n + 1 < DecodeBlock.Length)
                        {
                            t |= (uint)(b << (24 - n * 8));
                            n++;
                            continue;
                        }
                        t |= (uint)b;
                        if (t == 0)
                            WriteLine(so, 0x7a, lineLength, ref p);
                        else
                        {
                            for (var i = EncodeBlock.Length - 1; i >= 0; i--)
                            {
                                EncodeBlock[i] = (byte)(t % 85 + 33);
                                t /= 85;
                            }
                            foreach (var eb in EncodeBlock)
                                WriteLine(so, eb, lineLength, ref p);
                        }
                        t = 0;
                        n = 0;
                    }
                    if (n <= 0)
                        return;
                    for (var i = EncodeBlock.Length - 1; i >= 0; i--)
                    {
                        EncodeBlock[i] = (byte)(t % 85 + 33);
                        t /= 85;
                    }
                    for (var i = 0; i <= n; i++)
                        WriteLine(so, EncodeBlock[i], lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }

            /// <summary>
            ///     Decodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to decode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for decoding.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="DecoderFallbackException">
            ///     inputStream contains invalid characters.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void DecodeStream(Stream inputStream, Stream outputStream, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int b;
                    var n = 0;
                    var t = 0u;
                    while ((b = si.ReadByte()) != -1)
                    {
                        if (b == 0x7a)
                        {
                            if (n != 0)
                                throw new DecoderFallbackException(ExceptionMessages.FollowingCharCodeIsInvalid + 0x7a);
                            for (var i = 0; i < 4; i++)
                                DecodeBlock[i] = 0;
                            so.Write(DecodeBlock, 0, DecodeBlock.Length);
                            continue;
                        }
                        switch (b)
                        {
                            case 0x0:
                            case 0x8:
                            case 0x9:
                            case 0xa:
                            case 0xc:
                            case 0xd:
                                continue;
                        }
                        if (b < 0x21 || b > 0x75)
                            throw new DecoderFallbackException(ExceptionMessages.FollowingCharCodeIsInvalid + b);
                        t += (uint)((b - 33) * Pow85[n]);
                        n++;
                        if (n != EncodeBlock.Length)
                            continue;
                        for (var i = 0; i < DecodeBlock.Length; i++)
                            DecodeBlock[i] = (byte)(t >> (24 - i * 8));
                        so.Write(DecodeBlock, 0, DecodeBlock.Length);
                        t = 0;
                        n = 0;
                    }
                    switch (n)
                    {
                        case 0:
                            return;
                        case 1:
                            throw new DecoderFallbackException(ExceptionMessages.LastBlockIsSingleByte);
                    }
                    n--;
                    t += Pow85[n];
                    for (var i = 0; i < n; i++)
                        DecodeBlock[i] = (byte)(t >> (24 - i * 8));
                    for (var i = 0; i < n; i++)
                        so.WriteByte(DecodeBlock[i]);
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base91"/> class.
        /// </summary>
        public class Base91 : BinaryToText
        {
            /// <summary>
            ///     The default character table for translation.
            /// </summary>
            protected static readonly byte[] DefaultCharacterTable91 =
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

            private byte[] _characterTable91;

            /// <summary>
            ///     The character table for translation.
            /// </summary>
            protected virtual byte[] CharacterTable91
            {
                get => _characterTable91 ?? (_characterTable91 = DefaultCharacterTable91);
                set
                {
                    if (value == default || value.Distinct().Count() != DefaultCharacterTable91.Length)
                        return;
                    _characterTable91 = value;
                }
            }

            /// <summary>
            ///     Encodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to encode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for encoding.
            /// </param>
            /// <param name="lineLength">
            ///     The length of lines.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     inputStream or outputStream is invalid.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void EncodeStream(Stream inputStream, Stream outputStream, int lineLength = 0, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int b;
                    int[] ia = { 0, 0, 0 };
                    var p = 0;
                    while ((b = si.ReadByte()) != -1)
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
                        WriteLine(so, CharacterTable91[ia[2] % 91], lineLength, ref p);
                        WriteLine(so, CharacterTable91[ia[2] / 91], lineLength, ref p);
                    }
                    if (ia[1] == 0)
                        return;
                    WriteLine(so, CharacterTable91[ia[0] % 91], lineLength, ref p);
                    if (ia[1] >= 8 || ia[0] >= 91)
                        WriteLine(so, CharacterTable91[ia[0] / 91], lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }

            /// <summary>
            ///     Decodes the specified input stream into the specified output stream.
            /// </summary>
            /// <param name="inputStream">
            ///     The input stream to decode.
            /// </param>
            /// <param name="outputStream">
            ///     The output stream for decoding.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the input and output <see cref="Stream"/>;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     inputStream or outputStream is null.
            /// </exception>
            /// <exception cref="DecoderFallbackException">
            ///     inputStream contains invalid characters.
            /// </exception>
            /// <exception cref="NotSupportedException">
            ///     inputStream is not readable -or- outputStream is not writable.
            /// </exception>
            /// <exception cref="IOException">
            ///     An I/O error occured, such as the specified file cannot be found.
            /// </exception>
            /// <exception cref="ObjectDisposedException">
            ///     Methods were called after the inputStream or outputStream was closed.
            /// </exception>
            public override void DecodeStream(Stream inputStream, Stream outputStream, bool dispose = false)
            {
                if (inputStream == null)
                    throw new ArgumentNullException(nameof(inputStream));
                if (outputStream == null)
                    throw new ArgumentNullException(nameof(outputStream));
                var si = inputStream;
                var so = outputStream;
                try
                {
                    int b;
                    int[] ia = { 0, -1, 0, 0 };
                    var a91 = new Dictionary<int, int>();
                    for (var i = 0; i < byte.MaxValue; i++)
                        a91[i] = -1;
                    for (var i = 0; i < CharacterTable91.Length; i++)
                        a91[CharacterTable91[i]] = i;
                    while ((b = si.ReadByte()) != -1)
                    {
                        switch (b)
                        {
                            case 0x0:
                            case 0xa:
                            case 0xd:
                                continue;
                        }
                        if (!CharacterTable91.Contains((byte)b))
                            throw new DecoderFallbackException($"The character number '{b}' is invalid.");
                        ia[0] = a91[b];
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
                            so.WriteByte((byte)(ia[2] & byte.MaxValue));
                            ia[2] >>= 8;
                            ia[3] -= 8;
                        }
                        while (ia[3] > 7);
                        ia[1] = -1;
                    }
                    if (ia[1] != -1)
                        so.WriteByte((byte)((ia[2] | (ia[1] << ia[3])) & byte.MaxValue));
                }
                finally
                {
                    if (dispose)
                    {
                        si.Dispose();
                        so.Dispose();
                    }
                }
            }
        }

        #endregion

        #region Checksum Algorithms

        /// <summary>
        ///     Represents the base class from which all implementations of checksum encryption
        ///     algorithms must derive.
        /// </summary>
        public class Checksum
        {
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
                {
                    if (csp == null)
                        throw new ArgumentNullException(nameof(csp));
                    ba = csp.ComputeHash(stream);
                }
                var sb = new StringBuilder(ba.Length * 2);
                foreach (var b in ba)
                    sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
                return sb.ToString();
            }

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            /// <exception cref="NotSupportedException">
            ///     The current method has no functionality.
            /// </exception>
            public virtual string EncryptStream(Stream stream) =>
                throw new NotSupportedException();

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
                using (var ms = new MemoryStream(bytes))
                    s = EncryptStream(ms);
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
                {
                    if (csp == null)
                        throw new ArgumentNullException(nameof(csp));
                    ba = csp.ComputeHash(ba);
                }
                var s = BitConverter.ToString(ba);
                return s.RemoveChar('-').ToLowerInvariant();
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            /// <exception cref="NotSupportedException">
            ///     The current method has no functionality.
            /// </exception>
            public virtual string EncryptString(string text) =>
                throw new NotSupportedException();

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
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Adler32"/> class.
        /// </summary>
        public class Adler32 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 8;

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override string EncryptStream(Stream stream)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                int i;
                var uia = new[]
                {
                    1 & 0xffffu,
                    (1 >> 16) & 0xffffu
                };
                while ((i = stream.ReadByte()) != -1)
                {
                    uia[0] = (uia[0] + (uint)i) % 65521;
                    uia[1] = (uia[1] + uia[0]) % 65521;
                }
                var ui = (uia[1] << 16) | uia[0];
                return ui.ToString("x2", CultureInfo.InvariantCulture).PadLeft(8, '0');
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text) =>
                EncryptBytes(text?.ToBytes());
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Crc16"/> class.
        /// </summary>
        public class Crc16 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 4;

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override string EncryptStream(Stream stream)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                int i, x = 0xffff;
                while ((i = stream.ReadByte()) != -1)
                    for (var j = 0; j < 8; j++)
                    {
                        if (((x ^ i) & 1) == 1)
                        {
                            x >>= 1;
                            x ^= 0xa001;
                        }
                        else
                            x >>= 1;
                        i >>= 1;
                    }
                var ba = new[]
                {
                    (byte)(x % 256),
                    (byte)(x / 256)
                };
                var sb = new StringBuilder(ba.Length * 2);
                foreach (var b in ba)
                    sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
                return sb.ToString();
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text) =>
                EncryptBytes(text?.ToBytes());
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Crc32"/> class.
        /// </summary>
        public class Crc32 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 8;

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override string EncryptStream(Stream stream)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                int i;
                var ui = 0xffffffffu;
                while ((i = stream.ReadByte()) != -1)
                {
                    ui ^= (uint)i;
                    for (var j = 0; j < 8; j++)
                        ui = (uint)((ui >> 1) ^ (0xedb88320u & -(ui & 1)));
                }
                return (~ui).ToString("x2", CultureInfo.InvariantCulture).PadLeft(8, '0');
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text) =>
                EncryptBytes(text?.ToBytes());
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Md5"/> class.
        /// </summary>
        public class Md5 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 32;

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override string EncryptStream(Stream stream) =>
                EncryptStream(stream, new MD5CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override string EncryptString(string text)
            {
                var algo = default(MD5);
                try
                {
                    algo = MD5.Create();
                    return EncryptString(text, algo);
                }
                finally
                {
                    algo?.Dispose();
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Sha1"/> class.
        /// </summary>
        public class Sha1 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 40;

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
            public override string EncryptString(string text)
            {
                var algo = default(SHA1);
                try
                {
                    algo = SHA1.Create();
                    return EncryptString(text, algo);
                }
                finally
                {
                    algo?.Dispose();
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Sha256"/> class.
        /// </summary>
        public class Sha256 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 64;

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
            public override string EncryptString(string text)
            {
                var algo = default(SHA256);
                try
                {
                    algo = SHA256.Create();
                    return EncryptString(text, algo);
                }
                finally
                {
                    algo?.Dispose();
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Sha384"/> class.
        /// </summary>
        public class Sha384 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 96;

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
            public override string EncryptString(string text)
            {
                var algo = default(SHA384);
                try
                {
                    algo = SHA384.Create();
                    return EncryptString(text, algo);
                }
                finally
                {
                    algo?.Dispose();
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Sha512"/> class.
        /// </summary>
        public class Sha512 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 128;

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
            public override string EncryptString(string text)
            {
                var algo = default(SHA512);
                try
                {
                    algo = SHA512.Create();
                    return EncryptString(text, algo);
                }
                finally
                {
                    algo?.Dispose();
                }
            }
        }

        #endregion
    }
}
