#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Data.cs
// Version:  2016-10-18 23:33
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;

    /// <summary>
    ///     Provides static methods for the creation, copying and linking of data.
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
        ///     The file or directory to be linked
        ///     (environment variables are accepted).
        /// </param>
        /// <param name="targetPath">
        ///     The fully qualified name of the new link
        ///     (environment variables are accepted).
        /// </param>
        /// <param name="startArgs">
        ///     The arguments which applies when this shortcut is executed.
        /// </param>
        /// <param name="linkIcon">
        ///     The icon resource path for this shortcut
        ///     (environment variables are accepted).
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
                if (!Directory.Exists(Path.GetDirectoryName(link)) || !File.Exists(PathEx.Combine(targetPath)))
                    return false;
                if (File.Exists(link))
                {
                    if (skipExists)
                        return true;
                    File.Delete(link);
                }
                var shell = (IShellLink)new ShellLink();
                if (!string.IsNullOrWhiteSpace(startArgs))
                    shell.SetArguments(startArgs);
                shell.SetDescription(string.Empty);
                shell.SetPath(targetPath);
                shell.SetIconLocation(linkIcon ?? targetPath, 0);
                shell.SetWorkingDirectory(Path.GetDirectoryName(targetPath));
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
        ///     The file or directory to be linked
        ///     (environment variables are accepted).
        /// </param>
        /// <param name="targetPath">
        ///     The fully qualified name of the new link
        ///     (environment variables are accepted).
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
        ///     The file or directory to be linked
        ///     (environment variables are accepted).
        /// </param>
        /// <param name="targetPath">
        ///     The fully qualified name of the new link
        ///     (environment variables are accepted).
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
        ///     The shortcut path to get the target path
        ///     (environment variables are accepted).
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
                    var end = targetPath.IndexOf(string.Format("{0}{0}", Path.DirectorySeparatorChar), begin + 2, StringComparison.Ordinal) + 2;
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
                    throw new ArgumentNullException();
                IntPtr offset, buffer;
                GetPrincipalPointers(out offset, out buffer);
                var len = Marshal.ReadInt16(offset);
                if (string.IsNullOrEmpty(PrincipalName))
                    PrincipalName = Marshal.PtrToStringUni(buffer, len / 2);
                var principalDir = Path.GetDirectoryName(PrincipalName);
                if (string.IsNullOrEmpty(principalDir))
                    throw new DirectoryNotFoundException();
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
                    throw new FileNotFoundException();
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
        ///     The file to be pinned (environment variables are accepted).
        /// </param>
        public static bool PinToTaskbar(string path) =>
            PinUnpinTaskbar(path, true);

        /// <summary>
        ///     Unpin the specified file to taskbar.
        /// </summary>
        /// <param name="path">
        ///     The file to be unpinned (environment variables are accepted).
        /// </param>
        public static bool UnpinFromTaskbar(string path) =>
            PinUnpinTaskbar(path, false);

        /// <summary>
        ///     Determines whether the specified path specifies the specified
        ///     file attributes.
        /// </summary>
        /// <param name="path">
        ///     The file or directory to check (environment variables are accepted).
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

        private static void Linker(string linkPath, string destPath, bool destIsDir, bool backup = false, bool elevated = false)
        {
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
                return;
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
                return;
            }
            var cmd = string.Empty;
            if (backup)
                if (link.DirOrFileExists())
                    if (!DirIsLink(link))
                        cmd += $"MOVE /Y \"{link}\" \"{link}.SI13N7-BACKUP\"";
                    else
                        UnLinker(link, true, true, elevated);
            if (link.DirOrFileExists())
            {
                if (!string.IsNullOrEmpty(cmd))
                    cmd += " && ";
                cmd += $"{(destIsDir ? "RD /S /Q" : "DEL / F / Q")} \"{link}\"";
            }
            if (dest.DirOrFileExists())
            {
                if (!string.IsNullOrEmpty(cmd))
                    cmd += " && ";
                cmd += $"MKLINK {(destIsDir ? "/J " : string.Empty)}\"{link}\" \"{dest}\" && ATTRIB +H \"{link}\" /L";
            }
            if (string.IsNullOrEmpty(cmd))
                return;
            using (var p = ProcessEx.Send(cmd, elevated, false))
                if (p != null && !p.HasExited)
                    p.WaitForExit();
        }

        /// <summary>
        ///     Creates a symbolic link to the specified directory based on command prompt
        ///     which allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="linkPath">
        ///     The directory to be linked (environment variables are accepted).
        /// </param>
        /// <param name="destDir">
        ///     The fully qualified name of the new link (environment variables
        ///     are accepted).
        /// </param>
        /// <param name="backup">
        ///     true to create an backup for existing directories; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to create this link with highest privileges; otherwise, false.
        /// </param>
        public static void DirLink(string linkPath, string destDir, bool backup = false, bool elevated = false) =>
            Linker(linkPath, destDir, true, backup, elevated);

        /// <summary>
        ///     Creates a symbolic link to the specified file based on command prompt which
        ///     allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="linkPath">
        ///     The file to be linked (environment variables are accepted).
        /// </param>
        /// <param name="destFile">
        ///     The fully qualified name of the new link (environment variables
        ///     are accepted).
        /// </param>
        /// <param name="backup">
        ///     true to create an backup for existing files; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to create this link with highest privileges; otherwise, false.
        /// </param>
        public static void FileLink(string linkPath, string destFile, bool backup = false, bool elevated = false) =>
            Linker(linkPath, destFile, false, backup, elevated);

        private static void UnLinker(string path, bool pathIsDir, bool backup = false, bool elevated = false)
        {
            var link = PathEx.Combine(path);
            var cmd = string.Empty;
            if (backup)
                if ($"{link}.SI13N7-BACKUP".DirOrFileExists())
                {
                    if (link.DirOrFileExists())
                        cmd += $"{(pathIsDir ? "RD /S /Q" : "DEL / F / Q")} \"{link}\"";
                    if (!string.IsNullOrEmpty(cmd))
                        cmd += " && ";
                    cmd += $"MOVE /Y \"{link}.SI13N7-BACKUP\" \"{link}\"";
                }
            if (link.DirOrFileIsLink())
            {
                if (!string.IsNullOrEmpty(cmd))
                    cmd += " && ";
                cmd += $"{(pathIsDir ? "RD /S /Q" : "DEL /F /Q /A:L")} \"{link}\"";
            }
            if (string.IsNullOrEmpty(cmd))
                return;
            using (var p = ProcessEx.Send(cmd, elevated, false))
                if (p != null && !p.HasExited)
                    p.WaitForExit();
        }

        /// <summary>
        ///     Removes an symbolic link of the specified directory link based on command
        ///     prompt which allows a simple solution for the elevated execution of this
        ///     order.
        /// </summary>
        /// <param name="path">
        ///     The link to be removed (environment variables are accepted).
        /// </param>
        /// <param name="backup">
        ///     true to restore found backups; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to remove this link with highest privileges; otherwise, false.
        /// </param>
        public static void DirUnLink(string path, bool backup = false, bool elevated = false) =>
            UnLinker(path, true, backup, elevated);

        /// <summary>
        ///     Removes an symbolic link of the specified file link based on command prompt
        ///     which allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="path">
        ///     The link to be removed (environment variables are accepted).
        /// </param>
        /// <param name="backup">
        ///     true to restore found backups; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to remove this link with highest privileges; otherwise, false.
        /// </param>
        public static void FileUnLink(string path, bool backup = false, bool elevated = false) =>
            UnLinker(path, false, backup, elevated);

        /// <summary>
        ///     Copies an existing directory to a new location.
        /// </summary>
        /// <param name="srcDir">
        ///     The directory to copy (environment variables are accepted).
        /// </param>
        /// <param name="destDir">
        ///     The fully qualified name of the destination directory
        ///     (environment variables are accepted).
        /// </param>
        /// <param name="subDirs">
        ///     true to inlcude subdirectories; otherwise, false.
        /// </param>
        public static bool DirCopy(string srcDir, string destDir, bool subDirs = true)
        {
            try
            {
                var src = PathEx.Combine(srcDir);
                var di = new DirectoryInfo(src);
                if (!di.Exists)
                    throw new DirectoryNotFoundException();
                var dest = PathEx.Combine(destDir);
                if (!Directory.Exists(dest))
                    Directory.CreateDirectory(dest);
                foreach (var f in di.GetFiles())
                    f.CopyTo(Path.Combine(dest, f.Name), false);
                if (!subDirs)
                    return true;
                foreach (var d in di.GetDirectories())
                    DirCopy(d.FullName, Path.Combine(dest, d.Name));
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message + " (Source: '" + srcDir + "'; Destination: '" + destDir + "')", ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        ///     Copies an existing directory to a new location and deletes the source
        ///     directory if this task has been completed successfully.
        /// </summary>
        /// <param name="srcDir">
        ///     The directory to move (environment variables are accepted).
        /// </param>
        /// <param name="destDir">
        ///     The fully qualified name of the destination directory
        ///     (environment variables are accepted).
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
        public static int GetFullHashCode(this DirectoryInfo dirInfo)
        {
            try
            {
                var sb = new StringBuilder();
                long len = 0;
                foreach (var fi in dirInfo.GetFiles("*", SearchOption.AllDirectories))
                {
                    sb.Append(fi.Name);
                    len += fi.Length;
                }
                return $"{len}{sb}".GetHashCode();
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

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        private class ShellLink { }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
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
