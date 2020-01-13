﻿#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: MessageBoxButtonText.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Runtime.InteropServices;
    using Properties;

    /// <summary>
    ///     A based structure that provides the text for the <see cref="MessageBoxEx"/>
    ///     dialog box buttons.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MessageBoxButtonText : IEquatable<MessageBoxButtonText>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageBoxButtonText"/>
        ///     structure.
        /// </summary>
        /// <param name="enableDefaultText">
        ///     <see langword="true"/> to give each property a default value; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public MessageBoxButtonText(bool enableDefaultText) : this()
        {
            if (!enableDefaultText)
                return;
            Ok = UIStrings.Ok;
            Cancel = UIStrings.Cancel;
            Abort = UIStrings.Abort;
            Retry = UIStrings.Retry;
            Ignore = UIStrings.Ignore;
            Yes = UIStrings.Yes;
            No = UIStrings.No;
        }

        /// <summary>
        ///     The OK button.
        /// </summary>
        public string Ok { get; set; }

        /// <summary>
        ///     The Cancel button.
        /// </summary>
        public string Cancel { get; set; }

        /// <summary>
        ///     The Abort button.
        /// </summary>
        public string Abort { get; set; }

        /// <summary>
        ///     The Retry button.
        /// </summary>
        public string Retry { get; set; }

        /// <summary>
        ///     The Ignore button.
        /// </summary>
        public string Ignore { get; set; }

        /// <summary>
        ///     The Yes button.
        /// </summary>
        public string Yes { get; set; }

        /// <summary>
        ///     The No button.
        /// </summary>
        public string No { get; set; }

        /// <summary>
        ///     Determines whether this instance have same values as the specified
        ///     <see cref="MessageBoxButtonText"/> instance.
        /// </summary>
        /// <param name="other">
        ///     The <see cref="MessageBoxButtonText"/> instance to compare.
        /// </param>
        public bool Equals(MessageBoxButtonText other) =>
            Equals(GetHashCode(true), other.GetHashCode(true));

        /// <summary>
        ///     Determines whether this instance have same values as the specified
        ///     <see cref="object"/>.
        /// </summary>
        /// <param name="other">
        ///     The  <see cref="object"/> to compare.
        /// </param>
        public override bool Equals(object other)
        {
            if (!(other is MessageBoxButtonText item))
                return false;
            return Equals(item);
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <param name="nonReadOnly">
        ///     <see langword="true"/> to include the hashes of non-readonly properties;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public int GetHashCode(bool nonReadOnly) =>
            Crypto.GetStructHashCode(this, nonReadOnly);

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode() =>
            Crypto.GetStructHashCode(this);

        /// <summary>
        ///     Determines whether two specified <see cref="MessageBoxButtonText"/>
        ///     instances have same values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="MessageBoxButtonText"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="MessageBoxButtonText"/> instance to compare.
        /// </param>
        public static bool operator ==(MessageBoxButtonText left, MessageBoxButtonText right) =>
            left.Equals(right);

        /// <summary>
        ///     Determines whether two specified <see cref="MessageBoxButtonText"/>
        ///     instances have different values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="MessageBoxButtonText"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="MessageBoxButtonText"/> instance to compare.
        /// </param>
        public static bool operator !=(MessageBoxButtonText left, MessageBoxButtonText right) =>
            !(left == right);
    }
}
