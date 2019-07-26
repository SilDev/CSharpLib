#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: InputDevice.cs
// Version:  2019-07-26 04:31
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
    public enum VirtualKeys : ushort
    {
        /// <summary>
        ///     The left mouse button.
        /// </summary>
        LButton = 0x1,

        /// <summary>
        ///     The right mouse button.
        /// </summary>
        RButton = 0x2,

        /// <summary>
        ///     The CANCEL key.
        /// </summary>
        Cancel = 0x3,

        /// <summary>
        ///     The middle mouse button (three-button mouse).
        /// </summary>
        MButton = 0x4,

        /// <summary>
        ///     The first x mouse button (five-button mouse).
        /// </summary>
        XButton1 = 0x5,

        /// <summary>
        ///     The second x mouse button (five-button mouse).
        /// </summary>
        XButton2 = 0x6,

        /// <summary>
        ///     The BACKSPACE key.
        /// </summary>
        Back = 0x8,

        /// <summary>
        ///     The TAB key.
        /// </summary>
        Tab = 0x9,

        /// <summary>
        ///     The LINEFEED key.
        /// </summary>
        LineFeed = 0xa,

        /// <summary>
        ///     The CLEAR key.
        /// </summary>
        Clear = 0xc,

        /// <summary>
        ///     The RETURN key.
        /// </summary>
        Return = 0xd,

        /// <summary>
        ///     The ENTER key.
        /// </summary>
        Enter = Return,

        /// <summary>
        ///     The SHIFT key.
        /// </summary>
        Shift = 0x10,

        /// <summary>
        ///     The CTRL key.
        /// </summary>
        Control = 0x11,

        /// <summary>
        ///     The ALT key.
        /// </summary>
        Alt = 0x12,

        /// <summary>
        ///     The ALT key.
        /// </summary>
        Menu = Alt,

        /// <summary>
        ///     The PAUSE key.
        /// </summary>
        Pause = 0x13,

        /// <summary>
        ///     The CAPS LOCK key.
        /// </summary>
        CapsLock = 0x14,

        /// <summary>
        ///     The CAPS LOCK key.
        /// </summary>
        Capital = CapsLock,

        /// <summary>
        ///     The IME Kana mode key.
        /// </summary>
        KanaMode = 0x15,

        /// <summary>
        ///     The IME Hangul mode key.
        /// </summary>
        HangulMode = KanaMode,

        /// <summary>
        ///     The IME Junja mode key.
        /// </summary>
        JunjaMode = 0x17,

        /// <summary>
        ///     The IME final mode key.
        /// </summary>
        FinalMode = 0x18,

        /// <summary>
        ///     The IME Hanja mode key.
        /// </summary>
        HanjaMode = 0x19,

        /// <summary>
        ///     The IME Kanji mode key.
        /// </summary>
        KanjiMode = HanjaMode,

        /// <summary>
        ///     The ESC key.
        /// </summary>
        Escape = 0x1b,

        /// <summary>
        ///     The IME convert key.
        /// </summary>
        ImeConvert = 0x1c,

        /// <summary>
        ///     The IME nonconvert key.
        /// </summary>
        ImeNonconvert = 0x1d,

        /// <summary>
        ///     The IME accept key.
        /// </summary>
        ImeAccept = 0x1e,

        /// <summary>
        ///     The IME mode change key.
        /// </summary>
        ImeModeChange = 0x1f,

        /// <summary>
        ///     The SPACEBAR key.
        /// </summary>
        Space = 0x20,

        /// <summary>
        ///     The PAGE UP key.
        /// </summary>
        PageUp = 0x21,

        /// <summary>
        ///     The PAGE UP key.
        /// </summary>
        Prior = PageUp,

        /// <summary>
        ///     The PAGE DOWN key.
        /// </summary>
        PageDown = 0x22,

        /// <summary>
        ///     The PAGE DOWN key.
        /// </summary>
        Next = PageDown,

        /// <summary>
        ///     The END key.
        /// </summary>
        End = 0x23,

        /// <summary>
        ///     The HOME key.
        /// </summary>
        Home = 0x24,

        /// <summary>
        ///     The LEFT ARROW key.
        /// </summary>
        Left = 0x25,

        /// <summary>
        ///     The UP ARROW key.
        /// </summary>
        Up = 0x26,

        /// <summary>
        ///     The RIGHT ARROW key.
        /// </summary>
        Right = 0x27,

        /// <summary>
        ///     The DOWN ARROW key.
        /// </summary>
        Down = 0x28,

        /// <summary>
        ///     The SELECT key.
        /// </summary>
        Select = 0x29,

        /// <summary>
        ///     The PRINT key.
        /// </summary>
        Print = 0x2a,

        /// <summary>
        ///     The EXECUTE key.
        /// </summary>
        Execute = 0x2b,

        /// <summary>
        ///     The PRINT SCREEN key.
        /// </summary>
        PrintScreen = 0x2c,

        /// <summary>
        ///     The PRINT SCREEN key.
        /// </summary>
        Snapshot = PrintScreen,

        /// <summary>
        ///     The INS key.
        /// </summary>
        Insert = 0x2d,

        /// <summary>
        ///     The DEL key.
        /// </summary>
        Delete = 0x2e,

        /// <summary>
        ///     The HELP key.
        /// </summary>
        Help = 0x2f,

        /// <summary>
        ///     The 0 key.
        /// </summary>
        D0 = 0x30,

        /// <summary>
        ///     The 1 key.
        /// </summary>
        D1 = 0x31,

        /// <summary>
        ///     The 2 key.
        /// </summary>
        D2 = 0x32,

        /// <summary>
        ///     The 3 key.
        /// </summary>
        D3 = 0x33,

        /// <summary>
        ///     The 4 key.
        /// </summary>
        D4 = 0x34,

        /// <summary>
        ///     The 5 key.
        /// </summary>
        D5 = 0x35,

        /// <summary>
        ///     The 6 key.
        /// </summary>
        D6 = 0x36,

        /// <summary>
        ///     The 7 key.
        /// </summary>
        D7 = 0x37,

        /// <summary>
        ///     The 8 key.
        /// </summary>
        D8 = 0x38,

        /// <summary>
        ///     The 9 key.
        /// </summary>
        D9 = 0x39,

        /// <summary>
        ///     The A key.
        /// </summary>
        A = 0x41,

        /// <summary>
        ///     The B key.
        /// </summary>
        B = 0x42,

        /// <summary>
        ///     The C key.
        /// </summary>
        C = 0x43,

        /// <summary>
        ///     The D key.
        /// </summary>
        D = 0x44,

        /// <summary>
        ///     The E key.
        /// </summary>
        E = 0x45,

        /// <summary>
        ///     The F key.
        /// </summary>
        F = 0x46,

        /// <summary>
        ///     The G key.
        /// </summary>
        G = 0x47,

        /// <summary>
        ///     The H key.
        /// </summary>
        H = 0x48,

        /// <summary>
        ///     The I key.
        /// </summary>
        I = 0x49,

        /// <summary>
        ///     The J key.
        /// </summary>
        J = 0x4a,

        /// <summary>
        ///     The K key.
        /// </summary>
        K = 0x4b,

        /// <summary>
        ///     The L key.
        /// </summary>
        L = 0x4c,

        /// <summary>
        ///     The M key.
        /// </summary>
        M = 0x4d,

        /// <summary>
        ///     The N key.
        /// </summary>
        N = 0x4e,

        /// <summary>
        ///     The O key.
        /// </summary>
        O = 0x4f,

        /// <summary>
        ///     The P key.
        /// </summary>
        P = 0x50,

        /// <summary>
        ///     The Q key.
        /// </summary>
        Q = 0x51,

        /// <summary>
        ///     The R key.
        /// </summary>
        R = 0x52,

        /// <summary>
        ///     The S key.
        /// </summary>
        S = 0x53,

        /// <summary>
        ///     The T key.
        /// </summary>
        T = 0x54,

        /// <summary>
        ///     The U key.
        /// </summary>
        U = 0x55,

        /// <summary>
        ///     The V key.
        /// </summary>
        V = 0x56,

        /// <summary>
        ///     The W key.
        /// </summary>
        W = 0x57,

        /// <summary>
        ///     The X key.
        /// </summary>
        X = 0x58,

        /// <summary>
        ///     The Y key.
        /// </summary>
        Y = 0x59,

        /// <summary>
        ///     The Z key.
        /// </summary>
        Z = 0x5a,

        /// <summary>
        ///     The left Windows logo key.
        /// </summary>
        LWin = 0x5b,

        /// <summary>
        ///     The right Windows logo key.
        /// </summary>
        RWin = 0x5c,

        /// <summary>
        ///     The application key.
        /// </summary>
        Apps = 0x5d,

        /// <summary>
        ///     The computer sleep key.
        /// </summary>
        Sleep = 0x5f,

        /// <summary>
        ///     The 0 key on the numeric keypad.
        /// </summary>
        NumPad0 = 0x60,

        /// <summary>
        ///     The 1 key on the numeric keypad.
        /// </summary>
        NumPad1 = 0x61,

        /// <summary>
        ///     The 2 key on the numeric keypad.
        /// </summary>
        NumPad2 = 0x62,

        /// <summary>
        ///     The 3 key on the numeric keypad.
        /// </summary>
        NumPad3 = 0x63,

        /// <summary>
        ///     The 4 key on the numeric keypad.
        /// </summary>
        NumPad4 = 0x64,

        /// <summary>
        ///     The 5 key on the numeric keypad.
        /// </summary>
        NumPad5 = 0x65,

        /// <summary>
        ///     The 6 key on the numeric keypad.
        /// </summary>
        NumPad6 = 0x66,

        /// <summary>
        ///     The 7 key on the numeric keypad.
        /// </summary>
        NumPad7 = 0x67,

        /// <summary>
        ///     The 8 key on the numeric keypad.
        /// </summary>
        NumPad8 = 0x68,

        /// <summary>
        ///     The 9 key on the numeric keypad.
        /// </summary>
        NumPad9 = 0x69,

        /// <summary>
        ///     The multiply key.
        /// </summary>
        Multiply = 0x6a,

        /// <summary>
        ///     The add key.
        /// </summary>
        Add = 0x6b,

        /// <summary>
        ///     The separator key.
        /// </summary>
        Separator = 0x6c,

        /// <summary>
        ///     The subtract key.
        /// </summary>
        Subtract = 0x6d,

        /// <summary>
        ///     The decimal key.
        /// </summary>
        Decimal = 0x6e,

        /// <summary>
        ///     The divide key.
        /// </summary>
        Divide = 0x6f,

        /// <summary>
        ///     The F1 key.
        /// </summary>
        F1 = 0x70,

        /// <summary>
        ///     The F2 key.
        /// </summary>
        F2 = 0x71,

        /// <summary>
        ///     The F3 key.
        /// </summary>
        F3 = 0x72,

        /// <summary>
        ///     The F4 key.
        /// </summary>
        F4 = 0x73,

        /// <summary>
        ///     The F5 key.
        /// </summary>
        F5 = 0x74,

        /// <summary>
        ///     The F6 key.
        /// </summary>
        F6 = 0x75,

        /// <summary>
        ///     The F7 key.
        /// </summary>
        F7 = 0x76,

        /// <summary>
        ///     The F8 key.
        /// </summary>
        F8 = 0x77,

        /// <summary>
        ///     The F9 key.
        /// </summary>
        F9 = 0x78,

        /// <summary>
        ///     The F10 key.
        /// </summary>
        F10 = 0x79,

        /// <summary>
        ///     The F11 key.
        /// </summary>
        F11 = 0x7a,

        /// <summary>
        ///     The F12 key.
        /// </summary>
        F12 = 0x7b,

        /// <summary>
        ///     The F13 key.
        /// </summary>
        F13 = 0x7c,

        /// <summary>
        ///     The F14 key.
        /// </summary>
        F14 = 0x7d,

        /// <summary>
        ///     The F15 key.
        /// </summary>
        F15 = 0x7e,

        /// <summary>
        ///     The F16 key.
        /// </summary>
        F16 = 0x7f,

        /// <summary>
        ///     The F17 key.
        /// </summary>
        F17 = 0x80,

        /// <summary>
        ///     The F18 key.
        /// </summary>
        F18 = 0x81,

        /// <summary>
        ///     The F19 key.
        /// </summary>
        F19 = 0x82,

        /// <summary>
        ///     The F20 key.
        /// </summary>
        F20 = 0x83,

        /// <summary>
        ///     The F21 key.
        /// </summary>
        F21 = 0x84,

        /// <summary>
        ///     The F22 key.
        /// </summary>
        F22 = 0x85,

        /// <summary>
        ///     The F23 key.
        /// </summary>
        F23 = 0x86,

        /// <summary>
        ///     The F24 key.
        /// </summary>
        F24 = 0x87,

        /// <summary>
        ///     The NUM LOCK key.
        /// </summary>
        NumLock = 0x90,

        /// <summary>
        ///     The SCROLL LOCK key.
        /// </summary>
        Scroll = 0x91,

        /// <summary>
        ///     The left SHIFT key.
        /// </summary>
        LShift = 0xa0,

        /// <summary>
        ///     The right SHIFT key.
        /// </summary>
        RShift = 0xa1,

        /// <summary>
        ///     The left CTRL key.
        /// </summary>
        LControl = 0xa2,

        /// <summary>
        ///     The right CTRL key.
        /// </summary>
        RControl = 0xa3,

        /// <summary>
        ///     The left ALT key.
        /// </summary>
        LAlt = 0xa4,

        /// <summary>
        ///     The left ALT key.
        /// </summary>
        LMenu = LAlt,

        /// <summary>
        ///     The right ALT key.
        /// </summary>
        RAlt = 0xa5,

        /// <summary>
        ///     The right ALT key.
        /// </summary>
        RMenu = RAlt,

        /// <summary>
        ///     The browser back key.
        /// </summary>
        BrowserBack = 0xa6,

        /// <summary>
        ///     The browser forward key.
        /// </summary>
        BrowserForward = 0xa7,

        /// <summary>
        ///     The browser refresh key.
        /// </summary>
        BrowserRefresh = 0xa8,

        /// <summary>
        ///     The browser stop key.
        /// </summary>
        BrowserStop = 0xa9,

        /// <summary>
        ///     The browser search key.
        /// </summary>
        BrowserSearch = 0xaa,

        /// <summary>
        ///     The browser favorites key.
        /// </summary>
        BrowserFavorites = 0xab,

        /// <summary>
        ///     The browser home key.
        /// </summary>
        BrowserHome = 0xac,

        /// <summary>
        ///     The volume mute key.
        /// </summary>
        VolumeMute = 0xad,

        /// <summary>
        ///     The volume down key.
        /// </summary>
        VolumeDown = 0xae,

        /// <summary>
        ///     The volume up key.
        /// </summary>
        VolumeUp = 0xaf,

        /// <summary>
        ///     The media next track key.
        /// </summary>
        MediaNextTrack = 0xb0,

        /// <summary>
        ///     The media previous track key.
        /// </summary>
        MediaPreviousTrack = 0xb1,

        /// <summary>
        ///     The media Stop key.
        /// </summary>
        MediaStop = 0xb2,

        /// <summary>
        ///     The media play pause key.
        /// </summary>
        MediaPlayPause = 0xb3,

        /// <summary>
        ///     The launch mail key.
        /// </summary>
        LaunchMail = 0xb4,

        /// <summary>
        ///     The select media key.
        /// </summary>
        SelectMedia = 0xb5,

        /// <summary>
        ///     The start application one key.
        /// </summary>
        LaunchApplication1 = 0xb6,

        /// <summary>
        ///     The start application two key.
        /// </summary>
        LaunchApplication2 = 0xb7,

        /// <summary>
        ///     The OEM Semicolon key on a US standard keyboard.
        /// </summary>
        OemSemicolon = 0xba,

        /// <summary>
        ///     The OEM 1 key.
        /// </summary>
        Oem1 = OemSemicolon,

        /// <summary>
        ///     The OEM plus key on any country/region keyboard.
        /// </summary>
        Oemplus = 0xbb,

        /// <summary>
        ///     The OEM comma key on any country/region keyboard.
        /// </summary>
        Oemcomma = 0xbc,

        /// <summary>
        ///     The OEM minus key on any country/region keyboard.
        /// </summary>
        OemMinus = 0xbd,

        /// <summary>
        ///     The OEM period key on any country/region keyboard.
        /// </summary>
        OemPeriod = 0xbe,

        /// <summary>
        ///     The OEM question mark key on a US standard keyboard.
        /// </summary>
        OemQuestion = 0xbf,

        /// <summary>
        ///     The OEM 2 key.
        /// </summary>
        Oem2 = OemQuestion,

        /// <summary>
        ///     The OEM tilde key on a US standard keyboard.
        /// </summary>
        Oemtilde = 0xc0,

        /// <summary>
        ///     The OEM 3 key.
        /// </summary>
        Oem3 = Oemtilde,

        /// <summary>
        ///     The OEM open bracket key on a US standard keyboard.
        /// </summary>
        OemOpenBrackets = 0xdb,

        /// <summary>
        ///     The OEM 4 key.
        /// </summary>
        Oem4 = OemOpenBrackets,

        /// <summary>
        ///     The OEM pipe key on a US standard keyboard.
        /// </summary>
        OemPipe = 0xdc,

        /// <summary>
        ///     The OEM 5 key.
        /// </summary>
        Oem5 = OemPipe,

        /// <summary>
        ///     The OEM close bracket key on a US standard keyboard.
        /// </summary>
        OemCloseBrackets = 0xdd,

        /// <summary>
        ///     The OEM 6 key.
        /// </summary>
        Oem6 = OemCloseBrackets,

        /// <summary>
        ///     The OEM singled/double quote key on a US standard keyboard.
        /// </summary>
        OemQuotes = 0xde,

        /// <summary>
        ///     The OEM 7 key.
        /// </summary>
        Oem7 = OemQuotes,

        /// <summary>
        ///     The OEM 8 key.
        /// </summary>
        Oem8 = 0xdf,

        /// <summary>
        ///     The OEM angle bracket or backslash key on the RT 102 key keyboard.
        /// </summary>
        OemBackslash = 0xe2,

        /// <summary>
        ///     The OEM 102 key.
        /// </summary>
        Oem102 = OemBackslash,

        /// <summary>
        ///     The PROCESS KEY key.
        /// </summary>
        Process = 0xe5,

        /// <summary>
        ///     Used to pass Unicode characters as if they were keystrokes. The Packet key value
        ///     is the low word of a 32-bit virtual-key value used for non-keyboard input methods.
        /// </summary>
        Packet = 0xe7,

        /// <summary>
        ///     The ATTN key.
        /// </summary>
        Attn = 0xf6,

        /// <summary>
        ///     The CRSEL key.
        /// </summary>
        Crsel = 0xf7,

        /// <summary>
        ///     The EXSEL key.
        /// </summary>
        Exsel = 0xf8,

        /// <summary>
        ///     The ERASE EOF key.
        /// </summary>
        EraseEof = 0xf9,

        /// <summary>
        ///     The PLAY key.
        /// </summary>
        Play = 0xfa,

        /// <summary>
        ///     The ZOOM key.
        /// </summary>
        Zoom = 0xfb,

        /// <summary>
        ///     The PA1 key.
        /// </summary>
        Pa1 = 0xfd,

        /// <summary>
        ///     The CLEAR key.
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
            WinApi.NativeMethods.GetAsyncKeyState((int)key) < 0;

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
            {
                if (!GetKeyState(key))
                    continue;
                yield return key;
            }
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
