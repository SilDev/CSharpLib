#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ComSpec.cs
// Version:  2020-01-13 13:04
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Intern
{
    using System;
    using System.IO;

    internal static class ComSpec
    {
        internal const string DefaultEnvDir = "%SystemRoot%\\System32";
#if x86
        internal const string LowestEnvDir = "%SystemRoot%\\System32";
#elif x64
        internal const string LowestEnvDir = "%SystemRoot%\\SysWOW64";
#else
        private static string _lowestEnvDir;
        internal static string LowestEnvDir => _lowestEnvDir ??= Environment.Is64BitProcess ? "%SystemRoot%\\SysWOW64" : "%SystemRoot%\\System32";
#endif
#if x64
        internal const string SysNativeEnvDir = "%SystemRoot%\\System32";
#else
        private static string _sysNativeEnvDir;
        internal static string SysNativeEnvDir => _sysNativeEnvDir ??= Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess ? "%SystemRoot%\\Sysnative" : "%SystemRoot%\\System32";
#endif
        private static string _defaultEnvPath,
                              _defaultPath,
                              _lowestEnvPath,
                              _lowestPath,
                              _sysNativeEnvPath,
                              _sysNativePath;

        internal static string DefaultEnvPath => _defaultEnvPath ??= Path.Combine(DefaultEnvDir, "cmd.exe");

        internal static string DefaultPath => _defaultPath ??= PathEx.Combine(DefaultEnvPath);

        internal static string LowestEnvPath => _lowestEnvPath ??= Path.Combine(LowestEnvDir, "cmd.exe");

        internal static string LowestPath => _lowestPath ??= PathEx.Combine(LowestEnvPath);

        internal static string SysNativeEnvPath => _sysNativeEnvPath ??= Path.Combine(SysNativeEnvDir, "cmd.exe");

        internal static string SysNativePath => _sysNativePath ??= PathEx.Combine(SysNativeEnvPath);
    }
}
