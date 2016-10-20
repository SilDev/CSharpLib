#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Forms.cs
// Version:  2016-10-18 23:33
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Reflection;
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

    /// <summary>
    ///     Expands the functionality for the <see cref="Control"/> class.
    /// </summary>
    public static class ControlEx
    {
        /// <summary>
        ///     Enables or disables the specified <see cref="ControlStyles"/> for this <see cref="Control"/>, even it
        ///     is not directly supported.
        /// </summary>
        /// <param name="control">
        ///     The control to change.
        /// </param>
        /// <param name="controlStyles">
        ///     The new styles to enable or disable.
        /// </param>
        /// <param name="enable">
        ///     true to enable the specified styles; otherwise, false to disable the specified styles.
        /// </param>
        public static void SetControlStyle(this Control control, ControlStyles controlStyles, bool enable = true)
        {
            try
            {
                var method = typeof(Control).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
                method.Invoke(control, new object[] { controlStyles, enable });
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Draws a 12px large size grip <see cref="Image"/> in this <see cref="Control"/>.
        /// </summary>
        /// <param name="control">
        ///     The control that receives the size grip <see cref="Image"/>.
        /// </param>
        /// <param name="color">
        ///     The color for the size grip <see cref="Image"/>, <see cref="Color.White"/> is used by default.
        /// </param>
        public static void DrawSizeGrip(Control control, Color? color = null)
        {
            try
            {
                var img = ("89504e470d0a1a0a0000000d494844520000000c0000000c08" +
                           "0600000056755ce70000000467414d410000b18f0bfc610500" +
                           "0000097048597300000b1100000b11017f645f910000000774" +
                           "494d4507e00908102912b9edb66f0000003d494441542853ad" +
                           "8b0b0a00200c4277ff4b5b0c8410b78a121ef8c10070852d3b" +
                           "6c2950997574509975dc62cb09a5fedfa1640d54e7df0e47d8" +
                           "b2063100852bc6484e7044a50000000049454e44ae426082").FromHexStringToImage();
                if (img == null)
                    return;
                if (color != null && color != Color.White)
                    img = img.RecolorPixels(Color.White, (Color)color);
                control.BackgroundImage = img;
                control.BackgroundImageLayout = ImageLayout.Center;
                control.Size = new Size(12, 12);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }

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

    /// <summary>
    ///     Expands the functionality for the <see cref="LinkLabelEx"/> class.
    /// </summary>
    public static class LinkLabelEx
    {
        /// <summary>
        ///     Creates a link for the specified text and associates it with the specified link.
        /// </summary>
        /// <param name="linkLabel">
        ///     The <see cref="LinkLabel"/> control to change.
        /// </param>
        /// <param name="text">
        ///     The text to link.
        /// </param>
        /// <param name="uri">
        ///     The link to associate.
        /// </param>
        public static void LinkText(this LinkLabel linkLabel, string text, Uri uri)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                    throw new ArgumentNullException();
                var start = 0;
                int index;
                while ((index = linkLabel.Text.IndexOf(text, start, StringComparison.Ordinal)) > -1)
                {
                    linkLabel.Links.Add(index, text.Length, uri);
                    start = index + text.Length;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Creates a link for the specified text and associates it with the specified link.
        /// </summary>
        /// <param name="linkLabel">
        ///     The <see cref="LinkLabel"/> control to change.
        /// </param>
        /// <param name="text">
        ///     The text to link.
        /// </param>
        /// <param name="uri">
        ///     The link to associate.
        /// </param>
        public static void LinkText(this LinkLabel linkLabel, string text, string uri)
        {
            try
            {
                LinkText(linkLabel, text, new Uri(uri));
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }

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

    /// <summary>
    ///     Expands the functionality for the <see cref="ProgressBarEx"/> class.
    /// </summary>
    public static class ProgressBarEx
    {
        /// <summary>
        ///     Skips the very long animation and jumps directly to the <see cref="ProgressBar.Maximum"/>.
        /// </summary>
        /// <param name="progressBar">
        ///     The progress to change.
        /// </param>
        public static void JumpToEnd(this ProgressBar progressBar)
        {
            try
            {
                var maximum = progressBar.Maximum;
                progressBar.Maximum = int.MaxValue;
                progressBar.Value = progressBar.Maximum;
                progressBar.Value--;
                progressBar.Maximum = maximum;
                progressBar.Value = progressBar.Maximum;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }

    /// <summary>
    ///     Expands the functionality for the <see cref="RichTextBoxEx"/> class.
    /// </summary>
    public static class RichTextBoxEx
    {
        /// <summary>
        ///     Marks the specified text on this <see cref="RichTextBox"/> control.
        /// </summary>
        /// <param name="richTextBox">
        ///     The <see cref="RichTextBox"/> control to change.
        /// </param>
        /// <param name="text">
        ///     The text to mark.
        /// </param>
        /// <param name="foreColor">
        ///     The new forground color.
        /// </param>
        /// <param name="backColor">
        ///     The new background color.
        /// </param>
        /// <param name="font">
        ///     The new font.
        /// </param>
        public static void MarkText(this RichTextBox richTextBox, string text, Color foreColor, Color? backColor = null, Font font = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                    throw new ArgumentNullException();
                var selected = new Point(richTextBox.SelectionStart, richTextBox.SelectionLength);
                var startIndex = 0;
                int start;
                while ((start = richTextBox.Text.IndexOf(text, startIndex, StringComparison.Ordinal)) > -1)
                {
                    richTextBox.Select(start, text.Length);
                    richTextBox.SelectionColor = foreColor;
                    if (backColor != null)
                        richTextBox.SelectionBackColor = (Color)backColor;
                    if (font != null)
                        richTextBox.SelectionFont = font;
                    startIndex = start + text.Length;
                }
                richTextBox.SelectionStart = selected.X;
                richTextBox.SelectionLength = selected.Y;
                richTextBox.SelectionBackColor = richTextBox.BackColor;
                richTextBox.SelectionColor = richTextBox.ForeColor;
                richTextBox.SelectionFont = richTextBox.Font;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }

    /// <summary>
    ///     Expands the functionality for the <see cref="TextBoxEx"/> class.
    /// </summary>
    public static class TextBoxEx
    {
        /// <summary>
        ///     Draws a search symbol on the right side of this <see cref="TextBox"/>.
        /// </summary>
        /// <param name="textBox">
        ///     The <see cref="TextBox"/> control to change.
        /// </param>
        /// <param name="color">
        ///     The search symbol color, <see cref="TextBox"/>.ForeColor is used by default.
        /// </param>
        public static void DrawSearchSymbol(this TextBox textBox, Color? color = null)
        {
            try
            {
                var img = Depiction.DefaultSearchSymbol;
                if (img == null)
                    return;
                if (color == null)
                    color = textBox.ForeColor;
                if (color != Color.White)
                    img = img.RecolorPixels(Color.White, (Color)color);
                var panel = new Panel
                {
                    Anchor = textBox.Anchor,
                    BackColor = textBox.BackColor,
                    BorderStyle = textBox.BorderStyle,
                    Dock = textBox.Dock,
                    ForeColor = textBox.ForeColor,
                    Location = textBox.Location,
                    Name = $"{textBox.Name}Panel",
                    Parent = textBox.Parent,
                    Size = textBox.Size,
                    TabIndex = textBox.TabIndex
                };
                var pictureBox = new PictureBox
                {
                    BackColor = textBox.BackColor,
                    BackgroundImage = img,
                    BackgroundImageLayout = ImageLayout.Center,
                    Cursor = Cursors.IBeam,
                    Dock = DockStyle.Right,
                    ForeColor = textBox.ForeColor,
                    Name = $"{textBox.Name}PictureBox",
                    Size = new Size(16, 16)
                };
                pictureBox.Click += (sender, e) => textBox.Select();
                panel.Controls.Add(pictureBox);
                textBox.BorderStyle = BorderStyle.None;
                textBox.Dock = DockStyle.Fill;
                textBox.Parent = panel;
                panel.Parent.Update();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }
}
