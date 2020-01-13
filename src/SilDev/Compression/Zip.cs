#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Zip.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Compression
{
    using System;
    using System.IO;
    using System.IO.Compression;

    public static class Zip
    {
        /// <summary>
        ///     Creates a archive that contains the files and directories from the
        ///     specified directory.
        /// </summary>
        /// <param name="srcDir">
        ///     The path to the directory to be archived.
        /// </param>
        /// <param name="destPath">
        ///     The path of the archive to be created.
        /// </param>
        public static void CreateFromDir(string srcDir, string destPath)
        {
            try
            {
                var src = PathEx.Combine(srcDir);
                if (string.IsNullOrEmpty(src))
                    throw new ArgumentNullException(nameof(srcDir));
                if (!Directory.Exists(src))
                    throw new DirectoryNotFoundException();
                var dest = PathEx.Combine(destPath);
                if (string.IsNullOrEmpty(dest))
                    throw new ArgumentNullException(nameof(destPath));
                if (!PathEx.IsValidPath(dest))
                    throw new ArgumentInvalidException(nameof(destPath));
                if (File.Exists(dest))
                    File.Delete(dest);
                ZipFile.CreateFromDirectory(src, dest);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Extracts all the files in the specified zip archive to the specified
        ///     directory on the file system.
        /// </summary>
        /// <param name="srcPath">
        ///     The path of the zip archive to extract.
        /// </param>
        /// <param name="destDir">
        ///     The path to the directory to place the extracted files in.
        /// </param>
        /// <param name="delSrcPath">
        ///     <see langword="true"/> to delete the source archive after extracting;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static bool ExtractToDir(string srcPath, string destDir, bool delSrcPath = true)
        {
            try
            {
                var src = PathEx.Combine(srcPath);
                if (string.IsNullOrEmpty(src))
                    throw new ArgumentNullException(nameof(srcPath));
                if (!File.Exists(src))
                    throw new FileNotFoundException();
                var dest = PathEx.Combine(destDir);
                if (string.IsNullOrEmpty(dest))
                    throw new ArgumentNullException(nameof(destDir));
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
                                var entIsDir = entPath.EndsWithEx(PathEx.AltDirectorySeparatorStr, PathEx.DirectorySeparatorStr);
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
                            catch (Exception ex) when (ex.IsCaught())
                            {
                                Log.Write(ex);
                            }
                    }
                if (delSrcPath)
                    FileEx.TryDelete(src);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }
    }
}
