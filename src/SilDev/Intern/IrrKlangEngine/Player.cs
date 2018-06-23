#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Player.cs
// Version:  2018-06-23 22:44
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Intern.IrrKlangEngine
{
    using System;

    internal class Player
    {
        protected static dynamic AudioPlayer;
        protected static dynamic SoundEngine;

        internal static void Play(string path, bool loop = false)
        {
            if (SoundEngine == null)
            {
                if (IrrKlangReference.Assembly == null)
                    return;
                var type = IrrKlangReference.Assembly.GetType("IrrKlang.ISoundEngine", true);
                SoundEngine = Activator.CreateInstance(type);
            }
            AudioPlayer?.Stop();
            AudioPlayer = SoundEngine.Play2D(path, loop);
            AudioPlayer.Volume = 1F;
        }

        internal static void Stop() =>
            AudioPlayer?.Stop();
    }
}
