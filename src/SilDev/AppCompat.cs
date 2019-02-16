#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: AppCompat.cs
// Version:  2019-02-16 20:15
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using Microsoft.Win32;

    /// <summary>
    ///     Provides color mode options. For more information, see
    ///     <see cref="AppCompat.SetLayers(string, AppCompatLayers)"/>.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum AppCompatColorModes
    {
        /// <summary>
        ///     System default.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     Reduce color mode to 8-bit (256).
        /// </summary>
        _256Color = 1,

        /// <summary>
        ///     Reduce color mode to 16-bit (65536).
        /// </summary>
        _16BitColor = 2
    }

    /// <summary>
    ///     Provides DPI scaling behavior options. For more information, see
    ///     <see cref="AppCompat.SetLayers(string, AppCompatLayers)"/>.
    /// </summary>
    public enum AppCompatDpiScalingBehaviors
    {
        /// <summary>
        ///     System default.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     DPI scaling performed by application.
        /// </summary>
        HighDpiWare = 1,

        /// <summary>
        ///     DPI scaling performed by system.
        /// </summary>
        DpiUnaware = 2,

        /// <summary>
        ///     DPI scaling performed by system (enhanced).
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        GdiDpiScaling_DpiUnaware = 4
    }

    /// <summary>
    ///     Provides DPI scaling system options. For more information, see
    ///     <see cref="AppCompat.SetLayers(string, AppCompatLayers)"/>.
    /// </summary>
    public enum AppCompatDpiScalingSystems
    {
        /// <summary>
        ///     System default.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     Windows logon.
        /// </summary>
        PerProcessSystemDpiForceOff = 1,

        /// <summary>
        ///     Application start.
        /// </summary>
        PerProcessSystemDpiForceOn = 2
    }

    /// <summary>
    ///     Provides OS version options. For more information, see
    ///     <see cref="AppCompat.SetLayers(string, AppCompatLayers)"/>.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum AppCompatSystemVersions
    {
        /// <summary>
        ///     System default.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     Windows 95.
        /// </summary>
        Win95 = 1,

        /// <summary>
        ///     Windows 98 / Windows ME.
        /// </summary>
        Win98 = 2,

        /// <summary>
        ///     Windows XP (Service Pack 2).
        /// </summary>
        WinXPSP2 = 4,

        /// <summary>
        ///     Windows XP (Service Pack 3).
        /// </summary>
        WinXPSP3 = 8,

        /// <summary>
        ///     Windows Vista.
        /// </summary>
        VistaRTM = 16,

        /// <summary>
        ///     Windows Vista (Service Pack 1).
        /// </summary>
        VistaSP1 = 32,

        /// <summary>
        ///     Windows Vista (Service Pack 2).
        /// </summary>
        VistaSP2 = 64,

        /// <summary>
        ///     Windows 7.
        /// </summary>
        Win7RTM = 128,

        /// <summary>
        ///     Windows 8.
        /// </summary>
        Win8RTM = 256
    }

    /// <summary>
    ///     Appliciation compatiblity layers struct. For more information, see
    ///     <see cref="AppCompat.SetLayers(string, AppCompatLayers)"/>.
    /// </summary>
    public struct AppCompatLayers
    {
        /// <summary>
        ///     The color mode.
        /// </summary>
        public AppCompatColorModes ColorMode;

        /// <summary>
        ///     The DPI scaling behavior.
        /// </summary>
        public AppCompatDpiScalingBehaviors DpiScalingBehavior;

        /// <summary>
        ///     The DPI scaling system.
        /// </summary>
        public AppCompatDpiScalingSystems DpiScalingSystem;

        /// <summary>
        ///     The operating system.
        /// </summary>
        public AppCompatSystemVersions OperatingSystem;

        /// <summary>
        ///     true to disable the Windows 10 fullscreen optimizations; otherwise, false.
        /// </summary>
        public bool DisableFullscreenOptimizations;

        /// <summary>
        ///     true to run the program in 640x480 screen resolution; otherwise, false.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public bool RunIn640x480ScreenResolution;

        /// <summary>
        ///     true to run the program as administrator; otherwise, false.
        /// </summary>
        public bool RunAsAdministrator;
    }

    /// <summary>
    ///     Provides functionality for the Windows application compatibility layers.
    /// </summary>
    public static class AppCompat
    {
        /// <summary>
        ///     Enables the specified application compatibility layers for the specified executable file.
        /// </summary>
        /// <param name="path">
        ///     The path to the file to be configured.
        /// </param>
        /// <param name="compatLayers">
        ///     The compatibility layers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     path is null or empty.
        /// </exception>
        /// <exception cref="ArgumentInvalidException">
        ///     path is not a valid executable file.
        /// </exception>
        /// <exception cref="PathNotFoundException">
        ///     target does not exist.
        /// </exception>
        public static void SetLayers(string path, AppCompatLayers compatLayers)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            var file = PathEx.Combine(path);
            if (!File.Exists(file))
                throw new PathNotFoundException(file);

            var type = PortableExecutable.GetMachineTypes(file);
            if (type != MachineTypes.AMD64 && type != MachineTypes.I386)
                throw new ArgumentInvalidException(nameof(path));

            const string keyPath = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers";
            var builder = new StringBuilder();
            var osVer = Environment.OSVersion.Version;
            if (compatLayers.DisableFullscreenOptimizations)
                builder.AppendLine("DISABLEDXMAXIMIZEDWINDOWEDMODE");
            if (compatLayers.RunAsAdministrator)
                builder.AppendLine("RUNASADMIN");
            if (compatLayers.RunIn640x480ScreenResolution)
                builder.AppendLine("640x480");
            if (osVer.Major >= 10 && compatLayers.DpiScalingSystem != AppCompatDpiScalingSystems.Default)
                builder.AppendLine(Enum.GetName(typeof(AppCompatDpiScalingSystems), compatLayers.DpiScalingSystem)?.ToUpper());
            if (osVer.Major >= 10 && compatLayers.DpiScalingBehavior != AppCompatDpiScalingBehaviors.Default)
                builder.AppendLine(Enum.GetName(typeof(AppCompatDpiScalingBehaviors), compatLayers.DpiScalingBehavior)?.ToUpper());
            if (compatLayers.ColorMode != AppCompatColorModes.Default)
                builder.AppendLine(Enum.GetName(typeof(AppCompatColorModes), compatLayers.ColorMode)?.ToUpper().TrimStart('_'));
            if (compatLayers.OperatingSystem != AppCompatSystemVersions.Default)
            {
                var os = compatLayers.OperatingSystem;
                if (osVer.Major == 6)
                    if (osVer.Minor > 1)
                    {
                        if (os > AppCompatSystemVersions.Win7RTM)
                            os = AppCompatSystemVersions.VistaSP2;
                    }
                    else
                    {
                        if (os > AppCompatSystemVersions.VistaSP2)
                            os = AppCompatSystemVersions.VistaSP2;
                    }
                builder.AppendLine(Enum.GetName(typeof(AppCompatSystemVersions), os)?.ToUpper());
            }

            var compatFlags = builder.ToString().Replace(Environment.NewLine, " ").Replace("_", " ").Trim();
            if (string.IsNullOrEmpty(compatFlags))
            {
                Reg.RemoveEntry(Registry.CurrentUser, keyPath, path);
                return;
            }
            Reg.Write(Registry.CurrentUser, keyPath, path, $"~ {compatFlags}");
        }
    }
}
