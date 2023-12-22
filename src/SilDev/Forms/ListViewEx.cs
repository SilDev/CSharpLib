#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ListViewEx.cs
// Version:  2023-12-22 11:56
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Windows.Forms;
    using static WinApi;

    /// <summary>
    ///     Expands the functionality for the <see cref="ListView"/> class.
    /// </summary>
    public static class ListViewEx
    {
        private const int LvmSetHotCursor = 0x103e;
        private const int UiSfHideFocus = 0x1;
        private const int UiSSet = 0x1;
        private const int WmChangeUiState = 0x127;

        /// <summary>
        ///     Retrieves the <see cref="ListViewItem"/> at the current cursor's position.
        /// </summary>
        /// <param name="listView">
        ///     The <see cref="ListView"/> control to check.
        /// </param>
        public static ListViewItem ItemFromPoint(this ListView listView)
        {
            if (listView is not { } lv)
                return null;
            var pos = lv.PointToClient(Cursor.Position);
            return lv.GetItemAt(pos.X, pos.Y);
        }

        /// <summary>
        ///     Sets the cursor shape that is shown when the mouse is over an element of
        ///     the <see cref="ListView"/>.
        /// </summary>
        /// <param name="listView">
        ///     The <see cref="ListView"/> control to change.
        /// </param>
        /// <param name="cursor">
        ///     The <see cref="Cursor"/> to set.
        /// </param>
        public static void SetMouseOverCursor(this ListView listView, Cursor cursor = default)
        {
            if (listView is not { } lv)
                return;
            if (cursor == default)
                cursor = Cursors.Arrow;
            NativeHelper.SendMessage(lv.Handle, LvmSetHotCursor, IntPtr.Zero, cursor.Handle);
        }

        /// <summary>
        ///     Enables the Windows Explorer selection border style for
        ///     <see cref="ListView"/> elements.
        /// </summary>
        /// <param name="listView">
        ///     The <see cref="ListView"/> control to change.
        /// </param>
        public static void EnableExplorerSelectionStyle(this ListView listView)
        {
            if (listView is { } lv)
                NativeHelper.SetWindowTheme(lv.Handle, Desktop.AppsUseDarkTheme ? "DarkMode_Explorer" : "Explorer");
        }

        /// <summary>
        ///     Removes the dotted selection borders from <see cref="ListView"/> elements.
        /// </summary>
        /// <param name="listView">
        ///     The <see cref="ListView"/> control to change.
        /// </param>
        public static void RemoveDottedSelectionBorders(this ListView listView)
        {
            if (listView is { } lv)
                NativeHelper.SendMessage(lv.Handle, WmChangeUiState, new IntPtr(UiSSet | (0x10000 ^ UiSfHideFocus)), IntPtr.Zero);
        }
    }
}
