#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: DirectoryEx.cs
// Version:  2019-10-22 15:44
// 
// Copyright (c) 2019, Si13n7 Developments (r)
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
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     Provides static methods based on the <see cref="Directory"/> class to perform
    ///     directory operations.
    /// </summary>
    public static class DirectoryEx
    {
        /// <summary>
        ///     Determines whether the specified directory exists.
        /// </summary>
        /// <param name="path">
        ///     The directory to check.
        /// </param>
        public static bool Exists(string path)
        {
            var src = PathEx.Combine(path);
            return Directory.Exists(src);
        }

        /// <summary>
        ///     Determines whether the specified path specifies the specified attributes.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory instance member that contains the directory to check.
        /// </param>
        /// <param name="attr">
        ///     The attributes to match.
        /// </param>
        public static bool MatchAttributes(this DirectoryInfo dirInfo, FileAttributes attr)
        {
            try
            {
                if (dirInfo == null)
                    throw new ArgumentNullException(nameof(dirInfo));
                if (!dirInfo.Exists)
                    return false;
                var da = dirInfo.Attributes;
                return (da & attr) != 0;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified path specifies the specified attributes.
        /// </summary>
        /// <param name="path">
        ///     The directory to check.
        /// </param>
        /// <param name="attr">
        ///     The attributes to match.
        /// </param>
        public static bool MatchAttributes(string path, FileAttributes attr)
        {
            try
            {
                var src = PathEx.Combine(path);
                var di = new DirectoryInfo(src);
                return di.MatchAttributes(attr);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified directory is hidden.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory instance member that contains the directory to check.
        /// </param>
        public static bool IsHidden(this DirectoryInfo dirInfo) =>
            dirInfo?.MatchAttributes(FileAttributes.Hidden) == true;

        /// <summary>
        ///     Determines whether the specified directory is hidden.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static bool IsHidden(string path) =>
            MatchAttributes(path, FileAttributes.Hidden);

        /// <summary>
        ///     Determines whether the specified directory is specified as reparse point.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory instance member that contains the directory to check.
        /// </param>
        public static bool IsLink(this DirectoryInfo dirInfo) =>
            dirInfo?.MatchAttributes(FileAttributes.ReparsePoint) == true;

        /// <summary>
        ///     Determines whether the specified directory is specified as reparse point.
        /// </summary>
        /// <param name="path">
        ///     The directory to check.
        /// </param>
        public static bool IsLink(string path)
        {
            try
            {
                var src = PathEx.Combine(path);
                var di = new DirectoryInfo(src);
                return di.IsLink();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Sets the specified attributes for the specified directory.
        /// </summary>
        /// <param name="path">
        ///     The directory to change.
        /// </param>
        /// <param name="attr">
        ///     The attributes to set.
        /// </param>
        public static void SetAttributes(string path, FileAttributes attr)
        {
            try
            {
                var src = PathEx.Combine(path);
                var di = new DirectoryInfo(src);
                if (!di.Exists)
                    return;
                if (attr != FileAttributes.Normal)
                    di.Attributes |= attr;
                else
                    di.Attributes = attr;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search
        ///     pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">
        ///     The relative or absolute path to the directory to search. This string is
        ///     not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in path. This
        ///     parameter can contain a combination of valid literal path and wildcard
        ///     (* and ?) characters, but doesn't support regular expressions.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation
        ///     should include only the current directory or should include all  subdirectories.
        /// </param>
        public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var dirs = default(IEnumerable<string>);
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var dir = PathEx.Combine(path);
                if (!Directory.Exists(dir))
                    throw new PathNotFoundException(dir);
                dirs = Directory.EnumerateDirectories(path, searchPattern, searchOption);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return dirs;
        }

        /// <summary>
        ///     Returns an enumerable collection of directory names that match a search
        ///     pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">
        ///     The relative or absolute path to the directory to search. This string is
        ///     not case-sensitive.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation
        ///     should include only the current directory or should include all  subdirectories.
        /// </param>
        public static IEnumerable<string> EnumerateDirectories(string path, SearchOption searchOption) =>
            EnumerateDirectories(path, "*", searchOption);

        /// <summary>
        ///     Returns the names of the subdirectories (including their paths) that match the
        ///     specified search pattern in the specified directory, and optionally searches
        ///     subdirectories.
        /// </summary>
        /// <param name="path">
        ///     The relative or absolute path to the directory to search. This string is
        ///     not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of directories in path. This
        ///     parameter can contain a combination of valid literal path and wildcard
        ///     (* and ?) characters, but doesn't support regular expressions.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation
        ///     should include only the current directory or should include all  subdirectories.
        /// </param>
        public static string[] GetDirectories(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var dirs = default(string[]);
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var dir = PathEx.Combine(path);
                if (!Directory.Exists(dir))
                    throw new PathNotFoundException(dir);
                dirs = Directory.GetDirectories(path, searchPattern, searchOption);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return dirs;
        }

        /// <summary>
        ///     Returns the names of the subdirectories (including their paths) that match the
        ///     specified search pattern in the specified directory, and optionally searches
        ///     subdirectories.
        /// </summary>
        /// <param name="path">
        ///     The relative or absolute path to the directory to search. This string is
        ///     not case-sensitive.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation
        ///     should include only the current directory or should include all  subdirectories.
        /// </param>
        public static string[] GetDirectories(string path, SearchOption searchOption) =>
            GetDirectories(path, "*", searchOption);

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in
        ///     a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">
        ///     The relative or absolute path to the directory to search. This string is
        ///     not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of files in path. This parameter
        ///     can contain a combination of valid literal path and wildcard (* and ?)
        ///     characters, but doesn't support regular expressions.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation
        ///     should include only the current directory or should include all  subdirectories.
        /// </param>
        public static IEnumerable<string> EnumerateFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var files = default(IEnumerable<string>);
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var dir = PathEx.Combine(path);
                if (!Directory.Exists(dir))
                    throw new PathNotFoundException(dir);
                files = Directory.EnumerateFiles(path, searchPattern, searchOption);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return files;
        }

        /// <summary>
        ///     Returns an enumerable collection of file names that match a search pattern in
        ///     a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">
        ///     The relative or absolute path to the directory to search. This string is
        ///     not case-sensitive.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation
        ///     should include only the current directory or should include all  subdirectories.
        /// </param>
        public static IEnumerable<string> EnumerateFiles(string path, SearchOption searchOption) =>
            EnumerateFiles(path, "*", searchOption);

        /// <summary>
        ///     Returns the names of files (including their paths) that match the specified search
        ///     pattern in the specified directory, using a value to determine whether to search
        ///     subdirectories.
        /// </summary>
        /// <param name="path">
        ///     The relative or absolute path to the directory to search. This string is
        ///     not case-sensitive.
        /// </param>
        /// <param name="searchPattern">
        ///     The search string to match against the names of files in path. This parameter
        ///     can contain a combination of valid literal path and wildcard (* and ?)
        ///     characters, but doesn't support regular expressions.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation
        ///     should include only the current directory or should include all  subdirectories.
        /// </param>
        public static string[] GetFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var files = default(string[]);
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var dir = PathEx.Combine(path);
                if (!Directory.Exists(dir))
                    throw new PathNotFoundException(dir);
                files = Directory.GetFiles(path, searchPattern, searchOption);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return files;
        }

        /// <summary>
        ///     Returns the names of files (including their paths) that match the specified search
        ///     pattern in the specified directory, using a value to determine whether to search
        ///     subdirectories.
        /// </summary>
        /// <param name="path">
        ///     The relative or absolute path to the directory to search. This string is
        ///     not case-sensitive.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the search operation
        ///     should include only the current directory or should include all  subdirectories.
        /// </param>
        public static string[] GetFiles(string path, SearchOption searchOption) =>
            GetFiles(path, "*", searchOption);

        /// <summary>
        ///     Creates all directories and subdirectories in the specified path
        ///     unless they already exist.
        /// </summary>
        /// <param name="path">
        ///     The directory to create.
        /// </param>
        public static bool Create(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var dir = PathEx.Combine(path);
                if (Directory.Exists(dir))
                    return true;
                if (File.Exists(dir) && !PathEx.IsDir(dir))
                    File.Delete(dir);
                var di = Directory.CreateDirectory(dir);
                return di.Exists;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Copies an existing directory to a new location.
        /// </summary>
        /// <param name="srcDir">
        ///     The directory to copy.
        /// </param>
        /// <param name="destDir">
        ///     The fully qualified name of the destination directory.
        /// </param>
        /// <param name="subDirs">
        ///     true to include subdirectories; otherwise, false.
        /// </param>
        /// <param name="overwrite">
        ///     true to allow existing files to be overwritten; otherwise, false.
        /// </param>
        public static bool Copy(string srcDir, string destDir, bool subDirs = true, bool overwrite = false)
        {
            try
            {
                if (string.IsNullOrEmpty(srcDir))
                    throw new ArgumentNullException(nameof(srcDir));
                var src = PathEx.Combine(srcDir);
                var di = new DirectoryInfo(src);
                if (!di.Exists)
                    throw new PathNotFoundException(di.FullName);
                if (string.IsNullOrEmpty(destDir))
                    throw new ArgumentNullException(nameof(destDir));
                var dest = PathEx.Combine(destDir);
                if (!Directory.Exists(dest))
                    Directory.CreateDirectory(dest);
                foreach (var f in di.EnumerateFiles())
                {
                    var p = Path.Combine(dest, f.Name);
                    if (File.Exists(p) && !overwrite)
                        continue;
                    f.CopyTo(p, overwrite);
                }
                if (!subDirs)
                    return true;
                if (di.EnumerateDirectories().Any(x => !x.Copy(Path.Combine(dest, x.Name), true, overwrite)))
                    throw new OperationCanceledException();
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Copies an existing directory to a new location.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory instance member that contains the directory to copy.
        /// </param>
        /// <param name="destDir">
        ///     The fully qualified name of the destination directory.
        /// </param>
        /// <param name="subDirs">
        ///     true to include subdirectories; otherwise, false.
        /// </param>
        /// <param name="overwrite">
        ///     true to allow existing files to be overwritten; otherwise, false.
        /// </param>
        public static bool Copy(this DirectoryInfo dirInfo, string destDir, bool subDirs = true, bool overwrite = false) =>
            Copy(dirInfo?.FullName, destDir, subDirs, overwrite);

        /// <summary>
        ///     Copies an existing directory to a new location and deletes the source
        ///     directory if this task has been completed successfully.
        /// </summary>
        /// <param name="srcDir">
        ///     The directory to move.
        /// </param>
        /// <param name="destDir">
        ///     The fully qualified name of the destination directory.
        /// </param>
        /// <param name="overwrite">
        ///     true to allow existing files to be overwritten; otherwise, false.
        /// </param>
        public static bool Move(string srcDir, string destDir, bool overwrite = false)
        {
            if (!Copy(srcDir, destDir, overwrite))
                return false;
            var src = PathEx.Combine(srcDir);
            var dest = PathEx.Combine(destDir);
            try
            {
                if (!overwrite || GetFullHashCode(src) != GetFullHashCode(dest))
                    throw new AggregateException();
                if (!IsLink(src))
                    SetAttributes(src, FileAttributes.Directory | FileAttributes.Normal);
                Directory.Delete(src, true);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Deletes the specified directory, if it exists.
        /// </summary>
        /// <param name="path">
        ///     The path of the directory to be deleted.
        /// </param>
        /// <exception cref="IOException">
        ///     See <see cref="Directory.Delete(string, bool)"/>.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     See <see cref="Directory.Delete(string, bool)"/>.
        /// </exception>
        public static bool Delete(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;
            var dir = PathEx.Combine(path);
            if (!Directory.Exists(dir))
                return true;
            if (!IsLink(dir))
                SetAttributes(dir, FileAttributes.Directory | FileAttributes.Normal);
            Directory.Delete(dir, true);
            return true;
        }

        /// <summary>
        ///     Tries to delete the specified directory.
        /// </summary>
        /// <param name="path">
        ///     The path of the directory to be deleted.
        /// </param>
        public static bool TryDelete(string path)
        {
            try
            {
                return Delete(path);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                return false;
            }
        }

        /// <summary>
        ///     Returns the hash code for the specified directory instance member.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory instance member to get the hash code.
        /// </param>
        /// <param name="size">
        ///     true to include the size of each file; otherwise, false.
        /// </param>
        public static int GetFullHashCode(this DirectoryInfo dirInfo, bool size = true)
        {
            try
            {
                if (dirInfo == null)
                    throw new ArgumentNullException(nameof(dirInfo));
                var sb = new StringBuilder();
                var len = 0L;
                foreach (var fi in dirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                {
                    sb.Append(fi.Name);
                    if (size)
                        len += fi.Length;
                }
                var s = size ? len + sb.ToString() : sb.ToString();
                return s.GetHashCode();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return -1;
            }
        }

        /// <summary>
        ///     Returns the hash code for the specified directory instance member.
        /// </summary>
        /// <param name="path">
        ///     The directory to get the hash code.
        /// </param>
        /// <param name="size">
        ///     true to include the size of each file; otherwise, false.
        /// </param>
        public static int GetFullHashCode(string path, bool size = true)
        {
            try
            {
                var dir = PathEx.Combine(path);
                if (!Directory.Exists(dir))
                    return -1;
                var di = new DirectoryInfo(dir);
                return di.GetFullHashCode(size);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return -1;
            }
        }

        /// <summary>
        ///     Returns the total amount of free space available on the drive of the specified
        ///     directory, in bytes.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory instance member to check.
        /// </param>
        public static long GetFreeSpace(this DirectoryInfo dirInfo)
        {
            try
            {
                if (dirInfo == null)
                    throw new ArgumentNullException(nameof(dirInfo));
                var root = Path.GetPathRoot(dirInfo.FullName).ToUpperInvariant();
                var drive = DriveInfo.GetDrives().FirstOrDefault(x => root.Equals(x.Name, StringComparison.Ordinal));
                if (drive == default(DriveInfo))
                    throw new ArgumentNullException(nameof(drive));
                return drive.TotalFreeSpace;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                return 0L;
            }
        }

        /// <summary>
        ///     Returns the total amount of free space available on the drive of the specified
        ///     directory, in bytes.
        /// </summary>
        /// <param name="path">
        ///     The directory to check.
        /// </param>
        public static long GetFreeSpace(string path)
        {
            var dir = PathEx.Combine(path);
            if (!Directory.Exists(dir))
                return 0L;
            var di = new DirectoryInfo(dir);
            return di.GetFreeSpace();
        }

        /// <summary>
        ///     Returns the size, in bytes, of the specified directory instance member.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory instance member to get the size.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the operation should include
        ///     only the current directory or all subdirectories.
        /// </param>
        public static long GetSize(this DirectoryInfo dirInfo, SearchOption searchOption = SearchOption.AllDirectories)
        {
            try
            {
                var len = 0L;
                if (dirInfo == null)
                    return len;
                var files = dirInfo.GetFiles();
                if (files.Any())
                    Parallel.ForEach(files, fi => Interlocked.Add(ref len, fi.Length));
                if (searchOption == SearchOption.TopDirectoryOnly)
                    return len;
                var dirs = dirInfo.GetDirectories();
                if (dirs.Any())
                    Parallel.ForEach(dirs, di => Interlocked.Add(ref len, di.GetSize()));
                return len;
            }
            catch (OverflowException ex) when (ex.IsCaught())
            {
                return long.MaxValue;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                return 0L;
            }
        }

        /// <summary>
        ///     Returns the size, in bytes, of the specified directory.
        /// </summary>
        /// <param name="path">
        ///     The directory to get the size.
        /// </param>
        /// <param name="searchOption">
        ///     One of the enumeration values that specifies whether the operation should include
        ///     only the current directory or all subdirectories.
        /// </param>
        public static long GetSize(string path, SearchOption searchOption = SearchOption.AllDirectories)
        {
            var dir = PathEx.Combine(path);
            if (!Directory.Exists(dir))
                return 0L;
            var di = new DirectoryInfo(dir);
            return di.GetSize(searchOption);
        }

        /// <summary>
        ///     Returns a unique name starting with a given prefix, followed by a hash of the specified
        ///     length.
        /// </summary>
        /// <param name="prefix">
        ///     This text is at the beginning of the name.
        ///     <para>
        ///         Uppercase letters are converted to lowercase letters. Supported characters are only
        ///         from '0' to '9' and from 'a' to 'z' but can be completely empty to omit the prefix.
        ///     </para>
        /// </param>
        /// <param name="hashLen">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     hashLen is not between 4 and 24.
        /// </exception>
        /// <exception cref="ArgumentInvalidException">
        ///     prefix contains invalid characters.
        /// </exception>
        public static string GetUniqueName(string prefix = "tmp", int hashLen = 4) =>
            PathEx.GetUniqueName(prefix, null, hashLen);

        /// <summary>
        ///     Returns a unique name starting with 'tmp' prefix, followed by a hash of the specified
        ///     length.
        /// </summary>
        /// <param name="hashLen">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     hashLen is not between 4 and 24.
        /// </exception>
        public static string GetUniqueName(int hashLen) =>
            PathEx.GetUniqueName("tmp", null, hashLen);

        /// <summary>
        ///     Returns the current user's temporary path in combination with unique name starting with
        ///     a given prefix, followed by a hash of the specified length.
        /// </summary>
        /// <param name="prefix">
        ///     This text is at the beginning of the name.
        ///     <para>
        ///         Uppercase letters are converted to lowercase letters. Supported characters are only
        ///         from '0' to '9' and from 'a' to 'z' but can be completely empty to omit the prefix.
        ///     </para>
        /// </param>
        /// <param name="hashLen">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     hashLen is not between 4 and 24.
        /// </exception>
        /// <exception cref="ArgumentInvalidException">
        ///     prefix contains invalid characters.
        /// </exception>
        public static string GetUniqueTempPath(string prefix = "tmp", int hashLen = 4) =>
            PathEx.GetUniquePath("%TEMP%", prefix, null, hashLen);

        /// <summary>
        ///     Returns the current user's temporary path in combination with unique name starting with
        ///     'tmp' prefix, followed by a hash of the specified length.
        /// </summary>
        /// <param name="hashLen">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     hashLen is not between 4 and 24.
        /// </exception>
        public static string GetUniqueTempPath(int hashLen) =>
            PathEx.GetUniquePath("%TEMP%", "tmp", null, hashLen);

        /// <summary>
        ///     Creates a link to the specified directory.
        /// </summary>
        /// <param name="targetPath">
        ///     The directory to be linked.
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
        ///     true to skip existing shortcuts, even if the target path of
        ///     the same; otherwise, false.
        /// </param>
        public static bool CreateShellLink(string targetPath, string linkPath, string startArgs = null, (string, int) iconLocation = default, bool skipExists = false)
        {
            if (!PathEx.IsDir(targetPath))
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
        ///     Creates a link to the specified directory.
        /// </summary>
        /// <param name="targetPath">
        ///     The directory to be linked.
        /// </param>
        /// <param name="linkPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="startArgs">
        ///     The arguments which applies when the link is started.
        /// </param>
        /// <param name="skipExists">
        ///     true to skip existing shortcuts, even if the target path of
        ///     the same; otherwise, false.
        /// </param>
        public static bool CreateShellLink(string targetPath, string linkPath, string startArgs, bool skipExists) =>
            CreateShellLink(targetPath, linkPath, startArgs, (null, 0), skipExists);

        /// <summary>
        ///     Creates a link to the specified directory.
        /// </summary>
        /// <param name="targetPath">
        ///     The directory to be linked.
        /// </param>
        /// <param name="linkPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="skipExists">
        ///     true to skip existing shortcuts, even if the target path of
        ///     the same; otherwise, false.
        /// </param>
        public static bool CreateShellLink(string targetPath, string linkPath, bool skipExists) =>
            CreateShellLink(targetPath, linkPath, null, (null, 0), skipExists);

        /// <summary>
        ///     Removes a link of the specified directory.
        /// </summary>
        /// <param name="path">
        ///     The shortcut to be removed.
        /// </param>
        public static bool DestroyShellLink(string path) =>
            ShellLink.Destroy(path);

        /// <summary>
        ///     Returns the target path of the specified link if the target is a directory.
        /// </summary>
        /// <param name="path">
        ///     The link to get the target path.
        /// </param>
        public static string GetShellLinkTarget(string path)
        {
            var target = ShellLink.GetTarget(path);
            return PathEx.IsDir(target) ? target : string.Empty;
        }

        /// <summary>
        ///     Creates a symbolic link to the specified directory based on command prompt which
        ///     allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="linkPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="srcDir">
        ///     The directory to be linked.
        /// </param>
        /// <param name="backup">
        ///     true to create an backup for existing files; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to create this link with highest privileges; otherwise, false.
        /// </param>
        public static bool CreateSymbolicLink(string linkPath, string srcDir, bool backup = false, bool elevated = false) =>
            SymbolicLink.Create(linkPath, srcDir, true, backup, elevated);

        /// <summary>
        ///     Removes an symbolic link of the specified directory link based on command prompt
        ///     which allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="path">
        ///     The link to be removed.
        /// </param>
        /// <param name="backup">
        ///     true to restore found backups; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to remove this link with highest privileges; otherwise, false.
        /// </param>
        public static bool DestroySymbolicLink(string path, bool backup = false, bool elevated = false) =>
            SymbolicLink.Destroy(path, true, backup, elevated);

        /// <summary>
        ///     Find out which processes have locked the specified directories.
        /// </summary>
        /// <param name="dirs">
        ///     The directories to check.
        /// </param>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static IEnumerable<Process> GetLocks(IEnumerable<string> dirs)
        {
            var paths = dirs?.ToArray();
            return paths?.Any() == true ? FileEx.GetLocks(paths.Select(PathEx.Combine).Where(PathEx.IsDir).SelectMany(s => GetFiles(s, SearchOption.AllDirectories))) : null;
        }

        /// <summary>
        ///     Returns processes that have locked files of this directory instance member.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory instance member to check.
        /// </param>
        public static IEnumerable<Process> GetLocks(this DirectoryInfo dirInfo)
        {
            var locks = default(IEnumerable<Process>);
            try
            {
                if (dirInfo == null)
                    throw new ArgumentNullException(nameof(dirInfo));
                var files = Directory.EnumerateFiles(dirInfo.FullName, "*", SearchOption.AllDirectories);
                locks = PathEx.GetLocks(files);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return locks;
        }
    }
}
