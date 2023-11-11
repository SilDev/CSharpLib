#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PanelFakeProgressBar.cs
// Version:  2023-11-11 16:27
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    ///     Provides panel functions that are used as a progress bar. This enables a
    ///     more flexible design than <see cref="ProgressBar"/> and is very comfortable
    ///     to use compared to a complete <see cref="ProgressBar"/> overhaul.
    /// </summary>
    public class PanelFakeProgressBar : Panel
    {
        /// <summary>
        ///     Gets or sets the foreground color that is used when an error occurs.
        /// </summary>
        public static Color ErrorForeColor { get; set; } = Color.OrangeRed;

        /// <summary>
        ///     Gets or sets the foreground color that is used when the progress has
        ///     reached 100 percent.
        /// </summary>
        public static Color CompleteForeColor { get; set; } = Color.LimeGreen;

        /// <summary>
        ///     Sets the current position of the fake progress bar.
        /// </summary>
        /// <param name="panel">
        ///     The fake progress bar <see cref="Panel"/> control.
        /// </param>
        /// <param name="value">
        ///     The position to be set.
        /// </param>
        /// <param name="maxValue">
        ///     The maximum range.
        /// </param>
        public static Color SetProgress(Panel panel, int value, int maxValue = 100)
        {
            if (panel == null)
                return ErrorForeColor;
            var hWnd = panel.GetAncestor()?.Handle ?? IntPtr.Zero;
            if (hWnd != IntPtr.Zero)
                if (value == 0)
                    TaskBarProgress.SetState(hWnd, TaskBarProgressState.Indeterminate);
                else
                    TaskBarProgress.SetValue(hWnd, value, maxValue);
            var color = CompleteForeColor;
            if (CompleteForeColor == default)
                color = Color.FromArgb(byte.MaxValue - (byte)(value * (byte.MaxValue / (float)maxValue)), byte.MaxValue, value);
            using var g = panel.CreateGraphics();
            var width = value > 0 && value < maxValue ? (int)Math.Round(panel.Width / (double)maxValue * value, MidpointRounding.AwayFromZero) : panel.Width;
            using var b = new SolidBrush(value > 0 ? color : panel.BackColor);
            g.FillRectangle(b, 0, 0, width, panel.Height);
            return color;
        }

        /// <summary>
        ///     Sets the current position of the fake progress bar.
        /// </summary>
        /// <param name="value">
        ///     The position to be set.
        /// </param>
        /// <param name="maxValue">
        ///     The maximum range.
        /// </param>
        public Color SetProgress(int value, int maxValue = 100) =>
            SetProgress(this, value, maxValue);
    }
}
