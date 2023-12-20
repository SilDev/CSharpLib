#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ControlEx.cs
// Version:  2023-12-20 00:28
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using Drawing;
    using Properties;
    using static System.Windows.Forms.ListViewItem;
    using static WinApi;

    /// <summary>
    ///     Specifies the border style for a control.
    /// </summary>
    public enum ControlExBorderStyle
    {
        /// <summary>
        ///     A dotted border.
        /// </summary>
        Dotted = 1,

        /// <summary>
        ///     A dashed border.
        /// </summary>
        Dashed = 2,

        /// <summary>
        ///     A solid border.
        /// </summary>
        Solid = 3
    }

    /// <summary>
    ///     Specifies the color mode to apply for a control.
    /// </summary>
    public enum ControlExColorMode
    {
        /// <summary>
        ///     Increases the brightness.
        /// </summary>
        Light = 0,

        /// <summary>
        ///     Decreases the brightness.
        /// </summary>
        Dark = 1,

        /// <summary>
        ///     Increases the brightness a little more.
        /// </summary>
        LightLight = 2,

        /// <summary>
        ///     Decreases the brightness a little more.
        /// </summary>
        DarkDark = 3,

        /// <summary>
        ///     Extremely increases brightness.
        /// </summary>
        LightLightLight = 4,

        /// <summary>
        ///     Extremely decreases brightness.
        /// </summary>
        DarkDarkDark = 5,

        /// <summary>
        ///     Inverts the three RGB component (red, green, blue) values.
        /// </summary>
        InvertRgb = 6,

        /// <summary>
        ///     Inherit from a second <see cref="ControlExColorMode"/> option.
        /// </summary>
        Inherit = 7,

        /// <summary>
        ///     If possible, try to use the dark mode system back color; otherwise
        ///     <see cref="DarkDark"/> is used.
        /// </summary>
        SystemDark = 8
    }

    /// <summary>
    ///     Expands the functionality for the <see cref="Control"/> class.
    /// </summary>
    public static class ControlEx
    {
        /// <summary>
        ///     Gets the ancestor of this <see cref="Control"/>.
        /// </summary>
        /// <param name="control">
        ///     The control to query.
        /// </param>
        public static Control GetAncestor(this Control control)
        {
            if (control is not { } c)
                return default;
            var cur = c;
            while (cur.Parent != null)
                cur = cur.Parent;
            return cur;
        }

        /// <summary>
        ///     Determines whether the layout logic for this control has been temporarily
        ///     suspended.
        /// </summary>
        /// <param name="control">
        ///     The control to check.
        /// </param>
        public static bool LayoutIsSuspended(this Control control)
        {
            if (control is not { } c)
                return false;
            try
            {
                var fi = typeof(Control).GetField("layoutSuspendCount", BindingFlags.Instance | BindingFlags.NonPublic);
                return ((byte?)fi?.GetValue(c) ?? 0) > 0;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Changes the three RGB component values (Red, Green, Blue) of the
        ///     <see cref="Control.ForeColor"/> and <see cref="Control.BackColor"/>
        ///     properties inside all nested controls of the specified control.
        ///     <para>
        ///         Please note that it is not enough to change the colors of all control
        ///         elements to achieve a full color mode. To do this you would have to
        ///         create inherited classes that override important methods, or handle
        ///         draw events in which you would have to handle every little thing so
        ///         that it doesn't look crappy.
        ///     </para>
        /// </summary>
        /// <param name="control">
        ///     The control to change all colors.
        /// </param>
        /// <param name="foreMode">
        ///     The <see cref="Control.ForeColor"/> mode to apply.
        /// </param>
        /// <param name="backMode">
        ///     The <see cref="Control.BackColor"/> mode to apply.
        /// </param>
        /// <param name="changeNested">
        ///     <see langword="true"/> to change the color of the specified control and all
        ///     its nested controls; otherwise, <see langword="false"/> to change the color
        ///     only of the specified control.
        /// </param>
        public static void ChangeColorMode(this Control control, ControlExColorMode backMode = ControlExColorMode.SystemDark, ControlExColorMode foreMode = ControlExColorMode.Inherit, bool changeNested = true)
        {
            if (control == default)
                return;
            if (backMode == ControlExColorMode.Inherit)
                backMode = Inherit(foreMode);
            if (foreMode == ControlExColorMode.Inherit)
                foreMode = Inherit(backMode);
            var isSuspended = control.LayoutIsSuspended();
            if (!isSuspended)
                control.SuspendLayout();
            var queue = new Queue<Control>();
            queue.Enqueue(control);
            do
            {
                var parent = queue.Dequeue();
                parent.ForeColor = GetColor(parent.ForeColor, foreMode, false);
                parent.BackColor = GetColor(parent.BackColor, backMode, true);
                if (!changeNested)
                    break;
                switch (parent)
                {
                    case ContextMenuStrip cms:
                        foreach (var item in cms.Items.Cast<ToolStripItem>())
                        {
                            item.ForeColor = GetColor(item.ForeColor, foreMode, false);
                            item.BackColor = GetColor(item.BackColor, backMode, true);
                        }
                        break;
                    case Button { FlatStyle: FlatStyle.Flat } button:
                        button.FlatAppearance.BorderColor = GetColor(button.FlatAppearance.BorderColor, foreMode, false);
                        button.FlatAppearance.CheckedBackColor = GetColor(button.FlatAppearance.CheckedBackColor, backMode, true);
                        button.FlatAppearance.MouseDownBackColor = GetColor(button.FlatAppearance.MouseDownBackColor, backMode, true);
                        button.FlatAppearance.MouseDownBackColor = GetColor(button.FlatAppearance.MouseDownBackColor, backMode, true);
                        break;
                    case DataGridView { EnableHeadersVisualStyles: false } dgv:
                        dgv.BackgroundColor = GetColor(dgv.BackgroundColor, backMode, true);
                        dgv.ForeColor = GetColor(dgv.GridColor, foreMode, false);
                        dgv.GridColor = GetColor(dgv.GridColor, foreMode, false).EnsureDark();
                        dgv.ColumnHeadersDefaultCellStyle.BackColor = GetColor(dgv.ColumnHeadersDefaultCellStyle.BackColor, backMode, true);
                        dgv.ColumnHeadersDefaultCellStyle.ForeColor = GetColor(dgv.ColumnHeadersDefaultCellStyle.ForeColor, foreMode, false);
                        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = GetColor(dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor, backMode, true);
                        dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = GetColor(dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor, foreMode, false);
                        dgv.RowHeadersDefaultCellStyle.BackColor = GetColor(dgv.RowHeadersDefaultCellStyle.BackColor, backMode, true);
                        dgv.RowHeadersDefaultCellStyle.ForeColor = GetColor(dgv.RowHeadersDefaultCellStyle.ForeColor, foreMode, false);
                        dgv.RowHeadersDefaultCellStyle.SelectionBackColor = GetColor(dgv.RowHeadersDefaultCellStyle.SelectionBackColor, backMode, true);
                        dgv.RowHeadersDefaultCellStyle.SelectionForeColor = GetColor(dgv.RowHeadersDefaultCellStyle.SelectionForeColor, foreMode, false);
                        dgv.RowsDefaultCellStyle.BackColor = GetColor(dgv.RowsDefaultCellStyle.BackColor, backMode, true);
                        dgv.RowsDefaultCellStyle.ForeColor = GetColor(dgv.RowsDefaultCellStyle.ForeColor, foreMode, false);
                        dgv.RowsDefaultCellStyle.SelectionBackColor = GetColor(dgv.RowsDefaultCellStyle.SelectionBackColor, backMode, true);
                        dgv.RowsDefaultCellStyle.SelectionForeColor = GetColor(dgv.RowsDefaultCellStyle.SelectionForeColor, foreMode, false);
                        dgv.DefaultCellStyle.BackColor = GetColor(dgv.DefaultCellStyle.BackColor, backMode, true);
                        dgv.DefaultCellStyle.ForeColor = GetColor(dgv.DefaultCellStyle.ForeColor, foreMode, false);
                        dgv.DefaultCellStyle.SelectionBackColor = GetColor(dgv.DefaultCellStyle.SelectionBackColor, backMode, true);
                        dgv.DefaultCellStyle.SelectionForeColor = GetColor(dgv.DefaultCellStyle.SelectionForeColor, foreMode, false);
                        break;
                    case ListView listView:
                        foreach (var item in listView.Items.Cast<ListViewItem>())
                        {
                            item.ForeColor = GetColor(item.ForeColor, foreMode, false);
                            item.BackColor = GetColor(item.BackColor, backMode, true);
                            foreach (var subItem in item.SubItems.Cast<ListViewSubItem>())
                            {
                                subItem.ForeColor = GetColor(subItem.ForeColor, foreMode, false);
                                subItem.BackColor = GetColor(subItem.BackColor, backMode, true);
                            }
                        }
                        break;
                    case RichTextBox richTextBox:
                        richTextBox.SelectionColor = GetColor(richTextBox.SelectionColor, foreMode, false);
                        richTextBox.SelectionBackColor = GetColor(richTextBox.SelectionBackColor, backMode, true);
                        break;
                    case TabControl tabControl:
                        foreach (var tabPage in tabControl.TabPages.Cast<Control>())
                            queue.Enqueue(tabPage);
                        break;
                }
                if (parent.ContextMenuStrip is Control c)
                    queue.Enqueue(c);
                foreach (var child in parent.Controls.Cast<Control>())
                    queue.Enqueue(child);
            }
            while (queue.Count > 0);
            if (!isSuspended)
                control.ResumeLayout(true);

            static ControlExColorMode Inherit(ControlExColorMode mode) =>
                mode switch
                {
                    ControlExColorMode.Light => ControlExColorMode.Dark,
                    ControlExColorMode.Dark => ControlExColorMode.Light,
                    ControlExColorMode.LightLight => ControlExColorMode.DarkDark,
                    ControlExColorMode.DarkDark => ControlExColorMode.LightLight,
                    ControlExColorMode.LightLightLight => ControlExColorMode.DarkDarkDark,
                    ControlExColorMode.DarkDarkDark => ControlExColorMode.LightLightLight,
                    ControlExColorMode.SystemDark => ControlExColorMode.LightLightLight,
                    _ => ControlExColorMode.InvertRgb
                };

            static Color GetColor(Color color, ControlExColorMode mode, bool isBack)
            {
                if (color == default || color == Color.Empty || color == Color.Transparent)
                    return color;
                if (isBack && mode == ControlExColorMode.SystemDark)
                {
                    var backColor = Color.FromArgb(0x20, 0x20, 0x20);
                    var newColor = color.EnsureDarkDark();
                    return newColor.IsInRange(backColor, 16) ? backColor : newColor;
                }
                return mode switch
                {
                    ControlExColorMode.Light => color.EnsureLight(),
                    ControlExColorMode.Dark => color.EnsureDark(),
                    ControlExColorMode.LightLight => color.EnsureLightLight(),
                    ControlExColorMode.DarkDark => color.EnsureDarkDark(),
                    ControlExColorMode.LightLightLight => color.EnsureLightLightLight(),
                    ControlExColorMode.DarkDarkDark => color.EnsureDarkDarkDark(),
                    ControlExColorMode.SystemDark => color.EnsureLightLight(),
                    _ => color.InvertRgb()
                };
            }
        }

        /// <inheritdoc cref="ChangeColorMode(Control, ControlExColorMode, ControlExColorMode, bool)"/>
        public static void ChangeColorMode(this Control control, ControlExColorMode backMode, bool changeNested) =>
            control.ChangeColorMode(backMode, ControlExColorMode.Inherit, changeNested);

        /// <inheritdoc cref="ChangeColorMode(Control, ControlExColorMode, bool)"/>
        public static void ChangeColorMode(this Control control, bool changeNested) =>
            control.ChangeColorMode(ControlExColorMode.SystemDark, ControlExColorMode.Inherit, changeNested);

        /// <summary>
        ///     Enable dark mode for the specified control.
        ///     <para>
        ///         &#9888; Please note that this feature requires at least the Windows 10
        ///         October 2018 Update.
        ///     </para>
        /// </summary>
        /// <param name="control">
        ///     The control to change all colors.
        /// </param>
        public static void EnableDarkMode(this Control control)
        {
            if (control == default || !EnvironmentEx.IsAtLeastWindows(10, 17763))
                return;
            var queue = new Queue<Control>();
            queue.Enqueue(control);
            do
            {
                var parent = queue.Dequeue();
                if (parent is Button { FlatStyle: not FlatStyle.System })
                    continue;
                Desktop.EnableDarkMode(parent.Handle);
                foreach (Control child in parent.Controls)
                    queue.Enqueue(child);
            }
            while (queue.Count > 0);
        }

        /// <summary>
        ///     Enables a window to be dragged by a mouse with its left button down over
        ///     this <see cref="Control"/>.
        /// </summary>
        /// <param name="control">
        ///     The control to change.
        /// </param>
        /// <param name="cursor">
        ///     <see langword="true"/> to change <see cref="Control"/>.Cursor to
        ///     <see cref="Cursors.SizeAll"/> while dragging; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static void EnableDragMove(Control control, bool cursor = true)
        {
            if (control is not { } c)
                return;
            c.MouseDown += OnMouseDown;

            void OnMouseDown(object sender, MouseEventArgs e)
            {
                if (sender is not Control owner || e is not { Button: MouseButtons.Left })
                    return;
                var ancestor = owner.GetAncestor();
                if (ancestor == null)
                    return;
                var curCursor = owner.Cursor;
                if (cursor)
                    owner.Cursor = Cursors.SizeAll;
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(ancestor.Handle, 0xa1, new IntPtr(0x2), IntPtr.Zero);
                if (owner.Cursor != curCursor)
                    owner.Cursor = curCursor;
            }
        }

        /// <summary>
        ///     Enables or disables the double buffering for this <see cref="Control"/>,
        ///     even it is not directly supported.
        /// </summary>
        /// <param name="control">
        ///     The control to change.
        /// </param>
        /// <param name="enable">
        ///     <see langword="true"/> to enable double buffering; otherwise,
        ///     <see langword="false"/> to disable double buffering.
        /// </param>
        public static void SetDoubleBuffer(this Control control, bool enable = true)
        {
            if (control is not { } c)
                return;
            try
            {
                var pi = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
                pi?.SetValue(c, enable, null);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            var style = (int)NativeHelper.SendMessage(c.Handle, 0x1037u, IntPtr.Zero, IntPtr.Zero);
            var flags = new[]
            {
                0x8000,
                0x10000
            };
            foreach (var flag in flags)
            {
                if (enable)
                {
                    style |= flag;
                    continue;
                }
                style &= ~flag;
            }
            NativeHelper.SendMessage(c.Handle, 0x1036u, IntPtr.Zero, new IntPtr(style));
        }

        /// <summary>
        ///     Enables or disables the specified <see cref="ControlStyles"/> for this
        ///     <see cref="Control"/>, even it is not directly supported.
        /// </summary>
        /// <param name="control">
        ///     The control to change.
        /// </param>
        /// <param name="controlStyles">
        ///     The new styles to enable or disable.
        /// </param>
        /// <param name="enable">
        ///     <see langword="true"/> to enable the specified styles; otherwise,
        ///     <see langword="false"/> to disable the specified styles.
        /// </param>
        public static void SetControlStyle(this Control control, ControlStyles controlStyles, bool enable = true)
        {
            if (control is not { } c)
                return;
            try
            {
                var mi = typeof(Control).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
                mi?.Invoke(c, new object[] { controlStyles, enable });
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Set the border style for the entire tree.
        /// </summary>
        /// <typeparam name="TControl">
        ///     The type of the <see cref="Control"/> to be changed.
        /// </typeparam>
        /// <param name="control">
        ///     The control with the child controls to change.
        /// </param>
        /// <param name="borderStyle">
        ///     The style to set.
        /// </param>
        public static void SetBorderStyleOfType<TControl>(Control control, BorderStyle borderStyle) where TControl : Control
        {
            var queue = new Queue<Control>();
            queue.Enqueue(control);
            do
            {
                var parent = queue.Dequeue();
                foreach (var child in parent.Controls.OfType<Control>())
                    queue.Enqueue(child);
                if (parent is not TControl obj)
                    continue;
                try
                {
                    ((dynamic)obj).BorderStyle = borderStyle;
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    if (Log.DebugMode > 1)
                        Log.Write(ex);
                }
            }
            while (queue.Count > 0);
        }

        /// <summary>
        ///     Sets a value indicating whether all child controls of this control are
        ///     displayed.
        /// </summary>
        /// <param name="control">
        ///     The control with the child controls to change.
        /// </param>
        /// <param name="visibility">
        ///     <see langword="true"/> if all child controls are displayed; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="excludes">
        ///     Child controls that remain visible.
        /// </param>
        public static void SetChildVisibility(this Control control, bool visibility, params Control[] excludes)
        {
            if (control is not { } c)
                return;
            var ctrls = c.Controls.OfType<Control>().Where(x => !excludes.Contains(x));
            _ = ctrls.Aggregate(visibility, (r, x) => x.Visible = r);
        }

        /// <summary>
        ///     Draws a border with the specified color and style on a control.
        /// </summary>
        /// <param name="control">
        ///     The <see cref="Control"/> to draw on.
        /// </param>
        /// <param name="color">
        ///     The <see cref="Color"/> of the border.
        /// </param>
        /// <param name="style">
        ///     One of the <see cref="ControlExBorderStyle"/> values that specifies the
        ///     style of the border.
        /// </param>
        public static void DrawBorder(Control control, Color color, ControlExBorderStyle style = ControlExBorderStyle.Solid)
        {
            if (control is not { } c)
                return;
            c.Paint += OnPaint;
            c.Resize += OnResize;

            void OnPaint(object sender, PaintEventArgs e)
            {
                if (sender is not Control owner || e == null)
                    return;
                ControlPaint.DrawBorder(e.Graphics, owner.ClientRectangle, color, (ButtonBorderStyle)style);
            }

            static void OnResize(object sender, EventArgs e)
            {
                if (e == null)
                    return;
                var ancestor = (sender as Control)?.GetAncestor();
                if (ancestor == null || ancestor.LayoutIsSuspended())
                    return;
                ancestor.Invalidate();
            }
        }

        /// <summary>
        ///     Draws a 12px large size grip <see cref="Image"/> in this
        ///     <see cref="Control"/>.
        /// </summary>
        /// <param name="control">
        ///     The control that receives the size grip <see cref="Image"/>.
        /// </param>
        /// <param name="color">
        ///     The color for the size grip <see cref="Image"/>; <see cref="Color.White"/>
        ///     is used by default.
        /// </param>
        /// <param name="mouseDownEvent">
        ///     Occurs when the mouse pointer is over the control and a mouse button is
        ///     pressed.
        /// </param>
        /// <param name="mouseEnterEvent">
        ///     Occurs when the mouse pointer enters the control.
        /// </param>
        public static void DrawSizeGrip(Control control, Color? color = null, MouseEventHandler mouseDownEvent = null, EventHandler mouseEnterEvent = null)
        {
            if (control is not { } c || Resources.SizeGripImage is not Image i)
                return;
            if (color.HasValue && color != Color.White)
                i = i.RecolorPixels(Color.White, (Color)color);
            var pb = new PictureBoxNonClickable(mouseDownEvent != null && mouseEnterEvent != null)
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                BackColor = Color.Transparent,
                BackgroundImage = i,
                BackgroundImageLayout = ImageLayout.Center,
                Location = new Point(c.Right - 12, c.Bottom - 12),
                Size = new Size(12, 12)
            };
            if (mouseDownEvent != null)
                pb.MouseDown += mouseDownEvent;
            if (mouseEnterEvent != null)
                pb.MouseEnter += mouseEnterEvent;
            c.Controls.Add(pb);
            if (!c.LayoutIsSuspended())
                c.Update();
        }
    }
}
