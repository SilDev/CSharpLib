#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ControlEx.cs
// Version:  2017-10-21 13:53
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Properties;

    /// <summary>
    ///     Expands the functionality for the <see cref="Control"/> class.
    /// </summary>
    public static class ControlEx
    {
        /// <summary>
        ///     Specifies the border style for a control.
        /// </summary>
        public enum BorderStyle
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
        ///     Gets the ancestor of this <see cref="Control"/>.
        /// </summary>
        /// <param name="control">
        ///     The control to query.
        /// </param>
        public static Control GetAncestor(this Control control)
        {
            var c = control;
            while (c.Parent != null)
                c = c.Parent;
            return c;
        }

        /// <summary>
        ///     Enables a window to be dragged by a mouse with its left button down over this <see cref="Control"/>.
        /// </summary>
        /// <param name="control">
        ///     The control to change.
        /// </param>
        /// <param name="cursor">
        ///     true to change <see cref="Control"/>.Cursor to <see cref="Cursors.SizeAll"/> while dragging;
        ///     otherwise, false.
        /// </param>
        public static void EnableDragMove(this Control control, bool cursor = true)
        {
            control.MouseDown += (sender, args) =>
            {
                if (!(sender is Control c) || args == null || args.Button != MouseButtons.Left)
                    return;
                var cc = c.Cursor;
                if (cursor)
                    c.Cursor = Cursors.SizeAll;
                WinApi.NativeMethods.ReleaseCapture();
                WinApi.NativeMethods.SendMessage(c.GetAncestor().Handle, 0xa1, new IntPtr(0x2), IntPtr.Zero);
                if (c.Cursor != cc)
                    c.Cursor = cc;
            };
        }

        /// <summary>
        ///     Enables or disables the double buffering for this <see cref="Control"/>, even it is not directly
        ///     supported.
        /// </summary>
        /// <param name="control">
        ///     The control to change.
        /// </param>
        /// <param name="enable">
        ///     true to enable double buffering; otherwise, false to disable double buffering.
        /// </param>
        public static void SetDoubleBuffer(this Control control, bool enable = true)
        {
            try
            {
                var pi = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
                pi?.SetValue(control, enable, null);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            var style = (int)WinApi.NativeHelper.SendMessage(control.Handle, 0x1037u, IntPtr.Zero, IntPtr.Zero);
            if (enable)
            {
                style |= 0x8000;
                style |= 0x10000;
            }
            else
            {
                style &= ~0x8000;
                style &= ~0x10000;
            }
            WinApi.NativeHelper.SendMessage(control.Handle, 0x1036u, IntPtr.Zero, new IntPtr(style));
        }

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
                method?.Invoke(control, new object[] { controlStyles, enable });
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
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
        ///     One of the <see cref="BorderStyle"/> values that specifies the style of the border.
        /// </param>
        public static void DrawBorder(this Control control, Color color, BorderStyle style = BorderStyle.Solid)
        {
            control.Paint += (sender, args) =>
            {
                if (!(sender is Control c) || args == null)
                    return;
                ControlPaint.DrawBorder(args.Graphics, c.ClientRectangle, color, (ButtonBorderStyle)style);
            };
            control.Resize += (sender, args) =>
            {
                var c = (sender as Control)?.GetAncestor();
                if (c == null || args == null)
                    return;
                c.Invalidate();
            };
        }

        /// <summary>
        ///     Draws a 12px large size grip <see cref="Image"/> in this <see cref="Control"/>.
        /// </summary>
        /// <param name="control">
        ///     The control that receives the size grip <see cref="Image"/>.
        /// </param>
        /// <param name="color">
        ///     The color for the size grip <see cref="Image"/>; <see cref="Color.White"/> is used by default.
        /// </param>
        /// <param name="mouseDownEvent">
        ///     Occurs when the mouse pointer is over the control and a mouse button is pressed.
        /// </param>
        /// <param name="mouseEnterEvent">
        ///     Occurs when the mouse pointer enters the control.
        /// </param>
        public static void DrawSizeGrip(Control control, Color? color = null, MouseEventHandler mouseDownEvent = null, EventHandler mouseEnterEvent = null)
        {
            try
            {
                Image img = Resources.SizeGripImage;
                if (img == null)
                    return;
                if (color != null && color != Color.White)
                    img = img.RecolorPixels(Color.White, (Color)color);
                var pb = new PictureBox
                {
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                    BackColor = Color.Transparent,
                    BackgroundImage = img,
                    BackgroundImageLayout = ImageLayout.Center,
                    Location = new Point(control.Right - 12, control.Bottom - 12),
                    Size = new Size(12, 12)
                };
                if (mouseDownEvent != null)
                    pb.MouseDown += mouseDownEvent;
                else
                {
                    var c = pb.GetAncestor();
                    pb.MouseDown += (sender, args) =>
                    {
                        try
                        {
                            var point = new Point(c.Width - 1, c.Height - 1);
                            WinApi.NativeMethods.ClientToScreen(c.Handle, ref point);
                            WinApi.NativeMethods.SetCursorPos((uint)point.X, (uint)point.Y);
                            var mouseDown = new WinApi.DeviceInput();
                            mouseDown.Data.Mouse.Flags = 0x2;
                            mouseDown.Type = 0;
                            var mouseUp = new WinApi.DeviceInput();
                            mouseUp.Data.Mouse.Flags = 0x4;
                            mouseUp.Type = 0;
                            WinApi.DeviceInput[] inputs =
                            {
                                mouseUp,
                                mouseDown
                            };
                            WinApi.NativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(WinApi.DeviceInput)));
                        }
                        catch (Exception ex)
                        {
                            Log.Write(ex);
                        }
                    };
                }
                if (mouseEnterEvent != null)
                    pb.MouseEnter += mouseEnterEvent;
                else
                    pb.Cursor = Cursors.SizeNWSE;
                control.Controls.Add(pb);
                control.Update();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }
}
