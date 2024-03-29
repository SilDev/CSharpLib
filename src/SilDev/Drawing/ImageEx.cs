﻿#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ImageEx.cs
// Version:  2023-12-29 14:51
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Drawing
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Threading;
    using Properties;
    using static Crypto;
    using static WinApi;

    /// <summary>
    ///     Expands the functionality for the <see cref="Image"/> class.
    /// </summary>
    public static class ImageEx
    {
        private static ConcurrentDictionary<string, Image> _imageCache;

        /// <summary>
        ///     Gets an <see cref="Image"/> object which consists of a semi-transparent
        ///     black color.
        /// </summary>
        public static Image DimEmpty => Resources.DimEmptyImage;

        /// <summary>
        ///     Gets an <see cref="Image"/> object that contains a white 16px large search
        ///     symbol.
        /// </summary>
        public static Image DefaultSearchSymbol => Resources.SearchImage;

        private static ConcurrentDictionary<string, Image> ImageCache
        {
            get
            {
                if (_imageCache != null)
                    return _imageCache;
                var dict = new ConcurrentDictionary<string, Image>();
                Interlocked.CompareExchange(ref _imageCache, dict, null);
                return _imageCache;
            }
        }

        /// <summary>
        ///     Determines whether the specified pixel size indicator is within the allowed
        ///     range, depending on the specified pixel format.
        /// </summary>
        /// <param name="pixelIndicator">
        ///     The pixel size indicator to check.
        ///     <para>
        ///         The pixel size indicator represents the maximum value between the width
        ///         and height of an image.
        ///     </para>
        /// </param>
        /// <param name="pixelFormat">
        ///     The pixel format.
        /// </param>
        public static bool SizeIsValid(int pixelIndicator, PixelFormat pixelFormat)
        {
#if x86
            const double memoryLimit = 0x40000000;
#elif x64
            const double memoryLimit = 0x80000000;
#else
            var memoryLimit = Environment.Is64BitProcess ? 0x80000000d : 0x40000000;
#endif
            var bit = pixelFormat switch
            {
                PixelFormat.Format1bppIndexed => 1d,
                PixelFormat.Format4bppIndexed => 4d,
                PixelFormat.Format8bppIndexed => 8d,
                PixelFormat.Format16bppArgb1555 => 16d,
                PixelFormat.Format16bppGrayScale => 16d,
                PixelFormat.Format16bppRgb555 => 16d,
                PixelFormat.Format16bppRgb565 => 16d,
                PixelFormat.Format32bppArgb => 32d,
                PixelFormat.Format32bppPArgb => 32d,
                PixelFormat.Format32bppRgb => 32d,
                PixelFormat.Format48bppRgb => 48d,
                _ => 64d
            };
            var absolutRange = (int)Math.Ceiling(Math.Sqrt(memoryLimit / (bit * .125d)));
            return pixelIndicator.IsBetween(1, absolutRange);
        }

        /// <summary>
        ///     Determines whether the specified image width and height is within the
        ///     allowed range, depending on the specified pixel format.
        /// </summary>
        /// <param name="width">
        ///     The image width to check.
        /// </param>
        /// <param name="height">
        ///     The image height to check.
        /// </param>
        /// <param name="pixelFormat">
        ///     The pixel format.
        /// </param>
        public static bool SizeIsValid(int width, int height, PixelFormat pixelFormat)
        {
            var indicator = Math.Max(width, height);
            return SizeIsValid(indicator, pixelFormat);
        }

        /// <summary>
        ///     Determines whether the specified image size is within the allowed range,
        ///     depending on the specified pixel format.
        /// </summary>
        /// <param name="size">
        ///     The image size to check.
        /// </param>
        /// <param name="pixelFormat">
        ///     The pixel format.
        /// </param>
        public static bool SizeIsValid(Size size, PixelFormat pixelFormat)
        {
            var indicator = Math.Max(size.Width, size.Height);
            return SizeIsValid(indicator, pixelFormat);
        }

        /// <summary>
        ///     Determines whether the specified image size is within the allowed range.
        /// </summary>
        /// <param name="image">
        ///     The image to check.
        /// </param>
        public static bool SizeIsValid(Image image)
        {
            if (image is not { } img)
                return false;
            var indicator = Math.Max(img.Width, img.Height);
            return SizeIsValid(indicator, img.PixelFormat);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Image"/> class with the
        ///     specified color as background.
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
            var img = new Bitmap(width, height);
            using var g = Graphics.FromImage(img);
            using var b = new SolidBrush(color);
            g.FillRectangle(b, 0, 0, width, height);
            return img;
        }

        /// <summary>
        ///     Redraws this image with the specified size and with the specified rendering
        ///     quality.
        /// </summary>
        /// <param name="image">
        ///     The image to draw.
        /// </param>
        /// <param name="width">
        ///     The new width of the image.
        /// </param>
        /// <param name="heigth">
        ///     The new height of the image.
        /// </param>
        /// <param name="quality">
        ///     The rendering quality for the image.
        /// </param>
        public static Image Redraw(this Image image, int width, int heigth, SmoothingMode quality = SmoothingMode.HighQuality)
        {
            if (image is not { } img)
                return default;
            try
            {
                if (!SizeIsValid(width, heigth, PixelFormat.Format32bppArgb))
                    throw new ArgumentInvalidException($"{nameof(width)}+{nameof(heigth)}");
                var bmp = new Bitmap(width, heigth);
                bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                using var g = Graphics.FromImage(bmp);
                g.CompositingMode = CompositingMode.SourceCopy;
                switch (quality)
                {
                    case SmoothingMode.HighQuality:
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        break;
                    case SmoothingMode.AntiAlias:
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        break;
                    case SmoothingMode.HighSpeed:
                        g.CompositingQuality = CompositingQuality.HighSpeed;
                        g.InterpolationMode = InterpolationMode.NearestNeighbor;
                        g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                        g.SmoothingMode = SmoothingMode.HighSpeed;
                        break;
                    case SmoothingMode.Invalid:
                        g.CompositingQuality = CompositingQuality.Invalid;
                        g.InterpolationMode = InterpolationMode.Invalid;
                        g.PixelOffsetMode = PixelOffsetMode.Invalid;
                        g.SmoothingMode = SmoothingMode.Invalid;
                        break;
                    case SmoothingMode.None:
                        g.CompositingQuality = CompositingQuality.Default;
                        g.InterpolationMode = InterpolationMode.Default;
                        g.PixelOffsetMode = PixelOffsetMode.None;
                        g.SmoothingMode = SmoothingMode.None;
                        break;
                    case SmoothingMode.Default:
                        g.CompositingQuality = CompositingQuality.Default;
                        g.InterpolationMode = InterpolationMode.Default;
                        g.PixelOffsetMode = PixelOffsetMode.Default;
                        g.SmoothingMode = SmoothingMode.Default;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(quality), quality, null);
                }
                using var ia = new ImageAttributes();
                ia.SetWrapMode(WrapMode.TileFlipXY);
                g.DrawImage(img, new Rectangle(0, 0, width, heigth), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                return bmp;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return img;
            }
        }

        /// <summary>
        ///     Redraws this image with the specified maximum size indicator and with the
        ///     specified rendering quality.
        /// </summary>
        /// <param name="image">
        ///     The image to draw.
        /// </param>
        /// <param name="quality">
        ///     The rendering quality for the image.
        /// </param>
        /// <param name="indicator">
        ///     Specifies the maximal size indicator, which determines when the image gets
        ///     a new size.
        /// </param>
        /// <param name="enlargement">
        ///     <see langword="true"/> to allow enlargement of the image; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Image Redraw(this Image image, SmoothingMode quality = SmoothingMode.HighQuality, int indicator = 1024, bool enlargement = true)
        {
            if (image is not { } img || indicator < 1)
                return image;
            int[] size =
            {
                img.Width,
                img.Height
            };
            var max = Math.Max(size[0], size[1]);
            switch (enlargement)
            {
                case true when indicator != max:
                case false when indicator < max:
                {
                    if (size[0] == size[1])
                    {
                        size[0] = indicator;
                        size[1] = indicator;
                    }
                    else
                    {
                        var iMax = size[0] == max ? 0 : 1;
                        var iMin = iMax == 0 ? 1 : 0;
                        var per = Math.Round(100d / max * indicator);
                        size[iMax] = indicator;
                        size[iMin] = (int)Math.Round(size[iMin] * (per / 100d));
                    }
                    break;
                }
            }
            return img.Redraw(size[0], size[1], quality);
        }

        /// <summary>
        ///     Redraws this image with the specified maximum size indicator and with the
        ///     highest available rendering quality.
        /// </summary>
        /// <param name="image">
        ///     The image to draw.
        /// </param>
        /// <param name="indicator">
        ///     Specifies the maximal size indicator, which determines when the image gets
        ///     a new size.
        /// </param>
        /// <param name="enlargement">
        ///     <see langword="true"/> to allow enlargement of the image; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Image Redraw(this Image image, int indicator, bool enlargement = true) =>
            image.Redraw(SmoothingMode.HighQuality, indicator, enlargement);

        /// <summary>
        ///     Blurs this image with the specified strength.
        /// </summary>
        /// <param name="image">
        ///     The image to blur.
        /// </param>
        /// <param name="strength">
        ///     The strength, which must be between 1 and 99.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     image is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     strength is less than 1 or greater than 99.
        /// </exception>
        public static Image Blur(this Image image, int strength = 90)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (strength is < 1 or > 99)
                throw new ArgumentOutOfRangeException(nameof(image));
            var width = image.Width;
            var height = image.Height;
            strength = 100 - strength;
            var ws = (int)Math.Max(width / 100f * strength, 1f);
            var hs = (int)Math.Max(height / 100f * strength, 1f);
            return image.Redraw(ws, hs).Redraw(width, height);
        }

        /// <summary>
        ///     Sets the color-adjustment matrix for this image.
        /// </summary>
        /// <param name="image">
        ///     The image to change.
        /// </param>
        /// <param name="colorMatrix">
        ///     The color-adjustment matrix to set.
        /// </param>
        /// <param name="gamma">
        ///     The gamma value to set.
        /// </param>
        public static Image SetColorMatrix(this Image image, ColorMatrix colorMatrix, float gamma = 1f)
        {
            if (image is not { } img)
                return default;
            var bmp = new Bitmap(img.Width, img.Height);
            using var g = Graphics.FromImage(bmp);
            using var ia = new ImageAttributes();
            if (Math.Abs(gamma - 1f) > 0)
            {
                ia.ClearColorMatrix();
                ia.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                ia.SetGamma(gamma, ColorAdjustType.Bitmap);
            }
            ia.SetColorMatrix(colorMatrix);
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            return bmp;
        }

        /// <summary>
        ///     Changes the color-adjustments of this image.
        /// </summary>
        /// <param name="image">
        ///     The image to change.
        /// </param>
        /// <param name="brightness">
        ///     The brightness value to set.
        /// </param>
        /// <param name="contrast">
        ///     The contrast value to set.
        /// </param>
        /// <param name="gamma">
        ///     The gamma value to set.
        /// </param>
        /// <param name="alpha">
        ///     The alpha value to set.
        /// </param>
        public static Image ChangeColorMatrix(this Image image, float brightness = 1f, float contrast = 1f, float gamma = 1f, float alpha = 1f)
        {
            if (image is not { } img)
                return default;
            if (Math.Abs(brightness - 1f) == 0 &&
                Math.Abs(contrast - 1f) == 0 &&
                Math.Abs(gamma - 1f) == 0 &&
                Math.Abs(alpha - 1f) == 0)
                return img;
            brightness -= 1.0f;
            var cm = new ColorMatrix(new[]
            {
                new[] { contrast, 0f, 0f, 0f, 0f },
                new[] { 0f, contrast, 0f, 0f, 0f },
                new[] { 0f, 0f, contrast, 0f, 0f },
                new[] { 0f, 0f, 0f, alpha, 0f },
                new[]
                {
                    brightness, brightness, brightness, 0f, 1f
                }
            });
            return SetColorMatrix(img, cm, gamma);
        }

        /// <summary>
        ///     Sets the brightness of this image.
        /// </summary>
        /// <param name="image">
        ///     The image to change.
        /// </param>
        /// <param name="brightness">
        ///     The brightness value to set.
        /// </param>
        public static Image SetBrightness(this Image image, float brightness) =>
            image.ChangeColorMatrix(brightness);

        /// <summary>
        ///     Sets the contrast of this image.
        /// </summary>
        /// <param name="image">
        ///     The image to change.
        /// </param>
        /// <param name="contrast">
        ///     The contrast value to set.
        /// </param>
        public static Image SetContrast(this Image image, float contrast) =>
            image.ChangeColorMatrix(1f, contrast);

        /// <summary>
        ///     Sets the gamma of this image.
        /// </summary>
        /// <param name="image">
        ///     The image to change.
        /// </param>
        /// <param name="gamma">
        ///     The gamma value to set.
        /// </param>
        public static Image SetGamma(this Image image, float gamma) =>
            image.ChangeColorMatrix(1f, 1f, gamma);

        /// <summary>
        ///     Sets the alpha of this image.
        /// </summary>
        /// <param name="image">
        ///     The image to change.
        /// </param>
        /// <param name="alpha">
        ///     The alpha value to set.
        /// </param>
        public static Image SetAlpha(this Image image, float alpha) =>
            image.ChangeColorMatrix(1f, 1f, 1f, alpha);

        /// <summary>
        ///     Inverts the color matrix of this image.
        /// </summary>
        /// <param name="image">
        ///     The image to convert.
        /// </param>
        public static Image InvertColors(this Image image)
        {
            if (image is not { } img)
                return default;
            var cm = new ColorMatrix(new[]
            {
                new[] { -1f, 00f, 00f, 00f, 00f },
                new[] { 00f, -1f, 00f, 00f, 00f },
                new[] { 00f, 00f, -1f, 00f, 00f },
                new[] { 00f, 00f, 00f, 01f, 00f },
                new[] { 01f, 01f, 01f, 00f, 01f }
            });
            return SetColorMatrix(img, cm);
        }

        /// <summary>
        ///     Applies a color rotation to this image.
        /// </summary>
        /// <param name="image">
        ///     The image to change.
        /// </param>
        /// <param name="angle">
        ///     An angle to rotate.
        /// </param>
        public static Image HueRotate(this Image image, int angle)
        {
            if (image is not { } img)
                return default;
            if (angle == 0)
                return img;
            var rad = Math.PI * angle / 180d;
            var cos = (float)Math.Cos(rad);
            var sin = (float)Math.Sin(rad);
            var cm = new ColorMatrix(new[]
            {
                new[]
                {
                    0.213f + cos * 0.787f - sin * 0.213f,
                    0.213f - cos * 0.213f + sin * 0.143f,
                    0.213f - cos * 0.213f - sin * 0.787f,
                    0f,
                    0f
                },
                new[]
                {
                    0.715f - cos * 0.715f - sin * 0.715f,
                    0.715f + cos * 0.285f + sin * 0.140f,
                    0.715f - cos * 0.715f + sin * 0.715f,
                    0f,
                    0f
                },
                new[]
                {
                    0.072f - cos * 0.072f + sin * 0.928f,
                    0.072f - cos * 0.072f - sin * 0.283f,
                    0.072f + cos * 0.928f + sin * 0.072f,
                    0f,
                    0f
                },
                new[] { 0f, 0f, 0f, 1f, 0f },
                new[] { 0f, 0f, 0f, 0f, 1f }
            });
            return img.SetColorMatrix(cm);
        }

        /// <summary>
        ///     Scales the color matrix of this image to gray.
        /// </summary>
        /// <param name="image">
        ///     The image to scale.
        /// </param>
        public static Image ToGrayScale(this Image image)
        {
            if (image is not { } img)
                return default;
            var cm = new ColorMatrix(new[]
            {
                new[] { 0.30f, 0.30f, 0.30f, 0.00f, 0.00f },
                new[] { 0.59f, 0.59f, 0.59f, 0.00f, 0.00f },
                new[] { 0.11f, 0.11f, 0.11f, 0.00f, 0.00f },
                new[] { 0.00f, 0.00f, 0.00f, 1.00f, 0.00f },
                new[] { 0.00f, 0.00f, 0.00f, 0.00f, 1.00f }
            });
            return SetColorMatrix(img, cm);
        }

        /// <summary>
        ///     Scales the color matrix of this image to gray and switch back to the
        ///     original image the next time this function is called.
        /// </summary>
        /// <param name="image">
        ///     The image to switch.
        /// </param>
        /// <param name="key">
        ///     The key for the cache.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to dispose the cached images; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Image SwitchGrayScale(this Image image, object key, bool dispose = false) =>
            SwitchEffectInternal<object>(image, key, dispose);

        /// <summary>
        ///     Set the alpha of this image to the specified value and switch back to the
        ///     original image the next time this function is called.
        /// </summary>
        /// <param name="image">
        ///     The image to switch.
        /// </param>
        /// <param name="key">
        ///     The key for the cache.
        /// </param>
        /// <param name="alpha">
        ///     The alpha to set.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to dispose the cached images; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Image SwitchAlpha(this Image image, object key, float alpha = .5f, bool dispose = false) =>
            SwitchEffectInternal(image, key, dispose, alpha);

        /// <summary>
        ///     Blurs this image with the specified strength and switch back to the
        ///     original image the next time this function is called.
        /// </summary>
        /// <param name="image">
        ///     The image to switch.
        /// </param>
        /// <param name="key">
        ///     The key for the cache.
        /// </param>
        /// <param name="strength">
        ///     The strength, which must be between 1 and 99.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to dispose the cached images; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Image SwitchBlur(this Image image, object key, int strength = 20, bool dispose = false) =>
            SwitchEffectInternal(image, key, dispose, strength);

        /// <summary>
        ///     Adds an image associated with the specified key to the internal cache.
        /// </summary>
        /// <param name="image">
        ///     The image to cache.
        /// </param>
        /// <param name="key">
        ///     The key of the image.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to dispose the cached images; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Image Cache(this Image image, string key, bool dispose = false)
        {
            if (key != null && image != null && !ImageCache.ContainsKey(key))
                ImageCache.TryAdd(key, image);
            var img = ImageCache.TryGetValue(key);
            if (!dispose)
                return img;
            if (key != null && ImageCache.ContainsKey(key))
                ImageCache.TryRemove(key, out _);
            img?.Dispose();
            return img;
        }

        /// <summary>
        ///     Retrieves a previously added image associated with the specified key from
        ///     the internal cache.
        /// </summary>
        /// <param name="key">
        ///     The key of the image.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to dispose the cached images; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Image Cache(string key, bool dispose = false) =>
            Cache(null, key, dispose);

        /// <summary>
        ///     Recolors the pixels of this image using a specified old color and a
        ///     specified new color.
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
            if (image is not { } img)
                return default;
            var bmp = (Bitmap)img;
            for (var x = 0; x < img.Width; x++)
                for (var y = 0; y < img.Height; y++)
                {
                    var px = bmp.GetPixel(x, y);
                    if (px.R == from.R && px.G == from.G && px.B == from.B)
                        bmp.SetPixel(x, y, Color.FromArgb(px.A, to.R, to.G, to.B));
                }
            return bmp;
        }

        /// <summary>
        ///     Converts this image into a sequence of bytes.
        /// </summary>
        /// <param name="image">
        ///     The image to convert.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     image is null.
        /// </exception>
        public static byte[] ToBytes(this Image image)
        {
            if (image is not Bitmap bmp)
                throw new ArgumentNullException(nameof(image));
            var converter = new ImageConverter();
            return converter.ConvertTo(bmp, typeof(byte[])) as byte[];
        }

        /// <summary>
        ///     Gets the frames of this image.
        /// </summary>
        /// <param name="image">
        ///     The image to get the frames.
        /// </param>
        /// <param name="disposeImage">
        ///     <see langword="true"/> to dispose the original image; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     image is null.
        /// </exception>
        public static IEnumerable<ImageFrame> GetFrames(this Image image, bool disposeImage = true)
        {
            if (image is not { } img)
                throw new ArgumentNullException(nameof(image));
            try
            {
                var count = img.GetFrameCount(FrameDimension.Time);
                if (count <= 1)
                {
                    var frame = new ImageFrame(img, 0);
                    yield return frame;
                }
                else
                {
                    var times = img.GetPropertyItem(0x5100).Value;
                    for (var i = 0; i < count; i++)
                    {
                        var duration = BitConverter.ToInt32(times, 4 * i);
                        var frame = new ImageFrame(img, duration);
                        yield return frame;
                        if (i + 1 < count)
                            img.SelectActiveFrame(FrameDimension.Time, i);
                    }
                }
            }
            finally
            {
                if (disposeImage)
                    img.Dispose();
            }
        }

        /// <summary>
        ///     Determines whether this <see cref="Bitmap"/> has the same value as the
        ///     specified <see cref="Bitmap"/> based on its <see cref="PixelFormat"/>,
        ///     <see cref="ImageFormat"/>, and <see cref="ToBytes(Image)"/>.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="Bitmap"/> to check.
        /// </param>
        /// <param name="target">
        ///     The <see cref="Bitmap"/> to compare.
        /// </param>
        public static bool EqualsEx(this Bitmap source, Bitmap target)
        {
            if (source == null)
                return target == null;
            if (target == null)
                return false;
            if (!source.PixelFormat.Equals(target.PixelFormat))
                return false;
            if (!source.RawFormat.Equals(target.RawFormat))
                return false;
            var hash1 = source.ToBytes().EncryptRaw();
            var hash2 = target.ToBytes().EncryptRaw();
            return hash1 == hash2;
        }

        /// <summary>
        ///     Determines whether this <see cref="Image"/> has the same value as the
        ///     specified <see cref="Image"/> based on its <see cref="PixelFormat"/>,
        ///     <see cref="ImageFormat"/>, and <see cref="ToBytes(Image)"/>.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="Image"/> to check.
        /// </param>
        /// <param name="target">
        ///     The <see cref="Image"/> to compare.
        /// </param>
        public static bool EqualsEx(this Image source, Image target) =>
            (source as Bitmap).EqualsEx(target as Bitmap);

        /// <summary>
        ///     Captures the entire desktop under the specified window.
        /// </summary>
        /// <param name="hWnd">
        ///     Handle to a window.
        /// </param>
        /// <param name="x">
        ///     The x-coordinate, in logical units, of the upper-left corner of the
        ///     destination image.
        /// </param>
        /// <param name="y">
        ///     The y-coordinate, in logical units, of the upper-left corner of the
        ///     destination image.
        /// </param>
        /// <param name="width">
        ///     The width, in logical units, of the source and destination image.
        /// </param>
        /// <param name="height">
        ///     The height, in logical units, of the source and the destination image.
        /// </param>
        public static Image CaptureDesktop(IntPtr hWnd, int x, int y, int width, int height)
        {
            if (hWnd == IntPtr.Zero)
                return default;
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            var desktop = NativeMethods.GetDC(IntPtr.Zero);
            if (desktop == IntPtr.Zero ||
                !NativeMethods.BitBlt(g.GetHdc(), 0, 0, width, height, desktop, x, y, 0xcc0020) ||
                !NativeMethods.ReleaseDC(IntPtr.Zero, desktop) ||
                !NativeMethods.ReleaseDC(hWnd, NativeMethods.GetDC(hWnd)))
                return default;
            g.ReleaseHdc();
            return bmp;
        }

        private static Image SwitchEffectInternal<T>(Image image, object key, bool dispose, T effect = default)
        {
            if (image is not { } img)
                return default;
            var key1 = $"{typeof(T).Name}+{effect}+{key}";
            var key2 = $"{key1}+{key1.GetHashCode()}";
            if (!ImageCache.ContainsKey(key1) && !ImageCache.ContainsKey(key2))
            {
                ImageCache.TryAdd(key1, img);
                switch (effect)
                {
                    case float alpha:
                        ImageCache.TryAdd(key2, img.SetAlpha(alpha));
                        break;
                    case int strength:
                        ImageCache.TryAdd(key2, img.Blur(strength));
                        break;
                    default:
                        ImageCache.TryAdd(key2, img.ToGrayScale());
                        break;
                }
            }
            var img1 = ImageCache[key1];
            var img2 = ImageCache[key2];
            if (!dispose)
                return image == img1 ? img2 : img1;
            img1.Dispose();
            img2.Dispose();
            ImageCache.TryRemove(key1, out _);
            ImageCache.TryRemove(key2, out _);
            return image;
        }
    }
}
