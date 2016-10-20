#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Media.cs
// Version:  2016-10-18 23:33
// 
// Copyright (c) 2016, Si13n7 Developments (r)
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
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

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
                var volume = GetVolumeObject(name);
                if (volume == null)
                    return null;
                float level;
                volume.GetMasterVolume(out level);
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
                var volume = GetVolumeObject(name);
                if (volume == null)
                    return null;
                bool mute;
                volume.GetMute(out mute);
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
                var volume = GetVolumeObject(name);
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
                var volume = GetVolumeObject(name);
                if (volume == null)
                    return;
                var guid = Guid.Empty;
                volume.SetMute(mute, ref guid);
            }

            /*
            [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
            private static IEnumerable<string> EnumerateApplications()
            {
                var deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
                IMMDevice speakers;
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);
                var iidIAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
                object o;
                speakers.Activate(ref iidIAudioSessionManager2, 0, IntPtr.Zero, out o);
                var mgr = (IAudioSessionManager2)o;
                IAudioSessionEnumerator sessionEnumerator;
                mgr.GetSessionEnumerator(out sessionEnumerator);
                int count;
                sessionEnumerator.GetCount(out count);
                for (var i = 0; i < count; i++)
                {
                    IAudioSessionControl ctl;
                    sessionEnumerator.GetSession(i, out ctl);
                    string dn;
                    ctl.GetDisplayName(out dn);
                    yield return dn;
                    Marshal.ReleaseComObject(ctl);
                }
                Marshal.ReleaseComObject(sessionEnumerator);
                Marshal.ReleaseComObject(mgr);
                Marshal.ReleaseComObject(speakers);
                Marshal.ReleaseComObject(deviceEnumerator);
            }
            */

            [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
            private static ISimpleAudioVolume GetVolumeObject(string name)
            {
                var deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
                IMMDevice speakers;
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);
                var iidIAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
                object o;
                speakers.Activate(ref iidIAudioSessionManager2, 0, IntPtr.Zero, out o);
                var mgr = (IAudioSessionManager2)o;
                IAudioSessionEnumerator sessionEnumerator;
                mgr.GetSessionEnumerator(out sessionEnumerator);
                int count;
                sessionEnumerator.GetCount(out count);
                ISimpleAudioVolume volumeControl = null;
                for (var i = 0; i < count; i++)
                {
                    IAudioSessionControl ctl;
                    sessionEnumerator.GetSession(i, out ctl);
                    string dn;
                    ctl.GetDisplayName(out dn);
                    if (name.EqualsEx(dn))
                    {
                        volumeControl = ctl as ISimpleAudioVolume;
                        break;
                    }
                    Marshal.ReleaseComObject(ctl);
                }
                Marshal.ReleaseComObject(sessionEnumerator);
                Marshal.ReleaseComObject(mgr);
                Marshal.ReleaseComObject(speakers);
                Marshal.ReleaseComObject(deviceEnumerator);
                return volumeControl;
            }

            [ComImport]
            [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
            private class MMDeviceEnumerator { }

            [SuppressMessage("ReSharper", "InconsistentNaming")]
            [SuppressMessage("ReSharper", "UnusedMember.Local")]
            private enum EDataFlow
            {
                eRender,
                eCapture,
                eAll,
                EDataFlow_enum_count
            }

            [SuppressMessage("ReSharper", "InconsistentNaming")]
            [SuppressMessage("ReSharper", "UnusedMember.Local")]
            private enum ERole
            {
                eConsole,
                eMultimedia,
                eCommunications,
                ERole_enum_count
            }

            [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            private interface IMMDeviceEnumerator
            {
                int NotImpl1();

                [PreserveSig]
                int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);
            }

            [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            private interface IMMDevice
            {
                [PreserveSig]
                int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
            }

            [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            private interface IAudioSessionManager2
            {
                int NotImpl1();
                int NotImpl2();

                [PreserveSig]
                int GetSessionEnumerator(out IAudioSessionEnumerator sessionEnum);
            }

            [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            private interface IAudioSessionEnumerator
            {
                [PreserveSig]
                int GetCount(out int sessionCount);

                [PreserveSig]
                int GetSession(int sessionCount, out IAudioSessionControl session);
            }

            [Guid("F4B1A599-7266-4319-A8CA-E70ACB11E8CD")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            private interface IAudioSessionControl
            {
                int NotImpl1();

                [PreserveSig]
                int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);
            }

            [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            private interface ISimpleAudioVolume
            {
                [PreserveSig]
                int SetMasterVolume(float fLevel, ref Guid eventContext);

                [PreserveSig]
                int GetMasterVolume(out float pfLevel);

                [PreserveSig]
                int SetMute(bool bMute, ref Guid eventContext);

                [PreserveSig]
                int GetMute(out bool pbMute);
            }
        }

        /// <summary>
        ///     Provides basic functionality of the Windows library to play audio files.
        /// </summary>
        public static class WindowsPlayer
        {
            private static readonly string Alias = Assembly.GetEntryAssembly().GetName().Name.RemoveChar(' ');

            /// <summary>
            ///     Requests a minimum resolution for periodic timers.
            /// </summary>
            /// <param name="uPeriod">
            ///     Minimum timer resolution, in milliseconds, for the application or device driver. A lower
            ///     value specifies a higher (more accurate) resolution.
            /// </param>
            public static uint TimeBeginPeriod(uint uPeriod) =>
                WinApi.SafeNativeMethods.timeBeginPeriod(uPeriod);

            /// <summary>
            ///     Clears a previously set minimum timer resolution.
            /// </summary>
            /// <param name="uPeriod">
            ///     Minimum timer resolution specified in the previous call to the
            ///     <see cref="TimeBeginPeriod(uint)"/> function.
            /// </param>
            public static uint TimeEndPeriod(uint uPeriod) =>
                WinApi.SafeNativeMethods.timeEndPeriod(uPeriod);

            /// <summary>
            ///     Retrieves the sound volume of the current application.
            /// </summary>
            public static int GetSoundVolume()
            {
                uint currVol;
                WinApi.SafeNativeMethods.waveOutGetVolume(IntPtr.Zero, out currVol);
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
                WinApi.SafeNativeMethods.waveOutSetVolume(IntPtr.Zero, newVolumeAllChannels);
            }

            private static string SndStatus()
            {
                var sb = new StringBuilder(128);
                WinApi.SafeNativeMethods.mciSendString($"status {Alias} mode", sb, (uint)sb.Capacity, IntPtr.Zero);
                return sb.ToString();
            }

            private static void SndOpen(string path)
            {
                if (!string.IsNullOrEmpty(SndStatus()))
                    SndClose();
                var arg = $"open \"{path}\" alias {Alias}";
                WinApi.SafeNativeMethods.mciSendString(arg, null, 0, IntPtr.Zero);
            }

            private static void SndClose()
            {
                var arg = $"close {Alias}";
                WinApi.SafeNativeMethods.mciSendString(arg, null, 0, IntPtr.Zero);
            }

            private static void SndPlay(bool loop = false)
            {
                var arg = $"play {Alias}{(loop ? " repeat" : string.Empty)}";
                WinApi.SafeNativeMethods.mciSendString(arg, null, 0, IntPtr.Zero);
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
        ///     <para>
        ///         Provides basic functionality of the IrrKlang library.
        ///     </para>
        ///     <para>
        ///         Please note that this class requires the binaries of the IrrKlang library.
        ///     </para>
        /// </summary>
        public static class IrrKlangPlayer
        {
#if irrKlang
            protected static IrrKlang.ISoundEngine Engine = new IrrKlang.ISoundEngine();
            protected static IrrKlang.ISound _player;

            public static void Play(string path, bool loop = false, int volume = 100)
            {
                if (!File.Exists(path))
                    return;
                if (WindowsPlayer.GetSoundVolume() != volume)
                    WindowsPlayer.SetSoundVolume(volume);
                Stop();
                _player = Engine.Play2D(path, loop);
                _player.Volume = 1F;
            }

            public static void Play(string path, int volume) =>
                Play(path, false, volume);

            public static void Stop() =>
                _player?.Stop();
#endif
        }
    }
}
