#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PathEx.cs
// Version:  2016-10-21 05:11
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Provides static methods based on the <see cref="Path"/> class to perform operations on
    ///     <see cref="string"/> instances that contain file or directory path information.
    /// </summary>
    public static class PathEx
    {
        /// <summary>
        ///     Gets the full process executable path of the assembly based on
        ///     <see cref="Assembly.GetEntryAssembly()"/>.CodeBase.
        /// </summary>
        public static string LocalPath => Assembly.GetEntryAssembly().CodeBase.ToUri()?.LocalPath;

        /// <summary>
        ///     Gets the process executable located directory path of the assembly based on
        ///     <see cref="Assembly.GetEntryAssembly()"/>.CodeBase.
        /// </summary>
        public static string LocalDir => Path.GetDirectoryName(LocalPath)?.TrimEnd(Path.DirectorySeparatorChar);

        /// <summary>
        ///     Combines <see cref="Directory.Exists(string)"/> and <see cref="File.Exists(string)"/>
        ///     to determine whether the specified path element exists.
        /// </summary>
        /// <param name="path">
        ///     The file or directory to check.
        /// </param>
        public static bool DirOrFileExists(this string path) =>
            Directory.Exists(path) || File.Exists(path);

        /// <summary>
        ///     Combines <see cref="Directory.Exists(string)"/> and <see cref="File.Exists(string)"/>
        ///     to determine whether the specified path elements exists.
        /// </summary>
        /// <param name="paths">
        ///     An array of files and directories to check.
        /// </param>
        public static bool DirsOrFilesExists(params string[] paths)
        {
            var exists = false;
            foreach (var path in paths)
            {
                exists = path.DirOrFileExists();
                if (!exists)
                    break;
            }
            return exists;
        }

        /// <summary>
        ///     <para>
        ///         Combines an array of strings, based on <see cref="Path.Combine(string[])"/>,
        ///         <see cref="Path.GetFullPath(string)"/>,
        ///         <see cref="Environment.GetFolderPath(Environment.SpecialFolder)"/>,
        ///         <see cref="Environment.GetEnvironmentVariable(string)"/> and
        ///         <see cref="Regex.Match(string, string, RegexOptions)"/>, into a path.
        ///     </para>
        ///     <para>
        ///         <c>
        ///             Hint:
        ///         </c>
        ///         Allows superordinate directory navigation and environment variables
        ///         based on <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>,
        ///         for example, write <code>"%Desktop%"</code>, cases are ignored as well.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static string Combine(params string[] paths)
        {
            var path = string.Empty;
            try
            {
                if (paths.Length == 0 || paths.Count(string.IsNullOrWhiteSpace) == paths.Length)
                    throw new ArgumentNullException();
                var seperator = Path.DirectorySeparatorChar.ToString();
                if (!paths[0].EndsWith(seperator)) // fix for drive letter only paths
                    paths[0] += seperator;
                path = Path.Combine(paths);
                path = path.Trim().RemoveChar(Path.GetInvalidPathChars());
                if (path.StartsWith("%") && (path.Contains("%\\") || path.EndsWith("%")))
                {
                    var variable = Regex.Match(path, "%(.+?)%", RegexOptions.IgnoreCase).Groups[1].Value;
                    var value = EnvironmentEx.GetVariableValue(variable);
                    path = path.Replace($"%{variable}%", value);
                }
                while (path.Contains(seperator + seperator))
                    path = path.Replace(seperator + seperator, seperator);
                if (path.EndsWith(seperator))
                    path = path.Substring(0, path.Length - seperator.Length);
                path = Path.GetFullPath(path);
            }
            catch (ArgumentNullException) { }
            catch (ArgumentException) { }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return path;
        }

        /// <summary>
        ///     Returns a uniquely directory name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="label">
        ///     This text is at the beginning of the name.
        /// </param>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempDirName(string label = "tmp", int len = 4)
        {
            var s = label;
            try
            {
                var g = new string(Guid.NewGuid().ToString().Where(char.IsLetterOrDigit).ToArray());
                s = s.ToLower() + g.Substring(0, len.IsBetween(4, 24) ? len : 4).ToUpper();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return s;
        }

        /// <summary>
        ///     Returns a uniquely directory name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempDirName(int len) =>
            GetTempDirName("tmp", len);

        /// <summary>
        ///     Returns a uniquely file name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="label">
        ///     This text is at the beginning of the name.
        /// </param>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempFileName(string label = "tmp", int len = 4) =>
            GetTempDirName(label, len) + ".tmp";

        /// <summary>
        ///     Returns a uniquely file name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempFileName(int len) =>
            GetTempFileName("tmp", len);

        /// <summary>
        ///     Determines whether the specified file was compiled for a 64-bit platform environments
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static bool FileIs64Bit(string path)
        {
            ushort us = 0x0;
            try
            {
                using (var fs = new FileStream(Combine(path), FileMode.Open, FileAccess.Read))
                {
                    var br = new BinaryReader(fs);
                    fs.Seek(0x3c, SeekOrigin.Begin);
                    fs.Seek(br.ReadInt32(), SeekOrigin.Begin);
                    br.ReadUInt32();
                    us = br.ReadUInt16();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return us == 0x8664 || us == 0x200;
        }
    }
}
