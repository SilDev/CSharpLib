#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: InputDevice.cs
// Version:  2019-06-16 11:02
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     Provides enumerated values of Virtual-Key codes.
    /// </summary>
    public enum VirtualKeys
    {
        /// <summary>
        ///     Left mouse button.
        /// </summary>
        LButton = 0x1,

        /// <summary>
        ///     Right mouse button.
        /// </summary>
        RButton = 0x2,

        /// <summary>
        ///     Control-break processing.
        /// </summary>
        Cancel = 0x3,

        /// <summary>
        ///     Middle mouse button (three-button mouse).
        /// </summary>
        MButton = 0x4,

        /// <summary>
        ///     X1 mouse button.
        /// </summary>
        XButton1 = 0x5,

        /// <summary>
        ///     X2 mouse button.
        /// </summary>
        XButton2 = 0x6,

        /// <summary>
        ///     BACKSPACE key.
        /// </summary>
        Back = 0x8,

        /// <summary>
        ///     TAB key.
        /// </summary>
        Tab = 0x9,

        /// <summary>
        ///     CLEAR key.
        /// </summary>
        Clear = 0xc,

        /// <summary>
        ///     ENTER key.
        /// </summary>
        Return = 0xd,

        /// <summary>
        ///     SHIFT key.
        /// </summary>
        Shift = 0x10,

        /// <summary>
        ///     CTRL key.
        /// </summary>
        Control = 0x11,

        /// <summary>
        ///     ALT key.
        /// </summary>
        Menu = 0x12,

        /// <summary>
        ///     PAUSE key.
        /// </summary>
        Pause = 0x13,

        /// <summary>
        ///     CAPS LOCK key.
        /// </summary>
        Capital = 0x14,

        /// <summary>
        ///     IME Kana/Hangul mode.
        /// </summary>
        KanaHangul = 0x15,

        /// <summary>
        ///     IME Junja mode.
        /// </summary>
        Junja = 0x17,

        /// <summary>
        ///     IME final mode.
        /// </summary>
        Final = 0x18,

        /// <summary>
        ///     IME Hanja/Kanji mode.
        /// </summary>
        HanjaKanji = 0x19,

        /// <summary>
        ///     ESC key
        /// </summary>
        Escape = 0x1b,

        /// <summary>
        ///     IME convert.
        /// </summary>
        Convert = 0x1c,

        /// <summary>
        ///     IME non-convert.
        /// </summary>
        NonConvert = 0x1d,

        /// <summary>
        ///     IME accept.
        /// </summary>
        Accept = 0x1e,

        /// <summary>
        ///     IME mode change request.
        /// </summary>
        ModeChange = 0x1f,

        /// <summary>
        ///     SPACEBAR key.
        /// </summary>
        Space = 0x20,

        /// <summary>
        ///     PAGE UP key.
        /// </summary>
        Prior = 0x21,

        /// <summary>
        ///     PAGE DOWN key.
        /// </summary>
        Next = 0x22,

        /// <summary>
        ///     END key.
        /// </summary>
        End = 0x23,

        /// <summary>
        ///     HOME key.
        /// </summary>
        Home = 0x24,

        /// <summary>
        ///     LEFT ARROW key.
        /// </summary>
        Left = 0x25,

        /// <summary>
        ///     UP ARROW key.
        /// </summary>
        Up = 0x26,

        /// <summary>
        ///     RIGHT ARROW key.
        /// </summary>
        Right = 0x27,

        /// <summary>
        ///     DOWN ARROW key.
        /// </summary>
        Down = 0x28,

        /// <summary>
        ///     SELECT key.
        /// </summary>
        Select = 0x29,

        /// <summary>
        ///     PRINT key.
        /// </summary>
        Print = 0x2a,

        /// <summary>
        ///     EXECUTE key.
        /// </summary>
        Execute = 0x2b,

        /// <summary>
        ///     PRINT SCREEN key.
        /// </summary>
        Snapshot = 0x2c,

        /// <summary>
        ///     INS key.
        /// </summary>
        Insert = 0x2d,

        /// <summary>
        ///     DEL key.
        /// </summary>
        Delete = 0x2e,

        /// <summary>
        ///     HELP key.
        /// </summary>
        Help = 0x2f,

        /// <summary>
        ///     0 key.
        /// </summary>
        Num0 = 0x30,

        /// <summary>
        ///     1 key.
        /// </summary>
        Num1 = 0x31,

        /// <summary>
        ///     2 key.
        /// </summary>
        Num2 = 0x32,

        /// <summary>
        ///     3 key.
        /// </summary>
        Num3 = 0x33,

        /// <summary>
        ///     4 key.
        /// </summary>
        Num4 = 0x34,

        /// <summary>
        ///     5 key.
        /// </summary>
        Num5 = 0x35,

        /// <summary>
        ///     6 key.
        /// </summary>
        Num6 = 0x36,

        /// <summary>
        ///     7 key.
        /// </summary>
        Num7 = 0x37,

        /// <summary>
        ///     8 key.
        /// </summary>
        Num8 = 0x38,

        /// <summary>
        ///     9 key.
        /// </summary>
        Num9 = 0x39,

        /// <summary>
        ///     A key.
        /// </summary>
        A = 0x41,

        /// <summary>
        ///     B key.
        /// </summary>
        B = 0x42,

        /// <summary>
        ///     C key.
        /// </summary>
        C = 0x43,

        /// <summary>
        ///     D key.
        /// </summary>
        D = 0x44,

        /// <summary>
        ///     E key.
        /// </summary>
        E = 0x45,

        /// <summary>
        ///     F key.
        /// </summary>
        F = 0x46,

        /// <summary>
        ///     G key.
        /// </summary>
        G = 0x47,

        /// <summary>
        ///     H key.
        /// </summary>
        H = 0x48,

        /// <summary>
        ///     I key.
        /// </summary>
        I = 0x49,

        /// <summary>
        ///     J key.
        /// </summary>
        J = 0x4a,

        /// <summary>
        ///     K key.
        /// </summary>
        K = 0x4b,

        /// <summary>
        ///     L key.
        /// </summary>
        L = 0x4c,

        /// <summary>
        ///     M key.
        /// </summary>
        M = 0x4d,

        /// <summary>
        ///     N key.
        /// </summary>
        N = 0x4e,

        /// <summary>
        ///     O key.
        /// </summary>
        O = 0x4f,

        /// <summary>
        ///     P key.
        /// </summary>
        P = 0x50,

        /// <summary>
        ///     Q key.
        /// </summary>
        Q = 0x51,

        /// <summary>
        ///     R key.
        /// </summary>
        R = 0x52,

        /// <summary>
        ///     S key.
        /// </summary>
        S = 0x53,

        /// <summary>
        ///     T key.
        /// </summary>
        T = 0x54,

        /// <summary>
        ///     U key.
        /// </summary>
        U = 0x55,

        /// <summary>
        ///     V key.
        /// </summary>
        V = 0x56,

        /// <summary>
        ///     W key.
        /// </summary>
        W = 0x57,

        /// <summary>
        ///     X key.
        /// </summary>
        X = 0x58,

        /// <summary>
        ///     Y key.
        /// </summary>
        Y = 0x59,

        /// <summary>
        ///     Z key.
        /// </summary>
        Z = 0x5a,

        /// <summary>
        ///     Left Windows key (natural keyboard).
        /// </summary>
        LWin = 0x5b,

        /// <summary>
        ///     Right Windows key (natural keyboard).
        /// </summary>
        RWin = 0x5c,

        /// <summary>
        ///     Applications key (natural keyboard).
        /// </summary>
        Apps = 0x5d,

        /// <summary>
        ///     Computer Sleep key.
        /// </summary>
        Sleep = 0x5f,

        /// <summary>
        ///     Numeric keypad 0 key.
        /// </summary>
        Numpad0 = 0x60,

        /// <summary>
        ///     Numeric keypad 1 key.
        /// </summary>
        Numpad1 = 0x61,

        /// <summary>
        ///     Numeric keypad 2 key.
        /// </summary>
        Numpad2 = 0x62,

        /// <summary>
        ///     Numeric keypad 3 key.
        /// </summary>
        Numpad3 = 0x63,

        /// <summary>
        ///     Numeric keypad 4 key.
        /// </summary>
        Numpad4 = 0x64,

        /// <summary>
        ///     Numeric keypad 5 key.
        /// </summary>
        Numpad5 = 0x65,

        /// <summary>
        ///     Numeric keypad 6 key.
        /// </summary>
        Numpad6 = 0x66,

        /// <summary>
        ///     Numeric keypad 7 key.
        /// </summary>
        Numpad7 = 0x67,

        /// <summary>
        ///     Numeric keypad 8 key.
        /// </summary>
        Numpad8 = 0x68,

        /// <summary>
        ///     Numeric keypad 9 key.
        /// </summary>
        Numpad9 = 0x69,

        /// <summary>
        ///     Multiply key.
        /// </summary>
        Multiply = 0x6a,

        /// <summary>
        ///     Add key.
        /// </summary>
        Add = 0x6b,

        /// <summary>
        ///     Separator key.
        /// </summary>
        Separator = 0x6c,

        /// <summary>
        ///     Subtract key.
        /// </summary>
        Subtract = 0x6d,

        /// <summary>
        ///     Decimal key.
        /// </summary>
        Decimal = 0x6e,

        /// <summary>
        ///     Divide key.
        /// </summary>
        Divide = 0x6f,

        /// <summary>
        ///     F1 key.
        /// </summary>
        F1 = 0x70,

        /// <summary>
        ///     F2 key.
        /// </summary>
        F2 = 0x71,

        /// <summary>
        ///     F3 key.
        /// </summary>
        F3 = 0x72,

        /// <summary>
        ///     F4 key.
        /// </summary>
        F4 = 0x73,

        /// <summary>
        ///     F5 key.
        /// </summary>
        F5 = 0x74,

        /// <summary>
        ///     F6 key.
        /// </summary>
        F6 = 0x75,

        /// <summary>
        ///     F7 key.
        /// </summary>
        F7 = 0x76,

        /// <summary>
        ///     F8 key.
        /// </summary>
        F8 = 0x77,

        /// <summary>
        ///     F9 key.
        /// </summary>
        F9 = 0x78,

        /// <summary>
        ///     F10 key.
        /// </summary>
        F10 = 0x79,

        /// <summary>
        ///     F11 key.
        /// </summary>
        F11 = 0x7a,

        /// <summary>
        ///     F12 key.
        /// </summary>
        F12 = 0x7b,

        /// <summary>
        ///     F13 key.
        /// </summary>
        F13 = 0x7c,

        /// <summary>
        ///     F14 key.
        /// </summary>
        F14 = 0x7d,

        /// <summary>
        ///     F15 key.
        /// </summary>
        F15 = 0x7e,

        /// <summary>
        ///     F16 key.
        /// </summary>
        F16 = 0x7f,

        /// <summary>
        ///     F17 key.
        /// </summary>
        F17 = 0x80,

        /// <summary>
        ///     F18 key.
        /// </summary>
        F18 = 0x81,

        /// <summary>
        ///     F19 key.
        /// </summary>
        F19 = 0x82,

        /// <summary>
        ///     F20 key.
        /// </summary>
        F20 = 0x83,

        /// <summary>
        ///     F21 key.
        /// </summary>
        F21 = 0x84,

        /// <summary>
        ///     F22 key.
        /// </summary>
        F22 = 0x85,

        /// <summary>
        ///     F23 key.
        /// </summary>
        F23 = 0x86,

        /// <summary>
        ///     F24 key.
        /// </summary>
        F24 = 0x87,

        /// <summary>
        ///     NUM LOCK key.
        /// </summary>
        NumLock = 0x90,

        /// <summary>
        ///     SCROLL LOCK key.
        /// </summary>
        Scroll = 0x91,

        /// <summary>
        ///     Left SHIFT key.
        /// </summary>
        LShift = 0xa0,

        /// <summary>
        ///     Right SHIFT key.
        /// </summary>
        RShift = 0xa1,

        /// <summary>
        ///     Left CONTROL key.
        /// </summary>
        LControl = 0xa2,

        /// <summary>
        ///     Right CONTROL key.
        /// </summary>
        RControl = 0xa3,

        /// <summary>
        ///     Left MENU key.
        /// </summary>
        LMenu = 0xa4,

        /// <summary>
        ///     Right MENU key.
        /// </summary>
        RMenu = 0xa5,

        /// <summary>
        ///     Browser Back key.
        /// </summary>
        BrowserBack = 0xa6,

        /// <summary>
        ///     Browser Forward key.
        /// </summary>
        BrowserForward = 0xa7,

        /// <summary>
        ///     Browser Refresh key.
        /// </summary>
        BrowserRefresh = 0xa8,

        /// <summary>
        ///     Browser Stop key.
        /// </summary>
        BrowserStop = 0xa9,

        /// <summary>
        ///     Browser Search key.
        /// </summary>
        BrowserSearch = 0xaa,

        /// <summary>
        ///     Browser Favorites key.
        /// </summary>
        BrowserFavorites = 0xab,

        /// <summary>
        ///     Browser Start and Home key.
        /// </summary>
        BrowserHome = 0xac,

        /// <summary>
        ///     Volume Mute key.
        /// </summary>
        VolumeMute = 0xad,

        /// <summary>
        ///     Volume Down key.
        /// </summary>
        VolumeDown = 0xae,

        /// <summary>
        ///     Volume Up key.
        /// </summary>
        VolumeUp = 0xaf,

        /// <summary>
        ///     Next Track key.
        /// </summary>
        MediaNextTrack = 0xb0,

        /// <summary>
        ///     Previous Track key.
        /// </summary>
        MediaPrevTrack = 0xb1,

        /// <summary>
        ///     Stop Media key.
        /// </summary>
        MediaStop = 0xb2,

        /// <summary>
        ///     Play/Pause Media key.
        /// </summary>
        MediaPlayPause = 0xb3,

        /// <summary>
        ///     Start Mail key.
        /// </summary>
        LaunchMail = 0xb4,

        /// <summary>
        ///     Select Media key.
        /// </summary>
        LaunchMediaSelect = 0xb5,

        /// <summary>
        ///     Select Media key.
        /// </summary>
        LaunchApp1 = 0xb6,

        /// <summary>
        ///     Start Application 2 key.
        /// </summary>
        LaunchApp2 = 0xb7,

        /// <summary>
        ///     For the US standard keyboard, the ",:" key .
        /// </summary>
        Oem1 = 0xba,

        /// <summary>
        ///     For any country/region, the "+" key.
        /// </summary>
        OemPlus = 0xbb,

        /// <summary>
        ///     For any country/region, the "," key.
        /// </summary>
        OemComma = 0xbc,

        /// <summary>
        ///     For any country/region, the "-" key.
        /// </summary>
        OemMinus = 0xbd,

        /// <summary>
        ///     For any country/region, the "." key.
        /// </summary>
        OemPeriod = 0xbe,

        /// <summary>
        ///     For the US standard keyboard, the "/?" key.
        /// </summary>
        Oem2 = 0xbf,

        /// <summary>
        ///     For the US standard keyboard, the "`~" key.
        /// </summary>
        Oem3 = 0xc0,

        /// <summary>
        ///     For the US standard keyboard, the "[{" key.
        /// </summary>
        Oem4 = 0xdb,

        /// <summary>
        ///     For the US standard keyboard, the "\|" key.
        /// </summary>
        Oem5 = 0xdc,

        /// <summary>
        ///     For the US standard keyboard, the "]}" key.
        /// </summary>
        Oem6 = 0xdd,

        /// <summary>
        ///     For the US standard keyboard, the "single-quote/double-quote" key
        /// </summary>
        Oem7 = 0xdf,

        /// <summary>
        ///     Used for miscellaneous characters, it can vary by keyboard.
        /// </summary>
        Oem8 = 0xe1,

        /// <summary>
        ///     Either the angle bracket key or the backslash key on the RT 102-key keyboard.
        /// </summary>
        Oem102 = 0xe2,

        /// <summary>
        ///     IME PROCESS key.
        /// </summary>
        ProcessKey = 0xe5,

        /// <summary>
        ///     Used to pass Unicode characters as if they were keystrokes. The <see cref="Packet"/>
        ///     key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods.
        /// </summary>
        Packet = 0xe7,

        /// <summary>
        ///     Attn key.
        /// </summary>
        Attn = 0xf6,

        /// <summary>
        ///     CrSel key.
        /// </summary>
        CrSel = 0xf7,

        /// <summary>
        ///     ExSel key.
        /// </summary>
        ExSel = 0xf8,

        /// <summary>
        ///     Erase EOF key.
        /// </summary>
        ErEof = 0xf9,

        /// <summary>
        ///     Play key.
        /// </summary>
        Play = 0xfa,

        /// <summary>
        ///     Zoom key.
        /// </summary>
        Zoom = 0xfb,

        /// <summary>
        ///     PA1 key.
        /// </summary>
        Pa1 = 0xfd,

        /// <summary>
        ///     Clear key.
        /// </summary>
        OemClear = 0xfe
    }

    /// <summary>
    ///     Provides enumerated values of Virtual-Key code states.
    /// </summary>
    public enum VirtualKeyStates
    {
        /// <summary>
        ///     Posted to the window with the keyboard focus when a nonsystem key is pressed.
        /// </summary>
        KeyDown = 0x100,

        /// <summary>
        ///     Posted to the window with the keyboard focus when a nonsystem key is released.
        /// </summary>
        KeyUp = 0x101,

        /// <summary>
        ///     Posted to the window with the keyboard focus when the user presses the F10 key (which
        ///     activates the menu bar) or holds down the ALT key and then presses another key.
        /// </summary>
        SysKeyDown = 0x104,

        /// <summary>
        ///     Posted to the window with the keyboard focus when the user releases a key that was
        ///     pressed while the ALT key was held down.
        /// </summary>
        SysKeyUp = 0x105
    }

    /// <summary>
    ///     Provides the functionality to send or detect key states.
    /// </summary>
    public static class InputDevice
    {
        /// <summary>
        ///     Returns the <see cref="VirtualKeys"/> of the <see cref="ushort"/> representation of a
        ///     Virtual-Key code.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="VirtualKeys"/> value.
        /// </param>
        public static VirtualKeys GetKey(ushort key) =>
            (VirtualKeys)key;

        /// <summary>
        ///     Returns the <see cref="VirtualKeys"/> of the <see cref="string"/> representation of a
        ///     Virtual-Key code.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="string"/> representation of a Virtual-Key code.
        /// </param>
        public static VirtualKeys GetKey(string key) =>
            (VirtualKeys)GetKeyCode(key);

        /// <summary>
        ///     Returns the <see cref="ushort"/> representation of the <see cref="VirtualKeys"/> value.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="VirtualKeys"/> value.
        /// </param>
        public static ushort GetKeyCode(VirtualKeys key) =>
            (ushort)key;

        /// <summary>
        ///     Returns the <see cref="ushort"/> representation of the <see cref="string"/> representation
        ///     of a Virtual-Key code.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="string"/> representation of a Virtual-Key code.
        /// </param>
        public static ushort GetKeyCode(string key)
        {
            if (Enum.TryParse(key, out VirtualKeys vkey))
                return (ushort)vkey;
            return 0;
        }

        /// <summary>
        ///     Returns the <see cref="string"/> representation of the <see cref="VirtualKeys"/> value.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="VirtualKeys"/> value.
        /// </param>
        public static string GetKeyName(VirtualKeys key) =>
            Enum.GetName(typeof(VirtualKeys), key);

        /// <summary>
        ///     Returns the <see cref="string"/> representation of the <see cref="ushort"/> representation
        ///     of a Virtual-Key code.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="ushort"/> representation of a Virtual-Key code.
        /// </param>
        public static string GetKeyName(ushort key) =>
            GetKey(key).ToString();

        /// <summary>
        ///     Returns the Virtual-Key scan code.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="VirtualKeys"/> value.
        /// </param>
        public static uint GetScanCode(VirtualKeys key, bool extended = false)
        {
            var code = 1u | (WinApi.NativeMethods.MapVirtualKey((uint)key, 0u) << 16);
            if (extended)
                code |= 0x1000000;
            return code;
        }

        /// <summary>
        ///     Returns the Virtual-Key scan code.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="ushort"/> representation of a Virtual-Key code.
        /// </param>
        public static uint GetScanCode(ushort key, bool extended = false) =>
            GetScanCode(GetKey(key));

        /// <summary>
        ///     Determines whether a key is up or down at the time the function is called, and whether the
        ///     key was pressed after a previous call to <see cref="GetKeyState(VirtualKeys)"/>.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="VirtualKeys"/> value to check.
        /// </param>
        public static bool GetKeyState(VirtualKeys key) =>
            WinApi.NativeMethods.GetAsyncKeyState((ushort)key) < 0;

        /// <summary>
        ///     Determines whether a key is up or down at the time the function is called, and whether the
        ///     key was pressed after a previous call to <see cref="GetKeyState(ushort)"/>.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="ushort"/> representation of a Virtual-Key code to check.
        /// </param>
        public static bool GetKeyState(ushort key) =>
            WinApi.NativeMethods.GetAsyncKeyState(key) < 0;

        /// <summary>
        ///     Determines whether a key is up or down at the time the function is called, and whether the
        ///     key was pressed after a previous call to <see cref="GetKeyState(string)"/>.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="string"/> representation of a Virtual-Key code to check.
        /// </param>
        public static bool GetKeyState(string key) =>
            WinApi.NativeMethods.GetAsyncKeyState(GetKeyCode(key)) < 0;

        /// <summary>
        ///     Determines which keys were up or down at the time the function is called, and which keys
        ///     were pressed.
        /// </summary>
        public static IEnumerable<VirtualKeys> GetKeyStates()
        {
            var keys = Enum.GetValues(typeof(VirtualKeys)).Cast<VirtualKeys>();
            foreach (var key in keys)
                if (WinApi.NativeMethods.GetAsyncKeyState(GetKeyCode(key)) < 0)
                    yield return key;
        }

        /// <summary>
        ///     Places (posts) the specified key in the message queue associated with the thread that created
        ///     the specified window and returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window whose window procedure is to receive the message.
        /// </param>
        /// <param name="key">
        ///     The key to post.
        /// </param>
        /// <param name="keyState">
        ///     The key state to post.
        /// </param>
        /// <param name="scanCode">
        ///     true to post the scan code of the specified key; otherwise, false.
        /// </param>
        public static bool PostKeyState(IntPtr hWnd, VirtualKeys key, VirtualKeyStates keyState, bool scanCode = false)
        {
            var wParam = scanCode ? 0u : GetKeyCode(key);
            var lParam = !scanCode ? 0u : GetScanCode(key);
            return WinApi.NativeHelper.PostMessage(hWnd, (uint)keyState, (IntPtr)wParam, (IntPtr)lParam);
        }

        /// <summary>
        ///     Sends the specified key to a window. This function calls the window procedure for the
        ///     specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window whose window procedure is to receive the message.
        /// </param>
        /// <param name="key">
        ///     The key to send.
        /// </param>
        /// <param name="keyState">
        ///     The key state to send.
        /// </param>
        /// <param name="scanCode">
        ///     true to send the scan code of the specified key; otherwise, false.
        /// </param>
        public static IntPtr SendKeyState(IntPtr hWnd, VirtualKeys key, VirtualKeyStates keyState, bool scanCode = false)
        {
            var wParam = scanCode ? 0u : GetKeyCode(key);
            var lParam = !scanCode ? 0u : GetScanCode(key);
            return WinApi.NativeHelper.SendMessage(hWnd, (uint)keyState, (IntPtr)wParam, (IntPtr)lParam);
        }

        /// <summary>
        ///     Synthesizes a left mouse button click to the active window.
        /// </summary>
        /// <param name="directInput">
        /// </param>
        public static void SendMouseClick(bool directInput = true)
        {
            if (!directInput)
            {
                var hWnd = WinApi.NativeMethods.GetForegroundWindow();
                SendKeyState(hWnd, VirtualKeys.LButton, VirtualKeyStates.KeyDown, true);
                SendKeyState(hWnd, VirtualKeys.LButton, VirtualKeyStates.KeyUp, true);
                return;
            }

            var mouseDown = new WinApi.DeviceInput();
            mouseDown.Data.Mouse.Flags = 0x2;
            mouseDown.Type = 0;

            var mouseUp = new WinApi.DeviceInput();
            mouseUp.Data.Mouse.Flags = 0x4;
            mouseUp.Type = 0;

            var inputs = new[]
            {
                mouseDown,
                mouseUp
            };

            WinApi.NativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(WinApi.DeviceInput)));
        }
    }
}
