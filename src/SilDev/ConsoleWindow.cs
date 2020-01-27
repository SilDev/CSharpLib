#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ConsoleWindow.cs
// Version:  2020-01-27 23:21
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    ///     Provides the functionality to allocate a <see cref="Console"/> window.
    /// </summary>
    public static class ConsoleWindow
    {
        private const uint AttachParent = uint.MaxValue;
        private const uint ErrorAccessDenied = 0x5;
        private const uint FileAttributeNormal = 0x80;
        private const uint FileShareRead = 0x1;
        private const uint FileShareWrite = 0x2;
        private const uint GenericRead = 0x80000000;
        private const uint GenericWrite = 0x40000000;
        private const uint OpenExisting = 0x3;
        private static bool _isAllocated;
        private static SafeFileHandle _safeFileHandle;
        private static StreamReader _streamReader;
        private static StreamWriter _streamWriter;

        /// <summary>
        ///     Allocates a new <see cref="Console"/> for the current process.
        ///     <para>
        ///         <see langword="true"/> to disable the close button of the windows;
        ///         otherwise, <see langword="false"/>.
        ///     </para>
        /// </summary>
        public static void Allocate(bool disableCloseButton = false)
        {
            if (WinApi.NativeMethods.AttachConsole(AttachParent) != 0 || Marshal.GetLastWin32Error() == ErrorAccessDenied || WinApi.NativeMethods.AllocConsole() == 0)
                return;

            if (disableCloseButton)
            {
                var hWnd = WinApi.NativeMethods.GetConsoleWindow();
                if (hWnd == IntPtr.Zero)
                    return;
                var hMenu = WinApi.NativeMethods.GetSystemMenu(hWnd, false);
                if (hMenu != IntPtr.Zero)
                    _ = WinApi.NativeMethods.DeleteMenu(hMenu, (int)WinApi.WindowMenuFlags.ScClose, WinApi.ModifyMenuFlags.ByCommand);
            }

            var fsOut = CreateFileStream("CONOUT$", GenericWrite, FileShareWrite, FileAccess.Write);
            var fsIn = CreateFileStream("CONIN$", GenericRead, FileShareRead, FileAccess.Read);
            if (fsOut == null || fsIn == null)
            {
                fsOut?.Dispose();
                fsIn?.Dispose();
                return;
            }

            if (_isAllocated)
                OnProcessExit(null, null);
            else
            {
                _isAllocated = true;
                AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            }

            _streamWriter = new StreamWriter(fsOut)
            {
                AutoFlush = true
            };
            Console.SetOut(_streamWriter);
            Console.SetError(_streamWriter);

            _streamReader = new StreamReader(fsIn);
            Console.SetIn(_streamReader);
        }

        private static FileStream CreateFileStream(string name, uint win32DesiredAccess, uint win32ShareMode, FileAccess dotNetFileAccess)
        {
            _safeFileHandle = new SafeFileHandle(WinApi.NativeMethods.CreateFileW(name, win32DesiredAccess, win32ShareMode, IntPtr.Zero, OpenExisting, FileAttributeNormal, IntPtr.Zero), true);
            return _safeFileHandle.IsInvalid ? null : new FileStream(_safeFileHandle, dotNetFileAccess);
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            _streamWriter?.Dispose();
            _streamReader?.Dispose();
        }
    }
}
