#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: BasicPlayer.cs
// Version:  2020-01-13 13:04
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Media
{
    using System;
    using System.IO;
    using System.Media;
    using System.Threading;

    /// <summary>
    ///     Provides functionality for playing WAV files and controlling the volume of
    ///     applications.
    /// </summary>
    public static class BasicPlayer
    {
        /// <summary>
        ///     Plays audio data from the specified path.
        /// </summary>
        /// <param name="path">
        ///     The sound data to play.
        /// </param>
        public static void PlayWave(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                using var player = new SoundPlayer(path);
                player.Play();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Plays audio data from the specified stream.
        /// </summary>
        /// <param name="stream">
        ///     The sound data to play.
        /// </param>
        public static void PlayWave(Stream stream)
        {
            var audio = default(Stream);
            try
            {
                audio = stream;
                using var player = new SoundPlayer(audio);
                audio = null;
                player.Play();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                audio?.Dispose();
            }
        }

        /// <summary>
        ///     Plays audio data from the specified path.
        /// </summary>
        /// <param name="path">
        ///     The sound data to play.
        /// </param>
        public static void PlayWaveAsync(string path)
        {
            try
            {
                var thread = new Thread(() => PlayWave(path));
                thread.Start();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Plays audio data from the specified stream. This method does not block the
        ///     calling thread.
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
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }
    }
}
