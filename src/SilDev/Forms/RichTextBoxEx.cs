#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: RichTextBoxEx.cs
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
}
