#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ButtonEx.cs
// Version:  2017-10-21 13:51
// 
// Copyright (c) 2017, Si13n7 Developments (r)
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
            try
            {
                if (button.Width < 48 || button.Height < 16)
                    throw new NotSupportedException();
                if (button.FlatStyle != FlatStyle.Flat)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.MouseOverBackColor = SystemColors.Highlight;
                }
                button.Image = new Bitmap(12, button.Height);
                button.ImageAlign = ContentAlignment.MiddleRight;
                using (var gr = Graphics.FromImage(button.Image))
                {
                    var pen = new Pen(buttonText ?? SystemColors.ControlText, 1);
                    gr.DrawLine(pen, 0, 0, 0, button.Image.Height - 3);
                    var size = new Size(button.Image.Width - 6, button.Image.Height - 12);
                    gr.DrawLine(pen, size.Width, size.Height, size.Width + 5, size.Height);
                    gr.DrawLine(pen, size.Width + 1, size.Height + 1, size.Width + 4, size.Height + 1);
                    gr.DrawLine(pen, size.Width + 2, size.Height + 2, size.Width + 3, size.Height + 2);
                }
                button.MouseMove += Split_MouseMove;
                button.MouseLeave += Split_MouseLeave;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
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
            if (button.PointToClient(Cursor.Position).X < button.Right - 16)
                return false;
            contextMenuStrip.Show(button, new Point(0, button.Height), ToolStripDropDownDirection.BelowRight);
            return true;
        }

        private static void Split_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(sender is Button button))
                return;
            Split_MouseLeave(button, null);
            try
            {
                if (button.PointToClient(Cursor.Position).X >= button.Right - 16)
                {
                    if (button.BackgroundImage != null)
                        return;
                    button.BackgroundImage = new Bitmap(button.Width, button.Height);
                    var w = button.Right - 16 - button.FlatAppearance.BorderSize;
                    using (var g = Graphics.FromImage(button.BackgroundImage))
                        using (Brush b = new SolidBrush(button.BackColor))
                            g.FillRectangle(b, 0, 0, w, button.BackgroundImage.Height);
                }
                else
                {
                    button.BackgroundImage = new Bitmap(button.Width, button.Height);
                    var x = button.BackgroundImage.Width - 15 - button.FlatAppearance.BorderSize;
                    var w = 15 + button.FlatAppearance.BorderSize;
                    using (var g = Graphics.FromImage(button.BackgroundImage))
                        using (Brush b = new SolidBrush(button.BackColor))
                            g.FillRectangle(b, x, 0, w, button.BackgroundImage.Height);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private static void Split_MouseLeave(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.BackgroundImage == null)
                return;
            button.BackgroundImage.Dispose();
            button.BackgroundImage = null;
        }
    }
}
