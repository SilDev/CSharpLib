#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ContextMenuStripEx.cs
// Version:  2018-06-07 09:32
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;
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

        private static Color _menuBorder = SystemColors.ControlDark;
        private static readonly Dictionary<ContextMenuStrip, KeyValuePair<int, WinApi.AnimateWindowFlags>> EnabledAnimation = new Dictionary<ContextMenuStrip, KeyValuePair<int, WinApi.AnimateWindowFlags>>();

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
            if (!(contextMenuStrip is ContextMenuStrip cms))
                return;
            if (toleration < 0)
                toleration = 0;
            cms.Opened += (sender, args) =>
            {
                var timer = new Timer
                {
                    Interval = 1,
                    Enabled = true
                };
                timer.Tick += (s, a) =>
                {
                    var rect = cms.ClientRectangle;
                    rect = new Rectangle(rect.X - toleration, rect.Y - toleration, rect.Width + toleration * 2, rect.Height + toleration * 2);
                    if (rect.Contains(cms.PointToClient(Control.MousePosition)))
                        return;
                    cms.Close();
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
        ///     <para>
        ///         Please note that this parameter is ignored if the animation is set to
        ///         <see cref="Animations.Default"/>.
        ///     </para>
        /// </param>
        public static void EnableAnimation(this ContextMenuStrip contextMenuStrip, Animations animation = Animations.Default, int time = 200)
        {
            if (!(contextMenuStrip is ContextMenuStrip cms))
                return;
            var settings = new KeyValuePair<int, WinApi.AnimateWindowFlags>(time, (WinApi.AnimateWindowFlags)animation);
            if (EnabledAnimation.ContainsKey(cms))
            {
                EnabledAnimation[cms] = settings;
                return;
            }
            EnabledAnimation.Add(cms, settings);
            var loaded = false;
            cms.Opening += (sender, args) =>
            {
                if (animation != Animations.Default)
                {
                    WinApi.NativeMethods.AnimateWindow(cms.Handle, EnabledAnimation[cms].Key, EnabledAnimation[cms].Value);
                    if (loaded)
                        return;
                    loaded = true;
                    cms.Refresh();
                    return;
                }
                cms.Opacity = 0d;
                var timer = new Timer
                {
                    Interval = 1,
                    Enabled = true
                };
                timer.Tick += (s, a) =>
                {
                    if (cms.Opacity < 1d)
                    {
                        cms.Opacity += .1d;
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
            if (!(contextMenuStrip is ContextMenuStrip cms))
                return;
            _menuBorder = borderColor ?? SystemColors.ControlDark;
            cms.Renderer = new Renderer();
            cms.Paint += (sender, args) =>
            {
                using (var gp = new GraphicsPath())
                {
                    var rects = new RectangleF[2];
                    for (var i = 0; i < rects.Length; i++)
                        rects[i] = new RectangleF(2, 2, cms.Width - 4 - i, cms.Height - 4 - i);
                    cms.Region = new Region(rects.FirstOrDefault());
                    gp.AddRectangle(rects.LastOrDefault());
                    using (var b = new SolidBrush(cms.BackColor))
                        args.Graphics.FillPath(b, gp);
                    using (var p = new Pen(borderColor ?? SystemColors.ControlDark, 1))
                        args.Graphics.DrawPath(p, gp);
                }
            };
        }

        private class Renderer : ToolStripProfessionalRenderer
        {
            public Renderer() : base(new ColorTable()) { }
        }

        private class ColorTable : ProfessionalColorTable
        {
            public override Color MenuItemBorder => _menuBorder;

            public override Color MenuItemSelected => ProfessionalColors.ButtonSelectedHighlight;
        }
    }
}
