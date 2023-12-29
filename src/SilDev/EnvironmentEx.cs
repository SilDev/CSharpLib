#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: EnvironmentEx.cs
// Version:  2023-12-29 15:00
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Investment;
    using Microsoft.Win32;
    using QuickWmi;

    /// <summary>
    ///     Provides static methods based on the <see cref="Environment"/> class to
    ///     provide information about the current environment.
    /// </summary>
    public static class EnvironmentEx
    {
        private static List<string> _cmdLineArgs;
        private static bool _cmdLineArgsQuotes;
        private static string _commandLine;
        private static int? _machineId;
        private static Version _osversion, _version;

        /// <summary>
        ///     Gets a unique system identification number.
        /// </summary>
        public static int MachineId
        {
            get
            {
                if (_machineId.HasValue)
                    return (int)_machineId;
                var id = Win32_OperatingSystem.SerialNumber;
                if (string.IsNullOrWhiteSpace(id))
                    id = string.Concat(Environment.MachineName, Path.DirectorySeparatorChar, Environment.UserName);
                _machineId = Math.Abs(id.GetHashCode());
                return (int)_machineId;
            }
        }

        /// <summary>
        ///     Gets a <see cref="Environment.OSVersion"/> version object with support for
        ///     Windows 11 and later.
        /// </summary>
        public static Version OperatingSystemVersion
        {
            get
            {
                if (_osversion != default)
                    return _osversion;
                _osversion = Environment.OSVersion.Version;
                if (_osversion.Major != 10 || _osversion.Build < 22000)
                    return _osversion;
                var caption = Win32_OperatingSystem.Caption;
                var first = caption.FirstOrDefault(char.IsNumber);
                var second = caption.SecondOrDefault(char.IsNumber);
                var third = caption.ThirdOrDefault(char.IsNumber);
                var major = int.Parse($"{first}{second}");
                var minor = third != default ? int.Parse($"{third}") : _osversion.Minor;
                return _osversion = new Version(major, minor, _osversion.Build, _osversion.Revision);
            }
        }

        /// <summary>
        ///     Gets a <see cref="System.Version"/> object that describes the exact major,
        ///     minor, build and revision numbers of the common language runtime.
        /// </summary>
        public static Version Version
        {
            get
            {
                if (_version != default)
                    return _version;
                const string keyPath = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full";
                var version = Reg.Read<string>(Registry.LocalMachine, keyPath, "Version").ToVersion();
                if (version.Major >= 4)
                    return _version = version;
                var release = Reg.Read(Registry.LocalMachine, keyPath, "Release", 0);
                return release switch
                {
                    >= 533320 => _version = new Version(4, 8, 1),
                    >= 528040 => _version = new Version(4, 8),
                    >= 461808 => _version = new Version(4, 7, 2),
                    >= 461308 => _version = new Version(4, 7, 1),
                    >= 460798 => _version = new Version(4, 7),
                    >= 394802 => _version = new Version(4, 6, 2),
                    >= 394254 => _version = new Version(4, 6, 1),
                    >= 393295 => _version = new Version(4, 6),
                    >= 379893 => _version = new Version(4, 5, 2),
                    >= 378675 => _version = new Version(4, 5, 1),
                    >= 378389 => _version = new Version(4, 5),
                    _ => _version = Environment.Version
                };
            }
        }

        /// <summary>
        ///     Determines whether the current Windows version corresponds to the specified
        ///     values or is newer.
        /// </summary>
        /// <param name="major">
        ///     The minimum major version number.
        /// </param>
        /// <param name="build">
        ///     The minimum build version number.
        /// </param>
        public static bool IsAtLeastWindows(int major, int build = default)
        {
            if (major == 11)
            {
                major = 10;
                if (build < 22000)
                    build = 22000;
            }
            var osVersion = Environment.OSVersion.Version;
            return osVersion.Major >= major && osVersion.Build >= build;
        }

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string
        ///     <see cref="List{T}"/> containing the command-line arguments for the current
        ///     process.
        /// </summary>
        /// <param name="sort">
        ///     <see langword="true"/> to sort the arguments ascended with the rules of
        ///     <see cref="AlphaNumericComparer(bool)"/> before returning the arguments;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="skip">
        ///     The number of arguments to skip before returning the remaining arguments.
        /// </param>
        /// <param name="quotes">
        ///     <see langword="true"/> to store the arguments in quotation marks which
        ///     containing spaces; otherwise, <see langword="false"/>.
        /// </param>
        public static IList<string> CommandLineArgs(bool sort = true, int skip = 1, bool quotes = true)
        {
            if (_cmdLineArgs?.Count == Environment.GetCommandLineArgs().Length - skip && quotes == _cmdLineArgsQuotes)
                return _cmdLineArgs;
            _cmdLineArgs = new List<string>();
            _cmdLineArgsQuotes = quotes;
            if (Environment.GetCommandLineArgs().Length <= skip)
                return _cmdLineArgs;
            string arg = null;
            var defaultArgs = Environment.GetCommandLineArgs().ToList();
            if (defaultArgs.Any(x => (arg = x).EqualsEx($"/{Log.DebugKey}")))
            {
                var index = defaultArgs.IndexOf(arg);
                defaultArgs.RemoveAt(index);
                if (defaultArgs.Count > index)
                    defaultArgs.RemoveAt(index);
            }
            if (defaultArgs.Count > skip)
                defaultArgs = defaultArgs.Skip(skip).ToList();
            if (sort)
                defaultArgs = defaultArgs.OrderBy(x => x, CacheInvestor.GetDefault<AlphaNumericComparer<string>>()).ToList();
            return _cmdLineArgs = quotes ? defaultArgs.Select(x => x.Any(char.IsWhiteSpace) ? $"\"{x}\"" : x).ToList() : defaultArgs;
        }

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string
        ///     <see cref="List{T}"/> containing the command-line arguments for the current
        ///     process.
        /// </summary>
        /// <param name="sort">
        ///     <see langword="true"/> to sort the arguments ascended with the rules of
        ///     <see cref="AlphaNumericComparer(bool)"/> before returning the arguments;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="quotes">
        ///     <see langword="true"/> to store the arguments in quotation marks which
        ///     containing spaces; otherwise, <see langword="false"/>.
        /// </param>
        public static IList<string> CommandLineArgs(bool sort, bool quotes) =>
            CommandLineArgs(sort, 1, quotes);

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string
        ///     <see cref="List{T}"/> containing the command-line arguments for the current
        ///     process.
        /// </summary>
        /// <param name="skip">
        ///     The number of arguments to skip before returning the remaining arguments.
        /// </param>
        public static IList<string> CommandLineArgs(int skip) =>
            CommandLineArgs(true, skip);

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string containing the
        ///     command-line arguments for the current process.
        /// </summary>
        /// <param name="sort">
        ///     <see langword="true"/> to sort the arguments ascended with the rules of
        ///     <see cref="AlphaNumericComparer(bool)"/> before returning the arguments;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="skip">
        ///     The number of arguments to skip before returning the remaining arguments.
        /// </param>
        /// <param name="quotes">
        ///     <see langword="true"/> to store the arguments in quotation marks which
        ///     containing spaces; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to sort the arguments ascended with the rules of
        ///     <see cref="AlphaNumericComparer(bool)"/> before returning the arguments;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="quotes">
        ///     <see langword="true"/> to store the arguments in quotation marks which
        ///     containing spaces; otherwise, <see langword="false"/>.
        /// </param>
        public static string CommandLine(bool sort, bool quotes) =>
            CommandLine(sort, 1, quotes);

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
        ///     Separates the specified variable if it contains a colon before the last
        ///     percent sign, followed by a number, sometimes with the Alt keyword before
        ///     the number.
        ///     <para>
        ///         These filters are typically used to multiply
        ///         <see cref="Path.DirectorySeparatorChar"/> characters in paths. In the
        ///         case of the Alt keyword, the
        ///         <see cref="Path.AltDirectorySeparatorChar"/> character is used.
        ///     </para>
        /// </summary>
        /// <param name="variable">
        ///     The name of the environment variable with or without filter.
        /// </param>
        /// <param name="key">
        ///     The keyword that specifies the directory separator.
        /// </param>
        /// <param name="num">
        ///     The number indicating the directory separator multiplier.
        /// </param>
        public static void VariableFilter(ref string variable, out string key, out byte num)
        {
            key = null;
            num = 1;
            if (variable == null || !variable.Contains(':') || variable.ContainsEx(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar))
                return;
            var sa = variable.Split(':');
            var s = sa.FirstOrDefault();
            if (string.IsNullOrEmpty(s))
                return;
            variable = s;
            if (s.StartsWith("%", StringComparison.Ordinal))
                variable = $"{variable}%";
            s = sa.LastOrDefault()?.Trim('%');
            if (string.IsNullOrEmpty(s))
                return;
            if (s.All(char.IsDigit) && byte.TryParse(s, out num))
            {
                if (num < 1)
                    num = 1;
                return;
            }
            if (s.Length < 3)
                return;
            key = s.Substring(0, 3);
            if (s.Length < 4)
                return;
            s = s.RemoveChar(',').Substring(3);
            if (!byte.TryParse(s, out num))
                num = 1;
        }

        /// <summary>
        ///     Retrieves the value of an environment variable from the current process.
        ///     <para>
        ///         Allows <see cref="Environment.SpecialFolder"/> names to get its
        ///         <see cref="Environment.GetFolderPath(Environment.SpecialFolder)"/>
        ///         value and also supports the keyword "CurDir" to get the
        ///         <see cref="PathEx.LocalDir"/> value.
        ///     </para>
        ///     <para>
        ///         This function should not be confused with
        ///         <see cref="GetVariableWithPath(string, bool, bool)"/>.
        ///     </para>
        /// </summary>
        /// <param name="variable">
        ///     The name of the environment variable.
        /// </param>
        /// <param name="lower">
        ///     <see langword="true"/> to convert the result to lowercase; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="noSubPath">
        ///     <see langword="true"/> to get only the value of the environment variable,
        ///     even if the variable is an entire path; otherwise, <see langword="false"/>
        ///     to retrieve the entire path.
        /// </param>
        public static string GetVariableValue(string variable, bool lower = false, bool noSubPath = true)
        {
            var output = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(variable))
                    throw new ArgumentNullException(nameof(variable));
                var subPath = string.Empty;
                if (variable.ContainsEx(PathEx.PathSeparatorChars))
                {
                    var plains = variable.Split(PathEx.PathSeparatorChars, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    variable = plains[0];
                    if (!noSubPath && plains.Length > 1)
                        subPath = $"{Path.DirectorySeparatorChar}{plains.Skip(1).Join(Path.DirectorySeparatorChar)}";
                }
                variable = variable.RemoveChar('%');
                VariableFilter(ref variable, out var key, out var num);
                if (variable.EqualsEx("CurDir", "CurrentDir"))
                    output = PathEx.LocalDir;
                else
                {
                    var type = typeof(Environment.SpecialFolder);
                    var match = Enum.GetNames(type).FirstOrDefault(s => s.EqualsEx(variable));
                    if (string.IsNullOrEmpty(match))
                    {
                        var table = (Hashtable)Environment.GetEnvironmentVariables();
                        if (table.Keys.Cast<string>().FirstOrDefault(k => k.EqualsEx(variable)) is { } varKey)
                            output = (string)table[varKey];
                    }
                    else
                    {
                        var folder = (Environment.SpecialFolder)Enum.Parse(type, match);
                        output = Environment.GetFolderPath(folder);
                    }
                }
                if (!noSubPath)
                    output += subPath;
                if (lower)
                    output = output?.ToLowerInvariant();
                if (!string.IsNullOrEmpty(output) && (!string.IsNullOrEmpty(key) || num > 1))
                    if (string.IsNullOrEmpty(key))
                        output = output.Replace(PathEx.DirectorySeparatorStr, new string(Path.DirectorySeparatorChar, num));
                    else if (key.EqualsEx("Alt"))
                        output = output.Replace(PathEx.DirectorySeparatorStr, new string(Path.AltDirectorySeparatorChar, num));
            }
            catch (InvalidOperationException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return output;
        }

        /// <summary>
        ///     Returns an environment variable if the specified path contains an
        ///     environment variable or if elements of the specified path match a value of
        ///     an environment variable.
        /// </summary>
        /// <param name="path">
        ///     The path to check.
        /// </param>
        /// <param name="curDir">
        ///     <see langword="true"/> to consider the <see cref="PathEx.LocalDir"/> value;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="special">
        ///     <see langword="true"/> to consider the
        ///     <see cref="Environment.SpecialFolder"/> values; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static string GetVariableFromPath(string path, bool curDir = true, bool special = true) =>
            GetVariableFromPathIntern(path, curDir, special, out _);

        /// <summary>
        ///     Converts the beginning of the specified path to an environment variable.
        ///     <para>
        ///         &#9888; Please note that
        ///         <see cref="VariableFilter(ref string, out string, out byte)"/> has no
        ///         effect here.
        ///     </para>
        /// </summary>
        /// <param name="path">
        ///     The path to convert.
        /// </param>
        /// <param name="curDir">
        ///     <see langword="true"/> to consider the <see cref="PathEx.LocalDir"/> value;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="special">
        ///     <see langword="true"/> to consider the
        ///     <see cref="Environment.SpecialFolder"/> values; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static string GetVariableWithPath(string path, bool curDir = true, bool special = true)
        {
            if (path.StartsWith("%", StringComparison.Ordinal))
            {
                path = GetVariableValue(path, false, false);
                path = PathEx.Combine(path);
            }
            var variable = GetVariableFromPathIntern(path, curDir, special, out var length);
            if (string.IsNullOrEmpty(variable) || length < 1)
                return path;
            return $"%{variable}%{path.Substring(length)}";
        }

        private static string GetVariableFromPathIntern(string path, bool curDir, bool special, out int length)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                path = PathEx.Combine(path);
                if (!PathEx.IsValidPath(path))
                    throw new ArgumentInvalidException(nameof(path));
                if (curDir)
                {
                    var localDir = PathEx.LocalDir;
                    if (path.StartsWithEx(localDir))
                    {
                        length = localDir.Length;
                        return "CurDir";
                    }
                }
                var table = (Hashtable)Environment.GetEnvironmentVariables();
                foreach (var varKey in table.Keys)
                {
                    var varValue = (string)table[varKey];
                    if (varValue.Length < 3 || !PathEx.IsValidPath(varValue) || !path.StartsWithEx(varValue))
                        continue;
                    length = varValue.Length;
                    return (string)varKey;
                }
                if (special)
                {
                    var type = typeof(Environment.SpecialFolder);
                    foreach (var item in Enum.GetValues(type).Cast<Environment.SpecialFolder>())
                    {
                        var folder = Environment.GetFolderPath(item);
                        if (folder.Length < 3 || !path.StartsWithEx(folder))
                            continue;
                        var name = Enum.GetName(type, item);
                        length = folder.Length;
                        return name;
                    }
                }
                var sysDrive = Environment.GetEnvironmentVariable("SystemDrive");
                if (!string.IsNullOrEmpty(sysDrive) && path.StartsWithEx(sysDrive))
                {
                    length = sysDrive.Length;
                    return "SystemDrive";
                }
            }
            catch (InvalidOperationException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            length = 0;
            return string.Empty;
        }
    }
}
