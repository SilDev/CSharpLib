#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ListViewDoubleBuffered.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System.Windows.Forms;

    /// <summary>
    ///     Represents a Windows list view control, which displays a collection of
    ///     items that can be displayed using one of four different views.
    /// </summary>
    public class ListViewDoubleBuffered : ListView
    {
        private const int WmEraseBkGnd = 0x14;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ListView"/> class.
        /// </summary>
        public ListViewDoubleBuffered() =>
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
