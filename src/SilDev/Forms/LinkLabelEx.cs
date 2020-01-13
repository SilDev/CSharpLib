#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: LinkLabelEx.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Windows.Forms;
    using Network;

    /// <summary>
    ///     Expands the functionality for the <see cref="LinkLabel"/> class.
    /// </summary>
    public static class LinkLabelEx
    {
        /// <summary>
        ///     Creates a link for the specified text and associates it with the specified
        ///     link.
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
        /// <exception cref="ArgumentNullException">
        ///     linkLabel, text or uri is null.
        /// </exception>
        public static void LinkText(this LinkLabel linkLabel, string text, Uri uri)
        {
            if (linkLabel == null)
                throw new ArgumentNullException(nameof(linkLabel));
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (string.IsNullOrWhiteSpace(linkLabel.Text) || string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(uri.OriginalString))
                return;
            var start = 0;
            int index;
            while ((index = linkLabel.Text.IndexOf(text, start, CultureConfig.GlobalStringComparison)) > -1)
            {
                linkLabel.Links.Add(index, text.Length, uri);
                start = index + text.Length;
            }
        }

        /// <summary>
        ///     Creates a link for the specified text and associates it with the specified
        ///     link.
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
        /// <exception cref="ArgumentNullException">
        ///     linkLabel, text or uri is null.
        /// </exception>
        public static void LinkText(this LinkLabel linkLabel, string text, string uri) =>
            LinkText(linkLabel, text, uri?.ToUri());
    }
}
