#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: KeyState.cs
// Version:  2017-06-23 12:07
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     Provides the functionality to send or detect key states.
    /// </summary>
    public static class KeyState
    {
        /// <summary>
        ///     Provides enumerated values of Virtual-Key codes.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum VKey
        {
            /// <summary>
            ///     Left mouse button.
            /// </summary>
            VK_LBUTTON = 0x1,

            /// <summary>
            ///     Right mouse button.
            /// </summary>
            VK_RBUTTON = 0x2,

            /// <summary>
            ///     Control-break processing.
            /// </summary>
            VK_CANCEL = 0x3,

            /// <summary>
            ///     Middle mouse button (three-button mouse).
            /// </summary>
            VK_MBUTTON = 0x4,

            /// <summary>
            ///     X1 mouse button.
            /// </summary>
            VK_XBUTTON1 = 0x5,

            /// <summary>
            ///     X2 mouse button.
            /// </summary>
            VK_XBUTTON2 = 0x6,

            /// <summary>
            ///     BACKSPACE key.
            /// </summary>
            VK_BACK = 0x8,

            /// <summary>
            ///     TAB key.
            /// </summary>
            VK_TAB = 0x9,

            /// <summary>
            ///     CLEAR key.
            /// </summary>
            VK_CLEAR = 0xc,

            /// <summary>
            ///     ENTER key.
            /// </summary>
            VK_RETURN = 0xd,

            /// <summary>
            ///     SHIFT key.
            /// </summary>
            VK_SHIFT = 0x10,

            /// <summary>
            ///     CTRL key.
            /// </summary>
            VK_CONTROL = 0x11,

            /// <summary>
            ///     ALT key.
            /// </summary>
            VK_MENU = 0x12,

            /// <summary>
            ///     PAUSE key.
            /// </summary>
            VK_PAUSE = 0x13,

            /// <summary>
            ///     CAPS LOCK key.
            /// </summary>
            VK_CAPITAL = 0x14,

            /// <summary>
            ///     IME Kana/Hangul mode.
            /// </summary>
            VK_KANA_HANGUL = 0x15,

            /// <summary>
            ///     IME Junja mode.
            /// </summary>
            VK_JUNJA = 0x17,

            /// <summary>
            ///     IME final mode.
            /// </summary>
            VK_FINAL = 0x18,

            /// <summary>
            ///     IME Hanja/Kanji mode.
            /// </summary>
            VK_HANJA_KANJI = 0x19,

            /// <summary>
            ///     ESC key
            /// </summary>
            VK_ESCAPE = 0x1b,

            /// <summary>
            ///     IME convert.
            /// </summary>
            VK_CONVERT = 0x1c,

            /// <summary>
            ///     IME nonconvert.
            /// </summary>
            VK_NONCONVERT = 0x1d,

            /// <summary>
            ///     IME accept.
            /// </summary>
            VK_ACCEPT = 0x1e,

            /// <summary>
            ///     IME mode change request.
            /// </summary>
            VK_MODECHANGE = 0x1f,

            /// <summary>
            ///     SPACEBAR key.
            /// </summary>
            VK_SPACE = 0x20,

            /// <summary>
            ///     PAGE UP key.
            /// </summary>
            VK_PRIOR = 0x21,

            /// <summary>
            ///     PAGE DOWN key.
            /// </summary>
            VK_NEXT = 0x22,

            /// <summary>
            ///     END key.
            /// </summary>
            VK_END = 0x23,

            /// <summary>
            ///     HOME key.
            /// </summary>
            VK_HOME = 0x24,

            /// <summary>
            ///     LEFT ARROW key.
            /// </summary>
            VK_LEFT = 0x25,

            /// <summary>
            ///     UP ARROW key.
            /// </summary>
            VK_UP = 0x26,

            /// <summary>
            ///     RIGHT ARROW key.
            /// </summary>
            VK_RIGHT = 0x27,

            /// <summary>
            ///     DOWN ARROW key.
            /// </summary>
            VK_DOWN = 0x28,

            /// <summary>
            ///     SELECT key.
            /// </summary>
            VK_SELECT = 0x29,

            /// <summary>
            ///     PRINT key.
            /// </summary>
            VK_PRINT = 0x2a,

            /// <summary>
            ///     EXECUTE key.
            /// </summary>
            VK_EXECUTE = 0x2b,

            /// <summary>
            ///     PRINT SCREEN key.
            /// </summary>
            VK_SNAPSHOT = 0x2c,

            /// <summary>
            ///     INS key.
            /// </summary>
            VK_INSERT = 0x2d,

            /// <summary>
            ///     DEL key.
            /// </summary>
            VK_DELETE = 0x2e,

            /// <summary>
            ///     HELP key.
            /// </summary>
            VK_HELP = 0x2f,

            /// <summary>
            ///     0 key.
            /// </summary>
            VK_0 = 0x30,

            /// <summary>
            ///     1 key.
            /// </summary>
            VK_1 = 0x31,

            /// <summary>
            ///     2 key.
            /// </summary>
            VK_2 = 0x32,

            /// <summary>
            ///     3 key.
            /// </summary>
            VK_3 = 0x33,

            /// <summary>
            ///     4 key.
            /// </summary>
            VK_4 = 0x34,

            /// <summary>
            ///     5 key.
            /// </summary>
            VK_5 = 0x35,

            /// <summary>
            ///     6 key.
            /// </summary>
            VK_6 = 0x36,

            /// <summary>
            ///     7 key.
            /// </summary>
            VK_7 = 0x37,

            /// <summary>
            ///     8 key.
            /// </summary>
            VK_8 = 0x38,

            /// <summary>
            ///     9 key.
            /// </summary>
            VK_9 = 0x39,

            /// <summary>
            ///     A key.
            /// </summary>
            VK_A = 0x41,

            /// <summary>
            ///     B key.
            /// </summary>
            VK_B = 0x42,

            /// <summary>
            ///     C key.
            /// </summary>
            VK_C = 0x43,

            /// <summary>
            ///     D key.
            /// </summary>
            VK_D = 0x44,

            /// <summary>
            ///     E key.
            /// </summary>
            VK_E = 0x45,

            /// <summary>
            ///     F key.
            /// </summary>
            VK_F = 0x46,

            /// <summary>
            ///     G key.
            /// </summary>
            VK_G = 0x47,

            /// <summary>
            ///     H key.
            /// </summary>
            VK_H = 0x48,

            /// <summary>
            ///     I key.
            /// </summary>
            VK_I = 0x49,

            /// <summary>
            ///     J key.
            /// </summary>
            VK_J = 0x4a,

            /// <summary>
            ///     K key.
            /// </summary>
            VK_K = 0x4b,

            /// <summary>
            ///     L key.
            /// </summary>
            VK_L = 0x4c,

            /// <summary>
            ///     M key.
            /// </summary>
            VK_M = 0x4d,

            /// <summary>
            ///     N key.
            /// </summary>
            VK_N = 0x4e,

            /// <summary>
            ///     O key.
            /// </summary>
            VK_O = 0x4f,

            /// <summary>
            ///     P key.
            /// </summary>
            VK_P = 0x50,

            /// <summary>
            ///     Q key.
            /// </summary>
            VK_Q = 0x51,

            /// <summary>
            ///     R key.
            /// </summary>
            VK_R = 0x52,

            /// <summary>
            ///     S key.
            /// </summary>
            VK_S = 0x53,

            /// <summary>
            ///     T key.
            /// </summary>
            VK_T = 0x54,

            /// <summary>
            ///     U key.
            /// </summary>
            VK_U = 0x55,

            /// <summary>
            ///     V key.
            /// </summary>
            VK_V = 0x56,

            /// <summary>
            ///     W key.
            /// </summary>
            VK_W = 0x57,

            /// <summary>
            ///     X key.
            /// </summary>
            VK_X = 0x58,

            /// <summary>
            ///     Y key.
            /// </summary>
            VK_Y = 0x59,

            /// <summary>
            ///     Z key.
            /// </summary>
            VK_Z = 0x5a,

            /// <summary>
            ///     Left Windows key (natural keyboard).
            /// </summary>
            VK_LWIN = 0x5b,

            /// <summary>
            ///     Right Windows key (natural keyboard).
            /// </summary>
            VK_RWIN = 0x5c,

            /// <summary>
            ///     Applications key (natural keyboard).
            /// </summary>
            VK_APPS = 0x5d,

            /// <summary>
            ///     Computer Sleep key.
            /// </summary>
            VK_SLEEP = 0x5f,

            /// <summary>
            ///     Numeric keypad 0 key.
            /// </summary>
            VK_NUMPAD0 = 0x60,

            /// <summary>
            ///     Numeric keypad 1 key.
            /// </summary>
            VK_NUMPAD1 = 0x61,

            /// <summary>
            ///     Numeric keypad 2 key.
            /// </summary>
            VK_NUMPAD2 = 0x62,

            /// <summary>
            ///     Numeric keypad 3 key.
            /// </summary>
            VK_NUMPAD3 = 0x63,

            /// <summary>
            ///     Numeric keypad 4 key.
            /// </summary>
            VK_NUMPAD4 = 0x64,

            /// <summary>
            ///     Numeric keypad 5 key.
            /// </summary>
            VK_NUMPAD5 = 0x65,

            /// <summary>
            ///     Numeric keypad 6 key.
            /// </summary>
            VK_NUMPAD6 = 0x66,

            /// <summary>
            ///     Numeric keypad 7 key.
            /// </summary>
            VK_NUMPAD7 = 0x67,

            /// <summary>
            ///     Numeric keypad 8 key.
            /// </summary>
            VK_NUMPAD8 = 0x68,

            /// <summary>
            ///     Numeric keypad 9 key.
            /// </summary>
            VK_NUMPAD9 = 0x69,

            /// <summary>
            ///     Multiply key.
            /// </summary>
            VK_MULTIPLY = 0x6a,

            /// <summary>
            ///     Add key.
            /// </summary>
            VK_ADD = 0x6b,

            /// <summary>
            ///     Separator key.
            /// </summary>
            VK_SEPARATOR = 0x6c,

            /// <summary>
            ///     Subtract key.
            /// </summary>
            VK_SUBTRACT = 0x6d,

            /// <summary>
            ///     Decimal key.
            /// </summary>
            VK_DECIMAL = 0x6e,

            /// <summary>
            ///     Divide key.
            /// </summary>
            VK_DIVIDE = 0x6f,

            /// <summary>
            ///     F1 key.
            /// </summary>
            VK_F1 = 0x70,

            /// <summary>
            ///     F2 key.
            /// </summary>
            VK_F2 = 0x71,

            /// <summary>
            ///     F3 key.
            /// </summary>
            VK_F3 = 0x72,

            /// <summary>
            ///     F4 key.
            /// </summary>
            VK_F4 = 0x73,

            /// <summary>
            ///     F5 key.
            /// </summary>
            VK_F5 = 0x74,

            /// <summary>
            ///     F6 key.
            /// </summary>
            VK_F6 = 0x75,

            /// <summary>
            ///     F7 key.
            /// </summary>
            VK_F7 = 0x76,

            /// <summary>
            ///     F8 key.
            /// </summary>
            VK_F8 = 0x77,

            /// <summary>
            ///     F9 key.
            /// </summary>
            VK_F9 = 0x78,

            /// <summary>
            ///     F10 key.
            /// </summary>
            VK_F10 = 0x79,

            /// <summary>
            ///     F11 key.
            /// </summary>
            VK_F11 = 0x7a,

            /// <summary>
            ///     F12 key.
            /// </summary>
            VK_F12 = 0x7b,

            /// <summary>
            ///     F13 key.
            /// </summary>
            VK_F13 = 0x7c,

            /// <summary>
            ///     F14 key.
            /// </summary>
            VK_F14 = 0x7d,

            /// <summary>
            ///     F15 key.
            /// </summary>
            VK_F15 = 0x7e,

            /// <summary>
            ///     F16 key.
            /// </summary>
            VK_F16 = 0x7f,

            /// <summary>
            ///     F17 key.
            /// </summary>
            VK_F17 = 0x80,

            /// <summary>
            ///     F18 key.
            /// </summary>
            VK_F18 = 0x81,

            /// <summary>
            ///     F19 key.
            /// </summary>
            VK_F19 = 0x82,

            /// <summary>
            ///     F20 key.
            /// </summary>
            VK_F20 = 0x83,

            /// <summary>
            ///     F21 key.
            /// </summary>
            VK_F21 = 0x84,

            /// <summary>
            ///     F22 key.
            /// </summary>
            VK_F22 = 0x85,

            /// <summary>
            ///     F23 key.
            /// </summary>
            VK_F23 = 0x86,

            /// <summary>
            ///     F24 key.
            /// </summary>
            VK_F24 = 0x87,

            /// <summary>
            ///     NUM LOCK key.
            /// </summary>
            VK_NUMLOCK = 0x90,

            /// <summary>
            ///     SCROLL LOCK key.
            /// </summary>
            VK_SCROLL = 0x91,

            /// <summary>
            ///     Left SHIFT key.
            /// </summary>
            VK_LSHIFT = 0xa0,

            /// <summary>
            ///     Right SHIFT key.
            /// </summary>
            VK_RSHIFT = 0xa1,

            /// <summary>
            ///     Left CONTROL key.
            /// </summary>
            VK_LCONTROL = 0xa2,

            /// <summary>
            ///     Right CONTROL key.
            /// </summary>
            VK_RCONTROL = 0xa3,

            /// <summary>
            ///     Left MENU key.
            /// </summary>
            VK_LMENU = 0xa4,

            /// <summary>
            ///     Right MENU key.
            /// </summary>
            VK_RMENU = 0xa5,

            /// <summary>
            ///     Browser Back key.
            /// </summary>
            VK_BROWSER_BACK = 0xa6,

            /// <summary>
            ///     Browser Forward key.
            /// </summary>
            VK_BROWSER_FORWARD = 0xa7,

            /// <summary>
            ///     Browser Refresh key.
            /// </summary>
            VK_BROWSER_REFRESH = 0xa8,

            /// <summary>
            ///     Browser Stop key.
            /// </summary>
            VK_BROWSER_STOP = 0xa9,

            /// <summary>
            ///     Browser Search key.
            /// </summary>
            VK_BROWSER_SEARCH = 0xaa,

            /// <summary>
            ///     Browser Favorites key.
            /// </summary>
            VK_BROWSER_FAVORITES = 0xab,

            /// <summary>
            ///     Browser Start and Home key.
            /// </summary>
            VK_BROWSER_HOME = 0xac,

            /// <summary>
            ///     Volume Mute key.
            /// </summary>
            VK_VOLUME_MUTE = 0xad,

            /// <summary>
            ///     Volume Down key.
            /// </summary>
            VK_VOLUME_DOWN = 0xae,

            /// <summary>
            ///     Volume Up key.
            /// </summary>
            VK_VOLUME_UP = 0xaf,

            /// <summary>
            ///     Next Track key.
            /// </summary>
            VK_MEDIA_NEXT_TRACK = 0xb0,

            /// <summary>
            ///     Previous Track key.
            /// </summary>
            VK_MEDIA_PREV_TRACK = 0xb1,

            /// <summary>
            ///     Stop Media key.
            /// </summary>
            VK_MEDIA_STOP = 0xb2,

            /// <summary>
            ///     Play/Pause Media key.
            /// </summary>
            VK_MEDIA_PLAY_PAUSE = 0xb3,

            /// <summary>
            ///     Start Mail key.
            /// </summary>
            VK_LAUNCH_MAIL = 0xb4,

            /// <summary>
            ///     Select Media key.
            /// </summary>
            VK_LAUNCH_MEDIA_SELECT = 0xb5,

            /// <summary>
            ///     Select Media key.
            /// </summary>
            VK_LAUNCH_APP1 = 0xb6,

            /// <summary>
            ///     Start Application 2 key.
            /// </summary>
            VK_LAUNCH_APP2 = 0xb7,

            /// <summary>
            ///     For the US standard keyboard, the ",:" key .
            /// </summary>
            VK_OEM_1 = 0xba,

            /// <summary>
            ///     For any country/region, the "+" key.
            /// </summary>
            VK_OEM_PLUS = 0xbb,

            /// <summary>
            ///     For any country/region, the "," key.
            /// </summary>
            VK_OEM_COMMA = 0xbc,

            /// <summary>
            ///     For any country/region, the "-" key.
            /// </summary>
            VK_OEM_MINUS = 0xbd,

            /// <summary>
            ///     For any country/region, the "." key.
            /// </summary>
            VK_OEM_PERIOD = 0xbe,

            /// <summary>
            ///     For the US standard keyboard, the "/?" key.
            /// </summary>
            VK_OEM_2 = 0xbf,

            /// <summary>
            ///     For the US standard keyboard, the "`~" key.
            /// </summary>
            VK_OEM_3 = 0xc0,

            /// <summary>
            ///     For the US standard keyboard, the "[{" key.
            /// </summary>
            VK_OEM_4 = 0xdb,

            /// <summary>
            ///     For the US standard keyboard, the "\|" key.
            /// </summary>
            VK_OEM_5 = 0xdc,

            /// <summary>
            ///     For the US standard keyboard, the "]}" key.
            /// </summary>
            VK_OEM_6 = 0xdd,

            /// <summary>
            ///     For the US standard keyboard, the "single-quote/double-quote" key
            /// </summary>
            VK_OEM_7 = 0xdf,

            /// <summary>
            ///     Used for miscellaneous characters, it can vary by keyboard.
            /// </summary>
            VK_OEM_8 = 0xe1,

            /// <summary>
            ///     Either the angle bracket key or the backslash key on the RT 102-key keyboard.
            /// </summary>
            VK_OEM_102 = 0xe2,

            /// <summary>
            ///     IME PROCESS key.
            /// </summary>
            VK_PROCESSKEY = 0xe5,

            /// <summary>
            ///     Used to pass Unicode characters as if they were keystrokes. The <see cref="VK_PACKET"/>
            ///     key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods.
            /// </summary>
            VK_PACKET = 0xe7,

            /// <summary>
            ///     Attn key.
            /// </summary>
            VK_ATTN = 0xf6,

            /// <summary>
            ///     CrSel key.
            /// </summary>
            VK_CRSEL = 0xf7,

            /// <summary>
            ///     ExSel key.
            /// </summary>
            VK_EXSEL = 0xf8,

            /// <summary>
            ///     Erase EOF key.
            /// </summary>
            VK_EREOF = 0xf9,

            /// <summary>
            ///     Play key.
            /// </summary>
            VK_PLAY = 0xfa,

            /// <summary>
            ///     Zoom key.
            /// </summary>
            VK_ZOOM = 0xfb,

            /// <summary>
            ///     PA1 key.
            /// </summary>
            VK_PA1 = 0xfd,

            /// <summary>
            ///     Clear key.
            /// </summary>
            VK_OEM_CLEAR = 0xfe
        }

        /// <summary>
        ///     Returns the <see cref="VKey"/> of the <see cref="ushort"/> representation of a
        ///     Virtual-Key code.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="VKey"/> value.
        /// </param>
        public static VKey GetVKeyValue(ushort key) =>
            (VKey)key;

        /// <summary>
        ///     Returns the <see cref="VKey"/> of the <see cref="string"/> representation of a
        ///     Virtual-Key code.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="string"/> representation of a Virtual-Key code.
        /// </param>
        public static VKey GetVKeyValue(string key) =>
            (VKey)GetVKeyCode(key);

        /// <summary>
        ///     Returns the <see cref="ushort"/> representation of the <see cref="VKey"/> value.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="VKey"/> value.
        /// </param>
        public static ushort GetVKeyCode(VKey key) =>
            (ushort)key;

        /// <summary>
        ///     Returns the <see cref="ushort"/> representation of the <see cref="string"/> representation
        ///     of a Virtual-Key code.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="string"/> representation of a Virtual-Key code.
        /// </param>
        public static ushort GetVKeyCode(string key)
        {
            VKey vkey;
            if (Enum.TryParse(key, out vkey))
                return (ushort)vkey;
            return 0;
        }

        /// <summary>
        ///     Returns the <see cref="string"/> representation of the <see cref="VKey"/> value.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="VKey"/> value.
        /// </param>
        public static string GetVKeyString(VKey key) =>
            Enum.GetName(typeof(VKey), key);

        /// <summary>
        ///     Returns the <see cref="string"/> representation of the <see cref="ushort"/> representation
        ///     of a Virtual-Key code.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="ushort"/> representation of a Virtual-Key code.
        /// </param>
        public static string GetVKeyString(ushort key) =>
            GetVKeyValue(key).ToString();

        /// <summary>
        ///     Determines whether a key is up or down at the time the function is called, and whether the
        ///     key was pressed after a previous call to <see cref="GetState(VKey)"/>.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="VKey"/> value to check.
        /// </param>
        public static bool GetState(VKey key) =>
            WinApi.NativeMethods.GetAsyncKeyState((ushort)key) < 0;

        /// <summary>
        ///     Determines whether a key is up or down at the time the function is called, and whether the
        ///     key was pressed after a previous call to <see cref="GetState(ushort)"/>.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="ushort"/> representation of a Virtual-Key code to check.
        /// </param>
        public static bool GetState(ushort key) =>
            WinApi.NativeMethods.GetAsyncKeyState(key) < 0;

        /// <summary>
        ///     Determines whether a key is up or down at the time the function is called, and whether the
        ///     key was pressed after a previous call to <see cref="GetState(string)"/>.
        /// </summary>
        /// <param name="key">
        ///     The <see cref="string"/> representation of a Virtual-Key code to check.
        /// </param>
        public static bool GetState(string key) =>
            WinApi.NativeMethods.GetAsyncKeyState(GetVKeyCode(key)) < 0;

        /// <summary>
        ///     Determines which keys were up or down at the time the function is called, and which keys
        ///     were pressed.
        /// </summary>
        public static IEnumerable<VKey> GetStates()
        {
            var keys = Enum.GetValues(typeof(VKey)).Cast<VKey>();
            foreach (var key in keys)
                if (WinApi.NativeMethods.GetAsyncKeyState(GetVKeyCode(key)) < 0)
                    yield return key;
        }

        /// <summary>
        ///     Places (posts) a key up and down message in the message queue associated with the thread
        ///     that created the specified window and returns without waiting for the thread to process
        ///     the message.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window whose window procedure is to receive the message.
        /// </param>
        /// <param name="key">
        ///     The <see cref="VKey"/> value to send.
        /// </param>
        public static void SendState(IntPtr hWnd, VKey key)
        {
            WinApi.NativeHelper.PostMessage(hWnd, 0x100, new IntPtr((int)key), IntPtr.Zero);
            WinApi.NativeHelper.PostMessage(hWnd, 0x101, new IntPtr((int)key), IntPtr.Zero);
        }

        /// <summary>
        ///     Synthesizes a left mouse button click to the active window.
        /// </summary>
        public static void SendMouseClick()
        {
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
