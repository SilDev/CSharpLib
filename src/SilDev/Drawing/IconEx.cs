#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IconEx.cs
// Version:  2018-03-09 00:57
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Drawing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;

    /// <summary>
    ///     Expands the functionality for the <see cref="Icon"/> class.
    /// </summary>
    public static class IconEx
    {
        /// <summary>
        ///     Converts this GDI+ <see cref="Image"/> to an <see cref="Icon"/>.
        /// </summary>
        /// <param name="image">
        ///     The image to convert.
        /// </param>
        /// <param name="size">
        ///     The icon size.
        /// </param>
        public static Icon ToIcon(this Image image, int size = 32)
        {
            var ico = default(Icon);
            try
            {
                using (var img = (Image)image.Clone())
                    using (var ms = new MemoryStream())
                    {
                        var images = new[]
                        {
                            img.Width != size || img.Height != size ? img.Redraw(size, size) : img
                        };
                        Factory.Save(images, ms);
                        ms.Position = 0;
                        ico = new Icon(ms);
                    }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return ico;
        }

        /// <summary>
        ///     Provides functions for handling icons.
        /// </summary>
        public static class Factory
        {
            /// <summary>
            ///     The option for determining automatic resizing.
            /// </summary>
            public enum SizeOption
            {
                /// <summary>
                ///     The full set includes 8x8, 10x10, 14x14, 16x16, 20x20, 22x22, 24x24,
                ///     32x32, 40x40, 48x48, 64x64, 96x96, 128x128, and 256x256.
                /// </summary>
                Additional,

                /// <summary>
                ///     The minimal set includes 16x16, 24x24, 32x32, 48x48, 64x64, 128x128,
                ///     and 256x256.
                /// </summary>
                Application
            }

            /// <summary>
            ///     Represents the largest possible height of an icon.
            /// </summary>
            public const int MaxHeight = 256;

            /// <summary>
            ///     Represents the largest possible width of an icon.
            /// </summary>
            public const int MaxWidth = 256;

            /// <summary>
            ///     Represents the smallest possible height of an icon.
            /// </summary>
            public const int MinHeight = 2;

            /// <summary>
            ///     Represents the smallest possible width of an icon.
            /// </summary>
            public const int MinWidth = 2;

            private static IEnumerable<int> GetSizes(SizeOption option)
            {
                if (option == SizeOption.Application)
                    return new[]
                    {
                        256,
                        128,
                        64,
                        48,
                        32,
                        24,
                        16
                    };
                return new[]
                {
                    256,
                    128,
                    96,
                    64,
                    48,
                    40,
                    32,
                    24,
                    22,
                    20,
                    16,
                    14,
                    10,
                    8
                };
            }

            private static IEnumerable<Image> ImageCorrection(IEnumerable<Image> images) =>
                images.Select(ImageCorrection).Where(Comparison.IsNotEmpty);

            private static Image ImageCorrection(Image image)
            {
                var dispose = false;
                try
                {
                    if (image == null || image.Width < MinWidth || image.Height < MinHeight)
                    {
                        dispose = true;
                        return null;
                    }
                    var img = image;
                    if (!img.PixelFormat.Equals(PixelFormat.Format32bppArgb))
                    {
                        var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppPArgb);
                        using (var g = Graphics.FromImage(bmp))
                            g.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height));
                        img = bmp;
                    }
                    if (!img.RawFormat.Guid.Equals(ImageFormat.Png.Guid))
                        using (var ms = new MemoryStream())
                        {
                            img.Save(ms, ImageFormat.Png);
                            ms.Position = 0;
                            img = Image.FromStream(ms);
                        }
                    if (img.Width > MaxWidth || img.Height > MaxHeight)
                        img = img.Redraw(MaxWidth, MaxHeight);
                    if (img.Width != img.Height)
                    {
                        var size = Math.Max(img.Width, img.Height);
                        img = img.Redraw(size, size);
                    }
                    if (!image.EqualsEx(img))
                        dispose = true;
                    return img;
                }
                finally
                {
                    if (dispose)
                        image?.Dispose();
                }
            }

            private static byte[] CreateBuffer(Image image)
            {
                byte[] ba;
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    ba = ms.ToArray();
                }
                return ba;
            }

            private static byte GetHeight(Image image) =>
                image.Height >= MaxHeight ? byte.MinValue : (byte)image.Height;

            private static byte GetWidth(Image image) =>
                image.Width >= MaxWidth ? byte.MinValue : (byte)image.Width;

            /// <summary>
            ///     Saves the specified sequence of <see cref="Image"/>'s as a single icon
            ///     into the output stream.
            /// </summary>
            /// <param name="images">
            ///     The images to be converted into a single icon.
            /// </param>
            /// <param name="stream">
            ///     The output stream.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Stream"/> after writing;
            ///     otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     images or stream is null.
            /// </exception>
            public static void Save(IEnumerable<Image> images, Stream stream, bool dispose = false)
            {
                if (images == null)
                    throw new ArgumentNullException(nameof(images));
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                var array = ImageCorrection(images.OrderBy(x => x.Width).ThenBy(x => x.Height)).ToArray();
                var bw = new BinaryWriter(stream);
                try
                {
                    bw.Write((ushort)0);
                    bw.Write((ushort)1);
                    bw.Write((ushort)array.Length);
                    var buffers = new Dictionary<uint, byte[]>();
                    var offset = (uint)(6 + 16 * array.Length);
                    foreach (var image in array)
                    {
                        var buffer = CreateBuffer(image);
                        var imageWidth = GetWidth(image);
                        var imageHeight = GetHeight(image);
                        var formatSize = Image.GetPixelFormatSize(image.PixelFormat);
                        bw.Write(imageWidth);
                        bw.Write(imageHeight);
                        bw.Write((byte)0);
                        bw.Write((byte)0);
                        bw.Write((ushort)1);
                        bw.Write((ushort)formatSize);
                        bw.Write((uint)buffer.Length);
                        bw.Write(offset);
                        buffers.Add(offset, buffer);
                        offset += (uint)buffer.Length;
                    }
                    foreach (var buffer in buffers)
                    {
                        bw.BaseStream.Seek(buffer.Key, SeekOrigin.Begin);
                        bw.Write(buffer.Value);
                    }
                }
                finally
                {
                    if (dispose)
                        bw.Dispose();
                }
            }

            /// <summary>
            ///     Saves multiple sizes of the specified <see cref="Image"/> to a single
            ///     <see cref="Icon"/> file.
            /// </summary>
            /// <param name="image">
            ///     The images to be converted into a single icon.
            /// </param>
            /// <param name="stream">
            ///     The output stream.
            /// </param>
            /// <param name="option">
            ///     The option for determining automatic resizing.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Stream"/> after
            ///     writing; otherwise, false.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     image or stream is null.
            /// </exception>
            public static void Save(Image image, Stream stream, SizeOption option = SizeOption.Application, bool dispose = false)
            {
                if (image == null)
                    throw new ArgumentNullException(nameof(image));
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                var img = ImageCorrection(image);
                var imgs = ImageCorrection(GetSizes(option).Where(x => img.Width >= x).Select(x => img.Redraw(x, x)));
                Save(imgs, stream, dispose);
            }

            /// <summary>
            ///     Saves the specified sequence of <see cref="Image"/>'s to a single
            ///     <see cref="Icon"/> file.
            /// </summary>
            /// <param name="images">
            ///     The images to be converted into a single icon.
            /// </param>
            /// <param name="path">
            ///     The file path to the icon.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     images or path is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     path is invalid.
            /// </exception>
            public static void Save(IEnumerable<Image> images, string path)
            {
                if (images == null)
                    throw new ArgumentNullException(nameof(images));
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var file = PathEx.Combine(path);
                if (!PathEx.IsValidPath(file))
                    throw new ArgumentException();
                if (!FileEx.TryDelete(file))
                    return;
                using (var fs = new FileStream(file, FileMode.Create))
                    Save(images, fs, true);
            }

            /// <summary>
            ///     Saves multiple sizes of the specified <see cref="Image"/> to a single <see cref="Icon"/> file.
            /// </summary>
            /// <param name="image">
            ///     The images to be converted into a single icon.
            /// </param>
            /// <param name="path">
            ///     The file path to the icon.
            /// </param>
            /// <param name="option">
            ///     The option for determining automatic resizing.
            /// </param>
            /// <exception cref="ArgumentNullException">
            ///     image or path is null.
            /// </exception>
            /// <exception cref="ArgumentException">
            ///     path is invalid.
            /// </exception>
            public static void Save(Image image, string path, SizeOption option = SizeOption.Application)
            {
                if (image == null)
                    throw new ArgumentNullException(nameof(image));
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var file = PathEx.Combine(path);
                if (!PathEx.IsValidPath(file))
                    throw new ArgumentException();
                if (!FileEx.TryDelete(file))
                    return;
                using (var fs = new FileStream(file, FileMode.Create))
                    Save(image, fs, option, true);
            }
        }
    }
}
