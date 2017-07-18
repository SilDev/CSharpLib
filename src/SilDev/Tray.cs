#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Tray.cs
// Version:  2017-07-18 04:31
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
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
        ///     Refreshes the visible system tray area of the taskbar.
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
                    WinApi.NativeHelper.FindNestedWindow(ref hWnd, className);
                    if (hWnd == IntPtr.Zero)
                        throw new ArgumentNullException(nameof(hWnd));
                }
                WinApi.NativeMethods.GetClientRect(hWnd, out Rectangle rect);
                for (var x = 0; x < rect.Right; x += 5)
                    for (var y = 0; y < rect.Bottom; y += 5)
                        if ((uint)WinApi.NativeHelper.SendMessage(hWnd, (uint)WinApi.WindowMenuFlags.WmMouseMove, IntPtr.Zero, new IntPtr((y << 16) + x)) != 1)
                            WinApi.NativeHelper.ThrowLastError();
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
            var tBarBtn64 = new ToolBarButton64();
            var tBarBtn32 = new ToolBarButton32();
            var trayData = new TrayData();
            lock (Locker)
            {
                for (var pass = 1; pass < 3; pass++)
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
                            WinApi.NativeHelper.FindNestedWindow(ref hWnd, name);
                    }
                    catch
                    {
                        if (kind == 0)
                            continue;
                        break;
                    }
                    using (var pm = new ProcessMemory(hWnd))
                    {
                        var rBtnPtr = Environment.Is64BitOperatingSystem ? pm.Allocate(tBarBtn64) : pm.Allocate(tBarBtn32);
                        pm.Allocate(trayData);
                        var itemCount = (uint)WinApi.NativeMethods.SendMessage(hWnd, 0x418, IntPtr.Zero, IntPtr.Zero);
                        uint removedCount = 0;
                        for (var i = 0; i < itemCount; i++)
                            try
                            {
                                var item = i - removedCount;
                                if ((uint)WinApi.NativeMethods.SendMessage(hWnd, 0x417, new IntPtr(item), rBtnPtr) != 1)
                                    throw new ApplicationException("Can not get the button of item '" + item + "' (kind: '" + kind + "').");
                                if (Environment.Is64BitOperatingSystem)
                                {
                                    pm.Read(tBarBtn64, rBtnPtr);
                                    pm.Read(trayData, tBarBtn64.dwData);
                                }
                                else
                                {
                                    pm.Read(tBarBtn32, rBtnPtr);
                                    pm.Read(trayData, tBarBtn32.dwData);
                                }
                                var hWnd2 = trayData.hWnd;
                                if (hWnd2 == IntPtr.Zero)
                                    throw new ApplicationException("Invalid window handle.");
                                using (var pm2 = new ProcessMemory(hWnd2))
                                {
                                    var name = pm2.GetImageFileName();
                                    if (!string.IsNullOrWhiteSpace(name))
                                        Log.Write("Tray icon found: '" + name + "' (kind: '" + kind + "').");
                                    if (pass < 2 || !string.IsNullOrWhiteSpace(name))
                                        continue;
                                    if ((uint)WinApi.NativeMethods.SendMessage(hWnd, 0x416, new IntPtr(item), IntPtr.Zero) != 1)
                                        throw new ApplicationException("Can not remove the button of '" + name + "' (kind: '" + kind + "').");
                                    Log.Write("Tray icon removed: '" + name + "' (kind: '" + kind + "').");
                                    itemCount--;
                                    removedCount++;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Write(ex);
                            }
                    }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class ToolBarButton32
        {
            internal IntPtr dwData;
            internal byte fsState;
            internal byte fsStyle;
            internal uint iBitmap;
            internal uint idCommand;
            internal uint iString;
#pragma warning disable 169
            private byte bReserved0;
            private byte bReserved1;
#pragma warning restore 169
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class ToolBarButton64
        {
            internal IntPtr dwData;
            internal byte fsState;
            internal byte fsStyle;
            internal uint iBitmap;
            internal uint idCommand;
            internal uint iString;
#pragma warning disable 169
            private byte bReserved0;
            private byte bReserved1;
            private byte bReserved2;
            private byte bReserved3;
            private byte bReserved4;
            private byte bReserved5;
#pragma warning restore 169
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class TrayData
        {
            internal IntPtr hIcon;
            internal IntPtr hWnd;
            internal uint uCallbackMessage;
            internal uint uID;
#pragma warning disable 169
            private uint reserved0;
            private uint reserved1;
#pragma warning restore 169
        }
    }
}
