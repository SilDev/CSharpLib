#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IconFactory.cs
// Version:  2020-01-19 15:32
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Drawing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;

    /// <summary>
    ///     The option for determining automatic resizing.
    /// </summary>
    public enum IconFactorySizeOption
    {
        /// <summary>
        ///     The full set includes 8x8, 10x10, 14x14, 16x16, 20x20, 22x22, 24x24, 32x32,
        ///     40x40, 48x48, 64x64, 96x96, 128x128, and 256x256.
        /// </summary>
        Additional,

        /// <summary>
        ///     The minimal set includes 16x16, 24x24, 32x32, 48x48, 64x64, 128x128, and
        ///     256x256.
        /// </summary>
        Application
    }

    /// <summary>
    ///     Provides functions for handling the 'image/vnd.microsoft.icon' file format.
    /// </summary>
    public static class IconFactory
    {
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

        private const int SizeIconDir = 6;
        private const int SizeIconDirEntry = 16;
        private static Size _maxSize, _minSize;

        /// <summary>
        ///     Represents the largest possible size of an icon.
        /// </summary>
        public static Size MaxSize
        {
            get
            {
                if (_maxSize == default)
                    _maxSize = new Size(MaxWidth, MaxHeight);
                return _maxSize;
            }
        }

        /// <summary>
        ///     Represents the largest possible size of an icon.
        /// </summary>
        public static Size MinSize
        {
            get
            {
                if (_minSize == default)
                    _minSize = new Size(MinWidth, MinHeight);
                return _minSize;
            }
        }

        /// <summary>
        ///     Retrieves all size dimensions for the specified
        ///     <see cref="IconFactorySizeOption"/> value.
        /// </summary>
        /// <param name="option">
        ///     The <see cref="IconFactorySizeOption"/> value.
        /// </param>
        public static IEnumerable<int> GetSizes(IconFactorySizeOption option)
        {
            if (option == IconFactorySizeOption.Application)
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

        /// <summary>
        ///     Ensures that all <see cref="Image"/> objects has the correct format and the
        ///     size dimensions are equal and in range of <see cref="MinSize"/> and
        ///     <see cref="MaxSize"/>.
        /// </summary>
        /// <param name="images">
        ///     The <see cref="Image"/> objects to be processed.
        /// </param>
        public static IEnumerable<Image> ImageCorrection(IEnumerable<Image> images) =>
            images.Select(ImageCorrection).Where(Comparison.IsNotEmpty);

        /// <summary>
        ///     Ensures that the <see cref="Image"/> object has the correct format and the
        ///     size dimensions are equal and in range of <see cref="MinSize"/> and
        ///     <see cref="MaxSize"/>.
        /// </summary>
        /// <param name="image">
        ///     The <see cref="Image"/> object to be processed.
        /// </param>
        public static Image ImageCorrection(Image image)
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
                    var bitmap = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppPArgb);
                    using (var g = Graphics.FromImage(bitmap))
                        g.DrawImage(img, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                    img = bitmap;
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
                else if (img.Width != img.Height)
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

        /// <summary>
        ///     Extracts all the <see cref="Image"/>'s from a valid icon file.
        /// </summary>
        /// <param name="path">
        ///     The full path to the icon to be extracted.
        /// </param>
        public static void Extract(string path)
        {
            try
            {
                var file = PathEx.Combine(path);
                if (!File.Exists(file))
                    throw new PathNotFoundException(file);
                var dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir))
                    throw new NullReferenceException();
                var name = Path.GetFileNameWithoutExtension(file);
                if (string.IsNullOrEmpty(name))
                    throw new NullReferenceException();
                dir = Path.Combine(dir, $"{name} sources");
                if (Directory.Exists(dir))
                    DirectoryEx.EnumerateFiles(dir, "*.png")?.ForEach(x => FileEx.Delete(x));
                else
                    Directory.CreateDirectory(dir);
                var buffer = File.ReadAllBytes(file);
                var length = BitConverter.ToInt16(buffer, 4);
                for (var i = 0; i < length; i++)
                {
                    var imageWidth = buffer[SizeIconDir + SizeIconDirEntry * i];
                    var imageHeight = buffer[SizeIconDir + SizeIconDirEntry * i + 1];
                    var pixelFormat = (int)BitConverter.ToInt16(buffer, SizeIconDir + SizeIconDirEntry * i + 6);
                    var imagePath = Path.Combine(dir, $"{pixelFormat}@{imageWidth}x{imageHeight}.png");
                    if (File.Exists(imagePath))
                        for (var j = 0; j < byte.MaxValue; j++)
                        {
                            if (!File.Exists(imagePath))
                                break;
                            imagePath = Path.Combine(dir, $"{pixelFormat}@{imageWidth}x{imageHeight}_{i}.png");
                        }
                    var formatSize = BitConverter.ToInt32(buffer, SizeIconDir + SizeIconDirEntry * i + 8);
                    var offset = BitConverter.ToInt32(buffer, SizeIconDir + SizeIconDirEntry * i + 12);
                    var ms = default(MemoryStream);
                    try
                    {
                        ms = new MemoryStream();
                        using var bw = new BinaryWriter(ms);
                        bw.Write(buffer, offset, formatSize);
                        ms.Seek(0, SeekOrigin.Begin);
                        using var bmp = new Bitmap(ms);
                        ms = null;
                        bmp.Save(imagePath);
                    }
                    finally
                    {
                        ms?.Dispose();
                    }
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Saves the specified sequence of <see cref="Image"/>'s as a single icon into
        ///     the output stream.
        /// </summary>
        /// <param name="images">
        ///     The images to be converted into a single icon.
        /// </param>
        /// <param name="stream">
        ///     The output stream.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Stream"/> after writing; otherwise, <see langword="false"/>.
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
            var array = ImageCorrection(images.OrderByDescending(x => x.Width).ThenByDescending(x => x.Height)).ToArray();
            var bw = new BinaryWriter(stream);
            try
            {
                bw.Write((ushort)0);
                bw.Write((ushort)1);
                bw.Write((ushort)array.Length);
                var buffers = new Dictionary<uint, byte[]>();
                var offset = (uint)(6 + SizeIconDir + SizeIconDirEntry * array.Length);
                foreach (var image in array)
                {
                    var buffer = CreateBuffer(image);
                    var imageWidth = GetWidth(image);
                    var imageHeight = GetHeight(image);
                    var pixelFormat = Image.GetPixelFormatSize(image.PixelFormat);
                    bw.Write(imageWidth);
                    bw.Write(imageHeight);
                    bw.Write((byte)0);
                    bw.Write((byte)0);
                    bw.Write((ushort)1);
                    bw.Write((ushort)pixelFormat);
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
                {
                    foreach (var image in array)
                        image.Dispose();
                    bw.Dispose();
                }
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
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Stream"/> after writing; otherwise, <see langword="false"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     image or stream is null.
        /// </exception>
        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public static void Save(Image image, Stream stream, IconFactorySizeOption option = IconFactorySizeOption.Application, bool dispose = false)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            var img = default(Image);
            try
            {
                img = ImageCorrection(image);
                var size = Math.Max(img.Width, img.Height);
                var sizes = GetSizes(option);
                var images = sizes.Where(x => x <= size).Select(x => img.Redraw(x, x));
                Save(ImageCorrection(images), stream, dispose);
            }
            finally
            {
                if (dispose)
                    img?.Dispose();
            }
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
        /// <exception cref="ArgumentInvalidException">
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
                throw new ArgumentInvalidException(nameof(path));
            if (!FileEx.TryDelete(file))
                return;
            using var fs = new FileStream(file, FileMode.Create);
            Save(images, fs, true);
        }

        /// <summary>
        ///     Saves multiple sizes of the specified <see cref="Image"/> to a single
        ///     <see cref="Icon"/> file.
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
        /// <exception cref="ArgumentInvalidException">
        ///     path is invalid.
        /// </exception>
        public static void Save(Image image, string path, IconFactorySizeOption option = IconFactorySizeOption.Application)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            var file = PathEx.Combine(path);
            if (!PathEx.IsValidPath(file))
                throw new ArgumentInvalidException(nameof(path));
            if (!FileEx.TryDelete(file))
                return;
            using var fs = new FileStream(file, FileMode.Create);
            Save(image, fs, option, true);
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
    }
}
