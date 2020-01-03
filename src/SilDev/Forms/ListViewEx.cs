#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ListViewEx.cs
// Version:  2020-01-03 12:53
// 
// Copyright (c) 2020, Si13n7 Developments (r)
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
            if (!(listView is ListView lv))
                return null;
            var pos = lv.PointToClient(Cursor.Position);
            return lv.GetItemAt(pos.X, pos.Y);
        }

        /// <summary>
        ///     Sets the cursor shape that is shown when the mouse is over an element of the
        ///     <see cref="ListView"/>.
        /// </summary>
        /// <param name="listView">
        ///     The <see cref="ListView"/> control to change.
        /// </param>
        /// <param name="cursor">
        ///     The <see cref="Cursor"/> to set.
        /// </param>
        public static void SetMouseOverCursor(this ListView listView, Cursor cursor = default)
        {
            if (!(listView is ListView lv))
                return;
            if (cursor == default)
                cursor = Cursors.Arrow;
            WinApi.NativeHelper.SendMessage(lv.Handle, LvmSetHotCursor, IntPtr.Zero, cursor.Handle);
        }

        /// <summary>
        ///     Represents a Windows list view control, which displays a collection of items that
        ///     can be displayed using one of four different views.
        /// </summary>
        public class DoubleBuffered : ListView
        {
            private const int WmEraseBkGnd = 0x14;

            /// <summary>
            ///     Initializes a new instance of the <see cref="ListView"/> class.
            /// </summary>
            public DoubleBuffered() =>
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.EnableNotifyMessage |
                         ControlStyles.OptimizedDoubleBuffer, true);

            /// <summary>
            ///     Notifies the control of Windows messages.
            /// </summary>
            /// <param name="m">
            ///     A <see cref="Message"/> that represents the Windows message.
            /// </param>
            protected override void OnNotifyMessage(Message m)
            {
                switch (m.Msg)
                {
                    case WmEraseBkGnd:
                        break;
                    default:
                        base.OnNotifyMessage(m);
                        break;
                }
            }
        }
    }
}
