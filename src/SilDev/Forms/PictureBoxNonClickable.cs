#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PictureBoxNonClickable.cs
// Version:  2020-01-13 13:04
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Forms;

    /// <summary>
    ///     Represents a Windows non-click-able picture box control for displaying an
    ///     image.
    /// </summary>
    public class PictureBoxNonClickable : PictureBox
    {
        private const int HtTransparent = -0x1;
        private const int WmNcHitTest = 0x84;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PictureBoxNonClickable"/>
        ///     picture box class.
        /// </summary>
        /// <param name="clickable">
        ///     <see langword="true"/> to determine that the picture box is click-able;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public PictureBoxNonClickable(bool clickable = false)
        {
            if (clickable)
                return;
            if (Parent is IMouseEvent p)
                MouseClick += p.MouseClick;
        }

        /// <summary>
        ///     Processes Windows messages.
        /// </summary>
        /// <param name="m">
        ///     The Windows <see cref="Message"/> to process.
        /// </param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WmNcHitTest:
                    m.Result = new IntPtr(HtTransparent);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private interface IMouseEvent
        {
            void MouseClick(object sender, MouseEventArgs e);
        }
    }
}
