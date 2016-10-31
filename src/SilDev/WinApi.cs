#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: WinApi.cs
// Version:  2016-10-31 16:15
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    ///     An overkill class that provides a lot of Windows API (Application Programming Interface)
    ///     functions.
    /// </summary>
    public static class WinApi
    {
        /// <summary>
        ///     An application-defined callback function. It receives the child window handles. The
        ///     WNDENUMPROC type defines a pointer to this callback function. EnumChildProc is a
        ///     placeholder for the application-defined function name.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to a child window of the parent window.
        /// </param>
        /// <param name="lParam">
        ///     The application-defined value.
        /// </param>
        /// <returns>
        ///     To continue enumeration, the callback function must return TRUE; to stop enumeration,
        ///     it must return FALSE.
        /// </returns>
        public delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);

        /// <summary>
        ///     Represents a pointer to the hook procedure.
        /// </summary>
        /// <param name="nCode">
        ///     The hook code passed to the current hook procedure. The next hook procedure uses this
        ///     code to determine how to process the hook information.
        /// </param>
        /// <param name="wParam">
        ///     The wParam value passed to the current hook procedure. The meaning of this parameter
        ///     depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <param name="lParam">
        ///     The lParam value passed to the current hook procedure. The meaning of this parameter
        ///     depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is the handle to the hook procedure. If the
        ///     function fails, the return value is NULL.
        /// </returns>
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        ///     Defines a pointer to this callback function.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window associated with the timer.
        /// </param>
        /// <param name="uMsg">
        ///     The WM_TIMER (0x0113) message.
        /// </param>
        /// <param name="nIdEvent">
        ///     The timer's identifier.
        /// </param>
        /// <param name="dwTime">
        ///     The number of milliseconds that have elapsed since the system was started.
        /// </param>
        public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIdEvent, uint dwTime);

        /// <summary>
        ///     Provides enumerated values of process security and access rights.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum AccessRights : long
        {
            /// <summary>
            ///     Required to delete the object.
            /// </summary>
            DELETE = 0x10000,

            /// <summary>
            ///     Required to read information in the security descriptor for the object, not
            ///     including the information in the SACL.
            /// </summary>
            READ_CONTROL = 0x20000,

            /// <summary>
            ///     The right to use the object for synchronization. This enables a thread to wait
            ///     until the object is in the signaled state.
            /// </summary>
            SYNCHRONIZE = 0x100000,

            /// <summary>
            ///     Required to modify the DACL in the security descriptor for the object.
            /// </summary>
            WRITE_DAC = 0x40000,

            /// <summary>
            ///     Required to change the owner in the security descriptor for the object.
            /// </summary>
            WRITE_OWNER = 0x80000,

            /// <summary>
            ///     Required to create a process.
            /// </summary>
            PROCESS_CREATE_PROCESS = 0x80,

            /// <summary>
            ///     Required to create a thread.
            /// </summary>
            PROCESS_CREATE_THREAD = 0x2,

            /// <summary>
            ///     Required to duplicate a handle using
            ///     <see cref="UnsafeNativeMethods.DuplicateHandle(IntPtr, IntPtr, IntPtr, out IntPtr, uint, bool, uint)"/>
            /// </summary>
            PROCESS_DUP_HANDLE = 0x40,

            /// <summary>
            ///     Required to retrieve certain information about a process, such as its token,
            ///     exit code, and priority class.
            /// </summary>
            PROCESS_QUERY_INFORMATION = 0x400,

            /// <summary>
            ///     Required to retrieve certain information about a process.
            /// </summary>
            PROCESS_QUERY_LIMITED_INFORMATION = 0x1000,

            /// <summary>
            ///     Required to set certain information about a process, such as its priority class.
            /// </summary>
            PROCESS_SET_INFORMATION = 0x200,

            /// <summary>
            ///     Required to set memory limits using
            ///     <see cref="UnsafeNativeMethods.SetProcessWorkingSetSize(IntPtr, UIntPtr, UIntPtr)"/>.
            /// </summary>
            PROCESS_SET_QUOTA = 0x100,

            /// <summary>
            ///     Required to suspend or resume a process.
            /// </summary>
            PROCESS_SUSPEND_RESUME = 0x800,

            /// <summary>
            ///     Required to terminate a process using
            ///     <see cref="UnsafeNativeMethods.TerminateProcess(IntPtr, uint)"/>.
            /// </summary>
            PROCESS_TERMINATE = 0x1,

            /// <summary>
            ///     Required to perform an operation on the address space of a process.
            /// </summary>
            PROCESS_VM_OPERATION = 0x8,

            /// <summary>
            ///     Required to read memory in a process using
            ///     <see cref="UnsafeNativeMethods.ReadProcessMemory(IntPtr, IntPtr, IntPtr, IntPtr, ref IntPtr)"/>.
            /// </summary>
            PROCESS_VM_READ = 0x10,

            /// <summary>
            ///     Required to write to memory in a process using
            ///     <see cref="UnsafeNativeMethods.WriteProcessMemory(IntPtr, IntPtr, IntPtr, int, out IntPtr)"/>.
            /// </summary>
            PROCESS_VM_WRITE = 0x20
        }

        /// <summary>
        ///     Provides enumerated values of memory allocation.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum AllocationTypes : uint
        {
            /// <summary>
            ///     Allocates memory charges (from the overall size of memory and the paging files
            ///     on disk) for the specified reserved memory pages. The function also guarantees
            ///     that when the caller later initially accesses the memory, the contents will be
            ///     zero. Actual physical pages are not allocated unless/until the virtual
            ///     addresses are actually accessed.
            /// </summary>
            MEM_COMMIT = 0x1000,

            /// <summary>
            ///     Reserves a range of the process's virtual address space without allocating any
            ///     actual physical storage in memory or in the paging file on disk.
            /// </summary>
            MEM_RESERVE = 0x2000,

            /// <summary>
            ///     Decommits the specified region of committed pages. After the operation, the
            ///     pages are in the reserved state.
            /// </summary>
            MEM_DECOMMIT = 0x4000,

            /// <summary>
            ///     Releases the specified region of pages. After the operation, the pages are in
            ///     the free state.
            /// </summary>
            MEM_RELEASE = 0x8000,

            /// <summary>
            ///     Indicates that data in the memory range specified by lpAddress and dwSize is
            ///     no longer of interest. The pages should not be read from or written to the
            ///     paging file. However, the memory block will be used again later, so it should
            ///     not be decommitted. This value cannot be used with any other value.
            /// </summary>
            MEM_RESET = 0x80000,

            /// <summary>
            ///     Reserves an address range that can be used to map Address Windowing Extensions
            ///     (AWE) pages.
            /// </summary>
            MEM_PHYSICAL = 0x400000,

            /// <summary>
            ///     Allocates memory at the highest possible address. This can be slower than
            ///     regular allocations, especially when there are many allocations.
            /// </summary>
            MEM_TOP_DOWN = 0x100000,

            /// <summary>
            ///     Causes the system to track pages that are written to in the allocated region.
            ///     If you specify this value, you must also specify <see cref="MEM_RESERVE"/>.
            /// </summary>
            MEM_WRITE_WATCH = 0x200000,

            /// <summary>
            ///     Allocates memory using large page support.
            /// </summary>
            MEM_LARGE_PAGES = 0x20000000
        }

        /// <summary>
        ///     Provides enumerated values of appbar messages.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum AppBarMessageFunc
        {
            /// <summary>
            ///     Registers a new appbar and specifies the message identifier that the system
            ///     should use to send notification messages to the appbar
            /// </summary>
            ABM_NEW = 0x0,

            /// <summary>
            ///     Unregisters an appbar, removing the bar from the system's internal list.
            /// </summary>
            ABM_REMOVE = 0x1,

            /// <summary>
            ///     Requests a size and screen position for an appbar.
            /// </summary>
            ABM_QUERYPOS = 0x2,

            /// <summary>
            ///     Sets the size and screen position of an appbar.
            /// </summary>
            ABM_SETPOS = 0x3,

            /// <summary>
            ///     Retrieves the autohide and always-on-top states of the Windows taskbar.
            /// </summary>
            ABM_GETSTATE = 0x4,

            /// <summary>
            ///     Retrieves the bounding rectangle of the Windows taskbar. Note that this applies only
            ///     to the system taskbar. Other objects, particularly toolbars supplied with third-party
            ///     software, also can be present. As a result, some of the screen area not covered by the
            ///     Windows taskbar might not be visible to the user. To retrieve the area of the screen
            ///     not covered by both the taskbar and other app bars—the working area available to your
            ///     application—, use the GetMonitorInfo function.
            /// </summary>
            ABM_GETTASKBARPOS = 0x5,

            /// <summary>
            ///     Notifies the system to activate or deactivate an appbar. The lParam member of the
            ///     <see cref="APPBARDATA"/> pointed to by pData is set to TRUE to activate or FALSE to
            ///     deactivate.
            /// </summary>
            ABM_ACTIVATE = 0x6,

            /// <summary>
            ///     Retrieves the handle to the autohide appbar associated with a particular edge of the
            ///     screen.
            /// </summary>
            ABM_GETAUTOHIDEBAR = 0x7,

            /// <summary>
            ///     Registers or unregisters an autohide appbar for an edge of the screen.
            /// </summary>
            ABM_SETAUTOHIDEBAR = 0x8,

            /// <summary>
            ///     Notifies the system when an appbar's position has changed.
            /// </summary>
            ABM_WINDOWPOSCHANGED = 0x9,

            /// <summary>
            ///     Sets the state of the appbar's autohide and always-on-top attributes.
            /// </summary>
            ABM_SETSTATE = 0xa,

            /// <summary>
            ///     Retrieves the handle to the autohide appbar associated with a particular edge of a
            ///     particular monitor.
            /// </summary>
            ABM_GETAUTOHIDEBAREX = 0xb,

            /// <summary>
            ///     Registers or unregisters an autohide appbar for an edge of a particular monitor.
            /// </summary>
            ABM_SETAUTOHIDEBAREX = 0xc
        }

        /// <summary>
        ///     Provides enumerated values of handle duplication.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum DuplicateFuncOptions : uint
        {
            /// <summary>
            ///     Closes the source handle. This occurs regardless of any error status returned.
            /// </summary>
            DUPLICATE_CLOSE_SOURCE = 0x1,

            /// <summary>
            ///     Ignores the dwDesiredAccess parameter. The duplicate handle has the same access as the
            ///     source handle.
            /// </summary>
            DUPLICATE_SAME_ACCESS = 0x2
        }

        /// <summary>
        ///     Provides enumerated attributes of memory allocation.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum LocalAllocFuncAttr : uint
        {
            /// <summary>
            ///     Combines <see cref="LMEM_MOVEABLE"/> and <see cref="LMEM_ZEROINIT"/>.
            /// </summary>
            LHND = LMEM_MOVEABLE | LMEM_ZEROINIT,

            /// <summary>
            ///     Allocates fixed memory. The return value is a pointer to the memory object.
            /// </summary>
            LMEM_FIXED = 0x0,

            /// <summary>
            ///     Allocates movable memory. Memory blocks are never moved in physical memory,
            ///     but they can be moved within the default heap.
            /// </summary>
            LMEM_MOVEABLE = 0x2,

            /// <summary>
            ///     Initializes memory contents to zero.
            /// </summary>
            LMEM_ZEROINIT = 0x40,

            /// <summary>
            ///     Combines <see cref="LMEM_FIXED"/> and <see cref="LMEM_ZEROINIT"/>.
            /// </summary>
            LPTR = LMEM_FIXED | LMEM_ZEROINIT,

            /// <summary>
            ///     Same as <see cref="LMEM_MOVEABLE"/>.
            /// </summary>
            NONZEROLHND = LMEM_MOVEABLE,

            /// <summary>
            ///     Same as <see cref="LMEM_FIXED"/>.
            /// </summary>
            NONZEROLPTR = LMEM_FIXED
        }

        /// <summary>
        ///     Provides enumerated free type values of memory allocation.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum MemFreeTypes : uint
        {
            /// <summary>
            ///     Decommits the specified region of committed pages. After the operation, the
            ///     pages are in the reserved state.
            /// </summary>
            MEM_DECOMMIT = 0x4000,

            /// <summary>
            ///     Releases the specified region of pages. After the operation, the pages are in
            ///     the free state.
            /// </summary>
            MEM_RELEASE = 0x8000
        }

        /// <summary>
        ///     Provides enumerated values of menu items.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum ModifyMenuFunc : uint
        {
            /// <summary>
            ///     Indicates that the uPosition parameter gives the identifier of the menu item.
            ///     The <see cref="MF_BYCOMMAND"/> flag is the default if neither the
            ///     <see cref="MF_BYCOMMAND"/> nor <see cref="MF_BYPOSITION"/> flag is specified.
            /// </summary>
            MF_BYCOMMAND = 0x0,

            /// <summary>
            ///     Indicates that the uPosition parameter gives the zero-based relative position
            ///     of the menu item.
            /// </summary>
            MF_BYPOSITION = 0x4,

            /// <summary>
            ///     Uses a bitmap as the menu item. The lpNewItem parameter contains a handle to
            ///     the bitmap.
            /// </summary>
            MF_BITMAP = 0x4,

            /// <summary>
            ///     Places a check mark next to the item. If your application provides check-mark
            ///     bitmaps (see the SetMenuItemBitmaps function), this flag displays a selected
            ///     bitmap next to the menu item.
            /// </summary>
            MF_CHECKED = 0x8,

            /// <summary>
            ///     Disables the menu item so that it cannot be selected, but this flag does not
            ///     gray it.
            /// </summary>
            MF_DISABLED = 0x2,

            /// <summary>
            ///     Enables the menu item so that it can be selected and restores it from its
            ///     grayed state.
            /// </summary>
            MF_ENABLED = 0x0,

            /// <summary>
            ///     Disables the menu item and grays it so that it cannot be selected.
            /// </summary>
            MF_GRAYED = 0x1,

            /// <summary>
            ///     Functions the same as the MF_MENUBREAK flag for a menu bar. For a drop-down
            ///     menu, submenu, or shortcut menu, the new column is separated from the old
            ///     column by a vertical line.
            /// </summary>
            MF_MENUBARBREAK = 0x2,

            /// <summary>
            ///     Places the item on a new line (for menu bars) or in a new column (for a
            ///     drop-down menu, submenu, or shortcut menu) without separating columns.
            /// </summary>
            MF_MENUBREAK = 0x4,

            /// <summary>
            ///     Specifies that the item is an owner-drawn item. Before the menu is displayed
            ///     for the first time, the window that owns the menu receives a WM_MEASUREITEM
            ///     message to retrieve the width and height of the menu item. The WM_DRAWITEM
            ///     message is then sent to the window procedure of the owner window whenever
            ///     the appearance of the menu item must be updated.
            /// </summary>
            MF_OWNERDRAW = 0x1,

            /// <summary>
            ///     Specifies that the menu item opens a drop-down menu or submenu. The uIDNewItem
            ///     parameter specifies a handle to the drop-down menu or submenu. This flag is
            ///     used to add a menu name to a menu bar or a menu item that opens a submenu to a
            ///     drop-down menu, submenu, or shortcut menu.
            /// </summary>
            MF_POPUP = 0x1,

            /// <summary>
            ///     Draws a horizontal dividing line. This flag is used only in a drop-down menu,
            ///     submenu, or shortcut menu. The line cannot be grayed, disabled, or highlighted.
            ///     The lpNewItem and uIDNewItem parameters are ignored.
            /// </summary>
            MF_SEPARATOR = 0x8,

            /// <summary>
            ///     Specifies that the menu item is a text string; the lpNewItem parameter is a
            ///     pointer to the string.
            /// </summary>
            MF_STRING = 0x0,

            /// <summary>
            ///     Does not place a check mark next to the item (the default). If your application
            ///     supplies check-mark bitmaps (see the SetMenuItemBitmaps function), this flag
            ///     displays a clear bitmap next to the menu item.
            /// </summary>
            MF_UNCHECKED = 0x0,

            /// <summary>
            ///     Remove uPosition parameters.
            /// </summary>
            MF_REMOVE = 0x1
        }

        /// <summary>
        ///     Provides enumerated values of the process information class.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum ProcessInfoFunc : uint
        {
            /// <summary>
            ///     Retrieves a pointer to a PEB structure that can be used to determine whether the
            ///     specified process is being debugged, and a unique value used by the system to identify
            ///     the specified process.
            /// </summary>
            ProcessBasicInformation = 0x0,

            /// <summary>
            ///     Retrieves a DWORD_PTR value that is the port number of the debugger for the process. A
            ///     nonzero value indicates that the process is being run under the control of a ring 3
            ///     debugger.
            /// </summary>
            ProcessDebugPort = 0x7,

            /// <summary>
            ///     Determines whether the process is running in the WOW64 environment (WOW64 is the x86
            ///     emulator that allows Win32-based applications to run on 64-bit Windows).
            /// </summary>
            ProcessWow64Information = 0x1a,

            /// <summary>
            ///     Retrieves a <see cref="string"/> value containing the name of the image file for the
            ///     process.
            /// </summary>
            ProcessImageFileName = 0x1b,

            /// <summary>
            ///     Retrieves a <see cref="ulong"/> value indicating whether the process is considered
            ///     critical.
            /// </summary>
            ProcessBreakOnTermination = 0x1d
        }

        /// <summary>
        ///     Provides enumerated values of service errors.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum ServiceError
        {
            /// <summary>
            ///     The startup program logs the error in the event log, if possible. If the last-known-good
            ///     configuration is being started, the startup operation fails. Otherwise, the system is
            ///     restarted with the last-known good configuration.
            /// </summary>
            SERVICE_ERROR_CRITICAL = 0x3,

            /// <summary>
            ///     The startup program ignores the error and continues the startup operation.
            /// </summary>
            SERVICE_ERROR_IGNORE = 0x0,

            /// <summary>
            ///     The startup program logs the error in the event log but continues the startup operation.
            /// </summary>
            SERVICE_ERROR_NORMAL = 0x1,

            /// <summary>
            ///     The startup program logs the error in the event log. If the last-known-good configuration is
            ///     being started, the startup operation continues. Otherwise, the system is restarted with the
            ///     last-known-good configuration.
            /// </summary>
            SERVICE_ERROR_SEVERE = 0x2
        }

        /// <summary>
        ///     Provides enumerated attributes of memory allocation.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum SetWindowPosFunc : uint
        {
            /// <summary>
            ///     If the calling thread and the thread that owns the window are attached to different input
            ///     queues, the system posts the request to the thread that owns the window. This prevents the
            ///     calling thread from blocking its execution while other threads process the request.
            /// </summary>
            SWP_ASYNCWINDOWPOS = 0x4000,

            /// <summary>
            ///     Prevents generation of the WM_SYNCPAINT message.
            /// </summary>
            SWP_DEFERERASE = 0x2000,

            /// <summary>
            ///     Draws a frame (defined in the window's class description) around the window.
            /// </summary>
            SWP_DRAWFRAME = 0x20,

            /// <summary>
            ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message
            ///     to the window, even if the window's size is not being changed. If this flag is not specified,
            ///     WM_NCCALCSIZE is sent only when the window's size is being changed.
            /// </summary>
            SWP_FRAMECHANGED = 0x20,

            /// <summary>
            ///     Hides the window.
            /// </summary>
            SWP_HIDEWINDOW = 0x80,

            /// <summary>
            ///     Does not activate the window. If this flag is not set, the window is activated and moved to
            ///     the top of either the topmost or non-topmost group (depending on the setting of the
            ///     hWndInsertAfter parameter).
            /// </summary>
            SWP_NOACTIVATE = 0x10,

            /// <summary>
            ///     Discards the entire contents of the client area. If this flag is not specified, the valid
            ///     contents of the client area are saved and copied back into the client area after the window
            ///     is sized or repositioned.
            /// </summary>
            SWP_NOCOPYBITS = 0x100,

            /// <summary>
            ///     Retains the current position (ignores X and Y parameters).
            /// </summary>
            SWP_NOMOVE = 0x2,

            /// <summary>
            ///     Does not change the owner window's position in the Z order.
            /// </summary>
            SWP_NOOWNERZORDER = 0x200,

            /// <summary>
            ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This
            ///     applies to the client area, the nonclient area (including the title bar and scroll bars),
            ///     and any part of the parent window uncovered as a result of the window being moved. When this
            ///     flag is set, the application must explicitly invalidate or redraw any parts of the window and
            ///     parent window that need redrawing.
            /// </summary>
            SWP_NOREDRAW = 0x8,

            /// <summary>
            ///     Same as the <see cref="SetWindowPosFunc.SWP_NOOWNERZORDER"/> flag.
            /// </summary>
            SWP_NOREPOSITION = 0x200,

            /// <summary>
            ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING (0x0046) message.
            /// </summary>
            SWP_NOSENDCHANGING = 0x400,

            /// <summary>
            ///     Retains the current size (ignores the cx and cy parameters).
            /// </summary>
            SWP_NOSIZE = 0x1,

            /// <summary>
            ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOZORDER = 0x4,

            /// <summary>
            ///     Displays the window.
            /// </summary>
            SWP_SHOWWINDOW = 0x40
        }

        /// <summary>
        ///     Provides enumerated values of window's show statements.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum ShowWindowFunc : uint
        {
            /// <summary>
            ///     Minimizes a window, even if the thread that owns the window is not responding.
            ///     This flag should only be used when minimizing windows from a different thread.
            /// </summary>
            SW_FORCEMINIMIZE = 0xb,

            /// <summary>
            ///     Hides the window and activates another window.
            /// </summary>
            SW_HIDE = 0x0,

            /// <summary>
            ///     Maximizes the specified window.
            /// </summary>
            SW_MAXIMIZE = 0x3,

            /// <summary>
            ///     Minimizes the specified window and activates the next top-level window in the
            ///     Z order.
            /// </summary>
            SW_MINIMIZE = 0x6,

            /// <summary>
            ///     Activates and displays the window. If the window is minimized or maximized,
            ///     the system restores it to its original size and position. An application
            ///     should specify this flag when restoring a minimized window.
            /// </summary>
            SW_RESTORE = 0x9,

            /// <summary>
            ///     Activates the window and displays it in its current size and position.
            /// </summary>
            SW_SHOW = 0x5,

            /// <summary>
            ///     Sets the show state based on the SW_ value specified in the STARTUPINFO
            ///     structure passed to the CreateProcess function by the program that started
            ///     the application.
            /// </summary>
            SW_SHOWDEFAULT = 0xa,

            /// <summary>
            ///     Activates the window and displays it as a maximized window.
            /// </summary>
            SW_SHOWMAXIMIZED = 0x3,

            /// <summary>
            ///     Activates the window and displays it as a minimized window.
            /// </summary>
            SW_SHOWMINIMIZED = 0x2,

            /// <summary>
            ///     Displays the window as a minimized window. This value is similar to
            ///     SW_SHOWMINIMIZED, except the window is not activated.
            /// </summary>
            SW_SHOWMINNOACTIVE = 0x7,

            /// <summary>
            ///     Displays the window in its current size and position. This value is similar to
            ///     SW_SHOW, except that the window is not activated.
            /// </summary>
            SW_SHOWNA = 0x8,

            /// <summary>
            ///     Displays a window in its most recent size and position. This value is similar
            ///     to SW_SHOWNORMAL, except that the window is not activated.
            /// </summary>
            SW_SHOWNOACTIVATE = 0x4,

            /// <summary>
            ///     Activates and displays a window. If the window is minimized or maximized, the
            ///     system restores it to its original size and position. An application should
            ///     specify this flag when displaying the window for the first time.
            /// </summary>
            SW_SHOWNORMAL = 0x1
        }

        /// <summary>
        ///     Provides enumerated values of standard access rights.
        ///     <para>
        ///         Each type of securable object has a set of access rights that correspond to
        ///         operations specific to that type of object. In addition to these object-specific
        ///         access rights, there is a set of standard access rights that correspond to
        ///         operations common to most types of securable objects.
        ///     </para>
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum StandardAccessRights : long
        {
            /// <summary>
            ///     The right to delete the object.
            /// </summary>
            DELETE = 0x10000,

            /// <summary>
            ///     The right to read the information in the object's security descriptor, not
            ///     including the information in the system access control list (SACL).
            /// </summary>
            READ_CONTROL = 0x20000,

            /// <summary>
            ///     The right to use the object for synchronization. This enables a thread to wait
            ///     until the object is in the signaled state. Some object types do not support
            ///     this access right.
            /// </summary>
            SYNCHRONIZE = 0x100000,

            /// <summary>
            ///     The right to modify the discretionary access control list (DACL) in the object's
            ///     security descriptor.
            /// </summary>
            WRITE_DAC = 0x40000,

            /// <summary>
            ///     The right to change the owner in the object's security descriptor.
            /// </summary>
            WRITE_OWNER = 0x80000,

            /// <summary>
            ///     Combines DELETE, READ_CONTROL, WRITE_DAC, and WRITE_OWNER access.
            /// </summary>
            STANDARD_RIGHTS_ALL = DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER,

            /// <summary>
            ///     Currently defined to equal READ_CONTROL.
            /// </summary>
            STANDARD_RIGHTS_EXECUTE = READ_CONTROL,

            /// <summary>
            ///     Currently defined to equal READ_CONTROL.
            /// </summary>
            STANDARD_RIGHTS_READ = READ_CONTROL,

            /// <summary>
            ///     Combines DELETE, READ_CONTROL, WRITE_DAC, and WRITE_OWNER access.
            /// </summary>
            STANDARD_RIGHTS_REQUIRED = DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER,

            /// <summary>
            ///     Currently defined to equal READ_CONTROL.
            /// </summary>
            STANDARD_RIGHTS_WRITE = READ_CONTROL
        }

        /// <summary>
        ///     Provides enumerated constants of memory protection.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum VirtualAllocFuncMemProtect : uint
        {
            /// <summary>
            ///     Enables execute access to the committed region of pages. An attempt to write
            ///     to the committed region results in an access violation.
            /// </summary>
            PAGE_EXECUTE = 0x10,

            /// <summary>
            ///     Enables execute or read-only access to the committed region of pages. An
            ///     attempt to write to the committed region results in an access violation.
            /// </summary>
            PAGE_EXECUTE_READ = 0x20,

            /// <summary>
            ///     Enables execute, read-only, or read/write access to the committed region of
            ///     pages.
            /// </summary>
            PAGE_EXECUTE_READWRITE = 0x40,

            /// <summary>
            ///     Enables execute, read-only, or copy-on-write access to a mapped view of a file
            ///     mapping object. An attempt to write to a committed copy-on-write page results
            ///     in a private copy of the page being made for the process. The private page is
            ///     marked as <see cref="PAGE_EXECUTE_READWRITE"/>, and the change is written to
            ///     the new page.
            /// </summary>
            PAGE_EXECUTE_WRITECOPY = 0x80,

            /// <summary>
            ///     Disables all access to the committed region of pages. An attempt to read from,
            ///     write to, or execute the committed region results in an access violation.
            /// </summary>
            PAGE_NOACCESS = 0x1,

            /// <summary>
            ///     Enables read-only access to the committed region of pages. An attempt to write
            ///     to the committed region results in an access violation. If Data Execution
            ///     Prevention is enabled, an attempt to execute code in the committed region
            ///     results in an access violation.
            /// </summary>
            PAGE_READONLY = 0x2,

            /// <summary>
            ///     Enables read-only or read/write access to the committed region of pages. If
            ///     Data Execution Prevention is enabled, attempting to execute code in the
            ///     committed region results in an access violation.
            /// </summary>
            PAGE_READWRITE = 0x4,

            /// <summary>
            ///     Enables read-only or copy-on-write access to a mapped view of a file mapping
            ///     object. An attempt to write to a committed copy-on-write page results in a
            ///     private copy of the page being made for the process. The private page is
            ///     marked as <see cref="PAGE_READWRITE"/>, and the change is written to the new
            ///     page. If Data Execution Prevention is enabled, attempting to execute code in
            ///     the committed region results in an access violation.
            /// </summary>
            PAGE_WRITECOPY = 0x8,

            /// <summary>
            ///     Sets all locations in the pages as invalid targets for CFG. Used along with
            ///     any execute page protection like <see cref="PAGE_EXECUTE"/>,
            ///     <see cref="PAGE_EXECUTE_READ"/>, <see cref="PAGE_EXECUTE_READWRITE"/> and
            ///     <see cref="PAGE_EXECUTE_WRITECOPY"/>. Any indirect call to locations in those
            ///     pages will fail CFG checks and the process will be terminated. The default
            ///     behavior for executable pages allocated is to be marked valid call targets
            ///     for CFG.
            /// </summary>
            PAGE_TARGETS_INVALID = 0x40000000,

            /// <summary>
            ///     Pages in the region will not have their CFG information updated while the
            ///     protection changes for VirtualProtect. For example, if the pages in the region
            ///     was allocated using <see cref="PAGE_TARGETS_INVALID"/>, then the invalid
            ///     information will be maintained while the page protection changes. This flag is
            ///     only valid when the protection changes to an executable type like
            ///     <see cref="PAGE_EXECUTE"/>, <see cref="PAGE_EXECUTE_READ"/>,
            ///     <see cref="PAGE_EXECUTE_READWRITE"/> and <see cref="PAGE_EXECUTE_WRITECOPY"/>.
            ///     The default behavior for VirtualProtect protection change to executable is to
            ///     mark all locations as valid call targets for CFG.
            /// </summary>
            PAGE_TARGETS_NO_UPDATE = 0x40000000,

            /// <summary>
            ///     Pages in the region become guard pages. Any attempt to access a guard page
            ///     causes the system to raise a STATUS_GUARD_PAGE_VIOLATION (0x80000001) exception
            ///     and turn off the guard page status. Guard pages thus act as a one-time access
            ///     alarm.
            /// </summary>
            PAGE_GUARD = 0x100,

            /// <summary>
            ///     Sets all pages to be non-cachable. Applications should not use this attribute
            ///     except when explicitly required for a device. Using the interlocked functions
            ///     with memory that is mapped with <see cref="PAGE_NOCACHE"/> can result
            ///     in an <see cref="ExternalException"/>.
            /// </summary>
            PAGE_NOCACHE = 0x200,

            /// <summary>
            ///     Sets all pages to be write-combined.
            /// </summary>
            PAGE_WRITECOMBINE = 0x400
        }

        /// <summary>
        ///     Provides enumerated hook constants.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Win32HookFunc
        {
            /// <summary>
            ///     The system is about to activate a window.
            /// </summary>
            HCBT_ACTIVATE = 0x5,

            /// <summary>
            ///     The system has removed a mouse message from the system message queue. Upon
            ///     receiving this hook code, a CBT application must install a WH_JOURNALPLAYBACK
            ///     hook procedure in response to the mouse message.
            /// </summary>
            HCBT_CLICKSKIPPED = 0x6,

            /// <summary>
            ///     A window is about to be created. The system calls the hook procedure before
            ///     sending the WM_CREATE or WM_NCCREATE message to the window. If the hook
            ///     procedure returns a nonzero value, the system destroys the window; the
            ///     CreateWindow function returns NULL, but the WM_DESTROY message is not sent to
            ///     the window. If the hook procedure returns zero, the window is created normally.
            ///     At the time of the HCBT_CREATEWND notification, the window has been created,
            ///     but its final size and position may not have been determined and its parent
            ///     window may not have been established. It is possible to send messages to the
            ///     newly created window, although it has not yet received WM_NCCREATE or WM_CREATE
            ///     messages. It is also possible to change the position in the z-order of the
            ///     newly created window by modifying the hwndInsertAfter member of the
            ///     CBT_CREATEWND structure.
            /// </summary>
            HCBT_CREATEWND = 0x3,

            /// <summary>
            ///     A window is about to be destroyed.
            /// </summary>
            HCBT_DESTROYWND = 0x4,

            /// <summary>
            ///     The system has removed a keyboard message from the system message queue. Upon
            ///     receiving this hook code, a CBT application must install a WH_JOURNALPLAYBACK
            ///     hook procedure in response to the keyboard message.
            /// </summary>
            HCBT_KEYSKIPPED = 0x7,

            /// <summary>
            ///     A window is about to be minimized or maximized.
            /// </summary>
            HCBT_MINMAX = 0x1,

            /// <summary>
            ///     A window is about to be moved or sized.
            /// </summary>
            HCBT_MOVESIZE = 0x0,

            /// <summary>
            ///     The system has retrieved a WM_QUEUESYNC message from the system message queue.
            /// </summary>
            HCBT_QS = 0x2,

            /// <summary>
            ///     A window is about to receive the keyboard focus.
            /// </summary>
            HCBT_SETFOCUS = 0x9,

            /// <summary>
            ///     A system command is about to be carried out. This allows a CBT application to
            ///     prevent task switching by means of hot keys.
            /// </summary>
            HCBT_SYSCOMMAND = 0x8,

            /// <summary>
            ///     The input event occurred in a message box or dialog box.
            /// </summary>
            MSGF_DIALOGBOX = 0x0,

            /// <summary>
            ///     The input event occurred in a menu.
            /// </summary>
            MSGF_MENU = 0x2,

            /// <summary>
            ///     The input event occurred in a scroll bar.
            /// </summary>
            MSGF_SCROLLBAR = 0x5,

            /// <summary>
            ///     The WH_CALLWNDPROC and WH_CALLWNDPROCRET hooks enable you to monitor messages
            ///     sent to window procedures. The system calls a WH_CALLWNDPROC hook procedure
            ///     before passing the message to the receiving window procedure, and calls the
            ///     WH_CALLWNDPROCRET hook procedure after the window procedure has processed the
            ///     message. The WH_CALLWNDPROCRET hook passes a pointer to a CWPRETSTRUCT structure
            ///     to the hook procedure. The structure contains the return value from the window
            ///     procedure that processed the message, as well as the message parameters
            ///     associated with the message. Subclassing the window does not work for messages
            ///     set between processes.
            /// </summary>
            WH_CALLWNDPROC = 0x4,

            /// <summary>
            ///     The WH_CALLWNDPROC and WH_CALLWNDPROCRET hooks enable you to monitor messages
            ///     sent to window procedures. The system calls a WH_CALLWNDPROC hook procedure
            ///     before passing the message to the receiving window procedure, and calls the
            ///     WH_CALLWNDPROCRET hook procedure after the window procedure has processed the
            ///     message. The WH_CALLWNDPROCRET hook passes a pointer to a CWPRETSTRUCT
            ///     structure to the hook procedure. The structure contains the return value from
            ///     the window procedure that processed the message, as well as the message
            ///     parameters associated with the message. Subclassing the window does not work
            ///     for messages set between processes.
            /// </summary>
            WH_CALLWNDPROCRET = 0xc,

            /// <summary>
            ///     The system calls a WH_CBT hook procedure before activating, creating, destroying,
            ///     minimizing, maximizing, moving, or sizing a window; before completing a system
            ///     command; before removing a mouse or keyboard event from the system message queue;
            ///     before setting the input focus; or before synchronizing with the system message
            ///     queue. The value the hook procedure returns determines whether the system allows
            ///     or prevents one of these operations. The WH_CBT hook is intended primarily for
            ///     computer-based training (CBT) applications.
            /// </summary>
            WH_CBT = 0x5,

            /// <summary>
            ///     The system calls a WH_DEBUG hook procedure before calling hook procedures
            ///     associated with any other hook in the system. You can use this hook to determine
            ///     whether to allow the system to call hook procedures associated with other types
            ///     of hooks.
            /// </summary>
            WH_DEBUG = 0x9,

            /// <summary>
            ///     The WH_FOREGROUNDIDLE hook enables you to perform low priority tasks during times
            ///     when its foreground thread is idle. The system calls a WH_FOREGROUNDIDLE hook
            ///     procedure when the application's foreground thread is about to become idle.
            /// </summary>
            WH_FOREGROUNDIDLE = 0xb,

            /// <summary>
            ///     The WH_GETMESSAGE hook enables an application to monitor messages about to be
            ///     returned by the GetMessage or PeekMessage function. You can use the WH_GETMESSAGE
            ///     hook to monitor mouse and keyboard input and other messages posted to the message
            ///     queue.
            /// </summary>
            WH_GETMESSAGE = 0x3,

            /// <summary>
            ///     The WH_HARDWARE hook enables you to monitor various hardware events.
            /// </summary>
            WH_HARDWARE = 0x8,

            /// <summary>
            ///     The WH_JOURNALPLAYBACK hook enables an application to insert messages into the
            ///     system message queue. You can use this hook to play back a series of mouse and
            ///     keyboard events recorded earlier by using WH_JOURNALRECORD. Regular mouse and
            ///     keyboard input is disabled as long as a WH_JOURNALPLAYBACK hook is installed. A
            ///     WH_JOURNALPLAYBACK hook is a global hook—it cannot be used as a thread-specific
            ///     hook. The WH_JOURNALPLAYBACK hook returns a time-out value. This value tells the
            ///     system how many milliseconds to wait before processing the current message from
            ///     the playback hook. This enables the hook to control the timing of the events it
            ///     plays back.
            /// </summary>
            WH_JOURNALPLAYBACK = 0x1,

            /// <summary>
            ///     The WH_JOURNALRECORD hook enables you to monitor and record input events.
            ///     Typically, you use this hook to record a sequence of mouse and keyboard events
            ///     to play back later by using WH_JOURNALPLAYBACK. The WH_JOURNALRECORD hook is a
            ///     global hook—it cannot be used as a thread-specific hook.
            /// </summary>
            WH_JOURNALRECORD = 0x0,

            /// <summary>
            ///     The WH_KEYBOARD hook enables an application to monitor message traffic for
            ///     WM_KEYDOWN and WM_KEYUP messages about to be returned by the GetMessage or
            ///     PeekMessage function. You can use the WH_KEYBOARD hook to monitor keyboard
            ///     input posted to a message queue.
            /// </summary>
            WH_KEYBOARD = 0x2,

            /// <summary>
            ///     The WH_KEYBOARD_LL hook enables you to monitor keyboard input events about to be
            ///     posted in a thread input queue.
            /// </summary>
            WH_KEYBOARD_LL = 0xd,

            /// <summary>
            ///     The WH_MOUSE hook enables you to monitor mouse messages about to be returned by
            ///     the GetMessage or PeekMessage function. You can use the WH_MOUSE hook to monitor
            ///     mouse input posted to a message queue.
            /// </summary>
            WH_MOUSE = 0x7,

            /// <summary>
            ///     The WH_MOUSE_LL hook enables you to monitor mouse input events about to be posted
            ///     in a thread input queue.
            /// </summary>
            WH_MOUSE_LL = 0xe,

            /// <summary>
            ///     The WH_MSGFILTER and WH_SYSMSGFILTER hooks enable you to monitor messages about
            ///     to be processed by a menu, scroll bar, message box, or dialog box, and to detect
            ///     when a different window is about to be activated as a result of the user's pressing
            ///     the ALT+TAB or ALT+ESC key combination. The WH_MSGFILTER hook can only monitor
            ///     messages passed to a menu, scroll bar, message box, or dialog box created by the
            ///     application that installed the hook procedure. The WH_SYSMSGFILTER hook monitors
            ///     such messages for all applications.
            /// </summary>
            WH_MSGFILTER = -0x1,

            /// <summary>
            ///     A shell application can use the WH_SHELL hook to receive important notifications.
            ///     The system calls a WH_SHELL hook procedure when the shell application is about to
            ///     be activated and when a top-level window is created or destroyed. Note that custom
            ///     shell applications do not receive WH_SHELL messages. Therefore, any application
            ///     that registers itself as the default shell must call the SystemParametersInfo
            ///     function before it (or any other application) can receive WH_SHELL messages. This
            ///     function must be called with SPI_SETMINIMIZEDMETRICS and a MINIMIZEDMETRICS
            ///     structure. Set the iArrange member of this structure to ARW_HIDE.
            /// </summary>
            WH_SHELL = 0xa,

            /// <summary>
            ///     The WH_MSGFILTER and WH_SYSMSGFILTER hooks enable you to monitor messages about to
            ///     be processed by a menu, scroll bar, message box, or dialog box, and to detect when
            ///     a different window is about to be activated as a result of the user's pressing the
            ///     ALT+TAB or ALT+ESC key combination. The WH_MSGFILTER hook can only monitor messages
            ///     passed to a menu, scroll bar, message box, or dialog box created by the application
            ///     that installed the hook procedure. The WH_SYSMSGFILTER hook monitors such messages
            ///     for all applications. The WH_MSGFILTER and WH_SYSMSGFILTER hooks enable you to
            ///     perform message filtering during modal loops that is equivalent to the filtering
            ///     done in the main message loop. For example, an application often examines a new
            ///     message in the main loop between the time it retrieves the message from the queue
            ///     and the time it dispatches the message, performing special processing as
            ///     appropriate. However, during a modal loop, the system retrieves and dispatches
            ///     messages without allowing an application the chance to filter the messages in its
            ///     main message loop. If an application installs a WH_MSGFILTER or WH_SYSMSGFILTER
            ///     hook procedure, the system calls the procedure during the modal loop.
            /// </summary>
            WH_SYSMSGFILTER = 0x6
        }

        /// <summary>
        ///     Provides enumerated attribute values of window's statements.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum WindowLongFunc
        {
            /// <summary>
            ///     Retrieves the address of the dialog box procedure, or a handle representing the
            ///     address of the dialog box procedure. You must use the CallWindowProc function to
            ///     call the dialog box procedure.
            /// </summary>
            DWL_DLGPROC = 0x4,

            /// <summary>
            ///     Retrieves the return value of a message processed in the dialog box procedure.
            /// </summary>
            DWL_MSGRESULT = 0x0,

            /// <summary>
            ///     Retrieves extra information private to the application, such as handles or pointers.
            /// </summary>
            DWL_USER = 0x8,

            /// <summary>
            ///     Sets a new extended window style.
            /// </summary>
            GWL_EXSTYLE = -0x2,

            /// <summary>
            ///     Sets a new application instance handle.
            /// </summary>
            GWL_HINSTANCE = -0x6,

            /// <summary>
            ///     Sets a new identifier of the child window. The window cannot be a top-level window.
            /// </summary>
            GWL_ID = -0xc,

            /// <summary>
            ///     Sets a new window style.
            /// </summary>
            GWL_STYLE = -0x10,

            /// <summary>
            ///     Sets the user data associated with the window. This data is intended for use by the
            ///     application that created the window. Its value is initially zero.
            /// </summary>
            GWL_USERDATA = -0x15,

            /// <summary>
            ///     Sets a new address for the window procedure. You cannot change this attribute if the
            ///     window does not belong to the same process as the calling thread.
            /// </summary>
            GWL_WNDPROC = -0x4
        }

        /// <summary>
        ///     Provides enumerated values of system command requests.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum WindowMenuFunc : uint
        {
            /// <summary>
            ///     Closes the window.
            /// </summary>
            SC_CLOSE = 0xf06,

            /// <summary>
            ///     Changes the cursor to a question mark with a pointer. If the user then clicks a
            ///     control in the dialog box, the control receives a WM_HELP message.
            /// </summary>
            SC_CONTEXTHELP = 0xf18,

            /// <summary>
            ///     Selects the default item; the user double-clicked the window menu.
            /// </summary>
            SC_DEFAULT = 0xf16,

            /// <summary>
            ///     Activates the window associated with the application-specified hot key. The lParam
            ///     parameter identifies the window to activate.
            /// </summary>
            SC_HOTKEY = 0xf15,

            /// <summary>
            ///     Scrolls horizontally.
            /// </summary>
            SC_HSCROLL = 0xf08,

            /// <summary>
            ///     Retrieves the window menu as a result of a keystroke.
            /// </summary>
            SC_KEYMENU = 0xf1,

            /// <summary>
            ///     Maximizes the window.
            /// </summary>
            SC_MAXIMIZE = 0xf03,

            /// <summary>
            ///     Minimizes the window.
            /// </summary>
            SC_MINIMIZE = 0xf02,

            /// <summary>
            ///     Sets the state of the display. This command supports devices that have power-saving
            ///     features, such as a battery-powered personal computer. - The lParam parameter can
            ///     have the following values: -1 (the display is powering on), 1 (the display is going
            ///     to low power), 2 (the display is being shut off).
            /// </summary>
            SC_MONITORPOWER = 0xf17,

            /// <summary>
            ///     Retrieves the window menu as a result of a mouse click.
            /// </summary>
            SC_MOUSEMENU = 0xf09,

            /// <summary>
            ///     Moves the window.
            /// </summary>
            SC_MOVE = 0xf01,

            /// <summary>
            ///     Moves to the next window.
            /// </summary>
            SC_NEXTWINDOW = 0xf04,

            /// <summary>
            ///     Moves to the previous window.
            /// </summary>
            SC_PREVWINDOW = 0xf05,

            /// <summary>
            ///     Restores the window to its normal position and size.
            /// </summary>
            SC_RESTORE = 0xf12,

            /// <summary>
            ///     Executes the screen saver application specified in the [boot] section of the System.ini file.
            /// </summary>
            SC_SCREENSAVE = 0xf14,

            /// <summary>
            ///     Sizes the window.
            /// </summary>
            SC_SIZE = 0xf,

            /// <summary>
            ///     Activates the Start menu.
            /// </summary>
            SC_TASKLIST = 0xf13,

            /// <summary>
            ///     Scrolls vertically.
            /// </summary>
            SC_VSCROLL = 0xf07,

            /// <summary>
            ///     Indicates whether the screen saver is secure.
            /// </summary>
            SCF_ISSECURE = 0x1,

            /// <summary>
            ///     If the receiving application processes this message, it should return TRUE; otherwise, it
            ///     should return FALSE. The data being passed must not contain pointers or other references to
            ///     objects not accessible to the application receiving the data. While this message is being
            ///     sent, the referenced data must not be changed by another thread of the sending process. The
            ///     receiving application should consider the data read-only. The lParam parameter is valid only
            ///     during the processing of the message. The receiving application should not free the memory
            ///     referenced by lParam. If the receiving application must access the data after SendMessage
            ///     returns, it must copy the data into a local buffer.
            /// </summary>
            WM_COPYDATA = 0x4a,

            /// <summary>
            ///     The dialog box procedure should return TRUE to direct the system to set the keyboard focus to
            ///     the control specified by wParam. Otherwise, it should return FALSE to prevent the system from
            ///     setting the default keyboard focus.
            /// </summary>
            WM_INITDIALOG = 0x110,

            /// <summary>
            ///     Posted to a window when the cursor moves. If the mouse is not captured, the message is posted
            ///     to the window that contains the cursor. Otherwise, the message is posted to the window that
            ///     has captured the mouse.
            /// </summary>
            WM_MOUSEMOVE = 0x2,

            /// <summary>
            ///     A message that is sent to all top-level windows when the SystemParametersInfo  function changes
            ///     a system-wide setting or when policy settings have changed.
            /// </summary>
            WM_SETTINGCHANGE = 0x1a,

            /// <summary>
            ///     A window receives this message when the user chooses a command from the Window menu (formerly
            ///     known as the system or control menu) or when the user chooses the maximize button, minimize
            ///     button, restore button, or close button.
            /// </summary>
            WM_SYSCOMMAND = 0x112
        }

        /// <summary>
        ///     Provides enumerated flags of window placements.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum WindowPlacement : uint
        {
            /// <summary>
            ///     If the calling thread and the thread that owns the window are attached to different
            ///     input queues, the system posts the request to the thread that owns the window. This
            ///     prevents the calling thread from blocking its execution while other threads process
            ///     the request.
            /// </summary>
            WPF_ASYNCWINDOWPLACEMENT = 0x4,

            /// <summary>
            ///     The restored window will be maximized, regardless of whether it was maximized before it
            ///     was minimized. This setting is only valid the next time the window is restored. It does
            ///     not change the default restoration behavior.
            ///     <para>
            ///         This flag is only valid when the <see cref="ShowWindowFunc.SW_SHOWMINIMIZED"/> value
            ///         is specified for the showCmd member.
            ///     </para>
            /// </summary>
            WPF_RESTORETOMAXIMIZED = 0x2,

            /// <summary>
            ///     The coordinates of the minimized window may be specified.
            ///     <para>
            ///         This flag must be specified if the coordinates are set in the ptMinPosition member.
            ///     </para>
            /// </summary>
            WPF_SETMINPOSITION = 0x1
        }

        /// <summary>
        ///     Provides enumerated values of window styles.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum WindowStyleFunc : ulong
        {
            /// <summary>
            ///     The window has a thin-line border.
            /// </summary>
            WS_BORDER = 0x8,

            /// <summary>
            ///     The window has a title bar (includes the <see cref="WS_BORDER"/> style).
            /// </summary>
            WS_CAPTION = 0xc,

            /// <summary>
            ///     The window is a child window. A window with this style cannot have a menu bar. This style cannot
            ///     be used with the WS_POPUP style.
            /// </summary>
            WS_CHILD = 0x4,

            /// <summary>
            ///     Same as the WS_CHILD style.
            /// </summary>
            WS_CHILDWINDOW = WS_CHILD,

            /// <summary>
            ///     Excludes the area occupied by child windows when drawing occurs within the parent window. This
            ///     style is used when creating the parent window.
            /// </summary>
            WS_CLIPCHILDREN = 0x2,

            /// <summary>
            ///     Clips child windows relative to each other; that is, when a particular child window receives a
            ///     WM_PAINT message, the <see cref="WS_CLIPSIBLINGS"/> style clips all other overlapping child
            ///     windows out of the region of the child window to be updated. If <see cref="WS_CLIPSIBLINGS"/>
            ///     is not specified and child windows overlap, it is possible, when drawing within the client area
            ///     of a child window, to draw within the client area of a neighboring child window.
            /// </summary>
            WS_CLIPSIBLINGS = 0x4,

            /// <summary>
            ///     The window is initially disabled. A disabled window cannot receive input from the user. To change
            ///     this after a window has been created, use the EnableWindow function.
            /// </summary>
            WS_DISABLED = 0x8,

            /// <summary>
            ///     The window has a border of a style typically used with dialog boxes. A window with this style
            ///     cannot have a title bar.
            /// </summary>
            WS_DLGFRAME = 0x4,

            /// <summary>
            ///     The window is the first control of a group of controls. The group consists of this first control
            ///     and all controls defined after it, up to the next control with the WS_GROUP style. The first
            ///     control in each group usually has the <see cref="WS_TABSTOP"/> style so that the user can move
            ///     from group to group. The user can subsequently change the keyboard focus from one control in the
            ///     group to the next control in the group by using the direction keys.
            /// </summary>
            WS_GROUP = 0x2,

            /// <summary>
            ///     The window has a horizontal scroll bar.
            /// </summary>
            WS_HSCROLL = 0x1,

            /// <summary>
            ///     The window is initially minimized. Same as the <see cref="WS_MINIMIZE"/> style.
            /// </summary>
            WS_ICONIC = WS_MINIMIZE,

            /// <summary>
            ///     The window is initially maximized.
            /// </summary>
            WS_MAXIMIZE = 0x1,

            /// <summary>
            ///     The window has a maximize button. Cannot be combined with the <see cref="WS_EX_CONTEXTHELP"/> style.
            ///     The <see cref="WS_SYSMENU"/> style must also be specified.
            /// </summary>
            WS_MAXIMIZEBOX = 0x1,

            /// <summary>
            ///     The window is initially minimized. Same as the WS_ICONIC style.
            /// </summary>
            WS_MINIMIZE = 0x2,

            /// <summary>
            ///     The window has a minimize button. Cannot be combined with the <see cref="WS_EX_CONTEXTHELP"/> style.
            ///     The <see cref="WS_SYSMENU"/> style must also be specified.
            /// </summary>
            WS_MINIMIZEBOX = 0x2,

            /// <summary>
            ///     The window is an overlapped window. An overlapped window has a title bar and a border. Same
            ///     as the <see cref="WS_TILED"/> style.
            /// </summary>
            WS_OVERLAPPED = 0x0,

            /// <summary>
            ///     The window is an overlapped window. Same as the <see cref="WS_TILEDWINDOW"/> style.
            /// </summary>
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
#           if x64
            /// <summary>
            ///     The windows is a pop-up window. This style cannot be used with the WS_CHILD style.
            /// </summary>
            WS_POPUP = 0x80000000,

            /// <summary>
            ///     The window is a pop-up window. The <see cref="WS_CAPTION"/> and <see cref="WS_POPUPWINDOW"/> styles
            ///     must be combined to make the window menu visible.
            /// </summary>
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
#           endif

            /// <summary>
            ///     The window has a sizing border. Same as the <see cref="WS_THICKFRAME"/> style.
            /// </summary>
            WS_SIZEBOX = 0x4,

            /// <summary>
            ///     The window has a window menu on its title bar. The <see cref="WS_CAPTION"/> style must also be
            ///     specified.
            /// </summary>
            WS_SYSMENU = 0x8,

            /// <summary>
            ///     The window is a control that can receive the keyboard focus when the user presses the TAB key.
            ///     Pressing the TAB key changes the keyboard focus to the next control with the <see cref="WS_TABSTOP"/>
            ///     style. You can turn this style on and off to change dialog box navigation. To change this style after
            ///     a window has been created, use the SetWindowLong function. For user-created windows and modeless
            ///     dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
            /// </summary>
            WS_TABSTOP = 0x1,

            /// <summary>
            ///     The window has a sizing border. Same as the <see cref="WS_SIZEBOX"/> style.
            /// </summary>
            WS_THICKFRAME = 0x4,

            /// <summary>
            ///     The window is an overlapped window. An overlapped window has a title bar and a border. Same
            ///     as the <see cref="WS_OVERLAPPED"/> style.
            /// </summary>
            WS_TILED = 0x0,

            /// <summary>
            ///     The window is an overlapped window. Same as the <see cref="WS_OVERLAPPEDWINDOW"/> style.
            /// </summary>
            WS_TILEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

            /// <summary>
            ///     The window is initially visible. This style can be turned on and off by using the ShowWindow
            ///     or SetWindowPos function.
            /// </summary>
            WS_VISIBLE = 0x1,

            /// <summary>
            ///     The window has a vertical scroll bar.
            /// </summary>
            WS_VSCROLL = 0x2,

            /// <summary>
            ///     The window accepts drag-drop files.
            /// </summary>
            WS_EX_ACCEPTFILES = 0x1,

            /// <summary>
            ///     Forces a top-level window onto the taskbar when the window is visible.
            /// </summary>
            WS_EX_APPWINDOW = 0x4,

            /// <summary>
            ///     The window has a border with a sunken edge.
            /// </summary>
            WS_EX_CLIENTEDGE = 0x2,

            /// <summary>
            ///     Paints all descendants of a window in bottom-to-top painting order using double-buffering.
            /// </summary>
            WS_EX_COMPOSITED = 0x2,

            /// <summary>
            ///     The title bar of the window includes a question mark. When the user clicks the question mark,
            ///     the cursor changes to a question mark with a pointer. If the user then clicks a child window,
            ///     the child receives a WM_HELP message. The child window should pass the message to the parent
            ///     window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The
            ///     Help application displays a pop-up window that typically contains help for the child window.
            ///     <see cref="WS_EX_CONTEXTHELP"/> cannot be used with the <see cref="WS_MAXIMIZEBOX"/> or
            ///     <see cref="WS_MINIMIZEBOX"/> styles.
            /// </summary>
            WS_EX_CONTEXTHELP = 0x4,

            /// <summary>
            ///     The window itself contains child windows that should take part in dialog box navigation. If
            ///     this style is specified, the dialog manager recurses into children of this window when
            ///     performing navigation operations such as handling the TAB key, an arrow key, or a keyboard
            ///     mnemonic.
            /// </summary>
            WS_EX_CONTROLPARENT = 0x1,

            /// <summary>
            ///     The window has a double border; the window can, optionally, be created with a title bar by
            ///     specifying the <see cref="WS_CAPTION"/> style in the dwStyle parameter.
            /// </summary>
            WS_EX_DLGMODALFRAME = 0x1,

            /// <summary>
            ///     The window is a layered window. This style cannot be used if the window has a class style of
            ///     either CS_OWNDC or CS_CLASSDC.
            /// </summary>
            WS_EX_LAYERED = 0x8,

            /// <summary>
            ///     If the shell language is Hebrew, Arabic, or another language that supports reading order
            ///     alignment, the horizontal origin of the window is on the right edge. Increasing horizontal
            ///     values advance to the left.
            /// </summary>
            WS_EX_LAYOUTRTL = 0x4,

            /// <summary>
            ///     The window has generic left-aligned properties. This is the default.
            /// </summary>
            WS_EX_LEFT = 0x0,

            /// <summary>
            ///     If the shell language is Hebrew, Arabic, or another language that supports reading order
            ///     alignment, the vertical scroll bar (if present) is to the left of the client area. For
            ///     other languages, the style is ignored.
            /// </summary>
            WS_EX_LEFTSCROLLBAR = 0x4,

            /// <summary>
            ///     The window text is displayed using left-to-right reading-order properties. This is the
            ///     default.
            /// </summary>
            WS_EX_LTRREADING = 0x0,

            /// <summary>
            ///     The window is a MDI child window.
            /// </summary>
            WS_EX_MDICHILD = 0x4,

            /// <summary>
            ///     A top-level window created with this style does not become the foreground window when
            ///     the user clicks it. The system does not bring this window to the foreground when the
            ///     user minimizes or closes the foreground window. To activate the window, use the
            ///     SetActiveWindow or SetForegroundWindow function. The window does not appear on the
            ///     taskbar by default. To force the window to appear on the taskbar, use the
            ///     WS_EX_APPWINDOW style.
            /// </summary>
            WS_EX_NOACTIVATE = 0x8,

            /// <summary>
            ///     The window does not pass its window layout to its child windows.
            /// </summary>
            WS_EX_NOINHERITLAYOUT = 0x1,

            /// <summary>
            ///     The child window created with this style does not send the WM_PARENTNOTIFY message
            ///     to its parent window when it is created or destroyed.
            /// </summary>
            WS_EX_NOPARENTNOTIFY = 0x4,

            /// <summary>
            ///     The window does not render to a redirection surface. This is for windows that do not
            ///     have visible content or that use mechanisms other than surfaces to provide their visual.
            /// </summary>
            WS_EX_NOREDIRECTIONBITMAP = 0x2,

            /// <summary>
            ///     The window is an overlapped window.
            /// </summary>
            WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,

            /// <summary>
            ///     The window is palette window, which is a modeless dialog box that presents an array
            ///     of commands.
            /// </summary>
            WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,

            /// <summary>
            ///     The window has generic "right-aligned" properties. This depends on the window class.
            ///     This style has an effect only if the shell language is Hebrew, Arabic, or another
            ///     language that supports reading-order alignment; otherwise, the style is ignored.
            ///     Using the WS_EX_RIGHT style for static or edit controls has the same effect as using
            ///     the SS_RIGHT or ES_RIGHT style, respectively. Using this style with button controls
            ///     has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.
            /// </summary>
            WS_EX_RIGHT = 0x1,

            /// <summary>
            ///     The vertical scroll bar (if present) is to the right of the client area. This is the
            ///     default.
            /// </summary>
            WS_EX_RIGHTSCROLLBAR = 0x0,

            /// <summary>
            ///     If the shell language is Hebrew, Arabic, or another language that supports reading-order
            ///     alignment, the window text is displayed using right-to-left reading-order properties.
            ///     For other languages, the style is ignored.
            /// </summary>
            WS_EX_RTLREADING = 0x2,

            /// <summary>
            ///     The window has a three-dimensional border style intended to be used for items that do
            ///     not accept user input.
            /// </summary>
            WS_EX_STATICEDGE = 0x2,

            /// <summary>
            ///     The window is intended to be used as a floating toolbar. A tool window has a title bar
            ///     that is shorter than a normal title bar, and the window title is drawn using a smaller
            ///     font. A tool window does not appear in the taskbar or in the dialog that appears when
            ///     the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed
            ///     on the title bar. However, you can display the system menu by right-clicking or by
            ///     typing ALT+SPACE.
            /// </summary>
            WS_EX_TOOLWINDOW = 0x8,

            /// <summary>
            ///     The window should be placed above all non-topmost windows and should stay above them,
            ///     even when the window is deactivated. To add or remove this style, use the SetWindowPos
            ///     function.
            /// </summary>
            WS_EX_TOPMOST = 0x8,

            /// <summary>
            ///     The window should not be painted until siblings beneath the window (that were created
            ///     by the same thread) have been painted. The window appears transparent because the bits
            ///     of underlying sibling windows have already been painted. To achieve transparency without
            ///     these restrictions, use the SetWindowRgn function.
            /// </summary>
            WS_EX_TRANSPARENT = 0x2,

            /// <summary>
            ///     The window has a border with a raised edge.
            /// </summary>
            WS_EX_WINDOWEDGE = 0x1
        }

        /// <summary>
        ///     Throws the last error code returned by the last unmanaged function.
        /// </summary>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static void ThrowLastError()
        {
            var code = Marshal.GetLastWin32Error();
            if (code > 0)
                throw new Win32Exception(code);
        }

        /// <summary>
        ///     Sends the specified arguments to the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="args">
        ///     The arguments to send.
        /// </param>
        public static bool SendArgs(IntPtr hWnd, string args)
        {
            var cds = new COPYDATASTRUCT();
            try
            {
                cds.cbData = (args.Length + 1) * 2;
                cds.lpData = UnsafeNativeMethods.LocalAlloc(LocalAllocFuncAttr.LMEM_ZEROINIT, (UIntPtr)cds.cbData);
                Marshal.Copy(args.ToCharArray(), 0, cds.lpData, args.Length);
                cds.dwData = new IntPtr(1);
                UnsafeNativeMethods.SendMessage(hWnd, (int)WindowMenuFunc.WM_COPYDATA, IntPtr.Zero, ref cds);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
            finally
            {
                cds.Dispose();
            }
        }

        /// <summary>
        ///     Gets the current theme color of the operating system.
        /// </summary>
        /// <param name="alpha">
        ///     true to get also the alpha channel; otherwise, false.
        /// </param>
        public static Color GetSystemThemeColor(bool alpha = false)
        {
            try
            {
                DWM_COLORIZATION_PARAMS parameters;
                SafeNativeMethods.DwmGetColorizationParameters(out parameters);
                var color = Color.FromArgb(int.Parse(parameters.clrColor.ToString("X"), NumberStyles.HexNumber));
                if (!alpha)
                    color = Color.FromArgb(color.R, color.G, color.B);
                return color;
            }
            catch
            {
                return SystemColors.Highlight;
            }
        }

        /// <summary>
        ///     Refreshes the visible notification area of the taskbar.
        /// </summary>
        public static bool RefreshVisibleTrayArea()
        {
            try
            {
                var hWndTray = UnsafeNativeMethods.FindWindow("Shell_TrayWnd", null);
                if (hWndTray == IntPtr.Zero)
                    return false;
                foreach (var className in new[] { "TrayNotifyWnd", "SysPager", "ToolbarWindow32" })
                {
                    hWndTray = UnsafeNativeMethods.FindWindowEx(hWndTray, IntPtr.Zero, className, null);
                    if (hWndTray == IntPtr.Zero)
                        throw new ArgumentNullException(nameof(hWndTray));
                }
                Rectangle rect;
                UnsafeNativeMethods.GetClientRect(hWndTray, out rect);
                for (var x = 0; x < rect.Right; x += 5)
                    for (var y = 0; y < rect.Bottom; y += 5)
                        UnsafeNativeMethods.SendMessage(hWndTray, (uint)WindowMenuFunc.WM_MOUSEMOVE, IntPtr.Zero, (IntPtr)((y << 16) + x));
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Gets the window title from the foreground window (the window with which the user is currently working).
        /// </summary>
        public static string GetActiveWindowTitle()
        {
            var sb = new StringBuilder(256);
            var hWnd = UnsafeNativeMethods.GetForegroundWindow();
            return UnsafeNativeMethods.GetWindowText(hWnd, sb, 256) > 0 ? sb.ToString() : string.Empty;
        }

        /// <summary>
        ///     Retrieves a handle to the top-level window whose window name match the specified strings. This
        ///     function does not search child windows. This function does not perform a case-sensitive search.
        /// </summary>
        /// <param name="lpWindowName">
        ///     The window name (the window's title). If this parameter is NULL, all window names match.
        /// </param>
        public static IntPtr FindWindowByCaption(string lpWindowName) =>
            UnsafeNativeMethods.FindWindowByCaption(IntPtr.Zero, lpWindowName);

        /// <summary>
        ///     Gets basic process information about the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static PROCESS_BASIC_INFORMATION GetProcessBasicInformation(IntPtr hWnd)
        {
            PROCESS_BASIC_INFORMATION pbi;
            IntPtr retLen;
            var status = UnsafeNativeMethods.NtQueryInformationProcess(hWnd, 0, out pbi, (uint)Marshal.SizeOf(typeof(PROCESS_BASIC_INFORMATION)), out retLen);
            try
            {
                if (status >= 0xc0000000)
                    throw new ArgumentOutOfRangeException(nameof(status));
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return pbi;
        }

        /// <summary>
        ///     Gets information about the placement of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static WINDOWPLACEMENT GetWindowPlacement(IntPtr hWnd)
        {
            var placement = new WINDOWPLACEMENT();
            UnsafeNativeMethods.GetWindowPlacement(hWnd, ref placement);
            return placement;
        }

        /// <summary>
        ///     Changes the position and dimensions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="nRect">
        ///     The new position and size of the window.
        /// </param>
        public static void MoveWindow(IntPtr hWnd, Rectangle nRect)
        {
            var cRect = new Rectangle();
            UnsafeNativeMethods.GetWindowRect(hWnd, ref cRect);
            if (cRect == nRect)
                return;
            if (nRect.Width <= 0 || nRect.Height <= 0)
                nRect.Size = cRect.Size;
            UnsafeNativeMethods.MoveWindow(hWnd, nRect.X, nRect.Y, nRect.Width, nRect.Height, cRect.Size != nRect.Size);
        }

        /// <summary>
        ///     Changes the position and dimensions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="point">
        ///     The new position of the window.
        /// </param>
        /// <param name="size">
        ///     The new size of the window.
        /// </param>
        public static void MoveWindow(IntPtr hWnd, Point point, Size size) =>
            MoveWindow(hWnd, new Rectangle { Location = point, Size = size });

        /// <summary>
        ///     Changes the position of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="point">
        ///     The new position of the window.
        /// </param>
        public static void MoveWindow(IntPtr hWnd, Point point) =>
            MoveWindow(hWnd, new Rectangle { Location = point, Size = new Size(0, 0) });

        /// <summary>
        ///     Changes the position of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="x">
        ///     The new position of the left side of the window.
        /// </param>
        /// <param name="y">
        ///     The new position of the top of the window.
        /// </param>
        public static void MoveWindow(IntPtr hWnd, int x, int y) =>
            MoveWindow(hWnd, new Point(x, y));

        /// <summary>
        ///     Removes the borders and title bar of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static void RemoveWindowBorders(IntPtr hWnd)
        {
            var hMenu = UnsafeNativeMethods.GetMenu(hWnd);
            if (hMenu != IntPtr.Zero)
            {
                var count = UnsafeNativeMethods.GetMenuItemCount(hMenu);
                for (var i = 0; i < count; i++)
                    UnsafeNativeMethods.RemoveMenu(hMenu, 0, ModifyMenuFunc.MF_BYPOSITION | ModifyMenuFunc.MF_REMOVE);
            }
            UnsafeNativeMethods.DrawMenuBar(hWnd);
            var style = UnsafeNativeMethods.GetWindowLong(hWnd, WindowLongFunc.GWL_STYLE);
            style = style & ~(int)WindowStyleFunc.WS_SYSMENU;
            style = style & ~(int)WindowStyleFunc.WS_CAPTION;
            style = style & ~(int)WindowStyleFunc.WS_MINIMIZE;
            style = style & ~(int)WindowStyleFunc.WS_MAXIMIZEBOX;
            style = style & ~(int)WindowStyleFunc.WS_THICKFRAME;
            UnsafeNativeMethods.SetWindowLongPtr(hWnd, WindowLongFunc.GWL_STYLE, (IntPtr)style);
            style = UnsafeNativeMethods.GetWindowLong(hWnd, WindowLongFunc.GWL_EXSTYLE) | (int)WindowStyleFunc.WS_EX_DLGMODALFRAME;
            UnsafeNativeMethods.SetWindowLongPtr(hWnd, WindowLongFunc.GWL_EXSTYLE, (IntPtr)style);
        }

        /// <summary>
        ///     Removes specified window from taskbar.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static void RemoveWindowFromTaskbar(IntPtr hWnd)
        {
            UnsafeNativeMethods.ShowWindow(hWnd, ShowWindowFunc.SW_HIDE);
            var style = UnsafeNativeMethods.GetWindowLong(hWnd, WindowLongFunc.GWL_EXSTYLE) | (int)WindowStyleFunc.WS_EX_TOOLWINDOW;
            UnsafeNativeMethods.SetWindowLongPtr(hWnd, WindowLongFunc.GWL_EXSTYLE, (IntPtr)style);
            UnsafeNativeMethods.ShowWindow(hWnd, ShowWindowFunc.SW_SHOW);
        }

        /// <summary>
        ///     Disables the maximize button of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static void DisableWindowMaximizeButton(IntPtr hWnd)
        {
            var style = (int)(UnsafeNativeMethods.GetWindowLong(hWnd, WindowLongFunc.GWL_STYLE) & ~0x10000L);
            UnsafeNativeMethods.SetWindowLongPtr(hWnd, WindowLongFunc.GWL_STYLE, (IntPtr)style);
        }

        /// <summary>
        ///     Disables the minimize button of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static void DisableWindowMinimizeButton(IntPtr hWnd)
        {
            var style = (int)(UnsafeNativeMethods.GetWindowLong(hWnd, WindowLongFunc.GWL_STYLE) & ~0x20000L);
            UnsafeNativeMethods.SetWindowLongPtr(hWnd, WindowLongFunc.GWL_STYLE, (IntPtr)style);
        }

        /// <summary>
        ///     Removes the maximize and minimize button of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static void RemoveWindowMinMaxButtons(IntPtr hWnd)
        {
            DisableWindowMaximizeButton(hWnd);
            DisableWindowMinimizeButton(hWnd);
        }

        /// <summary>
        ///     Moves the cursor to the specified coordinates.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="point">
        ///     The new coordinates of the cursor.
        /// </param>
        public static void SetCursorPos(IntPtr hWnd, Point point)
        {
            UnsafeNativeMethods.ClientToScreen(hWnd, ref point);
            UnsafeNativeMethods.SetCursorPos((uint)point.X, (uint)point.Y);
        }

        /// <summary>
        ///     Moves the cursor to the specified coordinates.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="x">
        ///     The new x-coordinate of the cursor.
        /// </param>
        /// <param name="y">
        ///     The new y-coordinate of the cursor.
        /// </param>
        public static void SetCursorPos(IntPtr hWnd, int x, int y) =>
            SetCursorPos(hWnd, new Point(x, y));

        /// <summary>
        ///     Changes the position and dimensions of the specified window to fill the entire screen.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static void SetWindowBorderlessFullscreen(IntPtr hWnd)
        {
            RemoveWindowBorders(hWnd);
            SetWindowFullscreen(hWnd);
        }

        /// <summary>
        ///     Changes the position and dimensions of the specified window to fill the entire screen.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static void SetWindowFullscreen(IntPtr hWnd) =>
            UnsafeNativeMethods.MoveWindow(hWnd, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, true);

        /// <summary>
        ///     Changes the position of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="point">
        ///     The new position of the window.
        /// </param>
        public static void SetWindowPos(IntPtr hWnd, Point point)
        {
            var rect = new Rectangle();
            UnsafeNativeMethods.GetWindowRect(hWnd, ref rect);
            UnsafeNativeMethods.MoveWindow(hWnd, point.X, point.Y, rect.Width, rect.Height, false);
        }

        /// <summary>
        ///     Changes the position of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="x">
        ///     The new position of the left side of the window.
        /// </param>
        /// <param name="y">
        ///     The new position of the top of the window.
        /// </param>
        public static void SetWindowPos(IntPtr hWnd, int x, int y) =>
            SetWindowPos(hWnd, new Point(x, y));

        /// <summary>
        ///     Changes the dimensions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="size">
        ///     The new size of the window.
        /// </param>
        public static void SetWindowSize(IntPtr hWnd, Size size)
        {
            var rect = new Rectangle();
            UnsafeNativeMethods.GetWindowRect(hWnd, ref rect);
            if (rect.Size != size)
                UnsafeNativeMethods.MoveWindow(hWnd, rect.X, rect.Y, size.Width, size.Height, true);
        }

        /// <summary>
        ///     Changes the dimensions of the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="width">
        ///     The new width of the window.
        /// </param>
        /// <param name="height">
        ///     The new height of the window.
        /// </param>
        public static void SetWindowSize(IntPtr hWnd, int width, int height) =>
            SetWindowSize(hWnd, new Size(width, height));

        /// <summary>
        ///     Minimizes and hides the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static void HideWindow(IntPtr hWnd)
        {
            UnsafeNativeMethods.ShowWindow(hWnd, ShowWindowFunc.SW_MINIMIZE);
            UnsafeNativeMethods.ShowWindow(hWnd, ShowWindowFunc.SW_HIDE);
        }

        /// <summary>
        ///     Activates the window and displays it.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public static void ShowWindow(IntPtr hWnd)
        {
            UnsafeNativeMethods.ShowWindow(hWnd, ShowWindowFunc.SW_RESTORE);
            UnsafeNativeMethods.ShowWindow(hWnd, ShowWindowFunc.SW_SHOW);
        }

        /// <summary>
        ///     Provides enumerated values of the current state of the service.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum ServiceState
        {
            /// <summary>
            ///     The service continue is pending.
            /// </summary>
            SERVICE_CONTINUE_PENDING = 0x5,

            /// <summary>
            ///     The service pause is pending.
            /// </summary>
            SERVICE_PAUSE_PENDING = 0x6,

            /// <summary>
            ///     The service is paused.
            /// </summary>
            SERVICE_PAUSED = 0x7,

            /// <summary>
            ///     The service is running.
            /// </summary>
            SERVICE_RUNNING = 0x4,

            /// <summary>
            ///     The service is starting.
            /// </summary>
            SERVICE_START_PENDING = 0x2,

            /// <summary>
            ///     The service is stopping.
            /// </summary>
            SERVICE_STOP_PENDING = 0x3,

            /// <summary>
            ///     The service is not running.
            /// </summary>
            SERVICE_STOPPED = 0x1
        }

        /// <summary>
        ///     Provides enumerated values of the service controls.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum ServiceControls
        {
            /// <summary>
            ///     The service is a network component that can accept changes in its binding without being
            ///     stopped and restarted.
            /// </summary>
            SERVICE_ACCEPT_NETBINDCHANGE = 0x10,

            /// <summary>
            ///     The service can reread its startup parameters without being stopped and restarted.
            /// </summary>
            SERVICE_ACCEPT_PARAMCHANGE = 0x8,

            /// <summary>
            ///     The service can be paused and continued.
            /// </summary>
            SERVICE_ACCEPT_PAUSE_CONTINUE = 0x2,

            /// <summary>
            ///     The service can be stopped.
            /// </summary>
            SERVICE_ACCEPT_STOP = 0x1
        }

        /// <summary>
        ///     Contains status information for a service.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal class SERVICE_STATUS
        {
            /// <summary>
            ///     The check-point value the service increments periodically to report its progress during a
            ///     lengthy start, stop, pause, or continue operation. For example, the service should increment
            ///     this value as it completes each step of its initialization when it is starting up. The user
            ///     interface program that invoked the operation on the service uses this value to track the
            ///     progress of the service during a lengthy operation. This value is not valid and should be
            ///     zero when the service does not have a start, stop, pause, or continue operation pending.
            /// </summary>
            internal int dwCheckPoint;

            /// <summary>
            ///     The control codes the service accepts and processes in its handler function (see
            ///     Handler and HandlerEx). A user interface process can control a service by specifying
            ///     a control command in the ControlService or ControlServiceEx function. By default, all
            ///     services accept the  value. To accept the SERVICE_CONTROL_DEVICEEVENT value, the service
            ///     must register to receive device events by using the RegisterDeviceNotification function.
            /// </summary>
            internal ServiceControls dwControlsAccepted;

            /// <summary>
            ///     The current state of the service.
            /// </summary>
            internal ServiceState dwCurrentState;

            /// <summary>
            ///     A service-specific error code that the service returns when an error occurs while the
            ///     service is starting or stopping. This value is ignored unless the dwWin32ExitCode member
            ///     is set to ERROR_SERVICE_SPECIFIC_ERROR.
            /// </summary>
            internal int dwServiceSpecificExitCode;

            /// <summary>
            ///     The type of service.
            /// </summary>
            internal ServiceTypes dwServiceType;

            /// <summary>
            ///     The estimated time required for a pending start, stop, pause, or continue operation, in
            ///     milliseconds. Before the specified amount of time has elapsed, the service should make its
            ///     next call to the SetServiceStatus function with either an incremented dwCheckPoint value or
            ///     a change in dwCurrentState. If the amount of time specified by dwWaitHint passes, and
            ///     dwCheckPoint has not been incremented or dwCurrentState has not changed, the service control
            ///     manager or service control program can assume that an error has occurred and the service
            ///     should be stopped. However, if the service shares a process with other services, the service
            ///     control manager cannot terminate the service application because it would have to terminate
            ///     the other services sharing the process as well.
            /// </summary>
            internal int dwWaitHint;

            /// <summary>
            ///     The error code the service uses to report an error that occurs when it is starting or
            ///     stopping. To return an error code specific to the service, the service must set this
            ///     value to ERROR_SERVICE_SPECIFIC_ERROR to indicate that the dwServiceSpecificExitCode
            ///     member contains the error code. The service should set this value to NO_ERROR when it
            ///     is running and on normal termination.
            /// </summary>
            internal int dwWin32ExitCode;
        }

        /// <summary>
        ///     Provides enumerated values of the
        ///     <see cref="SafeNativeMethods.ControlService(IntPtr, ControlServiceFunc, SERVICE_STATUS)"/>
        ///     function.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum ControlServiceFunc
        {
            /// <summary>
            ///     Notifies a paused service that it should resume. The hService handle must have the
            ///     SERVICE_PAUSE_CONTINUE access right.
            /// </summary>
            SERVICE_CONTROL_CONTINUE = 0x3,

            /// <summary>
            ///     Notifies a service that it should report its current status information to the service control
            ///     manager. The hService handle must have the <see cref="ServiceAccessRights.SERVICE_INTERROGATE"/>
            ///     access right.
            ///     <para>
            ///         Note that this control is not generally useful as the SCM is aware of the current state of
            ///         the service.
            ///     </para>
            /// </summary>
            SERVICE_CONTROL_INTERROGATE = 0x4,

            /// <summary>
            ///     Notifies a network service that there is a new component for binding. The hService handle must
            ///     have the SERVICE_PAUSE_CONTINUE access right. However, this control code has been deprecated;
            ///     use Plug and Play functionality instead.
            /// </summary>
            SERVICE_CONTROL_NETBINDADD = 0x7,

            /// <summary>
            ///     Notifies a network service that one of its bindings has been disabled. The hService handle must
            ///     have the SERVICE_PAUSE_CONTINUE access right. However, this control code has been deprecated;
            ///     use Plug and Play functionality instead.
            /// </summary>
            SERVICE_CONTROL_NETBINDDISABLE = 0xa,

            /// <summary>
            ///     Notifies a network service that a disabled binding has been enabled. The hService handle must
            ///     have the SERVICE_PAUSE_CONTINUE access right. However, this control code has been deprecated;
            ///     use Plug and Play functionality instead.
            /// </summary>
            SERVICE_CONTROL_NETBINDENABLE = 0x9,

            /// <summary>
            ///     Notifies a network service that a component for binding has been removed. The hService handle
            ///     must have the SERVICE_PAUSE_CONTINUE access right. However, this control code has been deprecated;
            ///     use Plug and Play functionality instead.
            /// </summary>
            SERVICE_CONTROL_NETBINDREMOVE = 0x8,

            /// <summary>
            ///     Notifies a service that its startup parameters have changed. The hService handle must have the
            ///     <see cref="ServiceAccessRights.SERVICE_PAUSE_CONTINUE"/> access right.
            /// </summary>
            SERVICE_CONTROL_PARAMCHANGE = 0x6,

            /// <summary>
            ///     Notifies a service that it should pause. The hService handle must have the
            ///     <see cref="ServiceAccessRights.SERVICE_PAUSE_CONTINUE"/> access right.
            /// </summary>
            SERVICE_CONTROL_PAUSE = 0x2,

            /// <summary>
            ///     Notifies a service that it should stop. The hService handle must have the
            ///     <see cref="ServiceAccessRights.SERVICE_STOP"/> access right.
            ///     <para>
            ///         After sending the stop request to a service, you should not send other controls to the
            ///         service.
            ///     </para>
            /// </summary>
            SERVICE_CONTROL_STOP = 0x1
        }

        /// <summary>
        ///     Provides enumerated values of service access rights.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum ServiceAccessRights
        {
            /// <summary>
            ///     Includes STANDARD_RIGHTS_REQUIRED in addition to all access rights in this table.
            /// </summary>
            SERVICE_ALL_ACCESS = 0xf01ff,

            /// <summary>
            ///     Required to call the ChangeServiceConfig or ChangeServiceConfig2 function to
            ///     change the service configuration. Because this grants the caller the right to
            ///     change the executable file that the system runs, it should be granted only to
            ///     administrators.
            /// </summary>
            SERVICE_CHANGE_CONFIG = 0x2,

            /// <summary>
            ///     Required to call the EnumDependentServices function to enumerate all the services
            ///     dependent on the service.
            /// </summary>
            SERVICE_ENUMERATE_DEPENDENTS = 0x8,

            /// <summary>
            ///     Required to call the ControlService function to ask the service to report its
            ///     status immediately.
            /// </summary>
            SERVICE_INTERROGATE = 0x80,

            /// <summary>
            ///     Required to call the ControlService function to pause or continue the service.
            /// </summary>
            SERVICE_PAUSE_CONTINUE = 0x40,

            /// <summary>
            ///     Required to call the QueryServiceConfig and QueryServiceConfig2 functions
            ///     to query the service configuration.
            /// </summary>
            SERVICE_QUERY_CONFIG = 0x1,

            /// <summary>
            ///     Required to call the QueryServiceStatus or QueryServiceStatusEx function to ask
            ///     the service control manager about the status of the service.
            ///     <para>
            ///         Required to call the NotifyServiceStatusChange function to receive notification
            ///         when a service changes status.
            ///     </para>
            /// </summary>
            SERVICE_QUERY_STATUS = 0x4,

            /// <summary>
            ///     The standard rights.
            /// </summary>
            SERVICE_STANDARD_REQUIRED = 0xf0000,

            /// <summary>
            ///     Required to call the StartService function to start the service.
            /// </summary>
            SERVICE_START = 0x10,

            /// <summary>
            ///     Required to call the ControlService function to stop the service.
            /// </summary>
            SERVICE_STOP = 0x20,

            /// <summary>
            ///     Required to call the ControlService function to specify a user-defined control
            ///     code.
            /// </summary>
            SERVICE_USER_DEFINED_CONTROL = 0x100
        }

        /// <summary>
        ///     Provides enumerated values of the service start options.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum ServiceBootFlag
        {
            /// <summary>
            ///     A service started automatically by the service control manager during system startup.
            ///     For more information, see Automatically Starting Services.
            /// </summary>
            SERVICE_AUTO_START = 0x2,

            /// <summary>
            ///     A device driver started by the system loader. This value is valid only for driver
            ///     services.
            /// </summary>
            SERVICE_BOOT_START = 0x0,

            /// <summary>
            ///     A service started by the service control manager when a process calls the
            ///     StartService function. For more information, see Starting Services on Demand.
            /// </summary>
            SERVICE_DEMAND_START = 0x3,

            /// <summary>
            ///     A service that cannot be started. Attempts to start the service result in the error
            ///     code ERROR_SERVICE_DISABLED.
            /// </summary>
            SERVICE_DISABLED = 0x4,

            /// <summary>
            ///     A device driver started by the IoInitSystem function. This value is valid only for
            ///     driver services.
            /// </summary>
            SERVICE_SYSTEM_START = 0x1
        }

        /// <summary>
        ///     Provides enumerated values of service control manager access rights.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum ServiceManagerAccessRights
        {
            /// <summary>
            ///     Includes STANDARD_RIGHTS_REQUIRED, in addition to all access rights in this table.
            /// </summary>
            SC_MANAGER_ALL_ACCESS = 0xf003f,

            /// <summary>
            ///     Required to call the CreateService function to create a service object and add it
            ///     to the database.
            /// </summary>
            SC_MANAGER_CREATE_SERVICE = 0x0002,

            /// <summary>
            ///     Required to connect to the service control manager.
            /// </summary>
            SC_MANAGER_CONNECT = 0x0001,

            /// <summary>
            ///     Required to call the EnumServicesStatus or EnumServicesStatusEx function to list
            ///     the services that are in the database.
            ///     <para>
            ///         Required to call the NotifyServiceStatusChange function to receive notification
            ///         when any service is created or deleted.
            ///     </para>
            /// </summary>
            SC_MANAGER_ENUMERATE_SERVICE = 0x0004,

            /// <summary>
            ///     Required to call the LockServiceDatabase function to acquire a lock on the database.
            /// </summary>
            SC_MANAGER_LOCK = 0x0008,

            /// <summary>
            ///     Required to call the NotifyBootConfigStatus function.
            /// </summary>
            SC_MANAGER_MODIFY_BOOT_CONFIG = 0x0020,

            /// <summary>
            ///     Required to call the QueryServiceLockStatus function to retrieve the lock status
            ///     information for the database.
            /// </summary>
            SC_MANAGER_QUERY_LOCK_STATUS = 0x0010,

            /// <summary>
            ///     The standard rights.
            /// </summary>
            SC_MANAGER_STANDARD_REQUIRED = 0xf0000
        }

        /// <summary>
        ///     Provides enumerated values of service type constants.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum ServiceTypes
        {
            /// <summary>
            ///     Reserved.
            /// </summary>
            SERVICE_ADAPTER = 0x4,

            /// <summary>
            ///     File system driver service.
            /// </summary>
            SERVICE_FILE_SYSTEM_DRIVER = 0x2,

            /// <summary>
            ///     Driver service.
            /// </summary>
            SERVICE_KERNEL_DRIVER = 0x1,

            /// <summary>
            ///     Reserved.
            /// </summary>
            SERVICE_RECOGNIZER_DRIVER = 0x8,

            /// <summary>
            ///     Service that runs in its own process.
            /// </summary>
            SERVICE_WIN32_OWN_PROCESS = 0x10,

            /// <summary>
            ///     Service that shares a process with one or more other services.
            /// </summary>
            SERVICE_WIN32_SHARE_PROCESS = 0x20,

            /// <summary>
            ///     The service can interact with the desktop.
            ///     <para>
            ///         If you specify either <see cref="SERVICE_WIN32_OWN_PROCESS"/> or
            ///         <see cref="SERVICE_WIN32_SHARE_PROCESS"/>, and the service is running in the
            ///         context of the LocalSystem account, you can also specify this value.
            ///     </para>
            /// </summary>
            SERVICE_INTERACTIVE_PROCESS = 0x100
        }

        /// <summary>
        ///     Represents safe native methods.
        ///     <para>
        ///         This class suppresses stack walks for unmanaged code permission.
        ///         (<see cref="SuppressUnmanagedCodeSecurityAttribute"/> is applied to this class.)
        ///         This class is for methods that are safe for anyone to call. Callers of these
        ///         methods are not required to perform a full security review to make sure that the
        ///         usage is secure because the methods are harmless for any caller.
        ///     </para>
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            /// <summary>
            ///     Allocates a new console for the calling process.
            /// </summary>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, EntryPoint = "AllocConsole", SetLastError = true)]
            internal static extern int AllocConsole();

            /// <summary>
            ///     Retrieves the window handle used by the console associated with the calling
            ///     process.
            /// </summary>
            /// <returns>
            ///     The return value is a handle to the window used by the console associated with
            ///     the calling process or NULL if there is no such associated console.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            internal static extern IntPtr GetConsoleWindow();

            /// <summary>
            ///     ***This is an undocumented API and as such is not supported by Microsoft and can be changed
            ///     or removed in the future without futher notice.
            /// </summary>
            [DllImport(DllNames.Dwmapi, EntryPoint = "#127", PreserveSig = false, SetLastError = true)]
            internal static extern void DwmGetColorizationParameters(out DWM_COLORIZATION_PARAMS parameters);

            /// <summary>
            ///     ***This is an undocumented API and as such is not supported by Microsoft and can be changed
            ///     or removed in the future without futher notice.
            /// </summary>
            [DllImport(DllNames.Dwmapi, EntryPoint = "#131", PreserveSig = false, SetLastError = true)]
            internal static extern void DwmSetColorizationParameters(ref DWM_COLORIZATION_PARAMS parameters, bool unknown);

            /// <summary>
            ///     Retrieves information about an object in the file system, such as a file, folder, directory, or
            ///     drive root.
            /// </summary>
            /// <param name="pszPath">
            ///     A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and file
            ///     name. Both absolute and relative paths are valid.
            ///     <para>
            ///         If the uFlags parameter includes the SHGFI_PIDL flag, this parameter must be the address of
            ///         an ITEMIDLIST (PIDL) structure that contains the list of item identifiers that uniquely
            ///         identifies the file within the Shell's namespace. The PIDL must be a fully qualified PIDL.
            ///         Relative PIDLs are not allowed.
            ///     </para>
            ///     <para>
            ///         If the uFlags parameter includes the SHGFI_USEFILEATTRIBUTES flag, this parameter does not have
            ///         to be a valid file name. The function will proceed as if the file exists with the specified name
            ///         and with the file attributes passed in the dwFileAttributes parameter. This allows you to obtain
            ///         information about a file type by passing just the extension for pszPath and passing
            ///         FILE_ATTRIBUTE_NORMAL in dwFileAttributes.
            ///     </para>
            ///     <para>
            ///         This string can use either short (the 8.3 form) or long file names.
            ///     </para>
            /// </param>
            /// <param name="dwFileAttributes">
            ///     A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If
            ///     uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.
            /// </param>
            /// <param name="psfi">
            ///     Pointer to a <see cref="SHFILEINFO"/> structure to receive the file information.
            /// </param>
            /// <param name="cbFileInfo">
            ///     The size, in bytes, of the <see cref="SHFILEINFO"/> structure pointed to by the psfi parameter.
            /// </param>
            /// <param name="uFlags">
            ///     The flags that specify the file information to retrieve.
            /// </param>
            /// <returns>
            /// </returns>
            [DllImport(DllNames.Shell32, SetLastError = true, BestFitMapping = false, CharSet = CharSet.Unicode)]
            internal static extern IntPtr SHGetFileInfo([MarshalAs(UnmanagedType.LPStr, SizeConst = 32767)] string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, GetFileInfoFunc uFlags);

            /// <summary>
            ///     Retrieves the names of all sections in an initialization file.
            /// </summary>
            /// <param name="lpszReturnBuffer">
            ///     A pointer to a buffer that receives the section names associated with the named file. The buffer
            ///     is filled with one or more null-terminated strings; the last string is followed by a second null
            ///     character.
            /// </param>
            /// <param name="nSize">
            ///     The size of the buffer pointed to by the lpszReturnBuffer parameter, in characters.
            /// </param>
            /// <param name="lpFileName">
            ///     The name of the initialization file. If this parameter is NULL, the function searches the Win.ini
            ///     file. If this parameter does not contain a full path to the file, the system searches for the file
            ///     in the Windows directory.
            /// </param>
            /// <returns>
            ///     The return value specifies the number of characters copied to the specified buffer, not including
            ///     the terminating null character. If the buffer is not large enough to contain all the section names
            ///     associated with the specified initialization file, the return value is equal to the size specified
            ///     by nSize minus two.
            /// </returns>
            [DllImport(DllNames.Kernel32, BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            internal static extern int GetPrivateProfileSectionNames(byte[] lpszReturnBuffer, int nSize, [MarshalAs(UnmanagedType.LPStr)] string lpFileName);

            /// <summary>
            ///     Retrieves a string from the specified section in an initialization file.
            /// </summary>
            /// <param name="lpApplicationName">
            ///     The name of the section containing the key name. If this parameter is NULL, the
            ///     GetPrivateProfileString function copies all section names in the file to the supplied buffer.
            /// </param>
            /// <param name="lpKeyName">
            ///     The name of the key whose associated string is to be retrieved. If this parameter is NULL, all key
            ///     names in the section specified by the lpAppName parameter are copied to the buffer specified by the
            ///     lpReturnedString parameter.
            /// </param>
            /// <param name="nDefault">
            ///     A default string. If the lpKeyName key cannot be found in the initialization file,
            ///     GetPrivateProfileString copies the default string to the lpReturnedString buffer. If this parameter
            ///     is NULL, the default is an empty string.
            ///     <para>
            ///         Avoid specifying a default string with trailing blank characters. The function inserts a null
            ///         character in the lpReturnedString buffer to strip any trailing blanks.
            ///     </para>
            /// </param>
            /// <param name="retVal">
            ///     A pointer to the buffer that receives the retrieved string.
            /// </param>
            /// <param name="nSize">
            ///     The size of the buffer pointed to by the retVal parameter, in characters.
            /// </param>
            /// <param name="lpFileName">
            ///     The name of the initialization file. If this parameter does not contain a full path to the file, the
            ///     system searches for the file in the Windows directory.
            /// </param>
            /// <returns>
            ///     The return value is the number of characters copied to the buffer, not including the terminating null
            ///     character.
            ///     <para>
            ///         If neither lpAppName nor lpKeyName is NULL and the supplied destination buffer is too small to
            ///         hold the requested string, the string is truncated and followed by a null character, and the
            ///         return value is equal to nSize minus one.
            ///     </para>
            ///     <para>
            ///         If either lpAppName or lpKeyName is NULL and the supplied destination buffer is too small to hold
            ///         all the strings, the last string is truncated and followed by two null characters. In this case,
            ///         the return value is equal to nSize minus two.
            ///     </para>
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string nDefault, StringBuilder retVal, int nSize, string lpFileName);

            /// <summary>
            ///     Retrieves a string from the specified section in an initialization file.
            /// </summary>
            /// <param name="lpApplicationName">
            ///     The name of the section containing the key name. If this parameter is NULL, the
            ///     GetPrivateProfileString function copies all section names in the file to the supplied buffer.
            /// </param>
            /// <param name="lpKeyName">
            ///     The name of the key whose associated string is to be retrieved. If this parameter is NULL, all key
            ///     names in the section specified by the lpAppName parameter are copied to the buffer specified by the
            ///     lpReturnedString parameter.
            /// </param>
            /// <param name="nDefault">
            ///     A default string. If the lpKeyName key cannot be found in the initialization file,
            ///     GetPrivateProfileString copies the default string to the lpReturnedString buffer. If this parameter
            ///     is NULL, the default is an empty string.
            ///     <para>
            ///         Avoid specifying a default string with trailing blank characters. The function inserts a null
            ///         character in the lpReturnedString buffer to strip any trailing blanks.
            ///     </para>
            /// </param>
            /// <param name="retVal">
            ///     A pointer to the buffer that receives the retrieved string.
            /// </param>
            /// <param name="nSize">
            ///     The size of the buffer pointed to by the lpReturnedString parameter, in characters.
            /// </param>
            /// <param name="lpFileName">
            ///     The name of the initialization file. If this parameter does not contain a full path to the file, the
            ///     system searches for the file in the Windows directory.
            /// </param>
            /// <returns>
            ///     The return value is the number of characters copied to the buffer, not including the terminating null
            ///     character.
            ///     <para>
            ///         If neither lpAppName nor lpKeyName is NULL and the supplied destination buffer is too small to
            ///         hold the requested string, the string is truncated and followed by a null character, and the
            ///         return value is equal to nSize minus one.
            ///     </para>
            ///     <para>
            ///         If either lpAppName or lpKeyName is NULL and the supplied destination buffer is too small to hold
            ///         all the strings, the last string is truncated and followed by two null characters. In this case,
            ///         the return value is equal to nSize minus two.
            ///     </para>
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string nDefault, string retVal, int nSize, string lpFileName);

            /// <summary>
            ///     Replaces the keys and values for the specified section in an initialization file.
            /// </summary>
            /// <param name="lpAppName">
            ///     The name of the section in which data is written. This section name is typically the name of the
            ///     calling application.
            /// </param>
            /// <param name="lpString">
            ///     The new key names and associated values that are to be written to the named section. This string is
            ///     limited to 65535 bytes.
            /// </param>
            /// <param name="lpFileName">
            ///     The name of the initialization file. If this parameter does not contain a full path for the file, the
            ///     function searches the Windows directory for the file. If the file does not exist and lpFileName does
            ///     not contain a full path, the function creates the file in the Windows directory.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            internal static extern int WritePrivateProfileSection([MarshalAs(UnmanagedType.LPStr)] string lpAppName, [MarshalAs(UnmanagedType.LPStr, SizeConst = 65535)] string lpString, [MarshalAs(UnmanagedType.LPStr)] string lpFileName);

            /// <summary>
            ///     Copies a string into the specified section of an initialization file.
            /// </summary>
            /// <param name="lpAppName">
            ///     The name of the section to which the string will be copied. If the section does not exist, it is
            ///     created. The name of the section is case-independent; the string can be any combination of uppercase
            ///     and lowercase letters.
            /// </param>
            /// <param name="lpKeyName">
            ///     The name of the key to be associated with a string. If the key does not exist in the specified section,
            ///     it is created. If this parameter is NULL, the entire section, including all entries within the section,
            ///     is deleted.
            /// </param>
            /// <param name="lpString">
            ///     A null-terminated string to be written to the file. If this parameter is NULL, the key pointed to by the
            ///     lpKeyName parameter is deleted.
            /// </param>
            /// <param name="lpFileName">
            ///     The name of the initialization file.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

            /// <summary>
            ///     The mciSendString function sends a command string to an MCI device. The device that the
            ///     command is sent to is specified in the command string.
            /// </summary>
            /// <param name="lpszCommand">
            ///     Pointer to a null-terminated string that specifies an MCI command string.
            /// </param>
            /// <param name="lpszReturnString">
            ///     Pointer to a buffer that receives return information. If no return information is needed,
            ///     this parameter can be NULL.
            /// </param>
            /// <param name="cchReturn">
            ///     Size, in characters, of the return buffer specified by the lpszReturnString parameter.
            /// </param>
            /// <param name="hwndCallback">
            ///     Handle to a callback window if the "notify" flag was specified in the command string.
            /// </param>
            /// <returns>
            ///     Returns zero if successful or an error otherwise. The low-order word of the returned DWORD
            ///     value contains the error return value. If the error is device-specific, the high-order word
            ///     of the return value is the driver identifier; otherwise, the high-order word is zero.
            /// </returns>
            [DllImport(DllNames.Winmm, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int mciSendString(string lpszCommand, StringBuilder lpszReturnString, uint cchReturn, IntPtr hwndCallback);

            /// <summary>
            ///     The timeBeginPeriod function requests a minimum resolution for periodic timers.
            /// </summary>
            /// <param name="uPeriod">
            ///     Minimum timer resolution, in milliseconds, for the application or device driver. A lower value
            ///     specifies a higher (more accurate) resolution.
            /// </param>
            /// <returns>
            ///     Returns TIMERR_NOERROR if successful or TIMERR_NOCANDO if the resolution specified in uPeriod
            ///     is out of range.
            /// </returns>
            [DllImport(DllNames.Winmm, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern uint timeBeginPeriod(uint uPeriod);

            /// <summary>
            ///     The timeEndPeriod function clears a previously set minimum timer resolution.
            /// </summary>
            /// <param name="uPeriod">
            ///     Minimum timer resolution specified in the previous call to the timeBeginPeriod function.
            /// </param>
            /// <returns>
            ///     Returns TIMERR_NOERROR if successful or TIMERR_NOCANDO if the resolution specified in uPeriod
            ///     is out of range.
            /// </returns>
            [DllImport(DllNames.Winmm, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern uint timeEndPeriod(uint uPeriod);

            /// <summary>
            ///     The waveOutGetVolume function retrieves the current volume level of the specified waveform-audio
            ///     output device.
            /// </summary>
            /// <param name="hwo">
            ///     Handle to an open waveform-audio output device. This parameter can also be a device identifier.
            /// </param>
            /// <param name="dwVolume">
            ///     Pointer to a variable to be filled with the current volume setting. The low-order word of
            ///     this location contains the left-channel volume setting, and the high-order word contains the
            ///     right-channel setting. A value of 0xFFFF represents full volume, and a value of 0x0000 is silence.
            /// </param>
            /// <returns>
            ///     Returns MMSYSERR_NOERROR if successful or an error otherwise.
            /// </returns>
            [DllImport(DllNames.Winmm, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

            /// <summary>
            ///     The waveOutSetVolume function sets the volume level of the specified waveform-audio output device.
            /// </summary>
            /// <param name="hwo">
            ///     Handle to an open waveform-audio output device. This parameter can also be a device identifier.
            /// </param>
            /// <param name="dwVolume">
            ///     New volume setting. The low-order word contains the left-channel volume setting, and the high-order
            ///     word contains the right-channel setting. A value of 0xFFFF represents full volume, and a value of
            ///     0x0000 is silence.
            /// </param>
            /// <returns>
            ///     Returns MMSYSERR_NOERROR if successful or an error otherwise.
            /// </returns>
            [DllImport(DllNames.Winmm, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

            /// <summary>
            ///     Establishes a connection to the service control manager on the specified computer and opens
            ///     the specified service control manager database.
            /// </summary>
            /// <param name="lpMachineName">
            ///     The name of the target computer. If the pointer is NULL or points to an empty string, the
            ///     function connects to the service control manager on the local computer.
            /// </param>
            /// <param name="lpDatabaseName">
            ///     The name of the service control manager database. This parameter should be set to
            ///     SERVICES_ACTIVE_DATABASE. If it is NULL, the SERVICES_ACTIVE_DATABASE database is opened by
            ///     default.
            /// </param>
            /// <param name="dwDesiredAccess">
            ///     The access to the service control manager. For a list of access rights, see
            ///     <see cref="AccessRights"/>.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the specified service control
            ///     manager database.
            /// </returns>
            [DllImport(DllNames.Advapi32, EntryPoint = "OpenSCManagerA", BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr OpenSCManager([MarshalAs(UnmanagedType.LPStr)] string lpMachineName, [MarshalAs(UnmanagedType.LPStr)] string lpDatabaseName, ServiceManagerAccessRights dwDesiredAccess);

            /// <summary>
            ///     Opens an existing service.
            /// </summary>
            /// <param name="hScManager">
            ///     A handle to the service control manager database. The OpenSCManager function returns this handle.
            /// </param>
            /// <param name="lpServiceName">
            ///     The name of the service to be opened. This is the name specified by the lpServiceName parameter
            ///     of the CreateService function when the service object was created, not the service display name
            ///     that is shown by user interface applications to identify the service.
            ///     <para>
            ///         The maximum string length is 256 characters. The service control manager database preserves
            ///         the case of the characters, but service name comparisons are always case insensitive.
            ///         Forward-slash (/) and backslash (\) are invalid service name characters.
            ///     </para>
            /// </param>
            /// <param name="dwDesiredAccess">
            ///     The access to the service.
            ///     <para>
            ///         Before granting the requested access, the system checks the access token of the calling process
            ///         against the discretionary access-control list of the security descriptor associated with the
            ///         service object.
            ///     </para>
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the service.
            /// </returns>
            [DllImport(DllNames.Advapi32, EntryPoint = "OpenServiceA", BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr OpenService(IntPtr hScManager, [MarshalAs(UnmanagedType.LPStr)] string lpServiceName, ServiceAccessRights dwDesiredAccess);

            /// <summary>
            ///     Creates a service object and adds it to the specified service control manager database.
            /// </summary>
            /// <param name="hScManager">
            ///     A handle to the service control manager database. This handle is returned by the OpenSCManager
            ///     function and must have the <see cref="ServiceManagerAccessRights.SC_MANAGER_CREATE_SERVICE"/>
            ///     access right.
            /// </param>
            /// <param name="lpServiceName">
            ///     The name of the service to install. The maximum string length is 256 characters. The service
            ///     control manager database preserves the case of the characters, but service name comparisons
            ///     are always case insensitive. Forward-slash (/) and backslash (\) are not valid service name
            ///     characters.
            /// </param>
            /// <param name="lpDisplayName">
            ///     The display name to be used by user interface programs to identify the service. This string
            ///     has a maximum length of 256 characters. The name is case-preserved in the service control
            ///     manager. Display name comparisons are always case-insensitive.
            /// </param>
            /// <param name="dwDesiredAccess">
            ///     The access to the service. Before granting the requested access, the system checks the access
            ///     token of the calling process.
            /// </param>
            /// <param name="dwServiceType">
            ///     The service type.
            /// </param>
            /// <param name="dwStartType">
            ///     The service start options. This parameter can be one of the following values.
            /// </param>
            /// <param name="dwErrorControl">
            ///     The severity of the error, and action taken, if this service fails to start. This parameter
            ///     can be one of the following values.
            /// </param>
            /// <param name="lpBinaryPathName">
            ///     The fully qualified path to the service binary file. If the path contains a space, it must be
            ///     quoted so that it is correctly interpreted. For example, "d:\\my share\\myservice.exe" should
            ///     be specified as "\"d:\\my share\\myservice.exe\"".
            ///     <para>
            ///         The path can also include arguments for an auto-start service. For example,
            ///         "d:\\myshare\\myservice.exe arg1 arg2". These arguments are passed to the service entry
            ///         point (typically the main function).
            ///     </para>
            ///     <para>
            ///         If you specify a path on another computer, the share must be accessible by the computer
            ///         account of the local computer because this is the security context used in the remote
            ///         call. However, this requirement allows any potential vulnerabilities in the remote
            ///         computer to affect the local computer. Therefore, it is best to use a local file.
            ///     </para>
            /// </param>
            /// <param name="lpLoadOrderGroup">
            ///     The names of the load ordering group of which this service is a member. Specify NULL or an
            ///     empty string if the service does not belong to a group.
            ///     <para>
            ///         The startup program uses load ordering groups to load groups of services in a specified
            ///         order with respect to the other groups. The list of load ordering groups is contained in
            ///         the following registry value:
            ///         "HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\ServiceGroupOrder"
            ///     </para>
            /// </param>
            /// <param name="lpdwTagId">
            ///     A pointer to a variable that receives a tag value that is unique in the group specified in
            ///     the lpLoadOrderGroup parameter. Specify NULL if you are not changing the existing tag.
            ///     <para>
            ///         You can use a tag for ordering service startup within a load ordering group by specifying
            ///         a tag order vector in the following registry value:
            ///         "HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\GroupOrderList"
            ///     </para>
            ///     <para>
            ///         Tags are only evaluated for driver services that have
            ///         <see cref="ServiceBootFlag.SERVICE_BOOT_START"/> or
            ///         <see cref="ServiceBootFlag.SERVICE_SYSTEM_START"/> start types.
            ///     </para>
            /// </param>
            /// <param name="lpDependencies">
            ///     A pointer to a double null-terminated array of null-separated names of services or load
            ///     ordering groups that the system must start before this service. Specify NULL or an empty
            ///     string if the service has no dependencies. Dependency on a group means that this service
            ///     can run if at least one member of the group is running after an attempt to start all members
            ///     of the group.
            ///     <para>
            ///         You must prefix group names with SC_GROUP_IDENTIFIER so that they can be distinguished
            ///         from a service name, because services and service groups share the same name space.
            ///     </para>
            /// </param>
            /// <param name="lpServiceStartName ">
            ///     The name of the account under which the service should run. If the service type is
            ///     <see cref="ServiceTypes.SERVICE_WIN32_OWN_PROCESS"/>, use an account name in the form
            ///     DomainName\UserName. The service process will be logged on as this user. If the account
            ///     belongs to the built-in domain, you can specify .\UserName.
            ///     <para>
            ///         If this parameter is NULL, CreateService uses the LocalSystem account. If the service
            ///         type specifies <see cref="ServiceTypes.SERVICE_INTERACTIVE_PROCESS"/>, the service
            ///         must run in the LocalSystem account.
            ///     </para>
            ///     <para>
            ///         If this parameter is NT AUTHORITY\LocalService, CreateService uses the LocalService
            ///         account. If the parameter is NT AUTHORITY\NetworkService, CreateService uses the
            ///         NetworkService account.
            ///     </para>
            ///     <para>
            ///         A shared process can run as any user.
            ///     </para>
            ///     <para>
            ///         If the service type is <see cref="ServiceTypes.SERVICE_KERNEL_DRIVER"/> or
            ///         <see cref="ServiceTypes.SERVICE_FILE_SYSTEM_DRIVER"/>, the name is the driver
            ///         object name that the system uses to load the device driver. Specify NULL if the driver
            ///         is to use a default object name created by the I/O system.
            ///     </para>
            ///     <para>
            ///         A service can be configured to use a managed account or a virtual account. If the
            ///         service is configured to use a managed service account, the name is the managed service
            ///         account name. If the service is configured to use a virtual account, specify the name as
            ///         NT SERVICE\ServiceName. For more information about managed service accounts and virtual
            ///         accounts, see the Service Accounts Step-by-Step Guide.
            ///     </para>
            /// </param>
            /// <param name="lpPassword">
            ///     The password to the account name specified by the lpServiceStartName parameter. Specify an
            ///     empty string if the account has no password or if the service runs in the LocalService,
            ///     NetworkService, or LocalSystem account.
            ///     <para>
            ///         If the account name specified by the lpServiceStartName parameter is the name of a
            ///         managed service account or virtual account name, the lpPassword parameter must be NULL.
            ///     </para>
            ///     <para>
            ///         Passwords are ignored for driver services.
            ///     </para>
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the service.
            /// </returns>
            [DllImport(DllNames.Advapi32, EntryPoint = "CreateServiceA", BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr CreateService(IntPtr hScManager, [MarshalAs(UnmanagedType.LPStr)] string lpServiceName, [MarshalAs(UnmanagedType.LPStr)] string lpDisplayName, ServiceAccessRights dwDesiredAccess, ServiceTypes dwServiceType, ServiceBootFlag dwStartType, ServiceError dwErrorControl, [MarshalAs(UnmanagedType.LPStr)] string lpBinaryPathName, [MarshalAs(UnmanagedType.LPStr)] string lpLoadOrderGroup, IntPtr lpdwTagId, [MarshalAs(UnmanagedType.LPStr)] string lpDependencies, [MarshalAs(UnmanagedType.LPStr)] string lpServiceStartName, [MarshalAs(UnmanagedType.LPStr)] string lpPassword);

            /// <summary>
            ///     Closes a handle to a service control manager or service object.
            /// </summary>
            /// <param name="hScObject">
            ///     A handle to the service control manager object or the service object to close. Handles to
            ///     service control manager objects are returned by the OpenSCManager function, and handles to
            ///     service objects are returned by either the OpenService or CreateService function.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true, CharSet = CharSet.Ansi)]
            internal static extern int CloseServiceHandle(IntPtr hScObject);

            /// <summary>
            ///     Retrieves the current status of the specified service.
            /// </summary>
            /// <param name="hService">
            ///     A handle to the service. This handle is returned by the OpenService or the CreateService
            ///     function, and it must have the <see cref="ServiceAccessRights.SERVICE_QUERY_STATUS"/> access
            ///     right.
            /// </param>
            /// <param name="lpServiceStatus">
            ///     A pointer to a <see cref="SERVICE_STATUS"/> structure that receives the status information.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true, CharSet = CharSet.Ansi)]
            internal static extern int QueryServiceStatus(IntPtr hService, SERVICE_STATUS lpServiceStatus);

            /// <summary>
            ///     Marks the specified service for deletion from the service control manager database.
            /// </summary>
            /// <param name="hService">
            ///     A handle to the service. This handle is returned by the OpenService or CreateService function,
            ///     and it must have the <see cref="AccessRights.DELETE"/> access right.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true, CharSet = CharSet.Ansi)]
            internal static extern int DeleteService(IntPtr hService);

            /// <summary>
            ///     Sends a control code to a service.
            /// </summary>
            /// <param name="hService">
            ///     A handle to the service. This handle is returned by the OpenService or CreateService function.
            /// </param>
            /// <param name="dwControl">
            /// </param>
            /// <param name="lpServiceStatus">
            /// </param>
            /// <returns>
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true, CharSet = CharSet.Ansi)]
            internal static extern int ControlService(IntPtr hService, ControlServiceFunc dwControl, SERVICE_STATUS lpServiceStatus);

            /// <summary>
            ///     Starts a service.
            /// </summary>
            /// <param name="hService">
            ///     A handle to the service. This handle is returned by the OpenService or CreateService function,
            ///     and it must have the <see cref="ServiceAccessRights.SERVICE_START"/> access right.
            /// </param>
            /// <param name="dwNumServiceArgs">
            ///     The number of strings in the lpServiceArgVectors array. If lpServiceArgVectors is NULL, this
            ///     parameter can be zero.
            /// </param>
            /// <param name="lpServiceArgVectors">
            ///     The null-terminated strings to be passed to the ServiceMain function for the service as arguments.
            ///     If there are no arguments, this parameter can be NULL. Otherwise, the first argument
            ///     (lpServiceArgVectors[0]) is the name of the service, followed by any additional arguments
            ///     (lpServiceArgVectors[1] through lpServiceArgVectors[dwNumServiceArgs-1]).
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, EntryPoint = "StartServiceA", SetLastError = true, CharSet = CharSet.Ansi)]
            internal static extern int StartService(IntPtr hService, int dwNumServiceArgs, int lpServiceArgVectors);
        }

        /// <summary>
        ///     Represents unsafe native methods.
        ///     <para>
        ///         This class suppresses stack walks for unmanaged code permission.
        ///         (<see cref="SuppressUnmanagedCodeSecurityAttribute"/> is applied to this class.) This class is for
        ///         methods that are potentially dangerous. Any caller of these methods must perform a full security
        ///         review to make sure that the usage is secure because no stack walk will be performed.
        ///     </para>
        ///     <para>
        ///         The functions of this class are marked as public, which can have security implications if it is
        ///         implemented incorrectly. This doesn't automatically affect the security, not even if the code
        ///         analysis outputs a CA1401 (P/Invokes should not be visible) warning.
        ///     </para>
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        public static class UnsafeNativeMethods
        {
            /// <summary>
            ///     Passes the hook information to the next hook procedure in the current hook chain. A hook
            ///     procedure can call this function either before or after processing the hook information.
            /// </summary>
            /// <param name="hhk">
            ///     This parameter is ignored.
            /// </param>
            /// <param name="nCode">
            ///     The hook code passed to the current hook procedure. The next hook procedure uses this code
            ///     to determine how to process the hook information.
            /// </param>
            /// <param name="wParam">
            ///     The wParam value passed to the current hook procedure. The meaning of this parameter
            ///     depends on the type of hook associated with the current hook chain.
            /// </param>
            /// <param name="lParam">
            ///     The lParam value passed to the current hook procedure. The meaning of this parameter
            ///     depends on the type of hook associated with the current hook chain.
            /// </param>
            /// <returns>
            ///     This value is returned by the next hook procedure in the chain. The current hook procedure
            ///     must also return this value. The meaning of the return value depends on the hook type.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

            /// <summary>
            ///     The ClientToScreen function converts the client-area coordinates of a specified point to screen
            ///     coordinates.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose client area is used for the conversion.
            /// </param>
            /// <param name="lpPoint">
            ///     A pointer to a <see cref="Point"/> structure that contains the client coordinates to be converted.
            ///     The new screen coordinates are copied into this structure if the function succeeds.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

            /// <summary>
            ///     Closes an open object handle.
            /// </summary>
            /// <param name="handle">
            ///     A valid handle to an open object.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern bool CloseHandle(IntPtr handle);

            /// <summary>
            ///     Deletes an item from the specified menu. If the menu item opens a menu or submenu, this function
            ///     destroys the handle to the menu or submenu and frees the memory used by the menu or submenu.
            /// </summary>
            /// <param name="hMenu">
            ///     A handle to the menu to be changed.
            /// </param>
            /// <param name="nPosition">
            ///     The menu item to be deleted, as determined by the uFlags parameter.
            /// </param>
            /// <param name="wFlags">
            ///     Indicates how the uPosition parameter is interpreted. This parameter must be
            ///     <see cref="ModifyMenuFunc.MF_BYCOMMAND"/> or <see cref="ModifyMenuFunc.MF_BYPOSITION"/>.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern int DeleteMenu(IntPtr hMenu, uint nPosition, ModifyMenuFunc wFlags);

            /// <summary>
            ///     Destroys an icon and frees any memory the icon occupied.
            /// </summary>
            /// <param name="hIcon">
            ///     A handle to the icon to be destroyed. The icon must not be in use.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool DestroyIcon(IntPtr hIcon);

            /// <summary>
            ///     Redraws the menu bar of the specified window. If the menu bar changes after the system has
            ///     created the window, this function must be called to draw the changed menu bar.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose menu bar is to be redrawn.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool DrawMenuBar(IntPtr hWnd);

            /// <summary>
            ///     Duplicates an object handle.
            /// </summary>
            /// <param name="hSourceProcessHandle">
            ///     A handle to the process with the handle to be duplicated.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.PROCESS_DUP_HANDLE"/> access right.
            ///     </para>
            /// </param>
            /// <param name="hSourceHandle">
            ///     The handle to be duplicated. This is an open object handle that is valid in the context
            ///     of the source process. For a list of objects whose handles can be duplicated, see the
            ///     following Remarks section.
            /// </param>
            /// <param name="hTargetProcessHandle">
            ///     A handle to the process that is to receive the duplicated handle.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.PROCESS_DUP_HANDLE"/> access right.
            ///     </para>
            /// </param>
            /// <param name="lpTargetHandle">
            ///     A pointer to a variable that receives the duplicate handle. This handle value is valid
            ///     in the context of the target process.
            ///     <para>
            ///         If hSourceHandle is a pseudo handle returned by GetCurrentProcess or GetCurrentThread,
            ///         DuplicateHandle converts it to a real handle to a process or thread, respectively.
            ///     </para>
            ///     <para>
            ///         If lpTargetHandle is NULL, the function duplicates the handle, but does not return the
            ///         duplicate handle value to the caller. This behavior exists only for backward
            ///         compatibility with previous versions of this function. You should not use this feature,
            ///         as you will lose system resources until the target process terminates.
            ///     </para>
            /// </param>
            /// <param name="dwDesiredAccess">
            ///     The access requested for the new handle. For the flags that can be specified for each
            ///     object type, see the following Remarks section.
            ///     <para>
            ///         This parameter is ignored if the dwOptions parameter specifies the
            ///         <see cref="DuplicateFuncOptions.DUPLICATE_SAME_ACCESS"/> flag. Otherwise, the flags that
            ///         can be specified depend on the type of object whose handle is to be duplicated.
            ///     </para>
            /// </param>
            /// <param name="bInheritHandle">
            ///     A variable that indicates whether the handle is inheritable. If TRUE, the duplicate handle
            ///     can be inherited by new processes created by the target process. If FALSE, the new handle
            ///     cannot be inherited.
            /// </param>
            /// <param name="dwOptions">
            ///     Optional actions. This parameter can be zero, or any combination of
            ///     <see cref="DuplicateFuncOptions"/>.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

            /// <summary>
            ///     Destroys a modal dialog box, causing the system to end any processing for the dialog box.
            /// </summary>
            /// <param name="hDlg">
            ///     A handle to the dialog box to be destroyed.
            /// </param>
            /// <param name="nResult">
            ///     The value to be returned to the application from the function that created the dialog box.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

            /// <summary>
            ///     Enumerates the child windows that belong to the specified parent window by passing the handle
            ///     to each child window, in turn, to an application-defined callback function. EnumChildWindows
            ///     continues until the last child window is enumerated or the callback function returns FALSE.
            /// </summary>
            /// <param name="hWndParent">
            ///     A handle to the parent window whose child windows are to be enumerated. If this parameter is
            ///     NULL, this function is equivalent to EnumWindows.
            /// </param>
            /// <param name="lpEnumFunc">
            ///     A pointer to an application-defined callback function.
            /// </param>
            /// <param name="lParam">
            ///     An application-defined value to be passed to the callback function.
            /// </param>
            /// <returns>
            ///     The return value is not used.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);

            /// <summary>
            ///     Creates an array of handles to large or small icons extracted from the specified executable
            ///     file, DLL, or icon file.
            /// </summary>
            /// <param name="lpszFile">
            ///     The name of an executable file, DLL, or icon file from which icons will be extracted.
            /// </param>
            /// <param name="nIconIndex">
            ///     The zero-based index of the first icon to extract. For example, if this value is zero, the
            ///     function extracts the first icon in the specified file.
            ///     <para>
            ///         If this value is –1 and phiconLarge and phiconSmall are both NULL, the function returns the
            ///         total number of icons in the specified file. If the file is an executable file or DLL, the
            ///         return value is the number of RT_GROUP_ICON resources. If the file is an .ico file, the
            ///         return value is 1.
            ///     </para>
            ///     <para>
            ///         If this value is a negative number and either phiconLarge or phiconSmall is not NULL, the
            ///         function begins by extracting the icon whose resource identifier is equal to the absolute
            ///         value of nIconIndex. For example, use -3 to extract the icon whose resource identifier is 3.
            ///     </para>
            /// </param>
            /// <param name="phiconLarge">
            ///     An array of icon handles that receives handles to the large icons extracted from the file. If
            ///     this parameter is NULL, no large icons are extracted from the file.
            /// </param>
            /// <param name="phiconSmall">
            ///     An array of icon handles that receives handles to the small icons extracted from the file. If
            ///     this parameter is NULL, no small icons are extracted from the file.
            /// </param>
            /// <param name="nIcons">
            ///     The number of icons to be extracted from the file.
            /// </param>
            /// <returns>
            ///     If the nIconIndex parameter is -1, the phiconLarge parameter is NULL, and the phiconSmall
            ///     parameter is NULL, then the return value is the number of icons contained in the specified file.
            ///     Otherwise, the return value is the number of icons successfully extracted from the file.
            /// </returns>
            [DllImport(DllNames.Shell32, BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            public static extern int ExtractIconEx([MarshalAs(UnmanagedType.LPStr)] string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, int nIcons);

            /// <summary>
            ///     Retrieves a handle to the top-level window whose class name and window name match the specified
            ///     strings. This function does not search child windows. This function does not perform a
            ///     case-sensitive search.
            /// </summary>
            /// <param name="lpClassName">
            ///     The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx
            ///     function. The atom must be in the low-order word of lpClassName; the high-order word must be zero.
            ///     <para>
            ///         If lpClassName points to a string, it specifies the window class name. The class name can be
            ///         any name registered with RegisterClass or RegisterClassEx, or any of the predefined
            ///         control-class names.
            ///     </para>
            ///     <para>
            ///         If lpClassName is NULL, it finds any window whose title matches the lpWindowName parameter.
            ///     </para>
            /// </param>
            /// <param name="lpWindowName">
            ///     The window name (the window's title). If this parameter is NULL, all window names match.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the window that has the specified class
            ///     name and window name.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            /// <summary>
            ///     Retrieves a handle to the top-level window whose window name match the specified strings. This
            ///     function does not search child windows. This function does not perform a case-sensitive search.
            /// </summary>
            /// <param name="zeroOnly">
            ///     Must be <see cref="IntPtr.Zero"/>.
            /// </param>
            /// <param name="lpWindowName">
            ///     The window name (the window's title). If this parameter is NULL, all window names match.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the window that has the specified window
            ///     name.
            /// </returns>
            [DllImport(DllNames.User32, EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

            /// <summary>
            ///     Retrieves a handle to a window whose class name and window name match the specified strings. The
            ///     function searches child windows, beginning with the one following the specified child window. This
            ///     function does not perform a case-sensitive search.
            /// </summary>
            /// <param name="hwndParent">
            ///     A handle to the parent window whose child windows are to be searched.
            ///     <para>
            ///         If hwndParent is NULL, the function uses the desktop window as the parent window. The function
            ///         searches among windows that are child windows of the desktop.
            ///     </para>
            ///     <para>
            ///         If hwndParent is HWND_MESSAGE, the function searches all message-only windows.
            ///     </para>
            /// </param>
            /// <param name="hwndChildAfter">
            ///     A handle to a child window. The search begins with the next child window in the Z order. The child
            ///     window must be a direct child window of hwndParent, not just a descendant window.
            ///     <para>
            ///         If hwndChildAfter is NULL, the search begins with the first child window of hwndParent.
            ///     </para>
            ///     <para>
            ///         Note that if both hwndParent and hwndChildAfter are NULL, the function searches all top-level
            ///         and message-only windows.
            ///     </para>
            /// </param>
            /// <param name="lpszClass">
            ///     The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx
            ///     function. The atom must be placed in the low-order word of lpszClass; the high-order word must be
            ///     zero.
            ///     <para>
            ///         If lpszClass is a string, it specifies the window class name. The class name can be any name
            ///         registered with RegisterClass or RegisterClassEx, or any of the predefined control-class names,
            ///         or it can be MAKEINTATOM (0x8000). In this latter case, 0x8000 is the atom for a menu class. For
            ///         more information, see the Remarks section of this topic.
            ///     </para>
            /// </param>
            /// <param name="lpszWindow">
            ///     The window name (the window's title). If this parameter is NULL, all window names match.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the window that has the specified class and
            ///     window names.
            /// </returns>
            [DllImport(DllNames.User32, EntryPoint = "FindWindowEx", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

            /// <summary>
            ///     Retrieves the name of the class to which the specified window belongs.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window and, indirectly, the class to which the window belongs.
            /// </param>
            /// <param name="lpClassName">
            ///     The class name string.
            /// </param>
            /// <param name="nMaxCount">
            ///     The length of the lpClassName buffer, in characters. The buffer must be large enough to include the
            ///     terminating null character; otherwise, the class name string is truncated to nMaxCount-1 characters.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the number of characters copied to the buffer, not
            ///     including the terminating null character.
            /// </returns>
            [DllImport(DllNames.User32, EntryPoint = "GetClassNameW", BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            public static extern int GetClassName(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpClassName, int nMaxCount);

            /// <summary>
            ///     Retrieves the coordinates of a window's client area. The client coordinates specify the upper-left
            ///     and lower-right corners of the client area. Because client coordinates are relative to the upper-left
            ///     corner of a window's client area, the coordinates of the upper-left corner are (0,0).
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose client coordinates are to be retrieved.
            /// </param>
            /// <param name="lpRect">
            ///     A pointer to a <see cref="Rectangle"/> structure that receives the client coordinates. The left and
            ///     top members are zero. The right and bottom members contain the width and height of the window.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

            /// <summary>
            ///     Retrieves the thread identifier of the calling thread.
            /// </summary>
            /// <returns>
            ///     The return value is the thread identifier of the calling thread.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern uint GetCurrentThreadId();

            /// <summary>
            ///     Retrieves the identifier of the specified control.
            /// </summary>
            /// <param name="hwndCtl">
            ///     A handle to the control.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the identifier of the control.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true, CharSet = CharSet.Auto)]
            public static extern int GetDlgCtrlID(IntPtr hwndCtl);

            /// <summary>
            ///     Retrieves a handle to a control in the specified dialog box.
            /// </summary>
            /// <param name="hDlg">
            ///     A handle to the dialog box that contains the control.
            /// </param>
            /// <param name="nIddlgItem">
            ///     The identifier of the control to be retrieved.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the window handle of the specified control.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true, CharSet = CharSet.Auto)]
            public static extern IntPtr GetDlgItem(IntPtr hDlg, int nIddlgItem);

            /// <summary>
            ///     Retrieves a handle to the foreground window (the window with which the user is currently working).
            ///     The system assigns a slightly higher priority to the thread that creates the foreground window
            ///     than it does to other threads.
            /// </summary>
            /// <returns>
            ///     The return value is a handle to the foreground window. The foreground window can be NULL in certain
            ///     circumstances, such as when a window is losing activation.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern IntPtr GetForegroundWindow();

            /// <summary>
            ///     Retrieves the calling thread's last-error code value. The last-error code is
            ///     maintained on a per-thread basis. Multiple threads do not overwrite each
            ///     other's last-error code.
            /// </summary>
            /// <returns>
            ///     The return value is the calling thread's last-error code.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern int GetLastError();

            /// <summary>
            ///     Retrieves a handle to the menu assigned to the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose menu handle is to be retrieved.
            /// </param>
            /// <returns>
            ///     The return value is a handle to the menu. If the specified window has no menu, the return value is
            ///     NULL. If the window is a child window, the return value is undefined.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern IntPtr GetMenu(IntPtr hWnd);

            /// <summary>
            ///     Determines the number of items in the specified menu.
            /// </summary>
            /// <param name="hMenu">
            ///     A handle to the menu to be examined.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value specifies the number of items in the menu.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern int GetMenuItemCount(IntPtr hMenu);

            /// <summary>
            ///     Retrieves a handle to the specified window's parent or owner.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose parent window handle is to be retrieved.
            /// </param>
            /// <returns>
            ///     If the window is a child window, the return value is a handle to the parent window. If the window
            ///     is a top-level window with the WS_POPUP style, the return value is a handle to the owner window.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern IntPtr GetParent(IntPtr hWnd);

            /// <summary>
            ///     Retrieves the process identifier of the specified process.
            /// </summary>
            /// <param name="handle">
            ///     A handle to the process.
            /// </param>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern uint GetProcessId(IntPtr handle);

            /// <summary>
            ///     Retrieves the name of the executable file for the specified process.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process. The handle must have the
            ///     <see cref="AccessRights.PROCESS_QUERY_INFORMATION"/> or
            ///     <see cref="AccessRights.PROCESS_QUERY_LIMITED_INFORMATION"/> access right.
            /// </param>
            /// <param name="lpImageFileName">
            ///     A pointer to a buffer that receives the full path to the executable file.
            /// </param>
            /// <param name="nSize">
            ///     The size of the lpImageFileName buffer, in characters.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value specifies the length of the string
            ///     copied to the buffer.
            /// </returns>
            [DllImport(DllNames.Psapi, SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern bool GetProcessImageFileName(IntPtr hProcess, StringBuilder lpImageFileName, int nSize);

            /// <summary>
            ///     Retrieves a handle to the specified standard device (standard input, standard
            ///     output, or standard error).
            /// </summary>
            /// <param name="nStdHandle">
            ///     The standard device. This parameter can be one of the following values.
            ///     <para>
            ///         STD_INPUT_HANDLE (DWORD)-10: The standard input device. Initially, this is
            ///         the console input buffer, CONIN$.
            ///     </para>
            ///     <para>
            ///         STD_OUTPUT_HANDLE (DWORD)-11: The standard output device. Initially, this
            ///         is the active console screen buffer, CONOUT$.
            ///     </para>
            ///     <para>
            ///         STD_ERROR_HANDLE (DWORD)-12: The standard error device. Initially, this is
            ///         the active console screen buffer, CONOUT$.
            ///     </para>
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the specified device,
            ///     or a redirected handle set by a previous call to SetStdHandle. The handle has
            ///     GENERIC_READ and GENERIC_WRITE access rights, unless the application has used
            ///     SetStdHandle to set a standard handle with lesser access.
            /// </returns>
            [DllImport(DllNames.Kernel32, EntryPoint = "GetStdHandle", SetLastError = true)]
            public static extern IntPtr GetStdHandle(int nStdHandle);

            /// <summary>
            ///     Enables the application to access the window menu (also known as the system menu or the control
            ///     menu) for copying and modifying.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window that will own a copy of the window menu.
            /// </param>
            /// <param name="bRevert">
            ///     The action to be taken. If this parameter is FALSE, GetSystemMenu returns a handle to the copy of
            ///     the window menu currently in use. The copy is initially identical to the window menu, but it can
            ///     be modified. If this parameter is TRUE, GetSystemMenu resets the window menu back to the default
            ///     state. The previous window menu, if any, is destroyed.
            /// </param>
            /// <returns>
            ///     If the bRevert parameter is FALSE, the return value is a handle to a copy of the window menu. If
            ///     the bRevert parameter is TRUE, the return value is NULL.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

            /// <summary>
            ///     Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD)
            ///     value at the specified offset into the extra window memory.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window and, indirectly, the class to which the window belongs.
            /// </param>
            /// <param name="nIndex">
            ///     The zero-based offset to the value to be retrieved. Valid values are in the range zero through the
            ///     number of bytes of extra window memory, minus four; for example, if you specified 12 or more bytes
            ///     of extra memory, a value of 8 would be an index to the third 32-bit integer. To retrieve any other
            ///     value, specify one of the <see cref="WindowLongFunc"/>.GWL_??? values.
            /// </param>
            /// <returns>
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern int GetWindowLong(IntPtr hWnd, WindowLongFunc nIndex);

            /// <summary>
            ///     Gets the show state and the restored, minimized, and maximized positions of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="lpwndpl">
            ///     A pointer to a <see cref="WINDOWPLACEMENT"/> structure that specifies the new show state and window
            ///     positions.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

            /// <summary>
            ///     Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given
            ///     in screen coordinates that are relative to the upper-left corner of the screen.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="lpRect">
            ///     A pointer to a <see cref="Rectangle"/> structure that receives the screen coordinates of the
            ///     upper-left and lower-right corners of the window.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

            /// <summary>
            ///     Copies the text of the specified window's title bar (if it has one) into a buffer. If the specified
            ///     window is a control, the text of the control is copied. However, GetWindowText cannot retrieve the
            ///     text of a control in another application.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window or control containing the text.
            /// </param>
            /// <param name="text">
            ///     The buffer that will receive the text. If the string is as long or longer than the buffer, the string
            ///     is truncated and terminated with a null character.
            /// </param>
            /// <param name="maxLength">
            ///     The maximum number of characters to copy to the buffer, including the null character. If the text
            ///     exceeds this limit, it is truncated.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the length, in characters, of the copied string, not
            ///     including the terminating null character. If the window has no title bar or text, if the title bar is
            ///     empty, or if the window or control handle is invalid, the return value is zero.
            /// </returns>
            [DllImport(DllNames.User32, EntryPoint = "GetWindowTextW", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

            /// <summary>
            ///     Retrieves the length, in characters, of the specified window's title bar text (if the window has a
            ///     title bar). If the specified window is a control, the function retrieves the length of the text within
            ///     the control. However, GetWindowTextLength cannot retrieve the length of the text of an edit control in
            ///     another application.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window or control.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the length, in characters, of the text. Under certain
            ///     conditions, this value may actually be greater than the length of the text.
            /// </returns>
            [DllImport(DllNames.User32, EntryPoint = "GetWindowTextLengthW", SetLastError = true)]
            public static extern int GetWindowTextLength(IntPtr hWnd);

            /// <summary>
            ///     Retrieves the identifier of the thread that created the specified window and, optionally, the identifier
            ///     of the process that created the window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="lpdwProcessId">
            ///     A pointer to a variable that receives the process identifier. If this parameter is not NULL,
            ///     GetWindowThreadProcessId copies the identifier of the process to the variable; otherwise, it does not.
            /// </param>
            /// <returns>
            ///     The return value is the identifier of the thread that created the window.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

            /// <summary>
            ///     Note  The InsertMenu function has been superseded by the InsertMenuItem function. You can still use
            ///     InsertMenu, however, if you do not need any of the extended features of InsertMenuItem.
            /// </summary>
            /// <param name="hMenu">
            ///     A handle to the menu to be changed.
            /// </param>
            /// <param name="wPosition">
            ///     The menu item before which the new menu item is to be inserted, as determined by the uFlags parameter.
            /// </param>
            /// <param name="wFlags">
            ///     Controls the interpretation of the uPosition parameter and the content, appearance, and behavior of the
            ///     new menu item.
            /// </param>
            /// <param name="wIdNewItem">
            ///     The identifier of the new menu item or, if the uFlags parameter has the <see cref="ModifyMenuFunc.MF_POPUP"/>
            ///     flag set, a handle to the drop-down menu or submenu.
            /// </param>
            /// <param name="lpNewItem">
            ///     The content of the new menu item. The interpretation of lpNewItem depends on whether the uFlags parameter
            ///     includes the <see cref="ModifyMenuFunc.MF_BITMAP"/>, <see cref="ModifyMenuFunc.MF_OWNERDRAW"/>, or
            ///     <see cref="ModifyMenuFunc.MF_STRING"/> flag, as follows.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            public static extern bool InsertMenu(IntPtr hMenu, uint wPosition, ModifyMenuFunc wFlags, UIntPtr wIdNewItem, [MarshalAs(UnmanagedType.LPStr)] string lpNewItem);

            /// <summary>
            ///     Loads the specified module into the address space of the calling process. The
            ///     specified module may cause other modules to be loaded.
            /// </summary>
            /// <param name="lpFileName">
            ///     The name of the module. This can be either a library module (a .dll file) or an
            ///     executable module (an .exe file). The name specified is the file name of the
            ///     module and is not related to the name stored in the library module itself, as
            ///     specified by the LIBRARY keyword in the module-definition (.def) file.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the module.
            /// </returns>
            [DllImport(DllNames.Kernel32, BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

            /// <summary>
            ///     Loads a string resource from the executable file associated with a specified module, copies the string into
            ///     a buffer, and appends a terminating null character.
            /// </summary>
            /// <param name="hInstance">
            ///     A handle to an instance of the module whose executable file contains the string resource. To get the handle
            ///     to the application itself, call the GetModuleHandle function with NULL.
            /// </param>
            /// <param name="uId">
            ///     The identifier of the string to be loaded.
            /// </param>
            /// <param name="lpBuffer">
            ///     The buffer is to receive the string. Must be of sufficient length to hold a pointer (8 bytes).
            /// </param>
            /// <param name="nBufferMax">
            ///     The size of the buffer, in characters. The string is truncated and null-terminated if it is longer than the
            ///     number of characters specified. If this parameter is 0, then lpBuffer receives a read-only pointer to the
            ///     resource itself.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the number of characters copied into the buffer, not including
            ///     the terminating null character, or zero if the string resource does not exist.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern int LoadString(IntPtr hInstance, uint uId, StringBuilder lpBuffer, int nBufferMax);

            /// <summary>
            ///     Allocates the specified number of bytes from the heap.
            /// </summary>
            /// <param name="flag">
            ///     The memory allocation attributes. The default is the LMEM_FIXED value. This
            ///     parameter can be one or more of the <see cref="LocalAllocFuncAttr"/>.
            /// </param>
            /// <param name="size">
            ///     The number of bytes to allocate. If this parameter is zero and the uFlags
            ///     parameter specifies <see cref="LocalAllocFuncAttr.LMEM_MOVEABLE"/>, the function
            ///     returns a handle to a memory object that is marked as discarded.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the newly allocated
            ///     memory object.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern IntPtr LocalAlloc(LocalAllocFuncAttr flag, UIntPtr size);

            /// <summary>
            ///     Frees the specified local memory object and invalidates its handle.
            /// </summary>
            /// <param name="hMem">
            ///     A handle to the local memory object. This handle is returned by either the
            ///     <see cref="LocalAlloc"/> function.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is NULL.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern IntPtr LocalFree(IntPtr hMem);

            /// <summary>
            ///     Changes the position and dimensions of the specified window. For a top-level window, the position and
            ///     dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to
            ///     the upper-left corner of the parent window's client area.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="x">
            ///     The new position of the left side of the window.
            /// </param>
            /// <param name="y">
            ///     The new position of the top of the window.
            /// </param>
            /// <param name="nWidth">
            ///     The new width of the window.
            /// </param>
            /// <param name="nHeight">
            ///     The new height of the window.
            /// </param>
            /// <param name="bRepaint">
            ///     Indicates whether the window is to be repainted. If this parameter is TRUE, the window receives a message.
            ///     If the parameter is FALSE, no repainting of any kind occurs. This applies to the client area, the nonclient
            ///     area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of
            ///     moving a child window.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

            /// <summary>
            ///     Retrieves information about the specified process.
            /// </summary>
            /// <param name="hndl">
            ///     A handle to the process for which information is to be retrieved.
            /// </param>
            /// <param name="piCl">
            ///     The type of process information to be retrieved.
            /// </param>
            /// <param name="processInformation">
            ///     A pointer to a buffer supplied by the calling application into which the function writes the requested
            ///     information. The size of the information written varies depending on the data type of the
            ///     <see cref="PROCESS_BASIC_INFORMATION"/> parameter.
            /// </param>
            /// <param name="piLen">
            ///     The size of the buffer pointed to by the <see cref="PROCESS_BASIC_INFORMATION"/> parameter, in bytes.
            /// </param>
            /// <param name="rLen">
            ///     A pointer to a variable in which the function returns the size of the requested information. If the function
            ///     was successful, this is the size of the information written to the buffer pointed to by the
            ///     <see cref="PROCESS_BASIC_INFORMATION"/> parameter, but if the buffer was too small, this is the minimum size
            ///     of buffer needed to receive the information successfully.
            /// </param>
            /// <returns>
            ///     The function returns an NTSTATUS success or error code.
            /// </returns>
            [DllImport(DllNames.Ntdll, SetLastError = false)]
            public static extern uint NtQueryInformationProcess([In] IntPtr hndl, [In] ProcessInfoFunc piCl, [Out] out PROCESS_BASIC_INFORMATION processInformation, [In] uint piLen, [Out] out IntPtr rLen);

            /// <summary>
            ///     Opens an existing local process object.
            /// </summary>
            /// <param name="dwDesiredAccess">
            ///     The access to the process object. This access right is checked against the
            ///     security descriptor for the process. This parameter can be one or more of the
            ///     process access rights.
            /// </param>
            /// <param name="bInheritHandle">
            ///     If this value is TRUE, processes created by this process will inherit the handle.
            ///     Otherwise, the processes do not inherit this handle.
            /// </param>
            /// <param name="dwProcessId">
            ///     The identifier of the local process to be opened.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is an open handle to the specified
            ///     process.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

            /// <summary>
            ///     Places (posts) a message in the message queue associated with the thread that created the
            ///     specified window and returns without waiting for the thread to process the message.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose window procedure is to receive the message. The following values
            ///     have special meanings.
            ///     <para>
            ///         <c>
            ///             HWND_BROADCAST ((HWND)0xffff):
            ///         </c>
            ///         The message is posted to all top-level windows in the system, including disabled or invisible
            ///         unowned windows, overlapped windows, and pop-up windows. The message is not posted to child
            ///         windows.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             NULL:
            ///         </c>
            ///         The function behaves like a call to PostThreadMessage with the dwThreadId parameter set to
            ///         the identifier of the current thread.
            ///     </para>
            /// </param>
            /// <param name="msg">
            ///     The message to be posted.
            /// </param>
            /// <param name="wParam">
            ///     Additional message-specific information.
            /// </param>
            /// <param name="lParam">
            ///     Additional message-specific information.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport(DllNames.User32, SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool PostMessage(HandleRef hWnd, uint msg, IntPtr wParam, IntPtr lParam);

            /// <summary>
            ///     Reads data from an area of memory in a specified process. The entire area to be
            ///     read must be accessible or the operation fails.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process with memory that is being read. The handle must have
            ///     <see cref="AccessRights.PROCESS_VM_READ"/> access to the process.
            /// </param>
            /// <param name="lpBaseAddress">
            ///     A pointer to the base address in the specified process from which to read. Before
            ///     any data transfer occurs, the system verifies that all data in the base address
            ///     and memory of the specified size is accessible for read access, and if it is not
            ///     accessible the function fails.
            /// </param>
            /// <param name="lpBuffer">
            ///     A pointer to a buffer that receives the contents from the address space of the
            ///     specified process.
            /// </param>
            /// <param name="nSize">
            ///     The number of bytes to be read from the specified process.
            /// </param>
            /// <param name="lpNumberOfBytesRead">
            ///     A pointer to a variable that receives the number of bytes transferred into the
            ///     specified buffer. If lpNumberOfBytesRead is NULL, the parameter is ignored.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesRead);

            /// <summary>
            ///     Reads data from an area of memory in a specified process. The entire area to be
            ///     read must be accessible or the operation fails.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process with memory that is being read. The handle must have
            ///     <see cref="AccessRights.PROCESS_VM_READ"/> access to the process.
            /// </param>
            /// <param name="lpBaseAddress">
            ///     A pointer to the base address in the specified process from which to read. Before
            ///     any data transfer occurs, the system verifies that all data in the base address
            ///     and memory of the specified size is accessible for read access, and if it is not
            ///     accessible the function fails.
            /// </param>
            /// <param name="lpBuffer">
            ///     A pointer to a buffer that receives the contents from the address space of the
            ///     specified process.
            /// </param>
            /// <param name="nSize">
            ///     The number of bytes to be read from the specified process.
            /// </param>
            /// <param name="lpNumberOfBytesRead">
            ///     A pointer to a variable that receives the number of bytes transferred into the
            ///     specified buffer. If lpNumberOfBytesRead is NULL, the parameter is ignored.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, StringBuilder lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesRead);

            /// <summary>
            ///     Releases the mouse capture from a window in the current thread and restores normal mouse
            ///     input processing. A window that has captured the mouse receives all mouse input, regardless
            ///     of the position of the cursor, except when a mouse button is clicked while the cursor hot
            ///     spot is in the window of another thread.
            /// </summary>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool ReleaseCapture();

            /// <summary>
            ///     Deletes a menu item or detaches a submenu from the specified menu. If the menu item
            ///     opens a drop-down menu or submenu, RemoveMenu does not destroy the menu or its handle,
            ///     allowing the menu to be reused. Before this function is called, the GetSubMenu function
            ///     should retrieve a handle to the drop-down menu or submenu.
            /// </summary>
            /// <param name="hMenu">
            ///     A handle to the menu to be changed.
            /// </param>
            /// <param name="uPosition">
            ///     The menu item to be deleted, as determined by the uFlags parameter.
            /// </param>
            /// <param name="uFlags">
            ///     Indicates how the uPosition parameter is interpreted. This parameter must be
            ///     <see cref="ModifyMenuFunc.MF_BYCOMMAND"/> or <see cref="ModifyMenuFunc.MF_BYPOSITION"/>.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, ModifyMenuFunc uFlags);

            /// <summary>
            ///     Synthesizes keystrokes, mouse motions, and button clicks.
            /// </summary>
            /// <param name="nInputs">
            ///     The number of structures in the pInputs array.
            /// </param>
            /// <param name="pInputs">
            ///     An array of <see cref="INPUT"/> structures. Each structure represents an event
            ///     to be inserted into the keyboard or mouse input stream.
            /// </param>
            /// <param name="cbSize">
            ///     The size, in bytes, of an <see cref="INPUT"/> structure. If cbSize is not the
            ///     size of an <see cref="INPUT"/> structure, the function fails.
            /// </param>
            /// <returns>
            ///     The function returns the number of events that it successfully inserted into
            ///     the keyboard or mouse input stream. If the function returns zero, the input
            ///     was already blocked by another thread.
            /// </returns>
            [DllImport(DllNames.User32)]
            public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray)][In] INPUT[] pInputs, int cbSize);

            /// <summary>
            ///     Sends the specified message to a window or windows. The SendMessage function
            ///     calls the window procedure for the specified window and does not return until
            ///     the window procedure has processed the message.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose window procedure will receive the message. If this
            ///     parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level
            ///     windows in the system, including disabled or invisible unowned windows, overlapped
            ///     windows, and pop-up windows; but the message is not sent to child windows.
            /// </param>
            /// <param name="uMsg">
            ///     The message to be sent.
            /// </param>
            /// <param name="wParam">
            ///     Additional message-specific information.
            /// </param>
            /// <param name="lParam">
            ///     Additional message-specific information.
            /// </param>
            /// <returns>
            ///     The return value specifies the result of the message processing; it depends on the
            ///     message sent.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

            /// <summary>
            ///     Sends the specified message to a window or windows. The SendMessage function
            ///     calls the window procedure for the specified window and does not return until
            ///     the window procedure has processed the message.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose window procedure will receive the message. If this
            ///     parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level
            ///     windows in the system, including disabled or invisible unowned windows, overlapped
            ///     windows, and pop-up windows; but the message is not sent to child windows.
            /// </param>
            /// <param name="uMsg">
            ///     The message to be sent.
            /// </param>
            /// <param name="wParam">
            ///     Additional message-specific information.
            /// </param>
            /// <param name="lParam">
            ///     Additional message-specific information.
            /// </param>
            /// <returns>
            ///     The return value specifies the result of the message processing; it depends on the
            ///     message sent.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, ref COPYDATASTRUCT lParam);

            /// <summary>
            ///     Sends the specified message to one or more windows.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose window procedure will receive the message.
            ///     <para>
            ///         If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all
            ///         top-level windows in the system, including disabled or invisible unowned windows.
            ///         The function does not return until each window has timed out. Therefore, the total
            ///         wait time can be up to the value of uTimeout multiplied by the number of top-level
            ///         windows.
            ///     </para>
            /// </param>
            /// <param name="msg">
            ///     The message to be sent.
            /// </param>
            /// <param name="wParam">
            ///     Any additional message-specific information.
            /// </param>
            /// <param name="lParam">
            ///     Any additional message-specific information.
            /// </param>
            /// <param name="fuFlags">
            ///     The behavior of this function. This parameter can be one or more of the following values.
            ///     <para>
            ///         <c>
            ///             SMTO_ABORTIFHUNG (0x0002):
            ///         </c>
            ///         The function returns without waiting for the time-out period to elapse if the receiving
            ///         thread appears to not respond or hangs.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             SMTO_BLOCK (0x0001):
            ///         </c>
            ///         Prevents the calling thread from processing any other requests until the function returns.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             SMTO_NORMAL (0x0000):
            ///         </c>
            ///         The calling thread is not prevented from processing other requests while waiting for the
            ///         function to return.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             SMTO_NOTIMEOUTIFNOTHUNG (0x0008):
            ///         </c>
            ///         The function does not enforce the time-out period as long as the receiving thread is
            ///         processing messages.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             SMTO_ERRORONEXIT (0x0020):
            ///         </c>
            ///         The function should return 0 if the receiving window is destroyed or its owning thread dies
            ///         while the message is being processed.
            ///     </para>
            /// </param>
            /// <param name="uTimeout">
            ///     The duration of the time-out period, in milliseconds. If the message is a broadcast message,
            ///     each window can use the full time-out period. For example, if you specify a five second time-out
            ///     period and there are three top-level windows that fail to process the message, you could have up
            ///     to a 15 second delay.
            /// </param>
            /// <param name="lpdwResult">
            ///     The result of the message processing. The value of this parameter depends on the message that is
            ///     specified.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero. SendMessageTimeout does not provide
            ///     information about individual windows timing out if HWND_BROADCAST ((HWND)0xffff) is used.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, out UIntPtr lpdwResult);

            /// <summary>
            ///     Sends the specified message to one or more windows.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose window procedure will receive the message.
            ///     <para>
            ///         If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level
            ///         windows in the system, including disabled or invisible unowned windows. The function does
            ///         not return until each window has timed out. Therefore, the total wait time can be up to the
            ///         value of uTimeout multiplied by the number of top-level windows.
            ///     </para>
            /// </param>
            /// <param name="msg">
            ///     The message to be sent.
            /// </param>
            /// <param name="wParam">
            ///     Any additional message-specific information.
            /// </param>
            /// <param name="lParam">
            ///     Any additional message-specific information.
            /// </param>
            /// <param name="fuFlags">
            ///     The behavior of this function. This parameter can be one or more of the following values.
            ///     <para>
            ///         <c>
            ///             SMTO_ABORTIFHUNG (0x0002):
            ///         </c>
            ///         The function returns without waiting for the time-out period to elapse if the receiving
            ///         thread appears to not respond or hangs.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             SMTO_BLOCK (0x0001):
            ///         </c>
            ///         Prevents the calling thread from processing any other requests until the function returns.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             SMTO_NORMAL (0x0000):
            ///         </c>
            ///         The calling thread is not prevented from processing other requests while waiting for the
            ///         function to return.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             SMTO_NOTIMEOUTIFNOTHUNG (0x0008):
            ///         </c>
            ///         The function does not enforce the time-out period as long as the receiving thread is
            ///         processing messages.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             SMTO_ERRORONEXIT (0x0020):
            ///         </c>
            ///         The function should return 0 if the receiving window is destroyed or its owning thread dies
            ///         while the message is being processed.
            ///     </para>
            /// </param>
            /// <param name="uTimeout">
            ///     The duration of the time-out period, in milliseconds. If the message is a broadcast message,
            ///     each window can use the full time-out period. For example, if you specify a five second time-out
            ///     period and there are three top-level windows that fail to process the message, you could have up
            ///     to a 15 second delay.
            /// </param>
            /// <param name="lpdwResult">
            ///     The result of the message processing. The value of this parameter depends on the message that is
            ///     specified.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero. SendMessageTimeout does not provide
            ///     information about individual windows timing out if HWND_BROADCAST ((HWND)0xffff) is used.
            /// </returns>
            [DllImport(DllNames.User32, EntryPoint = "SendMessageTimeout", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr SendMessageTimeoutText(IntPtr hWnd, uint msg, UIntPtr wParam, StringBuilder lParam, uint fuFlags, uint uTimeout, out IntPtr lpdwResult);

            /// <summary>
            ///     Sends the specified message to a window or windows. If the window was created by the calling
            ///     thread, SendNotifyMessage calls the window procedure for the window and does not return until
            ///     the window procedure has processed the message. If the window was created by a different thread,
            ///     SendNotifyMessage passes the message to the window procedure and returns immediately; it does
            ///     not wait for the window procedure to finish processing the message.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose window procedure will receive the message. If this parameter is
            ///     HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system,
            ///     including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but
            ///     the message is not sent to child windows.
            /// </param>
            /// <param name="msg">
            ///     The message to be sent.
            /// </param>
            /// <param name="wParam">
            ///     Additional message-specific information.
            /// </param>
            /// <param name="lParam">
            ///     Additional message-specific information.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true, BestFitMapping = false, CharSet = CharSet.Unicode)]
            public static extern bool SendNotifyMessage(IntPtr hWnd, uint msg, UIntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

            /// <summary>
            ///     Moves the cursor to the specified screen coordinates. If the new coordinates are not within
            ///     the screen rectangle set by the most recent ClipCursor function call, the system automatically
            ///     adjusts the coordinates so that the cursor stays within the rectangle.
            /// </summary>
            /// <param name="x">
            ///     The new x-coordinate of the cursor, in screen coordinates.
            /// </param>
            /// <param name="y">
            ///     The new y-coordinate of the cursor, in screen coordinates.
            /// </param>
            /// <returns>
            ///     Returns nonzero if successful or zero otherwise.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern uint SetCursorPos(uint x, uint y);

            /// <summary>
            ///     Brings the thread that created the specified window into the foreground and activates the window.
            ///     Keyboard input is directed to the window, and various visual cues are changed for the user. The
            ///     system assigns a slightly higher priority to the thread that created the foreground window than
            ///     it does to other threads.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window that should be activated and brought to the foreground.
            /// </param>
            /// <returns>
            ///     If the window was brought to the foreground, the return value is nonzero.
            ///     <para>
            ///         If the window was not brought to the foreground, the return value is zero.
            ///     </para>
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            /// <summary>
            ///     Changes the parent window of the specified child window.
            /// </summary>
            /// <param name="hWndChild">
            ///     A handle to the child window.
            /// </param>
            /// <param name="hWndNewParent">
            ///     A handle to the new parent window. If this parameter is NULL, the desktop window becomes the new
            ///     parent window. If this parameter is HWND_MESSAGE, the child window becomes a message-only window.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the previous parent window.
            ///     <para>
            ///         If the function fails, the return value is NULL.
            ///     </para>
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            /// <summary>
            ///     Sets the minimum and maximum working set sizes for the specified process.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process whose working set sizes is to be set.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.PROCESS_SET_QUOTA"/> access right.
            ///     </para>
            /// </param>
            /// <param name="dwMinimumWorkingSetSize">
            ///     The minimum working set size for the process, in bytes. The virtual memory manager attempts
            ///     to keep at least this much memory resident in the process whenever the process is active.
            ///     <para>
            ///         This parameter must be greater than zero but less than or equal to the maximum working
            ///         set size. The default size is 50 pages (for example, this is 204,800 bytes on systems
            ///         with a 4K page size). If the value is greater than zero but less than 20 pages, the
            ///         minimum value is set to 20 pages.
            ///     </para>
            ///     <para>
            ///         If both dwMinimumWorkingSetSize and dwMaximumWorkingSetSize have the value (SIZE_T)–1,
            ///         the function removes as many pages as possible from the working set of the specified
            ///         process.
            ///     </para>
            /// </param>
            /// <param name="dwMaximumWorkingSetSize">
            ///     The maximum working set size for the process, in bytes. The virtual memory manager attempts
            ///     to keep no more than this much memory resident in the process whenever the process is active
            ///     and available memory is low.
            ///     <para>
            ///         This parameter must be greater than or equal to 13 pages (for example, 53,248 on systems
            ///         with a 4K page size), and less than the system-wide maximum (number of available pages
            ///         minus 512 pages). The default size is 345 pages (for example, this is 1,413,120 bytes on
            ///         systems with a 4K page size).
            ///     </para>
            ///     <para>
            ///         If both dwMinimumWorkingSetSize and dwMaximumWorkingSetSize have the value (SIZE_T)–1,
            ///         the function removes as many pages as possible from the working set of the specified
            ///         process.
            ///     </para>
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32)]
            public static extern bool SetProcessWorkingSetSize(IntPtr hProcess, UIntPtr dwMinimumWorkingSetSize, UIntPtr dwMaximumWorkingSetSize);

            /// <summary>
            ///     Creates a timer with the specified time-out value.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window to be associated with the timer. This window must be owned by the calling
            ///     thread. If a NULL value for hWnd is passed in along with an nIDEvent of an existing timer, that
            ///     timer will be replaced in the same way that an existing non-NULL hWnd timer will be.
            /// </param>
            /// <param name="nIdEvent">
            ///     A nonzero timer identifier. If the hWnd parameter is NULL, and the nIDEvent does not match an
            ///     existing timer then it is ignored and a new timer ID is generated. If the hWnd parameter is not
            ///     NULL and the window specified by hWnd already has a timer with the value nIDEvent, then the
            ///     existing timer is replaced by the new timer. When SetTimer replaces a timer, the timer is reset.
            ///     Therefore, a message will be sent after the current time-out value elapses, but the previously
            ///     set time-out value is ignored. If the call is not intended to replace an existing timer, nIDEvent
            ///     should be 0 if the hWnd is NULL.
            /// </param>
            /// <param name="uElapse">
            ///     The time-out value, in milliseconds.
            /// </param>
            /// <param name="lpTimerFunc">
            ///     A pointer to the function to be notified when the time-out value elapses.
            /// </param>
            /// <returns>
            ///     If the function succeeds and the hWnd parameter is NULL, the return value is an integer
            ///     identifying the new timer. An application can pass this value to the KillTimer function to destroy
            ///     the timer.
            ///     <para>
            ///         If the function succeeds and the hWnd parameter is not NULL, then the return value is a nonzero
            ///         integer. An application can pass the value of the nIDEvent parameter to the KillTimer function
            ///         to destroy the timer.
            ///     </para>
            ///     <para>
            ///         If the function fails to create a timer, the return value is zero.
            ///     </para>
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIdEvent, uint uElapse, TimerProc lpTimerFunc);

            /// <summary>
            ///     Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the
            ///     specified offset into the extra window memory.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window and, indirectly, the class to which the window belongs.
            /// </param>
            /// <param name="nIndex">
            ///     The zero-based offset to the value to be set. Valid values are in the range zero through the number
            ///     of bytes of extra window memory, minus the size of an integer.
            /// </param>
            /// <param name="dwNewLong">
            ///     The replacement value.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the previous value of the specified 32-bit integer.
            /// </returns>
            //[DllImport(DllNames.User32, SetLastError = true)]
            //public static extern int SetWindowLong(IntPtr hWnd, WindowLongFunc nIndex, long dwNewLong);
            public static IntPtr SetWindowLongPtr(IntPtr hWnd, WindowLongFunc nIndex, IntPtr dwNewLong) =>
                IntPtr.Size == 4 ? SetWindowLongPtr32(hWnd, (int)nIndex, dwNewLong) : SetWindowLongPtr64(hWnd, (int)nIndex, dwNewLong);

            [DllImport(DllNames.User32, SetLastError = true, EntryPoint = "SetWindowLong")]
            private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport(DllNames.User32, SetLastError = true, EntryPoint = "SetWindowLongPtr")]
            private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            /// <summary>
            ///     Sets the show state and the restored, minimized, and maximized positions of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="lpwndpl">
            ///     A pointer to a <see cref="WINDOWPLACEMENT"/> structure that specifies the new show state and window
            ///     positions.
            ///     <para>
            ///         Before calling SetWindowPlacement, set the length member of the <see cref="WINDOWPLACEMENT"/>
            ///         structure to sizeof(<see cref="WINDOWPLACEMENT"/>). SetWindowPlacement fails if the length
            ///         member is not set correctly.
            ///     </para>
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

            /// <summary>
            ///     Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are
            ///     ordered according to their appearance on the screen. The topmost window receives the highest rank
            ///     and is the first window in the Z order.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="hWndInsertAfter">
            ///     A handle to the window to precede the positioned window in the Z order. This parameter must be a
            ///     window handle or one of the following values.
            ///     <para>
            ///         <c>
            ///             HWND_BOTTOM ((HWND)1):
            ///         </c>
            ///         Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost
            ///         window, the window loses its topmost status and is placed at the bottom of all other windows.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             HWND_NOTOPMOST ((HWND)-2):
            ///         </c>
            ///         Places the window above all non-topmost windows (that is, behind all topmost windows). This
            ///         flag has no effect if the window is already a non-topmost window.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             HWND_TOP ((HWND)0):
            ///         </c>
            ///         Places the window at the top of the Z order.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             HWND_TOPMOST ((HWND)-1):
            ///         </c>
            ///         Places the window above all non-topmost windows. The window maintains its topmost position even
            ///         when it is deactivated.
            ///     </para>
            /// </param>
            /// <param name="x">
            ///     The new position of the left side of the window, in client coordinates.
            /// </param>
            /// <param name="y">
            ///     The new position of the top of the window, in client coordinates.
            /// </param>
            /// <param name="cx">
            ///     The new width of the window, in pixels.
            /// </param>
            /// <param name="cy">
            ///     The new height of the window, in pixels.
            /// </param>
            /// <param name="uFlags">
            ///     The window sizing and positioning flags.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFunc uFlags);

            /// <summary>
            ///     Installs an application-defined hook procedure into a hook chain. You would install a hook
            ///     procedure to monitor the system for certain types of events. These events are associated
            ///     either with a specific thread or with all threads in the same desktop as the calling thread.
            /// </summary>
            /// <param name="idHook">
            ///     The type of hook procedure to be installed.
            /// </param>
            /// <param name="lpfn">
            ///     A pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier
            ///     of a thread created by a different process, the lpfn parameter must point to a hook procedure in
            ///     a DLL. Otherwise, lpfn can point to a hook procedure in the code associated with the current
            ///     process.
            /// </param>
            /// <param name="hMod">
            ///     A handle to the DLL containing the hook procedure pointed to by the lpfn parameter. The hMod
            ///     parameter must be set to NULL if the dwThreadId parameter specifies a thread created by the current
            ///     process and if the hook procedure is within the code associated with the current process.
            /// </param>
            /// <param name="dwThreadId">
            ///     The identifier of the thread with which the hook procedure is to be associated. For desktop apps,
            ///     if this parameter is zero, the hook procedure is associated with all existing threads running in the
            ///     same desktop as the calling thread. For Windows Store apps, see the Remarks section.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the handle to the hook procedure. If the function
            ///     fails, the return value is NULL.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern IntPtr SetWindowsHookEx(Win32HookFunc idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

            /// <summary>
            ///     Changes the text of the specified window's title bar (if it has one). If the specified window is a
            ///     control, the text of the control is changed. However, SetWindowText cannot change the text of a control
            ///     in another application.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window or control whose text is to be changed.
            /// </param>
            /// <param name="lpString">
            ///     The new title or control text.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, EntryPoint = "SetWindowTextW", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern bool SetWindowText(IntPtr hWnd, string lpString);

            /// <summary>
            ///     Changes the current directory for the current process.
            /// </summary>
            /// <param name="lpPathName">
            ///     The path to the new current directory.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true, BestFitMapping = false, CharSet = CharSet.Unicode)]
            public static extern bool SetCurrentDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName);

            /// <summary>
            ///     Sends an appbar message to the system.
            /// </summary>
            /// <param name="dwMessage">
            ///     Appbar message value to send.
            /// </param>
            /// <param name="pData">
            ///     A pointer to an <see cref="APPBARDATA"/> structure. The content of the structure on entry and on exit
            ///     depends on the value set in the dwMessage parameter. See the individual message pages for specifics.
            /// </param>
            /// <returns>
            ///     This function returns a message-dependent value. For more information, see the Windows SDK documentation
            ///     for the specific appbar message sent. Links to those documents are given in the See Also section.
            /// </returns>
            [DllImport(DllNames.Shell32, SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern UIntPtr SHAppBarMessage(AppBarMessageFunc dwMessage, ref APPBARDATA pData);

            /// <summary>
            ///     The ShowScrollBar function shows or hides the specified scroll bar.
            /// </summary>
            /// <param name="hwnd">
            ///     Handle to a scroll bar control or a window with a standard scroll bar, depending on the value of the
            ///     wBar parameter.
            /// </param>
            /// <param name="wBar">
            ///     Specifies the scroll bar(s) to be shown or hidden.
            /// </param>
            /// <param name="bShow">
            ///     Specifies whether the scroll bar is shown or hidden. If this parameter is TRUE, the scroll bar is shown;
            ///     otherwise, it is hidden.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern long ShowScrollBar(IntPtr hwnd, int wBar, [MarshalAs(UnmanagedType.Bool)] bool bShow);

            /// <summary>
            ///     Sets the specified window's show state.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="nCmdShow">
            ///     Controls how the window is to be shown. This parameter is ignored the first time an application calls
            ///     ShowWindow, if the program that launched the application provides a STARTUPINFO structure. Otherwise,
            ///     the first time ShowWindow is called, the value should be the value obtained by the WinMain function in
            ///     its nCmdShow parameter.
            /// </param>
            /// <returns>
            ///     If the window was previously visible, the return value is nonzero.
            ///     <para>
            ///         If the window was previously hidden, the return value is zero.
            ///     </para>
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool ShowWindow(IntPtr hWnd, ShowWindowFunc nCmdShow);

            /// <summary>
            ///     Sets the show state of a window without waiting for the operation to complete.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="nCmdShow">
            ///     Controls how the window is to be shown. For a list of possible values, see the description of the
            ///     ShowWindow function.
            /// </param>
            /// <returns>
            ///     If the operation was successfully started, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern bool ShowWindowAsync(IntPtr hWnd, ShowWindowFunc nCmdShow);

            /// <summary>
            ///     Terminates the specified process and all of its threads.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process to be terminated.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.PROCESS_TERMINATE"/> access right.
            ///     </para>
            /// </param>
            /// <param name="uExitCode">
            ///     The exit code to be used by the process and threads terminated as a result of this call.
            ///     Use the GetExitCodeProcess function to retrieve a process's exit value. Use the
            ///     GetExitCodeThread function to retrieve a thread's exit value.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

            /// <summary>
            ///     Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
            /// </summary>
            /// <param name="hhk">
            ///     A handle to the hook to be removed. This parameter is a hook handle obtained by a previous call
            ///     SetWindowsHookEx.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero. If the function fails, the return value
            ///     is zero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            public static extern int UnhookWindowsHookEx(IntPtr hhk);

            /// <summary>
            ///     Reserves, commits, or changes the state of a region of memory within the virtual address space
            ///     of a specified process. The function initializes the memory it allocates to zero.
            /// </summary>
            /// <param name="hProcess">
            ///     The handle to a process. The function allocates memory within the virtual address
            ///     space of this process.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.PROCESS_VM_OPERATION"/> access
            ///         right.
            ///     </para>
            /// </param>
            /// <param name="lpAddress">
            ///     The pointer that specifies a desired starting address for the region of pages
            ///     that you want to allocate.
            /// </param>
            /// <param name="dwSize">
            ///     The size of the region of memory to allocate, in bytes.
            /// </param>
            /// <param name="flAllocationType">
            ///     The type of memory allocation. This parameter must contain one of the following values.
            /// </param>
            /// <param name="flProtect">
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the base address of the allocated region
            ///     of pages.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, AllocationTypes flAllocationType, VirtualAllocFuncMemProtect flProtect);

            /// <summary>
            ///     Releases, decommits, or releases and decommits a region of memory within the virtual address
            ///     space of a specified process.
            /// </summary>
            /// <param name="hProcess">
            ///     The handle to a process. The function allocates memory within the virtual address space of this
            ///     process.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.PROCESS_VM_OPERATION"/> access
            ///         right.
            ///     </para>
            /// </param>
            /// <param name="lpAddress">
            ///     A pointer to the starting address of the region of memory to be freed.
            ///     <para>
            ///         If the dwFreeType parameter is <see cref="AllocationTypes.MEM_RELEASE"/>, lpAddress must be
            ///         the base address returned by the VirtualAllocEx function when the region is reserved.
            ///     </para>
            /// </param>
            /// <param name="dwSize">
            ///     The size of the region of memory to free, in bytes.
            ///     <para>
            ///         If the dwFreeType parameter is <see cref="AllocationTypes.MEM_RELEASE"/>, dwSize must
            ///         be 0 (zero). The function frees the entire region that is reserved in the initial
            ///         allocation call to VirtualAllocEx.
            ///     </para>
            ///     <para>
            ///         If dwFreeType is <see cref="AllocationTypes.MEM_DECOMMIT"/>, the function decommits all memory
            ///         pages that contain one or more bytes in the range from the lpAddress parameter to
            ///         (lpAddress+dwSize). This means, for example, that a 2-byte region of memory that straddles a
            ///         page boundary causes both pages to be decommitted. If lpAddress is the base address returned by
            ///         VirtualAllocEx and dwSize is 0 (zero), the function decommits the entire region that is
            ///         allocated by VirtualAllocEx. After that, the entire region is in the reserved state.
            ///     </para>
            /// </param>
            /// <param name="dwFreeType">
            ///     The type of free operation.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a nonzero value.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, MemFreeTypes dwFreeType);

            /// <summary>
            ///     Writes data to an area of memory in a specified process. The entire area to be written to must be
            ///     accessible or the operation fails.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process memory to be modified. The handle must have
            ///     <see cref="AccessRights.PROCESS_VM_WRITE"/> and
            ///     <see cref="AccessRights.PROCESS_VM_OPERATION"/> access to the process.
            /// </param>
            /// <param name="lpBaseAddress">
            ///     A pointer to the base address in the specified process to which data is written. Before data transfer
            ///     occurs, the system verifies that all data in the base address and memory of the specified size is
            ///     accessible for write access, and if it is not accessible, the function fails.
            /// </param>
            /// <param name="lpBuffer">
            ///     A pointer to the buffer that contains data to be written in the address space of the specified process.
            /// </param>
            /// <param name="nSize">
            ///     The number of bytes to be written to the specified process.
            /// </param>
            /// <param name="lpNumberOfBytesWritten">
            ///     A pointer to a variable that receives the number of bytes transferred into the specified process. This
            ///     parameter is optional. If lpNumberOfBytesWritten is NULL, the parameter is ignored.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, [MarshalAs(UnmanagedType.SysInt)] int nSize, out IntPtr lpNumberOfBytesWritten);
        }

        /// <summary>
        ///     Contains the names of the used Windows dynamic-link library (DLL) files.
        /// </summary>
        public struct DllNames
        {
            internal const string Advapi32 = "advapi32.dll";
            internal const string Dwmapi = "dwmapi.dll";
            internal const string Winmm = "winmm.dll";
            public const string Kernel32 = "kernel32.dll";
            public const string Ntdll = "ntdll.dll";
            public const string Psapi = "psapi.dll";
            public const string Shell32 = "shell32.dll";
            public const string User32 = "user32.dll";
        }

        /// <summary>
        ///     Contains information about a file object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal struct SHFILEINFO
        {
            /// <summary>
            ///     A handle to the icon that represents the file. You are responsible for destroying this handle
            ///     with DestroyIcon when you no longer need it.
            /// </summary>
            public IntPtr hIcon;

            /// <summary>
            ///     The index of the icon image within the system image list.
            /// </summary>
            public int iIcon;

            /// <summary>
            ///     An array of values that indicates the attributes of the file object. For information about these
            ///     values, see the IShellFolder::GetAttributesOf method.
            /// </summary>
            internal uint dwAttributes;

            /// <summary>
            ///     A string that contains the name of the file as it appears in the Windows Shell, or the path and
            ///     file name of the file that contains the icon representing the file.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] internal string szDisplayName;

            /// <summary>
            ///     A string that describes the type of file.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] internal string szTypeName;
        }

        /// <summary>
        ///     Provides enumerated values that specify file informations.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum GetFileInfoFunc : uint
        {
            /// <summary>
            ///     Apply the appropriate overlays to the file's icon. The SHGFI_ICON flag must also be set.
            /// </summary>
            SHGFI_ADDOVERLAYS = 0x20,

            /// <summary>
            ///     Modify SHGFI_ATTRIBUTES to indicate that the dwAttributes member of the SHFILEINFO structure
            ///     at psfi contains the specific attributes that are desired. These attributes are passed to
            ///     IShellFolder::GetAttributesOf. If this flag is not specified, 0xFFFFFFFF is passed to
            ///     IShellFolder::GetAttributesOf, requesting all attributes. This flag cannot be specified with
            ///     the SHGFI_ICON flag.
            /// </summary>
            SHGFI_ATTR_SPECIFIED = 0x20000,

            /// <summary>
            ///     Retrieve the item attributes. The attributes are copied to the dwAttributes member of the
            ///     structure specified in the psfi parameter. These are the same attributes that are obtained
            ///     from IShellFolder::GetAttributesOf.
            /// </summary>
            SHGFI_ATTRIBUTES = 0x800,

            /// <summary>
            ///     Retrieve the display name for the file, which is the name as it appears in Windows Explorer.
            ///     The name is copied to the szDisplayName member of the structure specified in psfi. The
            ///     returned display name uses the long file name, if there is one, rather than the 8.3 form of
            ///     the file name. Note that the display name can be affected by settings such as whether
            ///     extensions are shown.
            /// </summary>
            SHGFI_DISPLAYNAME = 0x200,

            /// <summary>
            ///     Retrieve the type of the executable file if pszPath identifies an executable file. The
            ///     information is packed into the return value. This flag cannot be specified with any other
            ///     flags.
            /// </summary>
            SHGFI_EXETYPE = 0x2000,

            /// <summary>
            ///     Retrieve the handle to the icon that represents the file and the index of the icon within
            ///     the system image list. The handle is copied to the hIcon member of the structure specified
            ///     by psfi, and the index is copied to the iIcon member.
            /// </summary>
            SHGFI_ICON = 0x100,

            /// <summary>
            ///     Retrieve the name of the file that contains the icon representing the file specified by
            ///     pszPath, as returned by the IExtractIcon::GetIconLocation method of the file's icon handler.
            ///     Also retrieve the icon index within that file. The name of the file containing the icon is
            ///     copied to the szDisplayName member of the structure specified by psfi. The icon's index is
            ///     copied to that structure's iIcon member.
            /// </summary>
            SHGFI_ICONLOCATION = 0x1000,

            /// <summary>
            ///     Modify SHGFI_ICON, causing the function to retrieve the file's large icon. The SHGFI_ICON
            ///     flag must also be set.
            /// </summary>
            SHGFI_LARGEICON = 0x0,

            /// <summary>
            ///     Modify SHGFI_ICON, causing the function to add the link overlay to the file's icon. The
            ///     SHGFI_ICON flag must also be set.
            /// </summary>
            SHGFI_LINKOVERLAY = 0x8000,

            /// <summary>
            ///     Modify SHGFI_ICON, causing the function to retrieve the file's open icon. Also used to
            ///     modify SHGFI_SYSICONINDEX, causing the function to return the handle to the system image
            ///     list that contains the file's small open icon. A container object displays an open icon to
            ///     indicate that the container is open. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also
            ///     be set.
            /// </summary>
            SHGFI_OPENICON = 0x2,

            /// <summary>
            ///     Return the index of the overlay icon. The value of the overlay index is returned in the upper
            ///     eight bits of the iIcon member of the structure specified by psfi. This flag requires that the
            ///     SHGFI_ICON be set as well.
            /// </summary>
            SHGFI_OVERLAYINDEX = 0x40,

            /// <summary>
            ///     Indicate that pszPath is the address of an ITEMIDLIST structure rather than a path name.
            /// </summary>
            SHGFI_PIDL = 0x8,

            /// <summary>
            ///     Modify SHGFI_ICON, causing the function to blend the file's icon with the system highlight
            ///     color. The SHGFI_ICON flag must also be set.
            /// </summary>
            SHGFI_SELECTED = 0x10000,

            /// <summary>
            ///     Modify SHGFI_ICON, causing the function to retrieve a Shell-sized icon. If this flag is not
            ///     specified the function sizes the icon according to the system metric values. The SHGFI_ICON
            ///     flag must also be set.
            /// </summary>
            SHGFI_SHELLICONSIZE = 0x4,

            /// <summary>
            ///     Modify SHGFI_ICON, causing the function to retrieve the file's small icon. Also used to modify
            ///     SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that
            ///     contains small icon images. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
            /// </summary>
            SHGFI_SMALLICON = 0x1,

            /// <summary>
            ///     Retrieve the index of a system image list icon. If successful, the index is copied to the iIcon
            ///     member of psfi. The return value is a handle to the system image list. Only those images whose
            ///     indices are successfully copied to iIcon are valid. Attempting to access other images in the
            ///     system image list will result in undefined behavior.
            /// </summary>
            SHGFI_SYSICONINDEX = 0x4000,

            /// <summary>
            ///     Retrieve the string that describes the file's type. The string is copied to the szTypeName
            ///     member of the structure specified in psfi.
            /// </summary>
            SHGFI_TYPENAME = 0x400,

            /// <summary>
            ///     Indicates that the function should not attempt to access the file specified by pszPath. Rather,
            ///     it should act as if the file specified by pszPath exists with the file attributes passed in
            ///     dwFileAttributes. This flag cannot be combined with the SHGFI_ATTRIBUTES, SHGFI_EXETYPE, or
            ///     SHGFI_PIDL flags.
            /// </summary>
            SHGFI_USEFILEATTRIBUTES = 0x10
        }

        /// <summary>
        ///     Contains information about a system appbar message.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct APPBARDATA : IDisposable
        {
            /// <summary>
            ///     The size of the structure, in bytes.
            /// </summary>
            public uint cbSize;

            /// <summary>
            ///     The handle to the appbar window. Not all messages use this member. See the individual message
            ///     page to see if you need to provide an hWind value.
            /// </summary>
            public IntPtr hWnd;

            /// <summary>
            ///     An application-defined message identifier. The application uses the specified identifier for
            ///     notification messages that it sends to the appbar identified by the hWnd member.
            /// </summary>
            public uint uCallbackMessage;

            /// <summary>
            ///     A value that specifies an edge of the screen.
            ///     <para>
            ///         This member is used when sending one of these messages:
            ///         <see cref="AppBarMessageFunc.ABM_GETAUTOHIDEBAR"/>
            ///         <see cref="AppBarMessageFunc.ABM_SETAUTOHIDEBAR"/>
            ///         <see cref="AppBarMessageFunc.ABM_GETAUTOHIDEBAREX"/>
            ///         <see cref="AppBarMessageFunc.ABM_SETAUTOHIDEBAREX"/>
            ///         <see cref="AppBarMessageFunc.ABM_QUERYPOS"/>
            ///         <see cref="AppBarMessageFunc.ABM_SETPOS"/>.
            ///     </para>
            /// </summary>
            public uint uEdge;

            /// <summary>
            ///     A <see cref="Rectangle"/> structure whose use varies depending on the message:
            ///     <para>
            ///         <see cref="AppBarMessageFunc.ABM_GETTASKBARPOS"/>,
            ///         <see cref="AppBarMessageFunc.ABM_QUERYPOS"/>,
            ///         <see cref="AppBarMessageFunc.ABM_SETPOS"/>: The bounding rectangle, in screen
            ///         coordinates, of an appbar or the Windows taskbar.
            ///     </para>
            ///     <para>
            ///         <see cref="AppBarMessageFunc.ABM_GETAUTOHIDEBAREX"/>,
            ///         <see cref="AppBarMessageFunc.ABM_SETAUTOHIDEBAREX"/>,
            ///         <see cref="AppBarMessageFunc.ABM_SETPOS"/>: The monitor on which the operation
            ///         is being performed.
            ///     </para>
            /// </summary>
            public Rectangle rc;

            /// <summary>
            ///     A message-dependent value. This member is used with these messages:
            ///     <para>
            ///         <see cref="AppBarMessageFunc.ABM_SETAUTOHIDEBAR"/>: Registers or unregisters an
            ///         autohide appbar for a given edge of the screen. If the system has multiple monitors,
            ///         the monitor that contains the primary taskbar is used.
            ///     </para>
            ///     <para>
            ///         <see cref="AppBarMessageFunc.ABM_SETAUTOHIDEBAREX"/>: Registers or unregisters an
            ///         autohide appbar for a given edge of the screen. This message extends
            ///         <see cref="AppBarMessageFunc.ABM_SETAUTOHIDEBAR"/> by enabling you to specify a
            ///         particular monitor, for use in multiple monitor situations.
            ///     </para>
            ///     <para>
            ///         <see cref="AppBarMessageFunc.ABM_SETSTATE"/>: Sets the autohide and always-on-top
            ///         states of the Windows taskbar.
            ///     </para>
            /// </summary>
            public int lParam;

            /// <summary>
            ///     Releases all resources used by this <see cref="APPBARDATA"/>.
            /// </summary>
            public void Dispose()
            {
                if (hWnd == IntPtr.Zero)
                    return;
                UnsafeNativeMethods.LocalFree(hWnd);
                hWnd = IntPtr.Zero;
            }
        }

        /// <summary>
        ///     Contains data to be passed to another application by the
        ///     <see cref="WindowMenuFunc.WM_COPYDATA"/> message.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct COPYDATASTRUCT : IDisposable
        {
            /// <summary>
            ///     The data to be passed to the receiving application.
            /// </summary>
            public IntPtr dwData;

            /// <summary>
            ///     The size, in bytes, of the data pointed to by the lpData member.
            /// </summary>
            public int cbData;

            /// <summary>
            ///     The data to be passed to the receiving application. This member can be NULL.
            /// </summary>
            public IntPtr lpData;

            /// <summary>
            ///     Releases all resources used by this <see cref="COPYDATASTRUCT"/>.
            /// </summary>
            public void Dispose()
            {
                if (lpData == IntPtr.Zero)
                    return;
                UnsafeNativeMethods.LocalFree(lpData);
                lpData = IntPtr.Zero;
            }
        }

        /// <summary>
        ///     Defines the message parameters passed to a <see cref="Win32HookFunc.WH_CALLWNDPROCRET"/>
        ///     hook procedure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct CWPRETSTRUCT
        {
            /// <summary>
            ///     The return value of the window procedure that processed the message specified by
            ///     the message value.
            /// </summary>
            public IntPtr lResult;

            /// <summary>
            ///     Additional information about the message. The exact meaning depends on the message
            ///     value.
            /// </summary>
            public IntPtr lParam;

            /// <summary>
            ///     Additional information about the message. The exact meaning depends on the message
            ///     value.
            /// </summary>
            public IntPtr wParam;

            /// <summary>
            ///     The message.
            /// </summary>
            public uint message;

            /// <summary>
            ///     A handle to the window that processed the message specified by the message value.
            /// </summary>
            public IntPtr hwnd;
        }

        /// <summary>
        ///     Contains information about the colorization of a Windows.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal struct DWM_COLORIZATION_PARAMS
        {
            public uint clrColor;
            public uint clrAfterGlow;
            public uint nIntensity;
            public uint clrAfterGlowBalance;
            public uint clrBlurBalance;
            public uint clrGlassReflectionIntensity;
            public bool fOpaque;
        }

        /// <summary>
        ///     Contains information about the placement of a window on the screen.
        /// </summary>
        //      [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct WINDOWPLACEMENT
        {
            public int length;

            /// <summary>
            ///     The flags that control the position of the minimized window and the method by which
            ///     the window is restored.
            ///     <para>
            ///         This member can be one or more of the <see cref="WindowPlacement"/> values.
            ///     </para>
            /// </summary>
            public int flags;

            /// <summary>
            ///     The current show state of the window.
            ///     <para>
            ///         This member can be one of the <see cref="ShowWindowFunc"/> values.
            ///     </para>
            /// </summary>
            public int showCmd;

            /// <summary>
            ///     The coordinates of the window's upper-left corner when the window is minimized.
            /// </summary>
            public Point ptMinPosition;

            /// <summary>
            ///     The coordinates of the window's upper-left corner when the window is maximized.
            /// </summary>
            public Point ptMaxPosition;

            /// <summary>
            ///     The window's coordinates when the window is in the restored position.
            /// </summary>
            public Rectangle rcNormalPosition;
        }

        /// <summary>
        ///     Used by <see cref="UnsafeNativeMethods.SendInput(uint, INPUT[], int)"/> to store
        ///     information for synthesizing input events such as keystrokes, mouse movement, and
        ///     mouse clicks.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct INPUT
        {
            /// <summary>
            ///     The type of the input event. This member can be one of the following values.
            ///     <para>
            ///         <c>
            ///             INPUT_MOUSE ((DWORD)0):
            ///         </c>
            ///         The event is a mouse event. Use the mi structure of the union.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             INPUT_KEYBOARD ((DWORD)1):
            ///         </c>
            ///         The event is a keyboard event. Use the ki structure of the union.
            ///     </para>
            ///     <para>
            ///         <c>
            ///             INPUT_HARDWARE ((DWORD)2):
            ///         </c>
            ///         The event is a hardware event. Use the hi structure of the union.
            ///     </para>
            /// </summary>
            public uint Type;

            /// <summary>
            ///     The information about a simulated mouse, keyboard or hardware event.
            /// </summary>
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        /// <summary>
        ///     Stores information about a simulated mouse, keyboard or hardware event.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)] public MOUSEINPUT Mouse;
            //***Currently not implemented
            //[FieldOffset(1)]
            //public KEYBDINPUT Keyboard;
            //[FieldOffset(2)]
            //public HARDWAREINPUT Hardware;
        }

        /// <summary>
        ///     Stores information about a simulated mouse event.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct MOUSEINPUT
        {
            /// <summary>
            ///     The absolute position of the mouse, or the amount of motion since the last mouse event
            ///     was generated, depending on the value of the dwFlags member. Absolute data is specified
            ///     as the x coordinate of the mouse; relative data is specified as the number of pixels moved.
            /// </summary>
            public int X;

            /// <summary>
            ///     The absolute position of the mouse, or the amount of motion since the last mouse event
            ///     was generated, depending on the value of the dwFlags member. Absolute data is specified as
            ///     the y coordinate of the mouse; relative data is specified as the number of pixels moved.
            /// </summary>
            public int Y;

            /// <summary>
            ///     If dwFlags contains MOUSEEVENTF_WHEEL, then mouseData specifies the amount of wheel movement.
            ///     A positive value indicates that the wheel was rotated forward, away from the user; a negative
            ///     value indicates that the wheel was rotated backward, toward the user. One wheel click is
            ///     defined as WHEEL_DELTA, which is 120.
            /// </summary>
            public uint MouseData;

            /// <summary>
            ///     A set of bit flags that specify various aspects of mouse motion and button clicks. The bits
            ///     in this member can be any reasonable combination of the following values.
            /// </summary>
            public uint Flags;

            /// <summary>
            ///     The time stamp for the event, in milliseconds. If this parameter is 0, the system will provide
            ///     its own time stamp.
            /// </summary>
            public uint Time;

            /// <summary>
            ///     An additional value associated with the mouse event. An application calls GetMessageExtraInfo
            ///     to obtain this extra information.
            /// </summary>
            public IntPtr ExtraInfo;
        }

        /// <summary>
        ///     Contains basic information about a process.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct PROCESS_BASIC_INFORMATION
        {
            public IntPtr ExitStatus;
            public IntPtr PebBaseAddress;
            public IntPtr AffinityMask;
            public IntPtr BasePriority;
            public UIntPtr UniqueProcessId;
            public IntPtr InheritedFromUniqueProcessId;
            public int Size => Marshal.SizeOf(typeof(PROCESS_BASIC_INFORMATION));
        }
    }
}
