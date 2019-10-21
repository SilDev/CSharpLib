#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TextBoxEx.cs
// Version:  2019-10-20 17:14
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Drawing;

    /// <summary>
    ///     Expands the functionality for the <see cref="TextBox"/> class.
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
            if (!(textBox is TextBox tb))
                return;
            var img = ImageEx.DefaultSearchSymbol;
            if (img == null)
                return;
            if (color == null)
                color = tb.ForeColor;
            if (color != Color.White)
                img = img.RecolorPixels(Color.White, (Color)color);
            var panel = new Panel
            {
                Anchor = tb.Anchor,
                BackColor = tb.BackColor,
                BorderStyle = tb.BorderStyle,
                Dock = tb.Dock,
                ForeColor = tb.ForeColor,
                Location = tb.Location,
                Margin = tb.Margin,
                MaximumSize = tb.MaximumSize,
                MinimumSize = tb.MinimumSize,
                Name = $"{tb.Name}Panel",
                Padding = new Padding(0, 0, 0, 0),
                Parent = tb.Parent,
                Size = tb.Size,
                TabIndex = tb.TabIndex
            };
            var pBox = new PictureBox
            {
                BackColor = tb.BackColor,
                BackgroundImage = img,
                BackgroundImageLayout = ImageLayout.Center,
                Cursor = Cursors.IBeam,
                Dock = DockStyle.Right,
                ForeColor = tb.ForeColor,
                Name = $"{tb.Name}PictureBox",
                Size = new Size(16, 16)
            };
            pBox.Click += (sender, e) => tb.Select();
            panel.Controls.Add(pBox);
            tb.Parent = panel;
            tb.BorderStyle = BorderStyle.None;
            tb.Dock = DockStyle.Fill;
            if (!panel.Parent.LayoutIsSuspended())
                panel.Parent.Update();
        }

        /// <summary>
        ///     Displays the vertical scroll bar as needed.
        ///     <para>
        ///         Hint: This function has no effect if word wrap is disabled.
        ///     </para>
        /// </summary>
        /// <param name="textBox">
        ///     The <see cref="TextBox"/> control to change.
        /// </param>
        public static void AutoVerticalScrollBar(this TextBox textBox)
        {
            if (!(textBox is TextBox tb))
                return;
            tb.SizeChanged -= SetVerticalScrollBars;
            tb.SizeChanged += SetVerticalScrollBars;
            tb.TextChanged -= SetVerticalScrollBars;
            tb.TextChanged += SetVerticalScrollBars;
            if (!tb.Parent.LayoutIsSuspended())
                SetVerticalScrollBars(tb, EventArgs.Empty);
        }

        private static void SetVerticalScrollBars(object sender, EventArgs e)
        {
            if (!(sender is TextBox tb) || !tb.Enabled || !tb.Visible || !tb.WordWrap || tb.Width < SystemInformation.VerticalScrollBarWidth * 2 || tb.Width < SystemInformation.HorizontalScrollBarHeight * 2)
                return;
            var rect = TextRenderer.MeasureText(tb.Text, tb.Font, new Size(tb.Width, int.MaxValue), TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
            tb.ScrollBars = rect.Height > tb.Height - 8 ? ScrollBars.Vertical : ScrollBars.None;
        }
    }
}
