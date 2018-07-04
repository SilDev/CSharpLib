#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ShellLink.cs
// Version:  2018-07-04 12:06
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.InteropServices.ComTypes;
    using Intern;

    /// <summary>
    ///     Provides enumerated values of window show statements.
    /// </summary>
    public enum ShellLinkShowState
    {
        /// <summary>
        ///     Activates and displays a window.
        /// </summary>
        Normal = 0x1,

        /// <summary>
        ///     Activates the window and displays it as a maximized window.
        /// </summary>
        Maximized = 0x3,

        /// <summary>
        ///     Displays the window as a minimized window.
        /// </summary>
        MinNoActive = 0x7
    }

    /// <summary>
    ///     Specifies a set of values that are used when you create a shell link.
    /// </summary>
    public struct ShellLinkInfo
    {
        /// <summary>
        ///     The description for the link.
        /// </summary>
        public string Description;

        /// <summary>
        ///     The arguments which applies when the link is started.
        /// </summary>
        public string Arguments;

        /// <summary>
        ///     The file or directory to be linked.
        /// </summary>
        public string TargetPath;

        /// <summary>
        ///     The working directory for the link to be started.
        /// </summary>
        public string WorkingDirectory;

        /// <summary>
        ///     The icon resource path and resource identifier.
        /// </summary>
        public (string, int) IconLocation;

        /// <summary>
        ///     The show state of the window.
        /// </summary>
        public ShellLinkShowState ShowState;

        /// <summary>
        ///     The fully qualified name of the link.
        /// </summary>
        public string LinkPath;
    }

    /// <summary>
    ///     Provides the functionality to handle shell links.
    /// </summary>
    public static class ShellLink
    {
        /// <summary>
        ///     Creates a shell link based on the specified <see cref="ShellLinkInfo"/> structure.
        /// </summary>
        /// <param name="shellLinkInfo">
        ///     The <see cref="ShellLinkInfo"/> structure.
        /// </param>
        /// <param name="skipExists">
        ///     true to skip existing links without further checks; otherwise, false.
        /// </param>
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static bool Create(ShellLinkInfo shellLinkInfo, bool skipExists = false)
        {
            try
            {
                var linkExt = Path.GetExtension(shellLinkInfo.LinkPath);
                var linkPath = PathEx.Combine(!linkExt.EqualsEx(".lnk") ? $"{shellLinkInfo.LinkPath}.lnk" : shellLinkInfo.LinkPath);
                var linkDir = Path.GetDirectoryName(linkPath);
                if (!Directory.Exists(linkDir))
                    throw new PathNotFoundException(linkDir);

                if (File.Exists(linkPath))
                {
                    if (skipExists)
                        return true;
                    File.Delete(linkPath);
                }

                var targetPath = PathEx.Combine(shellLinkInfo.TargetPath);
                if (!PathEx.DirOrFileExists(targetPath))
                    throw new PathNotFoundException(targetPath);

                var shell = (ComImports.IShellLink)new ComImports.ShellLink();
                var description = shellLinkInfo.Description;
                if (!string.IsNullOrWhiteSpace(description))
                    shell.SetDescription(description);

                var workDir = PathEx.Combine(shellLinkInfo.WorkingDirectory);
                if (Directory.Exists(workDir))
                {
                    workDir = EnvironmentEx.GetVariablePathFull(workDir, false, false);
                    shell.SetWorkingDirectory(workDir);
                }

                targetPath = EnvironmentEx.GetVariablePathFull(targetPath, false, false);
                shell.SetPath(targetPath);

                var arguments = shellLinkInfo.Arguments;
                if (!string.IsNullOrWhiteSpace(arguments))
                    shell.SetArguments(arguments);

                var iconPath = PathEx.Combine(shellLinkInfo.IconLocation.Item1);
                var iconId = shellLinkInfo.IconLocation.Item2;
                if (File.Exists(iconPath))
                    iconPath = EnvironmentEx.GetVariablePathFull(iconPath, false, false);
                else
                {
                    if (PathEx.IsDir(targetPath))
                    {
                        iconPath = "%SystemRoot%\\System32\\imageres.dll";
                        iconId = 3;
                    }
                    else
                    {
                        iconPath = targetPath;
                        iconId = 0;
                    }
                }
                shell.SetIconLocation(iconPath, iconId);

                var showState = (int)shellLinkInfo.ShowState;
                if (showState > 1)
                    shell.SetShowCmd(showState);

                ((IPersistFile)shell).Save(linkPath, false);
                return File.Exists(linkPath);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Removes a link of the specified file or directory.
        /// </summary>
        /// <param name="path">
        ///     The shell link to be removed.
        /// </param>
        public static bool Destroy(string path)
        {
            try
            {
                var target = GetTarget(path);
                if (string.IsNullOrEmpty(target))
                    throw new ArgumentNullException(nameof(target));
                var link = PathEx.Combine(path);
                File.Delete(link);
                return File.Exists(link);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Returns the target path of the specified link.
        /// </summary>
        /// <param name="path">
        ///     The link to get the target path.
        /// </param>
        public static string GetTarget(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var file = PathEx.Combine(path);
                if (!File.Exists(file))
                    throw new PathNotFoundException(file);
                if (!Path.GetExtension(file).EqualsEx(".lnk"))
                    throw new ArgumentException();
                string targetPath;
                using (var fs = File.Open(file, FileMode.Open, FileAccess.Read))
                {
                    var br = new BinaryReader(fs);
                    fs.Seek(0x14, SeekOrigin.Begin);
                    var flags = br.ReadUInt32();
                    if ((flags & 1) == 1)
                    {
                        fs.Seek(0x4c, SeekOrigin.Begin);
                        fs.Seek(br.ReadUInt16(), SeekOrigin.Current);
                    }
                    var start = fs.Position;
                    var length = br.ReadUInt32();
                    fs.Seek(0xc, SeekOrigin.Current);
                    fs.Seek(start + br.ReadUInt32(), SeekOrigin.Begin);
                    targetPath = new string(br.ReadChars((int)(start + length - fs.Position - 2)));
                    var begin = targetPath.IndexOf("\0\0", StringComparison.Ordinal);
                    if (begin <= -1)
                        return targetPath;
                    var end = targetPath.IndexOf(new string(Path.DirectorySeparatorChar, 2), begin + 2, StringComparison.Ordinal) + 2;
                    end = targetPath.IndexOf('\0', end) + 1;
                    targetPath = Path.Combine(targetPath.Substring(0, begin), targetPath.Substring(end));
                }
                return targetPath;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }
    }
}
