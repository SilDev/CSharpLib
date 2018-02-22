#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: FileEx.cs
// Version:  2018-02-22 03:14
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
    using System.IO;
    using System.Linq;

    /// <summary>
    ///     Provides static methods based on the <see cref="File"/> class to perform file
    ///     operations.
    /// </summary>
    public static class FileEx
    {
        /// <summary>
        ///     Determines whether the specified path specifies the specified file attributes.
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
                var fa = fileInfo.Attributes;
                return (fa & attr) != 0;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified path specifies the specified file attributes.
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
            catch (Exception ex)
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
            fileInfo?.MatchAttributes(FileAttributes.Hidden) == true;

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
            fileInfo?.MatchAttributes(FileAttributes.ReparsePoint) == true;

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
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Replaces all occurrences of a specifed sequence of bytes in the specified file
        ///     with another sequence of bytes.
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
        ///     true to create a backup; otherwise, false.
        /// </param>
        public static bool BinaryReplace(string file, byte[] oldValue, byte[] newValue, bool backup = true)
        {
            var targetPath = PathEx.Combine(file);
            try
            {
                if (!File.Exists(targetPath))
                    throw new PathNotFoundException(targetPath);
            }
            catch (Exception ex)
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
            catch (Exception ex)
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
                    if (oldValue[index] == position)
                        if (index == oldValue.Length - 1)
                        {
                            targetStream.Write(newValue, 0, newValue.Length);
                            offset = -1;
                            index = 0;
                        }
                        else
                        {
                            if (index == 0)
                                offset = sourceStream.Position - 1;
                            ++index;
                        }
                    else
                    {
                        if (index == 0)
                            targetStream.WriteByte((byte)position);
                        else
                        {
                            targetStream.WriteByte(oldValue[0]);
                            sourceStream.Position = offset + 1;
                            offset = -1;
                            index = 0;
                        }
                    }
                result = true;
            }
            catch (Exception ex)
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
                catch (Exception exc)
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
                        var backupHash = new Crypto.Md5().EncryptFile(backupPath);
                        var targetHash = new Crypto.Md5().EncryptFile(targetPath);
                        if (backupHash.Equals(targetHash))
                            File.Delete(backupPath);
                    }
                }
                catch (Exception ex)
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
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return true;
        }

        /// <summary>
        ///     Creates an empty file in the specified path.
        /// </summary>
        /// <param name="path">
        ///     The path and name of the file to create.
        /// </param>
        /// <param name="overwrite">
        ///     true to overwrite an existing file; otherwise, false.
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
                    throw new ArgumentNullException(nameof(dir));
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.Create(file).Close();
                if (!File.Exists(file))
                    throw new PathNotFoundException(file);
                return true;
            }
            catch (Exception ex)
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
        ///     true to allow an existing file to be overwritten; otherwise, false.
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
                    if (!fi.Directory?.Exists == false)
                        fi.Directory.Create();
                }
                fi.CopyTo(dest);
                return true;
            }
            catch (Exception ex)
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
        ///     true to allow an existing file to be overwritten; otherwise, false.
        /// </param>
        public static bool Move(string srcFile, string destFile, bool overwrite = false)
        {
            if (!Copy(srcFile, destFile, overwrite))
                return false;
            var src = PathEx.Combine(srcFile);
            var dest = PathEx.Combine(destFile);
            try
            {
                if (!overwrite || new FileInfo(src).GetHashCode() != new FileInfo(dest).GetHashCode())
                    throw new AggregateException();
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
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Creates a link to the specified path.
        /// </summary>
        /// <param name="targetPath">
        ///     The file to be linked.
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
        public static bool CreateShortcut(string targetPath, string linkPath, string startArgs = null, string linkIcon = null, int linkIconId = 0, bool skipExists = false) =>
            !PathEx.IsDir(targetPath) && PathEx.CreateShortcut(targetPath, linkPath, startArgs, linkIcon, linkIconId, skipExists);

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
        ///     Removes a link of the specified file.
        /// </summary>
        /// <param name="path">
        ///     The shortcut to be removed.
        /// </param>
        public static bool DestroyShortcut(string path) =>
            PathEx.DestroyShortcut(path);

        /// <summary>
        ///     Returns the target path of the specified link.
        ///     <para>
        ///         The target path is returned only if the specified target is an existing
        ///         file.
        ///     </para>
        /// </summary>
        /// <param name="path">
        ///     The shortcut path to get the target path.
        /// </param>
        public static string GetShortcutTarget(string path)
        {
            var target = PathEx.GetShortcutTarget(path);
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
        ///     true to create an backup for existing files; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to create this link with highest privileges; otherwise, false.
        /// </param>
        public static bool CreateSymbolicLink(string linkPath, string destFile, bool backup = false, bool elevated = false) =>
            PathEx.CreateSymbolicLink(linkPath, destFile, false, backup, elevated);

        /// <summary>
        ///     Removes an symbolic link of the specified file link based on command prompt
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
            PathEx.DestroySymbolicLink(path, false, backup, elevated);

        /// <summary>
        ///     Find out which processes have a lock on this file instance member.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member to check.
        /// </param>
        public static List<Process> GetLocks(this FileInfo fileInfo) =>
            PathEx.GetLocks(fileInfo?.FullName);

        /// <summary>
        ///     Returns the highest version information associated with this file instance
        ///     member.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member to check.
        /// </param>
        public static Version GetVersion(this FileInfo fileInfo) =>
            GetVersion(fileInfo.FullName);

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
            catch (Exception ex)
            {
                Log.Write(ex);
                v = Version.Parse("0.0.0.0");
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
            GetFileVersion(fileInfo.FullName);

        /// <summary>
        ///     Returns the file version information associated with the specified file.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static Version GetFileVersion(string path)
        {
            Version v;
            try
            {
                var s = PathEx.Combine(path);
                if (!File.Exists(s))
                    throw new PathNotFoundException(s);
                var fvi = FileVersionInfo.GetVersionInfo(s);
                v = Version.Parse(fvi.FileVersion.VersionFilter());
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                v = Version.Parse("0.0.0.0");
            }
            return v;
        }

        /// <summary>
        ///     Returns the product version information associated with this file instance
        ///     member.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member to check.
        /// </param>
        public static Version GetProductVersion(this FileInfo fileInfo) =>
            GetProductVersion(fileInfo.FullName);

        /// <summary>
        ///     Returns the product version information associated with the specified file.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static Version GetProductVersion(string path)
        {
            Version v;
            try
            {
                var s = PathEx.Combine(path);
                if (!File.Exists(s))
                    throw new PathNotFoundException(s);
                var fvi = FileVersionInfo.GetVersionInfo(s);
                v = Version.Parse(fvi.ProductVersion.VersionFilter());
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                v = Version.Parse("0.0.0.0");
            }
            return v;
        }

        private static string VersionFilter(this string str)
        {
            var s = str;
            if (!s.Any(x => char.IsDigit(x) || x == '.') && s.Count(x => x == '.') > 0)
                return s;
            var sa = s.Split('.').ToList();
            for (var i = 0; i < sa.Count; i++)
                sa[i] = new string(sa[i].Where(char.IsDigit).ToArray());
            while (sa.Count < 4)
                sa.Add("0");
            if (sa.Count > 4)
                sa = sa.Take(4).ToList();
            s = sa.Join(".");
            return s;
        }
    }
}
