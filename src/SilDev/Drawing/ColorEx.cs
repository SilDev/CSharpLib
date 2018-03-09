#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ColorEx.cs
// Version:  2018-03-08 02:47
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
        public static Color FromHtmlToColor(this string htmlColor, Color defColor = default(Color))
        {
            try
            {
                var code = htmlColor?.TrimStart('#').ToUpper();
                if (string.IsNullOrEmpty(code))
                    throw new ArgumentNullException(nameof(htmlColor));
                if (!code.Length.IsBetween(1, 8) || !code.All(x => "0123456789ABCDEF".Contains(x)))
                    throw new ArgumentOutOfRangeException(nameof(htmlColor));
                string a, r, g, b;
                Switch:
                switch (code.Length)
                {
                    case 8:
                        a = code.Substring(0, 2);
                        r = code.Substring(2, 2);
                        g = code.Substring(4, 2);
                        b = code.Substring(6, 2);
                        break;
                    case 6:
                        a = byte.MaxValue.ToString("X2");
                        r = code.Substring(0, 2);
                        g = code.Substring(2, 2);
                        b = code.Substring(4, 2);
                        break;
                    case 3:
                        a = byte.MaxValue.ToString("X2");
                        r = code[0].ToString();
                        r += r;
                        g = code[1].ToString();
                        g += g;
                        b = code[2].ToString();
                        b += b;
                        break;
                    default:
                        while (code.Length < 6)
                            code += code;
                        code = code.Substring(6);
                        goto Switch;
                }
                var c = Color.FromArgb(Convert.ToInt32(a, 16), Convert.ToInt32(r, 16), Convert.ToInt32(g, 16), Convert.ToInt32(b, 16));
                return c;
            }
            catch
            {
                return defColor;
            }
        }

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
        public static Color FromHtmlToColor(this string htmlColor, Color defColor, byte alpha)
        {
            var c = htmlColor.FromHtmlToColor(defColor);
            if (c == default(Color))
                return c;
            c = Color.FromArgb(alpha, c.R, c.G, c.B);
            return c;
        }

        /// <summary>
        ///     Translates an HTML color representation to a GDI+ <see cref="Color"/> structure.
        /// </summary>
        /// <param name="htmlColor">
        ///     The string representation of the HTML color to translate.
        /// </param>
        /// <param name="alpha">
        ///     The alpha component. Valid values are 0 through 255.
        /// </param>
        public static Color FromHtmlToColor(this string htmlColor, byte alpha) =>
            htmlColor.FromHtmlToColor(default(Color), alpha);

        /// <summary>
        ///     Translates the specified <see cref="Color"/> structure to an HTML string color
        ///     representation.
        /// </summary>
        /// <param name="color">
        ///     The <see cref="Color"/> structure to translate.
        /// </param>
        /// <param name="alpha">
        ///     The alpha component. Valid values are 0 through 255.
        /// </param>
        public static string ToHtmlString(this Color color, byte? alpha = null)
        {
            var c = color;
            var s = string.Empty;
            if (c == default(Color))
                return s;
            if (alpha.HasValue)
                s += $"{alpha.Value:X2}";
            s = $"#{s}{c.R:X2}{c.G:X2}{c.B:X2}";
            return s;
        }

        /// <summary>
        ///     Translates the specified <see cref="Color"/> structure to an HTML string color
        ///     representation.
        /// </summary>
        /// <param name="color">
        ///     The <see cref="Color"/> structure to translate.
        /// </param>
        /// <param name="alpha">
        ///     true to translate also the alpha value; otherwise, false.
        /// </param>
        public static string ToHtmlString(this Color color, bool alpha)
        {
            var c = color;
            var a = c.A;
            return c.ToHtmlString(a);
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
                var scale = (byte)(c.R * .3f + c.G * .59f + c.B * .11f);
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
            if (!(image is Image img))
                return Color.Empty;
            Color c;
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    using (var b = new Bitmap(img))
                        g.DrawImage(b, 0, 0, 1, 1);
                }
                c = bmp.GetPixel(0, 0);
            }
            c = Color.FromArgb(c.R, c.G, c.B);
            if (disposeImage)
                img.Dispose();
            return c;
        }
    }
}
