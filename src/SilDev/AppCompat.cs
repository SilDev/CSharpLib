#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: AppCompat.cs
// Version:  2023-11-11 16:27
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.IO;
    using System.Text;
    using Microsoft.Win32;

    /// <summary>
    ///     Provides color mode options. For more information, see
    ///     <see cref="AppCompat.SetLayers(string, AppCompatLayers)"/>.
    /// </summary>
    public enum AppCompatColorMode
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
    public enum AppCompatDpiScalingBehavior
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
        /// ReSharper disable once InconsistentNaming
        GdiDpiScaling_DpiUnaware = 4
    }

    /// <summary>
    ///     Provides DPI scaling system options. For more information, see
    ///     <see cref="AppCompat.SetLayers(string, AppCompatLayers)"/>.
    /// </summary>
    public enum AppCompatDpiScalingSystem
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
    /// ReSharper disable InconsistentNaming
    public enum AppCompatSystemVersion
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
    ///     Application compatibility layers struct. For more information, see
    ///     <see cref="AppCompat.SetLayers(string, AppCompatLayers)"/>.
    /// </summary>
    public struct AppCompatLayers : IEquatable<AppCompatLayers>
    {
        /// <summary>
        ///     The color mode.
        /// </summary>
        public AppCompatColorMode ColorMode { get; set; }

        /// <summary>
        ///     The DPI scaling behavior.
        /// </summary>
        public AppCompatDpiScalingBehavior DpiScalingBehavior { get; set; }

        /// <summary>
        ///     The DPI scaling system.
        /// </summary>
        public AppCompatDpiScalingSystem DpiScalingSystem { get; set; }

        /// <summary>
        ///     The operating system.
        /// </summary>
        public AppCompatSystemVersion OperatingSystem { get; set; }

        /// <summary>
        ///     <see langword="true"/> to disable the Windows 10 fullscreen optimizations;
        ///     otherwise, <see langword="false"/>.
        /// </summary>
        public bool DisableFullscreenOptimizations { get; set; }

        /// <summary>
        ///     <see langword="true"/> to run the program in 640x480 screen resolution;
        ///     otherwise, <see langword="false"/>.
        /// </summary>
        /// ReSharper disable once InconsistentNaming
        public bool RunIn640x480ScreenResolution { get; set; }

        /// <summary>
        ///     <see langword="true"/> to run the program as administrator; otherwise,
        ///     <see langword="false"/>.
        /// </summary>
        public bool RunAsAdministrator { get; set; }

        /// <summary>
        ///     Determines whether this instance have same values as the specified
        ///     <see cref="AppCompatLayers"/> instance.
        /// </summary>
        /// <param name="other">
        ///     The <see cref="AppCompatLayers"/> instance to compare.
        /// </param>
        public readonly bool Equals(AppCompatLayers other) =>
            ColorMode == other.ColorMode &&
            DpiScalingBehavior == other.DpiScalingBehavior &&
            DpiScalingSystem == other.DpiScalingSystem &&
            OperatingSystem == other.OperatingSystem &&
            DisableFullscreenOptimizations == other.DisableFullscreenOptimizations &&
            RunIn640x480ScreenResolution == other.RunIn640x480ScreenResolution &&
            RunAsAdministrator == other.RunAsAdministrator;

        /// <summary>
        ///     Determines whether this instance have same values as the specified
        ///     <see cref="object"/>.
        /// </summary>
        /// <param name="other">
        ///     The  <see cref="object"/> to compare.
        /// </param>
        public readonly override bool Equals(object other)
        {
            if (other is AppCompatLayers acl)
                return Equals(acl);
            return false;
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        public readonly override int GetHashCode() =>
            typeof(AppCompatLayers).GetHashCode();

        /// <summary>
        ///     Determines whether two specified <see cref="AppCompatLayers"/> instances
        ///     have same values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="AppCompatLayers"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="AppCompatLayers"/> instance to compare.
        /// </param>
        public static bool operator ==(AppCompatLayers left, AppCompatLayers right) =>
            left.Equals(right);

        /// <summary>
        ///     Determines whether two specified <see cref="AppCompatLayers"/> instances
        ///     have different values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="AppCompatLayers"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="AppCompatLayers"/> instance to compare.
        /// </param>
        public static bool operator !=(AppCompatLayers left, AppCompatLayers right) =>
            !(left == right);
    }

    /// <summary>
    ///     Provides functionality for the Windows application compatibility layers.
    /// </summary>
    public static class AppCompat
    {
        /// <summary>
        ///     Sets the specified application compatibility layers for the specified
        ///     executable file.
        /// </summary>
        /// <param name="path">
        ///     The path to the file to be configured.
        /// </param>
        /// <param name="compatLayers">
        ///     The compatibility layers.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     path is null, empty or consists only of white-space characters.
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
                throw new ArgumentInvalidException(nameof(path));

            var file = PathEx.Combine(path);
            if (!File.Exists(file))
                throw new PathNotFoundException(file);

            var type = PortableExecutable.GetMachineTypes(file);
            if (type != MachineType.AMD64 && type != MachineType.I386)
                throw new ArgumentInvalidException(nameof(path));

            const string keyPath = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers";
            var builder = new StringBuilder();
            var osVer = EnvironmentEx.OperatingSystemVersion;
            if (compatLayers.DisableFullscreenOptimizations)
                builder.AppendLine("DISABLEDXMAXIMIZEDWINDOWEDMODE");
            if (compatLayers.RunAsAdministrator)
                builder.AppendLine("RUNASADMIN");
            if (compatLayers.RunIn640x480ScreenResolution)
                builder.AppendLine("640x480");
            if (osVer.Major >= 10 && compatLayers.DpiScalingSystem != AppCompatDpiScalingSystem.Default)
                builder.AppendLine(Enum.GetName(typeof(AppCompatDpiScalingSystem), compatLayers.DpiScalingSystem)?.ToUpperInvariant());
            if (osVer.Major >= 10 && compatLayers.DpiScalingBehavior != AppCompatDpiScalingBehavior.Default)
                builder.AppendLine(Enum.GetName(typeof(AppCompatDpiScalingBehavior), compatLayers.DpiScalingBehavior)?.ToUpperInvariant());
            if (compatLayers.ColorMode != AppCompatColorMode.Default)
                builder.AppendLine(Enum.GetName(typeof(AppCompatColorMode), compatLayers.ColorMode)?.ToUpperInvariant().TrimStart('_'));
            if (compatLayers.OperatingSystem != AppCompatSystemVersion.Default)
            {
                var os = compatLayers.OperatingSystem;
                if (osVer.Major == 6)
                    if (osVer.Minor > 1)
                    {
                        if (os > AppCompatSystemVersion.Win7RTM)
                            os = AppCompatSystemVersion.VistaSP2;
                    }
                    else
                    {
                        if (os > AppCompatSystemVersion.VistaSP2)
                            os = AppCompatSystemVersion.VistaSP2;
                    }
                builder.AppendLine(Enum.GetName(typeof(AppCompatSystemVersion), os)?.ToUpperInvariant());
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
