#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: MediaDeviceManager.cs
// Version:  2020-01-13 13:04
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Media
{
    using System;
    using Intern;

    /// <summary>
    ///     Provides functionality to control the volume of applications.
    /// </summary>
    public static class MediaDeviceManager
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
            _ = volume.GetMasterVolume(out var level);
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
            _ = volume.GetMute(out var mute);
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
            _ = volume.SetMasterVolume(level / 0x64, ref guid);
        }

        /// <summary>
        ///     Mutes the volume of the specified application.
        /// </summary>
        /// <param name="name">
        ///     The name of the application to change.
        /// </param>
        /// <param name="mute">
        ///     <see langword="true"/> to mute; otherwise, <see langword="false"/> to
        ///     unmute.
        /// </param>
        public static void SetApplicationMute(string name, bool mute)
        {
            var volume = ComImports.GetVolumeObject(name);
            if (volume == null)
                return;
            var guid = Guid.Empty;
            _ = volume.SetMute(mute, ref guid);
        }
    }
}
