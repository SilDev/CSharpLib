#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Player.cs
// Version:  2017-05-16 09:46
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Intern.IrrKlangEngine
{
    using IrrKlang;

    internal class Player
    {
        protected static ISoundEngine Engine = new ISoundEngine();
        protected static ISound Sound;

        internal static void Play(string path, bool loop = false)
        {
            Sound?.Stop();
            Sound = Engine.Play2D(path, loop);
            Sound.Volume = 1F;
        }

        internal static void Stop() =>
            Sound?.Stop();
    }
}
