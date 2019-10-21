#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PanelEx.cs
// Version:  2019-10-15 11:05
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="Panel"/> class.
    /// </summary>
    public static class PanelEx
    {
        /// <summary>
        ///     ***This is an undocumented class and can be changed or removed in the future
        ///     without further notice.
        /// </summary>
        public static class FakeProgressBar
        {
            public static Color ForeColor { get; set; } = default;

            public static Color Update(Panel panel, int value, int maxValue = 100)
            {
                if (panel == null)
                    return Color.OrangeRed;
                var hWnd = panel.GetAncestor()?.Handle ?? IntPtr.Zero;
                if (hWnd != IntPtr.Zero)
                    if (value == 0)
                        TaskBarProgress.SetState(hWnd, TaskBarProgressState.Indeterminate);
                    else
                        TaskBarProgress.SetValue(hWnd, value, maxValue);
                var color = ForeColor;
                if (ForeColor == default)
                    color = Color.FromArgb(byte.MaxValue - (byte)(value * (byte.MaxValue / (float)maxValue)), byte.MaxValue, value);
                using (var g = panel.CreateGraphics())
                {
                    var width = value > 0 && value < maxValue ? (int)Math.Round(panel.Width / (double)maxValue * value, MidpointRounding.AwayFromZero) : panel.Width;
                    using (Brush b = new SolidBrush(value > 0 ? color : panel.BackColor))
                        g.FillRectangle(b, 0, 0, width, panel.Height);
                }
                return color;
            }
        }
    }
}
