#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ContextMenuStripEx.cs
// Version:  2017-06-23 12:07
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="ContextMenuStrip"/> class.
    /// </summary>
    public static class ContextMenuStripEx
    {
        /// <summary>
        ///     Provides enumerated values of <see cref="ContextMenuStrip"/> animations.
        /// </summary>
        public enum Animations : uint
        {
            /// <summary>
            ///     Smooth fade in animation.
            /// </summary>
            Default = 0x0,

            /// <summary>
            ///     Fade in animation.
            /// </summary>
            Blend = WinApi.AnimateWindowFlags.Blend,

            /// <summary>
            ///     Makes the <see cref="ContextMenuStrip"/> appear to collapse inward.
            /// </summary>
            Center = WinApi.AnimateWindowFlags.Center,

            /// <summary>
            ///     Slide animation from left to right.
            /// </summary>
            SlideHorPositive = WinApi.AnimateWindowFlags.Slide | WinApi.AnimateWindowFlags.HorPositive,

            /// <summary>
            ///     Slide animation from right to left.
            /// </summary>
            SlideHorNegative = WinApi.AnimateWindowFlags.Slide | WinApi.AnimateWindowFlags.HorNegative,

            /// <summary>
            ///     Slide animation from top to bottom.
            /// </summary>
            SlideVerPositive = WinApi.AnimateWindowFlags.Slide | WinApi.AnimateWindowFlags.VerPositive,

            /// <summary>
            ///     Slide animation from bottom to top.
            /// </summary>
            SlideVerNegative = WinApi.AnimateWindowFlags.Slide | WinApi.AnimateWindowFlags.VerNegative
        }

        private static readonly Dictionary<ContextMenuStrip, object[]> EnabledAnimation = new Dictionary<ContextMenuStrip, object[]>();

        /// <summary>
        ///     Closes this <see cref="ContextMenuStrip"/> when the mouse cursor leaves it.
        /// </summary>
        /// <param name="contextMenuStrip">
        ///     The <see cref="ContextMenuStrip"/>.
        /// </param>
        /// <param name="toleration">
        ///     The toleration in pixel.
        /// </param>
        public static void CloseOnMouseLeave(this ContextMenuStrip contextMenuStrip, int toleration = 0)
        {
            if (contextMenuStrip == null)
                return;
            if (toleration < 0)
                toleration = 0;
            contextMenuStrip.Opened += (sender, args) =>
            {
                var timer = new Timer
                {
                    Interval = 1,
                    Enabled = true
                };
                timer.Tick += (s, a) =>
                {
                    var rect = contextMenuStrip.ClientRectangle;
                    rect = new Rectangle(rect.X - toleration, rect.Y - toleration, rect.Width + toleration * 2, rect.Height + toleration * 2);
                    if (rect.Contains(contextMenuStrip.PointToClient(Control.MousePosition)))
                        return;
                    contextMenuStrip.Close();
                    timer.Dispose();
                };
            };
        }

        /// <summary>
        ///     Enables you to produce special effects when showing this <see cref="ContextMenuStrip"/>.
        /// </summary>
        /// <param name="contextMenuStrip">
        ///     The <see cref="ContextMenuStrip"/>.
        /// </param>
        /// <param name="animation">
        ///     The type of animation.
        /// </param>
        /// <param name="time">
        ///     The time it takes to play the animation, in milliseconds.
        /// </param>
        public static void EnableAnimation(this ContextMenuStrip contextMenuStrip, Animations animation = Animations.Default, int time = 200)
        {
            if (contextMenuStrip == null)
                return;
            var aniConfig = new object[] { time, animation };
            if (EnabledAnimation.ContainsKey(contextMenuStrip))
            {
                EnabledAnimation[contextMenuStrip] = aniConfig;
                return;
            }
            EnabledAnimation.Add(contextMenuStrip, aniConfig);
            contextMenuStrip.Opening += (sender, args) =>
            {
                if (animation != Animations.Default)
                {
                    WinApi.NativeMethods.AnimateWindow(contextMenuStrip.Handle, (int)EnabledAnimation[contextMenuStrip][0], (WinApi.AnimateWindowFlags)EnabledAnimation[contextMenuStrip][1]);
                    return;
                }
                contextMenuStrip.Opacity = 0d;
                var timer = new Timer
                {
                    Interval = 1,
                    Enabled = true
                };
                timer.Tick += (s, a) =>
                {
                    if (contextMenuStrip.Opacity < 1d)
                    {
                        contextMenuStrip.Opacity += .1d;
                        return;
                    }
                    timer.Dispose();
                };
            };
        }

        /// <summary>
        ///     Sets a single line border style for this <see cref="ContextMenuStrip"/>.
        /// </summary>
        /// <param name="contextMenuStrip">
        ///     The <see cref="ContextMenuStrip"/> to redraw.
        /// </param>
        /// <param name="borderColor">
        ///     The border color.
        /// </param>
        public static void SetFixedSingle(this ContextMenuStrip contextMenuStrip, Color? borderColor = null)
        {
            if (contextMenuStrip == null)
                return;
            contextMenuStrip.Paint += (sender, args) =>
            {
                try
                {
                    using (var gp = new GraphicsPath())
                    {
                        contextMenuStrip.Region = new Region(new RectangleF(2, 2, contextMenuStrip.Width - 4, contextMenuStrip.Height - 4));
                        gp.AddRectangle(new RectangleF(2, 2, contextMenuStrip.Width - 5, contextMenuStrip.Height - 5));
                        using (Brush b = new SolidBrush(contextMenuStrip.BackColor))
                            args.Graphics.FillPath(b, gp);
                        using (var p = new Pen(borderColor ?? SystemColors.ControlDark, 1))
                            args.Graphics.DrawPath(p, gp);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            };
        }
    }
}
