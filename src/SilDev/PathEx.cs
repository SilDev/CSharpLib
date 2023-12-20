#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PathEx.cs
// Version:  2023-12-20 00:28
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using Network;
    using Properties;

    /// <summary>
    ///     Provides static methods based on the <see cref="Path"/> class to perform
    ///     operations on <see cref="string"/> instances that contain file or directory
    ///     path information.
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
        public static string VolumeSeparatorStr { get; } = Path.VolumeSeparatorChar.ToStringDefault();

        /// <summary>
        ///     A platform-specific separator character string used to separate path
        ///     strings in environment variables.
        ///     <para>
        ///         The <see cref="string"/> representation of
        ///         <see cref="Path.DirectorySeparatorChar"/>.
        ///     </para>
        /// </summary>
        public static string DirectorySeparatorStr { get; } = Path.DirectorySeparatorChar.ToStringDefault();

        /// <summary>
        ///     Provides a platform-specific alternate character string used to separate
        ///     directory levels in a path string that reflects a hierarchical file system
        ///     organization.
        ///     <para>
        ///         The <see cref="string"/> representation of
        ///         <see cref="Path.AltDirectorySeparatorChar"/>.
        ///     </para>
        /// </summary>
        public static string AltDirectorySeparatorStr { get; } = Path.AltDirectorySeparatorChar.ToStringDefault();

        /// <summary>
        ///     Gets the executable file path of the current process based on
        ///     <see cref="Assembly.GetEntryAssembly()"/>.CodeBase.
        /// </summary>
        public static string LocalPath { get; } = FindAssemblyPath(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets the executable located directory path of the current process based on
        ///     <see cref="Assembly.GetEntryAssembly()"/>.CodeBase.
        /// </summary>
        public static string LocalDir { get; } = FindAssemblyDir(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets the file path of the current loaded Si13n7 Dev.™ CSharp Library.
        /// </summary>
        public static string LibraryPath { get; } = FindAssemblyPath(Assembly.GetAssembly(typeof(PathEx)));

        /// <summary>
        ///     Gets the executable located directory path of the current loaded Si13n7
        ///     Dev.™ CSharp Library.
        /// </summary>
        public static string LibraryDir { get; } = FindAssemblyDir(Assembly.GetAssembly(typeof(PathEx)));

        /// <summary>
        ///     Returns the file path of the specified assembly, if available.
        /// </summary>
        /// <param name="element">
        ///     The assembly element to find the path.
        /// </param>
        public static string FindAssemblyPath(Assembly element) =>
            element?.CodeBase.ToUri()?.LocalPath;

        /// <summary>
        ///     Returns the directory path of the specified assembly, if available.
        /// </summary>
        /// <param name="element">
        ///     The assembly element to find the path.
        /// </param>
        public static string FindAssemblyDir(Assembly element)
        {
            var path = FindAssemblyPath(element);
            return string.IsNullOrEmpty(path) ? null : Path.GetDirectoryName(path)?.TrimEnd(Path.DirectorySeparatorChar);
        }

        /// <summary>
        ///     Returns the file path of the specified type, if available.
        /// </summary>
        /// <param name="type">
        ///     The type to find the path.
        /// </param>
        public static string FindTypePath(Type type)
        {
            var assembly = Assembly.GetAssembly(type);
            return FindAssemblyPath(assembly);
        }

        /// <summary>
        ///     Returns the directory path of the specified type, if available.
        /// </summary>
        /// <param name="type">
        ///     The type to find the path.
        /// </param>
        public static string FindTypeDir(Type type)
        {
            var path = FindTypePath(type);
            return string.IsNullOrEmpty(path) ? null : Path.GetDirectoryName(path)?.TrimEnd(Path.DirectorySeparatorChar);
        }

        /// <summary>
        ///     Combines <see cref="Directory.Exists(string)"/> and
        ///     <see cref="File.Exists(string)"/> to determine whether the specified path
        ///     element exists.
        /// </summary>
        /// <param name="path">
        ///     The file or directory to check.
        /// </param>
        public static bool DirOrFileExists(string path) =>
            DirectoryEx.Exists(path) || FileEx.Exists(path);

        /// <summary>
        ///     Combines <see cref="Directory.Exists(string)"/> and
        ///     <see cref="File.Exists(string)"/> to determine whether the specified path
        ///     elements exists.
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
        ///     Determines whether the specified path is specified as reparse point.
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
                    throw new NotSupportedException(ExceptionMessages.PathHasInvalidPrefix.FormatCurrent(prefix, path));
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
        ///     Filters the specified string into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="path0">
        ///     The path to be filtered.
        /// </param>
        public static string Combine(char[] invalidPathChars, string path0)
        {
            var path = string.Empty;
            try
            {
                var prefix = PathPrefixStrs.FirstOrDefault(prefix => path0.StartsWith(prefix, StringComparison.Ordinal));

                if (path0?.Split(PathSeparatorChars, StringSplitOptions.RemoveEmptyEntries) is not IEnumerable<string> plains)
                    throw new ArgumentNullException(nameof(path0));
                if (invalidPathChars?.Length is > 0)
                    plains = plains.Select(x => x.RemoveChar(invalidPathChars));
                path = !string.IsNullOrEmpty(path) ? Path.Combine(path, plains.Join(Path.DirectorySeparatorChar)) : plains.Join(Path.DirectorySeparatorChar);

                var key = default(string);
                var num = default(byte);
                if (path.StartsWith("%", StringComparison.Ordinal) && (path.Contains($"%{Path.DirectorySeparatorChar}") || path.EndsWith("%", StringComparison.Ordinal)))
                {
                    var length = path.IndexOf('%', 1);
                    var variable1 = path.Substring(1, --length);
                    if (!string.IsNullOrEmpty(variable1))
                    {
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

                if (path.StartsWith($".{Path.DirectorySeparatorChar}", StringComparison.Ordinal) || path.Equals(".", StringComparison.Ordinal) || path.Contains($"{Path.DirectorySeparatorChar}.."))
                    path = Path.GetFullPath(path);
                if (path.EndsWith(".", StringComparison.Ordinal))
                    path = path.TrimEnd('.');

                if (!string.IsNullOrEmpty(key) || num > 1)
                    if (string.IsNullOrEmpty(key))
                        path = path.Replace(DirectorySeparatorStr, new string(Path.DirectorySeparatorChar, num));
                    else if (key.EqualsEx("Alt"))
                        path = path.Replace(DirectorySeparatorStr, new string(Path.AltDirectorySeparatorChar, num));

                if (!string.IsNullOrEmpty(prefix) && !path.StartsWithEx(prefix))
                    path = prefix + path;
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
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="path0">
        ///     The first path to combine.
        /// </param>
        /// <param name="path1">
        ///     The second path to combine.
        /// </param>
        public static string Combine(char[] invalidPathChars, string path0, string path1) =>
            Combine(invalidPathChars, path0 + DirectorySeparatorStr + path1);

        /// <summary>
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="path0">
        ///     The first path to combine.
        /// </param>
        /// <param name="path1">
        ///     The second path to combine.
        /// </param>
        /// <param name="path2">
        ///     The third path to combine.
        /// </param>
        public static string Combine(char[] invalidPathChars, string path0, string path1, string path2) =>
            Combine(invalidPathChars, path0 + DirectorySeparatorStr + path1 + DirectorySeparatorStr + path2);

        /// <summary>
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="paths">
        ///     An array of paths to be combined.
        /// </param>
        public static string Combine(char[] invalidPathChars, params string[] paths) =>
            Combine(invalidPathChars, paths?.Join(Path.DirectorySeparatorChar));

        /// <summary>
        ///     Combines the specified <see cref="Environment.SpecialFolder"/> value with
        ///     the specified string and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="specialFolder">
        ///     A specified enumerated constant used to retrieve directory paths to system
        ///     special folders.
        /// </param>
        /// <param name="path0">
        ///     The path to be filtered.
        /// </param>
        public static string Combine(char[] invalidPathChars, Environment.SpecialFolder specialFolder, string path0) =>
            Combine(invalidPathChars, Environment.GetFolderPath(specialFolder), path0);

        /// <summary>
        ///     Combines the specified <see cref="Environment.SpecialFolder"/> value with
        ///     the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="specialFolder">
        ///     A specified enumerated constant used to retrieve directory paths to system
        ///     special folders.
        /// </param>
        /// <param name="path0">
        ///     The first path to combine.
        /// </param>
        /// <param name="path1">
        ///     The second path to combine.
        /// </param>
        public static string Combine(char[] invalidPathChars, Environment.SpecialFolder specialFolder, string path0, string path1) =>
            Combine(invalidPathChars, specialFolder, path0 + DirectorySeparatorStr + path1);

        /// <summary>
        ///     Combines the specified <see cref="Environment.SpecialFolder"/> value with
        ///     the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="specialFolder">
        ///     A specified enumerated constant used to retrieve directory paths to system
        ///     special folders.
        /// </param>
        /// <param name="path0">
        ///     The first path to combine.
        /// </param>
        /// <param name="path1">
        ///     The second path to combine.
        /// </param>
        /// <param name="path2">
        ///     The third path to combine.
        /// </param>
        public static string Combine(char[] invalidPathChars, Environment.SpecialFolder specialFolder, string path0, string path1, string path2) =>
            Combine(invalidPathChars, specialFolder, path0 + DirectorySeparatorStr + path1 + DirectorySeparatorStr + path2);

        /// <summary>
        ///     Combines the specified <see cref="Environment.SpecialFolder"/> value with
        ///     the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="specialFolder">
        ///     A specified enumerated constant used to retrieve directory paths to system
        ///     special folders.
        /// </param>
        /// <param name="paths">
        ///     An array of paths to be combined.
        /// </param>
        public static string Combine(char[] invalidPathChars, Environment.SpecialFolder specialFolder, params string[] paths) =>
            Combine(invalidPathChars, specialFolder, paths?.Join(Path.DirectorySeparatorChar));

        /// <summary>
        ///     Combines the specified <see cref="Environment.SpecialFolder"/> value with
        ///     the specified string and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="specialFolder">
        ///     A specified enumerated constant used to retrieve directory paths to system
        ///     special folders.
        /// </param>
        /// <param name="path0">
        ///     The path to be filtered.
        /// </param>
        public static string Combine(Environment.SpecialFolder specialFolder, string path0) =>
            Combine(InvalidPathChars, specialFolder, path0);

        /// <summary>
        ///     Combines the specified <see cref="Environment.SpecialFolder"/> value with
        ///     the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="path0">
        ///     The first path to combine.
        /// </param>
        /// <param name="path1">
        ///     The second path to combine.
        /// </param>
        public static string Combine(Environment.SpecialFolder specialFolder, string path0, string path1) =>
            Combine(InvalidPathChars, specialFolder, path0, path1);

        /// <summary>
        ///     Combines the specified <see cref="Environment.SpecialFolder"/> value with
        ///     the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="path0">
        ///     The first path to combine.
        /// </param>
        /// <param name="path1">
        ///     The second path to combine.
        /// </param>
        /// <param name="path2">
        ///     The third path to combine.
        /// </param>
        public static string Combine(Environment.SpecialFolder specialFolder, string path0, string path1, string path2) =>
            Combine(InvalidPathChars, specialFolder, path0, path1, path2);

        /// <summary>
        ///     Combines the specified <see cref="Environment.SpecialFolder"/> value with
        ///     the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="specialFolder">
        ///     A specified enumerated constant used to retrieve directory paths to system
        ///     special folders.
        /// </param>
        /// <param name="paths">
        ///     An array of paths to be combined.
        /// </param>
        public static string Combine(Environment.SpecialFolder specialFolder, params string[] paths) =>
            Combine(InvalidPathChars, specialFolder, paths);

        /// <summary>
        ///     Filters the specified string into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="path0">
        ///     The string to be filtered.
        /// </param>
        public static string Combine(string path0) =>
            Combine(InvalidPathChars, path0);

        /// <summary>
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="path0">
        ///     The first path to combine.
        /// </param>
        /// <param name="path1">
        ///     The second path to combine.
        /// </param>
        public static string Combine(string path0, string path1) =>
            Combine(InvalidPathChars, path0, path1);

        /// <summary>
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="path0">
        ///     The first path to combine.
        /// </param>
        /// <param name="path1">
        ///     The second path to combine.
        /// </param>
        /// <param name="path2">
        ///     The third path to combine.
        /// </param>
        public static string Combine(string path0, string path1, string path2) =>
            Combine(InvalidPathChars, path0, path1, path2);

        /// <summary>
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     The sequence of strings to be filtered.
        /// </param>
        public static string Combine(params string[] paths) =>
            Combine(InvalidPathChars, paths);

        /// <summary>
        ///     Filters the specified string into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        ///     <para>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path
        ///         levels.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="path0">
        ///     An array of parts of the path.
        /// </param>
        public static string AltCombine(char[] invalidPathChars, string path0)
        {
            var path = Combine(invalidPathChars, path0);
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path0));
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
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        ///     <para>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path
        ///         levels.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="path0">
        ///     The string to be filtered.
        /// </param>
        public static string AltCombine(char[] invalidPathChars, string path0, string path1) =>
            AltCombine(invalidPathChars, Combine((char[])null, path0, path1));

        /// <summary>
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        ///     <para>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path
        ///         levels.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="path0">
        ///     The string to be filtered.
        /// </param>
        public static string AltCombine(char[] invalidPathChars, string path0, string path1, string path2) =>
            AltCombine(invalidPathChars, Combine((char[])null, path0, path1, path2));

        /// <summary>
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        ///     <para>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path
        ///         levels.
        ///     </para>
        /// </summary>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        /// <param name="paths">
        ///     The sequence of strings to be filtered.
        /// </param>
        public static string AltCombine(char[] invalidPathChars, params string[] paths) =>
            AltCombine(invalidPathChars, Combine(null, paths));

        /// <summary>
        ///     Filters the specified string into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        ///     <para>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path
        ///         levels.
        ///     </para>
        /// </summary>
        /// <param name="path0">
        ///     The string to be filtered.
        /// </param>
        public static string AltCombine(string path0) =>
            AltCombine(InvalidPathChars, path0);

        /// <summary>
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        ///     <para>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path
        ///         levels.
        ///     </para>
        /// </summary>
        /// <param name="path0">
        ///     The string to be filtered.
        /// </param>
        public static string AltCombine(string path0, string path1) =>
            AltCombine(InvalidPathChars, path0, path1);

        /// <summary>
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        ///     <para>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path
        ///         levels.
        ///     </para>
        /// </summary>
        /// <param name="path0">
        ///     The string to be filtered.
        /// </param>
        public static string AltCombine(string path0, string path1, string path2) =>
            AltCombine(InvalidPathChars, path0, path1, path2);

        /// <summary>
        ///     Combines the specified strings and filters the result into a valid path.
        ///     <para>
        ///         Allows relative paths, superordinate directory navigation and
        ///         environment variables based on
        ///         <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>.
        ///     </para>
        ///     <para>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to separate path
        ///         levels.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     The sequence of strings to be filtered.
        /// </param>
        public static string AltCombine(params string[] paths) =>
            AltCombine(InvalidPathChars, paths);

        /// <summary>
        ///     Returns the directory information for the specified path string.
        /// </summary>
        /// <param name="path">
        ///     The path of a file or directory.
        /// </param>
        /// <param name="convertEnvVars">
        ///     <see langword="true"/> to convert environment variables; otherwise,
        ///     <see langword="false"/>.
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
        ///     Returns a fully qualified path with unique name starting with a given
        ///     prefix, followed by a hash of the specified length and a specified suffix.
        /// </summary>
        /// <param name="baseDir">
        ///     This home directory for the uniquely named file or directory.
        /// </param>
        /// <param name="namePrefix">
        ///     This text is at the beginning of the name.
        ///     <para>
        ///         Uppercase letters are converted to lowercase letters. Supported
        ///         characters are only from '0' to '9' and from 'a' to 'z' but can be
        ///         completely empty to omit the prefix.
        ///     </para>
        /// </param>
        /// <param name="nameSuffix">
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
        public static IEnumerable<Process> GetLocks(IEnumerable<string> paths) =>
            paths != null ? FileEx.GetLocks(paths.Select(Combine).SelectMany(x => IsDir(x) ? DirectoryEx.GetFiles(x, SearchOption.AllDirectories) : new[] { x })) : null;

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
        ///         Immediately stops all specified processes that are locking this file or
        ///         directory.
        ///     </para>
        /// </summary>
        /// <param name="path">
        ///     The path of the file or directory to be deleted.
        /// </param>
        /// <param name="elevated">
        ///     <see langword="true"/> to run this task with administrator privileges if
        ///     the deletion fails; otherwise, <see langword="false"/>.
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
                    foreach (var p in GetLocks(target))
                    {
                        if (p == current || ProcessEx.Terminate(p) || locked)
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
                    sb.AppendFormatLineCurrent("ROBOCOPY \"{0}\" \"{1}\" /MIR", tmpDir, target);
                    sb.AppendFormatLineCurrent("RMDIR /S /Q \"{0}\"", tmpDir);
                    sb.AppendFormatLineCurrent("RMDIR /S /Q \"{0}\"", target);
                    sb.AppendLine("EXIT");
                    File.WriteAllText(helper, sb.ToStringThenClear());

                    var call = $"CALL \"{helper}\"";
                    if (locked)
                        CmdExec.WaitForExitThenCmd(call, curName, elevated);
                    else
                        using (var p = CmdExec.Send(call, elevated, false))
                            if (p?.HasExited ?? false)
                                p.WaitForExit(timelimit);

                    DirectoryEx.Delete(tmpDir);
                    DirectoryEx.Delete(target);
                    CmdExec.WaitThenDelete(helper, elevated);
                }
                else
                    try
                    {
                        File.Delete(target);
                    }
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        if (locked)
                            CmdExec.WaitForExitThenDelete(target, curName, true);
                        else
                            using (var p = CmdExec.Send(sb.ToStringThenClear(), elevated, false))
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
