#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Elevation.cs
// Version:  2018-03-24 16:47
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Security.Principal;

    /// <summary>
    ///     Provides funcionality for the user authorization on Windows.
    /// </summary>
    public static class Elevation
    {
        /// <summary>
        ///     Returns a <see cref="WindowsPrincipal"/> object that represents the current
        ///     Windows user.
        /// </summary>
        public static WindowsPrincipal CurrentPrincipal => new WindowsPrincipal(WindowsIdentity.GetCurrent());

        /// <summary>
        ///     Determines whether the current principal belongs to the Windows administrator
        ///     user group.
        /// </summary>
        public static bool IsAdministrator => CurrentPrincipal.IsInRole(WindowsBuiltInRole.Administrator);

        /// <summary>
        ///     Determines whether the current principal has enough privileges to write in the
        ///     specified directory.
        /// </summary>
        /// <param name="path">
        ///     The path to check.
        /// </param>
        public static bool WritableLocation(string path)
        {
            var result = false;
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var dir = PathEx.Combine(path);
                if (!Directory.Exists(dir))
                    dir = Path.GetDirectoryName(dir);
                if (!Directory.Exists(dir))
                    throw new PathNotFoundException(path);
                var acl = Directory.GetAccessControl(dir);
                foreach (var rule in acl.GetAccessRules(true, true, typeof(NTAccount)).Cast<FileSystemAccessRule>())
                {
                    if ((rule.FileSystemRights & FileSystemRights.Write) == 0 || !CurrentPrincipal.IsInRole(rule.IdentityReference.Value))
                        continue;
                    result = rule.AccessControlType == AccessControlType.Allow;
                    break;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return result;
        }

        /// <summary>
        ///     Determines whether the current principal has enough privileges to write in the
        ///     <see cref="PathEx.LocalDir"/> directory.
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
                    args = $"/{Log.DebugKey} {Log.DebugMode} ";
                args += EnvironmentEx.CommandLine(false);
            }
            ProcessEx.Start(PathEx.LocalPath, PathEx.LocalDir, args, true);
            Environment.ExitCode = 0;
            Environment.Exit(Environment.ExitCode);
        }
    }
}
