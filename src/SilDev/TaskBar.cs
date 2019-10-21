#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TaskBar.cs
// Version:  2019-10-21 15:11
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using Intern;
    using Microsoft.Win32;

    /// <summary>
    ///     Provides enumerated flags of the taskbar location.
    /// </summary>
    public enum TaskBarLocation
    {
        /// <summary>
        ///     The taskbar is hidden.
        /// </summary>
        Hidden,

        /// <summary>
        ///     The taskbar is located at the top.
        /// </summary>
        Top,

        /// <summary>
        ///     The taskbar is located at the bottom.
        /// </summary>
        Bottom,

        /// <summary>
        ///     The taskbar is on the left side.
        /// </summary>
        Left,

        /// <summary>
        ///     The taskbar is on the right side.
        /// </summary>
        Right
    }

    /// <summary>
    ///     Provides enumerated options that control the current state of the progress
    ///     button.
    /// </summary>
    public enum TaskBarProgressState
    {
        /// <summary>
        ///     Stops displaying progress and returns the button to its normal state. Call this
        ///     method with this flag to dismiss the progress bar when the operation is complete
        ///     or canceled.
        /// </summary>
        NoProgress = 0x0,

        /// <summary>
        ///     The progress indicator does not grow in size, but cycles repeatedly along the
        ///     length of the taskbar button. This indicates activity without specifying what
        ///     proportion of the progress is complete. Progress is taking place, but there is
        ///     no prediction as to how long the operation will take.
        /// </summary>
        Indeterminate = 0x1,

        /// <summary>
        ///     The progress indicator grows in size from left to right in proportion to the
        ///     estimated amount of the operation completed. This is a determinate progress
        ///     indicator; a prediction is being made as to the duration of the operation.
        /// </summary>
        Normal = 0x2,

        /// <summary>
        ///     The progress indicator turns red to show that an error has occurred in one of
        ///     the windows that is broadcasting progress. This is a determinate state. If the
        ///     progress indicator is in the indeterminate state, it switches to a red
        ///     determinate display of a generic percentage not indicative of actual progress.
        /// </summary>
        Error = 0x4,

        /// <summary>
        ///     The progress indicator turns yellow to show that progress is currently stopped
        ///     in one of the windows but can be resumed by the user. No error condition exists
        ///     and nothing is preventing the progress from continuing. This is a determinate
        ///     state. If the progress indicator is in the indeterminate state, it switches to a
        ///     yellow determinate display of a generic percentage not indicative of actual
        ///     progress.
        /// </summary>
        Paused = 0x8
    }

    /// <summary>
    ///     Provides enumerated options that control the current state of the taskbar.
    /// </summary>
    public enum TaskBarState
    {
        /// <summary>
        ///     The taskbar is in the autohide state.
        /// </summary>
        AutoHide = 0x1,

        /// <summary>
        ///     The taskbar is in the always-on-top state.
        ///     <para>
        ///         Note that as of Windows 7, AlwaysOnTop is no longer returned because the taskbar
        ///         is always in that state.
        ///     </para>
        /// </summary>
        AlwaysOnTop = 0x2
    }

    /// <summary>
    ///     Provides static methods to enable you to get or set the state of the taskbar.
    /// </summary>
    public static class TaskBar
    {
        internal static ComImports.TaskbarInstance TaskBarInstance => new ComImports.TaskbarInstance();

        /// <summary>
        ///     Returns the current <see cref="TaskBarState"/> of the taskbar.
        /// </summary>
        public static TaskBarState GetState()
        {
            var data = new WinApi.AppBarData();
            try
            {
                data.cbSize = (uint)Marshal.SizeOf(data);
                data.hWnd = WinApi.NativeMethods.FindWindow("System_TrayWnd", null);
                return (TaskBarState)WinApi.NativeMethods.SHAppBarMessage(WinApi.AppBarMessageOption.GetState, ref data);
            }
            finally
            {
                data.Dispose();
            }
        }

        /// <summary>
        ///     Sets the new <see cref="TaskBarState"/> of the taskbar.
        /// </summary>
        /// <param name="state">
        ///     The new state to set.
        /// </param>
        public static void SetState(TaskBarState state)
        {
            var data = new WinApi.AppBarData();
            try
            {
                data.cbSize = (uint)Marshal.SizeOf(data);
                data.hWnd = WinApi.NativeMethods.FindWindow("System_TrayWnd", null);
                data.lParam = (int)state;
                WinApi.NativeMethods.SHAppBarMessage(WinApi.AppBarMessageOption.SetState, ref data);
            }
            finally
            {
                data.Dispose();
            }
        }

        /// <summary>
        ///     Returns the location of the taskbar.
        /// </summary>
        /// <param name="hWnd">
        ///     The handle of the window on which the taskbar is located.
        /// </param>
        public static TaskBarLocation GetLocation(IntPtr hWnd = default)
        {
            var screen = hWnd == default ? Screen.PrimaryScreen : Screen.FromHandle(hWnd);
            if (screen.WorkingArea == screen.Bounds)
                return TaskBarLocation.Hidden;
            if (screen.WorkingArea.Width != screen.Bounds.Width)
                return screen.WorkingArea.Left > 0 ? TaskBarLocation.Left : TaskBarLocation.Right;
            return screen.WorkingArea.Top > 0 ? TaskBarLocation.Top : TaskBarLocation.Bottom;
        }

        /// <summary>
        ///     Returns the size of the taskbar.
        /// </summary>
        /// <param name="hWnd">
        ///     The handle of the window on which the taskbar is located.
        /// </param>
        public static int GetSize(IntPtr hWnd = default)
        {
            var screen = hWnd == default ? Screen.PrimaryScreen : Screen.FromHandle(hWnd);
            switch (GetLocation())
            {
                case TaskBarLocation.Top:
                    return screen.WorkingArea.Top;
                case TaskBarLocation.Bottom:
                    return screen.Bounds.Bottom - screen.WorkingArea.Bottom;
                case TaskBarLocation.Right:
                    return screen.Bounds.Right - screen.WorkingArea.Right;
                case TaskBarLocation.Left:
                    return screen.WorkingArea.Left;
                default:
                    return 0;
            }
        }

        /// <summary>
        ///     Adds an item to the taskbar.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window to be added to the taskbar.
        /// </param>
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static void AddTab(IntPtr hWnd) =>
            (TaskBarInstance as ComImports.ITaskBarList3)?.AddTab(hWnd);

        /// <summary>
        ///     Deletes an item from the taskbar.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window to be deleted from the taskbar.
        /// </param>
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static void DeleteTab(IntPtr hWnd) =>
            (TaskBarInstance as ComImports.ITaskBarList3)?.DeleteTab(hWnd);

        /// <summary>
        ///     Gets the link path from a pinned item based on its file path.
        /// </summary>
        /// <param name="path">
        ///     The file to get the link.
        /// </param>
        public static string GetPinLink(string path)
        {
            var file = PathEx.Combine(path);
            var dir = PathEx.Combine(Environment.SpecialFolder.ApplicationData, "Microsoft\\Internet Explorer\\Quick Launch\\User Pinned\\TaskBar");
            foreach (var link in DirectoryEx.EnumerateFiles(dir, "*.lnk"))
            {
                var target = ShellLink.GetTarget(link);
                if (string.IsNullOrEmpty(target))
                    continue;
                if (target.StartsWith("%", StringComparison.Ordinal))
                    target = PathEx.Combine(target);
                if (!File.Exists(target))
                    continue;
                if (target.EqualsEx(file))
                    return link;
            }
            return null;
        }

        /// <summary>
        ///     Determines whether the specified file is pinned.
        /// </summary>
        /// <param name="path">
        ///     The file to be checked.
        /// </param>
        public static bool IsPinned(string path) =>
            File.Exists(GetPinLink(path));

        private static bool PinUnpin(string path, bool pin)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            var file = PathEx.Combine(path);
            if (!File.Exists(file))
                return false;

            if (IsPinned(file) == pin)
                return true;

            var isPresentWindows = Environment.OSVersion.Version.Major >= 10;
            var shellKeyPath = default(string);
            try
            {
                if (isPresentWindows)
                    //ProcessEx.CurrentPrincipal.ChangeName("explorer.exe");
                    throw new NotSupportedException();

                dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));
                var dir = shell.NameSpace(Path.GetDirectoryName(path));
                var name = Path.GetFileName(path);
                var link = dir.ParseName(name);
                var verbs = link.Verbs();

                var sb = new StringBuilder(byte.MaxValue);
                var lib = WinApi.NativeMethods.LoadLibrary(WinApi.DllNames.Shell32);
                _ = WinApi.NativeMethods.LoadString(lib, pin ? 0x150au : 0x150bu, sb, 0xff);
                var verb = sb.ToString();

                /*
                if (!isPresentWindows)
                {
                */
                var applied = false;
                for (var i = 0; i < verbs.Count(); i++)
                {
                    var e = verbs.Item(i);
                    if ((pin || !e.Name.ContainsEx(verb)) && (!pin || !e.Name.EqualsEx(verb)))
                        continue;
                    e.DoIt();
                    applied = true;
                    break;
                }
                if (applied)
                    goto Done;
                //}

                if (string.IsNullOrWhiteSpace(verb))
                    verb = "Toggle Taskbar Pin";
                const string cmdKeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\CommandStore\\shell\\Windows.taskbarpin";
                var cmdHandler = Reg.ReadString(Registry.LocalMachine, cmdKeyPath, "ExplorerCommandHandler");
                if (!string.IsNullOrEmpty(cmdHandler))
                {
                    shellKeyPath = $"Software\\Classes\\*\\shell\\{verb}";
                    Reg.Write(Registry.CurrentUser, shellKeyPath, "ExplorerCommandHandler", cmdHandler);
                }
                if (Reg.EntryExists(Registry.CurrentUser, shellKeyPath, "ExplorerCommandHandler"))
                    link.InvokeVerb(verb);

                Done:
                if (!pin)
                    return IsPinned(file) == false;
                var curLink = GetPinLink(path);
                if (!File.Exists(curLink))
                    return false;
                var target = ShellLink.GetTarget(curLink);
                var envVar = EnvironmentEx.GetVariablePathFull(target, false, false);
                if (!target.EqualsEx(envVar))
                    FileEx.CreateShellLink(file, curLink);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
            finally
            {
                /*
                if (isPresentWindows)
                    ProcessEx.CurrentPrincipal.RestoreName();
                */
                if (!string.IsNullOrEmpty(shellKeyPath))
                    Reg.RemoveSubKey(Registry.CurrentUser, shellKeyPath);
            }
        }

        /// <summary>
        ///     Pin the specified file to taskbar.
        /// </summary>
        /// <param name="path">
        ///     The file to be pinned.
        /// </param>
        public static bool Pin(string path) =>
            PinUnpin(path, true);

        /// <summary>
        ///     Unpin the specified file to taskbar.
        /// </summary>
        /// <param name="path">
        ///     The file to be unpinned.
        /// </param>
        public static bool Unpin(string path) =>
            PinUnpin(path, false);
    }

    /// <summary>
    ///     Provides static methods to manage a progress bar hosted in a taskbar button.
    /// </summary>
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public static class TaskBarProgress
    {
        /// <summary>
        ///     Sets the type and state of the progress indicator displayed on a taskbar button.
        /// </summary>
        /// <param name="hWnd">
        ///     The handle of the window in which the progress of an operation is being shown.
        /// </param>
        /// <param name="state">
        ///     The flag that control the current state of the progress button.
        /// </param>
        public static void SetState(IntPtr hWnd, TaskBarProgressState state) =>
            (TaskBar.TaskBarInstance as ComImports.ITaskBarList3)?.SetProgressState(hWnd, state);

        /// <summary>
        ///     Displays or updates a progress bar hosted in a taskbar button to show the specific
        ///     percentage completed of the full operation.
        /// </summary>
        /// <param name="hWnd">
        ///     The handle of the window whose associated taskbar button is being used as a progress
        ///     indicator.
        /// </param>
        /// <param name="progressValue">
        ///     An application-defined value that indicates the proportion of the operation that has
        ///     been completed at the time the method is called.
        /// </param>
        /// <param name="progressMax">
        ///     An application-defined value that specifies the value ullCompleted will have when the
        ///     operation is complete.
        /// </param>
        public static void SetValue(IntPtr hWnd, double progressValue, double progressMax) =>
            (TaskBar.TaskBarInstance as ComImports.ITaskBarList3)?.SetProgressValue(hWnd, (ulong)progressValue, (ulong)progressMax);
    }
}
