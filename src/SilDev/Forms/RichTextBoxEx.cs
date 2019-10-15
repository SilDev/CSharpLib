#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: RichTextBoxEx.cs
// Version:  2019-10-15 11:07
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="RichTextBox"/> class.
    /// </summary>
    public static class RichTextBoxEx
    {
        /// <summary>
        ///     Marks the specified text in this <see cref="RichTextBox"/> control.
        /// </summary>
        /// <param name="richTextBox">
        ///     The <see cref="RichTextBox"/> control to change.
        /// </param>
        /// <param name="text">
        ///     The text to mark.
        /// </param>
        /// <param name="foreColor">
        ///     The new foreground color.
        /// </param>
        /// <param name="backColor">
        ///     The new background color.
        /// </param>
        /// <param name="font">
        ///     The new font.
        /// </param>
        public static void MarkText(this RichTextBox richTextBox, string text, Color foreColor, Color? backColor = null, Font font = null)
        {
            if (!(richTextBox is RichTextBox rtb) || string.IsNullOrWhiteSpace(text))
                return;
            var selection = new Point(rtb.SelectionStart, rtb.SelectionLength);
            int start, startIndex = 0, end = rtb.Text.Length - 1, length = text.Length;
            while ((start = rtb.Text.IndexOf(text, startIndex, StringComparison.Ordinal)).IsBetween(0, end))
            {
                rtb.Select(start, length);
                rtb.SelectionColor = foreColor;
                if (backColor != null)
                    rtb.SelectionBackColor = (Color)backColor;
                if (font != null)
                    rtb.SelectionFont = font;
                startIndex = start + length;
            }
            if (selection.X >= 0)
                rtb.SelectionStart = selection.X;
            if (selection.Y >= 0)
                rtb.SelectionLength = selection.Y;
            rtb.SelectionBackColor = rtb.BackColor;
            rtb.SelectionColor = rtb.ForeColor;
            rtb.SelectionFont = rtb.Font;
        }

        /// <summary>
        ///     Marks the text depending on two specified keywords in this <see cref="RichTextBox"/> control.
        /// </summary>
        /// <param name="richTextBox">
        ///     The <see cref="RichTextBox"/> control to change.
        /// </param>
        /// <param name="startKeyword">
        ///     The start keyword for the text to mark.
        /// </param>
        /// <param name="endKeyword">
        ///     The end keyword for the text to mark.
        /// </param>
        /// <param name="foreColor">
        ///     The new foreground color.
        /// </param>
        /// <param name="backColor">
        ///     The new background color.
        /// </param>
        /// <param name="font">
        ///     The new font.
        /// </param>
        public static void MarkLine(this RichTextBox richTextBox, string startKeyword, string endKeyword, Color foreColor, Color? backColor = null, Font font = null)
        {
            if (!(richTextBox is RichTextBox rtb) || string.IsNullOrWhiteSpace(startKeyword))
                return;
            var selection = new Point(rtb.SelectionStart, rtb.SelectionLength);
            var lines = rtb.Lines;
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var length = line.Length;
                if (length < 1 || !line.StartsWith(startKeyword))
                    continue;
                var start = rtb.GetFirstCharIndexFromLine(i);
                if (start < 0)
                    continue;
                rtb.Select(start, length);
                rtb.SelectionColor = string.IsNullOrEmpty(endKeyword) || line.EndsWith(endKeyword) ? foreColor : Color.Red;
                if (backColor != null)
                    rtb.SelectionBackColor = (Color)backColor;
                if (font != null)
                    rtb.SelectionFont = font;
            }
            if (selection.X >= 0)
                rtb.SelectionStart = selection.X;
            if (selection.Y >= 0)
                rtb.SelectionLength = selection.Y;
        }

        /// <summary>
        ///     Marks the specified text in this <see cref="RichTextBox"/> control.
        /// </summary>
        /// <param name="richTextBox">
        ///     The <see cref="RichTextBox"/> control to change.
        /// </param>
        /// <param name="keyword">
        ///     The text to mark.
        /// </param>
        /// <param name="count">
        ///     The number of keywords to be marked.
        /// </param>
        /// <param name="foreColor">
        ///     The new foreground color.
        /// </param>
        /// <param name="backColor">
        ///     The new background color.
        /// </param>
        /// <param name="font">
        ///     The new font.
        /// </param>
        public static void MarkInLine(this RichTextBox richTextBox, string keyword, int count, Color foreColor, Color? backColor = null, Font font = null)
        {
            if (!(richTextBox is RichTextBox rtb) || string.IsNullOrWhiteSpace(keyword))
                return;
            var selection = new Point(rtb.SelectionStart, rtb.SelectionLength);
            var lines = rtb.Lines;
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var length = line.Length;
                if (length < 1 || !line.ContainsEx(keyword))
                    continue;
                var index = rtb.GetFirstCharIndexFromLine(i);
                if (index < 0)
                    continue;
                var num = count < 1 ? 1 : count;
                int start;
                var startIndex = 0;
                while (num > 0 && (start = line.IndexOf(keyword, startIndex, StringComparison.Ordinal)) > -1)
                {
                    index = rtb.GetFirstCharIndexFromLine(i) + start;
                    if (index < 0 || length < keyword.Length)
                        continue;
                    rtb.Select(index, keyword.Length);
                    rtb.SelectionColor = !line.StartsWith(keyword) ? foreColor : Color.Red;
                    if (backColor != null)
                        rtb.SelectionBackColor = (Color)backColor;
                    if (font != null)
                        rtb.SelectionFont = font;
                    startIndex = start + keyword.Length;
                    num--;
                }
            }
            if (selection.X >= 0)
                rtb.SelectionStart = selection.X;
            if (selection.Y >= 0)
                rtb.SelectionLength = selection.Y;
        }

        /// <summary>
        ///     Sets a default <see cref="ContextMenuStrip"/> to this <see cref="RichTextBox"/> with cut,
        ///     copy, paste, select all, load file, save file and undo.
        /// </summary>
        /// <param name="richTextBox">
        ///     The <see cref="RichTextBox"/> control to add the <see cref="ContextMenuStrip"/>.
        /// </param>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own modal dialog boxes.
        /// </param>
        public static void SetDefaultContextMenuStrip(this RichTextBox richTextBox, IWin32Window owner = null)
        {
            if (!(richTextBox is RichTextBox rtb))
                return;
            var cms = new ContextMenuStrip
            {
                AutoSize = true,
                RenderMode = ToolStripRenderMode.System,
                ShowImageMargin = false
            };
            cms.AddToolStripItem(new ToolStripMenuItem("Cut"), rtb.Cut);
            cms.AddToolStripItem(new ToolStripMenuItem("Copy"), rtb.Copy);
            cms.AddToolStripItem(new ToolStripMenuItem("Paste"), rtb.Paste);
            cms.AddToolStripItem(new ToolStripMenuItem("Select All"), rtb.SelectAll);
            cms.Items.Add(new ToolStripSeparator());
            cms.AddToolStripItem(new ToolStripMenuItem("Load File"), LoadTextFile, rtb, owner);
            cms.AddToolStripItem(new ToolStripMenuItem("Save All"), SaveTextFile, rtb, owner);
            cms.Items.Add(new ToolStripSeparator());
            cms.AddToolStripItem(new ToolStripMenuItem("Undo"), rtb.Undo);
            rtb.ContextMenuStrip = cms;
        }

        private static void AddToolStripItem(this IDisposable toolStrip, ToolStripItem toolStripItem, FileDialogHandler action, Control control, IWin32Window owner = null)
        {
            if (!(toolStripItem is ToolStripItem tsi) || !(toolStrip is ToolStrip ts))
                return;
            tsi.Click += (s, e) => action(control, owner);
            ts.Items.Add(toolStripItem);
        }

        private static void AddToolStripItem(this IDisposable toolStrip, ToolStripItem toolStripItem, Action action)
        {
            if (!(toolStripItem is ToolStripItem tsi) || !(toolStrip is ToolStrip ts))
                return;
            tsi.Click += (s, e) => action();
            ts.Items.Add(toolStripItem);
        }

        private static void LoadTextFile(Control control, IWin32Window owner = null)
        {
            if (!(control is Control c))
                return;
            using (var dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        c.Text = File.ReadAllText(dialog.FileName);
                        MessageBoxEx.Show(owner, "File successfully loaded!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    catch (Exception ex)
                    {
                        MessageBoxEx.Show(owner, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return;
                }
                MessageBoxEx.Show(owner, "Canceled!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private static void SaveTextFile(Control control, IWin32Window owner = null)
        {
            if (!(control is Control c))
                return;
            if (string.IsNullOrEmpty(c.Text))
            {
                MessageBoxEx.Show(owner, "The text can not be empty!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = @"Text Files|*.txt";
                dialog.FileName = $"{Path.GetFileNameWithoutExtension(PathEx.LocalPath)} {DateTime.Now:yyyy-MM-dd HH.mm.ss}.txt";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(dialog.FileName, TextEx.FormatNewLine(c.Text));
                        MessageBoxEx.Show(owner, "File successfully saved!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    catch (Exception ex)
                    {
                        MessageBoxEx.Show(owner, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return;
                }
                MessageBoxEx.Show(owner, "Canceled!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private delegate void FileDialogHandler(Control control, IWin32Window owner = null);
    }
}
