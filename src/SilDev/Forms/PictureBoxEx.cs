#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PictureBoxEx.cs
// Version:  2018-03-08 01:18
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="PictureBox"/> class.
    /// </summary>
    public static class PictureBoxEx
    {
        /// <summary>
        ///     Represents a Windows non-clickable picture box control for displaying an image.
        /// </summary>
        public class NonClickable : PictureBox
        {
            private const int HtTransparent = -0x1;
            private const int WmNcHitTest = 0x84;

            /// <summary>
            ///     Initializes a new instance of the <see cref="NonClickable"/> picture box class.
            /// </summary>
            /// <param name="clickable">
            ///     true to determine that the picture box is clickable; otherwise, false.
            /// </param>
            [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
            public NonClickable(bool clickable = false)
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
}
