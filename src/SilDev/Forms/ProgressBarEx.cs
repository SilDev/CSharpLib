#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ProgressBarEx.cs
// Version:  2019-10-20 17:09
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
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
            if (!(progressBar is ProgressBar pb))
                return;
            var max = pb.Maximum;
            pb.Maximum = int.MaxValue;
            pb.Value = pb.Maximum;
            pb.Value--;
            pb.Maximum = max;
            pb.Value = pb.Maximum;
        }
    }
}
