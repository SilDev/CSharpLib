#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Tray.cs
// Version:  2017-04-12 17:16
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     Provides the functionality to manage the items of the system tray area
    ///     of the taskbar.
    /// </summary>
    public static class Tray
    {
        private static readonly object Locker = new object();

        /// <summary>
        ///     Refreshes the visible system tray area of the taskbar (experimental
        ///     function).
        /// </summary>
        public static bool RefreshVisibleArea()
        {
            try
            {
                var hWnd = IntPtr.Zero;
                var classNames = new[]
                {
                    "Shell_TrayWnd",
                    "TrayNotifyWnd",
                    "SysPager",
                    "ToolbarWindow32"
                };
                foreach (var className in classNames)
                {
                    WinApi.FindNestedWindow(ref hWnd, className);
                    if (hWnd == IntPtr.Zero)
                        throw new ArgumentNullException(nameof(hWnd));
                }
                Rectangle rect;
                WinApi.UnsafeNativeMethods.GetClientRect(hWnd, out rect);
                for (var x = 0; x < rect.Right; x += 5)
                    for (var y = 0; y < rect.Bottom; y += 5)
                        WinApi.UnsafeNativeMethods.SendMessage(hWnd, (uint)WinApi.WindowMenuFunc.WM_MOUSEMOVE, IntPtr.Zero, new IntPtr((y << 16) + x));
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Removes invalid icons of the visible and hidden system tray area of
        ///     the taskbar (experimental function).
        /// </summary>
        public static void RemovePhantomIcons()
        {
            try
            {
                var tBarBtn64 = new ToolBarButton64();
                var tBarBtn32 = new ToolBarButton32();
                var trayData = new TrayData();
                var itemFound = false;
                var totalRemovedCount = 0;
                var totalItemCount = 0;
                lock (Locker)
                {
                    for (var pass = 1; pass < 3; pass++)
                    {
                        for (var kind = 0; kind < 2; kind++)
                        {
                            var hWnd = IntPtr.Zero;
                            string[] names;
                            if (kind == 0)
                                names = new[]
                                {
                                    "Shell_TrayWnd",
                                    "TrayNotifyWnd",
                                    "SysPager",
                                    "ToolbarWindow32"
                                };
                            else
                                names = new[]
                                {
                                    "NotifyIconOverflowWindow",
                                    "ToolbarWindow32"
                                };
                            try
                            {
                                foreach (var name in names)
                                    WinApi.FindNestedWindow(ref hWnd, name);
                            }
                            catch
                            {
                                if (kind == 0)
                                    continue;
                                break;
                            }
                            using (var p = new ProcessMemory(hWnd))
                            {
                                var remoteButtonPtr = Environment.Is64BitOperatingSystem ? p.Allocate(tBarBtn64) : p.Allocate(tBarBtn32);
                                p.Allocate(trayData);
                                var itemCount = (uint)WinApi.UnsafeNativeMethods.SendMessage(hWnd, 0x418, IntPtr.Zero, IntPtr.Zero);
                                uint removedCount = 0;
                                for (var i = 0; i < itemCount; i++)
                                {
                                    totalItemCount++;
                                    var item2 = i - removedCount;
                                    if (WinApi.UnsafeNativeMethods.SendMessage(hWnd, 0x417, new IntPtr(item2), remoteButtonPtr) != new IntPtr(1))
                                        throw new ApplicationException("TB_GETBUTTON failed");
                                    if (Environment.Is64BitOperatingSystem)
                                    {
                                        p.Read(tBarBtn64, remoteButtonPtr);
                                        p.Read(trayData, tBarBtn64.dwData);
                                    }
                                    else
                                    {
                                        p.Read(tBarBtn32, remoteButtonPtr);
                                        p.Read(trayData, tBarBtn32.dwData);
                                    }
                                    var hWnd2 = trayData.hWnd;
                                    if (hWnd2 == IntPtr.Zero)
                                        throw new ApplicationException("Invalid window handle.");
                                    using (var p2 = new ProcessMemory(hWnd2))
                                    {
                                        var name = p2.GetImageFileName();
                                        if (!string.IsNullOrWhiteSpace(name))
                                            Log.Write(nameof(Tray) + " found: '" + name + "' (kind: '" + kind + "').");
                                        if (pass == 1)
                                            if (name?.EndsWithEx(".exe") == true)
                                            {
                                                itemFound = true;
                                                break;
                                            }
                                        if (pass < 2 || !string.IsNullOrWhiteSpace(name))
                                            continue;
                                        if ((uint)WinApi.UnsafeNativeMethods.SendMessage(hWnd, 0x416, new IntPtr(item2), IntPtr.Zero) != 1)
                                            throw new WarningException(nameof(Tray) + " can not remove the button of '" + name + "' (kind: '" + kind + "').");
                                        removedCount++;
                                        totalRemovedCount++;
                                    }
                                }
                            }
                        }
                        if (totalItemCount != 0 && !itemFound)
                            throw new WarningException(nameof(Tray) + " failed to find any real icon.");
                    }
                }
                throw new WarningException(nameof(Tray) + " removed " + totalRemovedCount + "/" + totalItemCount + " icons.");
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class ToolBarButton32
        {
            internal uint iBitmap;
            internal uint idCommand;
            internal byte fsState;
            internal byte fsStyle;
#pragma warning disable 169
            private byte bReserved0;
            private byte bReserved1;
#pragma warning restore 169
            internal IntPtr dwData;
            internal uint iString;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class ToolBarButton64
        {
            internal uint iBitmap;
            internal uint idCommand;
            internal byte fsState;
            internal byte fsStyle;
#pragma warning disable 169
            private byte bReserved0;
            private byte bReserved1;
            private byte bReserved2;
            private byte bReserved3;
            private byte bReserved4;
            private byte bReserved5;
#pragma warning restore 169
            internal IntPtr dwData;
            internal uint iString;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class TrayData
        {
            internal IntPtr hWnd;
            internal uint uID;
            internal uint uCallbackMessage;
#pragma warning disable 169
            private uint reserved0;
            private uint reserved1;
#pragma warning restore 169
            internal IntPtr hIcon;
        }
    }
}
