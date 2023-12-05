﻿#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ColorDialogEx.cs
// Version:  2023-12-05 13:51
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
    using static WinApi;

    /// <summary>
    ///     Expands the functionality for the <see cref="ColorDialog"/> class.
    /// </summary>
    public class ColorDialogEx : ColorDialog
    {
        private readonly IWin32Window _owner;
        private readonly string _title;
        private Point _point;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorDialogEx"/> class.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="title">
        ///     The new title of the window.
        /// </param>
        public ColorDialogEx(IWin32Window owner, string title = null)
        {
            _owner = owner;
            _title = title;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorDialogEx"/> class.
        /// </summary>
        /// <param name="point">
        ///     The new position of the window.
        /// </param>
        /// <param name="title">
        ///     The new title of the window.
        /// </param>
        public ColorDialogEx(Point point, string title = null)
        {
            _point = point;
            _title = title;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorDialogEx"/> class.
        /// </summary>
        /// <param name="x">
        ///     The new position of the left side of the window.
        /// </param>
        /// <param name="y">
        ///     The new position of the top of the window.
        /// </param>
        /// <param name="title">
        ///     The new title of the window.
        /// </param>
        public ColorDialogEx(int x, int y, string title = null)
        {
            _point = new Point(x, y);
            _title = title;
        }

        protected override IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            var hookProc = base.HookProc(hWnd, msg, wparam, lparam);
            if (msg != (int)WindowMenuFlags.WmInitDialog)
                return hookProc;
            if (!string.IsNullOrEmpty(_title))
                NativeMethods.SetWindowText(hWnd, _title);
            if (_owner != null)
            {
                var cRect = new Rectangle(0, 0, 0, 0);
                if (NativeMethods.GetWindowRect(hWnd, ref cRect))
                {
                    var width = cRect.Width - cRect.X;
                    var height = cRect.Height - cRect.Y;
                    var pRect = new Rectangle(0, 0, 0, 0);
                    if (NativeMethods.GetWindowRect(_owner.Handle, ref pRect))
                    {
                        var ptCenter = new Point(pRect.X, pRect.Y);
                        ptCenter.X += (pRect.Width - pRect.X) / 2;
                        ptCenter.Y += (pRect.Height - pRect.Y) / 2 - 10;
                        var ptStart = new Point(ptCenter.X, ptCenter.Y);
                        ptStart.X -= width / 2;
                        if (ptStart.X < 0)
                            ptStart.X = 0;
                        ptStart.Y -= height / 2;
                        if (ptStart.Y < 0)
                            ptStart.Y = 0;
                        _point = ptStart;
                    }
                }
            }
            if (_point == null)
                return hookProc;
            NativeMethods.SetWindowPos(hWnd, IntPtr.Zero, _point.X, _point.Y, 0, 0, SetWindowPosFlags.NoSize | SetWindowPosFlags.NoZOrder | SetWindowPosFlags.ShowWindow);
            return hookProc;
        }
    }
}
