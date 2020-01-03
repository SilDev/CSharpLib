#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IconEx.cs
// Version:  2020-01-03 12:27
// 
// Copyright (c) 2020, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Drawing
{
    using System;
    using System.Drawing;
    using System.IO;

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
            var img = default(Image);
            var ico = default(Icon);
            try
            {
                if (image == null)
                    throw new ArgumentNullException(nameof(image));
                img = (Image)image.Clone();
                using (var ms = new MemoryStream())
                {
                    var images = new[]
                    {
                        img.Width != size || img.Height != size ? img.Redraw(size, size) : img
                    };
                    IconFactory.Save(images, ms);
                    ms.Position = 0;
                    ico = new Icon(ms);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                img?.Dispose();
            }
            return ico;
        }
    }
}
