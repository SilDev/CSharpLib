#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TextBoxEx.cs
// Version:  2016-10-24 15:57
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
