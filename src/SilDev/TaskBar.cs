#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TaskBar.cs
// Version:  2017-06-23 12:07
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    ///     Provides static methods to enable you to get or set the state of the taskbar.
    /// </summary>
    public static class TaskBar
    {
        /// <summary>
        ///     Provides enumerated flags of the taskbar location.
        /// </summary>
        public enum Location
        {
            Hidden,
            Top,
            Bottom,
            Left,
            Right
        }

        /// <summary>
        ///     Provides enumerated options that control the current state of the taskbar.
        /// </summary>
        public enum State
        {
            /// <summary>
            ///     The taskbar is in the autohide state.
            /// </summary>
            AutoHide = 0x1,

            /// <summary>
            ///     <para>
            ///         The taskbar is in the always-on-top state.
            ///     </para>
            ///     <para>
            ///         Note that as of Windows 7, AlwaysOnTop is no longer returned because the taskbar
            ///         is always in that state.
            ///     </para>
            /// </summary>
            AlwaysOnTop = 0x2
        }

        private static TaskbarInstance TaskBarInstance => new TaskbarInstance();

        /// <summary>
        ///     Returns the current <see cref="State"/> of the taskbar.
        /// </summary>
        public static State GetState()
        {
            var data = new WinApi.AppBarData();
            try
            {
                data.cbSize = (uint)Marshal.SizeOf(data);
                data.hWnd = WinApi.NativeMethods.FindWindow("System_TrayWnd", null);
                return (State)WinApi.NativeMethods.SHAppBarMessage(WinApi.AppBarMessageOptions.GetState, ref data);
            }
            finally
            {
                data.Dispose();
            }
        }

        /// <summary>
        ///     Sets the new <see cref="State"/> of the taskbar.
        /// </summary>
        /// <param name="state">
        ///     The new state to set.
        /// </param>
        public static void SetState(State state)
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
        public static Location GetLocation(IntPtr? hWnd = null)
        {
            var screen = hWnd == null ? Screen.PrimaryScreen : Screen.FromHandle((IntPtr)hWnd);
            if (screen.WorkingArea == screen.Bounds)
                return Location.Hidden;
            if (screen.WorkingArea.Width != screen.Bounds.Width)
                return screen.WorkingArea.Left > 0 ? Location.Left : Location.Right;
            return screen.WorkingArea.Top > 0 ? Location.Top : Location.Bottom;
        }

        /// <summary>
        ///     Returns the size of the taskbar.
        /// </summary>
        /// <param name="hWnd">
        ///     The handle of the window on which the taskbar is located.
        /// </param>
        public static int GetSize(IntPtr? hWnd = null)
        {
            var screen = hWnd == null ? Screen.PrimaryScreen : Screen.FromHandle((IntPtr)hWnd);
            switch (GetLocation())
            {
                case Location.Top:
                    return screen.WorkingArea.Top;
                case Location.Bottom:
                    return screen.Bounds.Bottom - screen.WorkingArea.Bottom;
                case Location.Right:
                    return screen.Bounds.Right - screen.WorkingArea.Right;
                case Location.Left:
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
            (TaskBarInstance as ITaskBarList3)?.AddTab(hWnd);

        /// <summary>
        ///     Deletes an item from the taskbar.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window to be deleted from the taskbar.
        /// </param>
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static void DeleteTab(IntPtr hWnd) =>
            (TaskBarInstance as ITaskBarList3)?.DeleteTab(hWnd);

        /// <summary>
        ///     Provides static methods to manage a progress bar hosted in a taskbar button.
        /// </summary>
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static class Progress
        {
            /// <summary>
            ///     Provides enumerated options that control the current state of the progres
            ///     button.
            /// </summary>
            public enum Flags
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
            ///     Sets the type and state of the progress indicator displayed on a taskbar button.
            /// </summary>
            /// <param name="hWnd">
            ///     The handle of the window in which the progress of an operation is being shown.
            /// </param>
            /// <param name="flags">
            ///     The flag that control the current state of the progress button.
            /// </param>
            public static void SetState(IntPtr hWnd, Flags flags) =>
                (TaskBarInstance as ITaskBarList3)?.SetProgressState(hWnd, flags);

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
                (TaskBarInstance as ITaskBarList3)?.SetProgressValue(hWnd, (ulong)progressValue, (ulong)progressMax);
        }

        [ComImport]
        [Guid("EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ITaskBarList3
        {
            [PreserveSig]
            void HrInit();

            [PreserveSig]
            void AddTab(IntPtr hwnd);

            [PreserveSig]
            void DeleteTab(IntPtr hwnd);

            [PreserveSig]
            void ActivateTab(IntPtr hwnd);

            [PreserveSig]
            void SetActiveAlt(IntPtr hwnd);

            [PreserveSig]
            void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

            [PreserveSig]
            void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

            [PreserveSig]
            void SetProgressState(IntPtr hwnd, Progress.Flags tbpFlags);
        }

        [ComImport]
        [Guid("56FDF344-FD6D-11D0-958A-006097C9A090")]
        [ClassInterface(ClassInterfaceType.None)]
        private class TaskbarInstance { }
    }
}
