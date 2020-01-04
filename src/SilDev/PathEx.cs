#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PathEx.cs
// Version:  2020-01-04 13:40
// 
// Copyright (c) 2020, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using Properties;

    /// <summary>
    ///     Provides static methods based on the <see cref="Path"/> class to perform operations on
    ///     <see cref="string"/> instances that contain file or directory path information.
    /// </summary>
    public static class PathEx
    {
        private static readonly char[] InvalidPathChars =
        {
            '\u0000', '\u0001', '\u0002', '\u0003',
            '\u0004', '\u0005', '\u0006', '\u0007',
            '\u0008', '\u0009', '\u000a', '\u000b',
            '\u000c', '\u000d', '\u000e', '\u000f',
            '\u0010', '\u0011', '\u0012', '\u0013',
            '\u0014', '\u0015', '\u0016', '\u0017',
            '\u0018', '\u0019', '\u001a', '\u001b',
            '\u001c', '\u001d', '\u001e', '\u001f',
            '\u0022', '\u002a', '\u003c', '\u003e',
            '\u003f', '\u007c'
        };

        private static readonly string[] PathPrefixStrs =
        {
            "\\??\\UNC\\",
            "\\??\\",
            "\\\\?\\",
            "\\\\.\\",
            "\\\\"
        };

        private static readonly char[] PathSeparatorChars =
        {
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar
        };

        /// <summary>
        ///     Provides a platform-specific volume separator character string.
        ///     <para>
        ///         The <see cref="string"/> representation of
        ///         <see cref="Path.VolumeSeparatorChar"/>.
        ///     </para>
        /// </summary>
        public static string VolumeSeparatorStr { get; } = Path.VolumeSeparatorChar.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///     A platform-specific separator character string used to separate path
        ///     strings in environment variables.
        ///     <para>
        ///         The <see cref="string"/> representation of
        ///         <see cref="Path.DirectorySeparatorChar"/>.
        ///     </para>
        /// </summary>
        public static string DirectorySeparatorStr { get; } = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///     Provides a platform-specific alternate character string used to separate
        ///     directory levels in a path string that reflects a hierarchical file system
        ///     organization.
        ///     <para>
        ///         The <see cref="string"/> representation of
        ///         <see cref="Path.AltDirectorySeparatorChar"/>.
        ///     </para>
        /// </summary>
        public static string AltDirectorySeparatorStr { get; } = Path.AltDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///     Gets the full process executable path of the assembly based on
        ///     <see cref="Assembly.GetEntryAssembly()"/>.CodeBase.
        /// </summary>
        public static string LocalPath { get; } = Assembly.GetEntryAssembly()?.CodeBase.ToUri()?.LocalPath;

        /// <summary>
        ///     Gets the process executable located directory path of the assembly based on
        ///     <see cref="Assembly.GetEntryAssembly()"/>.CodeBase.
        /// </summary>
        public static string LocalDir { get; } = Path.GetDirectoryName(LocalPath)?.TrimEnd(Path.DirectorySeparatorChar);

        /// <summary>
        ///     Combines <see cref="Directory.Exists(string)"/> and <see cref="File.Exists(string)"/>
        ///     to determine whether the specified path element exists.
        /// </summary>
        /// <param name="path">
        ///     The file or directory to check.
        /// </param>
        public static bool DirOrFileExists(string path) =>
            DirectoryEx.Exists(path) || FileEx.Exists(path);

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
                exists = DirOrFileExists(path);
                if (!exists)
                    break;
            }
            return exists;
        }

        /// <summary>
        ///     Determines whether the specified path is specified as reparse
        ///     point.
        /// </summary>
        /// <param name="path">
        ///     The file or directory to check.
        /// </param>
        public static bool DirOrFileIsLink(string path) =>
            IsDir(path) ? DirectoryEx.IsLink(path) : FileEx.IsLink(path);

        /// <summary>
        ///     Determines whether the specified path has a valid format.
        /// </summary>
        /// <param name="path">
        ///     The specified path to check.
        /// </param>
        public static bool IsValidPath(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                if (path.Length < 3)
                    throw new ArgumentException(ExceptionMessages.PathLengthIsTooLow + path);
                if (!path.Contains(Path.DirectorySeparatorChar))
                {
                    if (!path.Contains(Path.AltDirectorySeparatorChar))
                        throw new ArgumentException(ExceptionMessages.PathHasNoSeparators + path);
                    throw new ArgumentException(ExceptionMessages.PathHasInvalidSeparators + path);
                }
                foreach (var prefix in PathPrefixStrs)
                {
                    if (!path.StartsWith(prefix, StringComparison.Ordinal))
                        continue;
                    throw new NotSupportedException(string.Format(CultureConfig.GlobalCultureInfo, ExceptionMessages.PathHasInvalidPrefix, prefix, path));
                }
                if (path.Contains(new string(Path.DirectorySeparatorChar, 2)))
                    throw new ArgumentException(ExceptionMessages.ConsecutiveSeparatorsInPath + path);
                var drive = path.Substring(0, 3);
                if (!Regex.IsMatch(drive, @"^[a-zA-Z]:\\$"))
                    throw new DriveNotFoundException(ExceptionMessages.NoDriveInPath + path);
                if (path.Length >= 260)
                    throw new PathTooLongException(ExceptionMessages.PathLengthIsTooLong + path);
                var levels = path.Split(Path.DirectorySeparatorChar);
                if (levels.Any(s => s.Length >= 255))
                    throw new PathTooLongException(ExceptionMessages.PathSegmentLengthIsTooLong + path);
                var dirLength = Path.HasExtension(path) ? levels.Take(levels.Length - 1).Join(Path.DirectorySeparatorChar).Length : path.Length;
                if (dirLength >= 248)
                    throw new PathTooLongException(ExceptionMessages.DirLengthIsTooLong + path);
                if (!DriveInfo.GetDrives().Select(di => di.Name).Contains(drive))
                    throw new DriveNotFoundException(ExceptionMessages.InvalidDriveInPath + path);
                var subPath = path.Substring(3);
                if (subPath.Any(c => c != Path.DirectorySeparatorChar && Path.GetInvalidFileNameChars().Contains(c)))
                    throw new ArgumentException(ExceptionMessages.BadCharsInPath);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified path is specified as directory.
        /// </summary>
        /// <param name="path">
        ///     The path to check.
        /// </param>
        public static bool IsDir(string path) =>
            DirectoryEx.Exists(path) && FileEx.MatchAttributes(path, FileAttributes.Directory);

        /// <summary>
        ///     Determines whether the specified path is specified as file.
        /// </summary>
        /// <param name="path">
        ///     The path to check.
        /// </param>
        public static bool IsFile(string path) =>
            FileEx.Exists(path) && !FileEx.MatchAttributes(path, FileAttributes.Directory);

        /// <summary>
        ///     Sets the specified attributes for the specified path.
        /// </summary>
        /// <param name="path">
        ///     The file or directory to change.
        /// </param>
        /// <param name="attr">
        ///     The attributes to set.
        /// </param>
        public static void SetAttributes(string path, FileAttributes attr)
        {
            var src = Combine(path);
            if (IsDir(src))
                DirectoryEx.SetAttributes(src, attr);
            else
                FileEx.SetAttributes(src, attr);
        }

        /// <summary>
        ///     Combines a <see cref="Environment.SpecialFolder"/> constant with an array of
        ///     strings into a valid path.
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="specialFolder">
        ///     A specified enumerated constant used to retrieve directory paths to system
        ///     special folders.
        /// </param>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static string Combine(char[] invalidPathChars, Environment.SpecialFolder? specialFolder, params string[] paths)
        {
            var path = string.Empty;
            try
            {
                if (specialFolder != null)
                    path = Environment.GetFolderPath((Environment.SpecialFolder)specialFolder);
                if (!(paths?.Join(Path.DirectorySeparatorChar).Split(PathSeparatorChars, StringSplitOptions.RemoveEmptyEntries) is IEnumerable<string> plains))
                    throw new ArgumentNullException(nameof(paths));
                if (invalidPathChars?.Any() ?? false)
                    plains = plains.Select(x => x.RemoveChar(invalidPathChars));
                path = !string.IsNullOrEmpty(path) ? Path.Combine(path, plains.Join(Path.DirectorySeparatorChar)) : plains.Join(Path.DirectorySeparatorChar);

                var key = default(string);
                var num = default(byte);
                if (path.StartsWith("%", StringComparison.Ordinal) && (path.Contains($"%{Path.DirectorySeparatorChar}") || path.EndsWith("%", StringComparison.Ordinal)))
                {
                    var regex = Regex.Match(path, "%(.+?)%", RegexOptions.IgnoreCase);
                    if (regex.Groups.Count > 1)
                    {
                        var variable1 = regex.Groups[1].Value;
                        var variable2 = variable1;
                        EnvironmentEx.VariableFilter(ref variable2, out key, out num);
                        if (!string.IsNullOrEmpty(variable2))
                        {
                            var value = EnvironmentEx.GetVariableValue(variable2);
                            if (!string.IsNullOrEmpty(value))
                                path = path.Replace($"%{variable1}%", value);
                        }
                    }
                }
                else if (path.StartsWith($".{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
                    path = Path.Combine(LocalDir, path.Substring(2));
                else if (path.Equals(".", StringComparison.Ordinal))
                    path = LocalDir;

                if (path.Contains($"{Path.DirectorySeparatorChar}.."))
                    path = Path.GetFullPath(path);
                if (path.EndsWith(".", StringComparison.Ordinal))
                    path = path.TrimEnd('.');

                if (!string.IsNullOrEmpty(key) || num > 1)
                    if (string.IsNullOrEmpty(key))
                        path = path.Replace(DirectorySeparatorStr, new string(Path.DirectorySeparatorChar, num));
                    else if (key.EqualsEx("Alt"))
                        path = path.Replace(DirectorySeparatorStr, new string(Path.AltDirectorySeparatorChar, num));
            }
            catch (ArgumentException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return path;
        }

        /// <summary>
        ///     Combines an array of strings into a valid path.
        ///     <para>
        ///         Hint: Allows superordinate directory navigation and environment variables
        ///         based on <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static string Combine(char[] invalidPathChars, params string[] paths) =>
            Combine(invalidPathChars, null, paths);

        /// <summary>
        ///     Filters a string into a valid path.
        ///     <para>
        ///         Hint: Allows superordinate directory navigation and environment variables
        ///         based on <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="path">
        ///     The string to be filtered.
        /// </param>
        public static string Combine(char[] invalidPathChars, string path) =>
            Combine(invalidPathChars, new[] { path });

        /// <summary>
        ///     Combines a <see cref="Environment.SpecialFolder"/> constant with an array of
        ///     strings into a valid path.
        /// </summary>
        /// <param name="specialFolder">
        ///     A specified enumerated constant used to retrieve directory paths to system
        ///     special folders.
        /// </param>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static string Combine(Environment.SpecialFolder? specialFolder, params string[] paths) =>
            Combine(InvalidPathChars, specialFolder, paths);

        /// <summary>
        ///     Combines an array of strings into a valid path.
        ///     <para>
        ///         Hint: Allows superordinate directory navigation and environment variables
        ///         based on <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static string Combine(params string[] paths) =>
            Combine(InvalidPathChars, paths);

        /// <summary>
        ///     Filters a string into a valid path.
        ///     <para>
        ///         Hint: Allows superordinate directory navigation and environment variables
        ///         based on <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="path">
        ///     The string to be filtered.
        /// </param>
        public static string Combine(string path) =>
            Combine(InvalidPathChars, new[] { path });

        /// <summary>
        ///     Combines an array of strings, based on <see cref="Combine(string[])"/>, into a path.
        ///     <para>
        ///         Hint: <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path levels.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static string AltCombine(char[] invalidPathChars, params string[] paths)
        {
            var path = Combine(invalidPathChars, paths);
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var schemes = new[]
                {
                    "file:",
                    "ftp:",
                    "http:",
                    "https:"
                };
                for (var i = 0; i < schemes.Length; i++)
                {
                    var scheme = $"{schemes[i]}{Path.AltDirectorySeparatorChar}";
                    if (!path.StartsWithEx(scheme))
                        continue;
                    path = path.Replace(scheme, $"{scheme}{new string(Path.AltDirectorySeparatorChar, i < 1 ? 2 : 1)}");
                    break;
                }
            }
            catch (ArgumentException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return path;
        }

        /// <summary>
        ///     Filters a string, based on <see cref="Combine(string[])"/>, into a valid path.
        ///     <para>
        ///         Hint: <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path levels.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="path">
        ///     The string to be filtered.
        /// </param>
        public static string AltCombine(char[] invalidPathChars, string path) =>
            AltCombine(invalidPathChars, new[] { path });

        /// <summary>
        ///     Combines an array of strings, based on <see cref="Combine(string[])"/>, into a path.
        ///     <para>
        ///         Hint: <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path levels.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static string AltCombine(params string[] paths) =>
            AltCombine(InvalidPathChars, paths);

        /// <summary>
        ///     Filters a string, based on <see cref="Combine(string[])"/>, into a valid path.
        ///     <para>
        ///         Hint: <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path levels.
        ///     </para>
        /// </summary>
        /// <param name="path">
        ///     The string to be filtered.
        /// </param>
        public static string AltCombine(string path) =>
            AltCombine(InvalidPathChars, new[] { path });

        /// <summary>
        ///     Returns the directory information for the specified path string.
        /// </summary>
        /// <param name="path">
        ///     The path of a file or directory.
        /// </param>
        /// <param name="convertEnvVars">
        ///     true to convert environment variables; otherwise, false.
        /// </param>
        public static string GetDirectoryName(string path, bool convertEnvVars = false)
        {
            if (path == null)
                return null;
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;
            var str = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var index = str.LastIndexOf(Path.DirectorySeparatorChar);
            var alt = false;
            if (index < 0)
            {
                alt = true;
                index = str.LastIndexOf(Path.AltDirectorySeparatorChar);
            }
            if (index > 2 && index < str.Length)
            {
                str = str.Substring(0, index);
                if (convertEnvVars)
                    str = alt ? AltCombine(str) : Combine(str);
            }
            else
            {
                str = alt ? AltCombine(path) : Combine(path);
                str = Path.GetDirectoryName(str);
            }
            return !str.EqualsEx(path) ? str : null;
        }

        /// <summary>
        ///     Returns a unique name starting with a given prefix, followed by a hash of the specified
        ///     length and a specified suffix.
        /// </summary>
        /// <param name="prefix">
        ///     This text is at the beginning of the name.
        ///     <para>
        ///         Uppercase letters are converted to lowercase letters. Supported characters are only
        ///         from '0' to '9' and from 'a' to 'z' but can be completely empty to omit the prefix.
        ///     </para>
        /// </param>
        /// <param name="suffix">
        ///     This text is at the end of the name.
        ///     <para>
        ///         If it does not begin with a dot, it will be added. Uppercase letters are converted
        ///         to lowercase letters. Supported characters are only from '0' to '9' and from 'a' to
        ///         'z' but can be completely empty to omit the suffix.
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
        public static string GetUniqueName(string prefix = "tmp", string suffix = default, int hashLen = 4)
        {
            if (!hashLen.IsBetween(4, 24))
                throw new ArgumentOutOfRangeException(nameof(hashLen));
            var sa = new[]
            {
                prefix?.ToLowerInvariant() ?? string.Empty,
                suffix?.ToLowerInvariant() ?? string.Empty
            };
            for (var i = 0; i < sa.Length; i++)
            {
                var s = sa[i];
                if (string.IsNullOrEmpty(s))
                    continue;
                if (i > 0 && s.StartsWith(".", StringComparison.Ordinal))
                    s = s.Substring(1);
                if (!s.All(c => c.IsBetween('0', '9') || c.IsBetween('a', 'z')))
                    throw new ArgumentInvalidException(i == 0 ? nameof(prefix) : nameof(suffix));
            }
            var ran = new Random();
            var sb = new StringBuilder(hashLen);
            for (var i = 0; i < hashLen; i++)
            {
                if (ran.Next(sbyte.MinValue, sbyte.MaxValue) < 0)
                {
                    sb.Append((char)ran.Next('0', '9'));
                    continue;
                }
                sb.Append((char)ran.Next('A', 'F'));
            }
            return $"{sa.First()}{sb.ToStringThenClear()}{sa.Last()}";
        }

        /// <summary>
        ///     Returns a fully qualified path with unique name starting with a given prefix, followed by
        ///     a hash of the specified length and a specified suffix.
        /// </summary>
        /// <param name="baseDir">
        ///     This home directory for the uniquely named file or directory.
        /// </param>
        /// <param name="namePrefix">
        ///     This text is at the beginning of the name.
        ///     <para>
        ///         Uppercase letters are converted to lowercase letters. Supported characters are only
        ///         from '0' to '9' and from 'a' to 'z' but can be completely empty to omit the prefix.
        ///     </para>
        /// </param>
        /// <param name="nameSuffix">
        ///     This text is at the end of the name.
        ///     <para>
        ///         If it does not begin with a dot, it will be added. Uppercase letters are converted
        ///         to lowercase letters. Supported characters are only from '0' to '9' and from 'a' to
        ///         'z' but can be completely empty to omit the suffix.
        ///     </para>
        /// </param>
        /// <param name="hashLen">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     baseDir is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     hashLen is not between 4 and 24.
        /// </exception>
        /// <exception cref="ArgumentInvalidException">
        ///     baseDir is not a valid path.
        /// </exception>
        /// <exception cref="ArgumentInvalidException">
        ///     namePrefix or nameSuffix contains invalid characters.
        /// </exception>
        public static string GetUniquePath(string baseDir = "%TEMP%", string namePrefix = "tmp", string nameSuffix = default, int hashLen = 4)
        {
            if (baseDir == null)
                throw new ArgumentNullException(nameof(baseDir));
            var dir = baseDir;
            if (dir.StartsWith("%", StringComparison.Ordinal) && (dir.Contains($"%{Path.DirectorySeparatorChar}") || dir.EndsWith("%", StringComparison.Ordinal)))
                dir = Combine(dir);
            if (!IsValidPath(dir))
                throw new ArgumentInvalidException(nameof(baseDir));
            return Path.Combine(dir, GetUniqueName(namePrefix, nameSuffix, hashLen));
        }

        /// <summary>
        ///     Returns processes that have locked the specified paths.
        /// </summary>
        /// <param name="paths">
        ///     An sequence of strings that contains file and/or directory paths to check.
        /// </param>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static IEnumerable<Process> GetLocks(IEnumerable<string> paths)
        {
            if (paths == null)
                return null;
            var files = new List<string>();
            foreach (var path in paths.Select(Combine))
            {
                if (IsDir(path))
                {
                    var innerFiles = DirectoryEx.GetFiles(path, SearchOption.AllDirectories);
                    if (innerFiles?.Any() ?? false)
                        files.AddRange(innerFiles);
                    continue;
                }
                files.Add(path);
            }
            return files.Any() ? FileEx.GetLocks(files.ToArray()) : null;
        }

        /// <summary>
        ///     Returns processes that have locked the specified path.
        /// </summary>
        /// <param name="path">
        ///     The path of a file or directory to check.
        /// </param>
        public static IEnumerable<Process> GetLocks(string path)
        {
            var locks = default(IEnumerable<Process>);
            try
            {
                var s = Combine(path);
                if (!DirOrFileExists(s))
                    throw new PathNotFoundException(s);
                if (IsDir(s))
                {
                    var di = new DirectoryInfo(s);
                    locks = di.GetLocks();
                }
                else
                {
                    var fi = new FileInfo(s);
                    locks = fi.GetLocks();
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return locks;
        }

        /// <summary>
        ///     Deletes any file or directory.
        ///     <para>
        ///         Immediately stops all specified processes that are locking this file or directory.
        ///     </para>
        /// </summary>
        /// <param name="path">
        ///     The path of the file or directory to be deleted.
        /// </param>
        /// <param name="elevated">
        ///     true to run this task with administrator privileges if the deletion fails; otherwise,
        ///     false.
        /// </param>
        /// <param name="timelimit">
        ///     The time limit in milliseconds.
        /// </param>
        public static bool ForceDelete(string path, bool elevated = false, int timelimit = 60000)
        {
            var target = Combine(path);
            try
            {
                if (!DirOrFileExists(target))
                    throw new PathNotFoundException(target);

                var locked = false;
                using (var current = Process.GetCurrentProcess())
                {
                    foreach (var p in GetLocks(target).Where(x => x != current))
                    {
                        if (ProcessEx.Terminate(p) || locked)
                            continue;
                        locked = true;
                    }
                    if (!locked)
                        foreach (var p in GetLocks(target).Where(x => x != current))
                        {
                            if (!locked)
                                locked = true;
                            p?.Dispose();
                        }
                }

                var sb = new StringBuilder();
                var curName = $"{ProcessEx.CurrentName}.exe";
                if (IsDir(target))
                {
                    var tmpDir = DirectoryEx.GetUniqueTempPath();
                    if (!Directory.Exists(tmpDir))
                        Directory.CreateDirectory(tmpDir);

                    var helper = FileEx.GetUniqueTempPath("tmp", ".cmd");
                    sb.AppendLine("@ECHO OFF");
                    sb.AppendFormatLine("ROBOCOPY \"{0}\" \"{1}\" /MIR", tmpDir, target);
                    sb.AppendFormatLine("RMDIR /S /Q \"{0}\"", tmpDir);
                    sb.AppendFormatLine("RMDIR /S /Q \"{0}\"", target);
                    sb.AppendLine("EXIT");
                    File.WriteAllText(helper, sb.ToStringThenClear());

                    var call = $"CALL \"{helper}\"";
                    if (locked)
                        ProcessEx.SendHelper.WaitForExitThenCmd(call, curName, elevated);
                    else
                        using (var p = ProcessEx.Send(call, elevated, false))
                            if (p?.HasExited ?? false)
                                p.WaitForExit(timelimit);

                    DirectoryEx.Delete(tmpDir);
                    DirectoryEx.Delete(target);
                    ProcessEx.SendHelper.WaitThenDelete(helper, elevated);
                }
                else
                    try
                    {
                        File.Delete(target);
                    }
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        if (locked)
                            ProcessEx.SendHelper.WaitForExitThenDelete(target, curName, true);
                        else
                            using (var p = ProcessEx.Send(sb.ToStringThenClear(), elevated, false))
                                if (p?.HasExited ?? false)
                                    p.WaitForExit(timelimit);
                    }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return !DirOrFileExists(target);
        }
    }
}
