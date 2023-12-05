#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ContextMenuStripEx.cs
// Version:  2023-12-05 13:51
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;
    using System.Windows.Forms;
    using static WinApi;

    /// <summary>
    ///     Provides enumerated values of <see cref="ContextMenuStrip"/> animations.
    /// </summary>
    public enum ContextMenuStripExAnimation : uint
    {
        /// <summary>
        ///     Smooth fade in animation.
        /// </summary>
        Default = 0x0,

        /// <summary>
        ///     Fade in animation.
        /// </summary>
        Blend = AnimateWindowFlags.Blend,

        /// <summary>
        ///     Makes the <see cref="ContextMenuStrip"/> appear to collapse inward.
        /// </summary>
        Center = AnimateWindowFlags.Center,

        /// <summary>
        ///     Slide animation from left to right.
        /// </summary>
        SlideHorPositive = AnimateWindowFlags.Slide | AnimateWindowFlags.HorPositive,

        /// <summary>
        ///     Slide animation from right to left.
        /// </summary>
        SlideHorNegative = AnimateWindowFlags.Slide | AnimateWindowFlags.HorNegative,

        /// <summary>
        ///     Slide animation from top to bottom.
        /// </summary>
        SlideVerPositive = AnimateWindowFlags.Slide | AnimateWindowFlags.VerPositive,

        /// <summary>
        ///     Slide animation from bottom to top.
        /// </summary>
        SlideVerNegative = AnimateWindowFlags.Slide | AnimateWindowFlags.VerNegative
    }

    /// <summary>
    ///     Expands the functionality for the <see cref="ContextMenuStrip"/> class.
    /// </summary>
    public static class ContextMenuStripEx
    {
        private static HashSet<int> HashList { get; } = new();

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
            if (contextMenuStrip is not { } cms)
                return;

            var hash = cms.GetHashCode() + nameof(CloseOnMouseLeave).GetHashCode();
            if (HashList.Contains(hash))
                return;
            HashList.Add(hash);

            if (toleration < 0)
                toleration = 0;

            var timer = new Timer
            {
                Interval = 1,
                Enabled = false
            };
            timer.Tick += OnTick;
            cms.Opened += OnOpened;
            cms.Closed += OnClosed;
            cms.Disposed += OnDisposed;

            void OnTick(object sender, EventArgs e)
            {
                var rect = cms.ClientRectangle;
                rect = new Rectangle(rect.X - toleration, rect.Y - toleration, rect.Width + toleration * 2, rect.Height + toleration * 2);
                if (rect.Contains(cms.PointToClient(Control.MousePosition)))
                    return;
                cms.Close();
            }

            void OnOpened(object sender, EventArgs e) => timer.Enabled = true;

            void OnClosed(object sender, EventArgs e) => timer.Enabled = false;

            void OnDisposed(object sender, EventArgs e) => timer.Dispose();
        }

        /// <summary>
        ///     Enables you to produce special effects when showing this
        ///     <see cref="ContextMenuStrip"/>.
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
        ///         <see cref="ContextMenuStripExAnimation.Default"/>.
        ///     </para>
        /// </param>
        public static void EnableAnimation(this ContextMenuStrip contextMenuStrip, ContextMenuStripExAnimation animation = ContextMenuStripExAnimation.Default, int time = 200)
        {
            if (contextMenuStrip is not { } cms)
                return;

            var hash = cms.GetHashCode() + nameof(EnableAnimation).GetHashCode();
            if (HashList.Contains(hash))
                return;
            HashList.Add(hash);

            var loaded = false;
            var timer = new Timer
            {
                Interval = 1,
                Enabled = false
            };
            timer.Tick += OnTick;
            cms.Opening += OnOpening;
            cms.Disposed += OnDisposed;

            void OnTick(object sender, EventArgs e)
            {
                if (sender is not Timer owner)
                    return;
                if (cms.Opacity < 1d)
                {
                    cms.Opacity += .1d;
                    return;
                }
                owner.Enabled = false;
                cms.Opacity = 1d;
            }

            void OnOpening(object sender, CancelEventArgs e)
            {
                if (sender is not ContextMenuStrip owner || e.Cancel)
                    return;
                if (animation != ContextMenuStripExAnimation.Default)
                {
                    NativeMethods.AnimateWindow(owner.Handle, time, (AnimateWindowFlags)animation);
                    if (loaded)
                        return;
                    loaded = true;
                    owner.Refresh();
                    return;
                }
                owner.Opacity = 0d;
                timer.Enabled = true;
            }

            void OnDisposed(object sender, EventArgs e) => timer.Dispose();
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
            if (contextMenuStrip is not { } cms)
                return;

            var hash = cms.GetHashCode() + nameof(SetFixedSingle).GetHashCode();
            if (HashList.Contains(hash))
                return;
            HashList.Add(hash);

            cms.Renderer = new Renderer(borderColor ?? SystemColors.ControlDark);
            cms.Paint += OnPaint;

            void OnPaint(object sender, PaintEventArgs args)
            {
                using var gp = new GraphicsPath();
                var rects = new RectangleF[2];
                for (var i = 0; i < rects.Length; i++)
                    rects[i] = new RectangleF(2, 2, cms.Width - 4 - i, cms.Height - 4 - i);
                cms.Region = new Region(rects.FirstOrDefault());
                gp.AddRectangle(rects.LastOrDefault());
                using (var b = new SolidBrush(cms.BackColor))
                    args.Graphics.FillPath(b, gp);
                using var p = new Pen(borderColor ?? SystemColors.ControlDark, 1);
                args.Graphics.DrawPath(p, gp);
            }
        }

        private class Renderer : ToolStripProfessionalRenderer
        {
            public Renderer(Color menuItemBorder) : base(new ColorTable(menuItemBorder)) { }
        }

        private class ColorTable : ProfessionalColorTable
        {
            public override Color MenuItemBorder { get; }

            public override Color MenuItemSelected => ProfessionalColors.ButtonSelectedHighlight;

            public ColorTable(Color menuItemBorder) => MenuItemBorder = menuItemBorder;
        }
    }
}
