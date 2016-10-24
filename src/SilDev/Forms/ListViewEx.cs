#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ListViewEx.cs
// Version:  2016-10-24 15:58
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System.Collections;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="ListViewEx"/> class.
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
