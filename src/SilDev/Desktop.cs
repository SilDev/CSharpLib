#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Desktop.cs
// Version:  2023-11-28 16:57
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
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
        private const string ThemesPersonalizePath = "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize";

        /// <summary>
        ///     Determines whether dark mode is enabled for applications.
        /// </summary>
        public static bool AppsUseDarkTheme =>
            Reg.Read(ThemesPersonalizePath, "AppsUseLightTheme", 1) == 0;

        /// <summary>
        ///     Determines whether dark mode is enabled for the system.
        /// </summary>
        public static bool SystemUseDarkTheme =>
            Reg.Read(ThemesPersonalizePath, "SystemUseLightTheme", 1) == 0;

        /// <summary>
        ///     Refreshes the desktop.
        /// </summary>
        /// <param name="explorer">
        ///     <see langword="true"/> to refresh all open explorer windows; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="extended">
        ///     <see langword="true"/> to wait for a window to refresh, if there is no
        ///     window available; otherwise, <see langword="false"/>.
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
            InputDevice.SendKeyState(hWnd, VirtualKey.F5, VirtualKeyState.KeyDown);
            InputDevice.SendKeyState(hWnd, VirtualKey.F5, VirtualKeyState.KeyUp);
            if (explorer)
                RefreshExplorer(extended);
        }

        /// <summary>
        ///     Refreshes all open explorer windows.
        /// </summary>
        /// <param name="extended">
        ///     <see langword="true"/> to wait for a window to refresh, if there is no
        ///     window available; otherwise, <see langword="false"/>.
        /// </param>
        public static void RefreshExplorer(bool extended = false)
        {
            var hasUpdated = extended;
            var shellWindows = new ShellWindows();
            do
                foreach (InternetExplorer window in shellWindows)
                {
                    var name = Path.GetFileName(window?.FullName);
                    if (!name?.EqualsEx("explorer.exe") ?? true)
                        continue;
                    window.Refresh();
                    hasUpdated = true;
                }
            while (!hasUpdated);
        }
    }
}
