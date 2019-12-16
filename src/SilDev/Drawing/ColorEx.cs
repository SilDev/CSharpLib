#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ColorEx.cs
// Version:  2019-12-16 16:42
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Drawing
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using Investment;

    /// <summary>
    ///     Expands the functionality for the <see cref="Color"/> class.
    /// </summary>
    public static class ColorEx
    {
        private static RandomInvestor _randomInvestor;

        /// <summary>
        ///     Returns the range value between two colors based on their <see cref="Color.GetHue"/>
        ///     value.
        /// </summary>
        /// <param name="colorA">
        ///     The first color to compare.
        /// </param>
        /// <param name="colorB">
        ///     The second color to compare.
        /// </param>
        public static int RangeOf(this Color colorA, Color colorB)
        {
            var range = Math.Abs(colorA.GetHue() - colorB.GetHue());
            if (range > 180f)
                range = 360f - range;
            return (int)range;
        }

        /// <summary>
        ///     Determines whether this color is in range of the specified color.
        /// </summary>
        /// <param name="colorA">
        ///     The first color to compare.
        /// </param>
        /// <param name="colorB">
        ///     The second color to compare.
        /// </param>
        /// <param name="threshold">
        ///     The threshold.
        /// </param>
        public static bool IsInRange(this Color colorA, Color colorB, int threshold = 25) =>
            colorA.RangeOf(colorB) < threshold;

        /// <summary>
        ///     Determines whether this color is dark.
        /// </summary>
        /// <param name="color">
        ///     The color to check.
        /// </param>
        public static bool IsDark(this Color color) =>
            color.ToRgbArray().Sum() < byte.MaxValue * 3 / 2;

        /// <summary>
        ///     Determines whether this color is pretty dark.
        /// </summary>
        /// <param name="color">
        ///     The color to check.
        /// </param>
        public static bool IsDarkDark(this Color color) =>
            color.ToRgbArray().Sum() <= byte.MaxValue;

        /// <summary>
        ///     Determines whether this color is extremely dark.
        /// </summary>
        /// <param name="color">
        ///     The color to check.
        /// </param>
        public static bool IsDarkDarkDark(this Color color) =>
            color.ToRgbArray().Sum() <= sbyte.MaxValue;

        /// <summary>
        ///     Determines whether this color is light.
        /// </summary>
        /// <param name="color">
        ///     The color to check.
        /// </param>
        public static bool IsLight(this Color color) =>
            color.ToRgbArray().Sum() >= byte.MaxValue * 3 / 2;

        /// <summary>
        ///     Determines whether this color is pretty light.
        /// </summary>
        /// <param name="color">
        ///     The color to check.
        /// </param>
        public static bool IsLightLight(this Color color) =>
            color.ToRgbArray().Sum() >= byte.MaxValue * 2;

        /// <summary>
        ///     Determines whether this color is extremely light.
        /// </summary>
        /// <param name="color">
        ///     The color to check.
        /// </param>
        public static bool IsLightLightLight(this Color color) =>
            color.ToRgbArray().Sum() >= byte.MaxValue * 2 + sbyte.MaxValue;

        /// <summary>
        ///     Decreases the brightness of the specified color if it is too bright based on
        ///     <see cref="IsDark"/>.
        /// </summary>
        /// <param name="color">
        ///     The color to be adjusted.
        /// </param>
        public static Color EnsureDark(this Color color)
        {
            var current = color;
            while (!current.IsDark())
                current = ControlPaint.Dark(current);
            return current;
        }

        /// <summary>
        ///     Decreases the brightness of the specified color if it is too bright based on
        ///     <see cref="IsDarkDark"/>.
        /// </summary>
        /// <param name="color">
        ///     The color to be adjusted.
        /// </param>
        public static Color EnsureDarkDark(this Color color)
        {
            var current = color;
            while (!current.IsDarkDark())
                current = ControlPaint.Dark(current);
            return current;
        }

        /// <summary>
        ///     Decreases the brightness of the specified color if it is too bright based on
        ///     <see cref="IsDarkDarkDark"/>.
        /// </summary>
        /// <param name="color">
        ///     The color to be adjusted.
        /// </param>
        public static Color EnsureDarkDarkDark(this Color color)
        {
            var current = color;
            while (!current.IsDarkDarkDark())
                current = ControlPaint.Dark(current);
            return current;
        }

        /// <summary>
        ///     Increases the brightness of the specified color if it is too dark based on
        ///     <see cref="IsLight"/>.
        /// </summary>
        /// <param name="color">
        ///     The color to be adjusted.
        /// </param>
        public static Color EnsureLight(this Color color)
        {
            var current = color;
            while (!current.IsLight())
                current = ControlPaint.Light(current);
            return current;
        }

        /// <summary>
        ///     Increases the brightness of the specified color if it is too dark based on
        ///     <see cref="IsLightLight"/>.
        /// </summary>
        /// <param name="color">
        ///     The color to be adjusted.
        /// </param>
        public static Color EnsureLightLight(this Color color)
        {
            var current = color;
            while (!current.IsLightLight())
                current = ControlPaint.Light(current);
            return current;
        }

        /// <summary>
        ///     Increases the brightness of the specified color if it is too dark based on
        ///     <see cref="IsLightLightLight"/>.
        /// </summary>
        /// <param name="color">
        ///     The color to be adjusted.
        /// </param>
        public static Color EnsureLightLightLight(this Color color)
        {
            var current = color;
            while (!current.IsLightLightLight())
                current = ControlPaint.Light(current);
            return current;
        }

        /// <summary>
        ///     Creates a random color based on the specified seed.
        /// </summary>
        /// <param name="seed">
        /// </param>
        public static Color GetRandomColor(int seed = -1)
        {
            if (_randomInvestor == default(RandomInvestor))
                _randomInvestor = new RandomInvestor();
            var random = _randomInvestor.GetGenerator(seed);
            var buffer = new byte[3];
            random.NextBytes(buffer);
            return Color.FromArgb(buffer.First(), buffer.Second(), buffer.Last());
        }

        /// <summary>
        ///     Creates a random known system color based on the specified seed.
        /// </summary>
        /// <param name="seed">
        /// </param>
        public static Color GetRandomKnownColor(int seed = -1)
        {
            if (_randomInvestor == default(RandomInvestor))
                _randomInvestor = new RandomInvestor();
            var random = _randomInvestor.GetGenerator(seed);
            var names = Enum.GetValues(typeof(KnownColor)).Cast<KnownColor>().ToArray();
            return Color.FromKnownColor(names.Just(random.Next(names.Length)));
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
        public static Color FromHtml(string htmlColor, Color defColor = default)
        {
            try
            {
                var code = htmlColor?.TrimStart('#').ToUpperInvariant();
                if (string.IsNullOrEmpty(code))
                    throw new ArgumentNullException(nameof(htmlColor));
                if (!code.Length.IsBetween(1, 8) || code.Any(x => !"0123456789ABCDEF".Contains(x)))
                    throw new ArgumentOutOfRangeException(nameof(htmlColor));
                var sb = new StringBuilder();
                Switch:
                switch (code.Length)
                {
                    case 8:
                        return Color.FromArgb(int.Parse(code, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                    case 6:
                        return FromRgb(int.Parse(code, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                    case 3:
                        foreach (var c in code)
                        {
                            sb.Append(c);
                            sb.Append(c);
                        }
                        code = sb.ToStringThenClear();
                        goto Switch;
                    default:
                        while (sb.Length < 6)
                            sb.Append(code);
                        code = sb.ToStringThenClear(0, 6);
                        goto Switch;
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return defColor;
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
        public static Color FromHtml(string htmlColor, Color defColor, byte alpha)
        {
            var color = FromHtml(htmlColor, defColor);
            return color == default ? color : Color.FromArgb(alpha, color);
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
        public static Color FromHtml(string htmlColor, byte alpha) =>
            FromHtml(htmlColor, default, alpha);

        /// <summary>
        ///     Translates the specified 32-bit (A)RGB value to an HTML string color
        ///     representation.
        /// </summary>
        /// <param name="argb">
        ///     A value specifying the 32-bit (A)RGB value.
        /// </param>
        /// <param name="alpha">
        ///     true to translate also the alpha value; otherwise, false.
        /// </param>
        public static string ToHtml(uint argb, bool alpha = false) =>
            $"#{new string(argb.ToString("X", CultureInfo.InvariantCulture).TakeLast(alpha ? 8 : 6).ToArray()).PadLeft(alpha ? 8 : 6, '0')}";

        /// <summary>
        ///     Translates the specified 32-bit (A)RGB value to an HTML string color
        ///     representation.
        /// </summary>
        /// <param name="argb">
        ///     A value specifying the 32-bit (A)RGB value.
        /// </param>
        /// <param name="alpha">
        ///     true to translate also the alpha value; otherwise, false.
        /// </param>
        public static string ToHtml(long argb, bool alpha = false) =>
            ToHtml((uint)Math.Abs(argb), alpha);

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
        public static string ToHtml(Color color, bool alpha = false) =>
            color == default ? null : ToHtml(alpha ? color.ToArgb() : color.ToRgb(), alpha);

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
        public static string ToHtml(Color color, byte? alpha) =>
            color == default ? null : alpha.HasValue ? ToHtml(Color.FromArgb((int)alpha, color)) : ToHtml(color);

        /// <summary>
        ///     Creates a <see cref="Color"/> structure from a 32-bit RGB value.
        /// </summary>
        /// <param name="rgb">
        ///     A value specifying the 32-bit RGB value.
        /// </param>
        public static Color FromRgb(int rgb) =>
            Color.FromArgb(byte.MaxValue, (byte)((rgb & 0xff0000) >> 16), (byte)((rgb & 0xff00) >> 8), (byte)(rgb & 0xff));

        /// <summary>
        ///     Gets the 32-bit RGB value of this <see cref="Color"/> structure.
        /// </summary>
        /// <param name="color">
        ///     The <see cref="Color"/> structure to translate.
        /// </param>
        public static int ToRgb(this Color color) =>
            (int)(((color.R << 16) | (color.G << 8) | color.B | (0 << 24)) & 0xffffffL);

        /// <summary>
        ///     Copies the elements of the 32-bit ARGB value of this <see cref="Color"/> structure
        ///     to a new array.
        /// </summary>
        /// <param name="color">
        ///     The <see cref="Color"/> structure to translate.
        /// </param>
        public static int[] ToArgbArray(this Color color) =>
            new int[] { color.A, color.R, color.G, color.B };

        /// <summary>
        ///     Copies the elements of the 32-bit RGB value of this <see cref="Color"/> structure
        ///     to a new array.
        /// </summary>
        /// <param name="color">
        ///     The <see cref="Color"/> structure to translate.
        /// </param>
        public static int[] ToRgbArray(this Color color) =>
            new int[] { color.R, color.G, color.B };

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
            if (color == default)
                return color;
            var c = color;
            var scale = (byte)(c.R * .3f + c.G * .59f + c.B * .11f);
            c = Color.FromArgb(c.A, scale, scale, scale);
            return c;
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
