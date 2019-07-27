#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Compaction.cs
// Version:  2019-07-27 08:23
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///     Provides static methods for compressing and decompressing of data.
    /// </summary>
    public static class Compaction
    {
        /// <summary>
        ///     Compresses the specifed sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to compress.
        /// </param>
        public static byte[] Zip(this byte[] bytes)
        {
            if (bytes == null)
                return null;
            byte[] ba;
            using (var msi = new MemoryStream(bytes))
            {
                var mso = new MemoryStream();
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    msi.CopyTo(gs);
                ba = mso.ToArray();
            }
            return ba;
        }

        /// <summary>
        ///     Creates a archive that contains the files and directories from the specified
        ///     directory.
        /// </summary>
        /// <param name="srcDir">
        ///     The path to the directory to be archived.
        /// </param>
        /// <param name="destPath">
        ///     The path of the archive to be created.
        /// </param>
        public static void ZipDir(string srcDir, string destPath)
        {
            try
            {
                var src = PathEx.Combine(srcDir);
                if (string.IsNullOrEmpty(src))
                    throw new ArgumentNullException(nameof(src));
                if (!Directory.Exists(src))
                    throw new DirectoryNotFoundException();
                var dest = PathEx.Combine(destPath);
                if (string.IsNullOrEmpty(dest))
                    throw new ArgumentNullException(nameof(dest));
                if (!PathEx.IsValidPath(dest))
                    throw new ArgumentException();
                if (File.Exists(dest))
                    File.Delete(dest);
                ZipFile.CreateFromDirectory(src, dest);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Compresses the specifed <see cref="string"/> value.
        /// </summary>
        /// <param name="text">
        ///     The string to compress.
        /// </param>
        public static byte[] ZipText(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            var ba = TextEx.DefaultEncoding.GetBytes(text);
            return ba.Zip();
        }

        /// <summary>
        ///     Decompresses a compressed sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to decompress.
        /// </param>
        public static byte[] Unzip(this byte[] bytes)
        {
            if (bytes == null)
                return null;
            try
            {
                byte[] ba;
                using (var mso = new MemoryStream())
                {
                    var msi = new MemoryStream(bytes);
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                        gs.CopyTo(mso);
                    ba = mso.ToArray();
                }
                return ba;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Extracts all the files in the specified zip archive to the specified directory on
        ///     the file system.
        /// </summary>
        /// <param name="srcPath">
        ///     The path of the zip archive to extract.
        /// </param>
        /// <param name="destDir">
        ///     The path to the directory to place the extracted files in.
        /// </param>
        /// <param name="delSrcPath">
        ///     true to delete the source archive after extracting; otherwise, false.
        /// </param>
        public static bool Unzip(string srcPath, string destDir, bool delSrcPath = true)
        {
            try
            {
                var src = PathEx.Combine(srcPath);
                if (string.IsNullOrEmpty(src))
                    throw new ArgumentNullException(nameof(src));
                if (!File.Exists(src))
                    throw new FileNotFoundException();
                var dest = PathEx.Combine(destDir);
                if (string.IsNullOrEmpty(dest))
                    throw new ArgumentNullException(nameof(dest));
                using (var archive = ZipFile.OpenRead(src))
                    try
                    {
                        archive.ExtractToDirectory(dest);
                    }
                    catch
                    {
                        foreach (var ent in archive.Entries)
                            try
                            {
                                var entPath = ent.FullName;
                                var entIsDir = entPath.EndsWithEx(Path.AltDirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString());
                                entPath = PathEx.Combine(dest, entPath);
                                if (!PathEx.IsValidPath(entPath))
                                    throw new NotSupportedException();
                                if (entIsDir && !Directory.Exists(entPath))
                                {
                                    Directory.CreateDirectory(entPath);
                                    continue;
                                }
                                if (ent.Length == 0)
                                    continue;
                                FileEx.Delete(entPath);
                                var entDir = Path.GetDirectoryName(entPath);
                                if (string.IsNullOrEmpty(entDir))
                                    continue;
                                if (!Directory.Exists(entDir))
                                {
                                    FileEx.Delete(entDir);
                                    Directory.CreateDirectory(entDir);
                                }
                                ent.ExtractToFile(entPath, true);
                            }
                            catch (Exception ex)
                            {
                                Log.Write(ex);
                            }
                    }
                if (delSrcPath)
                    File.Delete(src);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Decompresses a compressed sequence of bytes back to a <see cref="string"/>
        ///     value.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to decompress.
        /// </param>
        public static string UnzipText(this byte[] bytes)
        {
            var ba = bytes?.Unzip();
            return ba == null ? null : Encoding.UTF8.GetString(ba);
        }

        /// <summary>
        ///     Provides functionality for compressing and decompressing files with 7-Zip.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static class SevenZipHelper
        {
            /// <summary>
            ///     Specifies how to compress.
            /// </summary>
            public enum CompressMode
            {
                /// <summary>
                ///     -t7z -mx -mmt -ms
                /// </summary>
                Default,

                /// <summary>
                ///     -t7z -mx -m0=lzma -md=128m -mfb=256 -ms
                /// </summary>
                Ultra
            }

            private static string _location, _filePath;

            /// <summary>
            ///     The location of the 7-Zip executable file.
            /// </summary>
            public static string Location
            {
                get
                {
                    if (_location != default(string))
                        return _location;
                    var dirs = new[]
                    {
#if any || x64
                        "Helper\\7z\\x64",
                        "Binaries\\Helper\\7z\\x64",
#endif
                        "Helper\\7z",
                        "Binaries\\Helper\\7z"
                    };
#if any
                    if (!Environment.Is64BitOperatingSystem)
                        dirs = dirs.Skip(2).ToArray();
#endif
                    foreach (var dir in dirs)
                    {
                        if (SetPathsIfValid(PathEx.Combine(PathEx.LocalDir, dir)))
                            break;
                        _location = string.Empty;
                    }
                    return _location;
                }
                set
                {
                    if (SetPathsIfValid(value?.RemoveText("7zG.exe", "7z.exe")))
                        return;
                    _location = string.Empty;
                    _filePath = default(string);
                }
            }

            /// <summary>
            ///     The file path of the 7-Zip executable file.
            /// </summary>
            public static string FilePath
            {
                get
                {
                    if (_filePath != default(string) && Directory.Exists(Location))
                        return _filePath;
                    return default(string);
                }
            }

            private static bool SetPathsIfValid(string dir)
            {
                if (dir == default(string))
                    return false;
                var fileDir = dir;
                if (!Directory.Exists(fileDir))
                    return false;
                var dllPath = Path.Combine(fileDir, "7z.dll");
                if (!File.Exists(dllPath))
                    return false;
                var exePath = Path.Combine(fileDir, "7zG.exe");
                if (!File.Exists(exePath))
                    exePath = Path.Combine(fileDir, "7z.exe");
                if (!File.Exists(exePath))
                    return false;
                _location = fileDir;
                _filePath = exePath;
                return true;
            }

            /// <summary>
            ///     Compress the specified file, or all files of the specified directory, to the
            ///     specified file on the file system.
            /// </summary>
            /// <param name="srcDirOrFile">
            ///     The path of the file or directory to compress.
            /// </param>
            /// <param name="destFile">
            ///     The path of the archive.
            /// </param>
            /// <param name="compressMode">
            ///     One of the enumeration values that indicates how to compress the file/s.
            /// </param>
            /// <param name="windowStyle">
            ///     The window state to use when the 7-Zip process is started.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Component"/> if the process
            ///     has been started; otherwise, false.
            /// </param>
            public static Process Zip(string srcDirOrFile, string destFile, CompressMode compressMode = CompressMode.Default, ProcessWindowStyle windowStyle = ProcessWindowStyle.Minimized, bool dispose = false)
            {
                if (!File.Exists(FilePath))
                    return null;
                string args;
                switch (compressMode)
                {
                    case CompressMode.Ultra:
                        args = "-t7z -mx -m0=lzma -md=128m -mfb=256 -ms";
                        break;
                    default:
                        args = "-t7z -mx -mmt -ms";
                        break;
                }
                var src = PathEx.Combine(srcDirOrFile);
                var dest = PathEx.Combine(destFile);
                var prfx = PathEx.IsDir(src) ? "\\*" : string.Empty;
                args = $"a {args} \"\"\"{dest}\"\"\" \"\"\"{src}{prfx}\"\"\"";
                return ProcessEx.Start(_filePath, args, false, windowStyle, dispose);
            }

            /// <summary>
            ///     Compress the specified file, or all files of the specified directory, to the
            ///     specified file on the file system.
            /// </summary>
            /// <param name="srcDirOrFile">
            ///     The path of the file or directory to compress.
            /// </param>
            /// <param name="destFile">
            ///     The path of the archive.
            /// </param>
            /// <param name="windowStyle">
            ///     The window state to use when the 7-Zip process is started.
            /// </param>
            public static Process Zip(string srcDirOrFile, string destFile, ProcessWindowStyle windowStyle) =>
                Zip(srcDirOrFile, destFile, CompressMode.Default, windowStyle);

            /// <summary>
            ///     Extracts all the files in the specified archive to the specified directory on the
            ///     file system.
            /// </summary>
            /// <param name="srcFile">
            ///     The path of the archive to extract.
            /// </param>
            /// <param name="destDir">
            ///     The path to the directory to place the extracted files in.
            /// </param>
            /// <param name="windowStyle">
            ///     The window state to use when the 7-Zip process is started.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Component"/> if the process
            ///     has been started; otherwise, false.
            /// </param>
            public static Process Unzip(string srcFile, string destDir, ProcessWindowStyle windowStyle = ProcessWindowStyle.Minimized, bool dispose = false)
            {
                if (!File.Exists(FilePath))
                    return null;
                var args = $"x \"\"\"{srcFile}\"\"\" -o\"\"\"{destDir}\"\"\" -y";
                return ProcessEx.Start(_filePath, args, false, windowStyle, dispose);
            }
        }
    }
}
