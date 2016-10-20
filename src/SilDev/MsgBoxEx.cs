#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: MsgBoxEx.cs
// Version:  2016-10-18 23:33
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    ///     <para>
    ///         Displays a message window, also known as a dialog box, based on <see cref="MessageBox"/>,
    ///         which presents a message to the user. It is a modal window, blocking other actions in the
    ///         application until the user closes it. A <see cref="MsgBoxEx"/> can contain text, buttons,
    ///         and symbols that inform and instruct the user.
    ///     </para>
    ///     <para>
    ///         The difference to <see cref="MessageBox"/> is that the message window displays
    ///         in the center of the specified <see cref="IWin32Window"/> owner object.
    ///     </para>
    /// </summary>
    public static class MsgBoxEx
    {
        private static IWin32Window _owner;
        private static readonly WinApi.HookProc HookProc;
        private static readonly WinApi.EnumChildProc EnumProc;
        [ThreadStatic] private static IntPtr _hHook;
        [ThreadStatic] private static int _nButton;

        /// <summary>
        ///     Specifies that the mouse pointer moves once to a new dialog box.
        /// </summary>
        public static bool CenterMousePointer = false;

        static MsgBoxEx()
        {
            HookProc = MessageBoxHookProc;
            EnumProc = MessageBoxEnumProc;
            _hHook = IntPtr.Zero;
        }

        /// <summary>
        ///     Displays a message box in the center of the specified object and with the
        ///     specified text, caption, buttons, icon, default button, and options.
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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies
        ///     the default button for the message box.
        /// </param>
        /// <param name="options">
        ///     One of the <see cref="MessageBoxOptions"/> values that specifies which
        ///     display and association options will be used for the message box. You may pass
        ///     in 0 if you wish to use the defaults.
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
            catch (Exception ex)
            {
                Log.Write(ex);
                Initialize();
                return MessageBox.Show(text, caption, buttons, icon, defButton, options);
            }
        }

        /// <summary>
        ///     Displays a message box in the center of the specified object and  with the
        ///     specified text, caption, buttons, icon, default, and button.
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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies
        ///     the default button for the message box.
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
            catch (Exception ex)
            {
                Log.Write(ex);
                Initialize();
                return MessageBox.Show(text, caption, buttons, icon, defButton);
            }
        }

        /// <summary>
        ///     Displays a message box in the center of the specified object and with the
        ///     specified text, caption, buttons, and icon.
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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
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
            catch (Exception ex)
            {
                Log.Write(ex);
                Initialize();
                return MessageBox.Show(text, caption, buttons, icon);
            }
        }

        /// <summary>
        ///     Displays a message box in the center of the specified object and with the
        ///     specified text, caption, and buttons.
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
            catch (Exception ex)
            {
                Log.Write(ex);
                Initialize();
                return MessageBox.Show(text, caption, buttons);
            }
        }

        /// <summary>
        ///     Displays a message box in the center of the specified object and with the
        ///     specified text and caption.
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
            catch (Exception ex)
            {
                Log.Write(ex);
                Initialize();
                return MessageBox.Show(text, caption);
            }
        }

        /// <summary>
        ///     Displays a message box in the center of the specified object and  with the
        ///     specified text.
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
            catch (Exception ex)
            {
                Log.Write(ex);
                Initialize();
                return MessageBox.Show(text);
            }
        }

        /// <summary>
        ///     Displays a message box in the center of the specified object and with the
        ///     specified text, buttons, icon, default button, and options.
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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies
        ///     the default button for the message box.
        /// </param>
        /// <param name="options">
        ///     One of the <see cref="MessageBoxOptions"/> values that specifies which
        ///     display and association options will be used for the message box. You may pass
        ///     in 0 if you wish to use the defaults.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            try
            {
                Initialize(owner);
                return MessageBox.Show(owner, text, string.Empty, buttons, icon, defButton, options);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Initialize();
                return MessageBox.Show(text, string.Empty, buttons, icon, defButton, options);
            }
        }

        /// <summary>
        ///     Displays a message box in the center of the specified object and with the
        ///     specified text, buttons, icon, and default button.
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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies
        ///     the default button for the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            try
            {
                Initialize(owner);
                return MessageBox.Show(owner, text, string.Empty, buttons, icon, defButton);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Initialize();
                return MessageBox.Show(text, string.Empty, buttons, icon, defButton);
            }
        }

        /// <summary>
        ///     Displays a message box in the center of the specified object and with the
        ///     specified text, buttons, and icon.
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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(IWin32Window owner, string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            try
            {
                Initialize(owner);
                return MessageBox.Show(owner, text, string.Empty, buttons, icon);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Initialize();
                return MessageBox.Show(text, string.Empty, buttons, icon);
            }
        }

        /// <summary>
        ///     Displays a message box in the center of the specified object and with the
        ///     specified text, and buttons.
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
        public static DialogResult Show(IWin32Window owner, string text, MessageBoxButtons buttons)
        {
            try
            {
                Initialize(owner);
                return MessageBox.Show(owner, text, string.Empty, buttons);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Initialize();
                return MessageBox.Show(text, string.Empty, buttons);
            }
        }

        /// <summary>
        ///     Displays a message box with the specified text, caption, buttons, icon, default
        ///     button, and options.
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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies
        ///     the default button for the message box.
        /// </param>
        /// <param name="options">
        ///     One of the <see cref="MessageBoxOptions"/> values that specifies which
        ///     display and association options will be used for the message box. You may pass
        ///     in 0 if you wish to use the defaults.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton, options);
        }

        /// <summary>
        ///     Displays a message box with the specified text, caption, buttons, icon, default,
        ///     and button.
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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies
        ///     the default button for the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton);
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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon);
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
            return MessageBox.Show(text, caption, buttons);
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
            return MessageBox.Show(text, caption);
        }

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
            return MessageBox.Show(text);
        }

        /// <summary>
        ///     Displays a message box with the specified text, buttons, icon, default button,
        ///     and options.
        /// </summary>
        /// <param name="text">
        ///     The text to display in the message box.
        /// </param>
        /// <param name="buttons">
        ///     One of the <see cref="MessageBoxButtons"/> values that specifies which
        ///     buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies
        ///     the default button for the message box.
        /// </param>
        /// <param name="options">
        ///     One of the <see cref="MessageBoxOptions"/> values that specifies which
        ///     display and association options will be used for the message box. You may pass
        ///     in 0 if you wish to use the defaults.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            Initialize();
            return MessageBox.Show(text, string.Empty, buttons, icon, defButton, options);
        }

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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <param name="defButton">
        ///     One of the <see cref="MessageBoxDefaultButton"/> values that specifies
        ///     the default button for the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            Initialize();
            return MessageBox.Show(text, string.Empty, buttons, icon, defButton);
        }

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
        ///     One of the <see cref="MessageBoxIcon"/> values that specifies which icon
        ///     to display in the message box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="DialogResult"/> values.
        /// </returns>
        public static DialogResult Show(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Initialize();
            return MessageBox.Show(text, string.Empty, buttons, icon);
        }

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
        public static DialogResult Show(string text, MessageBoxButtons buttons)
        {
            Initialize();
            return MessageBox.Show(text, string.Empty, buttons);
        }

        private static void Initialize(IWin32Window owner = null)
        {
            try
            {
                if (_hHook != IntPtr.Zero)
                    throw new NotSupportedException("Multiple calls are not supported.");
                if (owner != null)
                {
                    _owner = owner;
                    if (_owner.Handle != IntPtr.Zero)
                    {
                        var placement = new WinApi.WINDOWPLACEMENT();
                        WinApi.UnsafeNativeMethods.GetWindowPlacement(_owner.Handle, ref placement);
                        if (placement.showCmd == 2)
                            return;
                    }
                }
                if (_owner != null || ButtonText.OverrideEnabled)
                    _hHook = WinApi.UnsafeNativeMethods.SetWindowsHookEx(WinApi.Win32HookFunc.WH_CALLWNDPROCRET, HookProc, IntPtr.Zero, (int)WinApi.UnsafeNativeMethods.GetCurrentThreadId());
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return WinApi.UnsafeNativeMethods.CallNextHookEx(_hHook, nCode, wParam, lParam);
            var msg = (WinApi.CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(WinApi.CWPRETSTRUCT));
            var hook = _hHook;
            if (msg.message != (int)WinApi.Win32HookFunc.HCBT_ACTIVATE)
            {
                if (msg.message != (int)WinApi.WindowMenuFunc.WM_INITDIALOG)
                    return WinApi.UnsafeNativeMethods.CallNextHookEx(hook, nCode, wParam, lParam);
                if (!ButtonText.OverrideEnabled)
                    return MessageBoxUnhookProc();
                try
                {
                    var className = new StringBuilder(10);
                    WinApi.UnsafeNativeMethods.GetClassName(msg.hwnd, className, className.Capacity);
                    if (className.ToString() == "#32770")
                    {
                        _nButton = 0;
                        WinApi.UnsafeNativeMethods.EnumChildWindows(msg.hwnd, EnumProc, IntPtr.Zero);
                        if (_nButton == 1)
                        {
                            var hButton = WinApi.UnsafeNativeMethods.GetDlgItem(msg.hwnd, 2);
                            if (hButton != IntPtr.Zero)
                                WinApi.UnsafeNativeMethods.SetWindowText(hButton, ButtonText.OK);
                        }
                    }
                }
                catch (Exception ex)
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
                    if (WinApi.UnsafeNativeMethods.GetWindowRect(msg.hwnd, ref cRect))
                    {
                        var width = cRect.Width - cRect.X;
                        var height = cRect.Height - cRect.Y;
                        var pRect = new Rectangle(0, 0, 0, 0);
                        if (WinApi.UnsafeNativeMethods.GetWindowRect(_owner.Handle, ref pRect))
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
                            WinApi.UnsafeNativeMethods.MoveWindow(msg.hwnd, ptStart.X, ptStart.Y, width, height, false);
                            if (CenterMousePointer)
                                WinApi.SetCursorPos(msg.hwnd, new Point(width / 2, height / 2 + 24));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return !ButtonText.OverrideEnabled ? MessageBoxUnhookProc() : WinApi.UnsafeNativeMethods.CallNextHookEx(hook, nCode, wParam, lParam);
        }

        private static IntPtr MessageBoxUnhookProc()
        {
            WinApi.UnsafeNativeMethods.UnhookWindowsHookEx(_hHook);
            _hHook = IntPtr.Zero;
            _owner = null;
            if (ButtonText.OverrideEnabled)
                ButtonText.OverrideEnabled = false;
            return _hHook;
        }

        private static bool MessageBoxEnumProc(IntPtr hWnd, IntPtr lParam)
        {
            var className = new StringBuilder(10);
            WinApi.UnsafeNativeMethods.GetClassName(hWnd, className, className.Capacity);
            if (className.ToString() != "Button")
                return true;
            switch (WinApi.UnsafeNativeMethods.GetDlgCtrlID(hWnd))
            {
                case 1:
                    WinApi.UnsafeNativeMethods.SetWindowText(hWnd, ButtonText.OK);
                    break;
                case 2:
                    WinApi.UnsafeNativeMethods.SetWindowText(hWnd, ButtonText.Cancel);
                    break;
                case 3:
                    WinApi.UnsafeNativeMethods.SetWindowText(hWnd, ButtonText.Abort);
                    break;
                case 4:
                    WinApi.UnsafeNativeMethods.SetWindowText(hWnd, ButtonText.Retry);
                    break;
                case 5:
                    WinApi.UnsafeNativeMethods.SetWindowText(hWnd, ButtonText.Ignore);
                    break;
                case 6:
                    WinApi.UnsafeNativeMethods.SetWindowText(hWnd, ButtonText.Yes);
                    break;
                case 7:
                    WinApi.UnsafeNativeMethods.SetWindowText(hWnd, ButtonText.No);
                    break;
            }
            _nButton++;
            return true;
        }

        /// <summary>
        ///     Specifies button text overrides.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public static class ButtonText
        {
            /// <summary>
            ///     The OK button.
            /// </summary>
            public static string OK = "&OK";

            /// <summary>
            ///     The Cancel button.
            /// </summary>
            public static string Cancel = "&Cancel";

            /// <summary>
            ///     The Abort button.
            /// </summary>
            public static string Abort = "&Abort";

            /// <summary>
            ///     The Retry button.
            /// </summary>
            public static string Retry = "&Retry";

            /// <summary>
            ///     The Ignore button.
            /// </summary>
            public static string Ignore = "&Ignore";

            /// <summary>
            ///     The Yes button.
            /// </summary>
            public static string Yes = "&Yes";

            /// <summary>
            ///     The No button.
            /// </summary>
            public static string No = "&No";

            /// <summary>
            ///     Gets or sets a value indicating whether the override options to
            ///     be applied.
            /// </summary>
            public static bool OverrideEnabled { get; set; }
        }
    }
}
