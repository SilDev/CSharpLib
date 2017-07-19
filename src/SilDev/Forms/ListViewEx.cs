#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ListViewEx.cs
// Version:  2017-07-19 04:50
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="ListView"/> class.
    /// </summary>
    public static class ListViewEx
    {
        /// <summary>
        ///     Retrives the <see cref="ListViewItem"/> at the current cursor's position.
        /// </summary>
        /// <param name="listView">
        ///     The <see cref="ListView"/> control to check.
        /// </param>
        public static ListViewItem ItemFromPoint(this ListView listView)
        {
            try
            {
                var pos = listView.PointToClient(Cursor.Position);
                return listView.GetItemAt(pos.X, pos.Y);
            }
            catch
            {
                return null;
            }
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
        public static void SetMouseOverCursor(this ListView listView, Cursor cursor = default(Cursor)) =>
            WinApi.NativeHelper.SendMessage(listView.Handle, 0x103e, IntPtr.Zero, (cursor ?? Cursors.Arrow).Handle);

        /// <summary>
        ///     Represents a Windows list view control, which displays a collection of items that
        ///     can be displayed using one of four different views.
        /// </summary>
        public class DoubleBuffered : ListView
        {
            protected const int WmEraseBkGnd = 0x14;

            /// <summary>
            ///     Initializes a new instance of the <see cref="ListView"/> class.
            /// </summary>
            public DoubleBuffered() =>
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.EnableNotifyMessage |
                         ControlStyles.OptimizedDoubleBuffer, true);

            protected override void OnNotifyMessage(Message m)
            {
                if (m.Msg != WmEraseBkGnd)
                    base.OnNotifyMessage(m);
            }
        }

        /// <summary>
        ///     Provides a base class for comparison.
        /// </summary>
        public class AlphanumericComparer : IComparer
        {
            private readonly bool _d;

            /// <summary>
            ///     Initilazies a new instance of the <see cref="AlphanumericComparer"/> class. A
            ///     parameter specifies whether the order is descended.
            /// </summary>
            /// <param name="descendent">
            ///     true to enable the descending order; otherwise, false.
            /// </param>
            public AlphanumericComparer(bool descendent = false)
            {
                _d = descendent;
            }

            /// <summary>
            ///     Compare two specified objects and returns an integer that indicates their
            ///     relative position in the sort order.
            /// </summary>
            /// <param name="a">
            ///     The first object to compare.
            /// </param>
            /// <param name="b">
            ///     The second object to compare.
            /// </param>
            public int Compare(object a, object b)
            {
                if (!(a is ListViewItem) || !(b is ListViewItem))
                    return 0;
                var s1 = ((ListViewItem)a).Text;
                var s2 = ((ListViewItem)b).Text;
                return new Comparison.AlphanumericComparer(_d).Compare(s1, s2);
            }
        }
    }
}
