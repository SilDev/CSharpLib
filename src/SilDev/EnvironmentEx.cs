#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: EnvironmentEx.cs
// Version:  2017-06-06 20:52
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Management;
    using System.Reflection;
    using Microsoft.Win32;

    /// <summary>
    ///     Provides static methods based on the <see cref="Environment"/> class to provide informations
    ///     about the current environment.
    /// </summary>
    public static class EnvironmentEx
    {
        private static List<string> _cmdLineArgs;
        private static bool _cmdLineArgsQuotes;
        private static string _commandLine;
        private static Version _version;

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string <see cref="List{T}"/>
        ///     containing the command-line arguments for the current process.
        /// </summary>
        /// <param name="sort">
        ///     true to sort the arguments ascended with the rules of
        ///     <see cref="Comparison.AlphanumericComparer(bool)"/> before returning the arguments;
        ///     otherwise, false.
        /// </param>
        /// <param name="skip">
        ///     The number of arguments to skip before returning the remaining arguments.
        /// </param>
        /// <param name="quotes">
        ///     true to store the arguments in quotation marks which containing spaces; otherwise,
        ///     false.
        /// </param>
        public static List<string> CommandLineArgs(bool sort = true, int skip = 1, bool quotes = true)
        {
            if (_cmdLineArgs?.Count == Environment.GetCommandLineArgs().Length - skip && quotes == _cmdLineArgsQuotes)
                return _cmdLineArgs;
            _cmdLineArgs = new List<string>();
            _cmdLineArgsQuotes = quotes;
            if (Environment.GetCommandLineArgs().Length <= skip)
                return _cmdLineArgs;
            string arg = null;
            var defaultArgs = Environment.GetCommandLineArgs().ToList();
            if (defaultArgs.Any(x => (arg = x).EqualsEx("/debug")))
            {
                var index = defaultArgs.IndexOf(arg);
                defaultArgs.RemoveAt(index);
                if (defaultArgs.Count > index)
                    defaultArgs.RemoveAt(index);
            }
            if (defaultArgs.Count > skip)
                defaultArgs = defaultArgs.Skip(skip).ToList();
            if (sort)
                defaultArgs = defaultArgs.OrderBy(x => x, new Comparison.AlphanumericComparer()).ToList();
            _cmdLineArgs = quotes ? defaultArgs.Select(x => x.Any(char.IsWhiteSpace) ? $"\"{x}\"" : x).ToList() : defaultArgs;
            return _cmdLineArgs;
        }

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string <see cref="List{T}"/>
        ///     containing the command-line arguments for the current process.
        /// </summary>
        /// <param name="sort">
        ///     true to sort the arguments ascended with the rules of
        ///     <see cref="Comparison.AlphanumericComparer(bool)"/> before returning the arguments;
        ///     otherwise, false.
        /// </param>
        /// <param name="quotes">
        ///     true to store the arguments in quotation marks which containing spaces; otherwise,
        ///     false.
        /// </param>
        public static List<string> CommandLineArgs(bool sort, bool quotes) =>
            CommandLineArgs(sort, 1, quotes);

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string <see cref="List{T}"/>
        ///     containing the command-line arguments for the current process.
        /// </summary>
        /// <param name="skip">
        ///     The number of arguments to skip before returning the remaining arguments.
        /// </param>
        public static List<string> CommandLineArgs(int skip) =>
            CommandLineArgs(true, skip);

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string containing the
        ///     command-line arguments for the current process.
        /// </summary>
        /// <param name="sort">
        ///     true to sort the arguments ascended with the rules of
        ///     <see cref="Comparison.AlphanumericComparer(bool)"/> before returning the arguments;
        ///     otherwise, false.
        /// </param>
        /// <param name="skip">
        ///     The number of arguments to skip before returning the remaining arguments.
        /// </param>
        /// <param name="quotes">
        ///     true to store the arguments in quotation marks which containing spaces; otherwise,
        ///     false.
        /// </param>
        public static string CommandLine(bool sort = true, int skip = 1, bool quotes = true)
        {
            if (CommandLineArgs(sort).Count > 0)
                _commandLine = CommandLineArgs(sort, skip, quotes).Join(" ");
            return _commandLine ?? string.Empty;
        }

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string containing the
        ///     command-line arguments for the current process.
        /// </summary>
        /// <param name="sort">
        ///     true to sort the arguments ascended with the rules of
        ///     <see cref="Comparison.AlphanumericComparer(bool)"/> before returning the arguments;
        ///     otherwise, false.
        /// </param>
        /// <param name="quotes">
        ///     true to store the arguments in quotation marks which containing spaces; otherwise,
        ///     false.
        /// </param>
        public static string CommandLine(bool sort, bool quotes) =>
            CommandLine(true, 1, quotes);

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string containing the
        ///     command-line arguments for the current process.
        /// </summary>
        /// <param name="skip">
        ///     The number of arguments to skip before returning the remaining arguments.
        /// </param>
        public static string CommandLine(int skip) =>
            CommandLine(true, skip);

        /// <summary>
        ///     <para>
        ///         Retrieves the value of an environment variable from the current process.
        ///     </para>
        ///     <para>
        ///         <c>
        ///             Hint:
        ///         </c>
        ///         Allows <see cref="Environment.SpecialFolder"/> names inlcuding a
        ///         keyword "CurDir" to get the current code base location based
        ///         on <see cref="Assembly.GetEntryAssembly()"/>.CodeBase.
        ///     </para>
        /// </summary>
        /// <param name="variable">
        ///     The name of the environment variable.
        /// </param>
        /// <param name="lower">
        ///     true to convert the result to lowercase; otherwise, false.
        /// </param>
        public static string GetVariableValue(string variable, bool lower = false)
        {
            var output = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(variable))
                    throw new ArgumentNullException(nameof(variable));
                variable = variable.RemoveChar('%');
                if (variable.EqualsEx("CurrentDir", "CurDir"))
                    output = PathEx.LocalDir;
                else
                    try
                    {
                        var match = Enum.GetNames(typeof(Environment.SpecialFolder))
                                        .First(s => variable.EqualsEx(s));
                        Environment.SpecialFolder specialFolder;
                        if (!Enum.TryParse(match, out specialFolder))
                            throw new ArgumentException();
                        output = Environment.GetFolderPath(specialFolder);
                    }
                    catch
                    {
                        output = Environment.GetEnvironmentVariables().Cast<DictionaryEntry>()
                                            .First(x => variable.EqualsEx(x.Key.ToString())).Value.ToString();
                    }
                if (lower)
                    output = output?.ToLower();
            }
            catch (InvalidOperationException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
        }

        /// <summary>
        ///     Converts the specified path to an existing environment variable, if possible.
        /// </summary>
        /// <param name="path">
        ///     The path to convert.
        /// </param>
        /// <param name="curDir">
        ///     true to consider the <see cref="Assembly.GetEntryAssembly()"/>.CodeBase value;
        ///     otherwise, false.
        /// </param>
        public static string GetVariablePath(string path, bool curDir = true)
        {
            var output = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                var current = path.TrimEnd(Path.DirectorySeparatorChar);
                for (var i = 0; i < 2; i++)
                {
                    if (curDir && current.EqualsEx(GetVariableValue("CurDir")))
                        output = "CurDir";
                    else
                        try
                        {
                            output = Enum.GetValues(typeof(Environment.SpecialFolder))
                                         .Cast<Environment.SpecialFolder>()
                                         .First(s => current.EqualsEx(Environment.GetFolderPath(s)))
                                         .ToString();
                        }
                        catch
                        {
                            output = Environment.GetEnvironmentVariables()
                                                .Cast<DictionaryEntry>()
                                                .First(x => current.EqualsEx(x.Value.ToString()))
                                                .Key.ToString();
                        }
                    if (!string.IsNullOrWhiteSpace(output))
                        break;
                    current += Path.DirectorySeparatorChar;
                }
                if (!string.IsNullOrWhiteSpace(output))
                    output = $"%{output}%";
            }
            catch (InvalidOperationException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
        }

        /// <summary>
        ///     Returns a new string in which all occurrences in this instance are replaced
        ///     with a valid environment variable.
        /// </summary>
        /// <param name="path">
        ///     The path to convert.
        /// </param>
        /// <param name="curDir">
        ///     true to consider the <see cref="Assembly.GetEntryAssembly()"/>.CodeBase value;
        ///     otherwise, false.
        /// </param>
        public static string GetVariablePathFull(string path, bool curDir = true)
        {
            var output = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                var levels = path.Count(c => c == Path.DirectorySeparatorChar) + 1;
                var current = PathEx.Combine(path);
                for (var i = 0; i < levels; i++)
                {
                    output = GetVariablePath(current, curDir);
                    if (!string.IsNullOrEmpty(output))
                        break;
                    current = PathEx.Combine(current, "..").Trim(Path.DirectorySeparatorChar);
                }
                if (string.IsNullOrEmpty(output))
                    throw new ArgumentNullException(nameof(output));
                output += Path.DirectorySeparatorChar;
                output += path.Substring(current.Length).Trim(Path.DirectorySeparatorChar);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return !string.IsNullOrWhiteSpace(output) ? output.Trim(Path.DirectorySeparatorChar) : path;
        }

        /// <summary>
        ///     Gets a <see cref="System.Version"/> object that describes the major, minor and
        ///     build numbers of the common language runtime.
        /// </summary>
        public static Version Version
        {
            get
            {
                if (_version != null)
                    return _version;
                var release = Reg.Read(Registry.LocalMachine, @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full", "Release", 0);
                if (release > 460805)
                    _version = new Version(4, 7, 1);
                else if (release >= 460798)
                    _version = new Version(4, 7, 0);
                else if (release >= 394802)
                    _version = new Version(4, 6, 2);
                else if (release >= 394254)
                    _version = new Version(4, 6, 1);
                else if (release >= 393295)
                    _version = new Version(4, 6, 0);
                else if (release >= 379893)
                    _version = new Version(4, 5, 2);
                else if (release >= 378675)
                    _version = new Version(4, 5, 1);
                else if (release >= 378389)
                    _version = new Version(4, 5, 0);
                else
                    _version = Environment.Version;
                return _version;
            }
        }

        /// <summary>
        ///     Provides enumerated values of redistributable packages.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum RedistPack
        {
            /// <summary>
            ///     Visual C++ 2005 Redistributable Package (x86).
            /// </summary>
            VC2005_x86,

            /// <summary>
            ///     Visual C++ 2005 Redistributable Package (x64).
            /// </summary>
            VC2005_x64,

            /// <summary>
            ///     Visual C++ 2008 Redistributable Package (x86).
            /// </summary>
            VC2008_x86,

            /// <summary>
            ///     Visual C++ 2008 Redistributable Package (x64).
            /// </summary>
            VC2008_x64,

            /// <summary>
            ///     Visual C++ 2010 Redistributable Package (x86).
            /// </summary>
            VC2010_x86,

            /// <summary>
            ///     Visual C++ 2010 Redistributable Package (x64).
            /// </summary>
            VC2010_x64,

            /// <summary>
            ///     Visual C++ 2012 Redistributable Package (x86).
            /// </summary>
            VC2012_x86,

            /// <summary>
            ///     Visual C++ 2012 Redistributable Package (x64).
            /// </summary>
            VC2012_x64,

            /// <summary>
            ///     Visual C++ 2013 Redistributable Package (x86).
            /// </summary>
            VC2013_x86,

            /// <summary>
            ///     Visual C++ 2013 Redistributable Package (x64).
            /// </summary>
            VC2013_x64,

            /// <summary>
            ///     Visual C++ 2015 Redistributable Package (x86).
            /// </summary>
            VC2015_x86,

            /// <summary>
            ///     Visual C++ 2015 Redistributable Package (x64).
            /// </summary>
            VC2015_x64
        }

        /// <summary>
        ///     Determines whether the specified redistributable package is installed.
        /// </summary>
        /// <param name="key">
        ///     The redistributable package key value to check.
        /// </param>
        public static bool RedistPackIsInstalled(RedistPack key)
        {
            string[] guids = null;
            switch (key)
            {
                case RedistPack.VC2005_x86:
                    guids = new[]
                    {
                        "{A49F249F-0C91-497F-86DF-B2585E8E76B7}",
                        "{7299052B-02A4-4627-81F2-1818DA5D550D}",
                        "{837B34E3-7C30-493C-8F6A-2B0F04E2912C}"
                    };
                    break;
                case RedistPack.VC2005_x64:
                    guids = new[]
                    {
                        "{6E8E85E8-CE4B-4FF5-91F7-04999C9FAE6A}",
                        "{03ED71EA-F531-4927-AABD-1C31BCE8E187}",
                        "{071C9B48-7C32-4621-A0AC-3F809523288F}",
                        "{0F8FB34E-675E-42ED-850B-29D98C2ECE08}",
                        "{6CE5BAE9-D3CA-4B99-891A-1DC6C118A5FC}",
                        "{85025851-A784-46D8-950D-05CB3CA43A13}"
                    };
                    break;
                case RedistPack.VC2008_x86:
                    guids = new[]
                    {
                        "{FF66E9F6-83E7-3A3E-AF14-8DE9A809A6A4}",
                        "{1F1C2DFC-2D24-3E06-BCB8-725134ADF989}",
                        "{9BE518E6-ECC6-35A9-88E4-87755C07200F}"
                    };
                    break;
                case RedistPack.VC2008_x64:
                    guids = new[]
                    {
                        "{350AA351-21FA-3270-8B7A-835434E766AD}",
                        "{2B547B43-DB50-3139-9EBE-37D419E0F5FA}",
                        "{8220EEFE-38CD-377E-8595-13398D740ACE}",
                        "{5827ECE1-AEB0-328E-B813-6FC68622C1F9}",
                        "{4B6C7001-C7D6-3710-913E-5BC23FCE91E6}",
                        "{977AD349-C2A8-39DD-9273-285C08987C7B}",
                        "{5FCE6D76-F5DC-37AB-B2B8-22AB8CEDB1D4}",
                        "{515643D1-4E9E-342F-A75A-D1F16448DC04}"
                    };
                    break;
                case RedistPack.VC2010_x86:
                    guids = new[]
                    {
                        "{196BB40D-1578-3D01-B289-BEFC77A11A1E}",
                        "{F0C3E5D1-1ADE-321E-8167-68EF0DE699A5}"
                    };
                    break;
                case RedistPack.VC2010_x64:
                    guids = new[]
                    {
                        "{DA5E371C-6333-3D8A-93A4-6FD5B20BCC6E}",
                        "{C1A35166-4301-38E9-BA67-02823AD72A1B}",
                        "{1D8E6291-B0D5-35EC-8441-6616F567A0F7}",
                        "{88C73C1C-2DE5-3B01-AFB8-B46EF4AB41CD}"
                    };
                    break;
                case RedistPack.VC2012_x86:
                    guids = new[]
                    {
                        "{BD95A8CD-1D9F-35AD-981A-3E7925026EBB}",
                        "{B175520C-86A2-35A7-8619-86DC379688B9}"
                    };
                    break;
                case RedistPack.VC2012_x64:
                    guids = new[]
                    {
                        "{CF2BEA3C-26EA-32F8-AA9B-331F7E34BA97}",
                        "{37B8F9C7-03FB-3253-8781-2517C99D7C00}"
                    };
                    break;
                case RedistPack.VC2013_x86:
                    guids = new[]
                    {
                        "{13A4EE12-23EA-3371-91EE-EFB36DDFFF3E}",
                        "{F8CFEB22-A2E7-3971-9EDA-4B11EDEFC185}"
                    };
                    break;
                case RedistPack.VC2013_x64:
                    guids = new[]
                    {
                        "{A749D8E6-B613-3BE3-8F5F-045C84EBA29B}",
                        "{929FBD26-9020-399B-9A7A-751D61F0B942}"
                    };
                    break;
                case RedistPack.VC2015_x86:
                    guids = new[]
                    {
                        "{A2563E55-3BEC-3828-8D67-E5E8B9E8B675}",
                        "{BE960C1C-7BAD-3DE6-8B1A-2616FE532845}"
                    };
                    break;
                case RedistPack.VC2015_x64:
                    guids = new[]
                    {
                        "{0D3E9E15-DE7A-300B-96F1-B4AF12B96488}",
                        "{BC958BD2-5DAC-3862-BB1A-C1BE0790438D}"
                    };
                    break;
            }
            return guids?.Any(x => WinApi.UnsafeNativeMethods.MsiQueryProductState(x) == WinApi.INSTALLSTATE.INSTALLSTATE_DEFAULT) == true;
        }

        /// <summary>
        ///     Determines whether the system restoring is enabled.
        /// </summary>
        public static bool SystemRestoringIsEnabled =>
            Reg.Read(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore", "RPSessionInterval", 0) > 0;

        /// <summary>
        ///     The type of event. For more information, see <see cref="CreateSystemRestorePoint"/>.
        /// </summary>
        public enum EventType
        {
            /// <summary>
            ///     A system change has begun. A subsequent nested call does not create a new restore
            ///     point.
            ///     <para>
            ///         Subsequent calls must use <see cref="EventType.EndNestedSystemChange"/>, not
            ///         <see cref="EventType.EndSystemChange"/>.
            ///     </para>
            /// </summary>
            BeginNestedSystemChange = 0x66,

            /// <summary>
            ///     A system change has begun.
            /// </summary>
            BeginSystemChange = 0x64,

            /// <summary>
            ///     A system change has ended.
            /// </summary>
            EndNestedSystemChange = 0x67,

            /// <summary>
            ///     A system change has ended.
            /// </summary>
            EndSystemChange = 0x65
        }

        /// <summary>
        ///     The type of restore point. For more information, see <see cref="CreateSystemRestorePoint"/>.
        /// </summary>
        public enum RestorePointType
        {
            /// <summary>
            ///     An application has been installed.
            /// </summary>
            ApplicationInstall = 0x0,

            /// <summary>
            ///     An application has been uninstalled.
            /// </summary>
            ApplicationUninstall = 0x1,

            /// <summary>
            ///     An application needs to delete the restore point it created. For example, an
            ///     application would use this flag when a user cancels an installation.
            /// </summary>
            CancelledOperation = 0xd,

            /// <summary>
            ///     A device driver has been installed.
            /// </summary>
            DeviceDriverInstall = 0xa,

            /// <summary>
            ///     An application has had features added or removed.
            /// </summary>
            ModifySettings = 0xc
        }

        /// <summary>
        ///     Creates a restore point on the local system.
        /// </summary>
        /// <param name="description">
        ///     The description to be displayed so the user can easily identify a restore point.
        /// </param>
        /// <param name="eventType">
        ///     The type of event.
        /// </param>
        /// <param name="restorePointType">
        ///     The type of restore point.
        /// </param>
        public static void CreateSystemRestorePoint(string description, EventType eventType, RestorePointType restorePointType)
        {
            try
            {
                if (!SystemRestoringIsEnabled)
                    throw new WarningException("System restoring is disabled.");
                var mScope = new ManagementScope("\\\\localhost\\root\\default");
                var mPath = new ManagementPath("SystemRestore");
                var options = new ObjectGetOptions();
                using (var mClass = new ManagementClass(mScope, mPath, options))
                    using (var parameters = mClass.GetMethodParameters("CreateRestorePoint"))
                    {
                        parameters["Description"] = description;
                        parameters["EventType"] = (int)eventType;
                        parameters["RestorePointType"] = (int)restorePointType;
                        mClass.InvokeMethod("CreateRestorePoint", parameters, null);
                    }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }
}
