#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: SizeEx.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Drawing
{
    using System;
    using System.Drawing;

    /// <summary>
    ///     Expands the functionality for the <see cref="Size"/> class.
    /// </summary>
    public static class SizeEx
    {
        /// <summary>
        ///     Scales the specified width or height dimension based on the specified DPI
        ///     values.
        /// </summary>
        /// <param name="value">
        ///     The width or height dimension.
        /// </param>
        /// <param name="oldDpi">
        ///     The old resolution.
        /// </param>
        /// <param name="newDpi">
        ///     The new resolution.
        /// </param>
        public static int ScaleDimension(int value, float oldDpi, float newDpi)
        {
            var a = Math.Floor(oldDpi);
            var b = Math.Floor(newDpi);
            if (Math.Abs(a - b) < 1d)
                return value;
            return (int)Math.Floor(b / a * value);
        }

        /// <summary>
        ///     Scales the specified width and height dimensions based on the specified DPI
        ///     values.
        /// </summary>
        /// <param name="width">
        ///     The width dimension.
        /// </param>
        /// <param name="height">
        ///     The height dimension.
        /// </param>
        /// <param name="oldDpi">
        ///     The old resolution.
        /// </param>
        /// <param name="newDpi">
        ///     The new resolution.
        /// </param>
        public static Size ScaleDimensions(int width, int height, float oldDpi, float newDpi)
        {
            var size = Size.Empty;
            if (width > 0)
                size.Width = ScaleDimension(width, oldDpi, newDpi);
            if (height > 0)
                size.Height = ScaleDimension(height, oldDpi, newDpi);
            return size;
        }

        /// <summary>
        ///     Scales the specified width and height dimensions based on the specified
        ///     handle to a window.
        /// </summary>
        /// <param name="width">
        ///     The width dimension.
        /// </param>
        /// <param name="height">
        ///     The height dimension.
        /// </param>
        /// <param name="hWnd">
        ///     Handle to a window.
        ///     <para>
        ///         If this value is set to default, the handle of the current desktop will
        ///         be used.
        ///     </para>
        /// </param>
        public static Size ScaleDimensions(int width, int height, IntPtr hWnd = default)
        {
            var handle = hWnd == default ? WinApi.NativeMethods.GetDesktopWindow() : hWnd;
            using var graphics = Graphics.FromHwnd(handle);
            return ScaleDimensions(width, height, 96f, Math.Max(graphics.DpiX, graphics.DpiY));
        }

        /// <summary>
        ///     Scales the width and height dimensions of this <see cref="Size"/> object
        ///     based on the specified DPI values.
        /// </summary>
        /// <param name="size">
        ///     This <see cref="Size"/> object.
        /// </param>
        /// <param name="oldDpi">
        ///     The old resolution.
        /// </param>
        /// <param name="newDpi">
        ///     The new resolution.
        /// </param>
        public static Size ScaleDimensions(this Size size, float oldDpi, float newDpi) =>
            ScaleDimensions(size.Width, size.Height, oldDpi, newDpi);

        /// <summary>
        ///     Scales the width and height dimensions of this <see cref="Size"/> object
        ///     based on the specified handle to a window.
        /// </summary>
        /// <param name="size">
        ///     This <see cref="Size"/> object.
        /// </param>
        /// <param name="hWnd">
        ///     Handle to a window.
        ///     <para>
        ///         If this value is set to default, the handle of the current desktop will
        ///         be used.
        ///     </para>
        /// </param>
        public static Size ScaleDimensions(this Size size, IntPtr hWnd = default) =>
            ScaleDimensions(size.Width, size.Height, hWnd);
    }
}
