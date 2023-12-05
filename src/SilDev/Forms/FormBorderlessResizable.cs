#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: FormBorderlessResizable.cs
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
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    ///     Represents a window or dialog box that makes up an application's user
    ///     interface.
    /// </summary>
    public class FormBorderlessResizable : Form
    {
        /// <summary>
        ///     Provides enumerated values for determining sizing border areas.
        /// </summary>
        [Flags]
        public enum ResizingBorderFlags
        {
            /// <summary>
            ///     No border or corner of a window.
            /// </summary>
            None = 0x1,

            /// <summary>
            ///     The left border of a window.
            /// </summary>
            Left = 0x2,

            /// <summary>
            ///     The right border of a window.
            /// </summary>
            Right = 0x4,

            /// <summary>
            ///     The upper-horizontal border of a window.
            /// </summary>
            Top = 0x8,

            /// <summary>
            ///     The upper-left corner of a window border.
            /// </summary>
            TopLeft = 0x10,

            /// <summary>
            ///     The upper-right corner of a window border.
            /// </summary>
            TopRight = 0x20,

            /// <summary>
            ///     The lower-horizontal border of a window.
            /// </summary>
            Bottom = 0x40,

            /// <summary>
            ///     The lower-left corner of a border of a window.
            /// </summary>
            BottomLeft = 0x80,

            /// <summary>
            ///     The lower-right corner of a border of a window.
            /// </summary>
            BottomRight = 0x100,

            /// <summary>
            ///     All borders and corners of a window.
            /// </summary>
            All = Left | Right | Top | TopLeft | TopRight | Bottom | BottomLeft | BottomRight
        }

        /// <summary>
        ///     Provides enumerated Windows <see cref="Message"/>'s, used within a
        ///     <see cref="WndProc"/> override.
        /// </summary>
        protected enum WindowsMessage
        {
            /// <summary>
            ///     Specify the first mouse message.
            /// </summary>
            MouseFirst = 0x200,

            /// <summary>
            ///     Sent to a window in order to determine what part of the window corresponds
            ///     to a particular screen coordinate. This can happen, for example, when the
            ///     cursor moves, when a mouse button is pressed or released, or in response to
            ///     a call to a function such as WindowFromPoint. If the mouse is not captured,
            ///     the message is sent to the window beneath the cursor. Otherwise, the
            ///     message is sent to the window that has captured the mouse.
            /// </summary>
            NcHitTest = 0x84
        }

        private IReadOnlyDictionary<ResizingBorderFlags, (IntPtr, Rectangle)> _allBorderAreas;
        private int _borderThickness;

        /// <summary>
        ///     Gets all border areas.
        /// </summary>
        protected IReadOnlyDictionary<ResizingBorderFlags, (IntPtr, Rectangle)> AllBorderAreas
        {
            get
            {
                if (_allBorderAreas != default)
                    return _allBorderAreas;
                var width = Size.Width;
                var height = Size.Height;
                var thickness = BorderThickness;
                var areas = new Dictionary<ResizingBorderFlags, (IntPtr, Rectangle)>
                {
                    {
                        ResizingBorderFlags.Left,
                        (new IntPtr(10), new Rectangle(0, thickness, thickness, height - 2 * thickness))
                    },
                    {
                        ResizingBorderFlags.Right,
                        (new IntPtr(11), new Rectangle(width - thickness, thickness, thickness, height - 2 * thickness))
                    },
                    {
                        ResizingBorderFlags.Top,
                        (new IntPtr(12), new Rectangle(thickness, 0, width - 2 * thickness, thickness))
                    },
                    {
                        ResizingBorderFlags.TopLeft,
                        (new IntPtr(13), new Rectangle(0, 0, thickness, thickness))
                    },
                    {
                        ResizingBorderFlags.TopRight,
                        (new IntPtr(14), new Rectangle(width - thickness, 0, thickness, thickness))
                    },
                    {
                        ResizingBorderFlags.Bottom,
                        (new IntPtr(15), new Rectangle(thickness, height - thickness, width - 2 * thickness, thickness))
                    },
                    {
                        ResizingBorderFlags.BottomLeft,
                        (new IntPtr(16), new Rectangle(0, height - thickness, thickness, thickness))
                    },
                    {
                        ResizingBorderFlags.BottomRight,
                        (new IntPtr(17), new Rectangle(width - thickness, height - thickness, thickness, thickness))
                    }
                };
                return _allBorderAreas = new ReadOnlyDictionary<ResizingBorderFlags, (IntPtr, Rectangle)>(areas);
            }
        }

        /// <summary>
        ///     The <see cref="ResizingBorderFlags"/> flags for resizing.
        /// </summary>
        protected ResizingBorderFlags ResizingBorders { get; set; } = ResizingBorderFlags.All;

        /// <summary>
        ///     Determines whether the corners of the window are rounded.
        ///     <para>
        ///         &#9888; This feature requires at least Windows 11, where it is enabled
        ///         by default.
        ///     </para>
        /// </summary>
        protected bool RoundCorners { get; }

        /// <summary>
        ///     Gets the border thickness.
        /// </summary>
        protected int BorderThickness
        {
            get => _borderThickness;
            set
            {
                var thickness = Math.Min(Math.Max(1, value), 32);
                if (_borderThickness == thickness)
                    return;
                _borderThickness = thickness;
                _allBorderAreas = default;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FormBorderlessResizable"/>
        ///     form class.
        /// </summary>
        public FormBorderlessResizable()
        {
            BorderThickness = 6;
            RoundCorners = EnvironmentEx.IsAtLeastWindows(11);
        }

        /// ReSharper disable CommentTypo
        /// <summary>
        ///     Get the border areas depending on the specified
        ///     <see cref="ResizingBorderFlags"/> flags.
        ///     <para>
        ///         This method is used by the
        ///         <see cref="GetActiveResizingBorderMessage(ResizingBorderFlags, IntPtr, int)"/>
        ///         function.
        ///     </para>
        /// </summary>
        /// <param name="flags">
        ///     The <see cref="ResizingBorderFlags"/> flags.
        /// </param>
        /// <param name="thickness">
        ///     The border thickness. Valid values are 1 through 32.
        /// </param>
        /// <returns>
        ///     The return value is a <see cref="Dictionary{TKey, TValue}"/> with HTLEFT,
        ///     HTRIGHT, HTTOP, HTTOPLEFT, HTTOPRIGHT, HTBOTTOM, HTBOTTOMLEFT, and/or
        ///     HTBOTTOMRIGHT values, indicating the position of the cursor hot spot, as
        ///     key; and a <see cref="Rectangle"/> with the border coordinates as value.
        /// </returns>
        protected IEnumerable<(IntPtr, Rectangle)> GetResizingBorderAreas(ResizingBorderFlags flags, int thickness = 6)
        {
            if (flags.HasFlag(ResizingBorderFlags.None))
                yield break;
            if (BorderThickness != thickness)
                BorderThickness = thickness;
            if (flags.HasFlag(ResizingBorderFlags.Left))
                yield return AllBorderAreas[ResizingBorderFlags.Left];
            if (flags.HasFlag(ResizingBorderFlags.Right))
                yield return AllBorderAreas[ResizingBorderFlags.Right];
            if (flags.HasFlag(ResizingBorderFlags.Top))
                yield return AllBorderAreas[ResizingBorderFlags.Top];
            if (flags.HasFlag(ResizingBorderFlags.TopLeft))
                yield return AllBorderAreas[ResizingBorderFlags.TopLeft];
            if (flags.HasFlag(ResizingBorderFlags.TopRight))
                yield return AllBorderAreas[ResizingBorderFlags.TopRight];
            if (flags.HasFlag(ResizingBorderFlags.Bottom))
                yield return AllBorderAreas[ResizingBorderFlags.Bottom];
            if (flags.HasFlag(ResizingBorderFlags.BottomLeft))
                yield return AllBorderAreas[ResizingBorderFlags.BottomLeft];
            if (flags.HasFlag(ResizingBorderFlags.BottomRight))
                yield return AllBorderAreas[ResizingBorderFlags.BottomRight];
        }

        /// ReSharper disable CommentTypo
        /// <summary>
        ///     Get the active border area <see cref="Message"/> depending on the specified
        ///     <see cref="ResizingBorderFlags"/> flags.
        ///     <para>
        ///         This method is recommended to use within a <see cref="WndProc"/>
        ///         override.
        ///     </para>
        /// </summary>
        /// <param name="flags">
        ///     The <see cref="ResizingBorderFlags"/> flags.
        /// </param>
        /// <param name="lParam">
        ///     The <see cref="Message"/>.LParam field of the message.
        /// </param>
        /// <param name="thickness">
        ///     The border thickness. Valid values are 1 through 32.
        /// </param>
        /// <returns>
        ///     The return value is HTLEFT, HTRIGHT, HTTOP, HTTOPLEFT, HTTOPRIGHT,
        ///     HTBOTTOM, HTBOTTOMLEFT, or HTBOTTOMRIGHT, indicating the position of the
        ///     cursor hot spot, used to specify the value that is returned to Windows in
        ///     response to handling the <see cref="Message"/>.
        /// </returns>
        protected IntPtr GetActiveResizingBorderMessage(ResizingBorderFlags flags, IntPtr lParam, int thickness = 6)
        {
            var borderArea = GetResizingBorderAreas(flags, thickness);
            if (borderArea == null)
                return IntPtr.Zero;
            var screenPoint = new Point(lParam.ToInt32());
            var clientPoint = PointToClient(screenPoint);
            foreach (var (result, rect) in borderArea)
            {
                if (!rect.Contains(clientPoint))
                    continue;
                return result;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        ///     Sets the <see cref="ResizingBorders"/> flags depending on the specified
        ///     taskbar location and alignment.
        ///     <para>
        ///         Result for <see cref="TaskBarLocation.Left"/>:
        ///         <code>
        ///             <see cref="ResizingBorderFlags.Right"/> | <see cref="ResizingBorderFlags.Bottom"/> |
        ///             <see cref="ResizingBorderFlags.BottomRight"/>
        ///         </code>
        ///     </para>
        ///     <para>
        ///         Result for <see cref="TaskBarLocation.Right"/>:
        ///         <code>
        ///             <see cref="ResizingBorderFlags.Left"/> | <see cref="ResizingBorderFlags.Bottom"/> |
        ///             <see cref="ResizingBorderFlags.BottomLeft"/>
        ///         </code>
        ///     </para>
        ///     <para>
        ///         Result for <see cref="TaskBarLocation.Top"/>:
        ///         <code>
        ///             <see cref="ResizingBorderFlags.Right"/> | <see cref="ResizingBorderFlags.Bottom"/> |
        ///             <see cref="ResizingBorderFlags.BottomRight"/>
        ///         </code>
        ///     </para>
        ///     <para>
        ///         Result for <see cref="TaskBarLocation.Bottom"/>:
        ///         <code>
        ///             <see cref="ResizingBorderFlags.Right"/> | <see cref="ResizingBorderFlags.Top"/> |
        ///             <see cref="ResizingBorderFlags.TopRight"/>
        ///         </code>
        ///     </para>
        ///     <para>
        ///         Result for <see cref="TaskBarLocation.Hidden"/>:
        ///         <code>
        ///             <see cref="ResizingBorderFlags.All"/>
        ///         </code>
        ///     </para>
        ///     <para>
        ///         Result for <see cref="TaskBarAlignment.Center"/>:
        ///         <code>
        ///             <see cref="ResizingBorderFlags.Left"/> is added to <see cref="TaskBarLocation.Top"/> and <see cref="TaskBarLocation.Bottom"/>
        ///         </code>
        ///     </para>
        /// </summary>
        /// <param name="taskBarLocation">
        ///     The <see cref="TaskBarLocation"/>.
        /// </param>
        /// <param name="taskBarAlignment">
        ///     The <see cref="TaskBarAlignment"/>.
        /// </param>
        protected void SetResizingBorders(TaskBarLocation taskBarLocation, TaskBarAlignment taskBarAlignment)
        {
            var flags = taskBarLocation switch
            {
                TaskBarLocation.Left => ResizingBorderFlags.Right | ResizingBorderFlags.Bottom | ResizingBorderFlags.BottomRight,
                TaskBarLocation.Top => ResizingBorderFlags.Right | ResizingBorderFlags.Bottom | ResizingBorderFlags.BottomRight,
                TaskBarLocation.Right => ResizingBorderFlags.Left | ResizingBorderFlags.Bottom | ResizingBorderFlags.BottomLeft,
                TaskBarLocation.Bottom => ResizingBorderFlags.Right | ResizingBorderFlags.Top | ResizingBorderFlags.TopRight,
                _ => ResizingBorderFlags.All
            };
            if (taskBarAlignment == TaskBarAlignment.Center)
                flags = taskBarLocation switch
                {
                    TaskBarLocation.Top => ResizingBorderFlags.Left | flags,
                    TaskBarLocation.Bottom => ResizingBorderFlags.Left | flags,
                    _ => flags
                };
            ResizingBorders = flags;
        }

        /// <summary>
        ///     Raises the <see cref="Form.Load"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FormBorderStyle = FormBorderStyle.None;
            if (RoundCorners)
                Shown += WhenShown;
        }

        /// <summary>
        ///     Occurs whenever the form is first displayed.
        /// </summary>
        /// <param name="sender">
        ///     The object that owns the event.
        /// </param>
        /// <param name="e">
        ///     An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void WhenShown(object sender, EventArgs e) =>
            Desktop.RoundCorners(Handle);

        /// <summary>
        ///     Processes Windows messages.
        /// </summary>
        /// <param name="m">
        ///     The Windows <see cref="Message"/> to process.
        /// </param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg is (int)WindowsMessage.MouseFirst or (int)WindowsMessage.NcHitTest)
            {
                var result = GetActiveResizingBorderMessage(ResizingBorders, m.LParam);
                if (result != IntPtr.Zero)
                {
                    m.Result = result;
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}
