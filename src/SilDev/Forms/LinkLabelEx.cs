#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: LinkLabelEx.cs
// Version:  2017-06-23 12:07
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="LinkLabel"/> class.
    /// </summary>
    public static class LinkLabelEx
    {
        /// <summary>
        ///     Creates a link for the specified text and associates it with the specified link.
        /// </summary>
        /// <param name="linkLabel">
        ///     The <see cref="LinkLabel"/> control to change.
        /// </param>
        /// <param name="text">
        ///     The text to link.
        /// </param>
        /// <param name="uri">
        ///     The link to associate.
        /// </param>
        public static void LinkText(this LinkLabel linkLabel, string text, Uri uri)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                    throw new ArgumentNullException(nameof(text));
                var start = 0;
                int index;
                while ((index = linkLabel.Text.IndexOf(text, start, StringComparison.Ordinal)) > -1)
                {
                    linkLabel.Links.Add(index, text.Length, uri);
                    start = index + text.Length;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Creates a link for the specified text and associates it with the specified link.
        /// </summary>
        /// <param name="linkLabel">
        ///     The <see cref="LinkLabel"/> control to change.
        /// </param>
        /// <param name="text">
        ///     The text to link.
        /// </param>
        /// <param name="uri">
        ///     The link to associate.
        /// </param>
        public static void LinkText(this LinkLabel linkLabel, string text, string uri) =>
            LinkText(linkLabel, text, uri.ToUri());
    }
}
