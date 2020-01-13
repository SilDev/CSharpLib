#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: WindowsPlayer.cs
// Version:  2020-01-13 13:55
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Media
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Text;

    /// <summary>
    ///     Provides basic functionality of the Windows library to play audio files.
    /// </summary>
    public static class WindowsPlayer
    {
        private static string _alias;

        private static string Alias
        {
            get
            {
                if (_alias != default)
                    return _alias;
                var assembly = Assembly.GetEntryAssembly();
                _alias = assembly?.GetName().Name.RemoveChar(' ');
                return _alias;
            }
        }

        /// <summary>
        ///     Requests a minimum resolution for periodic timers.
        /// </summary>
        /// <param name="uPeriod">
        ///     Minimum timer resolution, in milliseconds, for the application or device
        ///     driver. A lower value specifies a higher (more accurate) resolution.
        /// </param>
        public static uint TimeBeginPeriod(uint uPeriod) =>
            WinApi.NativeMethods.TimeBeginPeriod(uPeriod);

        /// <summary>
        ///     Clears a previously set minimum timer resolution.
        /// </summary>
        /// <param name="uPeriod">
        ///     Minimum timer resolution specified in the previous call to the
        ///     <see cref="TimeBeginPeriod(uint)"/> function.
        /// </param>
        public static uint TimeEndPeriod(uint uPeriod) =>
            WinApi.NativeMethods.TimeEndPeriod(uPeriod);

        /// <summary>
        ///     Retrieves the sound volume of the current application.
        /// </summary>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static int GetSoundVolume()
        {
            WinApi.ThrowError(WinApi.NativeMethods.WaveOutGetVolume(IntPtr.Zero, out var currVol));
            var calcVol = (ushort)(currVol & 0xffff);
            return calcVol / (ushort.MaxValue / 0xa) * 0xa;
        }

        /// <summary>
        ///     Sets the specified sound volume of the current application.
        /// </summary>
        /// <param name="value">
        ///     The sound volume value, in percent.
        /// </param>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static void SetSoundVolume(int value)
        {
            var newVolume = ushort.MaxValue / 0xa * (value.IsBetween(0x0, 0x64) ? value / 0xa : 0x64);
            var newVolumeAllChannels = ((uint)newVolume & 0xffff) | ((uint)newVolume << 16);
            WinApi.ThrowError(WinApi.NativeMethods.WaveOutSetVolume(IntPtr.Zero, newVolumeAllChannels));
        }

        private static string SndStatus()
        {
            var sb = new StringBuilder(128);
            WinApi.ThrowError(WinApi.NativeMethods.MciSendString($"status {Alias} mode", sb, (uint)sb.Capacity, IntPtr.Zero));
            return sb.ToStringThenClear();
        }

        private static void SndOpen(string path)
        {
            if (!string.IsNullOrEmpty(SndStatus()))
                SndClose();
            var arg = $"open \"{path}\" alias {Alias}";
            WinApi.ThrowError(WinApi.NativeMethods.MciSendString(arg, null, 0, IntPtr.Zero));
        }

        private static void SndClose()
        {
            var arg = $"close {Alias}";
            WinApi.ThrowError(WinApi.NativeMethods.MciSendString(arg, null, 0, IntPtr.Zero));
        }

        private static void SndPlay(bool loop = false)
        {
            var arg = $"play {Alias}{(loop ? " repeat" : string.Empty)}";
            WinApi.ThrowError(WinApi.NativeMethods.MciSendString(arg, null, 0, IntPtr.Zero));
        }

        /// <summary>
        ///     Plays the specified sound file.
        /// </summary>
        /// <param name="path">
        ///     THe full path of the sound file to play.
        /// </param>
        /// <param name="loop">
        ///     <see langword="true"/> to loop the sound; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="volume">
        ///     The sound volume value, in percent.
        /// </param>
        public static void Play(string path, bool loop = false, int volume = 100)
        {
            path = PathEx.Combine(path);
            if (!File.Exists(path))
                return;
            try
            {
                if (GetSoundVolume() != volume)
                    SetSoundVolume(volume);
                SndOpen(path);
                SndPlay(loop);
            }
            catch (Win32Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Plays the specified sound file.
        /// </summary>
        /// <param name="path">
        ///     THe full path of the sound file to play.
        /// </param>
        /// <param name="volume">
        ///     The sound volume value, in percent.
        /// </param>
        public static void Play(string path, int volume) =>
            Play(path, false, volume);

        /// <summary>
        ///     Stops playing sounds.
        /// </summary>
        public static void Stop()
        {
            try
            {
                SndClose();
            }
            catch (Win32Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }
    }
}
