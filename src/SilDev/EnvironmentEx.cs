#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: EnvironmentEx.cs
// Version:  2020-01-14 16:42
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
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
        private static Version _version;

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
                if (release >= 528040)
                    return _version = new Version(4, 8);
                if (release >= 461808)
                    return _version = new Version(4, 7, 2);
                if (release >= 461308)
                    return _version = new Version(4, 7, 1);
                if (release >= 460798)
                    return _version = new Version(4, 7);
                if (release >= 394802)
                    return _version = new Version(4, 6, 2);
                if (release >= 394254)
                    return _version = new Version(4, 6, 1);
                if (release >= 393295)
                    return _version = new Version(4, 6);
                if (release >= 379893)
                    return _version = new Version(4, 5, 2);
                if (release >= 378675)
                    return _version = new Version(4, 5, 1);
                if (release >= 378389)
                    return _version = new Version(4, 5);
                return _version = Environment.Version;
            }
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
                defaultArgs = defaultArgs.OrderBy(x => x, new AlphaNumericComparer()).ToList();
            _cmdLineArgs = quotes ? defaultArgs.Select(x => x.Any(char.IsWhiteSpace) ? $"\"{x}\"" : x).ToList() : defaultArgs;
            return _cmdLineArgs;
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
        public static List<string> CommandLineArgs(bool sort, bool quotes) =>
            CommandLineArgs(sort, 1, quotes);

        /// <summary>
        ///     Provides filtering and sorting options, and returns a string
        ///     <see cref="List{T}"/> containing the command-line arguments for the current
        ///     process.
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
        ///     Provides filter for special environment variables.
        /// </summary>
        /// <param name="variable">
        ///     The name of the environment variable.
        /// </param>
        /// <param name="key">
        ///     The key that specifies the directory separator.
        /// </param>
        /// <param name="num">
        ///     The number that specifies the number of directory separators.
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
        /// </summary>
        /// <param name="variable">
        ///     The name of the environment variable.
        /// </param>
        /// <param name="lower">
        ///     <see langword="true"/> to convert the result to lowercase; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static string GetVariableValue(string variable, bool lower = false)
        {
            var output = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(variable))
                    throw new ArgumentNullException(nameof(variable));
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
                        foreach (var varKey in table.Keys)
                        {
                            if (!((string)varKey).EqualsEx(variable))
                                continue;
                            output = (string)table[varKey];
                            break;
                        }
                    }
                    else
                    {
                        var folder = (Environment.SpecialFolder)Enum.Parse(type, match);
                        output = Environment.GetFolderPath(folder);
                    }
                }
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

        private static string GetVariableFromPathIntern(string path, bool curDir, bool special, out int length)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                var current = path;
                if (current.StartsWith("%", StringComparison.Ordinal) && (current.ContainsEx($"%{Path.DirectorySeparatorChar}", $"%{Path.AltDirectorySeparatorChar}") || current.EndsWith("%", StringComparison.Ordinal)))
                {
                    length = current.IndexOf('%', current.IndexOf('%') + 1);
                    length = current.IndexOf('%', ++length);
                    return current.Substring(1, --length);
                }
                if (!PathEx.IsValidPath(path))
                    throw new ArgumentInvalidException(nameof(path));
                if (curDir)
                {
                    var localDir = PathEx.LocalDir;
                    if (current.StartsWithEx(localDir))
                    {
                        length = localDir.Length;
                        return "CurDir";
                    }
                }
                var table = (Hashtable)Environment.GetEnvironmentVariables();
                foreach (var varKey in table.Keys)
                {
                    var varValue = (string)table[varKey];
                    if (varValue.Length < 3 || !PathEx.IsValidPath(varValue) || !current.StartsWithEx(varValue))
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
                        if (folder.Length < 3 || !current.StartsWithEx(folder))
                            continue;
                        var name = Enum.GetName(type, item);
                        length = folder.Length;
                        return name;
                    }
                }
                var sysDrive = Environment.GetEnvironmentVariable("SystemDrive");
                if (!string.IsNullOrEmpty(sysDrive) && current.StartsWithEx(sysDrive))
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
            var variable = GetVariableFromPathIntern(path, curDir, special, out var length);
            if (string.IsNullOrEmpty(variable) || length <= 0)
                return path;
            return $"%{variable}%{path.Substring(length)}";
        }
    }
}
