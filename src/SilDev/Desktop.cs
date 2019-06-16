#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Desktop.cs
// Version:  2019-06-16 11:03
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System.IO;
    using SHDocVw;

    /// <summary>
    ///     Provides the functionality to manage desktop functions.
    /// </summary>
    public static class Desktop
    {
        /// <summary>
        ///     Refreshes the desktop.
        /// </summary>
        /// <param name="explorer">
        ///     true to refresh all open explorer windows; otherwise, false.
        /// </param>
        /// <param name="extended">
        ///     true to wait for a window to refresh, if there is no window available;
        ///     otherwise, false.
        /// </param>
        public static void Refresh(bool explorer = true, bool extended = false)
        {
            var hWnd = WinApi.NativeHelper.FindWindow("Progman", "Program Manager");
            var cNames = new[]
            {
                "SHELLDLL_DefView",
                "SysListView32"
            };
            foreach (var cName in cNames)
                WinApi.NativeHelper.FindNestedWindow(ref hWnd, cName);
            InputDevice.SendKeyState(hWnd, VirtualKeys.F5, VirtualKeyStates.KeyDown);
            InputDevice.SendKeyState(hWnd, VirtualKeys.F5, VirtualKeyStates.KeyUp);
            if (explorer)
                RefreshExplorer(extended);
        }

        /// <summary>
        ///     Refreshes all open explorer windows.
        /// </summary>
        /// <param name="extended">
        ///     true to wait for a window to refresh, if there is no window available;
        ///     otherwise, false.
        /// </param>
        public static void RefreshExplorer(bool extended = false)
        {
            var hasUpdated = extended;
            var shellWindows = new ShellWindows();
            do
                foreach (InternetExplorer window in shellWindows)
                {
                    var name = Path.GetFileName(window?.FullName);
                    if (name?.EqualsEx("explorer.exe") != true)
                        continue;
                    window.Refresh();
                    hasUpdated = true;
                }
            while (!hasUpdated);
        }
    }
}
