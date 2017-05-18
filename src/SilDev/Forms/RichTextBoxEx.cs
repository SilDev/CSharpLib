#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: RichTextBoxEx.cs
// Version:  2017-05-18 14:21
// 
// Copyright (c) 2017, Si13n7 Developments (r)
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
                    throw new ArgumentNullException(nameof(text));
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
            var cms = new ContextMenuStrip
            {
                AutoSize = true,
                RenderMode = ToolStripRenderMode.System,
                ShowImageMargin = false
            };
            cms.AddToolStripItem(new ToolStripMenuItem("Cut"), richTextBox.Cut);
            cms.AddToolStripItem(new ToolStripMenuItem("Copy"), richTextBox.Copy);
            cms.AddToolStripItem(new ToolStripMenuItem("Paste"), richTextBox.Paste);
            cms.AddToolStripItem(new ToolStripMenuItem("Select All"), richTextBox.SelectAll);
            cms.Items.Add(new ToolStripSeparator());
            cms.AddToolStripItem(new ToolStripMenuItem("Load File"), LoadTextFile, richTextBox, owner);
            cms.AddToolStripItem(new ToolStripMenuItem("Save All"), SaveTextFile, richTextBox, owner);
            cms.Items.Add(new ToolStripSeparator());
            cms.AddToolStripItem(new ToolStripMenuItem("Undo"), richTextBox.Undo);
            richTextBox.ContextMenuStrip = cms;
        }

        private static void AddToolStripItem(this ToolStrip toolStrip, ToolStripItem toolStripItem, FileDialogHandler action, Control control, IWin32Window owner = null)
        {
            toolStripItem.Click += (s, e) => action(control, owner);
            toolStrip.Items.Add(toolStripItem);
        }

        private static void AddToolStripItem(this ToolStrip toolStrip, ToolStripItem toolStripItem, Action action)
        {
            toolStripItem.Click += (s, e) => action();
            toolStrip.Items.Add(toolStripItem);
        }

        private static void LoadTextFile(Control control, IWin32Window owner = null)
        {
            try
            {
                using (var dialog = new OpenFileDialog())
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        control.Text = File.ReadAllText(dialog.FileName);
                        MessageBoxEx.Show(owner, "File successfully loaded!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                        MessageBoxEx.Show(owner, "Canceled!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private static void SaveTextFile(Control control, IWin32Window owner = null)
        {
            try
            {
                if (string.IsNullOrEmpty(control.Text))
                    throw new ArgumentException("This field is empty!");
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = @"Text File|*.txt";
                    dialog.FileName = $"{Path.GetFileNameWithoutExtension(PathEx.LocalPath)} {DateTime.Now:yyyy-MM-dd HH.mm.ss}.txt";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(dialog.FileName, TextEx.FormatNewLine(control.Text));
                        MessageBoxEx.Show(owner, "File successfully saved!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                        MessageBoxEx.Show(owner, "Canceled!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBoxEx.Show(owner, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private delegate void FileDialogHandler(Control control, IWin32Window owner = null);
    }
}
