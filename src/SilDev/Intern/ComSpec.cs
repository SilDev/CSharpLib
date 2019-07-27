#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ComSpec.cs
// Version:  2019-07-27 08:50
// 
// Copyright (c) 2019, Si13n7 Developments (r)
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

        internal static string LowestEnvDir
        {
            get
            {
                if (_lowestEnvDir == default(string))
                    _lowestEnvDir = Environment.Is64BitProcess ? "%SystemRoot%\\SysWOW64" : "%SystemRoot%\\System32";
                return _lowestEnvDir;
            }
        }
#endif
#if x86 || x64
        internal const string SysNativeEnvDir = "%SystemRoot%\\System32";
#else
        private static string _sysNativeEnvDir;

        internal static string SysNativeEnvDir
        {
            get
            {
                if (_sysNativeEnvDir == default(string))
                    _sysNativeEnvDir = Environment.Is64BitProcess ? "%SystemRoot%\\Sysnative" : "%SystemRoot%\\System32";
                return _sysNativeEnvDir;
            }
        }
#endif

        private static string _defaultEnvPath,
                              _defaultPath,
                              _lowestEnvPath,
                              _lowestPath,
                              _sysNativeEnvPath,
                              _sysNativePath;

        internal static string DefaultEnvPath
        {
            get
            {
                if (_defaultEnvPath == default(string))
                    _defaultEnvPath = Path.Combine(DefaultEnvDir, "cmd.exe");
                return _defaultEnvPath;
            }
        }

        internal static string DefaultPath
        {
            get
            {
                if (_defaultPath == default(string))
                    _defaultPath = PathEx.Combine(DefaultEnvPath);
                return _defaultPath;
            }
        }

        internal static string LowestEnvPath
        {
            get
            {
                if (_lowestEnvPath == default(string))
                    _lowestEnvPath = Path.Combine(LowestEnvDir, "cmd.exe");
                return _lowestEnvPath;
            }
        }

        internal static string LowestPath
        {
            get
            {
                if (_lowestPath == default(string))
                    _lowestPath = PathEx.Combine(LowestEnvPath);
                return _lowestPath;
            }
        }

        internal static string SysNativeEnvPath
        {
            get
            {
                if (_sysNativeEnvPath == default(string))
                    _sysNativeEnvPath = Path.Combine(SysNativeEnvDir, "cmd.exe");
                return _sysNativeEnvPath;
            }
        }

        internal static string SysNativePath
        {
            get
            {
                if (_sysNativePath == default(string))
                    _sysNativePath = PathEx.Combine(SysNativeEnvPath);
                return _sysNativePath;
            }
        }
    }
}
