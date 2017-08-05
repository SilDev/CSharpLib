#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Tray.cs
// Version:  2017-08-05 09:42
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Drawing;
    using System.Threading;

    /// <summary>
    ///     Provides the functionality to manage the items of the system tray area.
    /// </summary>
    public static class Tray
    {
        /// <summary>
        ///     Refreshes the symbols on system tray.
        /// </summary>
        public static void Refresh()
        {
            var arrays = new[]
            {
                new[]
                {
                    "Shell_TrayWnd",
                    "TrayNotifyWnd",
                    "SysPager",
                    "ToolbarWindow32"
                },
                new[]
                {
                    "NotifyIconOverflowWindow",
                    "ToolbarWindow32"
                }
            };
            foreach (var array in arrays)
                try
                {
                    var hWnd = IntPtr.Zero;
                    foreach (var str in array)
                    {
                        WinApi.NativeHelper.FindNestedWindow(ref hWnd, str);
                        if (hWnd == IntPtr.Zero)
                            throw new ArgumentNullException(nameof(hWnd));
                    }
                    MouseMove:
                    WinApi.NativeMethods.GetClientRect(hWnd, out Rectangle rect1);
                    for (var x = 0; x < rect1.Right; x += 5)
                    {
                        for (var y = 0; y < rect1.Bottom; y += 5)
                            WinApi.NativeHelper.SendMessage(hWnd, (uint)WinApi.WindowMenuFlags.WmMouseMove, IntPtr.Zero, new IntPtr((y << 16) + x));
                        WinApi.NativeMethods.GetClientRect(hWnd, out Rectangle rect2);
                        if (rect1 != rect2)
                            goto MouseMove;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
        }

        /// <summary>
        ///     Refreshes the symbols on system tray asynchronous.
        /// </summary>
        public static void RefreshAsync()
        {
            try
            {
                var thread = new Thread(Refresh)
                {
                    IsBackground = true
                };
                thread.Start();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Refreshes the symbols of the visible system tray area.
        /// </summary>
        [Obsolete("Kept for backward compatibility; just use Refresh method.")]
        public static bool RefreshVisibleArea()
        {
            RefreshAsync();
            return true;
        }
    }
}
