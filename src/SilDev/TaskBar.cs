#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TaskBar.cs
// Version:  2018-07-04 12:36
// 
// Copyright (c) 2018, Si13n7 Developments (r)
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
    ///     Provides enumerated options that control the current state of the progres
    ///     button.
    /// </summary>
    public enum TaskBarProgressFlags
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
                return (TaskBarState)WinApi.NativeMethods.SHAppBarMessage(WinApi.AppBarMessageOptions.GetState, ref data);
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
                WinApi.NativeMethods.SHAppBarMessage(WinApi.AppBarMessageOptions.SetState, ref data);
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
        public static TaskBarLocation GetLocation(IntPtr hWnd = default(IntPtr))
        {
            var screen = hWnd == default(IntPtr) ? Screen.PrimaryScreen : Screen.FromHandle(hWnd);
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
        public static int GetSize(IntPtr hWnd = default(IntPtr))
        {
            var screen = hWnd == default(IntPtr) ? Screen.PrimaryScreen : Screen.FromHandle(hWnd);
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

        private static bool PinUnpin(string path, bool pin)
        {
            var fPath = PathEx.Combine(path);
            try
            {
                if (!File.Exists(fPath))
                    throw new PathNotFoundException(path);
                if (Environment.OSVersion.Version.Major >= 10)
                    ProcessEx.CurrentPrincipal.ChangeName("explorer.exe");
                var sb = new StringBuilder(byte.MaxValue);
                var lib = WinApi.NativeMethods.LoadLibrary(WinApi.DllNames.Shell32);
                WinApi.NativeMethods.LoadString(lib, pin ? 0x150au : 0x150bu, sb, 0xff);
                var type = Type.GetTypeFromProgID("Shell.Application");
                dynamic shell = Activator.CreateInstance(type);
                var fDir = Path.GetDirectoryName(path);
                var dir = shell.NameSpace(fDir);
                var fName = Path.GetFileName(path);
                var link = dir.ParseName(fName);
                var verb = sb.ToString();
                var verbs = link.Verbs();
                for (var i = 0; i < verbs.Count(); i++)
                {
                    var d = verbs.Item(i);
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
                if (File.Exists(fPath) && Environment.OSVersion.Version.Major >= 10)
                    ProcessEx.CurrentPrincipal.RestoreName();
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
        /// <param name="flags">
        ///     The flag that control the current state of the progress button.
        /// </param>
        public static void SetState(IntPtr hWnd, TaskBarProgressFlags flags) =>
            (TaskBar.TaskBarInstance as ComImports.ITaskBarList3)?.SetProgressState(hWnd, flags);

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
