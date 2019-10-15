#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Media.cs
// Version:  2019-10-15 11:27
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Media;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using Intern;
    using Intern.IrrKlangEngine;

    /// <summary>
    ///     Provides functionality for playing WAV files and controlling the volume of applications.
    /// </summary>
    public static class Media
    {
        /// <summary>
        ///     Plays audio data from the specified stream.
        /// </summary>
        /// <param name="stream">
        ///     The sound data to play.
        /// </param>
        public static void PlayWave(Stream stream)
        {
            try
            {
                using (var audio = stream)
                {
                    var player = new SoundPlayer(audio);
                    player.Play();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Plays audio data from the specified stream. This method does not block the calling thread.
        /// </summary>
        /// <param name="stream">
        ///     The sound data to play.
        /// </param>
        public static void PlayWaveAsync(Stream stream)
        {
            try
            {
                var thread = new Thread(() => PlayWave(stream));
                thread.Start();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Provides functionality to control the volume of applications.
        /// </summary>
        public static class DeviceManager
        {
            /// <summary>
            ///     Retrieves the volume of the specified application.
            /// </summary>
            /// <param name="name">
            ///     The name of the application.
            /// </param>
            public static float? GetApplicationVolume(string name)
            {
                var volume = ComImports.GetVolumeObject(name);
                if (volume == null)
                    return null;
                volume.GetMasterVolume(out var level);
                return level * 0x64;
            }

            /// <summary>
            ///     Determines whether the specified application is muted.
            /// </summary>
            /// <param name="name">
            ///     The name of the application.
            /// </param>
            public static bool? GetApplicationMute(string name)
            {
                var volume = ComImports.GetVolumeObject(name);
                if (volume == null)
                    return null;
                volume.GetMute(out var mute);
                return mute;
            }

            /// <summary>
            ///     Sets the volume of the specified application.
            /// </summary>
            /// <param name="name">
            ///     The name of the application to change.
            /// </param>
            /// <param name="level">
            ///     The volume level to set.
            /// </param>
            public static void SetApplicationVolume(string name, float level)
            {
                var volume = ComImports.GetVolumeObject(name);
                if (volume == null)
                    return;
                var guid = Guid.Empty;
                volume.SetMasterVolume(level / 0x64, ref guid);
            }

            /// <summary>
            ///     Mutes the volume of the specified application.
            /// </summary>
            /// <param name="name">
            ///     The name of the application to change.
            /// </param>
            /// <param name="mute">
            ///     true to mute; otherwise, false to unmute.
            /// </param>
            public static void SetApplicationMute(string name, bool mute)
            {
                var volume = ComImports.GetVolumeObject(name);
                if (volume == null)
                    return;
                var guid = Guid.Empty;
                volume.SetMute(mute, ref guid);
            }
        }

        /// <summary>
        ///     Provides basic functionality of the Windows library to play audio files.
        /// </summary>
        public static class WindowsPlayer
        {
            [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
            private static readonly string Alias = Assembly.GetEntryAssembly().GetName().Name.RemoveChar(' ');

            /// <summary>
            ///     Requests a minimum resolution for periodic timers.
            /// </summary>
            /// <param name="uPeriod">
            ///     Minimum timer resolution, in milliseconds, for the application or device driver. A lower
            ///     value specifies a higher (more accurate) resolution.
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
            public static int GetSoundVolume()
            {
                WinApi.NativeMethods.WaveOutGetVolume(IntPtr.Zero, out var currVol);
                var calcVol = (ushort)(currVol & 0xffff);
                return calcVol / (ushort.MaxValue / 0xa) * 0xa;
            }

            /// <summary>
            ///     Sets the specified sound volume of the current application.
            /// </summary>
            /// <param name="value">
            ///     The sound volume value, in percent.
            /// </param>
            public static void SetSoundVolume(int value)
            {
                var newVolume = ushort.MaxValue / 0xa * (value.IsBetween(0x0, 0x64) ? value / 0xa : 0x64);
                var newVolumeAllChannels = ((uint)newVolume & 0xffff) | ((uint)newVolume << 16);
                WinApi.NativeMethods.WaveOutSetVolume(IntPtr.Zero, newVolumeAllChannels);
            }

            private static string SndStatus()
            {
                var sb = new StringBuilder(128);
                WinApi.NativeMethods.MciSendString($"status {Alias} mode", sb, (uint)sb.Capacity, IntPtr.Zero);
                return sb.ToString();
            }

            private static void SndOpen(string path)
            {
                if (!string.IsNullOrEmpty(SndStatus()))
                    SndClose();
                var arg = $"open \"{path}\" alias {Alias}";
                WinApi.NativeMethods.MciSendString(arg, null, 0, IntPtr.Zero);
            }

            private static void SndClose()
            {
                var arg = $"close {Alias}";
                WinApi.NativeMethods.MciSendString(arg, null, 0, IntPtr.Zero);
            }

            private static void SndPlay(bool loop = false)
            {
                var arg = $"play {Alias}{(loop ? " repeat" : string.Empty)}";
                WinApi.NativeMethods.MciSendString(arg, null, 0, IntPtr.Zero);
            }

            /// <summary>
            ///     Plays the specified sound file.
            /// </summary>
            /// <param name="path">
            ///     THe full path of the sound file to play.
            /// </param>
            /// <param name="loop">
            ///     true to loop the sound; otherwise, false.
            /// </param>
            /// <param name="volume">
            ///     The sound volume value, in percent.
            /// </param>
            public static void Play(string path, bool loop = false, int volume = 100)
            {
                path = PathEx.Combine(path);
                if (!File.Exists(path))
                    return;
                if (GetSoundVolume() != volume)
                    SetSoundVolume(volume);
                SndOpen(path);
                SndPlay(loop);
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
            public static void Stop() =>
                SndClose();
        }

        /// <summary>
        ///     Provides basic functionality of the IrrKlang library.
        ///     <para>
        ///         Please note that this class requires the binaries of the IrrKlang library.
        ///     </para>
        /// </summary>
        public static class IrrKlangPlayer
        {
            private static bool _assemblyFinalizer;

            private static void PlayIntern(dynamic source, bool loop, int volume)
            {
                try
                {
                    if (IrrKlangReference.Assembly == null)
                        throw new NotSupportedException("The required assembly could not be found.");
                    switch (source)
                    {
                        case null:
                            throw new ArgumentNullException(nameof(source));
                        case string file when !File.Exists(file):
                            throw new PathNotFoundException(file);
                    }
                    string curDir = null;
                    if (!_assemblyFinalizer)
                    {
                        curDir = Directory.GetCurrentDirectory();
                        Directory.SetCurrentDirectory(IrrKlangReference.Location);
                    }
                    Player.Play(source, loop, volume / 100f);
                    if (curDir == null)
                        return;
                    _assemblyFinalizer = true;
                    Directory.SetCurrentDirectory(curDir);
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }

            /// <summary>
            ///     Plays the sound data from the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The sound data to play.
            /// </param>
            /// <param name="loop">
            ///     true to repeat the sound track; otherwise, false.
            /// </param>
            /// <param name="volume">
            ///     The sound volume value, in percent.
            /// </param>
            public static void Play(Stream stream, bool loop = false, int volume = 100) =>
                PlayIntern(stream, loop, volume);

            /// <summary>
            ///     Plays the sound data from the specified stream.
            /// </summary>
            /// <param name="stream">
            ///     The sound data to play.
            /// </param>
            /// <param name="volume">
            ///     The sound volume value, in percent.
            /// </param>
            public static void Play(Stream stream, int volume) =>
                PlayIntern(stream, false, volume);

            /// <summary>
            ///     Plays the sound data from the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sound data to play.
            /// </param>
            /// <param name="loop">
            ///     true to repeat the sound track; otherwise, false.
            /// </param>
            /// <param name="volume">
            ///     The sound volume value, in percent.
            /// </param>
            public static void Play(byte[] bytes, bool loop = false, int volume = 100) =>
                PlayIntern(bytes, loop, volume);

            /// <summary>
            ///     Plays the sound data from the specified sequence of bytes.
            /// </summary>
            /// <param name="bytes">
            ///     The sound data to play.
            /// </param>
            /// <param name="volume">
            ///     The sound volume value, in percent.
            /// </param>
            public static void Play(byte[] bytes, int volume) =>
                PlayIntern(bytes, false, volume);

            /// <summary>
            ///     Plays the specified sound file.
            /// </summary>
            /// <param name="path">
            ///     The file to play.
            /// </param>
            /// <param name="loop">
            ///     true to repeat the sound track; otherwise, false.
            /// </param>
            /// <param name="volume">
            ///     The sound volume value, in percent.
            /// </param>
            public static void Play(string path, bool loop = false, int volume = 100) =>
                PlayIntern(path, loop, volume);

            /// <summary>
            ///     Plays the specified sound file.
            /// </summary>
            /// <param name="path">
            ///     The file to play.
            /// </param>
            /// <param name="volume">
            ///     The sound volume value, in percent.
            /// </param>
            public static void Play(string path, int volume) =>
                PlayIntern(path, false, volume);

            /// <summary>
            ///     Stops playing sounds.
            /// </summary>
            public static void Stop()
            {
                try
                {
                    if (IrrKlangReference.Assembly == null)
                        throw new NotSupportedException("The required assembly could not be found.");
                    Player.Stop();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }
        }
    }
}
