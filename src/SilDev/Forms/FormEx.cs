#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: FormEx.cs
// Version:  2018-06-07 09:32
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="Form"/> class.
    /// </summary>
    public static class FormEx
    {
        /// <summary>
        ///     Provides special <see cref="Form"/> settings.
        /// </summary>
        [Flags]
        public enum PlusSettings
        {
            /// <summary>
            ///     Logs the loading time of the specified <see cref="Form"/>.
            /// </summary>
            LogLoadingTime = 0x10
        }

        /// <summary>
        ///     Allows to dock the specifed <see cref="Form"/> to the virtual screen edges.
        /// </summary>
        /// <param name="form">
        ///     The form window to be dockable.
        /// </param>
        public static void Dockable(Form form)
        {
            form.ResizeEnd += (sender, e) =>
            {
                if (!(sender is Form f))
                    return;
                WinApi.NativeHelper.MoveWindowToVisibleScreenArea(f.Handle);
                f.Update();
            };
        }

        /// <summary>
        ///     Determines special settings for the specified <see cref="Form"/>.
        ///     <para>
        ///         Hint: This function should be called before the <see cref="Form"/> is created.
        ///     </para>
        /// </summary>
        /// <param name="form">
        ///     The form window to determine the settings.
        /// </param>
        /// <param name="settings">
        ///     The settings to be applied.
        /// </param>
        public static Form Plus(this Form form, PlusSettings settings = PlusSettings.LogLoadingTime)
        {
            if (Log.DebugMode <= 0 || !settings.HasFlag(PlusSettings.LogLoadingTime))
                return form;
            try
            {
                if (Application.OpenForms.OfType<Form>().Any(x => x == form))
                    return form;
            }
            catch
            {
                return form;
            }
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            form.Shown += delegate
            {
                stopwatch.Stop();
                Log.Write($"Stopwatch: {form.Name} loaded in {stopwatch.ElapsedMilliseconds}ms.");
            };
            return form;
        }

        /// <summary>
        ///     Represents a window or dialog box that makes up an application's user interface.
        /// </summary>
        public class BorderlessResizable : Form
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
            ///     Initializes a new instance of the <see cref="BorderlessResizable"/> form class.
            /// </summary>
            [SuppressMessage("ReSharper", "EmptyConstructor")]
            public BorderlessResizable() { }

            /// <summary>
            ///     The <see cref="ResizingBorderFlags"/> flags for resizing.
            /// </summary>
            protected ResizingBorderFlags ResizingBorders { get; set; } = ResizingBorderFlags.All;

            /// <summary>
            ///     Get the border areas depending on the specified <see cref="ResizingBorderFlags"/> flags.
            ///     <para>
            ///         This method is used by the
            ///         <see cref="GetActiveResizingBorderMessage(ResizingBorderFlags, IntPtr, int)"/> function.
            ///     </para>
            /// </summary>
            /// <param name="flags">
            ///     The <see cref="ResizingBorderFlags"/> flags.
            /// </param>
            /// <param name="thickness">
            ///     The border thickness. Valid values are 1 through 32.
            /// </param>
            /// <returns>
            ///     The return value is a <see cref="Dictionary{TKey, TValue}"/> with HTLEFT, HTRIGHT, HTTOP,
            ///     HTTOPLEFT, HTTOPRIGHT, HTBOTTOM, HTBOTTOMLEFT, and/or HTBOTTOMRIGHT values, indicating
            ///     the position of the cursor hot spot, as key; and a <see cref="Rectangle"/> with the border
            ///     coordinates as value.
            /// </returns>
            protected Dictionary<IntPtr, Rectangle> GetResizingBorderAreas(ResizingBorderFlags flags, int thickness = 6)
            {
                if (flags.HasFlag(ResizingBorderFlags.None))
                    return default(Dictionary<IntPtr, Rectangle>);
                var d = new Dictionary<IntPtr, Rectangle>();
                var s = Size;
                var t = Math.Max(1, thickness);
                t = Math.Min(t, 32);
                if (flags.HasFlag(ResizingBorderFlags.Left))
                    d.Add(new IntPtr(10), new Rectangle(0, t, t, s.Height - 2 * t));
                if (flags.HasFlag(ResizingBorderFlags.Right))
                    d.Add(new IntPtr(11), new Rectangle(s.Width - t, t, t, s.Height - 2 * t));
                if (flags.HasFlag(ResizingBorderFlags.Top))
                    d.Add(new IntPtr(12), new Rectangle(t, 0, s.Width - 2 * t, t));
                if (flags.HasFlag(ResizingBorderFlags.TopLeft))
                    d.Add(new IntPtr(13), new Rectangle(0, 0, t, t));
                if (flags.HasFlag(ResizingBorderFlags.TopRight))
                    d.Add(new IntPtr(14), new Rectangle(s.Width - t, 0, t, t));
                if (flags.HasFlag(ResizingBorderFlags.Bottom))
                    d.Add(new IntPtr(15), new Rectangle(t, s.Height - t, s.Width - 2 * t, t));
                if (flags.HasFlag(ResizingBorderFlags.BottomLeft))
                    d.Add(new IntPtr(16), new Rectangle(0, s.Height - t, t, t));
                if (flags.HasFlag(ResizingBorderFlags.BottomRight))
                    d.Add(new IntPtr(17), new Rectangle(s.Width - t, s.Height - t, t, t));
                return d;
            }

            /// <summary>
            ///     Get the active border area <see cref="Message"/> depending on the specified
            ///     <see cref="ResizingBorderFlags"/> flags.
            ///     <para>
            ///         This method is recommended to use within a <see cref="WndProc"/> override.
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
            ///     The return value is HTLEFT, HTRIGHT, HTTOP, HTTOPLEFT, HTTOPRIGHT, HTBOTTOM, HTBOTTOMLEFT,
            ///     or HTBOTTOMRIGHT, indicating the position of the cursor hot spot, used to specify the
            ///     value that is returned to Windows in response to handling the <see cref="Message"/>.
            /// </returns>
            protected IntPtr GetActiveResizingBorderMessage(ResizingBorderFlags flags, IntPtr lParam, int thickness = 6)
            {
                var result = IntPtr.Zero;
                var borderArea = GetResizingBorderAreas(flags, thickness);
                if (borderArea == default(Dictionary<IntPtr, Rectangle>))
                    return result;
                var screenPoint = new Point(lParam.ToInt32());
                var clientPoint = PointToClient(screenPoint);
                foreach (var entry in borderArea)
                {
                    if (!entry.Value.Contains(clientPoint))
                        continue;
                    result = entry.Key;
                    break;
                }
                return result;
            }

            /// <summary>
            ///     Sets the <see cref="ResizingBorders"/> flags depending on the specified
            ///     <see cref="TaskBarLocation"/> flag.
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
            /// </summary>
            /// <param name="taskBarLocation">
            ///     The <see cref="TaskBarLocation"/> flag.
            /// </param>
            protected void SetResizingBorders(TaskBarLocation taskBarLocation)
            {
                ResizingBorderFlags flags;
                switch (taskBarLocation)
                {
                    case TaskBarLocation.Left:
                    case TaskBarLocation.Top:
                        flags = ResizingBorderFlags.Right |
                                ResizingBorderFlags.Bottom |
                                ResizingBorderFlags.BottomRight;
                        break;
                    case TaskBarLocation.Right:
                        flags = ResizingBorderFlags.Left |
                                ResizingBorderFlags.Bottom |
                                ResizingBorderFlags.BottomLeft;
                        break;
                    case TaskBarLocation.Bottom:
                        flags = ResizingBorderFlags.Right |
                                ResizingBorderFlags.Top |
                                ResizingBorderFlags.TopRight;
                        break;
                    default:
                        flags = ResizingBorderFlags.All;
                        break;
                }
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
            }

            /// <summary>
            ///     Processes Windows messages.
            /// </summary>
            /// <param name="m">
            ///     The Windows <see cref="Message"/> to process.
            /// </param>
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == (int)WindowsMessages.MouseFirst || m.Msg == (int)WindowsMessages.NcHitTest)
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

            /// <summary>
            ///     Provides enumerated Windows <see cref="Message"/>'s, used within a <see cref="WndProc"/>
            ///     override.
            /// </summary>
            protected enum WindowsMessages
            {
                /// <summary>
                ///     Specify the first mouse message.
                /// </summary>
                MouseFirst = 0x200,

                /// <summary>
                ///     Sent to a window in order to determine what part of the window corresponds to a
                ///     particular screen coordinate. This can happen, for example, when the cursor moves,
                ///     when a mouse button is pressed or released, or in response to a call to a function
                ///     such as WindowFromPoint. If the mouse is not captured, the message is sent to the
                ///     window beneath the cursor. Otherwise, the message is sent to the window that has
                ///     captured the mouse.
                /// </summary>
                NcHitTest = 0x84
            }
        }
    }
}
