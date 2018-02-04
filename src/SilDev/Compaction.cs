#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Compaction.cs
// Version:  2018-02-04 04:20
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
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
            try
            {
                using (var msi = new MemoryStream(bytes))
                {
                    var mso = new MemoryStream();
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                        msi.CopyTo(gs);
                    bytes = mso.ToArray();
                }
                return bytes;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
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
            try
            {
                var ba = Encoding.UTF8.GetBytes(text);
                ba = ba.Zip();
                return ba;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Decompresses a compressed sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to decompress.
        /// </param>
        public static byte[] Unzip(this byte[] bytes)
        {
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
                using (var zip = ZipFile.OpenRead(src))
                    zip.ExtractToDirectory(dest);
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
            try
            {
                var ba = bytes.Unzip();
                var s = Encoding.UTF8.GetString(ba);
                return s;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        #region 7-Zip Helper

        /// <summary>
        ///     ***This is an undocumented class and can be changed or removed in the future
        ///     without futher notice.
        /// </summary>
        public static class Zip7Helper
        {
#pragma warning disable 1591
            public static string ExePath { get; set; } =
#if x64
                PathEx.Combine(PathEx.LocalDir, "Helper\\7z\\x64\\7zG.exe");
#else
                PathEx.Combine(PathEx.LocalDir, "Helper\\7z\\7zG.exe");
#endif

            public struct CompressTemplates
            {
                public const string Default = "-t7z -mx -mmt -ms";
                public const string Ultra = "-t7z -mx -m0=lzma -md=128m -mfb=256 -ms";
            }

            public static Process Zip(string srcDirOrFile, string destFile, string args = null, ProcessWindowStyle windowStyle = ProcessWindowStyle.Hidden, bool dispose = false)
            {
                args = args ?? CompressTemplates.Default;
                var prfx = PathEx.IsDir(srcDirOrFile) ? "\\*" : string.Empty;
                args = $"a {args} \"\"\"{destFile}\"\"\" \"\"\"{srcDirOrFile}{prfx}\"\"\"";
                return ProcessEx.Start(ExePath, args, false, windowStyle, dispose);
            }

            public static Process Zip(string srcDirOrFile, string destFile, ProcessWindowStyle windowStyle) =>
                Zip(srcDirOrFile, destFile, null, windowStyle);

            public static Process Unzip(string srcFile, string destDir, ProcessWindowStyle windowStyle = ProcessWindowStyle.Hidden, bool dispose = false)
            {
                var args = $"x \"\"\"{srcFile}\"\"\" -o\"\"\"{destDir}\"\"\" -y";
                return ProcessEx.Start(ExePath, args, false, windowStyle, dispose);
            }
#pragma warning restore 1591
        }

        #endregion
    }
}
