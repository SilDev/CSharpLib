#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: MessageBoxEx.cs
// Version:  2020-01-13 13:04
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using Properties;

    /// <summary>
    ///     Displays a message window, also known as a dialog box, based on
    ///     <see cref="MessageBox"/>, which presents a message to the user. It is a
    ///     modal window, blocking other actions in the application until the user
    ///     closes it. A <see cref="MessageBoxEx"/> can contain text, buttons, and
    ///     symbols that inform and instruct the user.
    ///     <para>
    ///         The difference to <see cref="MessageBox"/> is that the message window
    ///         displays in the center of the specified <see cref="IWin32Window"/>
    ///         owner object.
    ///     </para>
    /// </summary>
    public static class MessageBoxEx
    {
        [ThreadStatic]
        private static IntPtr _hHook;

        [ThreadStatic]
        private static int _nButton;

        private static IWin32Window _owner;
        private static readonly WinApi.EnumChildProc EnumProc = MessageBoxEnumProc;
        private static readonly WinApi.HookProc HookProc = MessageBoxHookProc;
        static MessageBoxEx() => _hHook = IntPtr.Zero;

        /// <summary>
        ///     Specifies that the mouse pointer moves once to a new dialog box.
        /// </summary>
        public static bool CenterMousePointer { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="MessageBoxButtonText"/> structure that is used
        ///     when <see cref="ButtonTextOverrideEnabled"/> is enabled.
        /// </summary>
        public static MessageBoxButtonText ButtonText { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the override options should be
        ///     applied once at the next dialog box.
        /// </summary>
        public static bool ButtonTextOverrideEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether all dialog boxes are placed above
        ///     all non-topmost windows.
        ///     <para>
        ///         This option has no effect if an <see cref="IWin32Window"/> owner is
        ///         defined.
        ///     </para>
        /// </summary>
        public static bool TopMost { get; set; }

        /// <summary>
        ///     Displays a message box with the specified text, caption, buttons, icon,
        ///     default button, and options in the center of the specified owner object.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="caption">
        ///     The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies the
        ///     default button for the message box.
        /// </param>
        /// <param name="options">
        ///     One of the <see cref="MessageBoxOptions"/> values that specifies which
        ///     display and association options will be used for the message box. You may
        ///     pass in 0 if you wish to use the defaults.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            try
            {
                Initialize(owner);
                return MessageBox.Show(owner, text, caption, buttons, icon, defButton, options);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return Show(text, caption, buttons, icon, defButton, options);
            }
        }

        /// <summary>
        ///     Displays a message box with the specified text, caption, buttons, icon, and
        ///     default button in the center of the specified owner object.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="caption">
        ///     The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies the
        ///     default button for the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            try
            {
                Initialize(owner);
                return MessageBox.Show(owner, text, caption, buttons, icon, defButton);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return Show(text, caption, buttons, icon, defButton);
            }
        }

        /// <summary>
        ///     Displays a message box with the specified text, caption, buttons, and icon
        ///     in the center of the specified owner object.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="caption">
        ///     The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            try
            {
                Initialize(owner);
                return MessageBox.Show(owner, text, caption, buttons, icon);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return Show(text, caption, buttons, icon);
            }
        }

        /// <summary>
        ///     Displays a message box with the specified text, caption, and buttons in the
        ///     center of the specified owner object.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="caption">
        ///     The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            try
            {
                Initialize(owner);
                return MessageBox.Show(owner, text, caption, buttons);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return Show(text, caption, buttons);
            }
        }

        /// <summary>
        ///     Displays a message box with the specified text and caption in the center of
        ///     the specified owner object.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="caption">
        ///     The text to display in the title bar of the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            try
            {
                Initialize(owner);
                return MessageBox.Show(owner, text, caption);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return Show(text, caption);
            }
        }

        /// <summary>
        ///     Displays a message box with the specified text, buttons, icon, default
        ///     button, and options in the center of the specified owner object.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies the
        ///     default button for the message box.
        /// </param>
        /// <param name="options">
        ///     One of the <see cref="MessageBoxOptions"/> values that specifies which
        ///     display and association options will be used for the message box. You may
        ///     pass in 0 if you wish to use the defaults.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options) =>
            Show(owner, text, string.Empty, buttons, icon, defButton, options);

        /// <summary>
        ///     Displays a message box with the specified text, buttons, icon, and default
        ///     button in the center of the specified owner object.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies the
        ///     default button for the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton) =>
            Show(owner, text, string.Empty, buttons, icon, defButton);

        /// <summary>
        ///     Displays a message box with the specified text, buttons, and icon in the
        ///     center of the specified owner object.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxIcon icon) =>
            Show(owner, text, string.Empty, buttons, icon);

        /// <summary>
        ///     Displays a message box with the specified text, icon, and buttons in the
        ///     center of the specified owner object.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, MessageBoxButtons buttons) =>
            Show(owner, text, string.Empty, buttons);

        /// <summary>
        ///     Displays a message box with the specified text in the center of the
        ///     specified owner object.
        /// </summary>
        /// <param name="owner">
        ///     An implementation of <see cref="IWin32Window"/> that will own the modal
        ///     dialog box.
        /// </param>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            try
            {
                Initialize(owner);
                return MessageBox.Show(owner, text);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return Show(text);
            }
        }

        /// <summary>
        ///     Displays a message box with the specified text, caption, buttons, icon,
        ///     default button, and options.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="caption">
        ///     The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies the
        ///     default button for the message box.
        /// </param>
        /// <param name="options">
        ///     One of the <see cref="MessageBoxOptions"/> values that specifies which
        ///     display and association options will be used for the message box. You may
        ///     pass in 0 if you wish to use the defaults.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            Initialize();
            if (!TopMost)
                return MessageBox.Show(text, caption, buttons, icon, defButton, options);
            using var f = new Form
            {
                TopMost = true
            };
            return MessageBox.Show(f, text, caption, buttons, icon, defButton, options);
        }

        /// <summary>
        ///     Displays a message box with the specified text, caption, buttons, icon,
        ///     default, and button.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="caption">
        ///     The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies the
        ///     default button for the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            Initialize();
            if (!TopMost)
                return MessageBox.Show(text, caption, buttons, icon, defButton);
            using var f = new Form
            {
                TopMost = true
            };
            return MessageBox.Show(f, text, caption, buttons, icon, defButton);
        }

        /// <summary>
        ///     Displays a message box with the specified text, caption, buttons, and icon.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="caption">
        ///     The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Initialize();
            if (!TopMost)
                return MessageBox.Show(text, caption, buttons, icon);
            using var f = new Form
            {
                TopMost = true
            };
            return MessageBox.Show(f, text, caption, buttons, icon);
        }

        /// <summary>
        ///     Displays a message box with the specified text, caption, and buttons.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="caption">
        ///     The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            Initialize();
            if (!TopMost)
                return MessageBox.Show(text, caption, buttons);
            using var f = new Form
            {
                TopMost = true
            };
            return MessageBox.Show(f, text, caption, buttons);
        }

        /// <summary>
        ///     Displays a message box with the specified text and caption.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="caption">
        ///     The text to display in the title bar of the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, string caption)
        {
            Initialize();
            if (!TopMost)
                return MessageBox.Show(text, caption);
            using var f = new Form
            {
                TopMost = true
            };
            return MessageBox.Show(f, text, caption);
        }

        /// <summary>
        ///     Displays a message box with the specified text, buttons, icon, default
        ///     button, and options.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies the
        ///     default button for the message box.
        /// </param>
        /// <param name="options">
        ///     One of the <see cref="MessageBoxOptions"/> values that specifies which
        ///     display and association options will be used for the message box. You may
        ///     pass in 0 if you wish to use the defaults.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options) =>
            Show(text, string.Empty, buttons, icon, defButton, options);

        /// <summary>
        ///     Displays a message box with the specified text, buttons, icon, and default
        ///     button.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies the
        ///     default button for the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton) =>
            Show(text, string.Empty, buttons, icon, defButton);

        /// <summary>
        ///     Displays a message box with the specified text, buttons, and icon.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon to
        ///     display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon) =>
            Show(text, string.Empty, buttons, icon);

        /// <summary>
        ///     Displays a message box with the specified text, and buttons.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons) =>
            Show(text, string.Empty, buttons);

        /// <summary>
        ///     Displays a message box with the specified text.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text)
        {
            Initialize();
            if (!TopMost)
                return MessageBox.Show(text);
            using var f = new Form
            {
                TopMost = true
            };
            return MessageBox.Show(f, text);
        }

        private static void Initialize(IWin32Window owner = null)
        {
            try
            {
                if (_hHook != IntPtr.Zero)
                    throw new NotSupportedException(ExceptionMessages.MultipleCalls);
                if (owner != null)
                {
                    _owner = owner;
                    if (_owner.Handle != IntPtr.Zero)
                    {
                        var placement = new WinApi.WindowPlacement();
                        WinApi.NativeMethods.GetWindowPlacement(_owner.Handle, ref placement);
                        if (placement.ShowCmd == 2)
                            return;
                    }
                }
                if (_owner != null || ButtonTextOverrideEnabled)
                    _hHook = WinApi.NativeMethods.SetWindowsHookEx(WinApi.Win32HookFlags.WhCallWndProcRet, HookProc, IntPtr.Zero, (int)WinApi.NativeMethods.GetCurrentThreadId());
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }

        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return WinApi.NativeHelper.CallNextHookEx(nCode, wParam, lParam);
            var msg = (WinApi.CallWndProcRet)Marshal.PtrToStructure(lParam, typeof(WinApi.CallWndProcRet));
            if (msg.Message != (int)WinApi.Win32HookFlags.HCbtActivate)
            {
                if (msg.Message != (int)WinApi.WindowMenuFlags.WmInitDialog)
                    return WinApi.NativeHelper.CallNextHookEx(nCode, wParam, lParam);
                if (!ButtonTextOverrideEnabled)
                    return MessageBoxUnhookProc();
                try
                {
                    var className = new StringBuilder(10);
                    _ = WinApi.NativeMethods.GetClassName(msg.HWnd, className, className.Capacity);
                    if (className.ToStringThenClear() == "#32770")
                    {
                        _nButton = 0;
                        WinApi.NativeMethods.EnumChildWindows(msg.HWnd, EnumProc, IntPtr.Zero);
                        if (_nButton == 1)
                        {
                            var hButton = WinApi.NativeMethods.GetDlgItem(msg.HWnd, 2);
                            if (hButton != IntPtr.Zero)
                                WinApi.NativeMethods.SetWindowText(hButton, ButtonText.Ok);
                        }
                    }
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                return MessageBoxUnhookProc();
            }
            try
            {
                if (_owner != null)
                {
                    var cRect = new Rectangle(0, 0, 0, 0);
                    if (WinApi.NativeMethods.GetWindowRect(msg.HWnd, ref cRect))
                    {
                        var width = cRect.Width - cRect.X;
                        var height = cRect.Height - cRect.Y;
                        WinApi.NativeHelper.CenterWindow(msg.HWnd, _owner.Handle, true);
                        if (CenterMousePointer)
                            WinApi.NativeHelper.SetCursorPos(msg.HWnd, new Point(width / 2, height / 2 + 24));
                    }
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return !ButtonTextOverrideEnabled ? MessageBoxUnhookProc() : WinApi.NativeHelper.CallNextHookEx(nCode, wParam, lParam);
        }

        private static IntPtr MessageBoxUnhookProc()
        {
            _ = WinApi.NativeMethods.UnhookWindowsHookEx(_hHook);
            _hHook = IntPtr.Zero;
            _owner = null;
            if (ButtonTextOverrideEnabled)
                ButtonTextOverrideEnabled = false;
            return _hHook;
        }

        private static bool MessageBoxEnumProc(IntPtr hWnd, IntPtr lParam)
        {
            var className = new StringBuilder(10);
            _ = WinApi.NativeMethods.GetClassName(hWnd, className, className.Capacity);
            if (!className.ToStringThenClear().EqualsEx("Button"))
                return true;
            switch (WinApi.NativeMethods.GetDlgCtrlID(hWnd))
            {
                case 1:
                    if (ButtonText.Ok != null)
                        WinApi.NativeMethods.SetWindowText(hWnd, ButtonText.Ok);
                    break;
                case 2:
                    if (ButtonText.Cancel != null)
                        WinApi.NativeMethods.SetWindowText(hWnd, ButtonText.Cancel);
                    break;
                case 3:
                    if (ButtonText.Abort != null)
                        WinApi.NativeMethods.SetWindowText(hWnd, ButtonText.Abort);
                    break;
                case 4:
                    if (ButtonText.Retry != null)
                        WinApi.NativeMethods.SetWindowText(hWnd, ButtonText.Retry);
                    break;
                case 5:
                    if (ButtonText.Ignore != null)
                        WinApi.NativeMethods.SetWindowText(hWnd, ButtonText.Ignore);
                    break;
                case 6:
                    if (ButtonText.Yes != null)
                        WinApi.NativeMethods.SetWindowText(hWnd, ButtonText.Yes);
                    break;
                case 7:
                    if (ButtonText.No != null)
                        WinApi.NativeMethods.SetWindowText(hWnd, ButtonText.No);
                    break;
            }
            _nButton++;
            return true;
        }
    }
}
