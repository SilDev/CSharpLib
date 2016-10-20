#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Elevation.cs
// Version:  2016-10-18 23:33
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.IO;
    using System.Security.Principal;

    /// <summary>
    ///     Provides funcionality for the user authorization on Windows.
    /// </summary>
    public static class Elevation
    {
        /// <summary>
        ///     Determines whether the current principal belongs to the Windows administrator
        ///     user group.
        /// </summary>
        public static bool IsAdministrator
        {
            get
            {
                try
                {
                    return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     Determines whether the current principal has enough privileges to write in the
        ///     specified directory.
        /// </summary>
        /// <param name="path">
        ///     The directory to check.
        /// </param>
        public static bool WritableLocation(string path)
        {
            try
            {
                File.Create(PathEx.Combine(path, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose).Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the current principal has enough privileges to write in the
        ///     specified directory.
        /// </summary>
        public static bool WritableLocation() =>
            WritableLocation(PathEx.LocalDir);

        /// <summary>
        ///     Restarts the current process with highest privileges.
        /// </summary>
        /// <param name="cmdLineArgs">
        ///     The command-line arguments to use when starting the application. Use null to use
        ///     the current arguments, which are already in use.
        /// </param>
        public static void RestartAsAdministrator(string cmdLineArgs = null)
        {
            if (IsAdministrator)
                return;
            var args = string.Empty;
            if (cmdLineArgs != null)
                args = cmdLineArgs;
            else
            {
                if (Log.DebugMode > 0)
                    args = "/debug " + Log.DebugMode + " ";
                args += EnvironmentEx.CommandLine(false);
            }
            ProcessEx.Start(PathEx.LocalPath, PathEx.LocalDir, args, true);
            Environment.ExitCode = 0;
            Environment.Exit(Environment.ExitCode);
        }
    }
}
