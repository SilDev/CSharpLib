#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: WinApi.cs
// Version:  2019-07-26 05:04
// 
// Copyright (c) 2019, Si13n7 Developments (r)
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
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Windows.Forms;
    using FileTime = System.Runtime.InteropServices.ComTypes.FILETIME;

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
        ///     An application-defined callback function used with the EnumThreadWindows function.
        ///     It receives the window handles associated with a thread. The WNDENUMPROC type defines
        ///     a pointer to this callback function. EnumThreadWndProc is a placeholder for the
        ///     application-defined function name.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to a window associated with the thread specified in the
        ///     <see cref="NativeHelper.EnumThreadWindows(uint, EnumThreadWndProc, IntPtr)"/> function.
        /// </param>
        /// <param name="lParam">
        ///     The application-defined value given in the
        ///     <see cref="NativeHelper.EnumThreadWindows(uint, EnumThreadWndProc, IntPtr)"/> function.
        /// </param>
        /// <returns>
        ///     To continue enumeration, the callback function must return TRUE; to stop enumeration, it
        ///     must return FALSE.
        /// </returns>
        public delegate bool EnumThreadWndProc(IntPtr hWnd, IntPtr lParam);

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
        ///     The WM_TIMER (0x113) message.
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
        public enum AccessRights : uint
        {
            /// <summary>
            ///     Required to delete the object.
            /// </summary>
            Delete = 0x10000,

            /// <summary>
            ///     Required to read information in the security descriptor for the object, not
            ///     including the information in the SACL.
            /// </summary>
            ReadControl = 0x20000,

            /// <summary>
            ///     The right to use the object for synchronization. This enables a thread to wait
            ///     until the object is in the signaled state.
            /// </summary>
            Synchronize = 0x100000,

            /// <summary>
            ///     Required to modify the DACL in the security descriptor for the object.
            /// </summary>
            WriteDac = 0x40000,

            /// <summary>
            ///     Required to change the owner in the security descriptor for the object.
            /// </summary>
            WriteOwner = 0x80000,

            /// <summary>
            ///     Required to create a process.
            /// </summary>
            ProcessCreateProcess = 0x80,

            /// <summary>
            ///     Required to create a thread.
            /// </summary>
            ProcessCreateThread = 0x2,

            /// <summary>
            ///     Required to duplicate a handle using
            ///     <see cref="NativeHelper.DuplicateHandle(IntPtr, IntPtr, IntPtr, out IntPtr, uint, bool, uint)"/>
            /// </summary>
            ProcessDupHandle = 0x40,

            /// <summary>
            ///     Required to retrieve certain information about a process, such as its token,
            ///     exit code, and priority class.
            /// </summary>
            ProcessQueryInformation = 0x400,

            /// <summary>
            ///     Required to retrieve certain information about a process.
            /// </summary>
            ProcessQueryLimitedInformation = 0x1000,

            /// <summary>
            ///     Required to set certain information about a process, such as its priority class.
            /// </summary>
            ProcessSetInformation = 0x200,

            /// <summary>
            ///     Required to set memory limits using
            ///     <see cref="NativeHelper.SetProcessWorkingSetSize(IntPtr, UIntPtr, UIntPtr)"/>.
            /// </summary>
            ProcessSetQuota = 0x100,

            /// <summary>
            ///     Required to suspend or resume a process.
            /// </summary>
            ProcessSuspendResume = 0x800,

            /// <summary>
            ///     Required to terminate a process using
            ///     <see cref="NativeHelper.TerminateProcess(IntPtr, uint)"/>.
            /// </summary>
            ProcessTerminate = 0x1,

            /// <summary>
            ///     Required to perform an operation on the address space of a process.
            /// </summary>
            ProcessVmOperation = 0x8,

            /// <summary>
            ///     Required to read memory in a process using
            ///     <see cref="NativeHelper.ReadProcessMemory(IntPtr, IntPtr, IntPtr, IntPtr, ref IntPtr)"/>.
            /// </summary>
            ProcessVmRead = 0x10,

            /// <summary>
            ///     Required to write to memory in a process using
            ///     <see cref="NativeHelper.WriteProcessMemory(IntPtr, IntPtr, IntPtr, int, out IntPtr)"/>.
            /// </summary>
            ProcessVmWrite = 0x20
        }

        /// <summary>
        ///     Provides enumerated values of window animations.
        /// </summary>
        [Flags]
        public enum AnimateWindowFlags : uint
        {
            /// <summary>
            ///     Activates the window. Do not use this value with <see cref="Hide"/>.
            /// </summary>
            Activate = 0x20000,

            /// <summary>
            ///     Uses a fade effect. This flag can be used only if hwnd is a top-level window.
            /// </summary>
            Blend = 0x80000,

            /// <summary>
            ///     Makes the window appear to collapse inward if <see cref="Hide"/> is
            ///     used or expand outward if the <see cref="Hide"/> is not used. The
            ///     various direction flags have no effect.
            /// </summary>
            Center = 0x10,

            /// <summary>
            ///     Hides the window. By default, the window is shown.
            /// </summary>
            Hide = 0x10000,

            /// <summary>
            ///     Animates the window from left to right. This flag can be used with roll or slide
            ///     animation. It is ignored when used with <see cref="Center"/> or
            ///     <see cref="Blend"/>.
            /// </summary>
            HorPositive = 0x1,

            /// <summary>
            ///     Animates the window from right to left. This flag can be used with roll or slide
            ///     animation. It is ignored when used with <see cref="Center"/>
            ///     or <see cref="Blend"/>.
            /// </summary>
            HorNegative = 0x2,

            /// <summary>
            ///     Uses slide animation. By default, roll animation is used. This flag is ignored
            ///     when used with <see cref="Center"/>.
            /// </summary>
            Slide = 0x40000,

            /// <summary>
            ///     Animates the window from top to bottom. This flag can be used with roll or slide
            ///     animation. It is ignored when used with <see cref="Center"/> or
            ///     <see cref="Blend"/>.
            /// </summary>
            VerPositive = 0x4,

            /// <summary>
            ///     Animates the window from bottom to top. This flag can be used with roll or slide
            ///     animation. It is ignored when used with <see cref="Center"/> or
            ///     <see cref="Blend"/>.
            /// </summary>
            VerNegative = 0x8
        }

        /// <summary>
        ///     Provides enumerated values of appbar messages.
        /// </summary>
        public enum AppBarMessageOptions
        {
            /// <summary>
            ///     Registers a new appbar and specifies the message identifier that the system
            ///     should use to send notification messages to the appbar
            /// </summary>
            New = 0x0,

            /// <summary>
            ///     Unregisters an appbar, removing the bar from the system's internal list.
            /// </summary>
            Remove = 0x1,

            /// <summary>
            ///     Requests a size and screen position for an appbar.
            /// </summary>
            QueryPos = 0x2,

            /// <summary>
            ///     Sets the size and screen position of an appbar.
            /// </summary>
            SetPos = 0x3,

            /// <summary>
            ///     Retrieves the autohide and always-on-top states of the Windows taskbar.
            /// </summary>
            GetState = 0x4,

            /// <summary>
            ///     Retrieves the bounding rectangle of the Windows taskbar. Note that this applies only
            ///     to the system taskbar. Other objects, particularly toolbars supplied with third-party
            ///     software, also can be present. As a result, some of the screen area not covered by the
            ///     Windows taskbar might not be visible to the user. To retrieve the area of the screen
            ///     not covered by both the taskbar and other app bars-the working area available to your
            ///     application-, use the GetMonitorInfo function.
            /// </summary>
            GetTaskBarPos = 0x5,

            /// <summary>
            ///     Notifies the system to activate or deactivate an appbar. The lParam member of the
            ///     <see cref="AppBarData"/> pointed to by pData is set to TRUE to activate or FALSE to
            ///     deactivate.
            /// </summary>
            Activate = 0x6,

            /// <summary>
            ///     Retrieves the handle to the autohide appbar associated with a particular edge of the
            ///     screen.
            /// </summary>
            GetAutoHideBar = 0x7,

            /// <summary>
            ///     Registers or unregisters an autohide appbar for an edge of the screen.
            /// </summary>
            SetAutoHideBar = 0x8,

            /// <summary>
            ///     Notifies the system when an appbar's position has changed.
            /// </summary>
            WindowPosChanged = 0x9,

            /// <summary>
            ///     Sets the state of the appbar's autohide and always-on-top attributes.
            /// </summary>
            SetState = 0xa,

            /// <summary>
            ///     Retrieves the handle to the autohide appbar associated with a particular edge of a
            ///     particular monitor.
            /// </summary>
            GetAutoHideBarEx = 0xb,

            /// <summary>
            ///     Registers or unregisters an autohide appbar for an edge of a particular monitor.
            /// </summary>
            SetAutoHideBarEx = 0xc
        }

        /// <summary>
        ///     Provides enumerated options of handle duplication.
        /// </summary>
        public enum DuplicateOptions : uint
        {
            /// <summary>
            ///     Closes the source handle. This occurs regardless of any error status returned.
            /// </summary>
            CloseSource = 0x1,

            /// <summary>
            ///     Ignores the dwDesiredAccess parameter. The duplicate handle has the same access as the
            ///     source handle.
            /// </summary>
            SameAccess = 0x2
        }

        /// <summary>
        ///     Provides enumerated attributes of memory allocation.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum LocalAllocFlags : uint
        {
            /// <summary>
            ///     Combines <see cref="LMemMoveable"/> and <see cref="LMemZeroInit"/>.
            /// </summary>
            LHND = LMemMoveable | LMemZeroInit,

            /// <summary>
            ///     Allocates fixed memory. The return value is a pointer to the memory object.
            /// </summary>
            LMemFixed = 0x0,

            /// <summary>
            ///     Allocates movable memory. Memory blocks are never moved in physical memory,
            ///     but they can be moved within the default heap.
            /// </summary>
            LMemMoveable = 0x2,

            /// <summary>
            ///     Initializes memory contents to zero.
            /// </summary>
            LMemZeroInit = 0x40,

            /// <summary>
            ///     Combines <see cref="LMemFixed"/> and <see cref="LMemZeroInit"/>.
            /// </summary>
            LPtr = LMemFixed | LMemZeroInit,

            /// <summary>
            ///     Same as <see cref="LMemMoveable"/>.
            /// </summary>
            NonZeroLHND = LMemMoveable,

            /// <summary>
            ///     Same as <see cref="LMemFixed"/>.
            /// </summary>
            NonZeroLPtr = LMemFixed
        }

        /// <summary>
        ///     Provides enumerated values of memory allocation types.
        /// </summary>
        [Flags]
        public enum MemAllocTypes : uint
        {
            /// <summary>
            ///     Allocates memory charges (from the overall size of memory and the paging files
            ///     on disk) for the specified reserved memory pages. The function also guarantees
            ///     that when the caller later initially accesses the memory, the contents will be
            ///     zero. Actual physical pages are not allocated unless/until the virtual
            ///     addresses are actually accessed.
            /// </summary>
            Commit = 0x1000,

            /// <summary>
            ///     Reserves a range of the process's virtual address space without allocating any
            ///     actual physical storage in memory or in the paging file on disk.
            /// </summary>
            Reserve = 0x2000,

            /// <summary>
            ///     Decommits the specified region of committed pages. After the operation, the
            ///     pages are in the reserved state.
            /// </summary>
            Decommit = 0x4000,

            /// <summary>
            ///     Releases the specified region of pages. After the operation, the pages are in
            ///     the free state.
            /// </summary>
            Release = 0x8000,

            /// <summary>
            ///     Indicates that data in the memory range specified by lpAddress and dwSize is
            ///     no longer of interest. The pages should not be read from or written to the
            ///     paging file. However, the memory block will be used again later, so it should
            ///     not be decommitted. This value cannot be used with any other value.
            /// </summary>
            Reset = 0x80000,

            /// <summary>
            ///     Reserves an address range that can be used to map Address Windowing Extensions
            ///     (AWE) pages.
            /// </summary>
            Physical = 0x400000,

            /// <summary>
            ///     Allocates memory at the highest possible address. This can be slower than
            ///     regular allocations, especially when there are many allocations.
            /// </summary>
            TopDown = 0x100000,

            /// <summary>
            ///     Causes the system to track pages that are written to in the allocated region.
            ///     If you specify this value, you must also specify <see cref="Reserve"/>.
            /// </summary>
            WriteWatch = 0x200000,

            /// <summary>
            ///     Allocates memory using large page support.
            /// </summary>
            LargePages = 0x20000000
        }

        /// <summary>
        ///     Provides enumerated free type values of memory allocation.
        /// </summary>
        public enum MemFreeTypes : uint
        {
            /// <summary>
            ///     Decommits the specified region of committed pages. After the operation, the
            ///     pages are in the reserved state.
            /// </summary>
            Decommit = 0x4000,

            /// <summary>
            ///     Releases the specified region of pages. After the operation, the pages are in
            ///     the free state.
            /// </summary>
            Release = 0x8000
        }

        /// <summary>
        ///     Provides enumerated constants of memory protection.
        /// </summary>
        [Flags]
        public enum MemProtectFlags : uint
        {
            /// <summary>
            ///     Enables execute access to the committed region of pages. An attempt to write
            ///     to the committed region results in an access violation.
            /// </summary>
            PageExecute = 0x10,

            /// <summary>
            ///     Enables execute or read-only access to the committed region of pages. An
            ///     attempt to write to the committed region results in an access violation.
            /// </summary>
            PageExecuteRead = 0x20,

            /// <summary>
            ///     Enables execute, read-only, or read/write access to the committed region of
            ///     pages.
            /// </summary>
            PageExecuteReadWrite = 0x40,

            /// <summary>
            ///     Enables execute, read-only, or copy-on-write access to a mapped view of a file
            ///     mapping object. An attempt to write to a committed copy-on-write page results
            ///     in a private copy of the page being made for the process. The private page is
            ///     marked as <see cref="PageExecuteReadWrite"/>, and the change is written to
            ///     the new page.
            /// </summary>
            PageExecuteWriteCopy = 0x80,

            /// <summary>
            ///     Disables all access to the committed region of pages. An attempt to read from,
            ///     write to, or execute the committed region results in an access violation.
            /// </summary>
            PageNoAccess = 0x1,

            /// <summary>
            ///     Enables read-only access to the committed region of pages. An attempt to write
            ///     to the committed region results in an access violation. If Data Execution
            ///     Prevention is enabled, an attempt to execute code in the committed region
            ///     results in an access violation.
            /// </summary>
            PageReadOnly = 0x2,

            /// <summary>
            ///     Enables read-only or read/write access to the committed region of pages. If
            ///     Data Execution Prevention is enabled, attempting to execute code in the
            ///     committed region results in an access violation.
            /// </summary>
            PageReadWrite = 0x4,

            /// <summary>
            ///     Enables read-only or copy-on-write access to a mapped view of a file mapping
            ///     object. An attempt to write to a committed copy-on-write page results in a
            ///     private copy of the page being made for the process. The private page is
            ///     marked as <see cref="PageReadWrite"/>, and the change is written to the new
            ///     page. If Data Execution Prevention is enabled, attempting to execute code in
            ///     the committed region results in an access violation.
            /// </summary>
            PageWriteCopy = 0x8,

            /// <summary>
            ///     Sets all locations in the pages as invalid targets for CFG. Used along with
            ///     any execute page protection like <see cref="PageExecute"/>,
            ///     <see cref="PageExecuteRead"/>, <see cref="PageExecuteReadWrite"/> and
            ///     <see cref="PageExecuteWriteCopy"/>. Any indirect call to locations in those
            ///     pages will fail CFG checks and the process will be terminated. The default
            ///     behavior for executable pages allocated is to be marked valid call targets
            ///     for CFG.
            /// </summary>
            PageTargetsInvalid = 0x40000000,

            /// <summary>
            ///     Pages in the region will not have their CFG information updated while the
            ///     protection changes for VirtualProtect. For example, if the pages in the region
            ///     was allocated using <see cref="PageTargetsInvalid"/>, then the invalid
            ///     information will be maintained while the page protection changes. This flag is
            ///     only valid when the protection changes to an executable type like
            ///     <see cref="PageExecute"/>, <see cref="PageExecuteRead"/>,
            ///     <see cref="PageExecuteReadWrite"/> and <see cref="PageExecuteWriteCopy"/>.
            ///     The default behavior for VirtualProtect protection change to executable is to
            ///     mark all locations as valid call targets for CFG.
            /// </summary>
            PageTargetsNoUpdate = 0x40000000,

            /// <summary>
            ///     Pages in the region become guard pages. Any attempt to access a guard page
            ///     causes the system to raise a STATUS_GUARD_PAGE_VIOLATION (0x80000001) exception
            ///     and turn off the guard page status. Guard pages thus act as a one-time access
            ///     alarm.
            /// </summary>
            PageGuard = 0x100,

            /// <summary>
            ///     Sets all pages to be non-cachable. Applications should not use this attribute
            ///     except when explicitly required for a device. Using the interlocked functions
            ///     with memory that is mapped with <see cref="PageNoCache"/> can result
            ///     in an <see cref="ExternalException"/>.
            /// </summary>
            PageNoCache = 0x200,

            /// <summary>
            ///     Sets all pages to be write-combined.
            /// </summary>
            PageWriteCombine = 0x400
        }

        /// <summary>
        ///     Provides enumerated options for MIME functions.
        /// </summary>
        [Flags]
        public enum MimeFlags
        {
            /// <summary>
            ///     No flags specified. Use default behavior for the function.
            /// </summary>
            Default = 0x0,

            /// <summary>
            ///     Treat the specified pwzUrl as a file name.
            /// </summary>
            UrlAsFileName = 0x1,

            /// <summary>
            ///     Internet Explorer 6 for Windows XP SP2 and later. Use MIME-type detection even
            ///     if FEATURE_MIME_SNIFFING is detected. Usually, this feature control key would
            ///     disable MIME-type detection.
            /// </summary>
            EnableMimeSniffing = 0x2,

            /// <summary>
            ///     Internet Explorer 6 for Windows XP SP2 and later. Perform MIME-type detection
            ///     if "text/plain" is proposed, even if data sniffing is otherwise disabled. Plain
            ///     text may be converted to text/html if HTML tags are detected.
            /// </summary>
            IgnoreMimeTextPlain = 0x4,

            /// <summary>
            ///     Internet Explorer 8. Use the authoritative MIME type specified in pwzMimeProposed.
            ///     Unless <see cref="IgnoreMimeTextPlain"/> is specified, no data sniffing is
            ///     performed.
            /// </summary>
            ServerMime = 0x8,

            /// <summary>
            ///     Internet Explorer 9. Do not perform detection if "text/plain" is specified in
            ///     pwzMimeProposed.
            /// </summary>
            RespectTextPlain = 0x10,

            /// <summary>
            ///     Internet Explorer 9. Returns image/png and image/jpeg instead of image/x-png and image/pjpeg.
            /// </summary>
            ReturnUpdatedImgMimes = 0x20
        }

        /// <summary>
        ///     Provides enumerated values of menu items.
        /// </summary>
        [Flags]
        public enum ModifyMenuFlags : uint
        {
            /// <summary>
            ///     Indicates that the uPosition parameter gives the identifier of the menu item.
            ///     The <see cref="ByCommand"/> flag is the default if neither the
            ///     <see cref="ByCommand"/> nor <see cref="ByPosition"/> flag is specified.
            /// </summary>
            ByCommand = 0x0,

            /// <summary>
            ///     Indicates that the uPosition parameter gives the zero-based relative position
            ///     of the menu item.
            /// </summary>
            ByPosition = 0x400,

            /// <summary>
            ///     Uses a bitmap as the menu item. The lpNewItem parameter contains a handle to
            ///     the bitmap.
            /// </summary>
            Bitmap = 0x4,

            /// <summary>
            ///     Places a check mark next to the item. If your application provides check-mark
            ///     bitmaps (see the SetMenuItemBitmaps function), this flag displays a selected
            ///     bitmap next to the menu item.
            /// </summary>
            Checked = 0x8,

            /// <summary>
            ///     Disables the menu item so that it cannot be selected, but this flag does not
            ///     gray it.
            /// </summary>
            Disabled = 0x2,

            /// <summary>
            ///     Enables the menu item so that it can be selected and restores it from its
            ///     grayed state.
            /// </summary>
            Enabled = 0x0,

            /// <summary>
            ///     Disables the menu item and grays it so that it cannot be selected.
            /// </summary>
            Grayed = 0x1,

            /// <summary>
            ///     Functions the same as the <see cref="MenuBreak"/> flag for a menu bar. For
            ///     a drop-down menu, submenu, or shortcut menu, the new column is separated from
            ///     the old column by a vertical line.
            /// </summary>
            MenuBarBreak = 0x20,

            /// <summary>
            ///     Places the item on a new line (for menu bars) or in a new column (for a
            ///     drop-down menu, submenu, or shortcut menu) without separating columns.
            /// </summary>
            MenuBreak = 0x40,

            /// <summary>
            ///     Specifies that the item is an owner-drawn item. Before the menu is displayed
            ///     for the first time, the window that owns the menu receives a WM_MEASUREITEM
            ///     message to retrieve the width and height of the menu item. The WM_DRAWITEM
            ///     message is then sent to the window procedure of the owner window whenever
            ///     the appearance of the menu item must be updated.
            /// </summary>
            OwnerDraw = 0x100,

            /// <summary>
            ///     Specifies that the menu item opens a drop-down menu or submenu. The uIDNewItem
            ///     parameter specifies a handle to the drop-down menu or submenu. This flag is
            ///     used to add a menu name to a menu bar or a menu item that opens a submenu to a
            ///     drop-down menu, submenu, or shortcut menu.
            /// </summary>
            Popup = 0x10,

            /// <summary>
            ///     Draws a horizontal dividing line. This flag is used only in a drop-down menu,
            ///     submenu, or shortcut menu. The line cannot be grayed, disabled, or highlighted.
            ///     The lpNewItem and uIDNewItem parameters are ignored.
            /// </summary>
            Separator = 0x800,

            /// <summary>
            ///     Specifies that the menu item is a text string; the lpNewItem parameter is a
            ///     pointer to the string.
            /// </summary>
            String = 0x0,

            /// <summary>
            ///     Does not place a check mark next to the item (the default). If your application
            ///     supplies check-mark bitmaps (see the SetMenuItemBitmaps function), this flag
            ///     displays a clear bitmap next to the menu item.
            /// </summary>
            Unchecked = 0x0,

            /// <summary>
            ///     Remove uPosition parameters.
            /// </summary>
            Remove = 0x1000
        }

        /// <summary>
        ///     Provides enumerated values of the process information class.
        /// </summary>
        [Flags]
        public enum ProcessInfoFlags : uint
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
        ///     Provides enumerated values of the redraw window flags.
        /// </summary>
        [Flags]
        public enum RedrawWindowFlags : uint
        {
            /// <summary>
            ///     Causes the window to receive a WM_ERASEBKGND message when the window is repainted.
            ///     The <see cref="Invalidate"/> flag must also be specified; otherwise,
            ///     <see cref="Erase"/> has no effect.
            /// </summary>
            Erase = 0x4,

            /// <summary>
            ///     Causes any part of the nonclient area of the window that intersects the update region
            ///     to receive a WM_NCPAINT message. The <see cref="Invalidate"/> flag must also be specified;
            ///     otherwise, <see cref="Frame"/> has no effect. The WM_NCPAINT message is typically not sent
            ///     during the execution of RedrawWindow unless either <see cref="UpdateNow"/> or
            ///     <see cref="EraseNow"/> is specified.
            /// </summary>
            Frame = 0x400,

            /// <summary>
            ///     Causes a WM_PAINT message to be posted to the window regardless of whether any portion of
            ///     the window is invalid.
            /// </summary>
            InternalPaint = 0x2,

            /// <summary>
            ///     Invalidates lprcUpdate or hrgnUpdate (only one may be non-NULL). If both are NULL, the
            ///     entire window is invalidated.
            /// </summary>
            Invalidate = 0x1,

            /// <summary>
            ///     Suppresses any pending WM_ERASEBKGND messages.
            /// </summary>
            NoErase = 0x20,

            /// <summary>
            ///     Suppresses any pending WM_NCPAINT messages. This flag must be used with <see cref="Validate"/>
            ///     and is typically used with <see cref="NoChildren"/>. <see cref="NoFrame"/> should be used with
            ///     care, as it could cause parts of a window to be painted improperly.
            /// </summary>
            NoFrame = 0x800,

            /// <summary>
            ///     Suppresses any pending internal WM_PAINT messages. This flag does not affect WM_PAINT messages
            ///     resulting from a non-NULL update area.
            /// </summary>
            NoInternalPaint = 0x10,

            /// <summary>
            ///     Validates lprcUpdate or hrgnUpdate (only one may be non-NULL). If both are NULL, the entire
            ///     window is validated. This flag does not affect internal WM_PAINT messages.
            /// </summary>
            Validate = 0x8,

            /// <summary>
            ///     Causes the affected windows (as specified by the RDW_ALLCHILDREN and RDW_NOCHILDREN flags) to
            ///     receive WM_NCPAINT and WM_ERASEBKGND messages, if necessary, before the function returns.
            ///     WM_PAINT messages are received at the ordinary time.
            /// </summary>
            EraseNow = 0x200,

            /// <summary>
            ///     Causes the affected windows (as specified by the RDW_ALLCHILDREN and RDW_NOCHILDREN flags) to
            ///     receive WM_NCPAINT, WM_ERASEBKGND, and WM_PAINT messages, if necessary, before the function
            ///     returns.
            /// </summary>
            UpdateNow = 0x100,

            /// <summary>
            ///     Includes child windows, if any, in the repainting operation.
            /// </summary>
            AllChildren = 0x80,

            /// <summary>
            ///     Excludes child windows, if any, from the repainting operation.
            /// </summary>
            NoChildren = 0x40
        }

        /// <summary>
        ///     Provides enumerated values of service errors.
        /// </summary>
        public enum ServiceErrors
        {
            /// <summary>
            ///     The startup program logs the error in the event log, if possible. If the last-known-good
            ///     configuration is being started, the startup operation fails. Otherwise, the system is
            ///     restarted with the last-known good configuration.
            /// </summary>
            Critical = 0x3,

            /// <summary>
            ///     The startup program ignores the error and continues the startup operation.
            /// </summary>
            Ignore = 0x0,

            /// <summary>
            ///     The startup program logs the error in the event log but continues the startup operation.
            /// </summary>
            Normal = 0x1,

            /// <summary>
            ///     The startup program logs the error in the event log. If the last-known-good configuration is
            ///     being started, the startup operation continues. Otherwise, the system is restarted with the
            ///     last-known-good configuration.
            /// </summary>
            Severe = 0x2
        }

        /// <summary>
        ///     Provides enumerated attributes of memory allocation.
        /// </summary>
        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum SetWindowPosFlags : uint
        {
            /// <summary>
            ///     If the calling thread and the thread that owns the window are attached to different input
            ///     queues, the system posts the request to the thread that owns the window. This prevents the
            ///     calling thread from blocking its execution while other threads process the request.
            /// </summary>
            AsyncWindowPos = 0x4000,

            /// <summary>
            ///     Prevents generation of the WM_SYNCPAINT message.
            /// </summary>
            DeferErase = 0x2000,

            /// <summary>
            ///     Draws a frame (defined in the window's class description) around the window.
            /// </summary>
            DrawFrame = 0x20,

            /// <summary>
            ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message
            ///     to the window, even if the window's size is not being changed. If this flag is not specified,
            ///     WM_NCCALCSIZE is sent only when the window's size is being changed.
            /// </summary>
            FrameChanged = 0x20,

            /// <summary>
            ///     Hides the window.
            /// </summary>
            HideWindow = 0x80,

            /// <summary>
            ///     Does not activate the window. If this flag is not set, the window is activated and moved to
            ///     the top of either the topmost or non-topmost group (depending on the setting of the
            ///     hWndInsertAfter parameter).
            /// </summary>
            NoActive = 0x10,

            /// <summary>
            ///     Discards the entire contents of the client area. If this flag is not specified, the valid
            ///     contents of the client area are saved and copied back into the client area after the window
            ///     is sized or repositioned.
            /// </summary>
            NoCopyBits = 0x100,

            /// <summary>
            ///     Retains the current position (ignores X and Y parameters).
            /// </summary>
            NoMove = 0x2,

            /// <summary>
            ///     Does not change the owner window's position in the Z order.
            /// </summary>
            NoOwnerZOrder = 0x200,

            /// <summary>
            ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This
            ///     applies to the client area, the nonclient area (including the title bar and scroll bars),
            ///     and any part of the parent window uncovered as a result of the window being moved. When this
            ///     flag is set, the application must explicitly invalidate or redraw any parts of the window and
            ///     parent window that need redrawing.
            /// </summary>
            NoRedraw = 0x8,

            /// <summary>
            ///     Same as the <see cref="SetWindowPosFlags.NoOwnerZOrder"/> flag.
            /// </summary>
            NoReposition = 0x200,

            /// <summary>
            ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING (0x46) message.
            /// </summary>
            NoSendChanging = 0x400,

            /// <summary>
            ///     Retains the current size (ignores the cx and cy parameters).
            /// </summary>
            NoSize = 0x1,

            /// <summary>
            ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
            /// </summary>
            NoZOrder = 0x4,

            /// <summary>
            ///     Displays the window.
            /// </summary>
            ShowWindow = 0x40
        }

        /// <summary>
        ///     Provides enumerated values of scroll bar show statements.
        /// </summary>
        public enum ShowScrollBarOptions
        {
            /// <summary>
            ///     Shows or hides a window's standard horizontal scroll bars.
            /// </summary>
            Horizontal = 0,

            /// <summary>
            ///     Shows or hides a window's standard vertical scroll bar.
            /// </summary>
            Vertical = 1,

            /// <summary>
            ///     Shows or hides a scroll bar control. The hwnd parameter must be the handle to the scroll bar control.
            /// </summary>
            Control = 2,

            /// <summary>
            ///     Shows or hides a window's standard horizontal and vertical scroll bars.
            /// </summary>
            Both = 3
        }

        /// <summary>
        ///     Provides enumerated values of window's show statements.
        /// </summary>
        [Flags]
        public enum ShowWindowFlags : uint
        {
            /// <summary>
            ///     Minimizes a window, even if the thread that owns the window is not responding.
            ///     This flag should only be used when minimizing windows from a different thread.
            /// </summary>
            ForceMinimize = 0xb,

            /// <summary>
            ///     Hides the window and activates another window.
            /// </summary>
            Hide = 0x0,

            /// <summary>
            ///     Maximizes the specified window.
            /// </summary>
            Maximize = 0x3,

            /// <summary>
            ///     Minimizes the specified window and activates the next top-level window in the
            ///     Z order.
            /// </summary>
            Minimize = 0x6,

            /// <summary>
            ///     Activates and displays the window. If the window is minimized or maximized,
            ///     the system restores it to its original size and position. An application
            ///     should specify this flag when restoring a minimized window.
            /// </summary>
            Restore = 0x9,

            /// <summary>
            ///     Activates the window and displays it in its current size and position.
            /// </summary>
            Show = 0x5,

            /// <summary>
            ///     Sets the show state based on the SW_ value specified in the STARTUPINFO
            ///     structure passed to the CreateProcess function by the program that started
            ///     the application.
            /// </summary>
            ShowDefault = 0xa,

            /// <summary>
            ///     Activates the window and displays it as a maximized window.
            /// </summary>
            ShowMaximized = 0x3,

            /// <summary>
            ///     Activates the window and displays it as a minimized window.
            /// </summary>
            ShowMinimized = 0x2,

            /// <summary>
            ///     Displays the window as a minimized window. This value is similar to
            ///     <see cref="ShowMinimized"/>, except the window is not activated.
            /// </summary>
            ShowMinNoActive = 0x7,

            /// <summary>
            ///     Displays the window in its current size and position. This value is similar to
            ///     <see cref="Show"/>, except that the window is not activated.
            /// </summary>
            ShowNa = 0x8,

            /// <summary>
            ///     Displays a window in its most recent size and position. This value is similar
            ///     to <see cref="ShowNormal"/>, except that the window is not activated.
            /// </summary>
            ShowNoActivate = 0x4,

            /// <summary>
            ///     Activates and displays a window. If the window is minimized or maximized, the
            ///     system restores it to its original size and position. An application should
            ///     specify this flag when displaying the window for the first time.
            /// </summary>
            ShowNormal = 0x1
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
        public enum StandardAccessRights : long
        {
            /// <summary>
            ///     Combines <see cref="Delete"/>, <see cref="ReadControl"/>, <see cref="WriteDac"/>,
            ///     and <see cref="WriteOwner"/> access.
            /// </summary>
            All = Delete | ReadControl | WriteDac | WriteOwner,

            /// <summary>
            ///     The right to delete the object.
            /// </summary>
            Delete = 0x10000,

            /// <summary>
            ///     The right to read the information in the object's security descriptor, not
            ///     including the information in the system access control list (SACL).
            /// </summary>
            ReadControl = 0x20000,

            /// <summary>
            ///     Combines <see cref="Delete"/>, <see cref="ReadControl"/>, <see cref="WriteDac"/>,
            ///     and <see cref="WriteOwner"/> access.
            /// </summary>
            Required = Delete | ReadControl | WriteDac | WriteOwner,

            /// <summary>
            ///     The right to use the object for synchronization. This enables a thread to wait
            ///     until the object is in the signaled state. Some object types do not support
            ///     this access right.
            /// </summary>
            Synchronize = 0x100000,

            /// <summary>
            ///     The right to modify the discretionary access control list (DACL) in the object's
            ///     security descriptor.
            /// </summary>
            WriteDac = 0x40000,

            /// <summary>
            ///     The right to change the owner in the object's security descriptor.
            /// </summary>
            WriteOwner = 0x80000
        }

        /// <summary>
        ///     Provides enumerated hook constants.
        /// </summary>
        [Flags]
        public enum Win32HookFlags
        {
            /// <summary>
            ///     The system is about to activate a window.
            /// </summary>
            HCbtActivate = 0x5,

            /// <summary>
            ///     The system has removed a mouse message from the system message queue. Upon receiving
            ///     this hook code, a CBT application must install a <see cref="WhJournalPlayback"/> hook
            ///     procedure in response to the mouse message.
            /// </summary>
            HCbtClickSkipped = 0x6,

            /// <summary>
            ///     A window is about to be created. The system calls the hook procedure before sending
            ///     the WM_CREATE or WM_NCCREATE message to the window. If the hook procedure returns a
            ///     nonzero value, the system destroys the window; the CreateWindow function returns NULL,
            ///     but the WM_DESTROY message is not sent to the window. If the hook procedure returns
            ///     zero, the window is created normally. At the time of the <see cref="HCbtCreateWnd"/>
            ///     notification, the window has been created, but its final size and position may not
            ///     have been determined and its parent window may not have been established. It is
            ///     possible to send messages to the newly created window, although it has not yet
            ///     received WM_NCCREATE or WM_CREATE messages. It is also possible to change the position
            ///     in the z-order of the newly created window by modifying the hwndInsertAfter member of
            ///     the CBT_CREATEWND structure.
            /// </summary>
            HCbtCreateWnd = 0x3,

            /// <summary>
            ///     A window is about to be destroyed.
            /// </summary>
            HCbtDestroyWnd = 0x4,

            /// <summary>
            ///     The system has removed a keyboard message from the system message queue. Upon receiving
            ///     this hook code, a CBT application must install a <see cref="WhJournalPlayback"/> hook
            ///     procedure in response to the keyboard message.
            /// </summary>
            HCbtKeySkipped = 0x7,

            /// <summary>
            ///     A window is about to be minimized or maximized.
            /// </summary>
            HCbtMinMax = 0x1,

            /// <summary>
            ///     A window is about to be moved or sized.
            /// </summary>
            HCbtMoveSize = 0x0,

            /// <summary>
            ///     The system has retrieved a WM_QUEUESYNC message from the system message queue.
            /// </summary>
            HCbtQs = 0x2,

            /// <summary>
            ///     A window is about to receive the keyboard focus.
            /// </summary>
            HCbtSetFocus = 0x9,

            /// <summary>
            ///     A system command is about to be carried out. This allows a CBT application to prevent
            ///     task switching by means of hot keys.
            /// </summary>
            HCbtSysCommand = 0x8,

            /// <summary>
            ///     The input event occurred in a message box or dialog box.
            /// </summary>
            MsgFDialogBox = 0x0,

            /// <summary>
            ///     The input event occurred in a menu.
            /// </summary>
            MsgFMenu = 0x2,

            /// <summary>
            ///     The input event occurred in a scroll bar.
            /// </summary>
            MsgFScrollbar = 0x5,

            /// <summary>
            ///     The <see cref="WhCallWndProc"/> and <see cref="WhCallWndProcRet"/> hooks enable you to
            ///     monitor messages sent to window procedures. The system calls a <see cref="WhCallWndProc"/>
            ///     hook procedure before passing the message to the receiving window procedure, and calls the
            ///     <see cref="WhCallWndProcRet"/> hook procedure after the window procedure has processed the
            ///     message. The <see cref="WhCallWndProcRet"/> hook passes a pointer to a CWPRETSTRUCT structure
            ///     to the hook procedure. The structure contains the return value from the window procedure that
            ///     processed the message, as well as the message parameters associated with the message.
            ///     Subclassing the window does not work for messages
            ///     set between processes.
            /// </summary>
            WhCallWndProc = 0x4,

            /// <summary>
            ///     The <see cref="WhCallWndProc"/> and <see cref="WhCallWndProcRet"/> hooks enable you to monitor
            ///     messages sent to window procedures. The system calls a <see cref="WhCallWndProc"/> hook procedure
            ///     before passing the message to the receiving window procedure, and calls the
            ///     <see cref="WhCallWndProcRet"/> hook procedure after the window procedure has processed the message.
            ///     The <see cref="WhCallWndProcRet"/> hook passes a pointer to a CWPRETSTRUCT structure to the hook
            ///     procedure. The structure contains the return value from the window procedure that processed the
            ///     message, as well as the message parameters associated with the message. Subclassing the window does
            ///     not work for messages set between processes.
            /// </summary>
            WhCallWndProcRet = 0xc,

            /// <summary>
            ///     The system calls a <see cref="WhCbt"/> hook procedure before activating, creating, destroying,
            ///     minimizing, maximizing, moving, or sizing a window; before completing a system command; before
            ///     removing a mouse or keyboard event from the system message queue; before setting the input focus;
            ///     or before synchronizing with the system message queue. The value the hook procedure returns
            ///     determines whether the system allows or prevents one of these operations. The <see cref="WhCbt"/>
            ///     hook is intended primarily for computer-based training (CBT) applications.
            /// </summary>
            WhCbt = 0x5,

            /// <summary>
            ///     The system calls a <see cref="WhDebug"/> hook procedure before calling hook procedures associated
            ///     with any other hook in the system. You can use this hook to determine whether to allow the system
            ///     to call hook procedures associated with other types
            ///     of hooks.
            /// </summary>
            WhDebug = 0x9,

            /// <summary>
            ///     The <see cref="WhForegroundIdle"/> hook enables you to perform low priority tasks during times
            ///     when its foreground thread is idle. The system calls a <see cref="WhForegroundIdle"/> hook
            ///     procedure when the application's foreground thread is about to become idle.
            /// </summary>
            WhForegroundIdle = 0xb,

            /// <summary>
            ///     The <see cref="WhGetMessage"/> hook enables an application to monitor messages about to be
            ///     returned by the GetMessage or PeekMessage function. You can use the <see cref="WhGetMessage"/>
            ///     hook to monitor mouse and keyboard input and other messages posted to the message
            ///     queue.
            /// </summary>
            WhGetMessage = 0x3,

            /// <summary>
            ///     The <see cref="WhHardware"/> hook enables you to monitor various hardware events.
            /// </summary>
            WhHardware = 0x8,

            /// <summary>
            ///     The <see cref="WhJournalPlayback"/> hook enables an application to insert messages into the
            ///     system message queue. You can use this hook to play back a series of mouse and
            ///     keyboard events recorded earlier by using <see cref="WhJournalRecord"/>. Regular mouse and
            ///     keyboard input is disabled as long as a <see cref="WhJournalPlayback"/> hook is installed. A
            ///     <see cref="WhJournalPlayback"/> hook is a global hook-it cannot be used as a thread-specific
            ///     hook. The <see cref="WhJournalPlayback"/> hook returns a time-out value. This value tells the
            ///     system how many milliseconds to wait before processing the current message from the playback
            ///     hook. This enables the hook to control the timing of the events it plays back.
            /// </summary>
            WhJournalPlayback = 0x1,

            /// <summary>
            ///     The <see cref="WhJournalRecord"/> hook enables you to monitor and record input events. Typically,
            ///     you use this hook to record a sequence of mouse and keyboard events to play back later by using
            ///     <see cref="WhJournalPlayback"/>. The <see cref="WhJournalRecord"/> hook is a global hook-it cannot
            ///     be used as a thread-specific hook.
            /// </summary>
            WhJournalRecord = 0x0,

            /// <summary>
            ///     The <see cref="WhKeyboard"/> hook enables an application to monitor message traffic for WM_KEYDOWN
            ///     and WM_KEYUP messages about to be returned by the GetMessage or PeekMessage function. You can use
            ///     the <see cref="WhKeyboard"/> hook to monitor keyboard input posted to a message queue.
            /// </summary>
            WhKeyboard = 0x2,

            /// <summary>
            ///     The <see cref="WhKeyboardLl"/> hook enables you to monitor keyboard input events about to be
            ///     posted in a thread input queue.
            /// </summary>
            WhKeyboardLl = 0xd,

            /// <summary>
            ///     The <see cref="WhMouse"/> hook enables you to monitor mouse messages about to be returned by
            ///     the GetMessage or PeekMessage function. You can use the <see cref="WhMouse"/> hook to monitor
            ///     mouse input posted to a message queue.
            /// </summary>
            WhMouse = 0x7,

            /// <summary>
            ///     The <see cref="WhMouseLl"/> hook enables you to monitor mouse input events about to be posted
            ///     in a thread input queue.
            /// </summary>
            WhMouseLl = 0xe,

            /// <summary>
            ///     The <see cref="WhMsgFilter"/> and <see cref="WhSysMsgFilter"/> hooks enable you to monitor messages
            ///     about to be processed by a menu, scroll bar, message box, or dialog box, and to detect when a
            ///     different window is about to be activated as a result of the user's pressing the ALT+TAB or ALT+ESC
            ///     key combination. The <see cref="WhMsgFilter"/> hook can only monitor messages passed to a menu,
            ///     scroll bar, message box, or dialog box created by the application that installed the hook procedure.
            ///     The <see cref="WhSysMsgFilter"/> hook monitors such messages for all applications.
            /// </summary>
            WhMsgFilter = -0x1,

            /// <summary>
            ///     A shell application can use the <see cref="WhShell"/> hook to receive important notifications. The
            ///     system calls a <see cref="WhShell"/> hook procedure when the shell application is about to be
            ///     activated and when a top-level window is created or destroyed. Note that custom shell applications
            ///     do not receive <see cref="WhShell"/> messages. Therefore, any application that registers itself as
            ///     the default shell must call the SystemParametersInfo function before it (or any other application)
            ///     can receive WH_SHELL messages. This function must be called with SPI_SETMINIMIZEDMETRICS and a
            ///     MINIMIZEDMETRICS structure. Set the iArrange member of this structure to ARW_HIDE.
            /// </summary>
            WhShell = 0xa,

            /// <summary>
            ///     The <see cref="WhMsgFilter"/> and <see cref="WhSysMsgFilter"/> hooks enable you to monitor messages
            ///     about to be processed by a menu, scroll bar, message box, or dialog box, and to detect when a
            ///     different window is about to be activated as a result of the user's pressing the ALT+TAB or ALT+ESC
            ///     key combination. The <see cref="WhMsgFilter"/> hook can only monitor messages passed to a menu,
            ///     scroll bar, message box, or dialog box created by the application that installed the hook procedure.
            ///     The <see cref="WhSysMsgFilter"/> hook monitors such messages for all applications. The
            ///     <see cref="WhMsgFilter"/> and <see cref="WhSysMsgFilter"/> hooks enable you to perform message
            ///     filtering during modal loops that is equivalent to the filtering done in the main message loop. For
            ///     example, an application often examines a new message in the main loop between the time it retrieves
            ///     the message from the queue and the time it dispatches the message, performing special processing as
            ///     appropriate. However, during a modal loop, the system retrieves and dispatches messages without
            ///     allowing an application the chance to filter the messages in its main message loop. If an
            ///     application installs a <see cref="WhMsgFilter"/> or <see cref="WhSysMsgFilter"/> hook procedure,
            ///     the system calls the procedure during the modal loop.
            /// </summary>
            WhSysMsgFilter = 0x6
        }

        /// <summary>
        ///     Provides enumerated attribute values of windows statements.
        /// </summary>
        [Flags]
        public enum WindowLongFlags
        {
            /// <summary>
            ///     Retrieves the address of the dialog box procedure, or a handle representing the
            ///     address of the dialog box procedure. You must use the CallWindowProc function to
            ///     call the dialog box procedure.
            /// </summary>
            DwlDlgProc = 0x4,

            /// <summary>
            ///     Retrieves the return value of a message processed in the dialog box procedure.
            /// </summary>
            DwlMsgResult = 0x0,

            /// <summary>
            ///     Retrieves extra information private to the application, such as handles or pointers.
            /// </summary>
            DwlUser = 0x8,

            /// <summary>
            ///     Sets a new extended window style.
            /// </summary>
            GwlExStyle = -0x2,

            /// <summary>
            ///     Sets a new application instance handle.
            /// </summary>
            GwlHandleInstance = -0x6,

            /// <summary>
            ///     Sets a new identifier of the child window. The window cannot be a top-level window.
            /// </summary>
            GwlId = -0xc,

            /// <summary>
            ///     Sets a new window style.
            /// </summary>
            GwlStyle = -0x10,

            /// <summary>
            ///     Sets the user data associated with the window. This data is intended for use by the
            ///     application that created the window. Its value is initially zero.
            /// </summary>
            GwlUserData = -0x15,

            /// <summary>
            ///     Sets a new address for the window procedure. You cannot change this attribute if the
            ///     window does not belong to the same process as the calling thread.
            /// </summary>
            GwlWndProc = -0x4
        }

        /// <summary>
        ///     Provides enumerated values of system command requests.
        /// </summary>
        [Flags]
        public enum WindowMenuFlags : uint
        {
            /// <summary>
            ///     Closes the window.
            /// </summary>
            ScClose = 0xf060,

            /// <summary>
            ///     Changes the cursor to a question mark with a pointer. If the user then clicks a
            ///     control in the dialog box, the control receives a WM_HELP message.
            /// </summary>
            ScContextHelp = 0xf180,

            /// <summary>
            ///     Selects the default item; the user double-clicked the window menu.
            /// </summary>
            ScDefault = 0xf160,

            /// <summary>
            ///     Activates the window associated with the application-specified hot key. The lParam
            ///     parameter identifies the window to activate.
            /// </summary>
            ScHotkey = 0xf150,

            /// <summary>
            ///     Scrolls horizontally.
            /// </summary>
            ScHScroll = 0xf080,

            /// <summary>
            ///     Retrieves the window menu as a result of a keystroke.
            /// </summary>
            ScKeyMenu = 0xf100,

            /// <summary>
            ///     Maximizes the window.
            /// </summary>
            ScMaximize = 0xf030,

            /// <summary>
            ///     Minimizes the window.
            /// </summary>
            ScMinimize = 0xf020,

            /// <summary>
            ///     Sets the state of the display. This command supports devices that have power-saving
            ///     features, such as a battery-powered personal computer. - The lParam parameter can
            ///     have the following values: -1 (the display is powering on), 1 (the display is going
            ///     to low power), 2 (the display is being shut off).
            /// </summary>
            ScMonitorPower = 0xf170,

            /// <summary>
            ///     Retrieves the window menu as a result of a mouse click.
            /// </summary>
            ScMouseMenu = 0xf090,

            /// <summary>
            ///     Moves the window.
            /// </summary>
            ScMove = 0xf010,

            /// <summary>
            ///     Moves to the next window.
            /// </summary>
            ScNextWindow = 0xf040,

            /// <summary>
            ///     Moves to the previous window.
            /// </summary>
            ScPrevWindow = 0xf050,

            /// <summary>
            ///     Restores the window to its normal position and size.
            /// </summary>
            ScRestore = 0xf120,

            /// <summary>
            ///     Executes the screen saver application specified in the [boot] section of the System.ini file.
            /// </summary>
            ScScreenSave = 0xf140,

            /// <summary>
            ///     Sizes the window.
            /// </summary>
            ScSize = 0xf000,

            /// <summary>
            ///     Activates the Start menu.
            /// </summary>
            ScTaskList = 0xf130,

            /// <summary>
            ///     Scrolls vertically.
            /// </summary>
            ScVScroll = 0xf070,

            /// <summary>
            ///     Indicates whether the screen saver is secure.
            /// </summary>
            ScfIsSecure = 0x1,

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
            WmCopyData = 0x4a,

            /// <summary>
            ///     The dialog box procedure should return TRUE to direct the system to set the keyboard focus to
            ///     the control specified by wParam. Otherwise, it should return FALSE to prevent the system from
            ///     setting the default keyboard focus.
            /// </summary>
            WmInitDialog = 0x110,

            /// <summary>
            ///     Posted to a window when the cursor moves. If the mouse is not captured, the message is posted
            ///     to the window that contains the cursor. Otherwise, the message is posted to the window that
            ///     has captured the mouse.
            /// </summary>
            WmMouseMove = 0x200,

            /// <summary>
            ///     A message that is sent to all top-level windows when the SystemParametersInfo  function changes
            ///     a system-wide setting or when policy settings have changed.
            /// </summary>
            WmSettingChange = 0x1a,

            /// <summary>
            ///     A window receives this message when the user chooses a command from the Window menu (formerly
            ///     known as the system or control menu) or when the user chooses the maximize button, minimize
            ///     button, restore button, or close button.
            /// </summary>
            WmSysCommand = 0x112
        }

        /// <summary>
        ///     Provides enumerated flags of window placements.
        /// </summary>
        [Flags]
        public enum WindowPlacementFlags : uint
        {
            /// <summary>
            ///     If the calling thread and the thread that owns the window are attached to different
            ///     input queues, the system posts the request to the thread that owns the window. This
            ///     prevents the calling thread from blocking its execution while other threads process
            ///     the request.
            /// </summary>
            AsyncWindowPlacement = 0x4,

            /// <summary>
            ///     The restored window will be maximized, regardless of whether it was maximized before it
            ///     was minimized. This setting is only valid the next time the window is restored. It does
            ///     not change the default restoration behavior.
            ///     <para>
            ///         This flag is only valid when the <see cref="ShowWindowFlags.ShowMinimized"/> value
            ///         is specified for the showCmd member.
            ///     </para>
            /// </summary>
            RestoreToMaximized = 0x2,

            /// <summary>
            ///     The coordinates of the minimized window may be specified.
            ///     <para>
            ///         This flag must be specified if the coordinates are set in the ptMinPosition member.
            ///     </para>
            /// </summary>
            SetMinimizedPosition = 0x1
        }

        /// <summary>
        ///     Provides enumerated values of window styles.
        /// </summary>
        [Flags]
        public enum WindowStyleFlags : ulong
        {
            /// <summary>
            ///     The window has a thin-line border.
            /// </summary>
            Border = 0x800000,

            /// <summary>
            ///     The window has a title bar (includes the <see cref="Border"/> style).
            /// </summary>
            Caption = 0xc00000,

            /// <summary>
            ///     The window is a child window. A window with this style cannot have a menu bar. This style cannot
            ///     be used with the WS_POPUP style.
            /// </summary>
            Child = 0x40000000,

            /// <summary>
            ///     Same as the WS_CHILD style.
            /// </summary>
            ChildWindow = Child,

            /// <summary>
            ///     Excludes the area occupied by child windows when drawing occurs within the parent window. This
            ///     style is used when creating the parent window.
            /// </summary>
            ClipChildren = 0x2000000,

            /// <summary>
            ///     Clips child windows relative to each other; that is, when a particular child window receives a
            ///     WM_PAINT message, the <see cref="ClipSiblings"/> style clips all other overlapping child
            ///     windows out of the region of the child window to be updated. If <see cref="ClipSiblings"/>
            ///     is not specified and child windows overlap, it is possible, when drawing within the client area
            ///     of a child window, to draw within the client area of a neighboring child window.
            /// </summary>
            ClipSiblings = 0x4000000,

            /// <summary>
            ///     The window is initially disabled. A disabled window cannot receive input from the user. To change
            ///     this after a window has been created, use the EnableWindow function.
            /// </summary>
            Disabled = 0x8000000,

            /// <summary>
            ///     The window has a border of a style typically used with dialog boxes. A window with this style
            ///     cannot have a title bar.
            /// </summary>
            DlgFrame = 0x400000,

            /// <summary>
            ///     The window is the first control of a group of controls. The group consists of this first control
            ///     and all controls defined after it, up to the next control with the WS_GROUP style. The first
            ///     control in each group usually has the <see cref="TabStop"/> style so that the user can move
            ///     from group to group. The user can subsequently change the keyboard focus from one control in the
            ///     group to the next control in the group by using the direction keys.
            /// </summary>
            Group = 0x20000,

            /// <summary>
            ///     The window has a horizontal scroll bar.
            /// </summary>
            HorScroll = 0x100000,

            /// <summary>
            ///     The window is initially minimized. Same as the <see cref="Minimize"/> style.
            /// </summary>
            Iconic = Minimize,

            /// <summary>
            ///     The window is initially maximized.
            /// </summary>
            Maximize = 0x1000000,

            /// <summary>
            ///     The window has a maximize button. Cannot be combined with the <see cref="ExContextHelp"/> style.
            ///     The <see cref="SysMenu"/> style must also be specified.
            /// </summary>
            MaximizeBox = 0x10000,

            /// <summary>
            ///     The window is initially minimized. Same as the WS_ICONIC style.
            /// </summary>
            Minimize = 0x20000000,

            /// <summary>
            ///     The window has a minimize button. Cannot be combined with the <see cref="ExContextHelp"/> style.
            ///     The <see cref="SysMenu"/> style must also be specified.
            /// </summary>
            MinimizeBox = 0x20000,

            /// <summary>
            ///     The window is an overlapped window. An overlapped window has a title bar and a border. Same
            ///     as the <see cref="Tiled"/> style.
            /// </summary>
            Overlapped = 0x0,

            /// <summary>
            ///     The window is an overlapped window. Same as the <see cref="TiledWindow"/> style.
            /// </summary>
            OverlappedWindow = Overlapped | Caption | SysMenu | ThickFrame | MinimizeBox | MaximizeBox,

            /// <summary>
            ///     The windows is a pop-up window. This style cannot be used with the WS_CHILD style.
            /// </summary>
            Popup = 0x80000000,

            /// <summary>
            ///     The window is a pop-up window. The <see cref="Caption"/> and <see cref="PopupWindow"/> styles
            ///     must be combined to make the window menu visible.
            /// </summary>
            PopupWindow = Popup | Border | SysMenu,

            /// <summary>
            ///     The window has a sizing border. Same as the <see cref="ThickFrame"/> style.
            /// </summary>
            SizeBox = 0x40000,

            /// <summary>
            ///     The window has a window menu on its title bar. The <see cref="Caption"/> style must also be
            ///     specified.
            /// </summary>
            SysMenu = 0x80000,

            /// <summary>
            ///     The window is a control that can receive the keyboard focus when the user presses the TAB key.
            ///     Pressing the TAB key changes the keyboard focus to the next control with the <see cref="TabStop"/>
            ///     style. You can turn this style on and off to change dialog box navigation. To change this style after
            ///     a window has been created, use the SetWindowLong function. For user-created windows and modeless
            ///     dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
            /// </summary>
            TabStop = 0x10000,

            /// <summary>
            ///     The window has a sizing border. Same as the <see cref="SizeBox"/> style.
            /// </summary>
            ThickFrame = 0x40000,

            /// <summary>
            ///     The window is an overlapped window. An overlapped window has a title bar and a border. Same
            ///     as the <see cref="Overlapped"/> style.
            /// </summary>
            Tiled = 0x0,

            /// <summary>
            ///     The window is an overlapped window. Same as the <see cref="OverlappedWindow"/> style.
            /// </summary>
            TiledWindow = Overlapped | Caption | SysMenu | ThickFrame | MinimizeBox | MaximizeBox,

            /// <summary>
            ///     The window is initially visible. This style can be turned on and off by using the ShowWindow
            ///     or SetWindowPos function.
            /// </summary>
            Visible = 0x10000000,

            /// <summary>
            ///     The window has a vertical scroll bar.
            /// </summary>
            VerScroll = 0x200000,

            /// <summary>
            ///     The window accepts drag-drop files.
            /// </summary>
            ExAcceptFiles = 0x10,

            /// <summary>
            ///     Forces a top-level window onto the taskbar when the window is visible.
            /// </summary>
            ExAppWindow = 0x40000,

            /// <summary>
            ///     The window has a border with a sunken edge.
            /// </summary>
            ExClientEdge = 0x200,

            /// <summary>
            ///     Paints all descendants of a window in bottom-to-top painting order using double-buffering.
            /// </summary>
            ExComposited = 0x2000000,

            /// <summary>
            ///     The title bar of the window includes a question mark. When the user clicks the question mark,
            ///     the cursor changes to a question mark with a pointer. If the user then clicks a child window,
            ///     the child receives a WM_HELP message. The child window should pass the message to the parent
            ///     window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The
            ///     Help application displays a pop-up window that typically contains help for the child window.
            ///     <see cref="ExContextHelp"/> cannot be used with the <see cref="MaximizeBox"/> or
            ///     <see cref="MinimizeBox"/> styles.
            /// </summary>
            ExContextHelp = 0x400,

            /// <summary>
            ///     The window itself contains child windows that should take part in dialog box navigation. If
            ///     this style is specified, the dialog manager recurses into children of this window when
            ///     performing navigation operations such as handling the TAB key, an arrow key, or a keyboard
            ///     mnemonic.
            /// </summary>
            ExControlParent = 0x10000,

            /// <summary>
            ///     The window has a double border; the window can, optionally, be created with a title bar by
            ///     specifying the <see cref="Caption"/> style in the dwStyle parameter.
            /// </summary>
            ExDlgModalFrame = 0x1,

            /// <summary>
            ///     The window is a layered window. This style cannot be used if the window has a class style of
            ///     either CS_OWNDC or CS_CLASSDC.
            /// </summary>
            ExLayered = 0x80000,

            /// <summary>
            ///     If the shell language is Hebrew, Arabic, or another language that supports reading order
            ///     alignment, the horizontal origin of the window is on the right edge. Increasing horizontal
            ///     values advance to the left.
            /// </summary>
            ExLayoutRightToLeft = 0x400000,

            /// <summary>
            ///     The window has generic left-aligned properties. This is the default.
            /// </summary>
            ExLeft = 0x0,

            /// <summary>
            ///     If the shell language is Hebrew, Arabic, or another language that supports reading order
            ///     alignment, the vertical scroll bar (if present) is to the left of the client area. For
            ///     other languages, the style is ignored.
            /// </summary>
            ExLeftScrollbar = 0x4000,

            /// <summary>
            ///     The window text is displayed using left-to-right reading-order properties. This is the
            ///     default.
            /// </summary>
            ExLeftToRightReading = 0x0,

            /// <summary>
            ///     The window is a MDI child window.
            /// </summary>
            ExMdiChild = 0x40,

            /// <summary>
            ///     A top-level window created with this style does not become the foreground window when
            ///     the user clicks it. The system does not bring this window to the foreground when the
            ///     user minimizes or closes the foreground window. To activate the window, use the
            ///     SetActiveWindow or SetForegroundWindow function. The window does not appear on the
            ///     taskbar by default. To force the window to appear on the taskbar, use the
            ///     WS_EX_APPWINDOW style.
            /// </summary>
            ExNoActivate = 0x8000000,

            /// <summary>
            ///     The window does not pass its window layout to its child windows.
            /// </summary>
            ExNoInheritLayout = 0x100000,

            /// <summary>
            ///     The child window created with this style does not send the WM_PARENTNOTIFY message
            ///     to its parent window when it is created or destroyed.
            /// </summary>
            ExNoParentNotify = 0x4,

            /// <summary>
            ///     The window does not render to a redirection surface. This is for windows that do not
            ///     have visible content or that use mechanisms other than surfaces to provide their visual.
            /// </summary>
            ExNoRedirectionBitmap = 0x200000,

            /// <summary>
            ///     The window is an overlapped window.
            /// </summary>
            ExOverlappedWindow = ExWindowEdge | ExClientEdge,

            /// <summary>
            ///     The window is palette window, which is a modeless dialog box that presents an array
            ///     of commands.
            /// </summary>
            ExPaletteWindow = ExWindowEdge | ExToolWindow | ExTopMost,

            /// <summary>
            ///     The window has generic "right-aligned" properties. This depends on the window class.
            ///     This style has an effect only if the shell language is Hebrew, Arabic, or another
            ///     language that supports reading-order alignment; otherwise, the style is ignored.
            ///     Using the WS_EX_RIGHT style for static or edit controls has the same effect as using
            ///     the SS_RIGHT or ES_RIGHT style, respectively. Using this style with button controls
            ///     has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.
            /// </summary>
            ExRight = 0x1000,

            /// <summary>
            ///     The vertical scroll bar (if present) is to the right of the client area. This is the
            ///     default.
            /// </summary>
            ExRightScrollbar = 0x0,

            /// <summary>
            ///     If the shell language is Hebrew, Arabic, or another language that supports reading-order
            ///     alignment, the window text is displayed using right-to-left reading-order properties.
            ///     For other languages, the style is ignored.
            /// </summary>
            ExRightToLeftReading = 0x2000,

            /// <summary>
            ///     The window has a three-dimensional border style intended to be used for items that do
            ///     not accept user input.
            /// </summary>
            ExStaticEdge = 0x20000,

            /// <summary>
            ///     The window is intended to be used as a floating toolbar. A tool window has a title bar
            ///     that is shorter than a normal title bar, and the window title is drawn using a smaller
            ///     font. A tool window does not appear in the taskbar or in the dialog that appears when
            ///     the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed
            ///     on the title bar. However, you can display the system menu by right-clicking or by
            ///     typing ALT+SPACE.
            /// </summary>
            ExToolWindow = 0x80,

            /// <summary>
            ///     The window should be placed above all non-topmost windows and should stay above them,
            ///     even when the window is deactivated. To add or remove this style, use the SetWindowPos
            ///     function.
            /// </summary>
            ExTopMost = 0x8,

            /// <summary>
            ///     The window should not be painted until siblings beneath the window (that were created
            ///     by the same thread) have been painted. The window appears transparent because the bits
            ///     of underlying sibling windows have already been painted. To achieve transparency without
            ///     these restrictions, use the SetWindowRgn function.
            /// </summary>
            ExTransparent = 0x20,

            /// <summary>
            ///     The window has a border with a raised edge.
            /// </summary>
            ExWindowEdge = 0x100
        }

        /// <summary>
        ///     Provides enumerated values of window theme attributes.
        /// </summary>
        [Flags]
        public enum WindowThemeAttributeFlags : uint
        {
            /// <summary>
            ///     Prevents the window caption from being drawn.
            /// </summary>
            NoDrawCaption = 0x1,

            /// <summary>
            ///     Prevents the system icon from being drawn.
            /// </summary>
            NoDrawIcon = 0x2,

            /// <summary>
            ///     Prevents the system icon menu from appearing.
            /// </summary>
            NoSysMenu = 0x4,

            /// <summary>
            ///     Prevents mirroring of the question mark, even in right-to-left (RTL) layout.
            /// </summary>
            NoMirrorHelp = 0x8,

            /// <summary>
            ///     A mask that contains all the valid bits.
            /// </summary>
            ValidBits = NoDrawCaption | NoDrawIcon | NoSysMenu | NoMirrorHelp
        }

        /// <summary>
        ///     Throws the specified error code if it is not specified as a handled error.
        /// </summary>
        /// <param name="error">
        ///     The Win32 error code associated with this exception.
        /// </param>
        /// <param name="handledErrors">
        ///     A sequence of handled Win32 error codes.
        /// </param>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static int ThrowError(int error, params int[] handledErrors)
        {
            if (handledErrors?.Any(i => i == error) == true)
                return error;
            throw new Win32Exception(error);
        }

        /// <summary>
        ///     Throws the specified error code if it is not specified as a handled error.
        /// </summary>
        /// <param name="error">
        ///     The Win32 error code associated with this exception.
        /// </param>
        /// <param name="handledError">
        ///     A handled Win32 error code.
        /// </param>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static int ThrowError(int error, int handledError = 0)
        {
            if (error != handledError)
                throw new Win32Exception(error);
            return error;
        }

        /// <summary>
        ///     Throws the last error code returned by the last unmanaged function.
        /// </summary>
        /// <param name="forceMessage">
        ///     A detailed description of the error that is thrown if no Win32 error code was found.
        ///     <para>
        ///         If this parameter is NULL, no exception is thrown in this case.
        ///     </para>
        /// </param>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static void ThrowLastError(string forceMessage = null)
        {
            var code = Marshal.GetLastWin32Error();
            if (code != 0)
                throw new Win32Exception(code);
            if (!string.IsNullOrWhiteSpace(forceMessage))
                throw new Win32Exception(forceMessage);
        }

        /// <summary>
        ///     Provides enumerated values of token access rights.
        /// </summary>
        [Flags]
        internal enum AccessTokenFlags : uint
        {
            /// <summary>
            ///     The right to read the information in the object's security descriptor, not including the
            ///     information in the system access control list (SACL), the right to delete the object,
            ///     the right to modify the discretionary access control list (DACL) in the object's security
            ///     descriptor, the right to change the owner in the object's security descriptor, and the
            ///     right to use the object for synchronization.
            /// </summary>
            StandardRightsRequired = 0xf0000,

            /// <summary>
            ///     The right to read the information in the object's security descriptor, not including the
            ///     information in the system access control list (SACL).
            /// </summary>
            StandardRightsRead = 0x20000,

            /// <summary>
            ///     Required to attach a primary token to a process. The SE_ASSIGNPRIMARYTOKEN_NAME privilege
            ///     is also required to accomplish this task.
            /// </summary>
            TokenAssignPrimary = 0x1,

            /// <summary>
            ///     Required to duplicate an access token.
            /// </summary>
            TokenDuplicate = 0x2,

            /// <summary>
            ///     Required to attach an impersonation access token to a process.
            /// </summary>
            TokenImpersonate = 0x4,

            /// <summary>
            ///     Required to query an access token.
            /// </summary>
            TokenQuery = 0x8,

            /// <summary>
            ///     Required to query the source of an access token.
            /// </summary>
            TokenQuerySource = 0x10,

            /// <summary>
            ///     Required to enable or disable the privileges in an access token.
            /// </summary>
            TokenAdjustPrivileges = 0x20,

            /// <summary>
            ///     Required to adjust the attributes of the groups in an access token.
            /// </summary>
            TokenAdjustGroups = 0x40,

            /// <summary>
            ///     Required to change the default owner, primary group, or DACL of an access token.
            /// </summary>
            TokenAdjustDefault = 0x80,

            /// <summary>
            ///     Required to adjust the session ID of an access token. The SE_TCB_NAME privilege is required.
            /// </summary>
            TokenAdjustSessionid = 0x100,

            /// <summary>
            ///     Combines <see cref="StandardRightsRead"/> and <see cref="TokenQuery"/>.
            /// </summary>
            TokenRead = StandardRightsRead | TokenQuery,

            /// <summary>
            ///     Combines all possible access rights for a token.
            /// </summary>
            TokenAllAccess = StandardRightsRequired | TokenAssignPrimary | TokenDuplicate | TokenImpersonate | TokenQuery | TokenQuerySource | TokenAdjustPrivileges | TokenAdjustGroups | TokenAdjustDefault | TokenAdjustSessionid
        }

        /// <summary>
        ///     Provides enumerated values that control how the process is created.
        /// </summary>
        [Flags]
        internal enum CreationFlags
        {
            /// <summary>
            ///     The new process does not inherit the error mode of the calling process. Instead, the new
            ///     process gets the current default error mode.
            /// </summary>
            CreateDefaultErrorMode = 0x4000000,

            /// <summary>
            ///     The new process has a new console, instead of inheriting the parent's console.
            /// </summary>
            CreateNewConsole = 0x10,

            /// <summary>
            ///     The new process is the root process of a new process group. The process group includes all
            ///     processes that are descendants of this root process. The process identifier of the new
            ///     process group is the same as the process identifier, which is returned in the lpProcessInfo
            ///     parameter.
            /// </summary>
            CreateNewProcessGroup = 0x200,

            /// <summary>
            ///     This flag is only valid starting a 16-bit Windows-based application. If set, the new process
            ///     runs in a private Virtual DOS Machine (VDM). By default, all 16-bit Windows-based applications
            ///     run in a single, shared VDM. The advantage of running separately is that a crash only terminates
            ///     the single VDM; any other programs running in distinct VDMs continue to function normally. Also,
            ///     16-bit Windows-based applications that run in separate VDMs have separate input queues. That
            ///     means that if one application stops responding momentarily, applications in separate VDMs continue
            ///     to receive input.
            /// </summary>
            CreateSeparateWowVdm = 0x800,

            /// <summary>
            ///     The primary thread of the new process is created in a suspended state.
            /// </summary>
            CreateSuspended = 0x4,

            /// <summary>
            ///     Indicates the format of the lpEnvironment parameter. If this flag is set, the environment block
            ///     pointed to by lpEnvironment uses Unicode characters. Otherwise, the environment block uses ANSI
            ///     characters.
            /// </summary>
            CreateUnicodeEnvironment = 0x400,

            /// <summary>
            ///     The process is created with extended startup information.
            /// </summary>
            ExtendedStartupInfoPresent = 0x80000
        }

        /// <summary>
        ///     Provides enumerated values that specify file informations.
        /// </summary>
        [Flags]
        internal enum FileInfoFlags : uint
        {
            /// <summary>
            ///     Apply the appropriate overlays to the file's icon. The <see cref="Icon"/> flag must also be set.
            /// </summary>
            AddOverlays = 0x20,

            /// <summary>
            ///     Modify <see cref="Attributes"/> to indicate that the dwAttributes member of the SHFILEINFO structure
            ///     at psfi contains the specific attributes that are desired. These attributes are passed to
            ///     IShellFolder::GetAttributesOf. If this flag is not specified, 0xFFFFFFFF is passed to
            ///     IShellFolder::GetAttributesOf, requesting all attributes. This flag cannot be specified with
            ///     the <see cref="Icon"/> flag.
            /// </summary>
            AttrSpecified = 0x20000,

            /// <summary>
            ///     Retrieve the item attributes. The attributes are copied to the dwAttributes member of the
            ///     structure specified in the psfi parameter. These are the same attributes that are obtained
            ///     from IShellFolder::GetAttributesOf.
            /// </summary>
            Attributes = 0x800,

            /// <summary>
            ///     Retrieve the display name for the file, which is the name as it appears in Windows Explorer.
            ///     The name is copied to the szDisplayName member of the structure specified in psfi. The
            ///     returned display name uses the long file name, if there is one, rather than the 8.3 form of
            ///     the file name. Note that the display name can be affected by settings such as whether
            ///     extensions are shown.
            /// </summary>
            DisplayName = 0x200,

            /// <summary>
            ///     Retrieve the type of the executable file if pszPath identifies an executable file. The
            ///     information is packed into the return value. This flag cannot be specified with any other
            ///     flags.
            /// </summary>
            ExeType = 0x2000,

            /// <summary>
            ///     Retrieve the handle to the icon that represents the file and the index of the icon within
            ///     the system image list. The handle is copied to the hIcon member of the structure specified
            ///     by psfi, and the index is copied to the iIcon member.
            /// </summary>
            Icon = 0x100,

            /// <summary>
            ///     Retrieve the name of the file that contains the icon representing the file specified by
            ///     pszPath, as returned by the IExtractIcon::GetIconLocation method of the file's icon handler.
            ///     Also retrieve the icon index within that file. The name of the file containing the icon is
            ///     copied to the szDisplayName member of the structure specified by psfi. The icon's index is
            ///     copied to that structure's iIcon member.
            /// </summary>
            IconLocation = 0x1000,

            /// <summary>
            ///     Modify <see cref="Icon"/>, causing the function to retrieve the file's large icon. The
            ///     <see cref="Icon"/> flag must also be set.
            /// </summary>
            LargeIcon = 0x0,

            /// <summary>
            ///     Modify <see cref="Icon"/>, causing the function to add the link overlay to the file's icon. The
            ///     <see cref="Icon"/> flag must also be set.
            /// </summary>
            LinkOverlay = 0x8000,

            /// <summary>
            ///     Modify <see cref="Icon"/>, causing the function to retrieve the file's open icon. Also used to modify
            ///     <see cref="SysIconIndex"/>, causing the function to return the handle to the system image list that
            ///     contains the file's small open icon. A container object displays an open icon to indicate that the
            ///     container is open. The <see cref="Icon"/> and/or <see cref="SysIconIndex"/> flag must also be set.
            /// </summary>
            OpenIcon = 0x2,

            /// <summary>
            ///     Return the index of the overlay icon. The value of the overlay index is returned in the upper
            ///     eight bits of the iIcon member of the structure specified by psfi. This flag requires that the
            ///     <see cref="Icon"/> be set as well.
            /// </summary>
            OverlayIndex = 0x40,

            /// <summary>
            ///     Indicate that pszPath is the address of an ITEMIDLIST structure rather than a path name.
            /// </summary>
            PidL = 0x8,

            /// <summary>
            ///     Modify <see cref="Icon"/>, causing the function to blend the file's icon with the system highlight
            ///     color. The <see cref="Icon"/> flag must also be set.
            /// </summary>
            Selected = 0x10000,

            /// <summary>
            ///     Modify <see cref="Icon"/>, causing the function to retrieve a Shell-sized icon. If this flag is not
            ///     specified the function sizes the icon according to the system metric values. The <see cref="Icon"/>
            ///     flag must also be set.
            /// </summary>
            ShellIconSize = 0x4,

            /// <summary>
            ///     Modify <see cref="Icon"/>, causing the function to retrieve the file's small icon. Also used to modify
            ///     <see cref="SysIconIndex"/>, causing the function to return the handle to the system image list that
            ///     contains small icon images. The <see cref="Icon"/> and/or <see cref="SysIconIndex"/> flag must also
            ///     be set.
            /// </summary>
            SmallIcon = 0x1,

            /// <summary>
            ///     Retrieve the index of a system image list icon. If successful, the index is copied to the iIcon
            ///     member of psfi. The return value is a handle to the system image list. Only those images whose
            ///     indices are successfully copied to iIcon are valid. Attempting to access other images in the
            ///     system image list will result in undefined behavior.
            /// </summary>
            SysIconIndex = 0x4000,

            /// <summary>
            ///     Retrieve the string that describes the file's type. The string is copied to the szTypeName
            ///     member of the structure specified in psfi.
            /// </summary>
            TypeName = 0x400,

            /// <summary>
            ///     Indicates that the function should not attempt to access the file specified by pszPath. Rather,
            ///     it should act as if the file specified by pszPath exists with the file attributes passed in
            ///     dwFileAttributes. This flag cannot be combined with the <see cref="Attributes"/>,
            ///     <see cref="ExeType"/>, or <see cref="PidL"/> flags.
            /// </summary>
            UseFileAttributes = 0x10
        }

        /// <summary>
        ///     Provides enumerated logon options.
        /// </summary>
        internal enum LogonOptions
        {
            /// <summary>
            ///     Log on, then load the user's profile in the HKEY_USERS registry key. The function returns
            ///     after the profile has been loaded. Loading the profile can be time-consuming, so it is best
            ///     to use this value only if you must access the information in the HKEY_CURRENT_USER registry
            ///     key.
            /// </summary>
            WithProfile = 1,

            /// <summary>
            ///     Log on, but use the specified credentials on the network only. The new process uses the same
            ///     token as the caller, but the system creates a new logon session within LSA, and the process
            ///     uses the specified credentials as the default credentials.
            ///     <para>
            ///         This value can be used to create a process that uses a different set of credentials locally
            ///         than it does remotely. This is useful in inter-domain scenarios where there is no trust
            ///         relationship.
            ///     </para>
            /// </summary>
            NetCredentialsOnly = 2
        }

        /// <summary>
        ///     Specifies the type of application that is described by the <see cref="RmProcessInfo"/> structure.
        /// </summary>
        internal enum RmAppTypes
        {
            /// <summary>
            ///     The application cannot be classified as any other type. An application of this type can
            ///     only be shut down by a forced shutdown.
            /// </summary>
            UnknownApp = 0x0,

            /// <summary>
            ///     A Windows application run as a stand-alone process that displays a top-level window.
            /// </summary>
            MainWindow = 0x1,

            /// <summary>
            ///     A Windows application that does not run as a stand-alone process and does not display a
            ///     top-level window.
            /// </summary>
            OtherWindow = 0x2,

            /// <summary>
            ///     The application is a Windows service.
            /// </summary>
            Service = 0x3,

            /// <summary>
            ///     The application is Windows Explorer.
            /// </summary>
            Explorer = 0x4,

            /// <summary>
            ///     The application is a stand-alone console application.
            /// </summary>
            Console = 0x5,

            /// <summary>
            ///     A system restart is required to complete the installation because a process cannot be shut
            ///     down. The process cannot be shut down because of the following reasons. The process may be
            ///     a critical process. The current user may not have permission to shut down the process. The
            ///     process may belong to the primary installer that started the Restart Manager.
            /// </summary>
            Critical = 0x3e8
        }

        /// <summary>
        ///     Provides enumerated values of service access rights.
        /// </summary>
        [Flags]
        internal enum ServiceAccessRights
        {
            /// <summary>
            ///     Includes <see cref="StandardRequired"/> in addition to all access rights in this table.
            /// </summary>
            AllAccess = 0xf01ff,

            /// <summary>
            ///     Required to call the ChangeServiceConfig or ChangeServiceConfig2 function to
            ///     change the service configuration. Because this grants the caller the right to
            ///     change the executable file that the system runs, it should be granted only to
            ///     administrators.
            /// </summary>
            ChangeConfig = 0x2,

            /// <summary>
            ///     Required to call the EnumDependentServices function to enumerate all the services
            ///     dependent on the service.
            /// </summary>
            EnumerateDependents = 0x8,

            /// <summary>
            ///     Required to call the ControlService function to ask the service to report its
            ///     status immediately.
            /// </summary>
            Interrogate = 0x80,

            /// <summary>
            ///     Required to call the ControlService function to pause or continue the service.
            /// </summary>
            PauseContinue = 0x40,

            /// <summary>
            ///     Required to call the QueryServiceConfig and QueryServiceConfig2 functions
            ///     to query the service configuration.
            /// </summary>
            QueryConfig = 0x1,

            /// <summary>
            ///     Required to call the QueryServiceStatus or QueryServiceStatusEx function to ask
            ///     the service control manager about the status of the service.
            ///     <para>
            ///         Required to call the NotifyServiceStatusChange function to receive notification
            ///         when a service changes status.
            ///     </para>
            /// </summary>
            QueryStatus = 0x4,

            /// <summary>
            ///     The standard rights.
            /// </summary>
            StandardRequired = 0xf0000,

            /// <summary>
            ///     Required to call the StartService function to start the service.
            /// </summary>
            Start = 0x10,

            /// <summary>
            ///     Required to call the ControlService function to stop the service.
            /// </summary>
            Stop = 0x20,

            /// <summary>
            ///     Required to call the ControlService function to specify a user-defined control
            ///     code.
            /// </summary>
            UserDefinedControl = 0x100
        }

        /// <summary>
        ///     Provides enumerated values of the service start options.
        /// </summary>
        internal enum ServiceBootFlags
        {
            /// <summary>
            ///     A service started automatically by the service control manager during system startup.
            ///     For more information, see Automatically Starting Services.
            /// </summary>
            AutoStart = 0x2,

            /// <summary>
            ///     A device driver started by the system loader. This value is valid only for driver
            ///     services.
            /// </summary>
            BootStart = 0x0,

            /// <summary>
            ///     A service started by the service control manager when a process calls the
            ///     StartService function. For more information, see Starting Services on Demand.
            /// </summary>
            DemandStart = 0x3,

            /// <summary>
            ///     A service that cannot be started. Attempts to start the service result in the error
            ///     code.
            /// </summary>
            Disabled = 0x4,

            /// <summary>
            ///     A device driver started by the IoInitSystem function. This value is valid only for
            ///     driver services.
            /// </summary>
            SystemStart = 0x1
        }

        /// <summary>
        ///     Provides enumerated values of the service control options.
        /// </summary>
        internal enum ServiceControlOptions
        {
            /// <summary>
            ///     Notifies a paused service that it should resume. The hService handle must have the
            ///     <see cref="ServiceAccessRights.PauseContinue"/> access right.
            /// </summary>
            Continue = 0x3,

            /// <summary>
            ///     Notifies a service that it should report its current status information to the service control
            ///     manager. The hService handle must have the <see cref="ServiceAccessRights.Interrogate"/>
            ///     access right.
            ///     <para>
            ///         Note that this control is not generally useful as the SCM is aware of the current state of
            ///         the service.
            ///     </para>
            /// </summary>
            Interrogate = 0x4,

            /// <summary>
            ///     Notifies a network service that there is a new component for binding. The hService handle must
            ///     have the <see cref="ServiceAccessRights.PauseContinue"/> access right. However, this
            ///     control code has been deprecated; use Plug and Play functionality instead.
            /// </summary>
            NetBindAdd = 0x7,

            /// <summary>
            ///     Notifies a network service that one of its bindings has been disabled. The hService handle must
            ///     have the <see cref="ServiceAccessRights.PauseContinue"/> access right. However, this
            ///     control code has been deprecated; use Plug and Play functionality instead.
            /// </summary>
            NetBindDisable = 0xa,

            /// <summary>
            ///     Notifies a network service that a disabled binding has been enabled. The hService handle must
            ///     have the <see cref="ServiceAccessRights.PauseContinue"/> access right. However, this
            ///     control code has been deprecated; use Plug and Play functionality instead.
            /// </summary>
            NetBindEnable = 0x9,

            /// <summary>
            ///     Notifies a network service that a component for binding has been removed. The hService handle
            ///     must have the <see cref="ServiceAccessRights.PauseContinue"/> access right. However,
            ///     this control code has been deprecated; use Plug and Play functionality instead.
            /// </summary>
            NetBindRemove = 0x8,

            /// <summary>
            ///     Notifies a service that its startup parameters have changed. The hService handle must have the
            ///     <see cref="ServiceAccessRights.PauseContinue"/> access right.
            /// </summary>
            ParamChange = 0x6,

            /// <summary>
            ///     Notifies a service that it should pause. The hService handle must have the
            ///     <see cref="ServiceAccessRights.PauseContinue"/> access right.
            /// </summary>
            Pause = 0x2,

            /// <summary>
            ///     Notifies a service that it should stop. The hService handle must have the
            ///     <see cref="ServiceAccessRights.Stop"/> access right.
            ///     <para>
            ///         After sending the stop request to a service, you should not send other controls to the
            ///         service.
            ///     </para>
            /// </summary>
            Stop = 0x1
        }

        /// <summary>
        ///     Provides enumerated values of the service control types.
        /// </summary>
        internal enum ServiceControlTypes
        {
            /// <summary>
            ///     The service is a network component that can accept changes in its binding without being
            ///     stopped and restarted.
            /// </summary>
            AcceptNetBindChange = 0x10,

            /// <summary>
            ///     The service can reread its startup parameters without being stopped and restarted.
            /// </summary>
            AcceptParamChange = 0x8,

            /// <summary>
            ///     The service can be paused and continued.
            /// </summary>
            AcceptPauseContinue = 0x2,

            /// <summary>
            ///     The service can be stopped.
            /// </summary>
            AcceptStop = 0x1
        }

        /// <summary>
        ///     Provides enumerated values of service control manager access rights.
        /// </summary>
        [Flags]
        internal enum ServiceManagerAccessRights
        {
            /// <summary>
            ///     Includes <see cref="StandardRequired"/>, in addition to all access rights in this table.
            /// </summary>
            AllAccess = 0xf003f,

            /// <summary>
            ///     Required to call the CreateService function to create a service object and add it
            ///     to the database.
            /// </summary>
            CreateService = 0x2,

            /// <summary>
            ///     Required to connect to the service control manager.
            /// </summary>
            Connect = 0x1,

            /// <summary>
            ///     Required to call the EnumServicesStatus or EnumServicesStatusEx function to list
            ///     the services that are in the database.
            ///     <para>
            ///         Required to call the NotifyServiceStatusChange function to receive notification
            ///         when any service is created or deleted.
            ///     </para>
            /// </summary>
            EnumerateService = 0x4,

            /// <summary>
            ///     Required to call the LockServiceDatabase function to acquire a lock on the database.
            /// </summary>
            Lock = 0x8,

            /// <summary>
            ///     Required to call the NotifyBootConfigStatus function.
            /// </summary>
            ModifyBootConfig = 0x20,

            /// <summary>
            ///     Required to call the QueryServiceLockStatus function to retrieve the lock status
            ///     information for the database.
            /// </summary>
            QueryLockStatus = 0x10,

            /// <summary>
            ///     The standard rights.
            /// </summary>
            StandardRequired = 0xf0000
        }

        /// <summary>
        ///     Provides enumerated values of the current state of the service.
        /// </summary>
        internal enum ServiceStateTypes
        {
            /// <summary>
            ///     The service continue is pending.
            /// </summary>
            ContinuePending = 0x5,

            /// <summary>
            ///     The service pause is pending.
            /// </summary>
            PausePending = 0x6,

            /// <summary>
            ///     The service is paused.
            /// </summary>
            Paused = 0x7,

            /// <summary>
            ///     The service is running.
            /// </summary>
            Running = 0x4,

            /// <summary>
            ///     The service is starting.
            /// </summary>
            StartPending = 0x2,

            /// <summary>
            ///     The service is stopping.
            /// </summary>
            StopPending = 0x3,

            /// <summary>
            ///     The service is not running.
            /// </summary>
            Stopped = 0x1
        }

        /// <summary>
        ///     Provides enumerated values that specify security impersonation levels.
        /// </summary>
        internal enum SecurityImpersonationLevels
        {
            /// <summary>
            ///     The server process cannot obtain identification information about the client, and it cannot
            ///     impersonate the client. It is defined with no value given, and thus, by ANSI C rules,
            ///     defaults to a value of zero.
            /// </summary>
            SecurityAnonymous = 0,

            /// <summary>
            ///     The server process can obtain information about the client, such as security identifiers and
            ///     privileges, but it cannot impersonate the client. This is useful for servers that export
            ///     their own objects, for example, database products that export tables and views. Using the
            ///     retrieved client-security information, the server can make access-validation decisions
            ///     without being able to use other services that are using the client's security context.
            /// </summary>
            SecurityIdentification = 1,

            /// <summary>
            ///     The server process can impersonate the client's security context on its local system. The
            ///     server cannot impersonate the client on remote systems.
            /// </summary>
            SecurityImpersonation = 2,

            /// <summary>
            ///     The server process can impersonate the client's security context on remote systems.
            /// </summary>
            SecurityDelegation = 3
        }

        /// <summary>
        ///     Provides enumerated values of service types.
        /// </summary>
        internal enum ServiceTypes
        {
            /// <summary>
            ///     Reserved.
            /// </summary>
            Adapter = 0x4,

            /// <summary>
            ///     File system driver service.
            /// </summary>
            FileSystemDriver = 0x2,

            /// <summary>
            ///     The service can interact with the desktop.
            ///     <para>
            ///         If you specify either <see cref="Win32OwnProcess"/> or
            ///         <see cref="Win32ShareProcess"/>, and the service is running in the
            ///         context of the LocalSystem account, you can also specify this value.
            ///     </para>
            /// </summary>
            InteractiveProcess = 0x100,

            /// <summary>
            ///     Driver service.
            /// </summary>
            KernelDriver = 0x1,

            /// <summary>
            ///     Reserved.
            /// </summary>
            RecognizerDriver = 0x8,

            /// <summary>
            ///     Service that runs in its own process.
            /// </summary>
            Win32OwnProcess = 0x10,

            /// <summary>
            ///     Service that shares a process with one or more other services.
            /// </summary>
            Win32ShareProcess = 0x20
        }

        /// <summary>
        ///     Provides enumerated values of process start flags.
        /// </summary>
        [Flags]
        internal enum StartFlags
        {
            /// <summary>
            ///     Indicates that the cursor is in feedback mode for two seconds after CreateProcess is called.
            ///     The Working in Background cursor is displayed (see the Pointers tab in the Mouse control
            ///     panel utility).
            ///     <para>
            ///         If during those two seconds the process makes the first GUI call, the system gives five
            ///         more seconds to the process. If during those five seconds the process shows a window,
            ///         the system gives five more seconds to the process to finish drawing the window.
            ///     </para>
            /// </summary>
            ForceOnFeedback = 0x40,

            /// <summary>
            ///     Indicates that the feedback cursor is forced off while the process is starting. The Normal
            ///     Select cursor is displayed.
            /// </summary>
            ForceOffFeedback = 0x80,

            /// <summary>
            ///     Indicates that any windows created by the process cannot be pinned on the taskbar.
            ///     <para>
            ///         This flag must be combined with <see cref="TitleIsAppId"/>.
            ///     </para>
            /// </summary>
            PreventPinning = 0x2000,

            /// <summary>
            ///     Indicates that the process should be run in full-screen mode, rather than in windowed mode.
            ///     <para>
            ///         This flag is only valid for console applications running on an x86 computer.
            ///     </para>
            /// </summary>
            RunFullscreen = 0x20,

            /// <summary>
            ///     The lpTitle member contains an AppUserModelID. This identifier controls how the taskbar and
            ///     Start menu present the application, and enables it to be associated with the correct shortcuts
            ///     and Jump Lists.
            ///     <para>
            ///         If <see cref="PreventPinning"/> is used, application windows cannot be pinned on the
            ///         taskbar. The use of any AppUserModelID-related window properties by the application
            ///         overrides this setting for that window only.
            ///     </para>
            ///     <para>
            ///         This flag cannot be used with <see cref="TitleIsLinkName"/>.
            ///     </para>
            /// </summary>
            TitleIsAppId = 0x1000,

            /// <summary>
            ///     The lpTitle member contains the path of the shortcut file (.lnk) that the user invoked to
            ///     start this process. This is typically set by the shell when a .lnk file pointing to the
            ///     launched application is invoked. Most applications will not need to set this value.
            ///     <para>
            ///         This flag cannot be used with <see cref="TitleIsAppId"/>.
            ///     </para>
            /// </summary>
            TitleIsLinkName = 0x800,

            /// <summary>
            ///     The command line came from an untrusted source.
            /// </summary>
            UntrustedSource = 0x8000,

            /// <summary>
            ///     The dwXCountChars and dwYCountChars members contain additional information.
            /// </summary>
            UseCountChars = 0x8,

            /// <summary>
            ///     The dwXCountChars and dwYCountChars members contain additional information.
            /// </summary>
            UseFillAttribute = 0x10,

            /// <summary>
            ///     The hStdInput member contains additional information.
            ///     <para>
            ///         This flag cannot be used with <see cref="UseStdHandles"/>.
            ///     </para>
            /// </summary>
            UseHotkey = 0x200,

            /// <summary>
            ///     The dwX and dwY members contain additional information.
            /// </summary>
            UsePosition = 0x4,

            /// <summary>
            ///     The wShowWindow member contains additional information.
            /// </summary>
            UseShowWindow = 0x1,

            /// <summary>
            ///     The dwXSize and dwYSize members contain additional information.
            /// </summary>
            UseSize = 0x2,

            /// <summary>
            ///     The hStdInput, hStdOutput, and hStdError members contain additional information.
            ///     <para>
            ///         If this flag is specified when calling one of the process creation functions, the
            ///         handles must be inheritable and the function's bInheritHandles parameter must be set
            ///         to TRUE.
            ///     </para>
            ///     <para>
            ///         Handles must be closed with <see cref="NativeMethods.CloseHandle"/> when they are no
            ///         longer needed.
            ///     </para>
            ///     <para>
            ///         This flag cannot be used with <see cref="UseHotkey"/>.
            ///     </para>
            /// </summary>
            UseStdHandles = 0x100
        }

        /// <summary>
        ///     Provides enumerated values that differentiate between a primary token and an impersonation token.
        /// </summary>
        internal enum TokenTypes
        {
            /// <summary>
            ///     Indicates a primary token.
            /// </summary>
            TokenPrimary = 1,

            /// <summary>
            ///     Indicates an impersonation token.
            /// </summary>
            TokenImpersonation = 2
        }

        /// <summary>
        ///     Provides enumerated values of window theme attribute types.
        /// </summary>
        internal enum WindowThemeAttributeTypes : uint
        {
            /// <summary>
            ///     Non-client area window attributes will be set.
            /// </summary>
            NonClient = 1
        }

        /// <summary>
        ///     Provides native based functions.
        /// </summary>
        public static class NativeHelper
        {
            /// <summary>
            ///     Enables or disables privileges in the specified access token. Enabling or disabling privileges in
            ///     an access token requires <see cref="AccessTokenFlags.TokenAdjustPrivileges"/> access.
            /// </summary>
            /// <param name="tokenHandle">
            ///     A handle to the access token that contains the privileges to be modified. The handle must have
            ///     <see cref="AccessTokenFlags.TokenAdjustPrivileges"/> access to the token. If the PreviousState
            ///     parameter is not NULL, the handle must also have <see cref="AccessTokenFlags.TokenQuery"/> access.
            /// </param>
            /// <param name="disableAllPrivileges">
            ///     Specifies whether the function disables all of the token's privileges. If this value is TRUE, the
            ///     function disables all privileges and ignores the NewState parameter. If it is FALSE, the function
            ///     modifies privileges based on the information pointed to by the NewState parameter.
            /// </param>
            /// <param name="newState">
            ///     A pointer to a <see cref="TokenPrivileges"/> structure that specifies an array of privileges and their
            ///     attributes. If the disableAllPrivileges parameter is FALSE, the AdjustTokenPrivileges function
            ///     enables, disables, or removes these privileges for the token. The following table describes the
            ///     action taken by the AdjustTokenPrivileges function, based on the privilege attribute. If
            ///     disableAllPrivileges is TRUE, the function ignores this parameter.
            /// </param>
            public static bool AdjustTokenPrivileges(IntPtr tokenHandle, bool disableAllPrivileges, ref TokenPrivileges newState) =>
                NativeMethods.AdjustTokenPrivileges(tokenHandle, disableAllPrivileges, ref newState, 0u, IntPtr.Zero, IntPtr.Zero);

            /// <summary>
            ///     Enables you to produce special effects when showing or hiding windows. There are four types of
            ///     animation: roll, slide, collapse or expand, and alpha-blended fade.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window to animate. The calling thread must own this window.
            /// </param>
            /// <param name="time">
            ///     The time it takes to play the animation, in milliseconds. Typically, an animation takes 200
            ///     milliseconds to play.
            /// </param>
            /// <param name="flags">
            ///     The type of animation.
            /// </param>
            public static bool AnimateWindow(IntPtr hWnd, int time = 200, AnimateWindowFlags flags = AnimateWindowFlags.Blend) =>
                NativeMethods.AnimateWindow(hWnd, time, flags);

            /// <summary>
            ///     Enables you to produce special effects when showing or hiding windows. There are four types of
            ///     animation: roll, slide, collapse or expand, and alpha-blended fade.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window to animate. The calling thread must own this window.
            /// </param>
            /// <param name="flags">
            ///     The type of animation.
            /// </param>
            public static bool AnimateWindow(IntPtr hWnd, AnimateWindowFlags flags) =>
                NativeMethods.AnimateWindow(hWnd, 200, flags);

            /// <summary>
            ///     Passes the hook information to the next hook procedure in the current hook chain. A hook
            ///     procedure can call this function either before or after processing the hook information.
            /// </summary>
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
            public static IntPtr CallNextHookEx(int nCode, IntPtr wParam, IntPtr lParam) =>
                NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

            /// <summary>
            ///     Centers the position of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window to change.
            /// </param>
            /// <param name="hPar">
            ///     A handle to the parent window.
            /// </param>
            /// <param name="alwaysVisible">
            ///     true to force the window to remain in screen area; otherwise, false.
            /// </param>
            public static void CenterWindow(IntPtr hWnd, IntPtr hPar, bool alwaysVisible = false)
            {
                var rect = new Rectangle();
                if (!NativeMethods.GetWindowRect(hWnd, ref rect) || rect.Width < 1 || rect.Height < 1)
                    return;
                var width = rect.Width - rect.X;
                var height = rect.Height - rect.Y;
                if (hPar == default(IntPtr))
                {
                    var cursorPos = GetCursorPos();
                    var screen = Screen.PrimaryScreen;
                    foreach (var scr in Screen.AllScreens)
                    {
                        if (!scr.Bounds.Contains(cursorPos))
                            continue;
                        screen = scr;
                        break;
                    }
                    rect.X = screen.Bounds.X + screen.Bounds.Width / 2;
                    rect.Y = screen.Bounds.Y + screen.Bounds.Height / 2;
                }
                else
                {
                    rect = new Rectangle();
                    if (!NativeMethods.GetWindowRect(hPar, ref rect) || rect.Width < 1 || rect.Height < 1)
                        return;
                    rect.X = rect.X + (rect.Width - rect.X) / 2;
                    rect.Y = rect.Y + (rect.Height - rect.Y) / 2;
                }
                rect.X = rect.X - width / 2;
                rect.Y = rect.Y - height / 2;
                if (alwaysVisible)
                {
                    var range = new Rectangle
                    {
                        X = SystemInformation.VirtualScreen.X,
                        Y = SystemInformation.VirtualScreen.Y,
                        Width = SystemInformation.VirtualScreen.Width - width,
                        Height = SystemInformation.VirtualScreen.Height - height
                    };
                    rect.X = Math.Max(rect.X, range.X);
                    rect.X = Math.Min(rect.X, range.Width);
                    rect.Y = Math.Max(rect.Y, range.Y);
                    rect.Y = Math.Min(rect.Y, range.Height);
                }
                rect.Width = width;
                rect.Height = height;
                NativeMethods.MoveWindow(hWnd, rect.X, rect.Y, rect.Width, rect.Height, false);
            }

            /// <summary>
            ///     Centers the position of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window to change.
            /// </param>
            /// <param name="alwaysVisible">
            ///     true to force the window to remain in screen area; otherwise, false.
            /// </param>
            public static void CenterWindow(IntPtr hWnd, bool alwaysVisible = true) =>
                CenterWindow(hWnd, default(IntPtr), alwaysVisible);

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
            public static bool ClientToScreen(IntPtr hWnd, ref Point lpPoint) =>
                NativeMethods.ClientToScreen(hWnd, ref lpPoint);

            /// <summary>
            ///     Closes an open object handle.
            /// </summary>
            /// <param name="handle">
            ///     A valid handle to an open object.
            /// </param>
            public static bool CloseHandle(IntPtr handle) =>
                NativeMethods.CloseHandle(handle);

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
            ///     <see cref="ModifyMenuFlags.ByCommand"/> or <see cref="ModifyMenuFlags.ByPosition"/>.
            /// </param>
            public static int DeleteMenu(IntPtr hMenu, uint nPosition, ModifyMenuFlags wFlags) =>
                NativeMethods.DeleteMenu(hMenu, nPosition, wFlags);

            /// <summary>
            ///     Destroys an icon and frees any memory the icon occupied.
            /// </summary>
            /// <param name="hIcon">
            ///     A handle to the icon to be destroyed. The icon must not be in use.
            /// </param>
            public static bool DestroyIcon(IntPtr hIcon) =>
                NativeMethods.DestroyIcon(hIcon);

            /// <summary>
            ///     Disables the maximize button of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            public static void DisableWindowMaximizeButton(IntPtr hWnd)
            {
                var style = (long)NativeMethods.GetWindowLong(hWnd, WindowLongFlags.GwlStyle);
                if ((style & 0x10000L) == 0x10000L)
                    style &= ~0x10000L;
                SetWindowLong(hWnd, WindowLongFlags.GwlStyle, (IntPtr)style);
            }

            /// <summary>
            ///     Disables the minimize button of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            public static void DisableWindowMinimizeButton(IntPtr hWnd)
            {
                var style = (long)NativeMethods.GetWindowLong(hWnd, WindowLongFlags.GwlStyle);
                if ((style & 0x20000L) == 0x20000L)
                    style &= ~0x20000L;
                SetWindowLong(hWnd, WindowLongFlags.GwlStyle, (IntPtr)style);
            }

            /// <summary>
            ///     Redraws the menu bar of the specified window. If the menu bar changes after the system has
            ///     created the window, this function must be called to draw the changed menu bar.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose menu bar is to be redrawn.
            /// </param>
            public static bool DrawMenuBar(IntPtr hWnd) =>
                NativeMethods.DrawMenuBar(hWnd);

            /// <summary>
            ///     Duplicates an object handle.
            /// </summary>
            /// <param name="hSourceProcessHandle">
            ///     A handle to the process with the handle to be duplicated.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.ProcessDupHandle"/> access right.
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
            ///         The handle must have the <see cref="AccessRights.ProcessDupHandle"/> access right.
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
            ///         <see cref="DuplicateOptions.SameAccess"/> flag. Otherwise, the flags that
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
            ///     <see cref="DuplicateOptions"/>.
            /// </param>
            public static bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle, uint dwDesiredAccess, bool bInheritHandle, uint dwOptions) =>
                NativeMethods.DuplicateHandle(hSourceProcessHandle, hSourceHandle, hTargetProcessHandle, out lpTargetHandle, dwDesiredAccess, bInheritHandle, dwOptions);

            /// <summary>
            ///     Extends the window frame into the client area.
            /// </summary>
            /// <param name="hWnd">
            ///     The handle to the window in which the frame will be extended into the client area.
            /// </param>
            /// <param name="pMarInset">
            ///     A pointer to a MARGINS structure that describes the margins to use when extending the frame
            ///     into the client area.
            /// </param>
            public static int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref ThemeMargins pMarInset) =>
                NativeMethods.DwmExtendFrameIntoClientArea(hWnd, ref pMarInset);

            /// <summary>
            ///     Obtains a value that indicates whether Desktop Window Manager (DWM) composition is enabled.
            /// </summary>
            /// <param name="pfEnabled">
            ///     A pointer to a value that, when this function returns successfully, receives TRUE if DWM
            ///     composition is enabled; otherwise, FALSE.
            /// </param>
            public static int DwmIsCompositionEnabled(ref int pfEnabled) =>
                NativeMethods.DwmIsCompositionEnabled(ref pfEnabled);

            /// <summary>
            ///     Destroys a modal dialog box, causing the system to end any processing for the dialog box.
            /// </summary>
            /// <param name="hDlg">
            ///     A handle to the dialog box to be destroyed.
            /// </param>
            /// <param name="nResult">
            ///     The value to be returned to the application from the function that created the dialog box.
            /// </param>
            public static int EndDialog(IntPtr hDlg, IntPtr nResult) =>
                NativeMethods.EndDialog(hDlg, nResult);

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
            public static bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam) =>
                NativeMethods.EnumChildWindows(hWndParent, lpEnumFunc, lParam);

            /// <summary>
            ///     Enumerates all nonchild windows associated with a thread by passing the handle to each window,
            ///     in turn, to an application-defined callback function.
            /// </summary>
            /// <param name="dwThreadId">
            ///     The identifier of the thread whose windows are to be enumerated.
            /// </param>
            /// <param name="lpfn">
            ///     A pointer to an application-defined callback function.
            /// </param>
            /// <param name="lParam">
            ///     An application-defined value to be passed to the callback function.
            /// </param>
            public static bool EnumThreadWindows(uint dwThreadId, EnumThreadWndProc lpfn, IntPtr lParam) =>
                NativeMethods.EnumThreadWindows(dwThreadId, lpfn, lParam);

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
            public static int ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, int nIcons) =>
                NativeMethods.ExtractIconEx(lpszFile, nIconIndex, phiconLarge, phiconSmall, nIcons);

            /// <summary>
            ///     Determines the MIME type from the data provided.
            /// </summary>
            /// <param name="pBc">
            ///     A pointer to the IBindCtx interface. Can be set to NULL.
            /// </param>
            /// <param name="pwzUrl">
            ///     A pointer to a string value that contains the URL of the data. Can be set to NULL if pBuffer
            ///     contains the data to be sniffed.
            /// </param>
            /// <param name="pBuffer">
            ///     A pointer to the buffer that contains the data to be sniffed. Can be set to NULL if pwzUrl
            ///     contains a valid URL.
            /// </param>
            /// <param name="cbSize">
            ///     An unsigned long integer value that contains the size of the buffer.
            /// </param>
            /// <param name="pwzMimeProposed">
            ///     A pointer to a string value that contains the proposed MIME type. This value is authoritative if
            ///     type cannot be determined from the data. If the proposed type contains a semi-colon (;) it is
            ///     removed. This parameter can be set to NULL.
            /// </param>
            /// <param name="dwMimeFlags">
            ///     The search and filter options.
            /// </param>
            /// <param name="ppwzMimeOut">
            ///     The address of a string value that receives the suggested MIME type.
            /// </param>
            /// <param name="dwReserved">
            ///     Reserved. Must be set to 0.
            /// </param>
            /// <returns>
            ///     This function can return one of these values.
            ///     <para>
            ///         S_OK: The operation completed successfully.
            ///     </para>
            ///     <para>
            ///         E_FAIL: The operation failed.
            ///     </para>
            ///     <para>
            ///         E_INVALIDARG: One or more arguments are invalid.
            ///     </para>
            ///     <para>
            ///         E_OUTOFMEMORY: There is insufficient memory to complete the operation.
            ///     </para>
            /// </returns>
            public static int FindMimeFromData(IntPtr pBc, string pwzUrl, byte[] pBuffer, int cbSize, string pwzMimeProposed, MimeFlags dwMimeFlags, out IntPtr ppwzMimeOut, int dwReserved) =>
                NativeMethods.FindMimeFromData(pBc, pwzUrl, pBuffer, cbSize, pwzMimeProposed, dwMimeFlags, out ppwzMimeOut, dwReserved);

            /// <summary>
            ///     Determines the MIME type from the file.
            /// </summary>
            /// <param name="fPath">
            ///     The file to check.
            /// </param>
            /// <param name="dwMimeFlags">
            ///     The search and filter options.
            /// </param>
            public static string FindMimeFromFile(string fPath, MimeFlags dwMimeFlags = MimeFlags.EnableMimeSniffing | MimeFlags.IgnoreMimeTextPlain | MimeFlags.ReturnUpdatedImgMimes)
            {
                try
                {
                    var file = PathEx.Combine(fPath);
                    if (!PathEx.IsValidPath(file))
                        throw new ArgumentException();
                    if (!File.Exists(file))
                        throw new PathNotFoundException(file);
                    var buffer = new byte[256];
                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                        fs.Read(buffer, 0, fs.Length >= buffer.Length ? buffer.Length : (int)fs.Length);
                    NativeMethods.FindMimeFromData(IntPtr.Zero, null, buffer, 256, null, dwMimeFlags, out var mimetype, 0);
                    var mime = Marshal.PtrToStringUni(mimetype);
                    Marshal.FreeCoTaskMem(mimetype);
                    return mime;
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return "unknown/unknown";
                }
            }

            /// <summary>
            ///     Retrieves a handle to a window whose class name is matched.
            /// </summary>
            /// <param name="hWndParent">
            ///     A handle to the parent window whose child windows are to be searched.
            /// </param>
            /// <param name="className">
            ///     The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx
            ///     function.
            /// </param>
            public static void FindNestedWindow(ref IntPtr hWndParent, string className)
            {
                try
                {
                    hWndParent = hWndParent == IntPtr.Zero ? NativeMethods.FindWindow(className, null) : NativeMethods.FindWindowEx(hWndParent, IntPtr.Zero, className, null);
                    if (hWndParent == IntPtr.Zero)
                        throw new ArgumentException("Failed to locate window (class: '" + className + "').");
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }

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
            public static IntPtr FindWindow(string lpClassName, string lpWindowName) =>
                NativeMethods.FindWindow(lpClassName, lpWindowName);

            /// <summary>
            ///     Retrieves a handle to the top-level window whose window name match the specified strings. This
            ///     function does not search child windows. This function does not perform a case-sensitive search.
            /// </summary>
            /// <param name="lpWindowName">
            ///     The window name (the window's title). If this parameter is NULL, all window names match.
            /// </param>
            public static IntPtr FindWindowByCaption(string lpWindowName) =>
                NativeMethods.FindWindowByCaption(IntPtr.Zero, lpWindowName);

            /// <summary>
            ///     Retrieves a handle to a window whose class name and window name match the specified strings. The
            ///     function searches child windows, beginning with the one following the specified child window. This
            ///     function does not perform a case-sensitive search.
            /// </summary>
            /// <param name="hWndParent">
            ///     A handle to the parent window whose child windows are to be searched.
            ///     <para>
            ///         If hwndParent is NULL, the function uses the desktop window as the parent window. The function
            ///         searches among windows that are child windows of the desktop.
            ///     </para>
            ///     <para>
            ///         If hwndParent is HWND_MESSAGE, the function searches all message-only windows.
            ///     </para>
            /// </param>
            /// <param name="hWndChildAfter">
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
            public static IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow) =>
                NativeMethods.FindWindowEx(hWndParent, hWndChildAfter, lpszClass, lpszWindow);

            /// <summary>
            ///     Gets the window title from the foreground window (the window with which the user is currently working).
            /// </summary>
            public static string GetActiveWindowTitle()
            {
                var hWnd = NativeMethods.GetForegroundWindow();
                var sb = new StringBuilder(256);
                return NativeMethods.GetWindowText(hWnd, sb, 256) > 0 ? sb.ToString() : string.Empty;
            }

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
            public static int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount) =>
                NativeMethods.GetClassName(hWnd, lpClassName, nMaxCount);

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
            public static bool GetClientRect(IntPtr hWnd, out Rectangle lpRect) =>
                NativeMethods.GetClientRect(hWnd, out lpRect);

            /// <summary>
            ///     Retrieves the thread identifier of the calling thread.
            /// </summary>
            public static uint GetCurrentThreadId() =>
                NativeMethods.GetCurrentThreadId();

            /// <summary>
            ///     Retrieves the position of the mouse cursor, in screen coordinates.
            /// </summary>
            public static Point GetCursorPos()
            {
                if (!NativeMethods.GetCursorPos(out var point))
                    point = new Point(0, 0);
                return point;
            }

            /// <summary>
            ///     Retrieves the identifier of the specified control.
            /// </summary>
            /// <param name="hWndCtl">
            ///     A handle to the control.
            /// </param>
            public static int GetDlgCtrlId(IntPtr hWndCtl) =>
                NativeMethods.GetDlgCtrlID(hWndCtl);

            /// <summary>
            ///     Retrieves a handle to a control in the specified dialog box.
            /// </summary>
            /// <param name="hDlg">
            ///     A handle to the dialog box that contains the control.
            /// </param>
            /// <param name="nIddlgItem">
            ///     The identifier of the control to be retrieved.
            /// </param>
            public static IntPtr GetDlgItem(IntPtr hDlg, int nIddlgItem) =>
                NativeMethods.GetDlgItem(hDlg, nIddlgItem);

            /// <summary>
            ///     Retrieves a handle to the foreground window (the window with which the user is currently working).
            ///     The system assigns a slightly higher priority to the thread that creates the foreground window
            ///     than it does to other threads.
            /// </summary>
            public static IntPtr GetForegroundWindow() =>
                NativeMethods.GetForegroundWindow();

            /// <summary>
            ///     Retrieves the calling thread's last-error code value. The last-error code is
            ///     maintained on a per-thread basis. Multiple threads do not overwrite each
            ///     other's last-error code.
            /// </summary>
            public static int GetLastError() =>
                NativeMethods.GetLastError();

            /// <summary>
            ///     Retrieves a handle to the menu assigned to the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose menu handle is to be retrieved.
            /// </param>
            public static IntPtr GetMenu(IntPtr hWnd) =>
                NativeMethods.GetMenu(hWnd);

            /// <summary>
            ///     Determines the number of items in the specified menu.
            /// </summary>
            /// <param name="hMenu">
            ///     A handle to the menu to be examined.
            /// </param>
            public static int GetMenuItemCount(IntPtr hMenu) =>
                NativeMethods.GetMenuItemCount(hMenu);

            /// <summary>
            ///     Retrieves a handle to the specified window's parent or owner.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose parent window handle is to be retrieved.
            /// </param>
            public static IntPtr GetParent(IntPtr hWnd) =>
                NativeMethods.GetParent(hWnd);

            /// <summary>
            ///     Gets basic process information about the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            public static ProcessBasicInformation GetProcessBasicInformation(IntPtr hWnd)
            {
                var status = NativeMethods.NtQueryInformationProcess(hWnd, ProcessInfoFlags.ProcessBasicInformation, out var pbi, (uint)Marshal.SizeOf(typeof(ProcessBasicInformation)), out IntPtr _);
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
            ///     Retrieves the process identifier of the specified process.
            /// </summary>
            /// <param name="handle">
            ///     A handle to the process.
            /// </param>
            public static uint GetProcessId(IntPtr handle) =>
                NativeMethods.GetProcessId(handle);

            /// <summary>
            ///     Retrieves the name of the executable file for the specified process.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process. The handle must have the
            ///     <see cref="AccessRights.ProcessQueryInformation"/> or
            ///     <see cref="AccessRights.ProcessQueryLimitedInformation"/> access right.
            /// </param>
            /// <param name="lpImageFileName">
            ///     A pointer to a buffer that receives the full path to the executable file.
            /// </param>
            /// <param name="nSize">
            ///     The size of the lpImageFileName buffer, in characters.
            /// </param>
            public static bool GetProcessImageFileName(IntPtr hProcess, StringBuilder lpImageFileName, int nSize) =>
                NativeMethods.GetProcessImageFileName(hProcess, lpImageFileName, nSize);

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
            public static IntPtr GetStdHandle(int nStdHandle) =>
                NativeMethods.GetStdHandle(nStdHandle);

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
            public static IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert) =>
                NativeMethods.GetSystemMenu(hWnd, bRevert);

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
                    NativeMethods.DwmGetColorizationParameters(out var parameters);
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
            ///     Returns the language identifier for the user UI language for the current user. If the current user
            ///     has not set a language, <see cref="GetUserDefaultUILanguage"/> returns the preferred language set
            ///     for the system. If there is no preferred language set for the system, then the system default UI
            ///     language (also known as "install language") is returned.
            /// </summary>
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public static ushort GetUserDefaultUILanguage() =>
                NativeMethods.GetUserDefaultUILanguage();

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
            ///     value, specify one of the <see cref="WindowLongFlags"/>.GWL_??? values.
            /// </param>
            public static int GetWindowLong(IntPtr hWnd, WindowLongFlags nIndex) =>
                NativeMethods.GetWindowLong(hWnd, nIndex);

            /// <summary>
            ///     Gets the show state and the restored, minimized, and maximized positions of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="lpwndpl">
            ///     A pointer to a <see cref="WindowPlacement"/> structure that specifies the new show state and window
            ///     positions.
            /// </param>
            public static bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl) =>
                NativeMethods.GetWindowPlacement(hWnd, ref lpwndpl);

            /// <summary>
            ///     Gets information about the placement of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            public static WindowPlacement GetWindowPlacement(IntPtr hWnd)
            {
                var placement = new WindowPlacement();
                NativeMethods.GetWindowPlacement(hWnd, ref placement);
                return placement;
            }

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
            public static bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect) =>
                NativeMethods.GetWindowRect(hWnd, ref lpRect);

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
            public static int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength) =>
                NativeMethods.GetWindowText(hWnd, text, maxLength);

            /// <summary>
            ///     Retrieves the length, in characters, of the specified window's title bar text (if the window has a
            ///     title bar). If the specified window is a control, the function retrieves the length of the text within
            ///     the control. However, GetWindowTextLength cannot retrieve the length of the text of an edit control in
            ///     another application.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window or control.
            /// </param>
            public static int GetWindowTextLength(IntPtr hWnd) =>
                NativeMethods.GetWindowTextLength(hWnd);

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
            public static uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId) =>
                NativeMethods.GetWindowThreadProcessId(hWnd, out lpdwProcessId);

            /// <summary>
            ///     Removes the caret from the screen. Hiding a caret does not destroy its current shape or invalidate the
            ///     insertion point.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window that owns the caret. If this parameter is NULL, HideCaret searches the current
            ///     task for the window that owns the caret.
            /// </param>
            public static bool HideCaret(IntPtr hWnd) =>
                NativeMethods.HideCaret(hWnd);

            /// <summary>
            ///     Minimizes and hides the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            public static void HideWindow(IntPtr hWnd)
            {
                NativeMethods.ShowWindow(hWnd, ShowWindowFlags.Minimize);
                NativeMethods.ShowWindow(hWnd, ShowWindowFlags.Hide);
            }

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
            ///     The identifier of the new menu item or, if the uFlags parameter has the <see cref="ModifyMenuFlags.Popup"/>
            ///     flag set, a handle to the drop-down menu or submenu.
            /// </param>
            /// <param name="lpNewItem">
            ///     The content of the new menu item. The interpretation of lpNewItem depends on whether the uFlags parameter
            ///     includes the <see cref="ModifyMenuFlags.Bitmap"/>, <see cref="ModifyMenuFlags.OwnerDraw"/>, or
            ///     <see cref="ModifyMenuFlags.String"/> flag, as follows.
            /// </param>
            public static bool InsertMenu(IntPtr hMenu, uint wPosition, ModifyMenuFlags wFlags, UIntPtr wIdNewItem, string lpNewItem) =>
                NativeMethods.InsertMenu(hMenu, wPosition, wFlags, wIdNewItem, lpNewItem);

            /// <summary>
            ///     Adds a rectangle to the specified window's update region. The update region represents the portion of the
            ///     window's client area that must be redrawn.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose update region has changed. If this parameter is NULL, the system invalidates
            ///     and redraws all windows, not just the windows for this application, and sends the WM_ERASEBKGND and
            ///     WM_NCPAINT messages before the function returns. Setting this parameter to NULL is not recommended.
            /// </param>
            /// <param name="lpRect">
            ///     A pointer to a RECT structure that contains the client coordinates of the rectangle to be added to the update
            ///     region. If this parameter is NULL, the entire client area is added to the update region.
            /// </param>
            /// <param name="bErase">
            ///     Specifies whether the background within the update region is to be erased when the update region is processed.
            ///     If this parameter is TRUE, the background is erased when the BeginPaint function is called. If this parameter
            ///     is FALSE, the background remains unchanged.
            /// </param>
            public static bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase) =>
                NativeMethods.InvalidateRect(hWnd, lpRect, bErase);

            /// <summary>
            ///     Tests if a visual style for the current application is active.
            /// </summary>
            public static bool IsThemeActive() =>
                NativeMethods.IsThemeActive();

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
            public static IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName) =>
                NativeMethods.LoadLibrary(lpFileName);

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
            public static int LoadString(IntPtr hInstance, uint uId, StringBuilder lpBuffer, int nBufferMax) =>
                NativeMethods.LoadString(hInstance, uId, lpBuffer, nBufferMax);

            /// <summary>
            ///     Allocates the specified number of bytes from the heap.
            /// </summary>
            /// <param name="flag">
            ///     The memory allocation attributes. The default is the LMEM_FIXED value. This
            ///     parameter can be one or more of the <see cref="LocalAllocFlags"/>.
            /// </param>
            /// <param name="size">
            ///     The number of bytes to allocate. If this parameter is zero and the uFlags
            ///     parameter specifies <see cref="LocalAllocFlags.LMemMoveable"/>, the function
            ///     returns a handle to a memory object that is marked as discarded.
            /// </param>
            public static IntPtr LocalAlloc(LocalAllocFlags flag, UIntPtr size) =>
                NativeMethods.LocalAlloc(flag, size);

            /// <summary>
            ///     Frees the specified local memory object and invalidates its handle.
            /// </summary>
            /// <param name="hMem">
            ///     A handle to the local memory object. This handle is returned by either the
            ///     <see cref="LocalAlloc"/> function.
            /// </param>
            public static IntPtr LocalFree(IntPtr hMem) =>
                NativeMethods.LocalFree(hMem);

            /// <summary>
            ///     Disables or enables drawing in the specified window. Only one window can be locked
            ///     at a time.
            /// </summary>
            /// <param name="hWndLock">
            ///     The window in which drawing will be disabled. If this parameter is NULL, drawing in
            ///     the locked window is enabled.
            /// </param>
            public static bool LockWindowUpdate(IntPtr hWndLock) =>
                NativeMethods.LockWindowUpdate(hWndLock);

            /// <summary>
            ///     Retrieves the name that corresponds to the privilege represented on a specific system by a specified
            ///     locally unique identifier (LUID).
            /// </summary>
            /// <param name="lpSystemName">
            ///     A pointer to a null-terminated string that specifies the name of the system on which the privilege
            ///     name is retrieved. If a null string is specified, the function attempts to find the privilege name
            ///     on the local system.
            /// </param>
            /// <param name="lpLuid">
            ///     A pointer to the <see cref="LuId"/> by which the privilege is known on the target system.
            /// </param>
            /// <param name="lpName">
            ///     A pointer to a buffer that receives a null-terminated string that represents the privilege name.
            ///     For example, this string could be "SeSecurityPrivilege".
            /// </param>
            /// <param name="cchName">
            ///     A pointer to a variable that specifies the size, in a TCHAR value, of the lpName buffer. When the
            ///     function returns, this parameter contains the length of the privilege name, not including the
            ///     terminating null character. If the buffer pointed to by the lpName parameter is too small, this
            ///     variable contains the required size.
            /// </param>
            public static bool LookupPrivilegeName(string lpSystemName, LuId lpLuid, StringBuilder lpName, ref int cchName)
            {
                var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(lpLuid));
                Marshal.StructureToPtr(lpLuid, ptr, true);
                try
                {
                    var len = 0;
                    if (!NativeMethods.LookupPrivilegeName(null, ptr, null, ref len))
                        return false;
                    lpName.EnsureCapacity(++len);
                    return NativeMethods.LookupPrivilegeName(null, ptr, lpName, ref len);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            /// <summary>
            ///     Retrieves the <see cref="LuId"/> used on a specified system to locally represent the specified
            ///     privilege name.
            /// </summary>
            /// <param name="lpSystemName">
            ///     A pointer to a null-terminated string that specifies the name of the system on which the privilege
            ///     name is retrieved. If a null string is specified, the function attempts to find the privilege name
            ///     on the local system.
            /// </param>
            /// <param name="lpName">
            ///     A pointer to a null-terminated string that specifies the name of the privilege, as defined in the
            ///     Winnt.h header file.
            /// </param>
            /// <param name="lpLuid">
            ///     A pointer to a variable that receives the <see cref="LuId"/> by which the privilege is known on
            ///     the system specified by the lpSystemName parameter.
            /// </param>
            public static bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LuId lpLuid) =>
                NativeMethods.LookupPrivilegeValue(lpSystemName, lpName, ref lpLuid);

            /// <summary>
            ///     Translates (maps) a virtual-key code into a scan code or character value, or translates a scan code
            ///     into a virtual-key code.
            /// </summary>
            /// <param name="uCode">
            ///     The virtual key code or scan code for a key. How this value is interpreted depends on the value of
            ///     the uMapType parameter.
            /// </param>
            /// <param name="uMapType">
            ///     The translation to be performed. The value of this parameter depends on the value of the uCode
            ///     parameter.
            /// </param>
            public static uint MapVirtualKey(uint uCode, uint uMapType) =>
                NativeMethods.MapVirtualKey(uCode, uMapType);

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
                NativeMethods.GetWindowRect(hWnd, ref cRect);
                if (cRect == nRect)
                    return;
                if (nRect.Width <= 0 || nRect.Height <= 0)
                    nRect.Size = cRect.Size;
                NativeMethods.MoveWindow(hWnd, nRect.X, nRect.Y, nRect.Width, nRect.Height, cRect.Size != nRect.Size);
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
            public static int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint) =>
                NativeMethods.MoveWindow(hWnd, x, y, nWidth, nHeight, bRepaint);

            /// <summary>
            ///     Changes the position of a specified window that is outside the virtual screen to move it back to the visible
            ///     screen area.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            public static void MoveWindowToVisibleScreenArea(IntPtr hWnd)
            {
                if (!WindowIsOutOfScreenArea(hWnd, out var rect))
                    return;
                NativeMethods.MoveWindow(hWnd, rect.X, rect.Y, rect.Width, rect.Height, false);
            }

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
            ///     <see cref="ProcessBasicInformation"/> parameter.
            /// </param>
            /// <param name="piLen">
            ///     The size of the buffer pointed to by the <see cref="ProcessBasicInformation"/> parameter, in bytes.
            /// </param>
            /// <param name="rLen">
            ///     A pointer to a variable in which the function returns the size of the requested information. If the function
            ///     was successful, this is the size of the information written to the buffer pointed to by the
            ///     <see cref="ProcessBasicInformation"/> parameter, but if the buffer was too small, this is the minimum size
            ///     of buffer needed to receive the information successfully.
            /// </param>
            public static uint NtQueryInformationProcess([In] IntPtr hndl, [In] ProcessInfoFlags piCl, [Out] out ProcessBasicInformation processInformation, [In] uint piLen, [Out] out IntPtr rLen) =>
                NativeMethods.NtQueryInformationProcess(hndl, piCl, out processInformation, piLen, out rLen);

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
            public static IntPtr OpenProcess(AccessRights dwDesiredAccess, bool bInheritHandle, uint dwProcessId) =>
                NativeMethods.OpenProcess(dwDesiredAccess, bInheritHandle, dwProcessId);

            /// <summary>
            ///     Places (posts) a message in the message queue associated with the thread that created the
            ///     specified window and returns without waiting for the thread to process the message.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose window procedure is to receive the message. The following values
            ///     have special meanings.
            ///     <para>
            ///         HWND_BROADCAST ((HWND)0xffff): The message is posted to all top-level windows in the system,
            ///         including disabled or invisible unowned windows, overlapped windows, and pop-up windows. The
            ///         message is not posted to child windows.
            ///     </para>
            ///     <para>
            ///         NULL: The function behaves like a call to PostThreadMessage with the dwThreadId parameter
            ///         set to the identifier of the current thread.
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
            public static bool PostMessage(HandleRef hWnd, uint msg, IntPtr wParam, IntPtr lParam) =>
                NativeMethods.PostMessage(hWnd, msg, wParam, lParam);

            /// <summary>
            ///     Places (posts) a message in the message queue associated with the thread that created the
            ///     specified window and returns without waiting for the thread to process the message.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose window procedure is to receive the message. The following values
            ///     have special meanings.
            ///     <para>
            ///         HWND_BROADCAST ((HWND)0xffff): The message is posted to all top-level windows in the system,
            ///         including disabled or invisible unowned windows, overlapped windows, and pop-up windows.
            ///         The message is not posted to child windows.
            ///     </para>
            ///     <para>
            ///         NULL: The function behaves like a call to PostThreadMessage with the dwThreadId parameter
            ///         set to the identifier of the current thread.
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
            public static bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) =>
                NativeMethods.PostMessage(new HandleRef(null, hWnd), msg, wParam, lParam);

            /// <summary>
            ///     Reads data from an area of memory in a specified process. The entire area to be
            ///     read must be accessible or the operation fails.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process with memory that is being read. The handle must have
            ///     <see cref="AccessRights.ProcessVmRead"/> access to the process.
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
            public static bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesRead) =>
                NativeMethods.ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, nSize, ref lpNumberOfBytesRead);

            /// <summary>
            ///     Reads data from an area of memory in a specified process. The entire area to be
            ///     read must be accessible or the operation fails.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process with memory that is being read. The handle must have
            ///     <see cref="AccessRights.ProcessVmRead"/> access to the process.
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
            public static bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, StringBuilder lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesRead) =>
                NativeMethods.ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, nSize, ref lpNumberOfBytesRead);

            /// <summary>
            ///     Updates the specified rectangle or region in a window's client area.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window to be redrawn. If this parameter is NULL, the desktop window
            ///     is updated.
            /// </param>
            /// <param name="lprcUpdate">
            ///     A pointer to a RECT structure containing the coordinates, in device units, of the
            ///     update rectangle. This parameter is ignored if the hrgnUpdate parameter identifies
            ///     a region.
            /// </param>
            /// <param name="hrgnUpdate">
            ///     A handle to the update region. If both the hrgnUpdate and lprcUpdate parameters are
            ///     NULL, the entire client area is added to the update region.
            /// </param>
            /// <param name="flags">
            ///     One or more redraw flags. This parameter can be used to invalidate or validate a
            ///     window, control repainting, and control which windows are affected by
            ///     <see cref="RedrawWindow"/>.
            /// </param>
            public static bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags) =>
                NativeMethods.RedrawWindow(hWnd, lprcUpdate, hrgnUpdate, flags);

            /// <summary>
            ///     Releases the mouse capture from a window in the current thread and restores normal mouse
            ///     input processing. A window that has captured the mouse receives all mouse input, regardless
            ///     of the position of the cursor, except when a mouse button is clicked while the cursor hot
            ///     spot is in the window of another thread.
            /// </summary>
            public static bool ReleaseCapture() =>
                NativeMethods.ReleaseCapture();

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
            ///     <see cref="ModifyMenuFlags.ByCommand"/> or <see cref="ModifyMenuFlags.ByPosition"/>.
            /// </param>
            public static bool RemoveMenu(IntPtr hMenu, uint uPosition, ModifyMenuFlags uFlags) =>
                NativeMethods.RemoveMenu(hMenu, uPosition, uFlags);

            /// <summary>
            ///     Removes the borders and title bar of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="menuBar">
            ///     true to remove an existing menu bar; otherwise, false.
            /// </param>
            /// <param name="extended">
            ///     true to remove an existing double border; otherwise, false.
            /// </param>
            public static void RemoveWindowBorders(IntPtr hWnd, bool menuBar = false, bool extended = false)
            {
                if (menuBar)
                {
                    var hMenu = NativeMethods.GetMenu(hWnd);
                    if (hMenu != IntPtr.Zero)
                    {
                        var count = NativeMethods.GetMenuItemCount(hMenu);
                        for (var i = 0; i < count; i++)
                            NativeMethods.RemoveMenu(hMenu, 0, ModifyMenuFlags.ByPosition | ModifyMenuFlags.Remove);
                        NativeMethods.DrawMenuBar(hWnd);
                    }
                }
                var style = NativeMethods.GetWindowLong(hWnd, WindowLongFlags.GwlStyle);
                var flags = new[]
                {
                    (int)WindowStyleFlags.SysMenu,
                    (int)WindowStyleFlags.Caption,
                    (int)WindowStyleFlags.Minimize,
                    (int)WindowStyleFlags.MaximizeBox,
                    (int)WindowStyleFlags.ThickFrame
                };
                foreach (var flag in flags)
                    if ((style & flag) == flag)
                        style &= ~flag;
                SetWindowLong(hWnd, WindowLongFlags.GwlStyle, (IntPtr)style);
                if (!extended)
                    return;
                style = NativeMethods.GetWindowLong(hWnd, WindowLongFlags.GwlExStyle) | (int)WindowStyleFlags.ExDlgModalFrame;
                SetWindowLong(hWnd, WindowLongFlags.GwlExStyle, (IntPtr)style);
            }

            /// <summary>
            ///     Removes specified window from taskbar.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            public static void RemoveWindowFromTaskbar(IntPtr hWnd)
            {
                NativeMethods.ShowWindow(hWnd, ShowWindowFlags.Hide);
                var style = NativeMethods.GetWindowLong(hWnd, WindowLongFlags.GwlExStyle) | (int)WindowStyleFlags.ExToolWindow;
                SetWindowLong(hWnd, WindowLongFlags.GwlExStyle, (IntPtr)style);
                NativeMethods.ShowWindow(hWnd, ShowWindowFlags.Show);
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
                var cds = new CopyData();
                try
                {
                    cds.cbData = (args.Length + 1) * 2;
                    cds.lpData = NativeMethods.LocalAlloc(LocalAllocFlags.LMemZeroInit, (UIntPtr)cds.cbData);
                    Marshal.Copy(args.ToCharArray(), 0, cds.lpData, args.Length);
                    cds.dwData = new IntPtr(1);
                    SendMessage(hWnd, WindowMenuFlags.WmCopyData, IntPtr.Zero, ref cds);
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
            ///     Synthesizes keystrokes, mouse motions, and button clicks.
            /// </summary>
            /// <param name="nInputs">
            ///     The number of structures in the pInputs array.
            /// </param>
            /// <param name="pInputs">
            ///     An array of <see cref="DeviceInput"/> structures. Each structure represents an event
            ///     to be inserted into the keyboard or mouse input stream.
            /// </param>
            /// <param name="cbSize">
            ///     The size, in bytes, of an <see cref="DeviceInput"/> structure. If cbSize is not the
            ///     size of an <see cref="DeviceInput"/> structure, the function fails.
            /// </param>
            public static uint SendInput(uint nInputs, [In] DeviceInput[] pInputs, int cbSize) =>
                NativeMethods.SendInput(nInputs, pInputs, cbSize);

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
            public static IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam) =>
                NativeMethods.SendMessage(hWnd, uMsg, wParam, lParam);

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
            public static IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, ref CopyData lParam) =>
                NativeMethods.SendMessage(hWnd, uMsg, wParam, ref lParam);

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
            public static IntPtr SendMessage(IntPtr hWnd, WindowMenuFlags uMsg, IntPtr wParam, ref CopyData lParam) =>
                NativeMethods.SendMessage(hWnd, (uint)uMsg, wParam, ref lParam);

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
            public static IntPtr SendMessage(IntPtr hWnd, WindowMenuFlags uMsg, IntPtr wParam, IntPtr lParam) =>
                NativeMethods.SendMessage(hWnd, (uint)uMsg, wParam, lParam);

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
            ///         SMTO_ABORTIFHUNG (0x2): The function returns without waiting for the time-out period to
            ///         elapse if the receiving thread appears to not respond or hangs.
            ///     </para>
            ///     <para>
            ///         SMTO_BLOCK (0x1): Prevents the calling thread from processing any other requests until
            ///         the function returns.
            ///     </para>
            ///     <para>
            ///         SMTO_NORMAL (0x0): The calling thread is not prevented from processing other requests while
            ///         waiting for the function to return.
            ///     </para>
            ///     <para>
            ///         SMTO_NOTIMEOUTIFNOTHUNG (0x8): The function does not enforce the time-out period as long as
            ///         the receiving thread is processing messages.
            ///     </para>
            ///     <para>
            ///         SMTO_ERRORONEXIT (0x20): The function should return 0 if the receiving window is destroyed
            ///         or its owning thread dies while the message is being processed.
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
            public static IntPtr SendMessageTimeout(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, out UIntPtr lpdwResult) =>
                NativeMethods.SendMessageTimeout(hWnd, msg, wParam, lParam, fuFlags, uTimeout, out lpdwResult);

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
            ///         SMTO_ABORTIFHUNG (0x2): The function returns without waiting for the time-out period to
            ///         elapse if the receiving thread appears to not respond or hangs.
            ///     </para>
            ///     <para>
            ///         SMTO_BLOCK (0x1): Prevents the calling thread from processing any other requests until the
            ///         function returns.
            ///     </para>
            ///     <para>
            ///         SMTO_NORMAL (0x0): The calling thread is not prevented from processing other requests while
            ///         waiting for the function to return.
            ///     </para>
            ///     <para>
            ///         SMTO_NOTIMEOUTIFNOTHUNG (0x8): The function does not enforce the time-out period as long as
            ///         the receiving thread is processing messages.
            ///     </para>
            ///     <para>
            ///         SMTO_ERRORONEXIT (0x20): The function should return 0 if the receiving window is destroyed or
            ///         its owning thread dies while the message is being processed.
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
            public static IntPtr SendMessageTimeoutText(IntPtr hWnd, uint msg, UIntPtr wParam, StringBuilder lParam, uint fuFlags, uint uTimeout, out IntPtr lpdwResult) =>
                NativeMethods.SendMessageTimeoutText(hWnd, msg, wParam, lParam, fuFlags, uTimeout, out lpdwResult);

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
            public static bool SendNotifyMessage(IntPtr hWnd, uint msg, UIntPtr wParam, string lParam) =>
                NativeMethods.SendNotifyMessage(hWnd, msg, wParam, lParam);

            /// <summary>
            ///     Changes the current directory for the current process.
            /// </summary>
            /// <param name="lpPathName">
            ///     The path to the new current directory.
            /// </param>
            public static bool SetCurrentDirectory(string lpPathName) =>
                NativeMethods.SetCurrentDirectory(lpPathName);

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
                NativeMethods.ClientToScreen(hWnd, ref point);
                NativeMethods.SetCursorPos((uint)point.X, (uint)point.Y);
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
            public static bool SetCursorPos(uint x, uint y) =>
                NativeMethods.SetCursorPos(x, y);

            /// <summary>
            ///     Brings the thread that created the specified window into the foreground and activates the window.
            ///     Keyboard input is directed to the window, and various visual cues are changed for the user. The
            ///     system assigns a slightly higher priority to the thread that created the foreground window than
            ///     it does to other threads.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window that should be activated and brought to the foreground.
            /// </param>
            public static bool SetForegroundWindow(IntPtr hWnd) =>
                NativeMethods.SetForegroundWindow(hWnd);

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
            public static IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent) =>
                NativeMethods.SetParent(hWndChild, hWndNewParent);

            /// <summary>
            ///     Sets the minimum and maximum working set sizes for the specified process.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process whose working set sizes is to be set.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.ProcessSetQuota"/> access right.
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
            public static bool SetProcessWorkingSetSize(IntPtr hProcess, UIntPtr dwMinimumWorkingSetSize, UIntPtr dwMaximumWorkingSetSize) =>
                NativeMethods.SetProcessWorkingSetSize(hProcess, dwMinimumWorkingSetSize, dwMaximumWorkingSetSize);

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
            public static UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIdEvent, uint uElapse, TimerProc lpTimerFunc) =>
                NativeMethods.SetTimer(hWnd, nIdEvent, uElapse, lpTimerFunc);

            /// <summary>
            ///     Changes the position and dimensions of the specified window to fill the entire screen.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="menuBar">
            ///     true to remove an existing menu bar; otherwise, false.
            /// </param>
            /// <param name="extended">
            ///     true to remove an existing double border; otherwise, false.
            /// </param>
            public static void SetWindowBorderlessFullscreen(IntPtr hWnd, bool menuBar = false, bool extended = false)
            {
                RemoveWindowBorders(hWnd, menuBar, extended);
                SetWindowFullscreen(hWnd);
            }

            /// <summary>
            ///     Changes the position and dimensions of the specified window to fill the entire screen.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            public static void SetWindowFullscreen(IntPtr hWnd) =>
                NativeMethods.MoveWindow(hWnd, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, true);

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
            public static IntPtr SetWindowLong(IntPtr hWnd, WindowLongFlags nIndex, IntPtr dwNewLong) =>
                NativeMethods.SetWindowLong(hWnd, (int)nIndex, dwNewLong);

            /// <summary>
            ///     Sets the show state and the restored, minimized, and maximized positions of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="lpwndpl">
            ///     A pointer to a <see cref="WindowPlacement"/> structure that specifies the new show state and window
            ///     positions.
            ///     <para>
            ///         Before calling SetWindowPlacement, set the length member of the <see cref="WindowPlacement"/>
            ///         structure to sizeof(<see cref="WindowPlacement"/>). SetWindowPlacement fails if the length
            ///         member is not set correctly.
            ///     </para>
            /// </param>
            public static bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement lpwndpl) =>
                NativeMethods.SetWindowPlacement(hWnd, ref lpwndpl);

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
                NativeMethods.GetWindowRect(hWnd, ref rect);
                NativeMethods.MoveWindow(hWnd, point.X, point.Y, rect.Width, rect.Height, false);
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
            ///         HWND_BOTTOM ((HWND)1): Places the window at the bottom of the Z order. If the hWnd parameter
            ///         identifies a topmost window, the window loses its topmost status and is placed at the bottom of
            ///         all other windows.
            ///     </para>
            ///     <para>
            ///         HWND_NOTOPMOST ((HWND)-2): Places the window above all non-topmost windows (that is, behind all
            ///         topmost windows). This flag has no effect if the window is already a non-topmost window.
            ///     </para>
            ///     <para>
            ///         HWND_TOP ((HWND)0): Places the window at the top of the Z order.
            ///     </para>
            ///     <para>
            ///         HWND_TOPMOST ((HWND)-1): Places the window above all non-topmost windows. The window maintains
            ///         its topmost position even when it is deactivated.
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
            public static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags) =>
                NativeMethods.SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags);

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
            public static IntPtr SetWindowsHookEx(Win32HookFlags idHook, HookProc lpfn, IntPtr hMod, int dwThreadId) =>
                NativeMethods.SetWindowsHookEx(idHook, lpfn, hMod, dwThreadId);

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
                NativeMethods.GetWindowRect(hWnd, ref rect);
                if (rect.Size != size)
                    NativeMethods.MoveWindow(hWnd, rect.X, rect.Y, size.Width, size.Height, true);
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
            public static bool SetWindowText(IntPtr hWnd, string lpString) =>
                NativeMethods.SetWindowText(hWnd, lpString);

            /// <summary>
            ///     Causes a window to use a different set of visual style information than its class normally uses.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose visual style information is to be changed.
            /// </param>
            /// <param name="pszSubAppName">
            ///     Pointer to a string that contains the application name to use in place of the calling application's
            ///     name. If this parameter is NULL, the calling application's name is used.
            /// </param>
            /// <param name="pszSubIdList">
            ///     Pointer to a string that contains a semicolon-separated list of CLSID names to use in place of the
            ///     actual list passed by the window's class. If this parameter is NULL, the ID list from the calling
            ///     class is used.
            /// </param>
            public static int SetWindowTheme(IntPtr hWnd, string pszSubAppName = null, string pszSubIdList = null) =>
                NativeMethods.SetWindowTheme(hWnd, pszSubAppName, pszSubIdList);

            /// <summary>
            ///     Sets attributes to control how visual styles are applied to a specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to a window to apply changes to.
            /// </param>
            /// <param name="pvAttribute">
            ///     A pointer that specifies attributes to set.
            /// </param>
            public static void SetWindowThemeAttribute([In] IntPtr hWnd, [In] ref WindowThemeAttributeOptions pvAttribute) =>
                NativeMethods.SetWindowThemeAttribute(hWnd, WindowThemeAttributeTypes.NonClient, ref pvAttribute, (uint)Marshal.SizeOf(typeof(WindowThemeAttributeOptions)));

            /// <summary>
            ///     Sends an appbar message to the system.
            /// </summary>
            /// <param name="dwMessage">
            ///     Appbar message value to send.
            /// </param>
            /// <param name="pData">
            ///     A pointer to an <see cref="AppBarData"/> structure. The content of the structure on entry and on exit
            ///     depends on the value set in the dwMessage parameter. See the individual message pages for specifics.
            /// </param>
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public static UIntPtr SHAppBarMessage(AppBarMessageOptions dwMessage, ref AppBarData pData) =>
                NativeMethods.SHAppBarMessage(dwMessage, ref pData);

            /// <summary>
            ///     Performs an operation on a specified file.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the parent window used for displaying a UI or error messages. This value can be NULL if the
            ///     operation is not associated with a window.
            /// </param>
            /// <param name="lpOperation">
            ///     A pointer to a null-terminated string, referred to in this case as a verb, that specifies the action to
            ///     be performed. The set of available verbs depends on the particular file or folder. Generally, the
            ///     actions available from an object's shortcut menu are available verbs.
            /// </param>
            /// <param name="lpFile">
            ///     A pointer to a null-terminated string that specifies the file or object on which to execute the specified
            ///     verb. To specify a Shell namespace object, pass the fully qualified parse name. Note that not all verbs
            ///     are supported on all objects. For example, not all document types support the "print" verb. If a relative
            ///     path is used for the lpDirectory parameter do not use a relative path for lpFile.
            /// </param>
            /// <param name="lpParameters">
            ///     If lpFile specifies an executable file, this parameter is a pointer to a null-terminated string that
            ///     specifies the parameters to be passed to the application. The format of this string is determined by the
            ///     verb that is to be invoked. If lpFile specifies a document file, lpParameters should be NULL.
            /// </param>
            /// <param name="lpDirectory">
            ///     A pointer to a null-terminated string that specifies the default (working) directory for the action. If
            ///     this value is NULL, the current working directory is used. If a relative path is provided at lpFile, do
            ///     not use a relative path for lpDirectory.
            /// </param>
            /// <param name="nShowCmd">
            ///     The flags that specify how an application is to be displayed when it is opened. If lpFile specifies a
            ///     document file, the flag is simply passed to the associated application. It is up to the application to
            ///     decide how to handle it.
            /// </param>
            public static IntPtr ShellExecute(IntPtr hWnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowWindowFlags nShowCmd) =>
                NativeMethods.ShellExecute(hWnd, lpOperation, lpFile, lpParameters, lpDirectory, nShowCmd);

            /// <summary>
            ///     The ShowScrollBar function shows or hides the specified scroll bar.
            /// </summary>
            /// <param name="hWnd">
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
            public static bool ShowScrollBar(IntPtr hWnd, ShowScrollBarOptions wBar, bool bShow) =>
                NativeMethods.ShowScrollBar(hWnd, wBar, bShow);

            /// <summary>
            ///     Activates the window and displays it.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            public static void ShowWindow(IntPtr hWnd)
            {
                NativeMethods.ShowWindow(hWnd, ShowWindowFlags.Restore);
                NativeMethods.ShowWindow(hWnd, ShowWindowFlags.Show);
            }

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
            public static bool ShowWindow(IntPtr hWnd, ShowWindowFlags nCmdShow) =>
                NativeMethods.ShowWindow(hWnd, nCmdShow);

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
            public static bool ShowWindowAsync(IntPtr hWnd, ShowWindowFlags nCmdShow) =>
                NativeMethods.ShowWindowAsync(hWnd, nCmdShow);

            /// <summary>
            ///     Terminates the specified process and all of its threads.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process to be terminated.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.ProcessTerminate"/> access right.
            ///     </para>
            /// </param>
            /// <param name="uExitCode">
            ///     The exit code to be used by the process and threads terminated as a result of this call.
            ///     Use the GetExitCodeProcess function to retrieve a process's exit value. Use the
            ///     GetExitCodeThread function to retrieve a thread's exit value.
            /// </param>
            public static bool TerminateProcess(IntPtr hProcess, uint uExitCode) =>
                NativeMethods.TerminateProcess(hProcess, uExitCode);

            /// <summary>
            ///     Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
            /// </summary>
            /// <param name="hhk">
            ///     A handle to the hook to be removed. This parameter is a hook handle obtained by a previous call
            ///     SetWindowsHookEx.
            /// </param>
            public static int UnhookWindowsHookEx(IntPtr hhk) =>
                NativeMethods.UnhookWindowsHookEx(hhk);

            /// <summary>
            ///     The UpdateWindow function updates the client area of the specified window by sending a WM_PAINT
            ///     message to the window if the window's update region is not empty. The function sends a WM_PAINT
            ///     message directly to the window procedure of the specified window, bypassing the application
            ///     queue. If the update region is empty, no message is sent.
            /// </summary>
            /// <param name="hWnd">
            ///     Handle to the window to be updated.
            /// </param>
            public static bool UpdateWindow(IntPtr hWnd) =>
                NativeMethods.UpdateWindow(hWnd);

            /// <summary>
            ///     Reserves, commits, or changes the state of a region of memory within the virtual address space
            ///     of a specified process. The function initializes the memory it allocates to zero.
            /// </summary>
            /// <param name="hProcess">
            ///     The handle to a process. The function allocates memory within the virtual address
            ///     space of this process.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.ProcessVmOperation"/> access
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
            ///     The memory protection for the region of pages to be allocated.
            /// </param>
            public static IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, MemAllocTypes flAllocationType, MemProtectFlags flProtect) =>
                NativeMethods.VirtualAllocEx(hProcess, lpAddress, dwSize, flAllocationType, flProtect);

            /// <summary>
            ///     Releases, decommits, or releases and decommits a region of memory within the virtual address
            ///     space of a specified process.
            /// </summary>
            /// <param name="hProcess">
            ///     The handle to a process. The function allocates memory within the virtual address space of this
            ///     process.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.ProcessVmOperation"/> access
            ///         right.
            ///     </para>
            /// </param>
            /// <param name="lpAddress">
            ///     A pointer to the starting address of the region of memory to be freed.
            ///     <para>
            ///         If the dwFreeType parameter is <see cref="MemAllocTypes.Release"/>, lpAddress must be
            ///         the base address returned by the VirtualAllocEx function when the region is reserved.
            ///     </para>
            /// </param>
            /// <param name="dwSize">
            ///     The size of the region of memory to free, in bytes.
            ///     <para>
            ///         If the dwFreeType parameter is <see cref="MemAllocTypes.Release"/>, dwSize must
            ///         be 0 (zero). The function frees the entire region that is reserved in the initial
            ///         allocation call to VirtualAllocEx.
            ///     </para>
            ///     <para>
            ///         If dwFreeType is <see cref="MemAllocTypes.Decommit"/>, the function decommits all memory
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
            public static bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, MemFreeTypes dwFreeType) =>
                NativeMethods.VirtualFreeEx(hProcess, lpAddress, dwSize, dwFreeType);

            /// <summary>
            ///     Determines whether the specified window is outside the visible screen area.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="vRect">
            ///     A pointer to a <see cref="Rectangle"/> structure that receives the best screen coordinates of the
            ///     upper-left and lower-right corners of the window.
            /// </param>
            public static bool WindowIsOutOfScreenArea(IntPtr hWnd, out Rectangle vRect)
            {
                vRect = new Rectangle();
                if (!NativeMethods.GetWindowRect(hWnd, ref vRect) || vRect.Width < 1 || vRect.Height < 1)
                    return false;
                vRect.Width = vRect.Width - vRect.X;
                vRect.Height = vRect.Height - vRect.Y;
                var cRect = vRect;
                var range = new Rectangle
                {
                    X = SystemInformation.VirtualScreen.X,
                    Y = SystemInformation.VirtualScreen.Y,
                    Width = SystemInformation.VirtualScreen.Width - cRect.Width,
                    Height = SystemInformation.VirtualScreen.Height - cRect.Height
                };
                for (var i = 0; i < 2; i++)
                {
                    if (i == 1)
                    {
                        var screen = Screen.PrimaryScreen;
                        range = new Rectangle
                        {
                            X = vRect.X + vRect.Width / 2,
                            Y = vRect.Y + vRect.Height / 2,
                            Width = vRect.Width / 2,
                            Height = vRect.Height / 2
                        };
                        foreach (var scr in Screen.AllScreens)
                        {
                            if (!scr.Bounds.Contains(range))
                                continue;
                            screen = scr;
                        }
                        range = new Rectangle
                        {
                            X = screen.WorkingArea.X,
                            Y = screen.WorkingArea.Y,
                            Width = screen.WorkingArea.Width + screen.WorkingArea.X - cRect.Width,
                            Height = screen.WorkingArea.Height + screen.WorkingArea.Y - cRect.Height
                        };
                    }
                    vRect.X = Math.Max(vRect.X, range.X);
                    vRect.X = Math.Min(vRect.X, range.Width);
                    vRect.Y = Math.Max(vRect.Y, range.Y);
                    vRect.Y = Math.Min(vRect.Y, range.Height);
                }
                return cRect != vRect;
            }

            /// <summary>
            ///     Determines whether the specified window is outside the visible screen area.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            public static bool WindowIsOutOfVisibleScreenArea(IntPtr hWnd) =>
                WindowIsOutOfScreenArea(hWnd, out Rectangle _);

            /// <summary>
            ///     Writes data to an area of memory in a specified process. The entire area to be written to must be
            ///     accessible or the operation fails.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process memory to be modified. The handle must have
            ///     <see cref="AccessRights.ProcessVmWrite"/> and
            ///     <see cref="AccessRights.ProcessVmOperation"/> access to the process.
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
            public static bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten) =>
                NativeMethods.WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, nSize, out lpNumberOfBytesWritten);
        }

        /// <summary>
        ///     Represents native methods.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        internal static class NativeMethods
        {
            /// <summary>
            ///     Enables or disables privileges in the specified access token. Enabling or disabling privileges in
            ///     an access token requires <see cref="AccessTokenFlags.TokenAdjustPrivileges"/> access.
            /// </summary>
            /// <param name="tokenHandle">
            ///     A handle to the access token that contains the privileges to be modified. The handle must have
            ///     <see cref="AccessTokenFlags.TokenAdjustPrivileges"/> access to the token. If the PreviousState
            ///     parameter is not NULL, the handle must also have <see cref="AccessTokenFlags.TokenQuery"/> access.
            /// </param>
            /// <param name="disableAllPrivileges">
            ///     Specifies whether the function disables all of the token's privileges. If this value is TRUE, the
            ///     function disables all privileges and ignores the NewState parameter. If it is FALSE, the function
            ///     modifies privileges based on the information pointed to by the NewState parameter.
            /// </param>
            /// <param name="newState">
            ///     A pointer to a <see cref="TokenPrivileges"/> structure that specifies an array of privileges and their
            ///     attributes. If the disableAllPrivileges parameter is FALSE, the AdjustTokenPrivileges function
            ///     enables, disables, or removes these privileges for the token. The following table describes the
            ///     action taken by the AdjustTokenPrivileges function, based on the privilege attribute. If
            ///     disableAllPrivileges is TRUE, the function ignores this parameter.
            /// </param>
            /// <param name="bufferLength">
            ///     Specifies the size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter
            ///     can be zero if the PreviousState parameter is NULL.
            /// </param>
            /// <param name="previousState">
            ///     A pointer to a buffer that the function fills with a <see cref="TokenPrivileges"/> structure that
            ///     contains the previous state of any privileges that the function modifies. That is, if a privilege
            ///     has been modified by this function, the privilege and its previous state are contained in the
            ///     <see cref="TokenPrivileges"/> structure referenced by PreviousState. If the PrivilegeCount member
            ///     of <see cref="TokenPrivileges"/> is zero, then no privileges have been changed by this function.
            ///     This parameter can be NULL. If you specify a buffer that is too small to receive the complete list
            ///     of modified privileges, the function fails and does not adjust any privileges. In this case, the
            ///     function sets the variable pointed to by the returnLength parameter to the number of bytes required
            ///     to hold the complete list of modified privileges.
            /// </param>
            /// <param name="returnLength">
            ///     A pointer to a variable that receives the required size, in bytes, of the buffer pointed to by the
            ///     previousState parameter. This parameter can be NULL if previousState is NULL.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true)]
            internal static extern bool AdjustTokenPrivileges(IntPtr tokenHandle, bool disableAllPrivileges, ref TokenPrivileges newState, uint bufferLength, IntPtr previousState, IntPtr returnLength);

            /// <summary>
            ///     Allocates a new console for the calling process.
            /// </summary>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, EntryPoint = "AllocConsole", SetLastError = true)]
            internal static extern int AllocConsole();

            /// <summary>
            ///     Enables you to produce special effects when showing or hiding windows. There are four types of
            ///     animation: roll, slide, collapse or expand, and alpha-blended fade.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window to animate. The calling thread must own this window.
            /// </param>
            /// <param name="time">
            ///     The time it takes to play the animation, in milliseconds. Typically, an animation takes 200
            ///     milliseconds to play.
            /// </param>
            /// <param name="flags">
            ///     The type of animation.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            internal static extern bool AnimateWindow(IntPtr hWnd, int time, AnimateWindowFlags flags);

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
            internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

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
            internal static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

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
            internal static extern bool CloseHandle(IntPtr handle);

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
            internal static extern int ControlService(IntPtr hService, ServiceControlOptions dwControl, ServiceStatus lpServiceStatus);

            /// <summary>
            ///     Creates a new process and its primary thread. The new process runs in the security context
            ///     of the specified token. It can optionally load the user profile for the specified user.
            /// </summary>
            /// <param name="hToken">
            ///     A handle to the primary token that represents a user. The handle must have the
            ///     <see cref="AccessTokenFlags.TokenQuery"/>, <see cref="AccessTokenFlags.TokenDuplicate"/>,
            ///     and <see cref="AccessTokenFlags.TokenAssignPrimary"/> access rights. The user represented
            ///     by the token must have read and execute access to the application specified by the
            ///     lpApplicationName or the lpCommandLine parameter.
            /// </param>
            /// <param name="dwLogonFlags">
            ///     The logon option.
            /// </param>
            /// <param name="lpApplicationName">
            ///     The name of the module to be executed. This module can be a Windows-based application. It
            ///     can be some other type of module (for example, MS-DOS or OS/2) if the appropriate subsystem
            ///     is available on the local computer.
            ///     <para>
            ///         The string can specify the full path and file name of the module to execute or it can
            ///         specify a partial name. In the case of a partial name, the function uses the current
            ///         drive and current directory to complete the specification. The function will not use
            ///         the search path. This parameter must include the file name extension; no default
            ///         extension is assumed.
            ///     </para>
            ///     <para>
            ///         The lpApplicationName parameter can be NULL. In that case, the module name must be the
            ///         first white space–delimited token in the lpCommandLine string. If you are using a long
            ///         file name that contains a space, use quoted strings to indicate where the file name ends
            ///         and the arguments begin; otherwise, the file name is ambiguous.
            ///     </para>
            /// </param>
            /// <param name="lpCommandLine">
            ///     The command line to be executed. The maximum length of this string is 1024 characters. If
            ///     lpApplicationName is NULL, the module name portion of lpCommandLine is limited to 255
            ///     characters.
            ///     <para>
            ///         The lpCommandLine parameter can be NULL. In that case, the function uses the string
            ///         pointed to by lpApplicationName as the command line.
            ///     </para>
            /// </param>
            /// <param name="dwCreationFlags">
            ///     The flags that control how the process is created.
            /// </param>
            /// <param name="lpEnvironment">
            ///     A pointer to an environment block for the new process. If this parameter is NULL, the new
            ///     process uses an environment created from the profile of the user specified by lpUsername.
            /// </param>
            /// <param name="lpCurrentDirectory">
            ///     The full path to the current directory for the process. The string can also specify a UNC path.
            ///     <para>
            ///         If this parameter is NULL, the new process will have the same current drive and directory
            ///         as the calling process. (This feature is provided primarily for shells that need to start
            ///         an application and specify its initial drive and working directory.)
            ///     </para>
            /// </param>
            /// <param name="lpStartupInfo">
            ///     A pointer to a <see cref="StartupInfo"/> structure.
            ///     <para>
            ///         If the lpDesktop member is NULL or an empty string, the new process inherits the desktop
            ///         and window station of its parent process. The function adds permission for the specified
            ///         user account to the inherited window station and desktop. Otherwise, if this member
            ///         specifies a desktop, it is the responsibility of the application to add permission for
            ///         the specified user account to the specified window station and desktop, even for
            ///         WinSta0\Default.
            ///     </para>
            /// </param>
            /// <param name="lpProcessInformation">
            ///     A pointer to a <see cref="ProcessInformation"/> structure that receives identification
            ///     information for the new process, including a handle to the process.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern bool CreateProcessWithTokenW(IntPtr hToken, LogonOptions dwLogonFlags, string lpApplicationName, string lpCommandLine, CreationFlags dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref StartupInfo lpStartupInfo, out ProcessInformation lpProcessInformation);

            /// <summary>
            ///     Creates a service object and adds it to the specified service control manager database.
            /// </summary>
            /// <param name="hScManager">
            ///     A handle to the service control manager database. This handle is returned by the OpenSCManager
            ///     function and must have the <see cref="ServiceManagerAccessRights.CreateService"/>
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
            ///         <see cref="ServiceBootFlags.BootStart"/> or
            ///         <see cref="ServiceBootFlags.SystemStart"/> start types.
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
            /// <param name="lpServiceStartName">
            ///     The name of the account under which the service should run. If the service type is
            ///     <see cref="ServiceTypes.Win32OwnProcess"/>, use an account name in the form
            ///     DomainName\UserName. The service process will be logged on as this user. If the account
            ///     belongs to the built-in domain, you can specify .\UserName.
            ///     <para>
            ///         If this parameter is NULL, CreateService uses the LocalSystem account. If the service
            ///         type specifies <see cref="ServiceTypes.InteractiveProcess"/>, the service
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
            ///         If the service type is <see cref="ServiceTypes.KernelDriver"/> or
            ///         <see cref="ServiceTypes.FileSystemDriver"/>, the name is the driver
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
            internal static extern IntPtr CreateService(IntPtr hScManager, [MarshalAs(UnmanagedType.LPStr)] string lpServiceName, [MarshalAs(UnmanagedType.LPStr)] string lpDisplayName, ServiceAccessRights dwDesiredAccess, ServiceTypes dwServiceType, ServiceBootFlags dwStartType, ServiceErrors dwErrorControl, [MarshalAs(UnmanagedType.LPStr)] string lpBinaryPathName, [MarshalAs(UnmanagedType.LPStr)] string lpLoadOrderGroup, IntPtr lpdwTagId, [MarshalAs(UnmanagedType.LPStr)] string lpDependencies, [MarshalAs(UnmanagedType.LPStr)] string lpServiceStartName, [MarshalAs(UnmanagedType.LPStr)] string lpPassword);

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
            ///     <see cref="ModifyMenuFlags.ByCommand"/> or <see cref="ModifyMenuFlags.ByPosition"/>.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            internal static extern int DeleteMenu(IntPtr hMenu, uint nPosition, ModifyMenuFlags wFlags);

            /// <summary>
            ///     Marks the specified service for deletion from the service control manager database.
            /// </summary>
            /// <param name="hService">
            ///     A handle to the service. This handle is returned by the OpenService or CreateService function,
            ///     and it must have the <see cref="AccessRights.Delete"/> access right.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true, CharSet = CharSet.Ansi)]
            internal static extern int DeleteService(IntPtr hService);

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
            internal static extern bool DestroyIcon(IntPtr hIcon);

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
            internal static extern bool DrawMenuBar(IntPtr hWnd);

            /// <summary>
            ///     Duplicates an object handle.
            /// </summary>
            /// <param name="hSourceProcessHandle">
            ///     A handle to the process with the handle to be duplicated.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.ProcessDupHandle"/> access right.
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
            ///         The handle must have the <see cref="AccessRights.ProcessDupHandle"/> access right.
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
            ///         <see cref="DuplicateOptions.SameAccess"/> flag. Otherwise, the flags that
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
            ///     <see cref="DuplicateOptions"/>.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

            /// <summary>
            ///     Creates a new access token that duplicates one already in existence.
            /// </summary>
            /// <param name="hExistingToken">
            ///     A handle to an access token opened with <see cref="AccessTokenFlags.TokenDuplicate"/> access.
            /// </param>
            /// <param name="dwDesiredAccess">
            ///     Specifies the requested access rights for the new token.
            /// </param>
            /// <param name="lpTokenAttributes">
            ///     A pointer to a <see cref="SecurityAttributes"/> structure that specifies a security descriptor
            ///     for the new token and determines whether child processes can inherit the token. If
            ///     lpTokenAttributes is NULL, the token gets a default security descriptor and the handle cannot
            ///     be inherited. If the security descriptor contains a system access control list (SACL), the token
            ///     gets system security access rights, even if it was not requested in dwDesiredAccess.
            /// </param>
            /// <param name="impersonationLevel">
            ///     Specifies a value from the <see cref="SecurityImpersonationLevels"/> enumeration that indicates
            ///     the impersonation level of the new token.
            /// </param>
            /// <param name="tokenType">
            ///     Specifies one of the following values from the <see cref="TokenType"/> enumeration.
            /// </param>
            /// <param name="phNewToken">
            ///     A pointer to a HANDLE variable that receives the new token.
            ///     <para>
            ///         When you have finished using the new token, call the <see cref="CloseHandle"/> function
            ///         to close the token handle.
            ///     </para>
            /// </param>
            /// <returns>
            ///     If the function succeeds, the function returns a nonzero value.
            /// </returns>
            [DllImport(DllNames.Advapi32, CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern bool DuplicateTokenEx(IntPtr hExistingToken, AccessTokenFlags dwDesiredAccess, ref SecurityAttributes lpTokenAttributes, SecurityImpersonationLevels impersonationLevel, TokenTypes tokenType, out IntPtr phNewToken);

            /// <summary>
            ///     Creates a new access token that duplicates one already in existence.
            /// </summary>
            /// <param name="hExistingToken">
            ///     A handle to an access token opened with <see cref="AccessTokenFlags.TokenDuplicate"/> access.
            /// </param>
            /// <param name="dwDesiredAccess">
            ///     Specifies the requested access rights for the new token.
            /// </param>
            /// <param name="lpTokenAttributes">
            ///     A pointer to a <see cref="SecurityAttributes"/> structure that specifies a security descriptor
            ///     for the new token and determines whether child processes can inherit the token. If
            ///     lpTokenAttributes is NULL, the token gets a default security descriptor and the handle cannot
            ///     be inherited. If the security descriptor contains a system access control list (SACL), the token
            ///     gets system security access rights, even if it was not requested in dwDesiredAccess.
            /// </param>
            /// <param name="impersonationLevel">
            ///     Specifies a value from the <see cref="SecurityImpersonationLevels"/> enumeration that indicates
            ///     the impersonation level of the new token.
            /// </param>
            /// <param name="tokenType">
            ///     Specifies one of the following values from the <see cref="TokenType"/> enumeration.
            /// </param>
            /// <param name="phNewToken">
            ///     A pointer to a HANDLE variable that receives the new token.
            ///     <para>
            ///         When you have finished using the new token, call the <see cref="CloseHandle"/> function
            ///         to close the token handle.
            ///     </para>
            /// </param>
            /// <returns>
            ///     If the function succeeds, the function returns a nonzero value.
            /// </returns>
            [DllImport(DllNames.Advapi32, CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern bool DuplicateTokenEx(IntPtr hExistingToken, uint dwDesiredAccess, IntPtr lpTokenAttributes, SecurityImpersonationLevels impersonationLevel, TokenTypes tokenType, out IntPtr phNewToken);

            /// <summary>
            ///     Extends the window frame into the client area.
            /// </summary>
            /// <param name="hWnd">
            ///     The handle to the window in which the frame will be extended into the client area.
            /// </param>
            /// <param name="pMarInset">
            ///     A pointer to a MARGINS structure that describes the margins to use when extending the frame
            ///     into the client area.
            /// </param>
            /// <returns>
            ///     If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
            /// </returns>
            [DllImport(DllNames.Dwmapi, SetLastError = true)]
            internal static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref ThemeMargins pMarInset);

            /// <summary>
            ///     ***This is an undocumented API and as such is not supported by Microsoft and can be changed
            ///     or removed in the future without futher notice.
            /// </summary>
            [DllImport(DllNames.Dwmapi, EntryPoint = "#127", PreserveSig = false, SetLastError = true)]
            internal static extern void DwmGetColorizationParameters(out DwmColorizationParams parameters);

            /// <summary>
            ///     Obtains a value that indicates whether Desktop Window Manager (DWM) composition is enabled.
            /// </summary>
            /// <param name="pfEnabled">
            ///     A pointer to a value that, when this function returns successfully, receives TRUE if DWM
            ///     composition is enabled; otherwise, FALSE.
            /// </param>
            /// <returns>
            ///     If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
            /// </returns>
            [DllImport(DllNames.Dwmapi, SetLastError = true)]
            internal static extern int DwmIsCompositionEnabled(ref int pfEnabled);

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
            internal static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

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
            internal static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);

            /// <summary>
            ///     Enumerates all nonchild windows associated with a thread by passing the handle to each window,
            ///     in turn, to an application-defined callback function.
            /// </summary>
            /// <param name="dwThreadId">
            ///     The identifier of the thread whose windows are to be enumerated.
            /// </param>
            /// <param name="lpfn">
            ///     A pointer to an application-defined callback function.
            /// </param>
            /// <param name="lParam">
            ///     An application-defined value to be passed to the callback function.
            /// </param>
            /// <returns>
            ///     If the callback function returns TRUE for all windows in the thread specified by dwThreadId, the
            ///     return value is TRUE. If the callback function returns FALSE on any enumerated window, or if
            ///     there are no windows found in the thread specified by dwThreadId, the return value is FALSE.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            internal static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadWndProc lpfn, IntPtr lParam);

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
            internal static extern int ExtractIconEx([MarshalAs(UnmanagedType.LPStr)] string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, int nIcons);

            /// <summary>
            ///     Determines the MIME type from the data provided.
            /// </summary>
            /// <param name="pBc">
            ///     A pointer to the IBindCtx interface. Can be set to NULL.
            /// </param>
            /// <param name="pwzUrl">
            ///     A pointer to a string value that contains the URL of the data. Can be set to NULL if pBuffer
            ///     contains the data to be sniffed.
            /// </param>
            /// <param name="pBuffer">
            ///     A pointer to the buffer that contains the data to be sniffed. Can be set to NULL if pwzUrl
            ///     contains a valid URL.
            /// </param>
            /// <param name="cbSize">
            ///     An unsigned long integer value that contains the size of the buffer.
            /// </param>
            /// <param name="pwzMimeProposed">
            ///     A pointer to a string value that contains the proposed MIME type. This value is authoritative if
            ///     type cannot be determined from the data. If the proposed type contains a semi-colon (;) it is
            ///     removed. This parameter can be set to NULL.
            /// </param>
            /// <param name="dwMimeFlags">
            ///     The search and filter options.
            /// </param>
            /// <param name="ppwzMimeOut">
            ///     The address of a string value that receives the suggested MIME type.
            /// </param>
            /// <param name="dwReserved">
            ///     Reserved. Must be set to 0.
            /// </param>
            /// <returns>
            ///     This function can return one of these values.
            ///     <para>
            ///         S_OK: The operation completed successfully.
            ///     </para>
            ///     <para>
            ///         E_FAIL: The operation failed.
            ///     </para>
            ///     <para>
            ///         E_INVALIDARG: One or more arguments are invalid.
            ///     </para>
            ///     <para>
            ///         E_OUTOFMEMORY: There is insufficient memory to complete the operation.
            ///     </para>
            /// </returns>
            [DllImport(DllNames.Urlmon, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
            internal static extern int FindMimeFromData(IntPtr pBc, [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)] byte[] pBuffer, int cbSize, [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed, MimeFlags dwMimeFlags, out IntPtr ppwzMimeOut, int dwReserved);

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
            [DllImport(DllNames.User32, EntryPoint = "FindWindowA", CallingConvention = CallingConvention.StdCall, BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPStr)] string lpClassName, [MarshalAs(UnmanagedType.LPStr)] string lpWindowName);

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
            internal static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

            /// <summary>
            ///     Retrieves a handle to a window whose class name and window name match the specified strings. The
            ///     function searches child windows, beginning with the one following the specified child window. This
            ///     function does not perform a case-sensitive search.
            /// </summary>
            /// <param name="hWndParent">
            ///     A handle to the parent window whose child windows are to be searched.
            ///     <para>
            ///         If hWndParent is NULL, the function uses the desktop window as the parent window. The function
            ///         searches among windows that are child windows of the desktop.
            ///     </para>
            ///     <para>
            ///         If hWndParent is HWND_MESSAGE, the function searches all message-only windows.
            ///     </para>
            /// </param>
            /// <param name="hWndChildAfter">
            ///     A handle to a child window. The search begins with the next child window in the Z order. The child
            ///     window must be a direct child window of hWndParent, not just a descendant window.
            ///     <para>
            ///         If hWndChildAfter is NULL, the search begins with the first child window of hWndParent.
            ///     </para>
            ///     <para>
            ///         Note that if both hWndParent and hWndChildAfter are NULL, the function searches all top-level
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
            [DllImport(DllNames.User32, EntryPoint = "FindWindowExA", CallingConvention = CallingConvention.StdCall, BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, [MarshalAs(UnmanagedType.LPStr)] string lpszClass, [MarshalAs(UnmanagedType.LPStr)] string lpszWindow);

            /// <summary>
            ///     Determines whether a key is up or down at the time the function is called, and whether the
            ///     key was pressed after a previous call to <see cref="GetAsyncKeyState"/>.
            /// </summary>
            /// <param name="vKey">
            ///     The virtual-key code.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value specifies whether the key was pressed since the
            ///     last call to <see cref="GetAsyncKeyState"/>, and whether the key is currently up or down.
            ///     If the most significant bit is set, the key is down, and if the least significant bit is
            ///     set, the key was pressed after the previous call to <see cref="GetAsyncKeyState"/>.
            ///     <para>
            ///         The return value is zero if the current desktop is not the active desktop, or if the
            ///         foreground thread belongs to another process and the desktop does not allow the hook or
            ///         the journal record.
            ///     </para>
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern short GetAsyncKeyState(int vKey);

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
            internal static extern int GetClassName(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpClassName, int nMaxCount);

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
            internal static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

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
            ///     Retrieves a pseudo handle for the current process.
            /// </summary>
            /// <returns>
            ///     The return value is a pseudo handle to the current process.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            internal static extern IntPtr GetCurrentProcess();

            /// <summary>
            ///     Retrieves the thread identifier of the calling thread.
            /// </summary>
            /// <returns>
            ///     The return value is the thread identifier of the calling thread.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            internal static extern uint GetCurrentThreadId();

            /// <summary>
            ///     Retrieves the position of the mouse cursor, in screen coordinates.
            /// </summary>
            /// <returns>
            ///     Returns nonzero if successful or zero otherwise.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetCursorPos(out Point lpPoint);

            /// <summary>
            ///     Retrieves the identifier of the specified control.
            /// </summary>
            /// <param name="hWndCtl">
            ///     A handle to the control.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the identifier of the control.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern int GetDlgCtrlID(IntPtr hWndCtl);

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
            internal static extern IntPtr GetDlgItem(IntPtr hDlg, int nIddlgItem);

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
            internal static extern IntPtr GetForegroundWindow();

            /// <summary>
            ///     Retrieves the calling thread's last-error code value. The last-error code is
            ///     maintained on a per-thread basis. Multiple threads do not overwrite each
            ///     other's last-error code.
            /// </summary>
            /// <returns>
            ///     The return value is the calling thread's last-error code.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            internal static extern int GetLastError();

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
            internal static extern IntPtr GetMenu(IntPtr hWnd);

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
            internal static extern int GetMenuItemCount(IntPtr hMenu);

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
            internal static extern IntPtr GetParent(IntPtr hWnd);

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
            ///     Retrieves the process identifier of the specified process.
            /// </summary>
            /// <param name="handle">
            ///     A handle to the process.
            /// </param>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            internal static extern uint GetProcessId(IntPtr handle);

            /// <summary>
            ///     Retrieves the name of the executable file for the specified process.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process. The handle must have the
            ///     <see cref="AccessRights.ProcessQueryInformation"/> or
            ///     <see cref="AccessRights.ProcessQueryLimitedInformation"/> access right.
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
            internal static extern bool GetProcessImageFileName(IntPtr hProcess, StringBuilder lpImageFileName, int nSize);

            /// <summary>
            ///     Retrieves a handle to the Shell's desktop window.
            /// </summary>
            /// <returns>
            ///     The return value is the handle of the Shell's desktop window. If no Shell
            ///     process is present, the return value is NULL.
            /// </returns>
            [DllImport(DllNames.User32)]
            internal static extern IntPtr GetShellWindow();

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
            internal static extern IntPtr GetStdHandle(int nStdHandle);

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
            internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

            /// <summary>
            ///     Returns the language identifier for the user UI language for the current user. If the current user
            ///     has not set a language, <see cref="GetUserDefaultUILanguage"/> returns the preferred language set
            ///     for the system. If there is no preferred language set for the system, then the system default UI
            ///     language (also known as "install language") is returned.
            /// </summary>
            /// <returns>
            ///     Returns the language identifier for the user UI language for the current user.
            /// </returns>
            [DllImport(DllNames.Kernel32, CharSet = CharSet.Auto)]
            internal static extern ushort GetUserDefaultUILanguage();

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
            ///     value, specify one of the <see cref="WindowLongFlags"/>.GWL_??? values.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the requested value.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            internal static extern int GetWindowLong(IntPtr hWnd, WindowLongFlags nIndex);

            /// <summary>
            ///     Gets the show state and the restored, minimized, and maximized positions of the specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window.
            /// </param>
            /// <param name="lpwndpl">
            ///     A pointer to a <see cref="WindowPlacement"/> structure that specifies the new show state and window
            ///     positions.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);

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
            internal static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

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
            internal static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

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
            internal static extern int GetWindowTextLength(IntPtr hWnd);

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
            internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

            /// <summary>
            ///     Removes the caret from the screen. Hiding a caret does not destroy its current shape or invalidate the
            ///     insertion point.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window that owns the caret. If this parameter is NULL, HideCaret searches the current
            ///     task for the window that owns the caret.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool HideCaret(IntPtr hWnd);

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
            ///     The identifier of the new menu item or, if the uFlags parameter has the <see cref="ModifyMenuFlags.Popup"/>
            ///     flag set, a handle to the drop-down menu or submenu.
            /// </param>
            /// <param name="lpNewItem">
            ///     The content of the new menu item. The interpretation of lpNewItem depends on whether the uFlags parameter
            ///     includes the <see cref="ModifyMenuFlags.Bitmap"/>, <see cref="ModifyMenuFlags.OwnerDraw"/>, or
            ///     <see cref="ModifyMenuFlags.String"/> flag, as follows.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            internal static extern bool InsertMenu(IntPtr hMenu, uint wPosition, ModifyMenuFlags wFlags, UIntPtr wIdNewItem, [MarshalAs(UnmanagedType.LPStr)] string lpNewItem);

            /// <summary>
            ///     Adds a rectangle to the specified window's update region. The update region represents the portion of the
            ///     window's client area that must be redrawn.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose update region has changed. If this parameter is NULL, the system invalidates
            ///     and redraws all windows, not just the windows for this application, and sends the WM_ERASEBKGND and
            ///     WM_NCPAINT messages before the function returns. Setting this parameter to NULL is not recommended.
            /// </param>
            /// <param name="lpRect">
            ///     A pointer to a RECT structure that contains the client coordinates of the rectangle to be added to the update
            ///     region. If this parameter is NULL, the entire client area is added to the update region.
            /// </param>
            /// <param name="bErase">
            ///     Specifies whether the background within the update region is to be erased when the update region is processed.
            ///     If this parameter is TRUE, the background is erased when the BeginPaint function is called. If this parameter
            ///     is FALSE, the background remains unchanged.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            internal static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

            /// <summary>
            ///     Tests if a visual style for the current application is active.
            /// </summary>
            /// <returns>
            ///     Returns one of the following values.
            ///     <para>
            ///         TRUE: A visual style is enabled, and windows with visual styles applied should call OpenThemeData to start
            ///         using theme drawing services.
            ///     </para>
            ///     <para>
            ///         FALSE: A visual style is not enabled, and the window message handler does not need to make another call to
            ///         <see cref="IsThemeActive"/> until it receives a WM_THEMECHANGED message.
            ///     </para>
            /// </returns>
            [DllImport(DllNames.Uxtheme, ExactSpelling = true, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool IsThemeActive();

            /// <summary>
            ///     Loads the specified module into the address space of the calling process. The specified module may cause other
            ///     modules to be loaded.
            /// </summary>
            /// <param name="lpFileName">
            ///     The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file).
            ///     The name specified is the file name of the module and is not related to the name stored in the library module
            ///     itself, as specified by the LIBRARY keyword in the module-definition (.def) file.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the module.
            /// </returns>
            [DllImport(DllNames.Kernel32, BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

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
            internal static extern int LoadString(IntPtr hInstance, uint uId, StringBuilder lpBuffer, int nBufferMax);

            /// <summary>
            ///     Allocates the specified number of bytes from the heap.
            /// </summary>
            /// <param name="flag">
            ///     The memory allocation attributes. The default is the LMEM_FIXED value. This parameter can be one or more
            ///     of the <see cref="LocalAllocFlags"/>.
            /// </param>
            /// <param name="size">
            ///     The number of bytes to allocate. If this parameter is zero and the uFlags parameter specifies
            ///     <see cref="LocalAllocFlags.LMemMoveable"/>, the function returns a handle to a memory object that is
            ///     marked as discarded.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is a handle to the newly allocated memory object.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            internal static extern IntPtr LocalAlloc(LocalAllocFlags flag, UIntPtr size);

            /// <summary>
            ///     Frees the specified local memory object and invalidates its handle.
            /// </summary>
            /// <param name="hMem">
            ///     A handle to the local memory object. This handle is returned by either the <see cref="LocalAlloc"/>
            ///     function.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is NULL.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            internal static extern IntPtr LocalFree(IntPtr hMem);

            /// <summary>
            ///     Disables or enables drawing in the specified window. Only one window can be locked at a time.
            /// </summary>
            /// <param name="hWndLock">
            ///     The window in which drawing will be disabled. If this parameter is NULL, drawing in the locked window
            ///     is enabled.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            internal static extern bool LockWindowUpdate(IntPtr hWndLock);

            /// <summary>
            ///     Retrieves the name that corresponds to the privilege represented on a specific system by a specified
            ///     locally unique identifier (LUID).
            /// </summary>
            /// <param name="lpSystemName">
            ///     A pointer to a null-terminated string that specifies the name of the system on which the privilege
            ///     name is retrieved. If a null string is specified, the function attempts to find the privilege name
            ///     on the local system.
            /// </param>
            /// <param name="lpLuid">
            ///     A pointer to the <see cref="LuId"/> by which the privilege is known on the target system.
            /// </param>
            /// <param name="lpName">
            ///     A pointer to a buffer that receives a null-terminated string that represents the privilege name.
            ///     For example, this string could be "SeSecurityPrivilege".
            /// </param>
            /// <param name="cchName">
            ///     A pointer to a variable that specifies the size, in a TCHAR value, of the lpName buffer. When the
            ///     function returns, this parameter contains the length of the privilege name, not including the
            ///     terminating null character. If the buffer pointed to by the lpName parameter is too small, this
            ///     variable contains the required size.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the function returns nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true, CharSet = CharSet.Unicode)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool LookupPrivilegeName(string lpSystemName, IntPtr lpLuid, StringBuilder lpName, ref int cchName);

            /// <summary>
            ///     Retrieves the <see cref="LuId"/> used on a specified system to locally represent the specified
            ///     privilege name.
            /// </summary>
            /// <param name="lpSystemName">
            ///     A pointer to a null-terminated string that specifies the name of the system on which the privilege name
            ///     is retrieved. If a null string is specified, the function attempts to find the privilege name on the
            ///     local system.
            /// </param>
            /// <param name="lpName">
            ///     A pointer to a null-terminated string that specifies the name of the privilege, as defined in the
            ///     Winnt.h header file.
            /// </param>
            /// <param name="lpLuid">
            ///     A pointer to a variable that receives the <see cref="LuId"/> by which the privilege is known on
            ///     the system specified by the lpSystemName parameter.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the function returns nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LuId lpLuid);

            /// <summary>
            ///     Translates (maps) a virtual-key code into a scan code or character value, or translates a scan code
            ///     into a virtual-key code.
            /// </summary>
            /// <param name="uCode">
            ///     The virtual key code or scan code for a key. How this value is interpreted depends on the value of
            ///     the uMapType parameter.
            /// </param>
            /// <param name="uMapType">
            ///     The translation to be performed. The value of this parameter depends on the value of the uCode
            ///     parameter.
            /// </param>
            /// <returns>
            ///     The return value is either a scan code, a virtual-key code, or a character value, depending on the
            ///     value of uCode and uMapType. If there is no translation, the return value is zero.
            /// </returns>
            [DllImport(DllNames.User32, CharSet = CharSet.Auto)]
            internal static extern uint MapVirtualKey(uint uCode, uint uMapType);

            /// <summary>
            ///     The mciSendString function sends a command string to an MCI device. The device that the command is sent
            ///     to is specified in the command string.
            /// </summary>
            /// <param name="lpszCommand">
            ///     Pointer to a null-terminated string that specifies an MCI command string.
            /// </param>
            /// <param name="lpszReturnString">
            ///     Pointer to a buffer that receives return information. If no return information is needed, this parameter
            ///     can be NULL.
            /// </param>
            /// <param name="cchReturn">
            ///     Size, in characters, of the return buffer specified by the lpszReturnString parameter.
            /// </param>
            /// <param name="hWndCallback">
            ///     Handle to a callback window if the "notify" flag was specified in the command string.
            /// </param>
            /// <returns>
            ///     Returns zero if successful or an error otherwise. The low-order word of the returned DWORD
            ///     value contains the error return value. If the error is device-specific, the high-order word
            ///     of the return value is the driver identifier; otherwise, the high-order word is zero.
            /// </returns>
            [DllImport(DllNames.Winmm, EntryPoint = "mciSendString", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int MciSendString(string lpszCommand, StringBuilder lpszReturnString, uint cchReturn, IntPtr hWndCallback);

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
            internal static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

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
            ///     <see cref="ProcessBasicInformation"/> parameter.
            /// </param>
            /// <param name="piLen">
            ///     The size of the buffer pointed to by the <see cref="ProcessBasicInformation"/> parameter, in bytes.
            /// </param>
            /// <param name="rLen">
            ///     A pointer to a variable in which the function returns the size of the requested information. If the function
            ///     was successful, this is the size of the information written to the buffer pointed to by the
            ///     <see cref="ProcessBasicInformation"/> parameter, but if the buffer was too small, this is the minimum size
            ///     of buffer needed to receive the information successfully.
            /// </param>
            /// <returns>
            ///     The function returns an NTSTATUS success or error code.
            /// </returns>
            [DllImport(DllNames.Ntdll, SetLastError = false)]
            internal static extern uint NtQueryInformationProcess([In] IntPtr hndl, [In] ProcessInfoFlags piCl, [Out] out ProcessBasicInformation processInformation, [In] uint piLen, [Out] out IntPtr rLen);

            /// <summary>
            ///     Opens an existing local process object.
            /// </summary>
            /// <param name="dwDesiredAccess">
            ///     The access to the process object. This access right is checked against the security descriptor for the
            ///     process. This parameter can be one or more of the process access rights.
            /// </param>
            /// <param name="bInheritHandle">
            ///     If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the
            ///     processes do not inherit this handle.
            /// </param>
            /// <param name="dwProcessId">
            ///     The identifier of the local process to be opened.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is an open handle to the specified
            ///     process.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            internal static extern IntPtr OpenProcess(AccessRights dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

            /// <summary>
            ///     Opens the access token associated with a process.
            /// </summary>
            /// <param name="processHandle">
            ///     A handle to the process whose access token is opened. The process must have the
            ///     <see cref="AccessRights.ProcessQueryInformation"/> access permission.
            /// </param>
            /// <param name="desiredAccess">
            ///     Specifies an access mask that specifies the requested types of access to the access token.
            ///     These requested access types are compared with the discretionary access control list (DACL)
            ///     of the token to determine which accesses are granted or denied.
            /// </param>
            /// <param name="tokenHandle">
            ///     A pointer to a handle that identifies the newly opened access token when the function returns.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

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
            ///     Places (posts) a message in the message queue associated with the thread that created the
            ///     specified window and returns without waiting for the thread to process the message.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose window procedure is to receive the message. The following values
            ///     have special meanings.
            ///     <para>
            ///         HWND_BROADCAST ((HWND)0xffff): The message is posted to all top-level windows in the system,
            ///         including disabled or invisible unowned windows, overlapped windows, and pop-up windows. The
            ///         message is not posted to child windows.
            ///     </para>
            ///     <para>
            ///         NULL: The function behaves like a call to PostThreadMessage with the dwThreadId parameter set
            ///         to the identifier of the current thread.
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
            internal static extern bool PostMessage(HandleRef hWnd, uint msg, IntPtr wParam, IntPtr lParam);

            /// <summary>
            ///     Retrieves the current status of the specified service.
            /// </summary>
            /// <param name="hService">
            ///     A handle to the service. This handle is returned by the OpenService or the CreateService
            ///     function, and it must have the <see cref="ServiceAccessRights.QueryStatus"/> access
            ///     right.
            /// </param>
            /// <param name="lpServiceStatus">
            ///     A pointer to a <see cref="ServiceStatus"/> structure that receives the status information.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.Advapi32, SetLastError = true, CharSet = CharSet.Ansi)]
            internal static extern int QueryServiceStatus(IntPtr hService, ServiceStatus lpServiceStatus);

            /// <summary>
            ///     Reads data from an area of memory in a specified process. The entire area to be
            ///     read must be accessible or the operation fails.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process with memory that is being read. The handle must have
            ///     <see cref="AccessRights.ProcessVmRead"/> access to the process.
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
            internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesRead);

            /// <summary>
            ///     Reads data from an area of memory in a specified process. The entire area to be
            ///     read must be accessible or the operation fails.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process with memory that is being read. The handle must have
            ///     <see cref="AccessRights.ProcessVmRead"/> access to the process.
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
            internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, StringBuilder lpBuffer, IntPtr nSize, ref IntPtr lpNumberOfBytesRead);

            /// <summary>
            ///     Updates the specified rectangle or region in a window's client area.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window to be redrawn. If this parameter is NULL, the desktop window
            ///     is updated.
            /// </param>
            /// <param name="lprcUpdate">
            ///     A pointer to a RECT structure containing the coordinates, in device units, of the
            ///     update rectangle. This parameter is ignored if the hrgnUpdate parameter identifies
            ///     a region.
            /// </param>
            /// <param name="hrgnUpdate">
            ///     A handle to the update region. If both the hrgnUpdate and lprcUpdate parameters are
            ///     NULL, the entire client area is added to the update region.
            /// </param>
            /// <param name="flags">
            ///     One or more redraw flags. This parameter can be used to invalidate or validate a
            ///     window, control repainting, and control which windows are affected by
            ///     <see cref="RedrawWindow"/>.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            internal static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);

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
            internal static extern bool ReleaseCapture();

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
            ///     <see cref="ModifyMenuFlags.ByCommand"/> or <see cref="ModifyMenuFlags.ByPosition"/>.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            internal static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, ModifyMenuFlags uFlags);

            /// <summary>
            ///     Ends the Restart Manager session. This function should be called by the primary installer
            ///     that has previously started the session by calling the RmStartSession function. The
            ///     RmEndSession function can be called by a secondary installer that is joined to the session
            ///     once no more resources need to be registered by the secondary installer.
            /// </summary>
            /// <param name="pSessionHandle">
            ///     A handle to an existing Restart Manager session.
            /// </param>
            /// <returns>
            ///     This is the most recent error received.
            /// </returns>
            [DllImport(DllNames.Rstrtmgr)]
            internal static extern int RmEndSession(uint pSessionHandle);

            /// <summary>
            ///     Gets a list of all applications and services that are currently using resources that have
            ///     been registered with the Restart Manager session.
            /// </summary>
            /// <param name="dwSessionHandle">
            ///     A handle to an existing Restart Manager session.
            /// </param>
            /// <param name="pnProcInfoNeeded">
            ///     A pointer to an array size necessary to receive RM_PROCESS_INFO structures required to
            ///     return information for all affected applications and services.
            /// </param>
            /// <param name="pnProcInfo">
            ///     A pointer to the total number of RM_PROCESS_INFO structures in an array and number of
            ///     structures filled.
            /// </param>
            /// <param name="rgAffectedApps">
            ///     An array of RM_PROCESS_INFO structures that list the applications and services using
            ///     resources that have been registered with the session.
            /// </param>
            /// <param name="lpdwRebootReasons">
            ///     Pointer to location that receives a value of the RM_REBOOT_REASON enumeration that
            ///     describes the reason a system restart is needed.
            /// </param>
            /// <returns>
            ///     This is the most recent error received.
            /// </returns>
            [DllImport(DllNames.Rstrtmgr)]
            internal static extern int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo, [In][Out] RmProcessInfo[] rgAffectedApps, ref uint lpdwRebootReasons);

            /// <summary>
            ///     Registers resources to a Restart Manager session. The Restart Manager uses the list of
            ///     resources registered with the session to determine which applications and services must
            ///     be shut down and restarted. Resources can be identified by filenames, service short names,
            ///     or RM_UNIQUE_PROCESS structures that describe running applications. The RmRegisterResources
            ///     function can be used by a primary or secondary installer.
            /// </summary>
            /// <param name="dwSessionHandle">
            ///     A handle to an existing Restart Manager session.
            /// </param>
            /// <param name="nFiles">
            ///     The number of files being registered.
            /// </param>
            /// <param name="rgsFilenames">
            ///     An array of null-terminated strings of full filename paths.
            /// </param>
            /// <param name="nApplications">
            ///     The number of processes being registered.
            /// </param>
            /// <param name="rgApplications">
            ///     An array of RM_UNIQUE_PROCESS structures.
            /// </param>
            /// <param name="nServices">
            ///     The number of services to be registered.
            /// </param>
            /// <param name="rgsServiceNames">
            ///     An array of null-terminated strings of service short names.
            /// </param>
            /// <returns>
            ///     This is the most recent error received.
            /// </returns>
            [DllImport(DllNames.Rstrtmgr, CharSet = CharSet.Unicode)]
            internal static extern int RmRegisterResources(uint dwSessionHandle, uint nFiles, string[] rgsFilenames, uint nApplications, [In] RmUniqueProcess[] rgApplications, uint nServices, string[] rgsServiceNames);

            /// <summary>
            ///     Starts a new Restart Manager session. A maximum of 64 Restart Manager sessions per user
            ///     session can be open on the system at the same time. When this function starts a session,
            ///     it returns a session handle and session key that can be used in subsequent calls to the
            ///     Restart Manager API.
            /// </summary>
            /// <param name="pSessionHandle">
            ///     A pointer to the handle of a Restart Manager session. The session handle can be passed in
            ///     subsequent calls to the Restart Manager API.
            /// </param>
            /// <param name="dwSessionFlags">
            ///     Reserved. This parameter should be 0.
            /// </param>
            /// <param name="strSessionKey">
            ///     A null-terminated string that contains the session key to the new session. The string must
            ///     be allocated before calling the RmStartSession function.
            /// </param>
            /// <returns>
            ///     This is the most recent error received.
            /// </returns>
            [DllImport(DllNames.Rstrtmgr, CharSet = CharSet.Unicode)]
            internal static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

            /// <summary>
            ///     Synthesizes keystrokes, mouse motions, and button clicks.
            /// </summary>
            /// <param name="nInputs">
            ///     The number of structures in the pInputs array.
            /// </param>
            /// <param name="pInputs">
            ///     An array of <see cref="DeviceInput"/> structures. Each structure represents an event
            ///     to be inserted into the keyboard or mouse input stream.
            /// </param>
            /// <param name="cbSize">
            ///     The size, in bytes, of an <see cref="DeviceInput"/> structure. If cbSize is not the
            ///     size of an <see cref="DeviceInput"/> structure, the function fails.
            /// </param>
            /// <returns>
            ///     The function returns the number of events that it successfully inserted into
            ///     the keyboard or mouse input stream. If the function returns zero, the input
            ///     was already blocked by another thread.
            /// </returns>
            [DllImport(DllNames.User32)]
            internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray)][In] DeviceInput[] pInputs, int cbSize);

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
            [DllImport(DllNames.User32, EntryPoint = "SendMessageA", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            internal static extern IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

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
            [DllImport(DllNames.User32, EntryPoint = "SendMessageA", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            internal static extern IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, ref CopyData lParam);

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
            ///         SMTO_ABORTIFHUNG (0x2): The function returns without waiting for the time-out period to
            ///         elapse if the receiving thread appears to not respond or hangs.
            ///     </para>
            ///     <para>
            ///         SMTO_BLOCK (0x1): Prevents the calling thread from processing any other requests until
            ///         the function returns.
            ///     </para>
            ///     <para>
            ///         SMTO_NORMAL (0x0): The calling thread is not prevented from processing other requests while
            ///         waiting for the function to return.
            ///     </para>
            ///     <para>
            ///         SMTO_NOTIMEOUTIFNOTHUNG (0x8): The function does not enforce the time-out period as long as
            ///         the receiving thread is processing messages.
            ///     </para>
            ///     <para>
            ///         SMTO_ERRORONEXIT (0x20): The function should return 0 if the receiving window is destroyed
            ///         or its owning thread dies while the message is being processed.
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
            internal static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, out UIntPtr lpdwResult);

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
            ///         SMTO_ABORTIFHUNG (0x2): The function returns without waiting for the time-out period to
            ///         elapse if the receiving thread appears to not respond or hangs.
            ///     </para>
            ///     <para>
            ///         SMTO_BLOCK (0x1): Prevents the calling thread from processing any other requests until the
            ///         function returns.
            ///     </para>
            ///     <para>
            ///         SMTO_NORMAL (0x0): The calling thread is not prevented from processing other requests while
            ///         waiting for the function to return.
            ///     </para>
            ///     <para>
            ///         SMTO_NOTIMEOUTIFNOTHUNG (0x8): The function does not enforce the time-out period as long as
            ///         the receiving thread is processing messages.
            ///     </para>
            ///     <para>
            ///         SMTO_ERRORONEXIT (0x20): The function should return 0 if the receiving window is destroyed
            ///         or its owning thread dies while the message is being processed.
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
            internal static extern IntPtr SendMessageTimeoutText(IntPtr hWnd, uint msg, UIntPtr wParam, StringBuilder lParam, uint fuFlags, uint uTimeout, out IntPtr lpdwResult);

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
            internal static extern bool SendNotifyMessage(IntPtr hWnd, uint msg, UIntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

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
            internal static extern bool SetCurrentDirectory([MarshalAs(UnmanagedType.LPWStr)] string lpPathName);

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
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetCursorPos(uint x, uint y);

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
            internal static extern bool SetForegroundWindow(IntPtr hWnd);

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
            internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            /// <summary>
            ///     Sets the minimum and maximum working set sizes for the specified process.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process whose working set sizes is to be set.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.ProcessSetQuota"/> access right.
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
            internal static extern bool SetProcessWorkingSetSize(IntPtr hProcess, UIntPtr dwMinimumWorkingSetSize, UIntPtr dwMaximumWorkingSetSize);

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
            internal static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIdEvent, uint uElapse, TimerProc lpTimerFunc);

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
            internal static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong) =>
                IntPtr.Size == 4 ? SetWindowLongPtr32(hWnd, nIndex, dwNewLong) : SetWindowLongPtr64(hWnd, nIndex, dwNewLong);

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
            ///     A pointer to a <see cref="WindowPlacement"/> structure that specifies the new show state and window
            ///     positions.
            ///     <para>
            ///         Before calling SetWindowPlacement, set the length member of the <see cref="WindowPlacement"/>
            ///         structure to sizeof(<see cref="WindowPlacement"/>). SetWindowPlacement fails if the length
            ///         member is not set correctly.
            ///     </para>
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement lpwndpl);

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
            ///         HWND_BOTTOM ((HWND)1): Places the window at the bottom of the Z order. If the hWnd parameter
            ///         identifies a topmost window, the window loses its topmost status and is placed at the bottom
            ///         of all other windows.
            ///     </para>
            ///     <para>
            ///         HWND_NOTOPMOST ((HWND)-2): Places the window above all non-topmost windows (that is, behind all
            ///         topmost windows). This flag has no effect if the window is already a non-topmost window.
            ///     </para>
            ///     <para>
            ///         HWND_TOP ((HWND)0): Places the window at the top of the Z order.
            ///     </para>
            ///     <para>
            ///         HWND_TOPMOST ((HWND)-1): Places the window above all non-topmost windows. The window maintains
            ///         its topmost position even when it is deactivated.
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
            internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);

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
            internal static extern IntPtr SetWindowsHookEx(Win32HookFlags idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

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
            internal static extern bool SetWindowText(IntPtr hWnd, string lpString);

            /// <summary>
            ///     Causes a window to use a different set of visual style information than its class normally uses.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the window whose visual style information is to be changed.
            /// </param>
            /// <param name="pszSubAppName">
            ///     Pointer to a string that contains the application name to use in place of the calling application's
            ///     name. If this parameter is NULL, the calling application's name is used.
            /// </param>
            /// <param name="pszSubIdList">
            ///     Pointer to a string that contains a semicolon-separated list of CLSID names to use in place of the
            ///     actual list passed by the window's class. If this parameter is NULL, the ID list from the calling
            ///     class is used.
            /// </param>
            /// <returns>
            ///     If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
            /// </returns>
            [DllImport(DllNames.Uxtheme, SetLastError = true, BestFitMapping = false, CharSet = CharSet.Unicode)]
            internal static extern int SetWindowTheme(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] string pszSubAppName, [MarshalAs(UnmanagedType.LPTStr)] string pszSubIdList);

            /// <summary>
            ///     Sets attributes to control how visual styles are applied to a specified window.
            /// </summary>
            /// <param name="hWnd">
            ///     Handle to a window to apply changes to.
            /// </param>
            /// <param name="eAttribute">
            ///     Value of type <see cref="WindowThemeAttributeTypes"/> that specifies the type of attribute to set.
            /// </param>
            /// <param name="pvAttribute">
            ///     A pointer that specifies attributes to set. Type is determined by the value of the eAttribute value.
            /// </param>
            /// <param name="cbAttribute">
            ///     Specifies the size, in bytes, of the data pointed to by pvAttribute.
            /// </param>
            [DllImport(DllNames.Uxtheme, PreserveSig = false, SetLastError = true)]
            internal static extern void SetWindowThemeAttribute([In] IntPtr hWnd, [In] WindowThemeAttributeTypes eAttribute, [In] ref WindowThemeAttributeOptions pvAttribute, [In] uint cbAttribute);

            /// <summary>
            ///     Sends an appbar message to the system.
            /// </summary>
            /// <param name="dwMessage">
            ///     Appbar message value to send.
            /// </param>
            /// <param name="pData">
            ///     A pointer to an <see cref="AppBarData"/> structure. The content of the structure on entry and on exit
            ///     depends on the value set in the dwMessage parameter. See the individual message pages for specifics.
            /// </param>
            /// <returns>
            ///     This function returns a message-dependent value. For more information, see the Windows SDK documentation
            ///     for the specific appbar message sent. Links to those documents are given in the See Also section.
            /// </returns>
            [DllImport(DllNames.Shell32, SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern UIntPtr SHAppBarMessage(AppBarMessageOptions dwMessage, ref AppBarData pData);

            /// <summary>
            ///     Performs an operation on a specified file.
            /// </summary>
            /// <param name="hWnd">
            ///     A handle to the parent window used for displaying a UI or error messages. This value can be NULL if the
            ///     operation is not associated with a window.
            /// </param>
            /// <param name="lpOperation">
            ///     A pointer to a null-terminated string, referred to in this case as a verb, that specifies the action to
            ///     be performed. The set of available verbs depends on the particular file or folder. Generally, the
            ///     actions available from an object's shortcut menu are available verbs.
            /// </param>
            /// <param name="lpFile">
            ///     A pointer to a null-terminated string that specifies the file or object on which to execute the specified
            ///     verb. To specify a Shell namespace object, pass the fully qualified parse name. Note that not all verbs
            ///     are supported on all objects. For example, not all document types support the "print" verb. If a relative
            ///     path is used for the lpDirectory parameter do not use a relative path for lpFile.
            /// </param>
            /// <param name="lpParameters">
            ///     If lpFile specifies an executable file, this parameter is a pointer to a null-terminated string that
            ///     specifies the parameters to be passed to the application. The format of this string is determined by the
            ///     verb that is to be invoked. If lpFile specifies a document file, lpParameters should be NULL.
            /// </param>
            /// <param name="lpDirectory">
            ///     A pointer to a null-terminated string that specifies the default (working) directory for the action. If
            ///     this value is NULL, the current working directory is used. If a relative path is provided at lpFile, do
            ///     not use a relative path for lpDirectory.
            /// </param>
            /// <param name="nShowCmd">
            ///     The flags that specify how an application is to be displayed when it is opened. If lpFile specifies a
            ///     document file, the flag is simply passed to the associated application. It is up to the application to
            ///     decide how to handle it.
            /// </param>
            /// <returns>
            ///     If the function succeeds, it returns a value greater than 32. If the function fails, it returns an error
            ///     value that indicates the cause of the failure. The return value is cast as an HINSTANCE for backward
            ///     compatibility with 16-bit Windows applications. It is not a true HINSTANCE, however.
            /// </returns>
            [DllImport(DllNames.Shell32, EntryPoint = "ShellExecute", SetLastError = true, BestFitMapping = false, CharSet = CharSet.Unicode)]
            internal static extern IntPtr ShellExecute(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] string lpOperation, [MarshalAs(UnmanagedType.LPTStr)] string lpFile, [MarshalAs(UnmanagedType.LPTStr)] string lpParameters, [MarshalAs(UnmanagedType.LPTStr)] string lpDirectory, ShowWindowFlags nShowCmd);

            /// <summary>
            ///     Retrieves information about an object in the file system, such as a file, folder, directory, or
            ///     drive root.
            /// </summary>
            /// <param name="pszPath">
            ///     A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and file
            ///     name. Both absolute and relative paths are valid.
            ///     <para>
            ///         If the uFlags parameter includes the <see cref="FileInfoFlags.PidL"/> flag, this parameter must
            ///         be the address of an ITEMIDLIST (PIDL) structure that contains the list of item identifiers that
            ///         uniquely identifies the file within the Shell's namespace. The PIDL must be a fully qualified PIDL.
            ///         Relative PIDLs are not allowed.
            ///     </para>
            ///     <para>
            ///         If the uFlags parameter includes the <see cref="FileInfoFlags.UseFileAttributes"/> flag, this
            ///         parameter does not have to be a valid file name. The function will proceed as if the file exists
            ///         with the specified name and with the file attributes passed in the dwFileAttributes parameter. This
            ///         allows you to obtain information about a file type by passing just the extension for pszPath and
            ///         passing <see cref="System.IO.FileAttributes.Normal"/> in dwFileAttributes.
            ///     </para>
            ///     <para>
            ///         This string can use either short (the 8.3 form) or long file names.
            ///     </para>
            /// </param>
            /// <param name="dwFileAttributes">
            ///     A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If
            ///     uFlags does not include the <see cref="FileInfoFlags.UseFileAttributes"/> flag, this parameter is
            ///     ignored.
            /// </param>
            /// <param name="psfi">
            ///     Pointer to a <see cref="ShFileInfo"/> structure to receive the file information.
            /// </param>
            /// <param name="cbFileInfo">
            ///     The size, in bytes, of the <see cref="ShFileInfo"/> structure pointed to by the psfi parameter.
            /// </param>
            /// <param name="uFlags">
            ///     The flags that specify the file information to retrieve.
            /// </param>
            /// <returns>
            /// </returns>
            [DllImport(DllNames.Shell32, SetLastError = true, BestFitMapping = false, CharSet = CharSet.Unicode)]
            internal static extern IntPtr SHGetFileInfo([MarshalAs(UnmanagedType.LPStr, SizeConst = 32767)] string pszPath, uint dwFileAttributes, ref ShFileInfo psfi, uint cbFileInfo, FileInfoFlags uFlags);

            /// <summary>
            ///     The ShowScrollBar function shows or hides the specified scroll bar.
            /// </summary>
            /// <param name="hWnd">
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
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ShowScrollBar(IntPtr hWnd, ShowScrollBarOptions wBar, [MarshalAs(UnmanagedType.Bool)] bool bShow);

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
            internal static extern bool ShowWindow(IntPtr hWnd, ShowWindowFlags nCmdShow);

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
            internal static extern bool ShowWindowAsync(IntPtr hWnd, ShowWindowFlags nCmdShow);

            /// <summary>
            ///     Starts a service.
            /// </summary>
            /// <param name="hService">
            ///     A handle to the service. This handle is returned by the OpenService or CreateService function,
            ///     and it must have the <see cref="ServiceAccessRights.Start"/> access right.
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

            /// <summary>
            ///     Terminates the specified process and all of its threads.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process to be terminated.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.ProcessTerminate"/> access right.
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
            internal static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

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
            [DllImport(DllNames.Winmm, EntryPoint = "timeBeginPeriod", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern uint TimeBeginPeriod(uint uPeriod);

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
            [DllImport(DllNames.Winmm, EntryPoint = "timeEndPeriod", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern uint TimeEndPeriod(uint uPeriod);

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
            internal static extern int UnhookWindowsHookEx(IntPtr hhk);

            /// <summary>
            ///     The UpdateWindow function updates the client area of the specified window by sending a WM_PAINT message to the
            ///     window if the window's update region is not empty. The function sends a WM_PAINT message directly to the window
            ///     procedure of the specified window, bypassing the application queue. If the update region is empty, no message
            ///     is sent.
            /// </summary>
            /// <param name="hWnd">
            ///     Handle to the window to be updated.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is nonzero.
            /// </returns>
            [DllImport(DllNames.User32, SetLastError = true)]
            internal static extern bool UpdateWindow(IntPtr hWnd);

            /// <summary>
            ///     Reserves, commits, or changes the state of a region of memory within the virtual address space
            ///     of a specified process. The function initializes the memory it allocates to zero.
            /// </summary>
            /// <param name="hProcess">
            ///     The handle to a process. The function allocates memory within the virtual address
            ///     space of this process.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.ProcessVmOperation"/> access
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
            ///     The memory protection for the region of pages to be allocated.
            /// </param>
            /// <returns>
            ///     If the function succeeds, the return value is the base address of the allocated region
            ///     of pages.
            /// </returns>
            [DllImport(DllNames.Kernel32, SetLastError = true)]
            internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, MemAllocTypes flAllocationType, MemProtectFlags flProtect);

            /// <summary>
            ///     Releases, decommits, or releases and decommits a region of memory within the virtual address
            ///     space of a specified process.
            /// </summary>
            /// <param name="hProcess">
            ///     The handle to a process. The function allocates memory within the virtual address space of this
            ///     process.
            ///     <para>
            ///         The handle must have the <see cref="AccessRights.ProcessVmOperation"/> access
            ///         right.
            ///     </para>
            /// </param>
            /// <param name="lpAddress">
            ///     A pointer to the starting address of the region of memory to be freed.
            ///     <para>
            ///         If the dwFreeType parameter is <see cref="MemAllocTypes.Release"/>, lpAddress must be
            ///         the base address returned by the VirtualAllocEx function when the region is reserved.
            ///     </para>
            /// </param>
            /// <param name="dwSize">
            ///     The size of the region of memory to free, in bytes.
            ///     <para>
            ///         If the dwFreeType parameter is <see cref="MemAllocTypes.Release"/>, dwSize must
            ///         be 0 (zero). The function frees the entire region that is reserved in the initial
            ///         allocation call to VirtualAllocEx.
            ///     </para>
            ///     <para>
            ///         If dwFreeType is <see cref="MemAllocTypes.Decommit"/>, the function decommits all memory
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
            internal static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, MemFreeTypes dwFreeType);

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
            ///     right-channel setting. A value of 0xFFFF represents full volume, and a value of 0x0 is silence.
            /// </param>
            /// <returns>
            ///     Returns MMSYSERR_NOERROR if successful or an error otherwise.
            /// </returns>
            [DllImport(DllNames.Winmm, EntryPoint = "waveOutGetVolume", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int WaveOutGetVolume(IntPtr hwo, out uint dwVolume);

            /// <summary>
            ///     The waveOutSetVolume function sets the volume level of the specified waveform-audio output device.
            /// </summary>
            /// <param name="hwo">
            ///     Handle to an open waveform-audio output device. This parameter can also be a device identifier.
            /// </param>
            /// <param name="dwVolume">
            ///     New volume setting. The low-order word contains the left-channel volume setting, and the high-order
            ///     word contains the right-channel setting. A value of 0xFFFF represents full volume, and a value of
            ///     0x0 is silence.
            /// </param>
            /// <returns>
            ///     Returns MMSYSERR_NOERROR if successful or an error otherwise.
            /// </returns>
            [DllImport(DllNames.Winmm, EntryPoint = "waveOutSetVolume", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);

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
            ///     Writes data to an area of memory in a specified process. The entire area to be written to must be
            ///     accessible or the operation fails.
            /// </summary>
            /// <param name="hProcess">
            ///     A handle to the process memory to be modified. The handle must have
            ///     <see cref="AccessRights.ProcessVmWrite"/> and
            ///     <see cref="AccessRights.ProcessVmOperation"/> access to the process.
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
            internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, [MarshalAs(UnmanagedType.SysInt)] int nSize, out IntPtr lpNumberOfBytesWritten);
        }

        /// <summary>
        ///     Contains information about a newly created process and its primary thread.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct ProcessInformation
        {
            /// <summary>
            ///     A handle to the newly created process. The handle is used to specify the process in all functions
            ///     that perform operations on the process object.
            /// </summary>
            internal IntPtr hProcess;

            /// <summary>
            ///     A handle to the primary thread of the newly created process. The handle is used to specify the
            ///     thread in all functions that perform operations on the thread object.
            /// </summary>
            internal IntPtr hThread;

            /// <summary>
            ///     A value that can be used to identify a process. The value is valid from the time the process is
            ///     created until all handles to the process are closed and the process object is freed; at this
            ///     point, the identifier may be reused.
            /// </summary>
            internal int dwProcessId;

            /// <summary>
            ///     A value that can be used to identify a thread. The value is valid from the time the thread is
            ///     created until all handles to the thread are closed and the thread object is freed; at this point,
            ///     the identifier may be reused.
            /// </summary>
            internal int dwThreadId;
        }

        /// <summary>
        ///     Contains the security descriptor for an object and specifies whether the handle retrieved
        ///     by specifying this structure is inheritable. This structure provides security settings for
        ///     objects created by various functions.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct SecurityAttributes
        {
            /// <summary>
            ///     The size, in bytes, of this structure. Set this value to the size of the
            ///     <see cref="SecurityAttributes"/> structure.
            /// </summary>
            internal int nLength;

            /// <summary>
            ///     A pointer to a security descriptor structure that controls access to the object. If the
            ///     value of this member is NULL, the object is assigned the default security descriptor
            ///     associated with the access token of the calling process. This is not the same as granting
            ///     access to everyone by assigning a NULL discretionary access control list (DACL). By default,
            ///     the default DACL in the access token of a process allows access only to the user represented
            ///     by the access token.
            /// </summary>
            internal IntPtr lpSecurityDescriptor;

            /// <summary>
            ///     A value that specifies whether the returned handle is inherited when a new process is created.
            ///     If this member is TRUE, the new process inherits the handle.
            /// </summary>
            internal int bInheritHandle;
        }

        /// <summary>
        ///     Contains status information for a service.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal class ServiceStatus
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
            internal ServiceControlTypes dwControlsAccepted;

            /// <summary>
            ///     The current state of the service.
            /// </summary>
            internal ServiceStateTypes dwCurrentState;

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
        ///     Specifies the window station, desktop, standard handles, and appearance of the main window for
        ///     a process at creation time.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct StartupInfo
        {
            /// <summary>
            ///     The size of the structure, in bytes.
            /// </summary>
            internal int cb;

            /// <summary>
            ///     Reserved; must be NULL.
            /// </summary>
            internal string lpReserved;

            /// <summary>
            ///     The name of the desktop, or the name of both the desktop and window station for this process.
            ///     A backslash in the string indicates that the string includes both the desktop and window station
            ///     names.
            /// </summary>
            internal string lpDesktop;

            /// <summary>
            ///     For console processes, this is the title displayed in the title bar if a new console window is
            ///     created. If NULL, the name of the executable file is used as the window title instead. This
            ///     parameter must be NULL for GUI or console processes that do not create a new console window.
            /// </summary>
            internal string lpTitle;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UsePosition"/>, this member is the x offset of the
            ///     upper left corner of a window if a new window is created, in pixels. Otherwise, this member is
            ///     ignored.
            /// </summary>
            internal int dwX;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UsePosition"/>, this member is the y offset of the
            ///     upper left corner of a window if a new window is created, in pixels. Otherwise, this member is
            ///     ignored.
            /// </summary>
            internal int dwY;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UseSize"/>, this member is the width of the window
            ///     if a new window is created, in pixels. Otherwise, this member is ignored.
            /// </summary>
            internal int dwXSize;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UseSize"/>, this member is the height of the window
            ///     if a new window is created, in pixels. Otherwise, this member is ignored.
            /// </summary>
            internal int dwYSize;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UseCountChars"/>, if a new console window is created
            ///     in a console process, this member specifies the screen buffer width, in character columns.
            ///     Otherwise, this member is ignored.
            /// </summary>
            internal int dwXCountChars;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UseCountChars"/>, if a new console window is created
            ///     in a console process, this member specifies the screen buffer height, in character rows.
            ///     Otherwise, this member is ignored.
            /// </summary>
            internal int dwYCountChars;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UseFillAttribute"/>, this member is the initial text
            ///     and background colors if a new console window is created in a console application. Otherwise,
            ///     this member is ignored.
            /// </summary>
            internal int dwFillAttribute;

            /// <summary>
            ///     A bitfield that determines whether certain <see cref="StartupInfo"/> members are used when the
            ///     process creates a window.
            /// </summary>
            internal StartFlags dwFlags;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UseShowWindow"/>.
            /// </summary>
            internal short wShowWindow;

            /// <summary>
            ///     Reserved for use by the C Run-time; must be zero.
            /// </summary>
            internal short cbReserved2;

            /// <summary>
            ///     Reserved for use by the C Run-time; must be zero.
            /// </summary>
            internal IntPtr lpReserved2;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UseStdHandles"/>, this member is the standard input
            ///     handle for the process. If <see cref="StartFlags.UseStdHandles"/> is not specified, the default
            ///     for standard input is the keyboard buffer.
            /// </summary>
            internal IntPtr hStdInput;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UseStdHandles"/>, this member is the standard output
            ///     handle for the process. Otherwise, this member is ignored and the default for standard output
            ///     is the console window's buffer.
            /// </summary>
            internal IntPtr hStdOutput;

            /// <summary>
            ///     If dwFlags specifies <see cref="StartFlags.UseStdHandles"/>, this member is the standard error
            ///     handle for the process. Otherwise, this member is ignored and the default for standard error is
            ///     the console window's buffer.
            /// </summary>
            internal IntPtr hStdError;
        }

        /// <summary>
        ///     Contains information about a system appbar message.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct AppBarData : IDisposable
        {
            /// <summary>
            ///     The size of the structure, in bytes.
            /// </summary>
            public uint cbSize;

            /// <summary>
            ///     The handle to the appbar window. Not all messages use this member. See the individual message
            ///     page to see if you need to provide an hWind value.
            /// </summary>
#pragma warning disable IDE1006
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public IntPtr hWnd { get; internal set; }
#pragma warning restore IDE1006
            /// <summary>
            ///     An application-defined message identifier. The application uses the specified identifier for
            ///     notification messages that it sends to the appbar identified by the hWnd member.
            /// </summary>
            public uint uCallbackMessage;

            /// <summary>
            ///     A value that specifies an edge of the screen.
            ///     <para>
            ///         This member is used when sending one of these messages:
            ///         <see cref="AppBarMessageOptions.GetAutoHideBar"/>
            ///         <see cref="AppBarMessageOptions.SetAutoHideBar"/>
            ///         <see cref="AppBarMessageOptions.GetAutoHideBarEx"/>
            ///         <see cref="AppBarMessageOptions.SetAutoHideBarEx"/>
            ///         <see cref="AppBarMessageOptions.QueryPos"/>
            ///         <see cref="AppBarMessageOptions.SetPos"/>.
            ///     </para>
            /// </summary>
            public uint uEdge;

            /// <summary>
            ///     A <see cref="Rectangle"/> structure whose use varies depending on the message:
            ///     <para>
            ///         <see cref="AppBarMessageOptions.GetTaskBarPos"/>,
            ///         <see cref="AppBarMessageOptions.QueryPos"/>,
            ///         <see cref="AppBarMessageOptions.SetPos"/>: The bounding rectangle, in screen
            ///         coordinates, of an appbar or the Windows taskbar.
            ///     </para>
            ///     <para>
            ///         <see cref="AppBarMessageOptions.GetAutoHideBarEx"/>,
            ///         <see cref="AppBarMessageOptions.SetAutoHideBarEx"/>,
            ///         <see cref="AppBarMessageOptions.SetPos"/>: The monitor on which the operation
            ///         is being performed.
            ///     </para>
            /// </summary>
            public Rectangle rc;

            /// <summary>
            ///     A message-dependent value. This member is used with these messages:
            ///     <para>
            ///         <see cref="AppBarMessageOptions.SetAutoHideBar"/>: Registers or unregisters an
            ///         autohide appbar for a given edge of the screen. If the system has multiple monitors,
            ///         the monitor that contains the primary taskbar is used.
            ///     </para>
            ///     <para>
            ///         <see cref="AppBarMessageOptions.SetAutoHideBarEx"/>: Registers or unregisters an
            ///         autohide appbar for a given edge of the screen. This message extends
            ///         <see cref="AppBarMessageOptions.SetAutoHideBar"/> by enabling you to specify a
            ///         particular monitor, for use in multiple monitor situations.
            ///     </para>
            ///     <para>
            ///         <see cref="AppBarMessageOptions.SetState"/>: Sets the autohide and always-on-top
            ///         states of the Windows taskbar.
            ///     </para>
            /// </summary>
            public int lParam;

            /// <summary>
            ///     Releases all resources used by this <see cref="AppBarData"/>.
            /// </summary>
            public void Dispose()
            {
                if (hWnd == IntPtr.Zero)
                    return;
                NativeMethods.LocalFree(hWnd);
                hWnd = IntPtr.Zero;
            }
        }

        /// <summary>
        ///     Defines the message parameters passed to a <see cref="Win32HookFlags.WhCallWndProcRet"/>
        ///     hook procedure.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [StructLayout(LayoutKind.Sequential)]
        public struct CallWndProcRet
        {
            /// <summary>
            ///     The return value of the window procedure that processed the message specified by
            ///     the message value.
            /// </summary>
#pragma warning disable IDE1006
            public IntPtr lResult { get; internal set; }

            /// <summary>
            ///     Additional information about the message. The exact meaning depends on the message
            ///     value.
            /// </summary>
            public IntPtr lParam { get; internal set; }

            /// <summary>
            ///     Additional information about the message. The exact meaning depends on the message
            ///     value.
            /// </summary>
            public IntPtr wParam { get; internal set; }
#pragma warning restore IDE1006
            /// <summary>
            ///     The message.
            /// </summary>
            public uint message;

            /// <summary>
            ///     A handle to the window that processed the message specified by the message value.
            /// </summary>
#pragma warning disable IDE1006
            public IntPtr hwnd { get; internal set; }
#pragma warning restore IDE1006
        }

        /// <summary>
        ///     Contains data to be passed to another application by the
        ///     <see cref="F:SilDev.WinApi.WindowMenuFlags.WmCopyData"/> message.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [StructLayout(LayoutKind.Sequential)]
        public struct CopyData : IDisposable
        {
            /// <summary>
            ///     The data to be passed to the receiving application.
            /// </summary>
#pragma warning disable IDE1006
            public IntPtr dwData { get; internal set; }
#pragma warning restore IDE1006
            /// <summary>
            ///     The size, in bytes, of the data pointed to by the lpData member.
            /// </summary>
            public int cbData;

            /// <summary>
            ///     The data to be passed to the receiving application. This member can be NULL.
            /// </summary>
#pragma warning disable IDE1006
            public IntPtr lpData { get; internal set; }
#pragma warning restore IDE1006
            /// <summary>
            ///     Releases all resources used by this <see cref="CopyData"/>.
            /// </summary>
            public void Dispose()
            {
                if (lpData == IntPtr.Zero)
                    return;
                NativeMethods.LocalFree(lpData);
                lpData = IntPtr.Zero;
            }
        }

        /// <summary>
        ///     Used by <see cref="NativeMethods.SendInput(uint, DeviceInput[], int)"/> to store
        ///     information for synthesizing input events such as keystrokes, mouse movement, and
        ///     mouse clicks.
        /// </summary>
        public struct DeviceInput
        {
            /// <summary>
            ///     The type of the input event. This member can be one of the following values.
            ///     <para>
            ///         0: The event is a mouse event. Use the mi structure of the union.
            ///     </para>
            ///     <para>
            ///         1: The event is a keyboard event. Use the ki structure of the union.
            ///     </para>
            ///     <para>
            ///         2: The event is a hardware event. Use the hi structure of the union.
            ///     </para>
            /// </summary>
            public uint Type;

            /// <summary>
            ///     The information about a simulated mouse, keyboard or hardware event.
            /// </summary>
            public MouseKeyboardHardwareInput Data;
        }

        /// <summary>
        ///     Contains the names of the used Windows dynamic-link library (DLL) files.
        /// </summary>
        public struct DllNames
        {
            internal const string Advapi32 = "advapi32.dll";
            internal const string Dwmapi = "dwmapi.dll";
            internal const string Msi = "msi.dll";
            internal const string Rstrtmgr = "rstrtmgr.dll";
            internal const string Urlmon = "urlmon.dll";
            internal const string Uxtheme = "uxtheme.dll";
            internal const string Winmm = "winmm.dll";
#pragma warning disable CS1591
            public const string Kernel32 = "kernel32.dll";
            public const string Ntdll = "ntdll.dll";
            public const string Psapi = "psapi.dll";
            public const string Shell32 = "shell32.dll";
            public const string User32 = "user32.dll";
#pragma warning restore CS1591
        }

        /// <summary>
        ///     Contains the locally unique identifier (LUID).
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct LuId
        {
            /// <summary>
            ///     Low-order bits.
            /// </summary>
            public uint LowPart;

            /// <summary>
            ///     High-order bits.
            /// </summary>
            public int HighPart;
        }

        /// <summary>
        ///     Represents a locally unique identifier (LUID) and its attributes.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct LuIdAndAttributes
        {
            /// <summary>
            ///     Specifies an <see cref="LuId"/> value.
            /// </summary>
            public LuId Luid;

            /// <summary>
            ///     Specifies attributes of the <see cref="LuId"/>. This value contains up to 32 one-bit flags.
            ///     Its meaning is dependent on the definition and use of the <see cref="LuId"/>.
            /// </summary>
            public uint Attributes;
        }

        /// <summary>
        ///     Stores information about a simulated mouse event.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
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
            public IntPtr ExtraInfo { get; internal set; }
        }

        /// <summary>
        ///     Stores information about a simulated mouse, keyboard or hardware event.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct MouseKeyboardHardwareInput
        {
#pragma warning disable CS1591
            [FieldOffset(0)]
            public MouseInput Mouse;

            /*
            [FieldOffset(1)]
            public KeyboardInput Keyboard;
            [FieldOffset(2)]
            public HardwareInput Hardware;
            */
#pragma warning restore CS1591
        }

        /// <summary>
        ///     Contains basic information about a process.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessBasicInformation
        {
#pragma warning disable CS1591
            public IntPtr ExitStatus { get; internal set; }

            public IntPtr PebBaseAddress { get; internal set; }

            public IntPtr AffinityMask { get; internal set; }

            public IntPtr BasePriority { get; internal set; }

            public UIntPtr UniqueProcessId { get; internal set; }

            public IntPtr InheritedFromUniqueProcessId { get; internal set; }
#pragma warning restore CS1591
        }

        /// <summary>
        ///     Returned by the GetThemeMargins function to define the margins of windows that have visual
        ///     styles applied.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ThemeMargins
        {
            /// <summary>
            ///     Width of the left border that retains its size.
            /// </summary>
            public int cxLeftWidth;

            /// <summary>
            ///     Width of the right border that retains its size.
            /// </summary>
            public int cxRightWidth;

            /// <summary>
            ///     Height of the top border that retains its size.
            /// </summary>
            public int cyTopHeight;

            /// <summary>
            ///     Height of the bottom border that retains its size.
            /// </summary>
            public int cyBottomHeight;
        }

        /// <summary>
        ///     Contains information about a set of privileges for an access token.
        /// </summary>
        public struct TokenPrivileges
        {
            /// <summary>
            ///     This must be set to the number of entries in the Privileges array.
            /// </summary>
            public uint PrivilegeCount;

            /// <summary>
            ///     Specifies an array of <see cref="LuIdAndAttributes"/> structures. Each structure
            ///     contains the <see cref="LuId"/> and attributes of a privilege. To get the name of
            ///     the privilege associated with a <see cref="LuId"/>, call the
            ///     <see cref="NativeHelper.LookupPrivilegeName"/> function, passing the address of
            ///     the <see cref="LuId"/> as the value of the lpLuid parameter.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public LuIdAndAttributes[] Privileges;
        }

        /// <summary>
        ///     Contains information about the placement of a window on the screen.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct WindowPlacement
        {
            /// <summary>
            ///     The length of the structure, in bytes.
            /// </summary>
            public int length;

            /// <summary>
            ///     The flags that control the position of the minimized window and the method by which
            ///     the window is restored.
            ///     <para>
            ///         This member can be one or more of the <see cref="WindowPlacementFlags"/> values.
            ///     </para>
            /// </summary>
            public int flags;

            /// <summary>
            ///     The current show state of the window.
            ///     <para>
            ///         This member can be one of the <see cref="ShowWindowFlags"/> values.
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
        ///     Contains information about the placement of a window on the screen.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct WindowThemeAttributeOptions
        {
            /// <summary>
            ///     A combination of flags that modify window visual style attributes.
            /// </summary>
            public WindowThemeAttributeFlags Flags;

            /// <summary>
            ///     A bitmask that describes how the values specified in dwFlags should be applied. If
            ///     the bit corresponding to a value in <see cref="Flags"/> is 0, that flag will be
            ///     removed. If the bit is 1, the flag will be added.
            /// </summary>
            public uint Mask;
        }

        /// <summary>
        ///     Contains information about the colorization of Windows.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct DwmColorizationParams
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
        ///     Describes an application that is to be registered with the Restart Manager.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct RmProcessInfo
        {
            /// <summary>
            ///     Contains an RM_UNIQUE_PROCESS structure that uniquely identifies the application
            ///     by its PID and the time the process began.
            /// </summary>
            public RmUniqueProcess Process;

            /// <summary>
            ///     If the process is a service, this parameter returns the long name for the service. If
            ///     the process is not a service, this parameter returns the user-friendly name for the
            ///     application. If the process is a critical process, and the installer is run with
            ///     elevated privileges, this parameter returns the name of the executable file of the
            ///     critical process. If the process is a critical process, and the installer is run as a
            ///     service, this parameter returns the long name of the critical process.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string strAppName;

            /// <summary>
            ///     If the process is a service, this is the short name for the service. This member is
            ///     not used if the process is not a service.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strServiceShortName;

            /// <summary>
            ///     Contains an RM_APP_TYPE enumeration value that specifies the type of application as
            ///     RmUnknownApp, RmMainWindow, RmOtherWindow, RmService, RmExplorer or RmCritical.
            /// </summary>
            public RmAppTypes ApplicationType;

            /// <summary>
            ///     Contains a bit mask that describes the current status of the application. See the
            ///     RM_APP_STATUS enumeration.
            /// </summary>
            public uint AppStatus;

            /// <summary>
            ///     Contains the Terminal Services session ID of the process. If the terminal session of
            ///     the process cannot be determined, the value of this member is set to RM_INVALID_SESSION
            ///     (-1). This member is not used if the process is a service or a system critical process.
            /// </summary>
            public uint TSSessionId;

            /// <summary>
            ///     TRUE if the application can be restarted by the Restart Manager; otherwise, FALSE. This
            ///     member is always TRUE if the process is a service. This member is always FALSE if the
            ///     process is a critical system process.
            /// </summary>
            [MarshalAs(UnmanagedType.Bool)]
            public bool bRestartable;
        }

        /// <summary>
        ///     Uniquely identifies a process by its PID and the time the process began. An array of
        ///     RM_UNIQUE_PROCESS structures can be passed to the RmRegisterResources function.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct RmUniqueProcess
        {
            /// <summary>
            ///     The product identifier (PID).
            /// </summary>
            public int dwProcessId;

            /// <summary>
            ///     The creation time of the process. The time is provided as a FILETIME structure that is
            ///     returned by the lpCreationTime parameter of the GetProcessTimes function.
            /// </summary>
            public FileTime ProcessStartTime;
        }

        /// <summary>
        ///     Contains information about a file object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct ShFileInfo
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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string szDisplayName;

            /// <summary>
            ///     A string that describes the type of file.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            internal string szTypeName;
        }
    }
}
