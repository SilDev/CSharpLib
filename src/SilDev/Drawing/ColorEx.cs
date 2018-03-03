#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ColorEx.cs
// Version:  2018-03-02 21:08
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Drawing
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;

    /// <summary>
    ///     Expands the functionality for the <see cref="Color"/> class.
    /// </summary>
    public static class ColorEx
    {
        /// <summary>
        ///     Translates an HTML color representation to a GDI+ <see cref="Color"/> structure.
        /// </summary>
        /// <param name="htmlColor">
        ///     The string representation of the HTML color to translate.
        /// </param>
        /// <param name="defColor">
        ///     The color that is set if no HTML color was found.
        /// </param>
        /// <param name="alpha">
        ///     The alpha component. Valid values are 0 through 255.
        /// </param>
        public static Color FromHtmlToColor(this string htmlColor, Color defColor, byte? alpha = null)
        {
            try
            {
                var code = htmlColor?.TrimStart('#').ToUpper();
                if (string.IsNullOrEmpty(code))
                    throw new ArgumentNullException(nameof(htmlColor));
                if (!code.Length.IsBetween(1, 6) || code.Any(x => !"0123456789ABCDEF".Contains(x)))
                    throw new ArgumentOutOfRangeException(nameof(htmlColor));
                if (code.Length < 6)
                {
                    while (code.Length < 6)
                        code += code;
                    code = code.Substring(6);
                }
                var c = ColorTranslator.FromHtml($"#{code}");
                if (alpha != null)
                    c = Color.FromArgb((byte)alpha, c.R, c.G, c.B);
                return c;
            }
            catch
            {
                return defColor;
            }
        }

        /// <summary>
        ///     Inverts the three RGB component (red, green, blue) values of the specified
        ///     <see cref="Color"/> structure.
        /// </summary>
        /// <param name="color">
        ///     The color to invert.
        /// </param>
        /// <param name="alpha">
        ///     The alpha component. Valid values are 0 through 255.
        /// </param>
        public static Color InvertRgb(this Color color, byte? alpha = null) =>
            Color.FromArgb(alpha ?? color.A, (byte)~color.R, (byte)~color.G, (byte)~color.B);

        /// <summary>
        ///     Scales the three RGB component (red, green, blue) values of the specified
        ///     <see cref="Color"/> structure to gray.
        /// </summary>
        /// <param name="color">
        ///     The color to scale.
        /// </param>
        public static Color ToGrayScale(this Color color)
        {
            try
            {
                var c = color;
                int scale = (byte)(c.R * .3f + c.G * .59f + c.B * .11f);
                c = Color.FromArgb(c.A, scale, scale, scale);
                return c;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return Color.Empty;
            }
        }

        /// <summary>
        ///     Gets the average RGB component (red, green, blue) values from the specified
        ///     <see cref="Image"/>.
        /// </summary>
        /// <param name="image">
        ///     The input image.
        /// </param>
        /// <param name="disposeImage">
        ///     true to release all resources used by the specified <see cref="Image"/>;
        ///     otherwise false.
        /// </param>
        public static Color GetAverageColor(this Image image, bool disposeImage = false)
        {
            try
            {
                Color c;
                using (var bmp = new Bitmap(1, 1))
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        using (var b = new Bitmap(image))
                            g.DrawImage(b, 0, 0, 1, 1);
                    }
                    c = bmp.GetPixel(0, 0);
                }
                c = Color.FromArgb(c.R, c.G, c.B);
                if (disposeImage)
                    image.Dispose();
                return c;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return Color.Empty;
            }
        }
    }
}
