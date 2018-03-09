#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TextBoxEx.cs
// Version:  2018-03-08 01:19
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
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
                Name = $"{tb.Name}Panel",
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
            tb.BorderStyle = BorderStyle.None;
            tb.Dock = DockStyle.Fill;
            tb.Parent = panel;
            panel.Parent.Update();
        }
    }
}
