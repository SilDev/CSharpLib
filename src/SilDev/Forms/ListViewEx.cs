#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ListViewEx.cs
// Version:  2021-04-22 19:45
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="ListView"/> class.
    /// </summary>
    public static class ListViewEx
    {
        private const int LvmSetHotCursor = 0x103e;

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
            WinApi.NativeHelper.SendMessage(lv.Handle, LvmSetHotCursor, IntPtr.Zero, cursor.Handle);
        }
    }
}
