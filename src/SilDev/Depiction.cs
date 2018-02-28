#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Depiction.cs
// Version:  2018-02-28 04:57
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;
    using Properties;

    /// <summary>
    ///     Provides functionality for the <see cref="Color"/>, <see cref="Image"/> descended classes.
    /// </summary>
    public static class Depiction
    {
        private static readonly Dictionary<object, ImagePair> SwitcherCache = new Dictionary<object, ImagePair>();

        /// <summary>
        ///     Gets an <see cref="Image"/> object which consists of a semi-transparent black color.
        /// </summary>
        public static Image DimEmpty => Resources.DimEmptyImage;

        /// <summary>
        ///     Gets an <see cref="Image"/> object that contains a white 16px large search symbol.
        /// </summary>
        public static Image DefaultSearchSymbol => Resources.SearchImage;

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
        ///     Initilazies a new instance of the <see cref="Image"/> class with
        ///     the specified color as background.
        /// </summary>
        /// <param name="color">
        ///     The color to convert.
        /// </param>
        /// <param name="width">
        ///     The width of the <see cref="Image"/>.
        /// </param>
        /// <param name="height">
        ///     The height of the <see cref="Image"/>.
        /// </param>
        public static Image ToImage(this Color color, int width = 1, int height = 1)
        {
            try
            {
                var img = new Bitmap(width, height);
                using (var gr = Graphics.FromImage(img))
                    using (Brush b = new SolidBrush(color))
                        gr.FillRectangle(b, 0, 0, width, height);
                return img;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
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
                        g.DrawImage(new Bitmap(image), new Rectangle(0, 0, 1, 1));
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

        /// <summary>
        ///     Redraws the specified <see cref="Image"/> with the specified size and
        ///     with the specified rendering quality.
        /// </summary>
        /// <param name="image">
        ///     The image to draw.
        /// </param>
        /// <param name="width">
        ///     The new width of the image.
        /// </param>
        /// <param name="heigth">
        ///     The new heigth of the image.
        /// </param>
        /// <param name="quality">
        ///     The rendering quality for the image.
        /// </param>
        public static Image Redraw(this Image image, int width, int heigth, SmoothingMode quality = SmoothingMode.HighQuality)
        {
            try
            {
#if x86
                const int maxRange = 23049;
#else
                const int maxRange = 23170;
#endif
                if (!width.IsBetween(1, maxRange))
                    throw new ArgumentOutOfRangeException(nameof(width));
                if (!heigth.IsBetween(1, maxRange))
                    throw new ArgumentOutOfRangeException(nameof(heigth));
                var bmp = new Bitmap(width, heigth);
                bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (var gr = Graphics.FromImage(bmp))
                {
                    gr.CompositingMode = CompositingMode.SourceCopy;
                    switch (quality)
                    {
                        case SmoothingMode.AntiAlias:
                            gr.CompositingQuality = CompositingQuality.HighQuality;
                            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            gr.SmoothingMode = SmoothingMode.AntiAlias;
                            break;
                        case SmoothingMode.HighQuality:
                            gr.CompositingQuality = CompositingQuality.HighQuality;
                            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            gr.SmoothingMode = SmoothingMode.HighQuality;
                            break;
                        case SmoothingMode.HighSpeed:
                            gr.CompositingQuality = CompositingQuality.HighSpeed;
                            gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                            gr.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                            gr.SmoothingMode = SmoothingMode.HighSpeed;
                            break;
                    }
                    using (var attr = new ImageAttributes())
                    {
                        attr.SetWrapMode(WrapMode.TileFlipXY);
                        gr.DrawImage(image, new Rectangle(0, 0, width, heigth), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attr);
                    }
                }
                return bmp;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return image;
            }
        }

        /// <summary>
        ///     Redraws the specified <see cref="Image"/> with the specified maximum size
        ///     indicator and with the specified rendering quality.
        /// </summary>
        /// <param name="image">
        ///     The image to draw.
        /// </param>
        /// <param name="quality">
        ///     The rendering quality for the image.
        /// </param>
        /// <param name="indicator">
        ///     Specifies the maximal size indicator, which determines when the image
        ///     gets a new size.
        /// </param>
        public static Image Redraw(this Image image, SmoothingMode quality = SmoothingMode.HighQuality, int indicator = 1024)
        {
            int[] size =
            {
                image.Width,
                image.Height
            };
            if (indicator <= 0 || indicator >= size.First() && indicator >= size.Last())
                goto Return;
            for (var i = 0; i < size.Length; i++)
            {
                if (size[i] <= indicator)
                    continue;
                var percent = (int)Math.Floor(100f / size[i] * indicator);
                size[i] = (int)(size[i] * (percent / 100f));
                size[i == 0 ? 1 : 0] = (int)(size[i == 0 ? 1 : 0] * (percent / 100f));
                break;
            }
            Return:
            return image.Redraw(size[0], size[1], quality);
        }

        /// <summary>
        ///     Redraws the specified <see cref="Image"/> with the specified maximum size
        ///     indicator and with the highest available rendering quality.
        /// </summary>
        /// <param name="image">
        ///     The image to draw.
        /// </param>
        /// <param name="indicator">
        ///     Specifies the maximal size indicator, which determines when the image
        ///     gets a new size.
        /// </param>
        public static Image Redraw(this Image image, int indicator) =>
            image.Redraw(SmoothingMode.HighQuality, indicator);

        /// <summary>
        ///     Inverts the color matrix of the specified <see cref="Image"/>.
        /// </summary>
        /// <param name="image">
        ///     The image to convert.
        /// </param>
        public static Image InvertColors(this Image image)
        {
            try
            {
                var bmp = new Bitmap(image.Width, image.Height);
                using (var gr = Graphics.FromImage(bmp))
                {
                    var cm = new ColorMatrix(new[]
                    {
                        new[] { -1f, 00f, 00f, 00f, 00f },
                        new[] { 00f, -1f, 00f, 00f, 00f },
                        new[] { 00f, 00f, -1f, 00f, 00f },
                        new[] { 00f, 00f, 00f, 01f, 00f },
                        new[] { 01f, 01f, 01f, 00f, 01f }
                    });
                    using (var attr = new ImageAttributes())
                    {
                        attr.SetColorMatrix(cm);
                        gr.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attr);
                    }
                }
                return bmp;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return image;
            }
        }

        /// <summary>
        ///     Scales the color matrix of the specified <see cref="Image"/> to gray.
        /// </summary>
        /// <param name="image">
        ///     The image to scale.
        /// </param>
        public static Image ToGrayScale(this Image image)
        {
            try
            {
                var bmp = new Bitmap(image.Width, image.Height);
                using (var gr = Graphics.FromImage(bmp))
                {
                    var cm = new ColorMatrix(new[]
                    {
                        new[] { 0.30f, 0.30f, 0.30f, 0.00f, 0.00f },
                        new[] { 0.59f, 0.59f, 0.59f, 0.00f, 0.00f },
                        new[] { 0.11f, 0.11f, 0.11f, 0.00f, 0.00f },
                        new[] { 0.00f, 0.00f, 0.00f, 1.00f, 0.00f },
                        new[] { 0.00f, 0.00f, 0.00f, 0.00f, 1.00f }
                    });
                    using (var attr = new ImageAttributes())
                    {
                        attr.SetColorMatrix(cm);
                        gr.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attr);
                    }
                }
                return bmp;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return image;
            }
        }

        /// <summary>
        ///     Scales the color matrix of the specified <see cref="Image"/> to gray and switch
        ///     back to the original image the next time this function is called.
        /// </summary>
        /// <param name="image">
        ///     The image to switch.
        /// </param>
        /// <param name="key">
        ///     The key for the cache.
        /// </param>
        /// <param name="dispose">
        ///     true to dispose the cached <see cref="ImagePair"/>; otherwise, false.
        /// </param>
        public static Image SwitchGrayScale(this Image image, object key, bool dispose = false)
        {
            try
            {
                Image img;
                if (!SwitcherCache.ContainsKey(key))
                {
                    var imgPair = new ImagePair(image, image.ToGrayScale());
                    SwitcherCache.Add(key, imgPair);
                    img = imgPair.Image2;
                }
                else
                {
                    if (!dispose)
                        img = image == SwitcherCache[key].Image1 ? SwitcherCache[key].Image2 : SwitcherCache[key].Image1;
                    else
                    {
                        img = new Bitmap(SwitcherCache[key].Image1);
                        SwitcherCache[key].Dispose();
                        SwitcherCache.Remove(key);
                    }
                }
                return img;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return image;
            }
        }

        /// <summary>
        ///     Recolors the pixels of the specified <see cref="Image"/> using a specified
        ///     old color and a specified new color.
        /// </summary>
        /// <param name="image">
        ///     The image to change.
        /// </param>
        /// <param name="from">
        ///     The color of the pixel to be changed.
        /// </param>
        /// <param name="to">
        ///     The new color of the pixel.
        /// </param>
        public static Image RecolorPixels(this Image image, Color from, Color to)
        {
            try
            {
                var bmp = (Bitmap)image;
                for (var x = 0; x < image.Width; x++)
                    for (var y = 0; y < image.Height; y++)
                    {
                        var px = bmp.GetPixel(x, y);
                        if (Color.FromArgb(0, px.R, px.G, px.B) == Color.FromArgb(0, from.R, from.G, from.B))
                            bmp.SetPixel(x, y, Color.FromArgb(px.A, to.R, to.G, to.B));
                    }
                return bmp;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return image;
            }
        }

        /// <summary>
        ///     Gets the frames of the specified <see cref="Image"/>.
        /// </summary>
        /// <param name="image">
        ///     The image to get the frames.
        /// </param>
        /// <param name="disposeImage">
        ///     true to dispose the original image; otherwise, false.
        /// </param>
        public static List<Frame> GetFrames(this Image image, bool disposeImage = true)
        {
            try
            {
                var imgList = new List<Frame>();
                var frames = image.GetFrameCount(FrameDimension.Time);
                if (frames <= 1)
                {
                    var bmp = new Bitmap(image);
                    var frame = new Frame(bmp, 0);
                    imgList.Add(frame);
                }
                else
                {
                    var times = image.GetPropertyItem(0x5100).Value;
                    for (var i = 0; i < frames; i++)
                    {
                        var bmp = new Bitmap(image);
                        var duration = BitConverter.ToInt32(times, 4 * i);
                        var frame = new Frame(bmp, duration);
                        imgList.Add(frame);
                        image.SelectActiveFrame(FrameDimension.Time, i);
                    }
                }
                if (disposeImage)
                    image.Dispose();
                return imgList;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     An base class that provides a pair of two elements of the <see cref="Image"/>
        ///     class.
        /// </summary>
        public class ImagePair : IDisposable
        {
            private readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);
            private bool _disposed;

            /// <summary>
            ///     Initilazies a new instance of the <see cref="ImagePair"/> class.
            /// </summary>
            /// <param name="image1">
            ///     The first <see cref="Image"/>.
            /// </param>
            /// <param name="image2">
            ///     The second <see cref="Image"/>.
            /// </param>
            public ImagePair(Image image1, Image image2)
            {
                Image1 = image1;
                Image2 = image2;
            }

            /// <summary>
            ///     Gets the first image of this <see cref="ImagePair"/>.
            /// </summary>
            public Image Image1 { get; }

            /// <summary>
            ///     Gets the second image of this <see cref="ImagePair"/>.
            /// </summary>
            public Image Image2 { get; }

            /// <summary>
            ///     Releases all resources used by this <see cref="ImagePair"/>.
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            ///     Releases all resources used by this <see cref="ImagePair"/>.
            /// </summary>
            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                    return;
                if (disposing)
                {
                    _handle.Dispose();
                    Image1.Dispose();
                    Image2.Dispose();
                }
                _disposed = true;
            }
        }

        /// <summary>
        ///     An base class that provides the <see cref="System.Drawing.Image"/> and duration
        ///     of a single frame.
        /// </summary>
        public class Frame : IDisposable
        {
            private readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);
            private bool _disposed;

            /// <summary>
            ///     Initilazies a new instance of the <see cref="Frame"/> class from the
            ///     specified existing image and duration time of a single frame.
            /// </summary>
            /// <param name="image">
            ///     The frame image from which to create the new Frame.
            /// </param>
            /// <param name="duration">
            ///     The duration time, in milliseconds, of the new frame.
            /// </param>
            public Frame(Image image, int duration)
            {
                Image = image;
                Duration = duration;
            }

            /// <summary>
            ///     Gets the image of this <see cref="Frame"/>.
            /// </summary>
            public Image Image { get; }

            /// <summary>
            ///     Gets the duration time, in milliseconds, of this <see cref="Frame"/>.
            /// </summary>
            public int Duration { get; }

            /// <summary>
            ///     Releases all resources used by this <see cref="Frame"/>.
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            ///     Releases all resources used by this <see cref="Frame"/>.
            /// </summary>
            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                    return;
                if (disposing)
                {
                    _handle.Dispose();
                    Image.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
