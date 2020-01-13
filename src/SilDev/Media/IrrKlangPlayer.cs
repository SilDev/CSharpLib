#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IrrKlangPlayer.cs
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
    using Intern;
    using Intern.IrrKlangEngine;
    using Properties;

    /// <summary>
    ///     Provides basic functionality of the IrrKlang library.
    ///     <para>
    ///         Please note that this class requires the latest binaries of the
    ///         IrrKlang library.
    ///     </para>
    ///     <para>
    ///         Visit: <see href="https://www.ambiera.com/irrklang/"/>
    ///     </para>
    /// </summary>
    public static class IrrKlangPlayer
    {
        private static bool _assemblyFinalizer;

        private static void PlayIntern<TSource>(TSource source, bool loop, int volume)
        {
            try
            {
                if (IrrKlangReference.Assembly == null)
                    throw new NotSupportedException(ExceptionMessages.AssemblyNotFound);
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
                    if (curDir.EqualsEx(IrrKlangReference.Location))
                        Directory.SetCurrentDirectory(IrrKlangReference.Location);
                    else
                        curDir = null;
                }
                Player.Play(source, loop, volume / 100f);
                if (curDir == null)
                    return;
                _assemblyFinalizer = true;
                Directory.SetCurrentDirectory(curDir);
            }
            catch (Exception ex) when (ex.IsCaught())
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
        ///     <see langword="true"/> to repeat the sound track; otherwise,
        ///     <see langword="false"/>.
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
        ///     <see langword="true"/> to repeat the sound track; otherwise,
        ///     <see langword="false"/>.
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
        ///     <see langword="true"/> to repeat the sound track; otherwise,
        ///     <see langword="false"/>.
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
                    throw new NotSupportedException(ExceptionMessages.AssemblyNotFound);
                Player.Stop();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }
    }
}
