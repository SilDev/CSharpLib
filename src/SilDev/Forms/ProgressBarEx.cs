#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ProgressBarEx.cs
// Version:  2016-10-24 15:58
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="ProgressBar"/> class.
    /// </summary>
    public static class ProgressBarEx
    {
        /// <summary>
        ///     Skips the very long animation and jumps directly to the <see cref="ProgressBar.Maximum"/>.
        /// </summary>
        /// <param name="progressBar">
        ///     The <see cref="ProgressBar"/> to progress.
        /// </param>
        public static void JumpToEnd(this ProgressBar progressBar)
        {
            try
            {
                var maximum = progressBar.Maximum;
                progressBar.Maximum = int.MaxValue;
                progressBar.Value = progressBar.Maximum;
                progressBar.Value--;
                progressBar.Maximum = maximum;
                progressBar.Value = progressBar.Maximum;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }
}
