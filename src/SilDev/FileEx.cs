#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: FileEx.cs
// Version:  2020-01-14 14:16
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using Compression;
    using Intern;
    using Properties;

    /// <summary>
    ///     Provides static methods based on the <see cref="File"/> class to perform
    ///     file operations.
    /// </summary>
    public static class FileEx
    {
        private const int DatHeaderSize = 16;

        private static byte[] DefDatHeader =>
            new byte[]
            {
                // type version
                1, 0, 0, 0,

                // type identifier 1-5 ("CSDAT", "C# Data")
                (byte)'C',
                (byte)'S',
                (byte)'D',
                (byte)'A',
                (byte)'T',

                // compression mode
                0,

                // reserved for upcoming features
                0, 0, 0,
                0, 0, 0
            };

        private static bool DatHeaderIsValid(IReadOnlyList<byte> header)
        {
            if (header.Count != DatHeaderSize)
                return false;
            var defHeader = DefDatHeader;
            for (var i = 0; i < DatHeaderSize; i++)
                switch ((DatHeaderKey)i)
                {
                    case DatHeaderKey.CompressionMode:
                    {
                        var value = header[i];
                        if (!value.IsBetween((byte)0, (byte)1))
                            return false;
                        break;
                    }
                    default:
                    {
                        var value = header[i];
                        if (value != defHeader[i])
                            return false;
                        break;
                    }
                }
            return true;
        }

        /// <summary>
        ///     Creates a new file, writes the specified object graph into to the file, and
        ///     then closes the file.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source.
        /// </typeparam>
        /// <param name="path">
        ///     The file to create.
        /// </param>
        /// <param name="source">
        ///     The object graph to write to the file.
        /// </param>
        /// <param name="compress">
        ///     <see langword="true"/> to compress the file after serialization; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="overwrite">
        ///     <see langword="true"/> to allow an existing file to be overwritten;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool Serialize<TSource>(string path, TSource source, bool compress = false, bool overwrite = true)
        {
            try
            {
                var dest = PathEx.Combine(path);
                using (var fs = new FileStream(dest, overwrite ? FileMode.Create : FileMode.CreateNew))
                {
                    var header = DefDatHeader;
                    header[(int)DatHeaderKey.CompressionMode] = Convert.ToByte(compress);
                    fs.WriteBytes(header);
                    var bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File));
                    if (compress)
                    {
                        using var ms = new MemoryStream();
                        bf.Serialize(ms, source);
                        ms.Position = 0;
                        GZip.Compress(ms, fs, false);
                        return true;
                    }
                    bf.Serialize(fs, source);
                }
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Deserializes the specified file into an object graph.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type of the result.
        /// </typeparam>
        /// <param name="path">
        ///     The file to deserialize.
        /// </param>
        /// <param name="defValue">
        ///     The default value.
        /// </param>
        public static TResult Deserialize<TResult>(string path, TResult defValue = default)
        {
            try
            {
                var src = PathEx.Combine(path);
                if (!File.Exists(src))
                    return defValue;
                using var fs = new FileStream(src, FileMode.Open, FileAccess.Read);
                bool decompress;
                var header = new byte[DatHeaderSize];
                fs.Read(header, 0, header.Length);
                if (DatHeaderIsValid(header))
                    decompress = header[(int)DatHeaderKey.CompressionMode] > 0;
                else
                {
                    decompress = !GZip.Header.Where((b, i) => header[i] != b).Any();
                    fs.Position = 0;
                }
                var bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File));
                if (!decompress)
                    return (TResult)bf.Deserialize(fs);
                using var ms = new MemoryStream();
                GZip.Decompress(fs, ms, false);
                ms.Position = 0;
                return (TResult)bf.Deserialize(ms);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return defValue;
            }
        }

        /// <summary>
        ///     Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static bool Exists(string path)
        {
            var src = PathEx.Combine(path);
            return File.Exists(src);
        }

        /// <summary>
        ///     Determines whether the specified path specifies the specified file
        ///     attributes.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member that contains the file to check.
        /// </param>
        /// <param name="attr">
        ///     The attributes to match.
        /// </param>
        public static bool MatchAttributes(this FileInfo fileInfo, FileAttributes attr)
        {
            try
            {
                if (fileInfo == null)
                    throw new ArgumentNullException(nameof(fileInfo));
                var fa = fileInfo.Attributes;
                return (fa & attr) != 0;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified path specifies the specified file
        ///     attributes.
        /// </summary>
        /// <param name="path">
        ///     The file or directory to check.
        /// </param>
        /// <param name="attr">
        ///     The attributes to match.
        /// </param>
        public static bool MatchAttributes(string path, FileAttributes attr)
        {
            try
            {
                var src = PathEx.Combine(path);
                var fa = File.GetAttributes(src);
                return (fa & attr) != 0;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified file is hidden.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member that contains the file to check.
        /// </param>
        public static bool IsHidden(this FileInfo fileInfo) =>
            fileInfo?.MatchAttributes(FileAttributes.Hidden) ?? false;

        /// <summary>
        ///     Determines whether the specified file is hidden.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static bool IsHidden(string path) =>
            MatchAttributes(path, FileAttributes.Hidden);

        /// <summary>
        ///     Determines whether the specified file is specified as reparse point.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member that contains the file to check.
        /// </param>
        public static bool IsLink(this FileInfo fileInfo) =>
            fileInfo?.MatchAttributes(FileAttributes.ReparsePoint) ?? false;

        /// <summary>
        ///     Determines whether the specified file is specified as reparse point.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static bool IsLink(string path) =>
            MatchAttributes(path, FileAttributes.ReparsePoint);

        /// <summary>
        ///     Sets the specified attributes for the specified file.
        /// </summary>
        /// <param name="file">
        ///     The file to change.
        /// </param>
        /// <param name="attr">
        ///     The attributes to set.
        /// </param>
        public static void SetAttributes(string file, FileAttributes attr)
        {
            try
            {
                var src = PathEx.Combine(file);
                var fi = new FileInfo(src);
                if (!fi.Exists)
                    return;
                if (attr != FileAttributes.Normal)
                    fi.Attributes |= attr;
                else
                    fi.Attributes = attr;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Opens two files, reads the contents of the files in a byte array buffer to
        ///     compare whether the specified files are equal.
        /// </summary>
        /// <param name="fileInfo">
        ///     The first file to compare.
        /// </param>
        /// <param name="otherFileInfo">
        ///     The second file to compare.
        /// </param>
        public static bool ContentIsEqual(this FileInfo fileInfo, FileInfo otherFileInfo)
        {
            if (fileInfo == null)
                return otherFileInfo == null;
            if (otherFileInfo == null)
                return false;
            if (!fileInfo.Exists)
                return !fileInfo.Exists;
            if (fileInfo.Length != otherFileInfo.Length)
                return false;
            int count;
            if (fileInfo.Length < 8)
                count = (int)fileInfo.Length;
            else
            {
                var len = (double)fileInfo.Length;
                var log = Math.Log(len);
                if (fileInfo.Length < 1024)
                    count = (int)Math.Floor(len / log);
                else if (fileInfo.Length < uint.MaxValue)
                {
                    var sqrt = Math.Sqrt(len);
                    count = (int)Math.Floor(sqrt / log);
                }
                else
                    count = 4096;
            }
            var buffer1 = new byte[count];
            var buffer2 = new byte[count];
            using var fs1 = fileInfo.OpenRead();
            using var fs2 = otherFileInfo.OpenRead();
            int len1, len2;
            while ((len1 = fs1.Read(buffer1, 0, count)) > 0 && (len2 = fs2.Read(buffer2, 0, count)) > 0)
                if (len1 != len2 || len1 < count && !buffer1.Take(len1).SequenceEqual(buffer2.Take(len2)) || !buffer1.SequenceEqual(buffer2))
                    return false;
            return true;
        }

        /// <summary>
        ///     Opens two files, reads the contents of the files in a byte array buffer to
        ///     compare whether the specified files are equal.
        /// </summary>
        /// <param name="firstFile">
        ///     The first file to compare.
        /// </param>
        /// <param name="secondFile">
        ///     The second file to compare.
        /// </param>
        /// <exception cref="ArgumentInvalidException">
        ///     firstFile or secondFile is invalid.
        /// </exception>
        public static bool ContentIsEqual(string firstFile, string secondFile)
        {
            var first = PathEx.Combine(firstFile);
            if (!PathEx.IsValidPath(first))
                throw new ArgumentInvalidException(nameof(firstFile));
            var second = PathEx.Combine(secondFile);
            if (!PathEx.IsValidPath(second))
                throw new ArgumentInvalidException(nameof(secondFile));
            var fs1 = new FileInfo(first);
            var fs2 = new FileInfo(second);
            return fs1.ContentIsEqual(fs2);
        }

        /// <summary>
        ///     Opens a binary file, reads the contents of the file into a byte array, and
        ///     then closes the file.
        /// </summary>
        /// <param name="path">
        ///     The file to open for reading.
        /// </param>
        public static byte[] ReadAllBytes(string path)
        {
            var file = PathEx.Combine(path);
            try
            {
                return File.ReadAllBytes(file);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Opens a file, reads all lines of the file with the specified encoding, and
        ///     then closes the file.
        /// </summary>
        /// <param name="path">
        ///     The file to open for reading.
        /// </param>
        /// <param name="encoding">
        ///     The encoding applied to the contents of the file.
        /// </param>
        public static string[] ReadAllLines(string path, Encoding encoding = null)
        {
            var file = PathEx.Combine(path);
            try
            {
                return encoding != null ? File.ReadAllLines(file, encoding) : File.ReadAllLines(file);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Opens a file, reads all lines of the file with the specified encoding, and
        ///     then closes the file.
        /// </summary>
        /// <param name="path">
        ///     The file to open for reading.
        /// </param>
        /// <param name="encoding">
        ///     The encoding applied to the contents of the file.
        /// </param>
        public static string ReadAllText(string path, Encoding encoding = null)
        {
            var file = PathEx.Combine(path);
            try
            {
                return encoding != null ? File.ReadAllText(file, encoding) : File.ReadAllText(file);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Appends lines to a file by using a specified encoding, and then closes the
        ///     file. If the specified file does not exist, this method creates a file,
        ///     writes the specified lines to the file, and then closes the file.
        /// </summary>
        /// <param name="path">
        ///     The file to append the lines to. The file is created if it doesn't already
        ///     exist.
        /// </param>
        /// <param name="contents">
        ///     The lines to append to the file.
        /// </param>
        /// <param name="encoding">
        ///     The character encoding to use.
        /// </param>
        public static bool AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding = null)
        {
            var file = PathEx.Combine(path);
            try
            {
                var dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentInvalidException(nameof(path));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                if (encoding != null)
                    File.AppendAllLines(file, contents, encoding);
                else
                    File.AppendAllLines(file, contents);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
            return File.Exists(file);
        }

        /// <summary>
        ///     Appends the specified string to the file, creating the file if it does not
        ///     already exist.
        /// </summary>
        /// <param name="path">
        ///     The file to append the specified string to.
        /// </param>
        /// <param name="contents">
        ///     The string to append to the file.
        /// </param>
        /// <param name="encoding">
        ///     The character encoding to use.
        /// </param>
        public static bool AppendAllText(string path, string contents, Encoding encoding = null)
        {
            var file = PathEx.Combine(path);
            try
            {
                var dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentInvalidException(nameof(path));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                if (encoding != null)
                    File.AppendAllText(file, contents, encoding);
                else
                    File.AppendAllText(file, contents);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
            return File.Exists(file);
        }

        /// <summary>
        ///     Replaces all occurrences of a specified sequence of bytes in the specified
        ///     file with another sequence of bytes.
        /// </summary>
        /// <param name="file">
        ///     The file to overwrite.
        /// </param>
        /// <param name="oldValue">
        ///     The sequence of bytes to be replaced.
        /// </param>
        /// <param name="newValue">
        ///     The sequence of bytes to replace all all occurrences of oldValue.
        /// </param>
        /// <param name="backup">
        ///     <see langword="true"/> to create a backup; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="offsets">
        ///     A list with all positions where bytes were overwritten.
        /// </param>
        public static bool BinaryReplace(string file, byte[] oldValue, byte[] newValue, bool backup, out List<long[]> offsets)
        {
            offsets = new List<long[]>();

            try
            {
                if (file == null)
                    throw new ArgumentNullException(nameof(file));
                if (oldValue == null)
                    throw new ArgumentNullException(nameof(oldValue));
                if (newValue == null)
                    throw new ArgumentNullException(nameof(newValue));
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }

            var targetPath = PathEx.Combine(file);
            try
            {
                if (!File.Exists(targetPath))
                    throw new PathNotFoundException(targetPath);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }

            var backupPath = $"{targetPath}.backup";
            for (var i = 0; i < short.MaxValue; i++)
            {
                var path = backupPath + i;
                if (File.Exists(path))
                    continue;
                backupPath = path;
                break;
            }
            try
            {
                File.Move(targetPath, backupPath);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }

            FileStream sourceStream = null, targetStream = null;
            var result = false;
            try
            {
                sourceStream = File.OpenRead(backupPath);
                targetStream = File.Create(targetPath);
                int index = 0, position;
                var offset = -1L;
                while ((position = sourceStream.ReadByte()) != -1)
                {
                    if (oldValue[index] == position)
                    {
                        if (index == oldValue.Length - 1)
                        {
                            var offsetStart = targetStream.Length;
                            var offsetEnd = offsetStart + newValue.Length;
                            var offsetList = new List<long>();
                            for (var i = offsetStart; i < offsetEnd; i++)
                                offsetList.Add(i);
                            offsets.Add(offsetList.ToArray());

                            targetStream.Write(newValue, 0, newValue.Length);
                            offset = -1;
                            index = 0;
                            continue;
                        }
                        if (index == 0)
                            offset = sourceStream.Position - 1;
                        ++index;
                        continue;
                    }
                    if (index == 0)
                    {
                        targetStream.WriteByte((byte)position);
                        continue;
                    }
                    targetStream.WriteByte(oldValue[0]);
                    sourceStream.Position = offset + 1;
                    offset = -1;
                    index = 0;
                }
                result = true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                sourceStream?.Dispose();
                targetStream?.Dispose();
            }

            if (!result)
            {
                try
                {
                    if (File.Exists(backupPath))
                    {
                        if (File.Exists(targetPath))
                            File.Delete(targetPath);
                        File.Move(backupPath, targetPath);
                    }
                }
                catch (Exception exc) when (exc.IsCaught())
                {
                    Log.Write(exc);
                }
                return false;
            }

            if (backup)
            {
                try
                {
                    if (File.Exists(backupPath) && File.Exists(targetPath))
                    {
                        var backupHash = new Crypto.Md5(backupPath, true);
                        var targetHash = new Crypto.Md5(targetPath, true);
                        if (backupHash == targetHash)
                            File.Delete(backupPath);
                    }
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                return true;
            }

            try
            {
                if (File.Exists(backupPath))
                    File.Delete(backupPath);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return true;
        }

        /// <summary>
        ///     Replaces all occurrences of a specified sequence of bytes in the specified
        ///     file with another sequence of bytes.
        /// </summary>
        /// <param name="file">
        ///     The file to overwrite.
        /// </param>
        /// <param name="oldValue">
        ///     The sequence of bytes to be replaced.
        /// </param>
        /// <param name="newValue">
        ///     The sequence of bytes to replace all all occurrences of oldValue.
        /// </param>
        /// <param name="backup">
        ///     <see langword="true"/> to create a backup; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static bool BinaryReplace(string file, byte[] oldValue, byte[] newValue, bool backup = true) =>
            BinaryReplace(file, oldValue, newValue, backup, out var _);

        /// <summary>
        ///     Creates a new file, writes the specified byte array to the file, and then
        ///     closes the file.
        /// </summary>
        /// <param name="path">
        ///     The file to write to.
        /// </param>
        /// <param name="bytes">
        ///     The bytes to write to the file.
        /// </param>
        /// <param name="overwrite">
        ///     <see langword="true"/> to allow an existing file to be overwritten;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool WriteAllBytes(string path, IEnumerable<byte> bytes, bool overwrite = true)
        {
            var file = PathEx.Combine(path);
            if (!overwrite && File.Exists(file))
                return true;
            try
            {
                var dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentInvalidException(nameof(path));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllBytes(file, bytes.ToArray());
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
            return File.Exists(file);
        }

        /// <summary>
        ///     Creates a new file by using the specified encoding, writes a collection of
        ///     strings to the file, and then closes the file.
        /// </summary>
        /// <param name="path">
        ///     The file to write to.
        /// </param>
        /// <param name="contents">
        ///     The lines to write to the file.
        /// </param>
        /// <param name="encoding">
        ///     The character encoding to use.
        /// </param>
        /// <param name="overwrite">
        ///     <see langword="true"/> to allow an existing file to be overwritten;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding, bool overwrite = true)
        {
            var file = PathEx.Combine(path);
            if (!overwrite && File.Exists(file))
                return true;
            try
            {
                var dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentInvalidException(nameof(path));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllLines(file, contents, encoding);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
            return File.Exists(file);
        }

        /// <summary>
        ///     Creates a new file, writes a collection of strings to the file, and then
        ///     closes the file.
        /// </summary>
        /// <param name="path">
        ///     The file to write to.
        /// </param>
        /// <param name="contents">
        ///     The lines to write to the file.
        /// </param>
        /// <param name="overwrite">
        ///     <see langword="true"/> to allow an existing file to be overwritten;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool WriteAllLines(string path, IEnumerable<string> contents, bool overwrite = true)
        {
            var file = PathEx.Combine(path);
            if (!overwrite && File.Exists(file))
                return true;
            try
            {
                var dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentInvalidException(nameof(path));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllLines(file, contents);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
            return File.Exists(file);
        }

        /// <summary>
        ///     Creates a new file, writes the specified string to the file using the
        ///     specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">
        ///     The file to write to.
        /// </param>
        /// <param name="contents">
        ///     The string to write to the file.
        /// </param>
        /// <param name="encoding">
        ///     The encoding to apply to the string.
        /// </param>
        /// <param name="overwrite">
        ///     <see langword="true"/> to allow an existing file to be overwritten;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool WriteAllText(string path, string contents, Encoding encoding, bool overwrite = true)
        {
            var file = PathEx.Combine(path);
            if (!overwrite && File.Exists(file))
                return true;
            try
            {
                var dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentInvalidException(nameof(path));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(file, contents, encoding);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
            return File.Exists(file);
        }

        /// <summary>
        ///     Creates a new file, writes the specified string to the file, and then
        ///     closes the file.
        /// </summary>
        /// <param name="path">
        ///     The file to write to.
        /// </param>
        /// <param name="contents">
        ///     The string to write to the file.
        /// </param>
        /// <param name="overwrite">
        ///     <see langword="true"/> to allow an existing file to be overwritten;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool WriteAllText(string path, string contents, bool overwrite = true)
        {
            var file = PathEx.Combine(path);
            if (!overwrite && File.Exists(file))
                return true;
            try
            {
                var dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentInvalidException(nameof(path));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(file, contents);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
            return File.Exists(file);
        }

        /// <summary>
        ///     Creates an empty file in the specified path.
        /// </summary>
        /// <param name="path">
        ///     The path and name of the file to create.
        /// </param>
        /// <param name="overwrite">
        ///     <see langword="true"/> to overwrite an existing file; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static bool Create(string path, bool overwrite = false)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var file = PathEx.Combine(path);
                if (!overwrite && File.Exists(file))
                    return true;
                var dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir))
                    throw new ArgumentInvalidException(nameof(path));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.Create(file).Close();
                if (!File.Exists(file))
                    throw new PathNotFoundException(file);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Copies an existing file to a new location.
        /// </summary>
        /// <param name="srcFile">
        ///     The file to copy.
        /// </param>
        /// <param name="destFile">
        ///     The fully qualified name of the destination file.
        /// </param>
        /// <param name="overwrite">
        ///     <see langword="true"/> to allow an existing file to be overwritten;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool Copy(string srcFile, string destFile, bool overwrite = false)
        {
            var src = PathEx.Combine(srcFile);
            try
            {
                var fi = new FileInfo(src);
                if (!fi.Exists)
                    throw new PathNotFoundException(fi.FullName);
                var dest = PathEx.Combine(destFile);
                if (File.Exists(dest))
                {
                    if (!overwrite)
                        return true;
                    if (!IsLink(src))
                        SetAttributes(src, FileAttributes.Normal);
                    File.Delete(dest);
                }
                else
                {
                    if (!fi.Directory?.Exists ?? false)
                        fi.Directory.Create();
                }
                fi.CopyTo(dest);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Copies an existing file to a new location and deletes the source file if
        ///     this task has been completed successfully.
        /// </summary>
        /// <param name="srcFile">
        ///     The file to move.
        /// </param>
        /// <param name="destFile">
        ///     The fully qualified name of the destination file.
        /// </param>
        /// <param name="overwrite">
        ///     <see langword="true"/> to allow an existing file to be overwritten;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool Move(string srcFile, string destFile, bool overwrite = false)
        {
            if (!Copy(srcFile, destFile, overwrite))
                return false;
            var src = PathEx.Combine(srcFile);
            var dest = PathEx.Combine(destFile);
            try
            {
                if (!overwrite && !ContentIsEqual(src, dest))
                    throw new AggregateException();
                File.Delete(src);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Deletes the specified file, if it exists.
        /// </summary>
        /// <param name="path">
        ///     The path of the file to be deleted.
        /// </param>
        /// <exception cref="IOException">
        ///     See <see cref="File.Delete(string)"/>.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     See <see cref="File.Delete(string)"/>.
        /// </exception>
        public static bool Delete(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;
            var file = PathEx.Combine(path);
            if (!File.Exists(file))
                return true;
            if (!IsLink(file))
                SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
            return true;
        }

        /// <summary>
        ///     Tries to delete the specified file.
        /// </summary>
        /// <param name="path">
        ///     The path of the file to be deleted.
        /// </param>
        public static bool TryDelete(string path)
        {
            try
            {
                return Delete(path);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Returns a unique name starting with a given prefix, followed by a hash of
        ///     the specified length and a specified suffix.
        /// </summary>
        /// <param name="prefix">
        ///     This text is at the beginning of the name.
        ///     <para>
        ///         Uppercase letters are converted to lowercase letters. Supported
        ///         characters are only from '0' to '9' and from 'a' to 'z' but can be
        ///         completely empty to omit the prefix.
        ///     </para>
        /// </param>
        /// <param name="suffix">
        ///     This text is at the end of the name.
        ///     <para>
        ///         If it does not begin with a dot, it will be added. Uppercase letters
        ///         are converted to lowercase letters. Supported characters are only from
        ///         '0' to '9' and from 'a' to 'z' but can be completely empty to omit the
        ///         suffix.
        ///     </para>
        /// </param>
        /// <param name="hashLen">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     hashLen is not between 4 and 24.
        /// </exception>
        /// <exception cref="ArgumentInvalidException">
        ///     prefix or suffix contains invalid characters.
        /// </exception>
        public static string GetUniqueName(string prefix = "tmp", string suffix = ".tmp", int hashLen = 4) =>
            PathEx.GetUniqueName(prefix, suffix, hashLen);

        /// <summary>
        ///     Returns a unique name starting with 'tmp' prefix, followed by a hash of the
        ///     specified length and '.tmp' suffix.
        /// </summary>
        /// <param name="hashLen">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     hashLen is not between 4 and 24.
        /// </exception>
        public static string GetUniqueName(int hashLen) =>
            PathEx.GetUniqueName("tmp", ".tmp", hashLen);

        /// <summary>
        ///     Returns the current user's temporary path in combination with unique name
        ///     starting with a given prefix, followed by a hash of the specified length
        ///     and a specified suffix.
        /// </summary>
        /// <param name="prefix">
        ///     This text is at the beginning of the name.
        ///     <para>
        ///         Uppercase letters are converted to lowercase letters. Supported
        ///         characters are only from '0' to '9' and from 'a' to 'z' but can be
        ///         completely empty to omit the prefix.
        ///     </para>
        /// </param>
        /// <param name="suffix">
        ///     This text is at the end of the name.
        ///     <para>
        ///         If it does not begin with a dot, it will be added. Uppercase letters
        ///         are converted to lowercase letters. Supported characters are only from
        ///         '0' to '9' and from 'a' to 'z' but can be completely empty to omit the
        ///         suffix.
        ///     </para>
        /// </param>
        /// <param name="hashLen">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     hashLen is not between 4 and 24.
        /// </exception>
        /// <exception cref="ArgumentInvalidException">
        ///     prefix or suffix contains invalid characters.
        /// </exception>
        public static string GetUniqueTempPath(string prefix = "tmp", string suffix = ".tmp", int hashLen = 4) =>
            PathEx.GetUniquePath("%TEMP%", prefix, suffix, hashLen);

        /// <summary>
        ///     Returns the current user's temporary path in combination with unique name
        ///     starting with 'tmp' prefix, followed by a hash of the specified length and
        ///     '.tmp' suffix.
        /// </summary>
        /// <param name="hashLen">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     hashLen is not between 4 and 24.
        /// </exception>
        public static string GetUniqueTempPath(int hashLen) =>
            PathEx.GetUniquePath("%TEMP%", "tmp", ".tmp", hashLen);

        /// <summary>
        ///     Creates a link to the specified file.
        /// </summary>
        /// <param name="targetPath">
        ///     The file to be linked.
        /// </param>
        /// <param name="linkPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="startArgs">
        ///     The arguments which applies when the link is started.
        /// </param>
        /// <param name="iconLocation">
        ///     The icon resource path and resource identifier.
        /// </param>
        /// <param name="skipExists">
        ///     <see langword="true"/> to skip existing shortcuts, even if the target path
        ///     of the same; otherwise, <see langword="false"/>.
        /// </param>
        public static bool CreateShellLink(string targetPath, string linkPath, string startArgs = null, (string, int) iconLocation = default, bool skipExists = false)
        {
            if (PathEx.IsDir(targetPath))
                return false;
            var linkInfo = new ShellLinkInfo
            {
                Arguments = startArgs,
                IconLocation = iconLocation,
                LinkPath = linkPath,
                TargetPath = targetPath
            };
            return ShellLink.Create(linkInfo, skipExists);
        }

        /// <summary>
        ///     Creates a link to the specified file.
        /// </summary>
        /// <param name="targetPath">
        ///     The file to be linked.
        /// </param>
        /// <param name="linkPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="startArgs">
        ///     The arguments which applies when the link is started.
        /// </param>
        /// <param name="skipExists">
        ///     <see langword="true"/> to skip existing shortcuts, even if the target path
        ///     of the same; otherwise, <see langword="false"/>.
        /// </param>
        public static bool CreateShellLink(string targetPath, string linkPath, string startArgs, bool skipExists) =>
            CreateShellLink(targetPath, linkPath, startArgs, (null, 0), skipExists);

        /// <summary>
        ///     Creates a link to the specified file.
        /// </summary>
        /// <param name="targetPath">
        ///     The file to be linked.
        /// </param>
        /// <param name="linkPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="skipExists">
        ///     <see langword="true"/> to skip existing shortcuts, even if the target path
        ///     of the same; otherwise, <see langword="false"/>.
        /// </param>
        public static bool CreateShellLink(string targetPath, string linkPath, bool skipExists) =>
            CreateShellLink(targetPath, linkPath, null, (null, 0), skipExists);

        /// <summary>
        ///     Removes a link of the specified file.
        /// </summary>
        /// <param name="path">
        ///     The shortcut to be removed.
        /// </param>
        public static bool DestroyShellLink(string path) =>
            ShellLink.Destroy(path);

        /// <summary>
        ///     Returns the target path of the specified link if the target is a file.
        /// </summary>
        /// <param name="path">
        ///     The link to get the target path.
        /// </param>
        public static string GetShellLinkTarget(string path)
        {
            var target = ShellLink.GetTarget(path);
            return !PathEx.IsDir(target) ? target : string.Empty;
        }

        /// <summary>
        ///     Creates a symbolic link to the specified file based on command prompt which
        ///     allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="linkPath">
        ///     The file to be linked.
        /// </param>
        /// <param name="destFile">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="backup">
        ///     <see langword="true"/> to create an backup for existing files; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="elevated">
        ///     <see langword="true"/> to create this link with highest privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool CreateSymbolicLink(string linkPath, string destFile, bool backup = false, bool elevated = false) =>
            SymbolicLink.Create(linkPath, destFile, false, backup, elevated);

        /// <summary>
        ///     Removes an symbolic link of the specified file link based on command prompt
        ///     which allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="path">
        ///     The link to be removed.
        /// </param>
        /// <param name="backup">
        ///     <see langword="true"/> to restore found backups; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="elevated">
        ///     <see langword="true"/> to remove this link with highest privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool DestroySymbolicLink(string path, bool backup = false, bool elevated = false) =>
            SymbolicLink.Destroy(path, false, backup, elevated);

        /// <summary>
        ///     Returns processes that have locked the specified files.
        /// </summary>
        /// <param name="files">
        ///     The files to check.
        /// </param>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static IEnumerable<Process> GetLocks(IEnumerable<string> files)
        {
            var paths = files?.Select(PathEx.Combine).ToArray();
            if (!paths?.Any() ?? true)
                yield break;
            WinApi.ThrowError(WinApi.NativeMethods.RmStartSession(out var handle, 0, Guid.NewGuid().ToString()));
            IEnumerable<int> procIds;
            try
            {
                WinApi.ThrowError(WinApi.NativeMethods.RmRegisterResources(handle, (uint)paths.Length, paths, 0u, null, 0u, null));
                var pnProcInfo = 0u;
                var lpdwRebootReasons = 0u;
                if (WinApi.ThrowError(WinApi.NativeMethods.RmGetList(handle, out var pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons), 0, 234) == 0)
                    yield break;
                var processInfo = new WinApi.RmProcessInfo[pnProcInfoNeeded];
                pnProcInfo = pnProcInfoNeeded;
                WinApi.ThrowError(WinApi.NativeMethods.RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons));
                procIds = processInfo.Select(e => e.Process.dwProcessId);
            }
            finally
            {
                _ = WinApi.NativeMethods.RmEndSession(handle);
            }
            foreach (var id in procIds)
            {
                Process proc;
                try
                {
                    proc = Process.GetProcessById(id);
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    if (Log.DebugMode > 1)
                        Log.Write(ex);
                    continue;
                }
                yield return proc;
            }
        }

        /// <summary>
        ///     Find out which processes have a lock on this file instance member.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member to check.
        /// </param>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static IEnumerable<Process> GetLocks(this FileInfo fileInfo)
        {
            var path = fileInfo?.FullName;
            return path != null ? GetLocks(new[] { path }) : null;
        }

        /// <summary>
        ///     Gets the subject distinguished name from the certificate of the specified
        ///     file.
        /// </summary>
        /// <param name="path">
        ///     The path to the file to be checked.
        /// </param>
        /// <param name="multiLine">
        ///     <see langword="true"/> if the return string should contain carriage
        ///     returns; otherwise, <see langword="false"/>.
        /// </param>
        public static string GetSignatureSubject(string path, bool multiLine = true)
        {
            if (path == null)
                return null;
            var file = PathEx.Combine(path);
            if (!File.Exists(file))
                return null;
            try
            {
                using var cert1 = X509Certificate.CreateFromSignedFile(file);
                using var cert2 = new X509Certificate2(cert1);
                return cert2.SubjectName.Format(multiLine);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Gets the certificate status of the specified file.
        /// </summary>
        /// <param name="path">
        ///     The path to the file to be checked.
        /// </param>
        public static string GetSignatureStatus(string path)
        {
            if (path == null)
                return null;

            var file = PathEx.Combine(path);
            if (!File.Exists(file))
                return null;

            try
            {
                if (PowerShellReference.Assembly == null)
                    throw new NotSupportedException(ExceptionMessages.AssemblyNotFound);

                const string rcClass = "System.Management.Automation.Runspaces.RunspaceConfiguration";
                const string rcFunc = "Create";
                var rcType = PowerShellReference.Assembly.GetType(rcClass, true);
                var rcMethod = rcType.GetMethods().First(x => x.Name == rcFunc && x.GetParameters().Length == 0);

                const string rfClass = "System.Management.Automation.Runspaces.RunspaceFactory";
                const string rfFunc = "CreateRunspace";
                const string rfPara = "runspaceConfiguration";
                var rfType = PowerShellReference.Assembly.GetType(rfClass, true);
                var rfMethod = rfType.GetMethods().First(x => x.Name == rfFunc && x.GetParameters().FirstOrDefault()?.Name == rfPara);

                using var rf = (dynamic)rfMethod.Invoke(null, new[]
                {
                    (dynamic)rcMethod.Invoke(null, null)
                });
                rf.Open();
                using var p = rf.CreatePipeline();
                const string command = "Get-AuthenticodeSignature \"{0}\"";
                p.Commands.AddScript(command.FormatCurrent(path));
                var s = p.Invoke()[0];
                rf.Close();
                return s.Status.ToString();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Returns the highest version information associated with this file instance
        ///     member.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member to check.
        /// </param>
        public static Version GetVersion(this FileInfo fileInfo) =>
            GetVersion(fileInfo?.FullName);

        /// <summary>
        ///     Returns the highest version information associated with the specified file.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static Version GetVersion(string path)
        {
            Version v;
            try
            {
                var v1 = GetFileVersion(path);
                var v2 = GetProductVersion(path);
                v = v1 > v2 ? v1 : v2;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return new Version("0.0.0.0");
            }
            return v;
        }

        /// <summary>
        ///     Returns the file version information associated with this file instance
        ///     member.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member to check.
        /// </param>
        public static Version GetFileVersion(this FileInfo fileInfo) =>
            GetFileVersion(fileInfo?.FullName);

        /// <summary>
        ///     Returns the file version information associated with the specified file.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static Version GetFileVersion(string path)
        {
            try
            {
                var s = PathEx.Combine(path);
                if (!File.Exists(s))
                    throw new PathNotFoundException(s);
                var fvi = FileVersionInfo.GetVersionInfo(s);
                return fvi.FileVersion.ToVersion();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return new Version("0.0.0.0");
            }
        }

        /// <summary>
        ///     Returns the product version information associated with this file instance
        ///     member.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member to check.
        /// </param>
        public static Version GetProductVersion(this FileInfo fileInfo) =>
            GetProductVersion(fileInfo?.FullName);

        /// <summary>
        ///     Returns the product version information associated with the specified file.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static Version GetProductVersion(string path)
        {
            try
            {
                var s = PathEx.Combine(path);
                if (!File.Exists(s))
                    throw new PathNotFoundException(s);
                var fvi = FileVersionInfo.GetVersionInfo(s);
                return fvi.ProductVersion.ToVersion();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return new Version("0.0.0.0");
            }
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private enum DatHeaderKey
        {
            VersionNumber1,
            VersionNumber2,
            VersionNumber3,
            VersionNumber4,
            IdentifierChar1,
            IdentifierChar2,
            IdentifierChar3,
            IdentifierChar4,
            IdentifierChar5,
            CompressionMode,
            Reserved1,
            Reserved2,
            Reserved3,
            Reserved4,
            Reserved5,
            Reserved6
        }
    }
}
