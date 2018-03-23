#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PathEx.cs
// Version:  2018-03-21 21:35
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Runtime.Serialization;
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

        private static Dictionary<int, string> CachedPaths { get; } = new Dictionary<int, string>();

        /// <summary>
        ///     Gets the full process executable path of the assembly based on
        ///     <see cref="Assembly.GetEntryAssembly()"/>.CodeBase.
        /// </summary>
        public static string LocalPath { get; } = Assembly.GetEntryAssembly().CodeBase.ToUri()?.LocalPath;

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
                    throw new ArgumentException($"The path length is lower than 3 characters. - PATH: \'{path}\'");
                if (!path.Contains(Path.DirectorySeparatorChar))
                {
                    if (!path.Contains(Path.AltDirectorySeparatorChar))
                        throw new ArgumentException($"The path does not contain any separator. - PATH: \'{path}\'");
                    throw new ArgumentException($"The path does not contain a valid separator. - PATH: \'{path}\'");
                }
                if (path.StartsWith("\\\\?\\"))
                    throw new NotSupportedException($"The \"\\\\?\\\" prefix is not supported. - PATH: \'{path}\'");
                if (path.Contains(new string(Path.DirectorySeparatorChar, 2)))
                    throw new ArgumentException($"The path cannot contain several consecutive separators. - PATH: \'{path}\'");
                var drive = path.Substring(0, 3);
                if (!Regex.IsMatch(drive, @"^[a-zA-Z]:\\$"))
                    throw new DriveNotFoundException($"The path does not contain any drive. - PATH: \'{path}\'");
                if (path.Length >= 260)
                    throw new PathTooLongException($"The specified path is longer than 260 characters. - PATH: \'{path}\'");
                var levels = path.Split(Path.DirectorySeparatorChar);
                if (levels.Any(s => s.Length >= 255))
                    throw new PathTooLongException($"A segment of the path is longer than 255 characters. - PATH: \'{path}\'");
                var dirLength = Path.HasExtension(path) ? levels.Take(levels.Length - 1).Join(Path.DirectorySeparatorChar).Length : path.Length;
                if (dirLength >= 248)
                    throw new PathTooLongException($"The directory name is longer than 248 characters. - PATH: \'{path}\'");
                if (!DriveInfo.GetDrives().Select(di => di.Name).Contains(drive))
                    throw new DriveNotFoundException($"The path does not contain a valid drive. - PATH: \'{path}\'");
                var subPath = path.Substring(3);
                if (subPath.Any(c => c != Path.DirectorySeparatorChar && Path.GetInvalidFileNameChars().Contains(c)))
                    throw new ArgumentException("The path contains invalid characters.");
                return true;
            }
            catch (Exception ex)
            {
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
            FileEx.MatchAttributes(path, FileAttributes.Directory);

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
        ///         based on <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>;
        ///         for example, write <code>"%Desktop%"</code>, cases are ignored as well.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        public static string Combine(char[] invalidPathChars, params string[] paths)
        {
            var path = string.Empty;
            try
            {
                if (paths.Length == 0 || paths.Count(string.IsNullOrWhiteSpace) == paths.Length)
                    throw new ArgumentNullException(nameof(paths));
                var hash = paths.GetHashCode();
                if (hash != -1 && CachedPaths.ContainsKey(hash))
                {
                    path = CachedPaths[hash];
                    goto Return;
                }
                var levels = paths;
                var sepChar = Path.DirectorySeparatorChar;
                for (var i = 0; i < levels.Length; i++)
                {
                    if (i > 0)
                        levels[i] = levels[i].RemoveChar(Path.VolumeSeparatorChar);
                    if (invalidPathChars != null)
                        levels[i] = levels[i].RemoveChar(invalidPathChars);
                    levels[i] = levels[i].Replace(Path.AltDirectorySeparatorChar, sepChar);
                    if (levels[i].Contains(sepChar))
                        levels[i] = levels[i].Split(sepChar).Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Trim()).Join(sepChar);
                    if (i > 0)
                        continue;
                    if (levels[i].EndsWith(Path.VolumeSeparatorChar.ToString()))
                        levels[i] += sepChar;
                }
                path = Path.Combine(levels);
                string key = null;
                byte num = 0;
                if (path.StartsWith("%") && (path.Contains($"%{sepChar}") || path.EndsWith("%")))
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
                if (path.Contains($"{sepChar}.."))
                    path = Path.GetFullPath(path);
                if (path.Contains('.'))
                    path = path.TrimEnd('.');
                if (!string.IsNullOrEmpty(key) || num > 1)
                    if (string.IsNullOrEmpty(key))
                        path = path.Replace(Path.DirectorySeparatorChar.ToString(), new string(Path.DirectorySeparatorChar, num));
                    else if (key.EqualsEx("Alt"))
                        path = path.Replace(Path.DirectorySeparatorChar.ToString(), new string(Path.AltDirectorySeparatorChar, num));
                if (hash != -1)
                {
                    if (CachedPaths.Count >= byte.MaxValue)
                    {
                        var code = CachedPaths.Keys.FirstOrDefault(x => x != hash);
                        if (CachedPaths.ContainsKey(code))
                            CachedPaths.Remove(code);
                    }
                    CachedPaths[hash] = path;
                }
            }
            catch (ArgumentException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            Return:
            return path;
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
        ///         based on <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>;
        ///         for example, write <code>"%Desktop%"</code>, cases are ignored as well.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static string Combine(params string[] paths) =>
            Combine(InvalidPathChars, paths);

        /// <summary>
        ///     <para>
        ///         Combines an array of strings, based on <see cref="Combine(string[])"/>, into a
        ///         path.
        ///     </para>
        ///     <para>
        ///         <c>
        ///             Hint:
        ///         </c>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to seperate path levels.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
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
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return path;
        }

        /// <summary>
        ///     <para>
        ///         Combines an array of strings, based on <see cref="Combine(string[])"/>, into a
        ///         path.
        ///     </para>
        ///     <para>
        ///         <c>
        ///             Hint:
        ///         </c>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to seperate path levels.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
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
        ///     true to convert environment variables; otherwise, false.
        /// </param>
        public static string GetDirectoryName(string path, bool convertEnvVars = false)
        {
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
        ///     Returns a uniquely directory name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="prefix">
        ///     This text is at the beginning of the name.
        /// </param>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempDirName(string prefix = "tmp", int len = 4)
        {
            var s = prefix;
            var g = new string(Guid.NewGuid().ToString().Where(char.IsLetterOrDigit).ToArray());
            s = $"{s.ToLower()}{g.Substring(0, len.IsBetween(4, 24) ? len : 4).ToUpper()}";
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
        /// <param name="prefix">
        ///     This text is at the beginning of the name.
        /// </param>
        /// <param name="suffix">
        ///     This text is at the end of the name.
        /// </param>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempFileName(string prefix, string suffix = ".tmp", int len = 4) =>
            GetTempDirName(prefix, len) + suffix;

        /// <summary>
        ///     Returns a uniquely file name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="prefix">
        ///     This text is at the beginning of the name.
        /// </param>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempFileName(string prefix = "tmp", int len = 4) =>
            GetTempFileName(prefix, ".tmp", len);

        /// <summary>
        ///     Returns a uniquely file name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempFileName(int len) =>
            GetTempFileName("tmp", len);

        /// <summary>
        ///     Creates a link to the specified path.
        /// </summary>
        /// <param name="targetPath">
        ///     The file or directory to be linked.
        /// </param>
        /// <param name="linkPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="startArgs">
        ///     The arguments which applies when this shortcut is executed.
        /// </param>
        /// <param name="linkIcon">
        ///     The icon resource path for this shortcut.
        /// </param>
        /// <param name="linkIconId">
        ///     The icon resource id for this shortcut.
        /// </param>
        /// <param name="skipExists">
        ///     true to skip existing shortcuts, even if the target path of
        ///     the same; otherwise, false.
        /// </param>
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static bool CreateShortcut(string targetPath, string linkPath, string startArgs = null, string linkIcon = null, int linkIconId = 0, bool skipExists = false)
        {
            try
            {
                var ext = Path.GetExtension(linkPath);
                var link = Combine(!ext.EqualsEx(".lnk") ? $"{linkPath}.lnk" : linkPath);
                var dir = Path.GetDirectoryName(link);
                var path = Combine(targetPath);
                if (!Directory.Exists(dir) || !DirOrFileExists(path))
                    return false;
                if (File.Exists(link))
                {
                    if (skipExists)
                        return true;
                    File.Delete(link);
                }
                var env = EnvironmentEx.GetVariablePathFull(path, false, false);
                dir = Path.GetDirectoryName(env);
                var shell = (IShellLink)new ShellLink();
                if (!string.IsNullOrWhiteSpace(startArgs))
                    shell.SetArguments(startArgs);
                shell.SetDescription(string.Empty);
                shell.SetPath(env);
                var ico = EnvironmentEx.GetVariablePathFull(linkIcon, false, false);
                var id = linkIconId;
                if (string.IsNullOrWhiteSpace(ico))
                    if (IsDir(path))
                    {
                        ico = "%SystemRoot%\\System32\\imageres.dll";
                        id = 3;
                    }
                    else
                    {
                        ico = env;
                        id = 0;
                    }
                shell.SetIconLocation(ico, id);
                shell.SetWorkingDirectory(dir);
                ((IPersistFile)shell).Save(link, false);
                return File.Exists(link);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Creates a link to the specified path.
        /// </summary>
        /// <param name="linkPath">
        ///     The file or directory to be linked.
        /// </param>
        /// <param name="targetPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="startArgs">
        ///     The arguments which applies when this shortcut is executed.
        /// </param>
        /// <param name="skipExists">
        ///     true to skip existing shortcuts, even if the target path of
        ///     the same; otherwise, false.
        /// </param>
        public static bool CreateShortcut(string targetPath, string linkPath, string startArgs, bool skipExists) =>
            CreateShortcut(targetPath, linkPath, startArgs, null, 0, skipExists);

        /// <summary>
        ///     Creates a link to the specified path.
        /// </summary>
        /// <param name="linkPath">
        ///     The file or directory to be linked.
        /// </param>
        /// <param name="targetPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="skipExists">
        ///     true to skip existing shortcuts, even if the target path of
        ///     the same; otherwise, false.
        /// </param>
        public static bool CreateShortcut(string targetPath, string linkPath, bool skipExists) =>
            CreateShortcut(targetPath, linkPath, null, null, 0, skipExists);

        /// <summary>
        ///     Removes a link of the specified file or directory.
        /// </summary>
        /// <param name="path">
        ///     The shortcut to be removed.
        /// </param>
        public static bool DestroyShortcut(string path)
        {
            try
            {
                var target = GetShortcutTarget(path);
                if (string.IsNullOrEmpty(target))
                    throw new ArgumentNullException(nameof(target));
                var link = Combine(path);
                File.Delete(link);
                return File.Exists(link);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Returns the target path of the specified link.
        /// </summary>
        /// <param name="path">
        ///     The shortcut path to get the target path.
        /// </param>
        public static string GetShortcutTarget(string path)
        {
            try
            {
                if (!path.EndsWithEx(".lnk"))
                    throw new ArgumentException();
                string targetPath;
                using (var fs = File.Open(Combine(path), FileMode.Open, FileAccess.Read))
                {
                    var br = new BinaryReader(fs);
                    fs.Seek(0x14, SeekOrigin.Begin);
                    var flags = br.ReadUInt32();
                    if ((flags & 1) == 1)
                    {
                        fs.Seek(0x4c, SeekOrigin.Begin);
                        fs.Seek(br.ReadUInt16(), SeekOrigin.Current);
                    }
                    var start = fs.Position;
                    var length = br.ReadUInt32();
                    fs.Seek(0xc, SeekOrigin.Current);
                    fs.Seek(start + br.ReadUInt32(), SeekOrigin.Begin);
                    targetPath = new string(br.ReadChars((int)(start + length - fs.Position - 2)));
                    var begin = targetPath.IndexOf("\0\0", StringComparison.Ordinal);
                    if (begin <= -1)
                        return targetPath;
                    var end = targetPath.IndexOf(new string(Path.DirectorySeparatorChar, 2), begin + 2, StringComparison.Ordinal) + 2;
                    end = targetPath.IndexOf('\0', end) + 1;
                    targetPath = Path.Combine(targetPath.Substring(0, begin), targetPath.Substring(end));
                }
                return targetPath;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     Creates a symbolic link to the specified file or directory based on command prompt
        ///     which allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="linkPath">
        ///     The file or directory to be linked.
        /// </param>
        /// <param name="destPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="destIsDir">
        ///     true to determine that the destination path is a directory; otherwise, false.
        /// </param>
        /// <param name="backup">
        ///     true to create an backup for existing files; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to create this link with highest privileges; otherwise, false.
        /// </param>
        public static bool CreateSymbolicLink(string linkPath, string destPath, bool destIsDir, bool backup = false, bool elevated = false)
        {
            /*
             * The idea was to replace the code below with this code that uses the
             * p/invoke method to create symbolic links. But this doesn't work
             * without administrator privileges while a CMD function called
             * MKLINK can do that simply as normal user...

            var dest = PathEx.Combine(targetPath);
            try
            {
                if (targetIsDir)
                {
                    if (!Directory.Exists(dest))
                        Directory.CreateDirectory(dest);
                }
                else
                {
                    if (!File.Exists(dest))
                        File.Create(dest).Close();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }

            var link = PathEx.Combine(linkPath);
            try
            {
                var linkDir = Path.GetDirectoryName(link);
                if (linkDir != null && !Directory.Exists(linkDir))
                    Directory.CreateDirectory(linkDir);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }

            if (PathEx.DirOrFileExists(link))
                if (!DirIsLink(link) && backup)
                    try
                    {
                        File.Move(link, link + $"-{{{EnvironmentEx.MachineId}}}.backup");
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                        return false;
                    }
                else
                    try
                    {
                        if (Directory.Exists(link))
                            Directory.Delete(link);
                        if (File.Exists(link))
                            File.Delete(link);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                    }


            if (!PathEx.DirOrFileExists(dest) || PathEx.DirOrFileExists(link))
                return false;

            var created = WinApi.SafeNativeMethods.CreateSymbolicLink(link, dest, (WinApi.SymbolicLinkFlags)Convert.ToInt32(targetIsDir));
            if (created)
                SetAttributes(link, FileAttributes.Hidden);

            return created;
            */

            var dest = Combine(destPath);
            try
            {
                if (destIsDir)
                {
                    if (!Directory.Exists(dest))
                        Directory.CreateDirectory(dest);
                }
                else
                {
                    if (!File.Exists(dest))
                        File.Create(dest).Close();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }

            var link = Combine(linkPath);
            try
            {
                var linkDir = Path.GetDirectoryName(link);
                if (!Directory.Exists(linkDir))
                    if (linkDir != null)
                        Directory.CreateDirectory(linkDir);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }

            var cmd = string.Empty;
            if (backup)
                if (DirOrFileExists(link))
                    if (!DirectoryEx.IsLink(link))
                        cmd += $"MOVE /Y \"{link}\" \"{link}-{{{EnvironmentEx.MachineId}}}.backup\"";
                    else
                        DestroySymbolicLink(link, true, true, elevated);

            if (DirOrFileExists(link))
            {
                if (!string.IsNullOrEmpty(cmd))
                    cmd += " & ";
                cmd += $"{(destIsDir ? "RD /S /Q" : "DEL /F /Q")} \"{link}\"";
            }

            if (DirOrFileExists(dest))
            {
                if (!string.IsNullOrEmpty(cmd))
                    cmd += " & ";
                cmd += $"MKLINK {(destIsDir ? "/J " : string.Empty)}\"{link}\" \"{dest}\" && ATTRIB +H \"{link}\" /L";
            }

            if (string.IsNullOrEmpty(cmd))
                return false;

            int? exitCode;
            using (var p = ProcessEx.Send(cmd, elevated, false))
            {
                if (p?.HasExited == false)
                    p.WaitForExit();
                exitCode = p?.ExitCode;
            }
            return exitCode == 0 && DirOrFileIsLink(link);
        }

        /// <summary>
        ///     Removes an symbolic link of the specified file or directory link based on command
        ///     prompt which allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="path">
        ///     The link to be removed.
        /// </param>
        /// <param name="pathIsDir">
        ///     true to determine that the path is a directory; otherwise, false.
        /// </param>
        /// <param name="backup">
        ///     true to restore found backups; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to remove this link with highest privileges; otherwise, false.
        /// </param>
        public static bool DestroySymbolicLink(string path, bool pathIsDir, bool backup = false, bool elevated = false)
        {
            var link = Combine(path);
            var isLink = DirOrFileIsLink(link);
            var cmd = $"{(pathIsDir ? "RD /Q" : "DEL /F /Q")}{(!pathIsDir && isLink ? " /A:L" : string.Empty)} \"{link}\"";
            if (backup && DirOrFileExists($"{link}-{{{EnvironmentEx.MachineId}}}.backup"))
                cmd += $" & MOVE /Y \"{link}-{{{EnvironmentEx.MachineId}}}.backup\" \"{link}\"";
            if (string.IsNullOrEmpty(cmd))
                return false;
            int? exitCode;
            using (var p = ProcessEx.Send(cmd, elevated, false))
            {
                if (p?.HasExited == false)
                    p.WaitForExit();
                exitCode = p?.ExitCode;
            }
            return exitCode == 0 && isLink;
        }

        /// <summary>
        ///     Find out which processes have a lock on this file instance member.
        /// </summary>
        /// <param name="files">
        ///     An sequence of strings that contains the file paths to check.
        /// </param>
        public static IEnumerable<Process> GetLocks(IEnumerable<string> files)
        {
            var locks = default(IEnumerable<Process>);
            try
            {
                var paths = files?.ToArray();
                if (paths == null || !paths.Any())
                    throw new ArgumentNullException(nameof(paths));
                var result = WinApi.NativeMethods.RmStartSession(out var handle, 0, Guid.NewGuid().ToString());
                if (result != 0)
                    throw new InvalidOperationException("Could not begin restart session. Unable to determine file locker.");
                try
                {
                    var resources = paths.Select(s => Combine(s)).Where(File.Exists).ToArray();
                    if (!resources.Any())
                        throw new PathNotFoundException(paths.Join("'; '"));
                    result = WinApi.NativeMethods.RmRegisterResources(handle, (uint)resources.Length, resources, 0u, null, 0u, null);
                    if (result != 0)
                        throw new Exception("Could not register resource.");
                    var pnProcInfo = 0u;
                    var lpdwRebootReasons = 0u;
                    result = WinApi.NativeMethods.RmGetList(handle, out var pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);
                    if (result != 234)
                        throw new InvalidOperationException("Could not list processes locking resource. Failed to get size of result.");
                    var processInfo = new WinApi.RmProcessInfo[pnProcInfoNeeded];
                    pnProcInfo = pnProcInfoNeeded;
                    result = WinApi.NativeMethods.RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
                    if (result != 0)
                        throw new InvalidOperationException("Could not list processes locking resource.");
                    var ids = processInfo.Select(e => e.Process.dwProcessId);
                    var procs = new List<Process>();
                    foreach (var id in ids)
                    {
                        Process proc;
                        try
                        {
                            proc = Process.GetProcessById(id);
                        }
                        catch
                        {
                            continue;
                        }
                        if (procs.Contains(proc))
                            continue;
                        procs.Add(proc);
                    }
                    locks = procs;
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
                finally
                {
                    WinApi.NativeMethods.RmEndSession(handle);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return locks;
        }

        /// <summary>
        ///     Find out which processes have a lock on the specified path.
        /// </summary>
        /// <param name="path">
        ///     The full path to check.
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
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return locks;
        }

        /// <summary>
        ///     <para>
        ///         Deletes any file or directory.
        ///     </para>
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
                    foreach (var p in GetLocks(target))
                    {
                        if (p == current)
                        {
                            p.Dispose();
                            continue;
                        }
                        if (ProcessEx.Terminate(p) || locked)
                            continue;
                        locked = true;
                    }
                    if (!locked)
                        foreach (var p in GetLocks(target))
                        {
                            if (!locked && p != current)
                                locked = true;
                            p.Dispose();
                        }
                }
                var curName = $"{ProcessEx.CurrentName}.exe";
                if (IsDir(target))
                {
                    var tmpDir = Combine(Path.GetTempPath(), GetTempDirName());
                    if (!Directory.Exists(tmpDir))
                        Directory.CreateDirectory(tmpDir);
                    var helper = Combine(Path.GetTempPath(), GetTempFileName("tmp", ".cmd"));
                    var content = string.Format(Resources.Cmd_DeleteForce, tmpDir, target);
                    File.WriteAllText(helper, content);
                    var command = string.Format(Resources.Cmd_Call, helper);
                    if (locked)
                    {
                        command = string.Format(Resources.Cmd_WaitForProcThenCmd, curName, command);
                        ProcessEx.Send(command, elevated);
                    }
                    else
                        using (var p = ProcessEx.Send(command, elevated, false))
                            if (p?.HasExited == false)
                                p.WaitForExit(timelimit);
                    DirectoryEx.Delete(tmpDir);
                    DirectoryEx.Delete(target);
                    command = string.Format(Resources.Cmd_DeleteFile, helper);
                    command = string.Format(Resources.Cmd_WaitThenCmd, 5, command);
                    command = string.Format(Resources.Cmd_WaitForProcThenCmd, curName, command);
                    ProcessEx.Send(command);
                }
                else
                    try
                    {
                        File.Delete(target);
                    }
                    catch
                    {
                        var command = string.Format(Resources.Cmd_DeleteFile, target);
                        if (locked)
                        {
                            command = string.Format(Resources.Cmd_WaitForProcThenCmd, curName, command);
                            ProcessEx.Send(command, true);
                        }
                        else
                            using (var p = ProcessEx.Send(command, elevated, false))
                                if (p?.HasExited == false)
                                    p.WaitForExit(timelimit);
                    }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return !DirOrFileExists(target);
        }

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        private class ShellLink { }

        [ComImport]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellLink
        {
            void GetPath([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }
    }

    /// <summary>
    ///     The exception that is thrown when an attempt to access a target that does not exist
    ///     fails.
    /// </summary>
    [Serializable]
    public class PathNotFoundException : Exception
    {
        /// <summary>
        ///     Create the exception.
        /// </summary>
        public PathNotFoundException() { }

        /// <summary>
        ///     Create the exception with path.
        /// </summary>
        /// <param name="target">
        ///     Exception target.
        /// </param>
        public PathNotFoundException(string target) : base(target) =>
            Message = $"Could not find target \'{target}\'.";

        /// <summary>
        ///     Create the exception with path and inner cause.
        /// </summary>
        /// <param name="target">
        ///     Exception target.
        /// </param>
        /// <param name="innerException">
        ///     Exception inner cause.
        /// </param>
        public PathNotFoundException(string target, Exception innerException) : base(target, innerException) =>
            Message = $"Could not find target \'{target}\'.";

        /// <summary>
        ///     Initializes a new instance of the <see cref="PathNotFoundException"/> class with serialized data.
        /// </summary>
        protected PathNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Gets the error message and the path, or only the exception message if no path
        ///     is set.
        /// </summary>
        public sealed override string Message { get; } = "Unable to find the target from the specified path.";
    }
}
