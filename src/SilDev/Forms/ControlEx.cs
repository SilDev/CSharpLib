#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ControlEx.cs
// Version:  2016-10-24 18:19
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

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
                var c = sender as Control;
                if (c == null || args == null || args.Button != MouseButtons.Left)
                    return;
                var cc = c.Cursor;
                if (cursor)
                    c.Cursor = Cursors.SizeAll;
                WinApi.UnsafeNativeMethods.ReleaseCapture();
                WinApi.UnsafeNativeMethods.SendMessage(c.GetAncestor().Handle, 0xa1, new IntPtr(0x02), IntPtr.Zero);
                if (c.Cursor != cc)
                    c.Cursor = cc;
            };
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
}
