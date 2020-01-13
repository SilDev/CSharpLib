#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Player.cs
// Version:  2020-01-13 13:04
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Intern.IrrKlangEngine
{
    using System;
    using System.IO;

    internal class Player
    {
        protected static dynamic AudioPlayer;
        protected static dynamic SoundEngine;

        protected static bool InitializeComponent()
        {
            if (SoundEngine != null)
                return true;
            if (IrrKlangReference.Assembly == null)
                return false;
            var engine = IrrKlangReference.Assembly.GetType("IrrKlang.ISoundEngine", false);
            if (engine == null)
                return false;
            SoundEngine = Activator.CreateInstance(engine);
            return true;
        }

        internal static void Play(dynamic source, bool loop, float volume)
        {
            if (!InitializeComponent())
                return;
            Stop();
            var setVolume = volume.IsBetween(.01f, .99f);
            switch (source)
            {
                case Stream stream:
                {
                    var sound = SoundEngine.AddSoundSourceFromIOStream(stream, nameof(stream));
                    AudioPlayer = SoundEngine.Play2D(sound, loop, setVolume, false);
                    break;
                }
                case byte[] bytes:
                {
                    var sound = SoundEngine.AddSoundSourceFromMemory(bytes, nameof(bytes));
                    AudioPlayer = SoundEngine.Play2D(sound, loop, setVolume, false);
                    break;
                }
                case string file:
                    AudioPlayer = SoundEngine.Play2D(file, loop, setVolume);
                    break;
                default:
                    return;
            }
            AudioPlayer.Volume = setVolume ? volume : 1f;
            if (setVolume)
                AudioPlayer.Paused = false;
        }

        internal static void Stop()
        {
            AudioPlayer?.Stop();
            SoundEngine?.RemoveAllSoundSources();
        }
    }
}
