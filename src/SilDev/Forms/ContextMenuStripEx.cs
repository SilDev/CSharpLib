#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ContextMenuStripEx.cs
// Version:  2016-10-24 15:58
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="ContextMenuStrip"/> class.
    /// </summary>
    public static class ContextMenuStripEx
    {
        /// <summary>
        ///     Represents the method that is used for the <see cref="ContextMenuStrip"/> paint <see cref="EventHandler"/>
        ///     which redraws the menu control with a similar border style, which is known from
        ///     <see cref="BorderStyle.FixedSingle"/>.
        /// </summary>
        /// <param name="contextMenuStrip">
        ///     The <see cref="ContextMenuStrip"/> to redraw.
        /// </param>
        /// <param name="paintEventArgs">
        ///     The paint event data.
        /// </param>
        /// <param name="borderColor">
        ///     THe border color.
        /// </param>
        public static void SetFixedSingle(this ContextMenuStrip contextMenuStrip, PaintEventArgs paintEventArgs, Color? borderColor = null)
        {
            try
            {
                using (var gp = new GraphicsPath())
                {
                    contextMenuStrip.Region = new Region(new RectangleF(2, 2, contextMenuStrip.Width - 4, contextMenuStrip.Height - 4));
                    gp.AddRectangle(new RectangleF(2, 2, contextMenuStrip.Width - 5, contextMenuStrip.Height - 5));
                    using (Brush b = new SolidBrush(contextMenuStrip.BackColor))
                        paintEventArgs.Graphics.FillPath(b, gp);
                    using (var p = new Pen(borderColor ?? SystemColors.ControlDark, 1))
                        paintEventArgs.Graphics.DrawPath(p, gp);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }
}
