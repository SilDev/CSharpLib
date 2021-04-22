#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ImageEx.cs
// Version:  2021-04-22 19:45
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Drawing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Threading;
    using Properties;

    /// <summary>
    ///     Expands the functionality for the <see cref="Image"/> class.
    /// </summary>
    public static class ImageEx
    {
        private static volatile object _syncObject;
        private static volatile Dictionary<int, Tuple<Image, Image>> _imagePairCache;

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

        private static object SyncObject
        {
            get
            {
                if (_syncObject != null)
                    return _syncObject;
                var obj = new object();
                Interlocked.CompareExchange<object>(ref _syncObject, obj, null);
                return _syncObject;
            }
        }

        private static Dictionary<int, Tuple<Image, Image>> ImagePairCache
        {
            get
            {
                if (_imagePairCache != null)
                    return _imagePairCache;
                lock (SyncObject)
                    _imagePairCache = new Dictionary<int, Tuple<Image, Image>>();
                return _imagePairCache;
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
            double bit;
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    bit = 1d;
                    break;
                case PixelFormat.Format4bppIndexed:
                    bit = 4d;
                    break;
                case PixelFormat.Format8bppIndexed:
                    bit = 8d;
                    break;
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    bit = 16d;
                    break;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    bit = 32d;
                    break;
                case PixelFormat.Format48bppRgb:
                    bit = 48d;
                    break;
                default:
                    bit = 64d;
                    break;
            }
            var absolutRange = (int)Math.Ceiling(Math.Sqrt(memoryLimit / (bit * .125d)));
            return pixelIndicator.IsBetween(1, absolutRange);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="Image"/> size is within the
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
        ///     Determines whether the specified <see cref="Image"/> size is within the
        ///     allowed range, depending on the specified pixel format.
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
        ///     Determines whether the specified <see cref="Image"/> size is within the
        ///     allowed range.
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
        ///     Redraws the specified <see cref="Image"/> with the specified size and with
        ///     the specified rendering quality.
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
        ///     Specifies the maximal size indicator, which determines when the image gets
        ///     a new size.
        /// </param>
        public static Image Redraw(this Image image, SmoothingMode quality = SmoothingMode.HighQuality, int indicator = 1024)
        {
            if (image is not { } img)
                return default;
            int[] size =
            {
                img.Width,
                img.Height
            };
            if (indicator <= 0 || indicator >= size.First() && indicator >= size.Last())
                goto Return;
            for (var i = 0; i < size.Length; i++)
            {
                if (size[i] <= indicator)
                    continue;
                var percent = (int)Math.Floor(100d / size[i] * indicator);
                size[i] = (int)(size[i] * (percent / 100d));
                size[i == 0 ? 1 : 0] = (int)(size[i == 0 ? 1 : 0] * (percent / 100d));
                break;
            }
            Return:
            return img.Redraw(size.First(), size.Last(), quality);
        }

        /// <summary>
        ///     Redraws the specified <see cref="Image"/> with the specified maximum size
        ///     indicator and with the highest available rendering quality.
        /// </summary>
        /// <param name="image">
        ///     The image to draw.
        /// </param>
        /// <param name="indicator">
        ///     Specifies the maximal size indicator, which determines when the image gets
        ///     a new size.
        /// </param>
        public static Image Redraw(this Image image, int indicator) =>
            image.Redraw(SmoothingMode.HighQuality, indicator);

        /// <summary>
        ///     Sets the color-adjustment matrix for the specified <see cref="Image"/>.
        /// </summary>
        /// <param name="image">
        ///     The image to change.
        /// </param>
        /// <param name="colorMatrix">
        ///     The color-adjustment matrix to set.
        /// </param>
        public static Image SetColorMatrix(this Image image, ColorMatrix colorMatrix)
        {
            if (image is not { } img)
                return default;
            var bmp = new Bitmap(img.Width, img.Height);
            using var g = Graphics.FromImage(bmp);
            using var ia = new ImageAttributes();
            ia.SetColorMatrix(colorMatrix);
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            return bmp;
        }

        /// <summary>
        ///     Inverts the color matrix of the specified <see cref="Image"/>.
        /// </summary>
        /// <param name="image">
        ///     The image to convert.
        /// </param>
        public static Image InvertColors(this Image image)
        {
            var cm = new ColorMatrix(new[]
            {
                new[] { -1f, 00f, 00f, 00f, 00f },
                new[] { 00f, -1f, 00f, 00f, 00f },
                new[] { 00f, 00f, -1f, 00f, 00f },
                new[] { 00f, 00f, 00f, 01f, 00f },
                new[] { 01f, 01f, 01f, 00f, 01f }
            });
            return SetColorMatrix(image, cm);
        }

        /// <summary>
        ///     Scales the color matrix of the specified <see cref="Image"/> to gray.
        /// </summary>
        /// <param name="image">
        ///     The image to scale.
        /// </param>
        public static Image ToGrayScale(this Image image)
        {
            var cm = new ColorMatrix(new[]
            {
                new[] { 0.30f, 0.30f, 0.30f, 0.00f, 0.00f },
                new[] { 0.59f, 0.59f, 0.59f, 0.00f, 0.00f },
                new[] { 0.11f, 0.11f, 0.11f, 0.00f, 0.00f },
                new[] { 0.00f, 0.00f, 0.00f, 1.00f, 0.00f },
                new[] { 0.00f, 0.00f, 0.00f, 0.00f, 1.00f }
            });
            return SetColorMatrix(image, cm);
        }

        /// <summary>
        ///     Scales the color matrix of the specified <see cref="Image"/> to gray and
        ///     switch back to the original image the next time this function is called.
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
        public static Image SwitchGrayScale(this Image image, object key, bool dispose = false)
        {
            lock (SyncObject)
            {
                if (image is not { } img)
                    return default;
                var code = (key ?? '\0').GetHashCode();
                if (!ImagePairCache.ContainsKey(code))
                {
                    var imgPair = Tuple.Create(img, img.ToGrayScale());
                    ImagePairCache.Add(code, imgPair);
                    img = imgPair.Item2;
                }
                else
                {
                    if (!dispose)
                        img = img == ImagePairCache[code].Item1 ? ImagePairCache[code].Item2 : ImagePairCache[code].Item1;
                    else
                    {
                        img = new Bitmap(ImagePairCache[code].Item1);
                        ImagePairCache[code].Item1.Dispose();
                        ImagePairCache[code].Item2.Dispose();
                        ImagePairCache.Remove(code);
                    }
                }
                return img;
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
        ///     Gets the frames of the specified <see cref="Image"/>.
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
    }
}
