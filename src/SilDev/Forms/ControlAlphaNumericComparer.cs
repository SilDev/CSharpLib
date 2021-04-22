#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ControlAlphaNumericComparer.cs
// Version:  2021-04-22 19:45
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System.Windows.Forms;

    /// <summary>
    ///     Provides a base class for comparison.
    /// </summary>
    public sealed class ControlAlphaNumericComparer : AlphaNumericComparer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ControlAlphaNumericComparer"/>
        ///     class. A parameter specifies whether the order is descended.
        /// </summary>
        /// <param name="descendant">
        ///     <see langword="true"/> to enable the descending order; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public ControlAlphaNumericComparer(bool descendant) : base(descendant) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ControlAlphaNumericComparer"/>
        ///     class.
        /// </summary>
        public ControlAlphaNumericComparer() : base(false) { }

        /// <summary>
        ///     Gets the string of the object that is used for comparison.
        /// </summary>
        /// <param name="value">
        ///     The object to compare.
        /// </param>
        public override string GetString(object value) =>
            value is not Control ctrl ? null : ctrl.Text;
    }
}
