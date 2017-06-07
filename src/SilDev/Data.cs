#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Data.cs
// Version:  2017-06-07 22:12
// 
// Copyright (c) 2017, Si13n7 Developments (r)
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
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;
    using Properties;

    /// <summary>
    ///     Provides static methods for the creation, copying, linking of data and to handle file
    ///     information.
    /// </summary>
    public static class Data
    {
        /// <summary>
        ///     <para>
        ///         Gets the original name of the current principal.
        ///     </para>
        ///     <para>
        ///         This variable is only set if <see cref="ChangePrincipalName(string)"/>
        ///         was previously called.
        ///     </para>
        /// </summary>
        public static string PrincipalName { get; private set; }

        /// <summary>
        ///     Reads the bytes from the specified stream and writes them to another stream.
        /// </summary>
        /// <param name="src">
        ///     The <see cref="Stream"/> to copy.
        /// </param>
        /// <param name="dest">
        ///     The <see cref="Stream"/> to override.
        /// </param>
        /// <param name="buffer">
        ///     The maximum number of bytes to buffer.
        /// </param>
        public static void CopyTo(this Stream src, Stream dest, int buffer = 4096)
        {
            try
            {
                var ba = new byte[buffer];
                int i;
                while ((i = src.Read(ba, 0, ba.Length)) > 0)
                    dest.Write(ba, 0, i);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
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
        /// <param name="linkIcon">
        ///     The icon resource path for this shortcut.
        /// </param>
        /// <param name="skipExists">
        ///     true to skip existing shortcuts, even if the target path of
        ///     the same; otherwise, false.
        /// </param>
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static bool CreateShortcut(string targetPath, string linkPath, string startArgs = null, string linkIcon = null, bool skipExists = false)
        {
            try
            {
                var link = PathEx.Combine(!linkPath.EndsWithEx(".lnk") ? $"{linkPath}.lnk" : linkPath);
                var name = Path.GetDirectoryName(link);
                var path = PathEx.Combine(targetPath);
                if (!Directory.Exists(name) || !PathEx.DirOrFileExists(path))
                    return false;
                if (File.Exists(link))
                {
                    if (skipExists)
                        return true;
                    File.Delete(link);
                }
                name = Path.GetDirectoryName(targetPath);
                var shell = (IShellLink)new ShellLink();
                if (!string.IsNullOrWhiteSpace(startArgs))
                    shell.SetArguments(startArgs);
                shell.SetDescription(string.Empty);
                shell.SetPath(targetPath);
                shell.SetIconLocation(linkIcon ?? targetPath, 0);
                shell.SetWorkingDirectory(name);
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
            CreateShortcut(targetPath, linkPath, startArgs, null, skipExists);

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
            CreateShortcut(targetPath, linkPath, null, null, skipExists);

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
                using (var fs = File.Open(PathEx.Combine(path), FileMode.Open, FileAccess.Read))
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

        private static void GetPrincipalPointers(out IntPtr offset, out IntPtr buffer)
        {
            var curHandle = Process.GetCurrentProcess().Handle;
            var pebBaseAddress = WinApi.GetProcessBasicInformation(curHandle).PebBaseAddress;
            var processParameters = Marshal.ReadIntPtr(pebBaseAddress, 4 * IntPtr.Size);
            var unicodeSize = IntPtr.Size * 2;
            offset = processParameters.Increment((IntPtr)(4 * 4 + 5 * IntPtr.Size + unicodeSize + IntPtr.Size + unicodeSize));
            buffer = Marshal.ReadIntPtr(offset, IntPtr.Size);
        }

        /// <summary>
        ///     Changes the name of the current principal.
        /// </summary>
        /// <param name="newName">
        ///     The new name for the current principal, which
        ///     cannot be longer than the original one.
        /// </param>
        public static void ChangePrincipalName(string newName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newName))
                    throw new ArgumentNullException(nameof(newName));
                IntPtr offset, buffer;
                GetPrincipalPointers(out offset, out buffer);
                var len = Marshal.ReadInt16(offset);
                if (string.IsNullOrEmpty(PrincipalName))
                    PrincipalName = Marshal.PtrToStringUni(buffer, len / 2);
                var principalDir = Path.GetDirectoryName(PrincipalName);
                if (string.IsNullOrEmpty(principalDir))
                    throw new PathNotFoundException(principalDir);
                var newPrincipalName = Path.Combine(principalDir, newName);
                if (newPrincipalName.Length > PrincipalName.Length)
                    throw new ArgumentException("The new principal name cannot be longer than the original one.");
                var ptr = buffer;
                foreach (var c in newPrincipalName)
                {
                    Marshal.WriteInt16(ptr, c);
                    ptr = ptr.Increment((IntPtr)2);
                }
                Marshal.WriteInt16(ptr, 0);
                Marshal.WriteInt16(offset, (short)(newPrincipalName.Length * 2));
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Restores the name of the current principal.
        /// </summary>
        public static void RestorePrincipalName()
        {
            try
            {
                if (string.IsNullOrEmpty(PrincipalName))
                    throw new InvalidOperationException();
                IntPtr offset, buffer;
                GetPrincipalPointers(out offset, out buffer);
                foreach (var c in PrincipalName)
                {
                    Marshal.WriteInt16(buffer, c);
                    buffer = buffer.Increment((IntPtr)2);
                }
                Marshal.WriteInt16(buffer, 0);
                Marshal.WriteInt16(offset, (short)(PrincipalName.Length * 2));
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private static bool PinUnpinTaskbar(string path, bool pin)
        {
            try
            {
                if (!File.Exists(PathEx.Combine(path)))
                    throw new PathNotFoundException(path);
                if (Environment.OSVersion.Version.Major >= 10)
                    ChangePrincipalName("explorer.exe");
                var sb = new StringBuilder(255);
                var hDll = WinApi.UnsafeNativeMethods.LoadLibrary("shell32.dll");
                WinApi.UnsafeNativeMethods.LoadString(hDll, (uint)(pin ? 0x150a : 0x150b), sb, 0xff);
                dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));
                dynamic dir = shell.NameSpace(Path.GetDirectoryName(path));
                dynamic link = dir.ParseName(Path.GetFileName(path));
                var verb = sb.ToString();
                dynamic verbs = link.Verbs();
                for (var i = 0; i < verbs.Count(); i++)
                {
                    dynamic d = verbs.Item(i);
                    if ((!pin || !d.Name.Equals(verb)) && (pin || !d.Name.Contains(verb)))
                        continue;
                    d.DoIt();
                    break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
            finally
            {
                if (File.Exists(path) && Environment.OSVersion.Version.Major >= 10)
                    RestorePrincipalName();
            }
        }

        /// <summary>
        ///     Pin the specified file to taskbar.
        /// </summary>
        /// <param name="path">
        ///     The file to be pinned.
        /// </param>
        public static bool PinToTaskbar(string path) =>
            PinUnpinTaskbar(path, true);

        /// <summary>
        ///     Unpin the specified file to taskbar.
        /// </summary>
        /// <param name="path">
        ///     The file to be unpinned.
        /// </param>
        public static bool UnpinFromTaskbar(string path) =>
            PinUnpinTaskbar(path, false);

        /// <summary>
        ///     Determines whether the specified path specifies the specified
        ///     file attributes.
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
        ///     Determines whether the specified path specifies the specified
        ///     file attributes.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory to check.
        /// </param>
        /// <param name="attr">
        ///     The attributes to match.
        /// </param>
        public static bool MatchAttributes(this DirectoryInfo dirInfo, FileAttributes attr)
        {
            try
            {
                var da = dirInfo.Attributes;
                return (da & attr) != 0;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified path specifies the specified
        ///     file attributes.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file to check.
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
        ///     Determines whether the specified path is specified as reparse
        ///     point.
        /// </summary>
        /// <param name="path">
        ///     The file or directory to check.
        /// </param>
        public static bool DirOrFileIsLink(this string path) =>
            MatchAttributes(path, FileAttributes.ReparsePoint);

        /// <summary>
        ///     Determines whether the specified directory is specified as
        ///     reparse point.
        /// </summary>
        /// <param name="path">
        ///     The directory to check.
        /// </param>
        public static bool DirIsLink(string path) =>
            path.DirOrFileIsLink();

        /// <summary>
        ///     Determines whether the specified file is specified as reparse
        ///     point.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static bool FileIsLink(string path) =>
            path.DirOrFileIsLink();

        /// <summary>
        ///     Determines whether the specified path is specified as directory.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static bool IsDir(string path) =>
            MatchAttributes(path, FileAttributes.Directory);

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
            try
            {
                var src = PathEx.Combine(path);
                if (IsDir(src))
                {
                    var di = new DirectoryInfo(src);
                    if (!di.Exists)
                        return;
                    if (attr != FileAttributes.Normal)
                        di.Attributes |= attr;
                    else
                        di.Attributes = attr;
                }
                else
                {
                    var fi = new FileInfo(src);
                    if (attr != FileAttributes.Normal)
                        fi.Attributes |= attr;
                    else
                        fi.Attributes = attr;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private static bool Linker(string linkPath, string destPath, bool destIsDir, bool backup = false, bool elevated = false)
        {
            /*
             * The idea was to replace the code below with this code that uses the
             * p/invoke method to create symbolic links. But this doesn't work
             * without administrator privileges...

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
                        File.Move(link, link + ".SI13N7-BACKUP");
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

            var dest = PathEx.Combine(destPath);
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

            var link = PathEx.Combine(linkPath);
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
                if (PathEx.DirOrFileExists(link))
                    if (!DirIsLink(link))
                        cmd += $"MOVE /Y \"{link}\" \"{link}.SI13N7-BACKUP\"";
                    else
                        UnLinker(link, true, true, elevated);

            if (PathEx.DirOrFileExists(link))
            {
                if (!string.IsNullOrEmpty(cmd))
                    cmd += " & ";
                cmd += $"{(destIsDir ? "RD /S /Q" : "DEL / F / Q")} \"{link}\"";
            }

            if (PathEx.DirOrFileExists(dest))
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
                if (!p?.HasExited == true)
                    p?.WaitForExit();
                exitCode = p?.ExitCode;
            }
            return exitCode == 0 && DirOrFileIsLink(link);
        }

        /// <summary>
        ///     Creates a symbolic link to the specified directory based on command prompt
        ///     which allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="linkPath">
        ///     The directory to be linked.
        /// </param>
        /// <param name="destDir">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="backup">
        ///     true to create an backup for existing directories; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to create this link with highest privileges; otherwise, false.
        /// </param>
        public static bool DirLink(string linkPath, string destDir, bool backup = false, bool elevated = false) =>
            Linker(linkPath, destDir, true, backup, elevated);

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
        public static bool FileLink(string linkPath, string destFile, bool backup = false, bool elevated = false) =>
            Linker(linkPath, destFile, false, backup, elevated);

        private static bool UnLinker(string path, bool pathIsDir, bool backup = false, bool elevated = false)
        {
            var link = PathEx.Combine(path);
            var isLink = link.DirOrFileIsLink();
            var cmd = $"{(pathIsDir ? "RD /Q" : "DEL /F /Q")}{(!pathIsDir && isLink ? " /A:L" : string.Empty)} \"{link}\"";
            if (backup && PathEx.DirOrFileExists($"{link}.SI13N7-BACKUP"))
                cmd += $" & MOVE /Y \"{link}.SI13N7-BACKUP\" \"{link}\"";
            if (string.IsNullOrEmpty(cmd))
                return false;
            int? exitCode;
            using (var p = ProcessEx.Send(cmd, elevated, false))
            {
                if (!p?.HasExited == true)
                    p?.WaitForExit();
                exitCode = p?.ExitCode;
            }
            return exitCode == 0 && isLink;
        }

        /// <summary>
        ///     Removes an symbolic link of the specified directory link based on command
        ///     prompt which allows a simple solution for the elevated execution of this
        ///     order.
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
        public static bool DirUnLink(string path, bool backup = false, bool elevated = false) =>
            UnLinker(path, true, backup, elevated);

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
        public static bool FileUnLink(string path, bool backup = false, bool elevated = false) =>
            UnLinker(path, false, backup, elevated);

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
        ///     true to inlcude subdirectories; otherwise, false.
        /// </param>
        /// <param name="overwrite">
        ///     true to allow an existing file to be overwritten; otherwise, false.
        /// </param>
        public static bool DirCopy(string srcDir, string destDir, bool subDirs = true, bool overwrite = false)
        {
            try
            {
                var src = PathEx.Combine(srcDir);
                var di = new DirectoryInfo(src);
                if (!di.Exists)
                    throw new PathNotFoundException(di.FullName);
                var dest = PathEx.Combine(destDir);
                if (!Directory.Exists(dest))
                    Directory.CreateDirectory(dest);
                foreach (var f in di.GetFiles())
                    f.CopyTo(Path.Combine(dest, f.Name), overwrite);
                if (!subDirs)
                    return true;
                foreach (var d in di.GetDirectories("*", SearchOption.TopDirectoryOnly))
                    DirCopy(d.FullName, Path.Combine(dest, d.Name), true, overwrite);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

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
        public static void DirSafeMove(string srcDir, string destDir)
        {
            try
            {
                if (!DirCopy(srcDir, destDir))
                    return;
                var src = PathEx.Combine(srcDir);
                var dest = PathEx.Combine(destDir);
                if (new DirectoryInfo(src).GetFullHashCode() != new DirectoryInfo(dest).GetFullHashCode())
                    throw new AggregateException();
                Directory.Delete(src, true);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
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
                var sb = new StringBuilder();
                long len = 0;
                foreach (var fi in dirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                {
                    sb.Append(fi.Name);
                    if (size)
                        len += fi.Length;
                }
                return size ? $"{len}{sb}".GetHashCode() : sb.ToString().GetHashCode();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return $"{new Random().Next(int.MinValue, int.MaxValue)}".GetHashCode();
            }
        }

        /// <summary>
        ///     Gets the full size, in bytes, of the specified directory instance member.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory instance to get the size.
        /// </param>
        public static long GetSize(this DirectoryInfo dirInfo)
        {
            try
            {
                var size = dirInfo.GetFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
                return size;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        ///     Find out which processes have a lock on this file instance member.
        /// </summary>
        /// <param name="paths">
        ///     An array that contains the file paths to check.
        /// </param>
        public static List<Process> GetLocks(IList<string> paths)
        {
            var list = new List<Process>();
            try
            {
                uint handle;
                var res = WinApi.SafeNativeMethods.RmStartSession(out handle, 0, Guid.NewGuid().ToString());
                if (res != 0)
                    throw new Exception("Could not begin restart session. Unable to determine file locker.");
                try
                {
                    if (paths == null || !paths.Any())
                        throw new ArgumentNullException(nameof(paths));
                    uint pnProcInfoNeeded;
                    uint pnProcInfo = 0;
                    uint lpdwRebootReasons = 0;
                    var resources = paths.Select(s => PathEx.Combine(s)).Where(File.Exists).ToArray();
                    if (resources.Length == 0)
                        throw new PathNotFoundException(paths.Join("'; '"));
                    res = WinApi.SafeNativeMethods.RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);
                    if (res != 0)
                        throw new Exception("Could not register resource.");
                    res = WinApi.SafeNativeMethods.RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);
                    if (res == 0xea)
                    {
                        var processInfo = new WinApi.RM_PROCESS_INFO[pnProcInfoNeeded];
                        pnProcInfo = pnProcInfoNeeded;
                        res = WinApi.SafeNativeMethods.RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
                        if (res == 0)
                        {
                            var ids = processInfo.Select(e => e.Process.dwProcessId).Distinct().ToList();
                            foreach (var id in ids)
                                try
                                {
                                    list.Add(Process.GetProcessById(id));
                                }
                                catch (Exception ex)
                                {
                                    Log.Write(ex);
                                }
                        }
                        else
                            throw new Exception("Could not list processes locking resource.");
                    }
                    else if (res != 0)
                        throw new Exception("Could not list processes locking resource. Failed to get size of result.");
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
                finally
                {
                    WinApi.SafeNativeMethods.RmEndSession(handle);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return list;
        }

        /// <summary>
        ///     Find out which processes have a lock on this file instance member.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member to check.
        /// </param>
        public static List<Process> GetLocks(this FileInfo fileInfo) =>
            GetLocks(new[] { fileInfo.FullName });

        /// <summary>
        ///     Find out which processes have a lock on the files of this directory instance member.
        /// </summary>
        /// <param name="dirInfo">
        ///     The directory instance member to check.
        /// </param>
        public static List<Process> GetLocks(this DirectoryInfo dirInfo)
        {
            var list = new List<Process>();
            try
            {
                var sa = Directory.GetFiles(dirInfo.FullName, "*", SearchOption.AllDirectories);
                list = GetLocks(sa);
                if (list.Count > 0)
                    list = list.Distinct().ToList();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return list;
        }

        /// <summary>
        ///     Find out which processes have a lock on the specified path.
        /// </summary>
        /// <param name="path">
        ///     The full path to check.
        /// </param>
        public static List<Process> GetLocks(string path)
        {
            var list = new List<Process>();
            try
            {
                var s = PathEx.Combine(path);
                if (!PathEx.DirOrFileExists(s))
                    throw new PathNotFoundException(s);
                if (IsDir(s))
                {
                    var di = new DirectoryInfo(s);
                    list = di.GetLocks();
                }
                else
                {
                    var fi = new FileInfo(s);
                    list = fi.GetLocks();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return list;
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
        public static bool ForceDelete(string path, bool elevated = false)
        {
            var target = PathEx.Combine(path);
            try
            {
                if (!PathEx.DirOrFileExists(target))
                    throw new PathNotFoundException(target);
                var locked = false;
                using (var current = Process.GetCurrentProcess())
                {
                    var locks = GetLocks(target);
                    if (locks != null)
                        foreach (var p in locks)
                        {
                            if (p != current)
                                continue;
                            locked = true;
                            p.Dispose();
                            break;
                        }
                    locks = GetLocks(target)?.Where(p => p != current).ToList();
                    if (locks?.Any() == true)
                        ProcessEx.Terminate(locks);
                    if (!locked)
                        locks = GetLocks(target);
                    if (locks?.Any() == true)
                    {
                        locked = true;
                        foreach (var p in locks)
                            p?.Dispose();
                    }
                }
                var curName = $"{ProcessEx.CurrentName}.exe";
                if (IsDir(target))
                {
                    var tmpDir = PathEx.Combine(Path.GetTempPath(), PathEx.GetTempDirName());
                    if (!Directory.Exists(tmpDir))
                        Directory.CreateDirectory(tmpDir);
                    var helper = PathEx.Combine(Path.GetTempPath(), PathEx.GetTempFileName("tmp", ".cmd"));
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
                            if (!p?.HasExited == true)
                                p?.WaitForExit();
                    if (Directory.Exists(tmpDir))
                        Directory.Delete(tmpDir, true);
                    if (Directory.Exists(target))
                        Directory.Delete(target, true);
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
                                if (!p?.HasExited == true)
                                    p?.WaitForExit();
                    }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return !PathEx.DirOrFileExists(target);
        }

        /// <summary>
        ///     Returns the version information associated with this file instance member.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file instance member to check.
        /// </param>
        public static Version GetVersion(this FileInfo fileInfo)
        {
            Version v;
            try
            {
                var fvi = FileVersionInfo.GetVersionInfo(fileInfo.FullName);
                v = Version.Parse(fvi.ProductVersion);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                v = Version.Parse("0.0.0.0");
            }
            return v;
        }

        /// <summary>
        ///     Returns the version information associated with the specified file.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static Version GetVersion(string path)
        {
            Version v;
            try
            {
                var s = PathEx.Combine(path);
                if (!File.Exists(s))
                    throw new PathNotFoundException(s);
                var fvi = FileVersionInfo.GetVersionInfo(s);
                v = Version.Parse(fvi.ProductVersion);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                v = Version.Parse("0.0.0.0");
            }
            return v;
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
}
