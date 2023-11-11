#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ResourcesEx.cs
// Version:  2023-11-11 16:27
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
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     Provides enumerated symbol index values of the Windows Image Resource
    ///     dynamic link library ('imageres.dll') file on Windows 10.
    /// </summary>
    public enum ImageResourceSymbol
    {
        Flip3D = 0,
        Shield = 1,
        UnknownFile = 2,
        Directory = 3,
        Folder = 4,
        DirectoryFace = 5,
        DirectoryPage = 6,
        Application = 11,
        OpenSearch = 13,
        Postcard = 15,
        Film = 18,
        Network = 20,
        SystemControl = 22,
        FloppyDrive = 23,
        DiscDrive = 25,
        Drive = 27,
        Chip = 29,
        HardDrive = 30,
        SystemDrive = 31,
        DvdDrive = 32,
        DvdR = 33,
        DvdRam = 34,
        DvdRom = 35,
        DvdRw = 36,
        VideoCamera = 41,
        Handy = 42,
        Printer = 46,
        RecycleBinFull = 49,
        RecycleBinEmpty = 50,
        Dvd = 51,
        PhotoCamera = 52,
        Security = 54,
        SdCard = 55,
        Cd = 56,
        CdR = 57,
        CdRom = 58,
        CdRw = 59,
        MediaPlayer = 61,
        ApplicationExtension = 62,
        Batch = 63,
        SetupInformationFile = 64,
        Picture = 65,
        Bitmap = 66,
        JoinPhotographic = 67,
        UnknownDrive = 70,
        Uac = 73,
        Asterisk = 76,
        Key = 77,
        NetworkGraphics = 78,
        Warning = 79,
        Barrier = 81,
        Install = 82,
        Sharing = 83,
        RichTextFile = 85,
        Error = 93,
        Help = 94,
        Run = 95,
        Screensaver = 96,
        HelpShield = 99,
        ErrorShield = 100,
        CheckShield = 101,
        WarnShield = 102,
        Computer = 104,
        Desktop = 105,
        Defrag = 106,
        UserDirectory = 117,
        TaskManager = 144,
        ShortcutMark = 154,
        SharedMark = 155,
        ZipFile = 165,
        Search = 168,
        DownArrow = 175,
        UpperArrow = 176,
        Explorer = 203,
        Favorite = 204,
        Stop = 207,
        User = 208,
        OneDriveDirectory = 217,
        SyncWarnMark = 218,
        SyncMark = 219,
        OneDrive = 220,
        Lock = 224,
        Briefcase = 226,
        FileLocallyAvailable = 227,
        FileSync = 228,
        SyncError = 229,
        SyncWarn = 230,
        FileOnlyOnlineAvailable = 231,
        FileAlwaysKeepOnDevice = 232,
        Unpin = 233,
        Pin = 234,
        Close = 235,
        IsoFile = 238,
        Clipboard = 241,
        Retry = 251,
        Undo = 255,
        CommandPrompt = 262,
        Play = 280
    }

    /// <summary>
    ///     Provides static methods for the usage of data resources.
    /// </summary>
    public static class ResourcesEx
    {
        /// <summary>
        ///     Extracts all icon resources from the file under the specified path, and
        ///     returns its <see cref="Tuple{T1, T2}"/> instances with the large icon as
        ///     the first item and the small icon as the second.
        /// </summary>
        /// <param name="path">
        ///     The path to the file that contains icon resources.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     path is null.
        /// </exception>
        /// <exception cref="PathNotFoundException">
        ///     path not found.
        /// </exception>
        /// <exception cref="Win32Exception">
        ///     path has no icon resources.
        /// </exception>
        public static IEnumerable<Tuple<Icon, Icon>> GetIconPairsFromFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            path = PathEx.Combine(path);
            if (!File.Exists(path))
                throw new PathNotFoundException(path);
            var count = WinApi.NativeMethods.ExtractIconEx(path, -1, null, null, 0);
            if (count < 1)
            {
                WinApi.ThrowLastError();
                yield break;
            }
            var ptrs1 = new IntPtr[count];
            var ptrs2 = new IntPtr[count];
            count = WinApi.NativeMethods.ExtractIconEx(path, 0, ptrs1, ptrs2, count);
            for (var i = 0; i < count; i++)
                yield return Tuple.Create(Icon.FromHandle(ptrs1[i]), Icon.FromHandle(ptrs2[i]));
        }

        /// <summary>
        ///     Extracts all large or small icon resources from the file under the
        ///     specified path, and returns its <see cref="Icon"/> instances.
        /// </summary>
        /// <param name="path">
        ///     The path to the file that contains icon resources.
        /// </param>
        /// <param name="large">
        ///     <see langword="true"/> to return the large icons; otherwise,
        ///     <see langword="false"/> to return the small icons.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     path is null.
        /// </exception>
        /// <exception cref="PathNotFoundException">
        ///     path not found.
        /// </exception>
        /// <exception cref="Win32Exception">
        ///     path has no icon resources.
        /// </exception>
        public static IEnumerable<Icon> GetIconsFromFile(string path, bool large = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            path = PathEx.Combine(path);
            if (!File.Exists(path))
                throw new PathNotFoundException(path);
            var count = WinApi.NativeMethods.ExtractIconEx(path, -1, null, null, 0);
            if (count < 1)
            {
                WinApi.ThrowLastError();
                yield break;
            }
            var ptrs = new IntPtr[count];
            count = WinApi.NativeMethods.ExtractIconEx(path, 0, large ? ptrs : null, !large ? ptrs : null, count);
            for (var i = 0; i < count; i++)
                yield return Icon.FromHandle(ptrs[i]);
        }

        /// <summary>
        ///     Extracts an icon resource under the specified index, from the file under
        ///     the specified path, and returns its <see cref="Icon"/> instance.
        /// </summary>
        /// <param name="path">
        ///     The path to the file that contains icon resources.
        /// </param>
        /// <param name="index">
        ///     The index of the icon to extract.
        /// </param>
        /// <param name="large">
        ///     <see langword="true"/> to return the large icon; otherwise,
        ///     <see langword="false"/> to return the small icon.
        /// </param>
        public static Icon GetIconFromFile(string path, int index = 0, bool large = false)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                var file = PathEx.Combine(path);
                if (!File.Exists(file))
                    throw new PathNotFoundException(file);
                var ptrs = new IntPtr[1];
                var count = WinApi.NativeMethods.ExtractIconEx(file, index, large ? ptrs : null, !large ? ptrs : null, 1);
                if (count < 1)
                {
                    WinApi.ThrowLastError();
                    return null;
                }
                var ptr = ptrs.FirstOrDefault();
                if (ptr == IntPtr.Zero)
                    throw new NullReferenceException();
                return Icon.FromHandle(ptr);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Retrieves a backward-compatible integer value of the specified
        ///     <see cref="ImageResourceSymbol"/> value, which depends on the file version
        ///     of the 'imageres.dll' file under the specified location.
        /// </summary>
        /// <param name="value">
        ///     The <see cref="ImageResourceSymbol"/> value.
        /// </param>
        /// <param name="location">
        ///     The directory where the 'imageres.dll' file is located.
        /// </param>
        public static int GetImageResourceValue(ImageResourceSymbol value, string location = "%system%")
        {
            var path = PathEx.Combine(location);
            if (!string.IsNullOrWhiteSpace(path) && PathEx.IsDir(path))
                path = Path.Combine(path, "imageres.dll");
            if (!path.EndsWithEx("imageres.dll") || !File.Exists(path))
                path = PathEx.Combine("%system%\\imageres.dll");
            var version = FileEx.GetFileVersion(path);

            // Windows 10
            if (version.Major >= 10)
                return (int)value;

            // Windows 7 + 8
            var index = (int)value;
            if (index < 187)
                return index;
            if (index.IsBetween(187, 215))
                return --index;
            if (index.IsBetween(233, version.Minor < 2 ? 235 : 322))
                return index - (index < 257 ? 17 : 16);
            return -1;
        }

        /// <summary>
        ///     Retrieves a backward-compatible string value of the specified symbol index
        ///     value, which depends on the file version of the 'imageres.dll' file under
        ///     the specified location.
        /// </summary>
        /// <param name="value">
        ///     The symbol index value.
        /// </param>
        /// <param name="location">
        ///     The directory where the 'imageres.dll' file is located.
        /// </param>
        public static string GetImageResourceName(int value, string location = "%system%")
        {
            var path = PathEx.Combine(location);
            if (!string.IsNullOrWhiteSpace(path) && PathEx.IsDir(path))
                path = Path.Combine(path, "imageres.dll");
            if (!path.EndsWithEx("imageres.dll") || !File.Exists(path))
                path = PathEx.Combine("%system%\\imageres.dll");
            var version = FileEx.GetFileVersion(path);

            // Windows 10
            if (version.Major >= 10)
                return Enum.GetName(typeof(ImageResourceSymbol), value);

            // Windows 7 + 8
            if (value < 187)
                return Enum.GetName(typeof(ImageResourceSymbol), value);
            if (value.IsBetween(186, 214))
                return Enum.GetName(typeof(ImageResourceSymbol), ++value);
            if (value.IsBetween(216, version.Minor < 2 ? 218 : 306))
                return Enum.GetName(typeof(ImageResourceSymbol), value + (value < 240 ? 17 : 16));
            return null;
        }

        /// <summary>
        ///     Extracts a large or small icon resource under the specified index, from the
        ///     'imageres.dll' under specified location, and returns its <see cref="Icon"/>
        ///     instance.
        /// </summary>
        /// <param name="index">
        ///     The index of the icon to extract.
        /// </param>
        /// <param name="large">
        ///     <see langword="true"/> to return the large image; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="location">
        ///     The directory where the 'imageres.dll' file is located.
        /// </param>
        public static Icon GetSystemIcon(ImageResourceSymbol index, bool large = false, string location = "%system%")
        {
            try
            {
                var path = PathEx.Combine(location);
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(location));
                if (PathEx.IsDir(path))
                    path = Path.Combine(path, "imageres.dll");
                if (!File.Exists(path))
                    path = PathEx.Combine("%system%\\imageres.dll");
                return GetIconFromFile(path, GetImageResourceValue(index), large);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Extracts a small icon resource under the specified index, from the
        ///     'imageres.dll' under specified location, and returns its <see cref="Icon"/>
        ///     instance.
        /// </summary>
        /// <param name="index">
        ///     The index of the icon to extract.
        /// </param>
        /// <param name="location">
        ///     The directory where the 'imageres.dll' file is located.
        /// </param>
        public static Icon GetSystemIcon(ImageResourceSymbol index, string location) =>
            GetSystemIcon(index, false, location);

        /// <summary>
        ///     Returns an file type icon of the specified file.
        /// </summary>
        /// <param name="path">
        ///     The file to get the file type icon.
        /// </param>
        /// <param name="large">
        ///     <see langword="true"/> to return the large image; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Icon GetFileTypeIcon(string path, bool large = false)
        {
            try
            {
                path = PathEx.Combine(path);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                if (!File.Exists(path))
                    throw new PathNotFoundException(path);
                var shfi = new WinApi.ShFileInfo();
                var flags = WinApi.FileInfoFlags.Icon | WinApi.FileInfoFlags.UseFileAttributes;
                flags |= large ? WinApi.FileInfoFlags.LargeIcon : WinApi.FileInfoFlags.SmallIcon;
                WinApi.NativeMethods.SHGetFileInfo(path, 0x80, ref shfi, (uint)Marshal.SizeOf(shfi), flags);
                var ico = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
                WinApi.NativeMethods.DestroyIcon(shfi.hIcon);
                return ico;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Extracts the specified resources from the current process to a new file.
        /// </summary>
        /// <param name="resData">
        ///     The resource to extract.
        /// </param>
        /// <param name="destPath">
        ///     The file to create.
        /// </param>
        /// <param name="reverseBytes">
        ///     <see langword="true"/> to invert the order of the bytes in the specified
        ///     sequence before extracting; otherwise, <see langword="false"/>.
        /// </param>
        public static void Extract(byte[] resData, string destPath, bool reverseBytes = false)
        {
            try
            {
                var path = PathEx.Combine(destPath);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(destPath));
                var dir = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(dir))
                    throw new NullReferenceException();
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                using var ms = new MemoryStream(resData);
                var data = ms.ToArray();
                if (reverseBytes)
                    data = data.Reverse().ToArray();
                using var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                fs.Write(data, 0, data.Length);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }
    }
}
