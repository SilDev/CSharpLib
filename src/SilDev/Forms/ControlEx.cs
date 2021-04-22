﻿#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ControlEx.cs
// Version:  2021-04-22 19:45
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
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
            if (!(control is { } c))
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
                WinApi.NativeMethods.ReleaseCapture();
                WinApi.NativeMethods.SendMessage(ancestor.Handle, 0xa1, new IntPtr(0x2), IntPtr.Zero);
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
            var style = (int)WinApi.NativeHelper.SendMessage(c.Handle, 0x1037u, IntPtr.Zero, IntPtr.Zero);
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
            WinApi.NativeHelper.SendMessage(c.Handle, 0x1036u, IntPtr.Zero, new IntPtr(style));
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
            while (queue.Any());
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
