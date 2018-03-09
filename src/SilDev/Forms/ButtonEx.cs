#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ButtonEx.cs
// Version:  2018-03-08 01:18
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="Button"/> class.
    /// </summary>
    public static class ButtonEx
    {
        /// <summary>
        ///     <para>
        ///         Creates a small split button on the right side of this <see cref="Button"/> which is
        ///         mostly used for drop down menu controls.
        ///     </para>
        ///     <para>
        ///         Please note that the <see cref="FlatStyle"/> is overwritten to <see cref="FlatStyle.Flat"/>
        ///         which is required to apply highlight effects.
        ///     </para>
        /// </summary>
        /// <param name="button">
        ///     The button to split.
        /// </param>
        /// <param name="buttonText">
        ///     The button text color, <see cref="SystemColors.ControlText"/> is used by default.
        /// </param>
        public static void Split(this Button button, Color? buttonText = null)
        {
            if (!(button is Button b))
                return;
            if (b.Width < 48 || b.Height < 16)
                throw new NotSupportedException();
            if (b.FlatStyle != FlatStyle.Flat)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.MouseOverBackColor = SystemColors.Highlight;
            }
            b.Image = new Bitmap(12, b.Height);
            b.ImageAlign = ContentAlignment.MiddleRight;
            using (var g = Graphics.FromImage(b.Image))
            {
                var p = new Pen(buttonText ?? SystemColors.ControlText, 1);
                g.DrawLine(p, 0, 0, 0, b.Image.Height - 3);
                var s = new Size(b.Image.Width - 6, b.Image.Height - 12);
                g.DrawLine(p, s.Width, s.Height, s.Width + 5, s.Height);
                g.DrawLine(p, s.Width + 1, s.Height + 1, s.Width + 4, s.Height + 1);
                g.DrawLine(p, s.Width + 2, s.Height + 2, s.Width + 3, s.Height + 2);
            }
            b.MouseMove += Split_MouseMove;
            b.MouseLeave += Split_MouseLeave;
        }

        /// <summary>
        ///     Represents the method that is used for the <see cref="Button"/> click <see cref="EventHandler"/>
        ///     that determines whether the split area of this <see cref="Button"/>, which opens the
        ///     specified <see cref="ContextMenuStrip"/> control, is clicked.
        /// </summary>
        /// <param name="button">
        ///     The button that contains a splitted area, which is created by <see cref="Split(Button, Color?)"/>.
        /// </param>
        /// <param name="contextMenuStrip">
        ///     The drop down menu that opens for the splitted area.
        /// </param>
        public static bool SplitClickHandler(this Button button, ContextMenuStrip contextMenuStrip)
        {
            if (!(button is Button b) || !(contextMenuStrip is ContextMenuStrip cms) || b.PointToClient(Cursor.Position).X < b.Right - 16)
                return false;
            cms.Show(b, new Point(0, b.Height), ToolStripDropDownDirection.BelowRight);
            return true;
        }

        private static void Split_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(sender is Button b))
                return;
            Split_MouseLeave(b, null);
            if (b.PointToClient(Cursor.Position).X >= b.Right - 16)
            {
                if (b.BackgroundImage != null)
                    return;
                b.BackgroundImage = new Bitmap(b.Width, b.Height);
                var w = b.Right - 16 - b.FlatAppearance.BorderSize;
                using (var g = Graphics.FromImage(b.BackgroundImage))
                    using (var sb = new SolidBrush(b.BackColor))
                        g.FillRectangle(sb, 0, 0, w, b.BackgroundImage.Height);
            }
            else
            {
                b.BackgroundImage = new Bitmap(b.Width, b.Height);
                var x = b.BackgroundImage.Width - 15 - b.FlatAppearance.BorderSize;
                var w = 15 + b.FlatAppearance.BorderSize;
                using (var g = Graphics.FromImage(b.BackgroundImage))
                    using (var sb = new SolidBrush(b.BackColor))
                        g.FillRectangle(sb, x, 0, w, b.BackgroundImage.Height);
            }
        }

        private static void Split_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b?.BackgroundImage == null)
                return;
            b.BackgroundImage.Dispose();
            b.BackgroundImage = null;
        }
    }
}
