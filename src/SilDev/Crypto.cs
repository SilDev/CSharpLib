#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Crypto.cs
// Version:  2021-04-22 19:46
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
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
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.Text;
    using Intern;
    using Investment;
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
        ///     Cyclic Redundancy Check (CRC-64).
        /// </summary>
        Crc64,

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
        /// <summary>
        ///     Combines the specified hash codes.
        /// </summary>
        /// <param name="hash1">
        ///     The first hash code.
        /// </param>
        /// <param name="hash2">
        ///     The second hash code.
        /// </param>
        public static int CombineHashCodes(int hash1, int hash2)
        {
            var hash = (uint)((hash1 << 5) | (int)((uint)hash1 >> 27));
            return ((int)hash + hash1) ^ hash2;
        }

        /// <summary>
        ///     Combines the hash codes of the specified objects.
        /// </summary>
        /// <param name="obj1">
        ///     The first object.
        /// </param>
        /// <param name="obj2">
        ///     The second object.
        /// </param>
        public static int CombineHashCodes(object obj1, object obj2) =>
            CombineHashCodes(obj1?.GetHashCode() ?? 17011, obj2?.GetHashCode() ?? 23011);

        /// <summary>
        ///     Combines the specified hash codes.
        /// </summary>
        /// <param name="hash1">
        ///     The first hash code.
        /// </param>
        /// <param name="hash2">
        ///     The second hash code.
        /// </param>
        /// <param name="hash3">
        ///     The third hash code.
        /// </param>
        public static int CombineHashCodes(int hash1, int hash2, int hash3) =>
            CombineHashCodes(CombineHashCodes(hash1, hash2), hash3);

        /// <summary>
        ///     Combines the hash codes of the specified objects.
        /// </summary>
        /// <param name="obj1">
        ///     The first object.
        /// </param>
        /// <param name="obj2">
        ///     The second object.
        /// </param>
        /// <param name="obj3">
        ///     The third object.
        /// </param>
        public static int CombineHashCodes(object obj1, object obj2, object obj3) =>
            CombineHashCodes(CombineHashCodes(obj1, obj2), obj3);

        /// <summary>
        ///     Combines the specified hash codes.
        /// </summary>
        /// <param name="hashes">
        ///     A sequence of hash codes.
        /// </param>
        public static int CombineHashCodes(params int[] hashes)
        {
            switch (hashes?.Length)
            {
                case null:
                case 0:
                    return 0;
                case 1:
                    return hashes[0];
                case 2:
                    return CombineHashCodes(hashes[0], hashes[1]);
                case 3:
                    return CombineHashCodes(hashes[0], hashes[1], hashes[2]);
                default:
                    var hash = hashes[0];
                    for (var i = 1; i < hashes.Length; i++)
                        hash = CombineHashCodes(hash, hashes[i]);
                    return hash;
            }
        }

        /// <summary>
        ///     Combines the hash codes of the specified objects.
        /// </summary>
        /// <param name="objects">
        ///     A sequence of hash codes.
        /// </param>
        public static int CombineHashCodes(params object[] objects)
        {
            switch (objects?.Length)
            {
                case null:
                case 0:
                    return 0;
                case 1:
                    return CombineHashCodes(objects[0], null);
                case 2:
                    return CombineHashCodes(objects[0], objects[1]);
                case 3:
                    return CombineHashCodes(objects[0], objects[1], objects[2]);
                default:
                    var hash = objects[0]?.GetHashCode() ?? 17011;
                    for (var i = 1; i < objects.Length; i++)
                        hash = CombineHashCodes(hash, objects[i]?.GetHashCode() ?? 23011);
                    return hash;
            }
        }

        /// <summary>
        ///     Encrypts this <typeparamref name="TSource"/> object with the specified
        ///     <see cref="ChecksumAlgorithm"/> and combines both hashes into a unique
        ///     GUID.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of source.
        /// </typeparam>
        /// <param name="source">
        ///     The object to encrypt.
        /// </param>
        /// <param name="braces">
        ///     <see langword="true"/> to place the GUID between braces; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="algorithm1">
        ///     The first algorithm to use.
        /// </param>
        /// <param name="algorithm2">
        ///     The second algorithm to use.
        /// </param>
        public static string GetGuid<TSource>(this TSource source, bool braces = false, ChecksumAlgorithm algorithm1 = ChecksumAlgorithm.Crc32, ChecksumAlgorithm algorithm2 = ChecksumAlgorithm.Crc64)
        {
            var guid = new StringBuilder(braces ? 38 : 36);
            CombineHashes(guid, source?.Encrypt(algorithm1), source?.Encrypt(algorithm2), braces);
            return guid.ToStringThenClear();
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
        public static byte[] Encrypt(this byte[] bytes, byte[] password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256) =>
            algorithm switch
            {
                SymmetricKeyAlgorithm.Aes128 => Aes.EncryptBytes(bytes, password, salt, Aes.KeySize.Aes128),
                SymmetricKeyAlgorithm.Aes192 => Aes.EncryptBytes(bytes, password, salt, Aes.KeySize.Aes192),
                _ => Aes.EncryptBytes(bytes, password, salt)
            };

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
        public static byte[] Encrypt(this byte[] bytes, string password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256) =>
            algorithm switch
            {
                SymmetricKeyAlgorithm.Aes128 => Aes.EncryptBytes(bytes, password.ToBytesUtf8(), salt, Aes.KeySize.Aes128),
                SymmetricKeyAlgorithm.Aes192 => Aes.EncryptBytes(bytes, password.ToBytesUtf8(), salt, Aes.KeySize.Aes192),
                _ => Aes.EncryptBytes(bytes, password.ToBytesUtf8(), salt)
            };

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
        public static byte[] Encrypt(this string text, byte[] password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256) =>
            algorithm switch
            {
                SymmetricKeyAlgorithm.Aes128 => Aes.EncryptBytes(text.ToBytesUtf8(), password, salt, Aes.KeySize.Aes128),
                SymmetricKeyAlgorithm.Aes192 => Aes.EncryptBytes(text.ToBytesUtf8(), password, salt, Aes.KeySize.Aes192),
                _ => Aes.EncryptBytes(text.ToBytesUtf8(), password, salt)
            };

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
        public static byte[] Encrypt(this string text, string password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256) =>
            algorithm switch
            {
                SymmetricKeyAlgorithm.Aes128 => Aes.EncryptBytes(text.ToBytesUtf8(), password.ToBytesUtf8(), salt, Aes.KeySize.Aes128),
                SymmetricKeyAlgorithm.Aes192 => Aes.EncryptBytes(text.ToBytesUtf8(), password.ToBytesUtf8(), salt, Aes.KeySize.Aes192),
                _ => Aes.EncryptBytes(text.ToBytesUtf8(), password.ToBytesUtf8(), salt)
            };

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
        public static byte[] EncryptFile(this string path, byte[] password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256) =>
            algorithm switch
            {
                SymmetricKeyAlgorithm.Aes128 => Aes.EncryptFile(path, password, salt, Aes.KeySize.Aes128),
                SymmetricKeyAlgorithm.Aes192 => Aes.EncryptFile(path, password, salt, Aes.KeySize.Aes192),
                _ => Aes.EncryptFile(path, password, salt)
            };

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
        public static byte[] EncryptFile(this string path, string password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256) =>
            algorithm switch
            {
                SymmetricKeyAlgorithm.Aes128 => Aes.EncryptFile(path, password.ToBytesUtf8(), salt, Aes.KeySize.Aes128),
                SymmetricKeyAlgorithm.Aes192 => Aes.EncryptFile(path, password.ToBytesUtf8(), salt, Aes.KeySize.Aes192),
                _ => Aes.EncryptFile(path, password.ToBytesUtf8(), salt)
            };

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
        public static byte[] Decrypt(this byte[] bytes, byte[] password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256) =>
            algorithm switch
            {
                SymmetricKeyAlgorithm.Aes128 => Aes.DecryptBytes(bytes, password, salt, Aes.KeySize.Aes128),
                SymmetricKeyAlgorithm.Aes192 => Aes.DecryptBytes(bytes, password, salt, Aes.KeySize.Aes192),
                _ => Aes.DecryptBytes(bytes, password, salt)
            };

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
        public static byte[] Decrypt(this byte[] bytes, string password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256) =>
            algorithm switch
            {
                SymmetricKeyAlgorithm.Aes128 => Aes.DecryptBytes(bytes, password.ToBytesUtf8(), salt, Aes.KeySize.Aes128),
                SymmetricKeyAlgorithm.Aes192 => Aes.DecryptBytes(bytes, password.ToBytesUtf8(), salt, Aes.KeySize.Aes192),
                _ => Aes.DecryptBytes(bytes, password.ToBytesUtf8(), salt)
            };

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
        public static byte[] DecryptFile(this string path, byte[] password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256) =>
            algorithm switch
            {
                SymmetricKeyAlgorithm.Aes128 => Aes.DecryptFile(path, password, salt, Aes.KeySize.Aes128),
                SymmetricKeyAlgorithm.Aes192 => Aes.DecryptFile(path, password, salt, Aes.KeySize.Aes192),
                _ => Aes.DecryptFile(path, password, salt)
            };

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
        public static byte[] DecryptFile(this string path, string password, byte[] salt = null, SymmetricKeyAlgorithm algorithm = SymmetricKeyAlgorithm.Aes256) =>
            algorithm switch
            {
                SymmetricKeyAlgorithm.Aes128 => Aes.DecryptFile(path, password.ToBytesUtf8(), salt, Aes.KeySize.Aes128),
                SymmetricKeyAlgorithm.Aes192 => Aes.DecryptFile(path, password.ToBytesUtf8(), salt, Aes.KeySize.Aes192),
                _ => Aes.DecryptFile(path, password.ToBytesUtf8(), salt)
            };

        /// <summary>
        ///     Encodes this sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to encode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encode(this byte[] bytes, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64) =>
            GetDefaultInstance(algorithm).EncodeBytes(bytes);

        /// <summary>
        ///     Encodes this string with the specified algorithm.
        /// </summary>
        /// <param name="text">
        ///     The string to encode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encode(this string text, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64) =>
            GetDefaultInstance(algorithm).EncodeString(text);

        /// <summary>
        ///     Encodes this file into a sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to encode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string EncodeFile(this string path, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64) =>
            GetDefaultInstance(algorithm).EncodeFile(path);

        /// <summary>
        ///     Decodes this string into a sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] Decode(this string code, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64) =>
            GetDefaultInstance(algorithm).DecodeBytes(code);

        /// <summary>
        ///     Decodes this string into a sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="code">
        ///     The string to decode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string DecodeString(this string code, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64) =>
            GetDefaultInstance(algorithm).DecodeString(code);

        /// <summary>
        ///     Decodes this file into a sequence of bytes with the specified algorithm.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to decode.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static byte[] DecodeFile(this string path, BinaryToTextEncoding algorithm = BinaryToTextEncoding.Base64) =>
            GetDefaultInstance(algorithm).DecodeFile(path);

        /// <summary>
        ///     Encrypts this <typeparamref name="TSource"/> object with the
        ///     <see cref="ChecksumAlgorithm.Crc32"/> algorithm.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of source.
        /// </typeparam>
        /// <param name="source">
        ///     The object to encrypt.
        /// </param>
        public static uint EncryptRaw<TSource>(this TSource source)
        {
            var instance = CacheInvestor.GetDefault<Crc32>();
            switch (source)
            {
                case null:
                    return 0u;
                case Stream stream:
                    instance.Encrypt(stream);
                    break;
                case IEnumerable<byte> bytes:
                    instance.Encrypt(bytes.AsArray());
                    break;
                case IEnumerable<char> chars:
                    instance.Encrypt(chars.AsString());
                    break;
                default:
                    try
                    {
                        using var ms = new MemoryStream();
                        var bf = new BinaryFormatter();
                        bf.Serialize(ms, source);
                        ms.Position = 0L;
                        instance.Encrypt(ms);
                    }
                    catch (SerializationException ex) when (ex.IsCaught())
                    {
                        Log.Write(ex);
                    }
                    break;
            }
            return instance.RawHash;
        }

        /// <summary>
        ///     Encrypts this <typeparamref name="TSource"/> object with the specified
        ///     algorithm.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of source.
        /// </typeparam>
        /// <param name="source">
        ///     The object to encrypt.
        /// </param>
        /// <param name="algorithm">
        ///     The algorithm to use.
        /// </param>
        public static string Encrypt<TSource>(this TSource source, ChecksumAlgorithm algorithm = ChecksumAlgorithm.Md5)
        {
            var instance = GetDefaultInstance(algorithm);
            switch (source)
            {
                case Stream stream:
                    instance.Encrypt(stream);
                    return instance.Hash;
                case IEnumerable<byte> bytes:
                    instance.Encrypt(bytes.AsArray());
                    return instance.Hash;
                case IEnumerable<char> chars:
                    instance.Encrypt(chars.AsString());
                    return instance.Hash;
            }
            if (source?.SerializeObject() is not { } data)
                return string.Empty;
            instance.Encrypt(data);
            return instance.Hash;
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
            var instance = GetDefaultInstance(algorithm);
            instance.EncryptFile(path);
            return instance.Hash;
        }

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

        private static BinaryToText GetDefaultInstance(BinaryToTextEncoding algorithm) =>
            algorithm switch
            {
                BinaryToTextEncoding.Base2 => CacheInvestor.GetDefault<Base2>(),
                BinaryToTextEncoding.Base8 => CacheInvestor.GetDefault<Base8>(),
                BinaryToTextEncoding.Base10 => CacheInvestor.GetDefault<Base10>(),
                BinaryToTextEncoding.Base16 => CacheInvestor.GetDefault<Base16>(),
                BinaryToTextEncoding.Base32 => CacheInvestor.GetDefault<Base32>(),
                BinaryToTextEncoding.Base85 => CacheInvestor.GetDefault<Base85>(),
                BinaryToTextEncoding.Base91 => CacheInvestor.GetDefault<Base91>(),
                _ => CacheInvestor.GetDefault<Base64>()
            };

        private static Checksum GetDefaultInstance(ChecksumAlgorithm algorithm) =>
            algorithm switch
            {
                ChecksumAlgorithm.Adler32 => CacheInvestor.GetDefault<Adler32>(),
                ChecksumAlgorithm.Crc16 => CacheInvestor.GetDefault<Crc16>(),
                ChecksumAlgorithm.Crc32 => CacheInvestor.GetDefault<Crc32>(),
                ChecksumAlgorithm.Crc64 => CacheInvestor.GetDefault<Crc64>(),
                ChecksumAlgorithm.Sha1 => CacheInvestor.GetDefault<Sha1>(),
                ChecksumAlgorithm.Sha256 => CacheInvestor.GetDefault<Sha256>(),
                ChecksumAlgorithm.Sha384 => CacheInvestor.GetDefault<Sha384>(),
                ChecksumAlgorithm.Sha512 => CacheInvestor.GetDefault<Sha512>(),
                _ => CacheInvestor.GetDefault<Md5>()
            };

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
                    XmlHelper.SerializeToFile(privPath, csp.ExportParameters(true));
                    var pubPath = PathEx.Combine(dirPath, $"{keyStamp}-public.xml");
                    XmlHelper.SerializeToFile(pubPath, csp.ExportParameters(false));
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
                    var pubKey = XmlHelper.DeserializeFile<RSAParameters>(publicKeyPath);
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
                    var privKey = XmlHelper.DeserializeFile<RSAParameters>(privateKeyPath);
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
                DecryptBytes(privateKeyPath, code)?.ToStringUtf8();
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
                    using var rm = new RijndaelManaged
                    {
                        BlockSize = 128,
                        KeySize = (int)keySize
                    };
                    using (var db = new Rfc2898DeriveBytes(password, salt ?? password.Encrypt(ChecksumAlgorithm.Sha512).ToBytesUtf8(), 1000))
                    {
                        rm.Key = db.GetBytes(rm.KeySize / 8);
                        rm.IV = db.GetBytes(rm.BlockSize / 8);
                    }
                    rm.Mode = CipherMode.CBC;
                    var ms = new MemoryStream();
                    using (var cs = new CryptoStream(ms, rm.CreateEncryptor(), CryptoStreamMode.Write))
                        cs.WriteBytes(bytes);
                    return ms.ToArray();
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
                    using var rm = new RijndaelManaged
                    {
                        BlockSize = 128,
                        KeySize = (int)keySize
                    };
                    using (var db = new Rfc2898DeriveBytes(password, salt ?? password.Encrypt(ChecksumAlgorithm.Sha512).ToBytesUtf8(), 1000))
                    {
                        rm.Key = db.GetBytes(rm.KeySize / 8);
                        rm.IV = db.GetBytes(rm.BlockSize / 8);
                    }
                    rm.Mode = CipherMode.CBC;
                    var ms = new MemoryStream();
                    using (var cs = new CryptoStream(ms, rm.CreateDecryptor(), CryptoStreamMode.Write))
                        cs.WriteBytes(code);
                    return ms.ToArray();
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
        ///     Represents the base class from which all implementations of binary-to-text
        ///     encoding algorithms must derive.
        /// </summary>
        public abstract class BinaryToText
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
            /// </param>
            public abstract void EncodeStream(Stream inputStream, Stream outputStream, int lineLength = 0, bool dispose = false);

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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
            /// </param>
            /// <exception cref="NotSupportedException">
            ///     <see cref="EncodeStream(Stream, Stream, int, bool)"/> method has no
            ///     functionality.
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
                    byte[] ba;
                    using var msi = new MemoryStream(bytes);
                    using (var mso = new MemoryStream())
                    {
                        EncodeStream(msi, mso, lineLength);
                        ba = mso.ToArray();
                    }
                    return ba.ToStringDefault();
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
                EncodeBytes(text?.ToBytesUtf8(), lineLength);

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
            ///     <see langword="true"/> to allow an existing file to be overwritten;
            ///     otherwise, <see langword="false"/>.
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
                    using var fsi = new FileStream(src, FileMode.Open, FileAccess.Read);
                    using var fso = new FileStream(dest, overwrite ? FileMode.Create : FileMode.CreateNew);
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
                    using var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    using var ms = new MemoryStream();
                    EncodeStream(fs, ms, lineLength);
                    var ba = ms.ToArray();
                    return ba.ToStringDefault();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
            /// </param>
            public abstract void DecodeStream(Stream inputStream, Stream outputStream, bool dispose = false);

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
                    using var msi = new MemoryStream(code.ToBytesUtf8());
                    using var mso = new MemoryStream();
                    DecodeStream(msi, mso);
                    return mso.ToArray();
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
                DecodeBytes(code)?.ToStringUtf8();

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
            ///     <see langword="true"/> to allow an existing file to be overwritten;
            ///     otherwise, <see langword="false"/>.
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
                    using var fsi = new FileStream(src, FileMode.Open, FileAccess.Read);
                    using var fso = new FileStream(dest, overwrite ? FileMode.Create : FileMode.CreateNew);
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
            ///     Decodes the specified string into a sequence of bytes containing a small
            ///     file.
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int i;
                    var p = 0;
                    while ((i = inputStream.ReadByte()) != -1)
                        foreach (var b in Convert.ToString(i, 2).PadLeft(8, '0').ToBytesUtf8())
                            WriteLine(outputStream, b, lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int i;
                    var cl = new List<char>();
                    while ((i = inputStream.ReadByte()) != -1)
                    {
                        if (i is <= 0 or 0x20 or 0x2c || Separator.Contains((byte)i))
                            continue;
                        if (i != 0x30 && i != 0x31)
                            throw new DecoderFallbackException(ExceptionMessages.CharsInStreamAreInvalid);
                        cl.Add((char)i);
                        if (cl.Count % 8 != 0)
                            continue;
                        outputStream.WriteByte(Convert.ToByte(new string(cl.ToArray()), 2));
                        cl.Clear();
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int i;
                    var p = 0;
                    while ((i = inputStream.ReadByte()) != -1)
                        foreach (var b in Convert.ToString(i, 8).PadLeft(3, '0').ToBytesUtf8())
                            WriteLine(outputStream, b, lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int i;
                    var cl = new List<char>();
                    while ((i = inputStream.ReadByte()) != -1)
                    {
                        if (i is <= 0 or 0x20 or 0x2c || Separator.Contains((byte)i))
                            continue;
                        if (!i.IsBetween(0x30, 0x39))
                            throw new DecoderFallbackException(ExceptionMessages.CharsInStreamAreInvalid);
                        cl.Add((char)i);
                        if (cl.Count % 3 != 0)
                            continue;
                        outputStream.WriteByte(Convert.ToByte(new string(cl.ToArray()), 8));
                        cl.Clear();
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int i;
                    var p = 0;
                    while ((i = inputStream.ReadByte()) != -1)
                        foreach (var b in Convert.ToString(i, 10).PadLeft(3, '0').ToBytesUtf8())
                            WriteLine(outputStream, b, lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int i;
                    var cl = new List<char>();
                    while ((i = inputStream.ReadByte()) != -1)
                    {
                        if (i is <= 0 or 0x20 or 0x2c || Separator.Contains((byte)i))
                            continue;
                        if (!i.IsBetween(0x30, 0x39))
                            throw new DecoderFallbackException(ExceptionMessages.CharsInStreamAreInvalid);
                        cl.Add((char)i);
                        if (cl.Count % 3 != 0)
                            continue;
                        outputStream.WriteByte(Convert.ToByte(new string(cl.ToArray()), 10));
                        cl.Clear();
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int i;
                    var p = 0;
                    while ((i = inputStream.ReadByte()) != -1)
                        foreach (var b in i.ToStringDefault("x2").PadLeft(2, '0').ToBytesUtf8())
                            WriteLine(outputStream, b, lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int i;
                    var cl = new List<char>();
                    while ((i = inputStream.ReadByte()) != -1)
                    {
                        if (i is <= 0 or 0x20 or 0x2c || Separator.Contains((byte)i))
                            continue;
                        if (!i.IsBetween(0x30, 0x39) && !i.IsBetween(0x41, 0x46) && !i.IsBetween(0x61, 0x66))
                            throw new DecoderFallbackException(ExceptionMessages.CharsInStreamAreInvalid);
                        cl.Add((char)i);
                        if (cl.Count % 2 != 0)
                            continue;
                        outputStream.WriteByte(Convert.ToByte(new string(cl.ToArray()), 16));
                        cl.Clear();
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int i;
                    var ba = new byte[inputStream.Length];
                    var p = 0;
                    while ((i = inputStream.Read(ba, 0, ba.Length)) > 0)
                    {
                        var len = (i > ba.Length ? Math.Pow(ba.Length, Math.Max(Math.Floor((double)i / ba.Length), 1)) : i) * 8;
                        for (var j = 0; j < len; j += 5)
                        {
                            var c = ba[j / 8] << 8;
                            if (j / 8 + 1 < ba.Length)
                                c |= ba[j / 8 + 1];
                            c = 31 & (c >> (16 - j % 8 - 5));
                            WriteLine(outputStream, CharacterTable32[c], lineLength, ref p);
                        }
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    var ba1 = new byte[inputStream.Length];
                    var a32 = Encoding.ASCII.GetString(CharacterTable32);
                    while (inputStream.Read(ba1, 0, ba1.Length) > 0)
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
                            outputStream.WriteByte((byte)c);
                        }
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int i;
                    var cs = new CryptoStream(inputStream, new ToBase64Transform(), CryptoStreamMode.Read);
                    var ba = new byte[lineLength < 1 ? 4096 : lineLength];
                    while ((i = cs.Read(ba, 0, ba.Length)) > 0)
                    {
                        outputStream.Write(ba, 0, i);
                        if (lineLength < 1 || i < ba.Length)
                            continue;
                        outputStream.WriteBytes(Separator);
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    var bai = new byte[4096];
                    var bao = new byte[bai.Length];
                    using var fbt = new FromBase64Transform();
                    int i;
                    while ((i = inputStream.Read(bai, 0, bai.Length)) > 0)
                    {
                        i = fbt.TransformBlock(bai, 0, i, bao, 0);
                        outputStream.Write(bao, 0, i);
                    }
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int b;
                    var n = 0;
                    var t = 0u;
                    var p = 0;
                    while ((b = inputStream.ReadByte()) != -1)
                    {
                        if (n + 1 < DecodeBlock.Length)
                        {
                            t |= (uint)(b << (24 - n * 8));
                            n++;
                            continue;
                        }
                        t |= (uint)b;
                        if (t == 0)
                            WriteLine(outputStream, 0x7a, lineLength, ref p);
                        else
                        {
                            for (var i = EncodeBlock.Length - 1; i >= 0; i--)
                            {
                                EncodeBlock[i] = (byte)(t % 85 + 33);
                                t /= 85;
                            }
                            foreach (var eb in EncodeBlock)
                                WriteLine(outputStream, eb, lineLength, ref p);
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
                        WriteLine(outputStream, EncodeBlock[i], lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int b;
                    var n = 0;
                    var t = 0u;
                    while ((b = inputStream.ReadByte()) != -1)
                    {
                        if (b == 0x7a)
                        {
                            if (n != 0)
                                throw new DecoderFallbackException(ExceptionMessages.FollowingCharCodeIsInvalid + 0x7a);
                            for (var i = 0; i < 4; i++)
                                DecodeBlock[i] = 0;
                            outputStream.Write(DecodeBlock, 0, DecodeBlock.Length);
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
                        if (b is < 0x21 or > 0x75)
                            throw new DecoderFallbackException(ExceptionMessages.FollowingCharCodeIsInvalid + b);
                        t += (uint)((b - 33) * Pow85[n]);
                        n++;
                        if (n != EncodeBlock.Length)
                            continue;
                        for (var i = 0; i < DecodeBlock.Length; i++)
                            DecodeBlock[i] = (byte)(t >> (24 - i * 8));
                        outputStream.Write(DecodeBlock, 0, DecodeBlock.Length);
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
                        outputStream.WriteByte(DecodeBlock[i]);
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Base91"/> class.
        /// </summary>
        public class Base91 : BinaryToText
        {
            private static readonly byte[] CharacterTable91 =
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int b;
                    int[] ia = { 0, 0, 0 };
                    var p = 0;
                    while ((b = inputStream.ReadByte()) != -1)
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
                        WriteLine(outputStream, CharacterTable91[ia[2] % 91], lineLength, ref p);
                        WriteLine(outputStream, CharacterTable91[ia[2] / 91], lineLength, ref p);
                    }
                    if (ia[1] == 0)
                        return;
                    WriteLine(outputStream, CharacterTable91[ia[0] % 91], lineLength, ref p);
                    if (ia[1] >= 8 || ia[0] >= 91)
                        WriteLine(outputStream, CharacterTable91[ia[0] / 91], lineLength, ref p);
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
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
            ///     <see langword="true"/> to release all resources used by the input and
            ///     output <see cref="Stream"/>; otherwise, <see langword="false"/>.
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
            ///     An I/O error occurred, such as the specified file cannot be found.
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
                try
                {
                    int b;
                    int[] ia = { 0, -1, 0, 0 };
                    var a91 = new Dictionary<int, int>();
                    for (var i = 0; i < byte.MaxValue; i++)
                        a91[i] = -1;
                    for (var i = 0; i < CharacterTable91.Length; i++)
                        a91[CharacterTable91[i]] = i;
                    while ((b = inputStream.ReadByte()) != -1)
                    {
                        switch (b)
                        {
                            case 0x0:
                            case 0xa:
                            case 0xd:
                                continue;
                        }
                        if (!CharacterTable91.Contains((byte)b))
                            throw new DecoderFallbackException(ExceptionMessages.FollowingCharCodeIsInvalid + b);
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
                            outputStream.WriteByte((byte)(ia[2] & byte.MaxValue));
                            ia[2] >>= 8;
                            ia[3] -= 8;
                        }
                        while (ia[3] > 7);
                        ia[1] = -1;
                    }
                    if (ia[1] != -1)
                        outputStream.WriteByte((byte)((ia[2] | (ia[1] << ia[3])) & byte.MaxValue));
                }
                finally
                {
                    if (dispose)
                    {
                        inputStream.Dispose();
                        outputStream.Dispose();
                    }
                }
            }
        }

        #endregion

        #region Checksum Algorithms

        /// <summary>
        ///     Represents the base class from which all implementations of checksum
        ///     encryption algorithms must derive.
        /// </summary>
        public abstract class Checksum
        {
            /// <summary>
            ///     Gets the computed hash code value.
            /// </summary>
            public virtual IReadOnlyList<byte> RawHash { get; protected set; }

            /// <summary>
            ///     Gets the string representation of the computed hash code.
            /// </summary>
            public string Hash => ToString();

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public abstract void Encrypt(Stream stream);

            /// <summary>
            ///     Encrypts the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt.
            /// </param>
            public void Encrypt(byte[] bytes)
            {
                if (bytes == null || !bytes.Any())
                    return;
                using var ms = new MemoryStream(bytes);
                Encrypt(ms);
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public abstract void Encrypt(string text);

            /// <summary>
            ///     Encrypts the specified file.
            /// </summary>
            /// <param name="path">
            ///     The full path of the file to encrypt.
            /// </param>
            public void EncryptFile(string path)
            {
                try
                {
                    var s = PathEx.Combine(path);
                    using var fs = File.OpenRead(s);
                    Encrypt(fs);
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
            }

            /// <summary>
            ///     Determines whether this instance have same values as the specified
            ///     <see cref="Checksum"/> instance.
            /// </summary>
            /// <param name="other">
            ///     The <see cref="Checksum"/> instance to compare.
            /// </param>
            public virtual bool Equals(Checksum other)
            {
                if (RawHash is not byte[] ba1)
                    return other?.RawHash == null;
                if (other?.RawHash is not byte[] ba2 || ba1.Length != ba2.Length)
                    return false;
                return !ba2.Where((b, i) => ba1[i] != b).Any();
            }

            /// <summary>
            ///     Determines whether this instance have same values as the specified
            ///     <see cref="object"/>.
            /// </summary>
            /// <param name="other">
            ///     The  <see cref="object"/> to compare.
            /// </param>
            public override bool Equals(object other) =>
                other is Checksum item && Equals(item);

            /// <summary>
            ///     Returns the hash code for this instance.
            /// </summary>
            public override int GetHashCode() =>
                GetType().GetHashCode();

            /// <summary>
            ///     Converts the <see cref="RawHash"/> of this instance to its equivalent
            ///     string representation.
            /// </summary>
            public override string ToString()
            {
                if (RawHash is not byte[] ba)
                    return string.Empty;
                var sb = new StringBuilder(ba.Length * 2);
                foreach (var b in ba)
                    sb.Append(b.ToStringDefault("x2"));
                return sb.ToStringThenClear();
            }

            /// <summary>
            ///     Encrypts the specified stream with the specified
            ///     <see cref="HashAlgorithm"/>.
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
            /// <exception cref="ArgumentNullException">
            ///     algorithm is null.
            /// </exception>
            protected void Encrypt<THashAlgorithm>(Stream stream, THashAlgorithm algorithm) where THashAlgorithm : HashAlgorithm
            {
                using var csp = algorithm;
                if (csp == null)
                    throw new ArgumentNullException(nameof(algorithm));
                RawHash = csp.ComputeHash(stream);
            }

            /// <summary>
            ///     Encrypts the specified string with the specified
            ///     <see cref="HashAlgorithm"/>.
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
            /// <exception cref="ArgumentNullException">
            ///     algorithm is null.
            /// </exception>
            protected void Encrypt<THashAlgorithm>(string text, THashAlgorithm algorithm) where THashAlgorithm : HashAlgorithm
            {
                if (string.IsNullOrEmpty(text))
                    return;
                var ba = text.ToBytesUtf8();
                using var csp = algorithm;
                if (csp == null)
                    throw new ArgumentNullException(nameof(algorithm));
                RawHash = csp.ComputeHash(ba);
            }

            /// <summary>
            ///     Determines whether two specified <see cref="Checksum"/> instances have same
            ///     values.
            /// </summary>
            /// <param name="left">
            ///     The first <see cref="Checksum"/> instance to compare.
            /// </param>
            /// <param name="right">
            ///     The second <see cref="Checksum"/> instance to compare.
            /// </param>
            public static bool operator ==(Checksum left, Checksum right) =>
                left?.Equals(right) ?? right is null;

            /// <summary>
            ///     Determines whether two specified <see cref="Checksum"/> instances have
            ///     different values.
            /// </summary>
            /// <param name="left">
            ///     The first <see cref="Checksum"/> instance to compare.
            /// </param>
            /// <param name="right">
            ///     The second <see cref="Checksum"/> instance to compare.
            /// </param>
            public static bool operator !=(Checksum left, Checksum right) =>
                !(left == right);
        }

        /// <summary>
        ///     Provides functionality to compute Adler-32 hashes.
        /// </summary>
        public sealed class Adler32 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 8;

            /// <summary>
            ///     Gets the computed hash code value.
            /// </summary>
            public new uint RawHash { get; private set; }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Adler32"/> class.
            /// </summary>
            public Adler32() { }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Adler32"/> class and encrypts
            ///     the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public Adler32(Stream stream) =>
                Encrypt(stream);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Adler32"/> class and encrypts
            ///     the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt
            /// </param>
            public Adler32(byte[] bytes) =>
                Encrypt(bytes);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Adler32"/> class and encrypts
            ///     the specified text or file.
            /// </summary>
            /// <param name="textOrFile">
            ///     The text or file to encrypt
            /// </param>
            /// <param name="strIsFilePath">
            ///     <see langword="true"/> if the specified value is a file path; otherwise,
            ///     <see langword="false"/>
            /// </param>
            public Adler32(string textOrFile, bool strIsFilePath)
            {
                if (strIsFilePath)
                {
                    EncryptFile(textOrFile);
                    return;
                }
                Encrypt(textOrFile);
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Adler32"/> class and encrypts
            ///     the specified text.
            /// </summary>
            /// <param name="str">
            ///     The text to encrypt
            /// </param>
            public Adler32(string str) =>
                Encrypt(str);

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     stream is null.
            /// </exception>
            [SuppressMessage("ReSharper", "ShiftExpressionResultEqualsZero")]
            public override void Encrypt(Stream stream)
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
                RawHash = (uia[1] << 16) | uia[0];
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override void Encrypt(string text) =>
                Encrypt(text?.ToBytesUtf8());

            /// <summary>
            ///     Determines whether this instance have same values as the specified
            ///     <see cref="Checksum"/> instance.
            /// </summary>
            /// <param name="other">
            ///     The <see cref="Checksum"/> instance to compare.
            /// </param>
            public override bool Equals(Checksum other) =>
                other is Adler32 adler32 && RawHash == adler32.RawHash;

            /// <summary>
            ///     Converts the <see cref="RawHash"/> of this instance to its equivalent
            ///     string representation.
            /// </summary>
            public override string ToString() =>
                RawHash.ToStringDefault("x2").PadLeft(HashLength, '0');
        }

        /// <summary>
        ///     Provides functionality to compute Cyclic Redundancy Check (CRC-16) hashes.
        /// </summary>
        public sealed class Crc16 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 4;

            private const ushort Polynomial = 0xa001;
            private const ushort Seed = ushort.MaxValue;

            /// <summary>
            ///     Gets the raw data of computed hash.
            /// </summary>
            public new ushort RawHash { get; private set; }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc16"/> class.
            /// </summary>
            public Crc16() { }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc16"/> class and encrypts
            ///     the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public Crc16(Stream stream) =>
                Encrypt(stream);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc16"/> class and encrypts
            ///     the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt
            /// </param>
            public Crc16(byte[] bytes) =>
                Encrypt(bytes);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc16"/> class and encrypts
            ///     the specified text or file.
            /// </summary>
            /// <param name="textOrFile">
            ///     The text or file to encrypt
            /// </param>
            /// <param name="strIsFilePath">
            ///     <see langword="true"/> if the specified value is a file path; otherwise,
            ///     <see langword="false"/>
            /// </param>
            public Crc16(string textOrFile, bool strIsFilePath)
            {
                if (strIsFilePath)
                {
                    EncryptFile(textOrFile);
                    return;
                }
                Encrypt(textOrFile);
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc16"/> class and encrypts
            ///     the specified text.
            /// </summary>
            /// <param name="str">
            ///     The text to encrypt
            /// </param>
            public Crc16(string str) =>
                Encrypt(str);

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override void Encrypt(Stream stream)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                int i, x = Seed;
                while ((i = stream.ReadByte()) != -1)
                    for (var j = 0; j < 8; j++)
                    {
                        x = ((x ^ i) & 1) == 1 ? (x >> 1) ^ Polynomial : x >> 1;
                        i >>= 1;
                    }
                RawHash = (ushort)(((byte)(x % 256) << 8) | (byte)(x / 256));
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override void Encrypt(string text) =>
                Encrypt(text?.ToBytesUtf8());

            /// <summary>
            ///     Determines whether this instance have same values as the specified
            ///     <see cref="Checksum"/> instance.
            /// </summary>
            /// <param name="other">
            ///     The <see cref="Checksum"/> instance to compare.
            /// </param>
            public override bool Equals(Checksum other) =>
                other is Crc16 crc16 && RawHash == crc16.RawHash;

            /// <summary>
            ///     Converts the <see cref="RawHash"/> of this instance to its equivalent
            ///     string representation.
            /// </summary>
            public override string ToString() =>
                RawHash.ToStringDefault("x2").PadLeft(HashLength, '0');
        }

        /// <summary>
        ///     Provides functionality to compute Cyclic Redundancy Check (CRC-32) hashes.
        /// </summary>
        public sealed class Crc32 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 8;

            private const uint Polynomial = 0xedb88320u;
            private const uint Seed = uint.MaxValue;
            private static IReadOnlyList<uint> _crcTable;

            /// <summary>
            ///     Gets the raw data of computed hash.
            /// </summary>
            public new uint RawHash { get; private set; }

            private static IReadOnlyList<uint> CrcTable
            {
                get
                {
                    if (_crcTable != null)
                        return _crcTable;
                    var table = new uint[256];
                    for (var i = 0; i < 256; i++)
                    {
                        var ui = (uint)i;
                        for (var j = 0; j < 8; j++)
                            ui = (ui & 1) == 1 ? (ui >> 1) ^ Polynomial : ui >> 1;
                        table[i] = ui;
                    }
                    _crcTable = table;
                    return _crcTable;
                }
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc32"/> class.
            /// </summary>
            public Crc32() { }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc32"/> class and encrypts
            ///     the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public Crc32(Stream stream) =>
                Encrypt(stream);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc32"/> class and encrypts
            ///     the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt
            /// </param>
            public Crc32(byte[] bytes) =>
                Encrypt(bytes);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc32"/> class and encrypts
            ///     the specified text or file.
            /// </summary>
            /// <param name="textOrFile">
            ///     The text or file to encrypt
            /// </param>
            /// <param name="strIsFilePath">
            ///     <see langword="true"/> if the specified value is a file path; otherwise,
            ///     <see langword="false"/>
            /// </param>
            public Crc32(string textOrFile, bool strIsFilePath)
            {
                if (strIsFilePath)
                {
                    EncryptFile(textOrFile);
                    return;
                }
                Encrypt(textOrFile);
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc32"/> class and encrypts
            ///     the specified text.
            /// </summary>
            /// <param name="str">
            ///     The text to encrypt
            /// </param>
            public Crc32(string str) =>
                Encrypt(str);

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override void Encrypt(Stream stream)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));

                /* old code without table
                int i;
                var ui = Seed;
                while ((i = stream.ReadByte()) != -1)
                {
                    ui ^= (uint)i;
                    for (var j = 0; j < 8; j++)
                        ui = (uint)((ui >> 1) ^ (Polynomial & -(ui & 1)));
                }
                RawHash = ~ui;
                */

                int i;
                var ui = Seed;
                while ((i = stream.ReadByte()) != -1)
                    ui = (ui >> 8) ^ CrcTable[(int)(i ^ (ui & 0xff))];
                RawHash = ~ui;
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override void Encrypt(string text) =>
                Encrypt(text?.ToBytesUtf8());

            /// <summary>
            ///     Determines whether this instance have same values as the specified
            ///     <see cref="Checksum"/> instance.
            /// </summary>
            /// <param name="other">
            ///     The <see cref="Checksum"/> instance to compare.
            /// </param>
            public override bool Equals(Checksum other) =>
                other is Crc32 crc32 && RawHash == crc32.RawHash;

            /// <summary>
            ///     Converts the <see cref="RawHash"/> of this instance to its equivalent
            ///     string representation.
            /// </summary>
            public override string ToString() =>
                RawHash.ToStringDefault("x2").PadLeft(HashLength, '0');
        }

        /// <summary>
        ///     Provides functionality to compute Cyclic Redundancy Check (CRC-64) hashes.
        /// </summary>
        public sealed class Crc64 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 16;

            private const ulong Polynomial = 0xd800000000000000uL;
            private const ulong Seed = ulong.MinValue;
            private static IReadOnlyList<ulong> _crcTable;

            /// <summary>
            ///     Gets the raw data of computed hash.
            /// </summary>
            public new ulong RawHash { get; private set; }

            private static IReadOnlyList<ulong> CrcTable
            {
                get
                {
                    if (_crcTable != null)
                        return _crcTable;
                    var table = new ulong[256];
                    for (var i = 0; i < 256; i++)
                    {
                        var ul = (ulong)i;
                        for (var j = 0; j < 8; j++)
                            ul = (ul & 1) == 1 ? (ul >> 1) ^ Polynomial : ul >> 1;
                        table[i] = ul;
                    }
                    _crcTable = table;
                    return _crcTable;
                }
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc64"/> class.
            /// </summary>
            public Crc64() { }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc64"/> class and encrypts
            ///     the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public Crc64(Stream stream) =>
                Encrypt(stream);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc64"/> class and encrypts
            ///     the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt
            /// </param>
            public Crc64(byte[] bytes) =>
                Encrypt(bytes);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc64"/> class and encrypts
            ///     the specified text or file.
            /// </summary>
            /// <param name="textOrFile">
            ///     The text or file to encrypt
            /// </param>
            /// <param name="strIsFilePath">
            ///     <see langword="true"/> if the specified value is a file path; otherwise,
            ///     <see langword="false"/>
            /// </param>
            public Crc64(string textOrFile, bool strIsFilePath)
            {
                if (strIsFilePath)
                {
                    EncryptFile(textOrFile);
                    return;
                }
                Encrypt(textOrFile);
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Crc64"/> class and encrypts
            ///     the specified text.
            /// </summary>
            /// <param name="str">
            ///     The text to encrypt
            /// </param>
            public Crc64(string str) =>
                Encrypt(str);

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override void Encrypt(Stream stream)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                int i;
                var ul = Seed;
                while ((i = stream.ReadByte()) != -1)
                    ul = (ul >> 8) ^ CrcTable[(int)(i ^ (long)ul) & 0xff];
                RawHash = ~ul;
            }

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override void Encrypt(string text) =>
                Encrypt(text?.ToBytesUtf8());

            /// <summary>
            ///     Determines whether this instance have same values as the specified
            ///     <see cref="Checksum"/> instance.
            /// </summary>
            /// <param name="other">
            ///     The <see cref="Checksum"/> instance to compare.
            /// </param>
            public override bool Equals(Checksum other) =>
                other is Crc64 crc64 && RawHash == crc64.RawHash;

            /// <summary>
            ///     Converts the <see cref="RawHash"/> of this instance to its equivalent
            ///     string representation.
            /// </summary>
            public override string ToString() =>
                RawHash.ToStringDefault("x2").PadLeft(HashLength, '0');
        }

        /// <summary>
        ///     Provides functionality to compute Message-Digest 5 (MD5) hashes.
        /// </summary>
        public sealed class Md5 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 32;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Md5"/> class.
            /// </summary>
            public Md5() { }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Md5"/> class and encrypts the
            ///     specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public Md5(Stream stream) =>
                Encrypt(stream);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Md5"/> class and encrypts the
            ///     specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt
            /// </param>
            public Md5(byte[] bytes) =>
                Encrypt(bytes);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Md5"/> class and encrypts the
            ///     specified text or file.
            /// </summary>
            /// <param name="textOrFile">
            ///     The text or file to encrypt
            /// </param>
            /// <param name="strIsFilePath">
            ///     <see langword="true"/> if the specified value is a file path; otherwise,
            ///     <see langword="false"/>
            /// </param>
            public Md5(string textOrFile, bool strIsFilePath)
            {
                if (strIsFilePath)
                {
                    EncryptFile(textOrFile);
                    return;
                }
                Encrypt(textOrFile);
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Md5"/> class and encrypts the
            ///     specified text.
            /// </summary>
            /// <param name="str">
            ///     The text to encrypt
            /// </param>
            public Md5(string str) =>
                Encrypt(str);

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override void Encrypt(Stream stream) =>
                Encrypt(stream, new MD5CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override void Encrypt(string text)
            {
                var algo = default(MD5);
                try
                {
                    algo = MD5.Create();
                    Encrypt(text, algo);
                }
                finally
                {
                    algo?.Dispose();
                }
            }
        }

        /// <summary>
        ///     Provides functionality to compute Secure Hash Algorithm 1 (SHA-1) hashes.
        /// </summary>
        public sealed class Sha1 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 40;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha1"/> class.
            /// </summary>
            public Sha1() { }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha1"/> class and encrypts the
            ///     specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public Sha1(Stream stream) =>
                Encrypt(stream);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha1"/> class and encrypts the
            ///     specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt
            /// </param>
            public Sha1(byte[] bytes) =>
                Encrypt(bytes);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha1"/> class and encrypts the
            ///     specified text or file.
            /// </summary>
            /// <param name="textOrFile">
            ///     The text or file to encrypt
            /// </param>
            /// <param name="strIsFilePath">
            ///     <see langword="true"/> if the specified value is a file path; otherwise,
            ///     <see langword="false"/>
            /// </param>
            public Sha1(string textOrFile, bool strIsFilePath)
            {
                if (strIsFilePath)
                {
                    EncryptFile(textOrFile);
                    return;
                }
                Encrypt(textOrFile);
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha1"/> class and encrypts the
            ///     specified text.
            /// </summary>
            /// <param name="str">
            ///     The text to encrypt
            /// </param>
            public Sha1(string str) =>
                Encrypt(str);

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override void Encrypt(Stream stream) =>
                Encrypt(stream, new SHA1CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override void Encrypt(string text)
            {
                var algo = default(SHA1);
                try
                {
                    algo = SHA1.Create();
                    Encrypt(text, algo);
                }
                finally
                {
                    algo?.Dispose();
                }
            }
        }

        /// <summary>
        ///     Provides functionality to compute Secure Hash Algorithm 2 (SHA-256) hashes.
        /// </summary>
        public sealed class Sha256 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 64;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha256"/> class.
            /// </summary>
            public Sha256() { }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha256"/> class and encrypts
            ///     the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public Sha256(Stream stream) =>
                Encrypt(stream);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha256"/> class and encrypts
            ///     the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt
            /// </param>
            public Sha256(byte[] bytes) =>
                Encrypt(bytes);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha256"/> class and encrypts
            ///     the specified text or file.
            /// </summary>
            /// <param name="textOrFile">
            ///     The text or file to encrypt
            /// </param>
            /// <param name="strIsFilePath">
            ///     <see langword="true"/> if the specified value is a file path; otherwise,
            ///     <see langword="false"/>
            /// </param>
            public Sha256(string textOrFile, bool strIsFilePath)
            {
                if (strIsFilePath)
                {
                    EncryptFile(textOrFile);
                    return;
                }
                Encrypt(textOrFile);
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha256"/> class and encrypts
            ///     the specified text.
            /// </summary>
            /// <param name="str">
            ///     The text to encrypt
            /// </param>
            public Sha256(string str) =>
                Encrypt(str);

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override void Encrypt(Stream stream) =>
                Encrypt(stream, new SHA256CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override void Encrypt(string text)
            {
                var algo = default(SHA256);
                try
                {
                    algo = SHA256.Create();
                    Encrypt(text, algo);
                }
                finally
                {
                    algo?.Dispose();
                }
            }
        }

        /// <summary>
        ///     Provides functionality to compute Secure Hash Algorithm 2 (SHA-384) hashes.
        /// </summary>
        public sealed class Sha384 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 96;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha384"/> class.
            /// </summary>
            public Sha384() { }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha384"/> class and encrypts
            ///     the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public Sha384(Stream stream) =>
                Encrypt(stream);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha384"/> class and encrypts
            ///     the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt
            /// </param>
            public Sha384(byte[] bytes) =>
                Encrypt(bytes);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha384"/> class and encrypts
            ///     the specified text or file.
            /// </summary>
            /// <param name="textOrFile">
            ///     The text or file to encrypt
            /// </param>
            /// <param name="strIsFilePath">
            ///     <see langword="true"/> if the specified value is a file path; otherwise,
            ///     <see langword="false"/>
            /// </param>
            public Sha384(string textOrFile, bool strIsFilePath)
            {
                if (strIsFilePath)
                {
                    EncryptFile(textOrFile);
                    return;
                }
                Encrypt(textOrFile);
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha384"/> class and encrypts
            ///     the specified text.
            /// </summary>
            /// <param name="str">
            ///     The text to encrypt
            /// </param>
            public Sha384(string str) =>
                Encrypt(str);

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override void Encrypt(Stream stream) =>
                Encrypt(stream, new SHA384CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override void Encrypt(string text)
            {
                var algo = default(SHA384);
                try
                {
                    algo = SHA384.Create();
                    Encrypt(text, algo);
                }
                finally
                {
                    algo?.Dispose();
                }
            }
        }

        /// <summary>
        ///     Provides functionality to compute Secure Hash Algorithm 2 (SHA-512) hashes.
        /// </summary>
        public sealed class Sha512 : Checksum
        {
            /// <summary>
            ///     Gets the required hash length.
            /// </summary>
            public const int HashLength = 128;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha512"/> class.
            /// </summary>
            public Sha512() { }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha512"/> class and encrypts
            ///     the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public Sha512(Stream stream) =>
                Encrypt(stream);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha512"/> class and encrypts
            ///     the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sequence of bytes to encrypt
            /// </param>
            public Sha512(byte[] bytes) =>
                Encrypt(bytes);

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha512"/> class and encrypts
            ///     the specified text or file.
            /// </summary>
            /// <param name="textOrFile">
            ///     The text or file to encrypt
            /// </param>
            /// <param name="strIsFilePath">
            ///     <see langword="true"/> if the specified value is a file path; otherwise,
            ///     <see langword="false"/>
            /// </param>
            public Sha512(string textOrFile, bool strIsFilePath)
            {
                if (strIsFilePath)
                {
                    EncryptFile(textOrFile);
                    return;
                }
                Encrypt(textOrFile);
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Sha512"/> class and encrypts
            ///     the specified text.
            /// </summary>
            /// <param name="str">
            ///     The text to encrypt
            /// </param>
            public Sha512(string str) =>
                Encrypt(str);

            /// <summary>
            ///     Encrypts the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The stream to encrypt.
            /// </param>
            public override void Encrypt(Stream stream) =>
                Encrypt(stream, new SHA512CryptoServiceProvider());

            /// <summary>
            ///     Encrypts the specified string.
            /// </summary>
            /// <param name="text">
            ///     The string to encrypt.
            /// </param>
            public override void Encrypt(string text)
            {
                var algo = default(SHA512);
                try
                {
                    algo = SHA512.Create();
                    Encrypt(text, algo);
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
