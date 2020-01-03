#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ControlAlphaNumericComparer.cs
// Version:  2020-01-03 13:25
// 
// Copyright (c) 2020, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System.Windows.Forms;

    /// <summary>
    ///     Provides a base class for comparison.
    /// </summary>
    public class ControlAlphaNumericComparer : AlphaNumericComparer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ControlAlphaNumericComparer"/>
        ///     class. A parameter specifies whether the order is descended.
        /// </summary>
        /// <param name="descendant">
        ///     true to enable the descending order; otherwise, false.
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
        protected override string GetString(object value)
        {
            if (!(value is Control ctrl))
                return null;
            return ctrl.Text;
        }
    }
}
