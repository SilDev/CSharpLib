#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: SymbolicLink.cs
// Version:  2019-10-22 16:15
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using Properties;

    /// <summary>
    ///     Provides the functionality to handle symbolic links.
    /// </summary>
    public static class SymbolicLink
    {
        /// <summary>
        ///     Creates a symbolic link to the specified file or directory based on command prompt
        ///     which allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="linkPath">
        ///     The file or directory to be linked.
        /// </param>
        /// <param name="destPath">
        ///     The fully qualified name of the new link.
        /// </param>
        /// <param name="destIsDir">
        ///     true to determine that the destination path is a directory; otherwise, false.
        /// </param>
        /// <param name="backup">
        ///     true to create an backup for existing files; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to create this link with highest privileges; otherwise, false.
        /// </param>
        public static bool Create(string linkPath, string destPath, bool destIsDir, bool backup = false, bool elevated = false)
        {
            #region p/invoke

            /*
             * The idea was to replace the code below with this code that uses the
             * p/invoke method to create symbolic links. But this doesn't work
             * without administrator privileges while a CMD function called
             * MKLINK can do that simply as normal user...

            var dest = PathEx.Combine(targetPath);
            try
            {
                if (targetIsDir)
                {
                    if (!Directory.Exists(dest))
                        Directory.CreateDirectory(dest);
                }
                else
                {
                    if (!File.Exists(dest))
                        File.Create(dest).Close();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }

            var link = PathEx.Combine(linkPath);
            try
            {
                var linkDir = Path.GetDirectoryName(link);
                if (linkDir != null && !Directory.Exists(linkDir))
                    Directory.CreateDirectory(linkDir);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }

            if (PathEx.DirOrFileExists(link))
                if (!DirIsLink(link) && backup)
                    try
                    {
                        File.Move(link, link + $"-{{{EnvironmentEx.MachineId}}}.backup");
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                        return false;
                    }
                else
                    try
                    {
                        if (Directory.Exists(link))
                            Directory.Delete(link);
                        if (File.Exists(link))
                            File.Delete(link);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                    }


            if (!PathEx.DirOrFileExists(dest) || PathEx.DirOrFileExists(link))
                return false;

            var created = WinApi.SafeNativeMethods.CreateSymbolicLink(link, dest, (WinApi.SymbolicLinkFlags)Convert.ToInt32(targetIsDir));
            if (created)
                SetAttributes(link, FileAttributes.Hidden);

            return created;
            */

            #endregion

            var dest = PathEx.Combine(destPath);
            try
            {
                if (destIsDir)
                {
                    if (!Directory.Exists(dest))
                        Directory.CreateDirectory(dest);
                }
                else
                {
                    if (!File.Exists(dest))
                        File.Create(dest).Close();
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }

            var link = PathEx.Combine(linkPath);
            try
            {
                var linkDir = Path.GetDirectoryName(link);
                if (!Directory.Exists(linkDir) && linkDir != null)
                    Directory.CreateDirectory(linkDir);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }

            var sb = new StringBuilder();
            if (backup && PathEx.DirOrFileExists(link))
                if (!DirectoryEx.IsLink(link))
                {
                    var prior = string.Format(CultureInfo.InvariantCulture, Resources.BackupFormat, link, EnvironmentEx.MachineId);
                    sb.AppendFormat(CultureInfo.InvariantCulture, "MOVE /Y \"{0}\" \"{1}\"", link, prior);
                }
                else
                    Destroy(link, true, true, elevated);

            if (PathEx.DirOrFileExists(link))
            {
                if (sb.Length > 0)
                    sb.Append(" & ");
                sb.AppendFormat(CultureInfo.InvariantCulture, destIsDir ? "RMDIR /S /Q \"{0}\"" : "DEL /F /Q \"{0}\"", link);
            }

            if (PathEx.DirOrFileExists(dest))
            {
                if (sb.Length > 0)
                    sb.Append(" & ");
                sb.Append("MKLINK");
                if (destIsDir)
                    sb.Append(" /J");
                sb.AppendFormat(CultureInfo.InvariantCulture, " \"{0}\" \"{1}\" && ATTRIB +H \"{0}\" /L", link, dest);
            }

            if (sb.Length <= 0)
                return false;

            int? exitCode;
            using (var p = ProcessEx.Send(sb.ToString(), elevated, false))
            {
                if (p?.HasExited == false)
                    p.WaitForExit();
                exitCode = p?.ExitCode;
            }
            return exitCode == 0 && PathEx.DirOrFileIsLink(link);
        }

        /// <summary>
        ///     Removes an symbolic link of the specified file or directory link based on command
        ///     prompt which allows a simple solution for the elevated execution of this order.
        /// </summary>
        /// <param name="path">
        ///     The link to be removed.
        /// </param>
        /// <param name="pathIsDir">
        ///     true to determine that the path is a directory; otherwise, false.
        /// </param>
        /// <param name="backup">
        ///     true to restore found backups; otherwise, false.
        /// </param>
        /// <param name="elevated">
        ///     true to remove this link with highest privileges; otherwise, false.
        /// </param>
        public static bool Destroy(string path, bool pathIsDir, bool backup = false, bool elevated = false)
        {
            var link = PathEx.Combine(path);
            var isLink = PathEx.DirOrFileIsLink(link);

            var sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, pathIsDir ? "RMDIR /Q \"{0}\"" : isLink ? "DEL /F /Q /A:L \"{0}\"" : "DEL /F /Q \"{0}\"", link);

            var prior = string.Format(CultureInfo.InvariantCulture, Resources.BackupFormat, link, EnvironmentEx.MachineId);
            if (backup && PathEx.DirOrFileExists(prior))
                sb.AppendFormat(CultureInfo.InvariantCulture, " && MOVE /Y \"{0}\" \"{1}\"", prior, link);

            if (sb.Length <= 0)
                return false;

            int? exitCode;
            using (var p = ProcessEx.Send(sb.ToString(), elevated, false))
            {
                if (p?.HasExited == false)
                    p.WaitForExit();
                exitCode = p?.ExitCode;
            }

            return exitCode == 0 && isLink;
        }
    }
}
