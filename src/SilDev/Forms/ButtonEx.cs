#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ButtonEx.cs
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
        ///         which is required for highlight effects.
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
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Represents the method that is used for the <see cref="Button"/> click <see cref="EventHandler"/>
        ///     which determines whether the split area of this <see cref="Button"/> is clicked which opens the
        ///     specified <see cref="ContextMenuStrip"/> control.
        /// </summary>
        /// <param name="button">
        ///     The button which contains a splitted area, created by <see cref="Split(Button, Color?)"/>.
        /// </param>
        /// <param name="contextMenuStrip">
        ///     The drop down menu that opens for the splitted area.
        /// </param>
        public static bool Split_ClickEvent(this Button button, ContextMenuStrip contextMenuStrip)
        {
            if (button.PointToClient(Cursor.Position).X < button.Width - 16)
                return false;
            contextMenuStrip.Show(button, new Point(0, button.Height), ToolStripDropDownDirection.BelowRight);
            return true;
        }

        /// <summary>
        ///     Represents the method that is used for the <see cref="Button"/> mouse move <see cref="EventHandler"/>
        ///     which handles the different highlighting for both areas.
        /// </summary>
        /// <param name="button">
        ///     The button which contains a splitted area, created by <see cref="Split(Button, Color?)"/>.
        /// </param>
        /// <param name="backColor">
        ///     The background color, <see cref="Button"/>.BackColor is used by default.
        /// </param>
        /// <param name="hoverColor">
        ///     The highlight color, <see cref="Button"/>.FlatAppearance.MouseOverBackColor is used by default.
        /// </param>
        public static void Split_MouseMoveEvent(this Button button, Color? backColor = null, Color? hoverColor = null)
        {
            Split_MouseLeaveEvent(button);
            try
            {
                if (backColor == null)
                    backColor = button.BackColor;
                if (hoverColor == null)
                    hoverColor = button.FlatAppearance.MouseOverBackColor;
                if (button.PointToClient(Cursor.Position).X >= button.Width - 16)
                {
                    if (button.BackgroundImage != null)
                        return;
                    button.BackgroundImage = new Bitmap(button.Width - 16 - button.FlatAppearance.BorderSize, button.Height);
                    using (var g = Graphics.FromImage(button.BackgroundImage))
                        using (Brush b = new SolidBrush((Color)backColor))
                            g.FillRectangle(b, 0, 0, button.BackgroundImage.Width, button.BackgroundImage.Height);
                }
                else
                {
                    button.BackgroundImage = new Bitmap(button.Width, button.Height);
                    using (var g = Graphics.FromImage(button.BackgroundImage))
                    {
                        using (Brush b = new SolidBrush((Color)backColor))
                            g.FillRectangle(b, 0, 0, button.BackgroundImage.Width, button.BackgroundImage.Height);
                        using (Brush b = new SolidBrush((Color)hoverColor))
                            g.FillRectangle(b, 0, 0, button.BackgroundImage.Width - 15 - button.FlatAppearance.BorderSize, button.BackgroundImage.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Represents the method that is used for the <see cref="Button"/> mouse leave <see cref="EventHandler"/>
        ///     which is absolutly required for the functionality of
        ///     the <see cref="Split_MouseMoveEvent(Button, Color?, Color?)"/> function.
        /// </summary>
        /// <param name="button">
        ///     The button which contains a splitted area, created by <see cref="Split(Button, Color?)"/>.
        /// </param>
        public static void Split_MouseLeaveEvent(this Button button)
        {
            if (button.BackgroundImage != null)
                button.BackgroundImage = null;
        }
    }
}
