#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: EnvironmentEx.cs
// Version:  2016-10-28 08:27
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Provides static methods based on the <see cref="Environment"/> class to provide informations
    ///     about the current environment.
    /// </summary>
    public static class EnvironmentEx
    {
        private static List<string> _cmdLineArgs = new List<string>();
        private static bool _cmdLineArgsQuotes = true;
        private static string _commandLine = string.Empty;

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
            if (_cmdLineArgs.Count == Environment.GetCommandLineArgs().Length - skip && quotes == _cmdLineArgsQuotes)
                return _cmdLineArgs;
            _cmdLineArgsQuotes = quotes;
            var filteredArgs = new List<string>();
            try
            {
                if (Environment.GetCommandLineArgs().Length > skip)
                {
                    var defaultArgs = Environment.GetCommandLineArgs().Skip(skip).ToList();
                    if (sort)
                        defaultArgs = defaultArgs.OrderBy(x => x, new Comparison.AlphanumericComparer()).ToList();
                    var debugArg = false;
                    foreach (var arg in defaultArgs)
                    {
                        if (arg.StartsWithEx("/debug") || debugArg)
                        {
                            debugArg = !debugArg;
                            continue;
                        }
                        filteredArgs.Add(quotes && arg.Any(char.IsWhiteSpace) ? $"\"{arg}\"" : arg);
                    }
                    _cmdLineArgs = filteredArgs;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
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
            return _commandLine;
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
        ///         Allows <see cref="Environment.SpecialFolder"/> names inlcuding an
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
            var value = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(variable))
                    throw new ArgumentNullException(nameof(variable));
                variable = variable.RemoveChar('%');
                if (variable.EqualsEx("currentdir", "curdir"))
                    value = PathEx.LocalDir;
                else
                    try
                    {
                        var match = Enum.GetNames(typeof(Environment.SpecialFolder))
                                        .First(s => variable.EqualsEx(s));
                        Environment.SpecialFolder specialFolder;
                        if (!Enum.TryParse(match, out specialFolder))
                            throw new ArgumentException();
                        value = Environment.GetFolderPath(specialFolder);
                    }
                    catch
                    {
                        value = Environment.GetEnvironmentVariables().Cast<DictionaryEntry>()
                                           .First(x => variable.EqualsEx(x.Key.ToString())).Value.ToString();
                    }
                if (lower)
                    value = value?.ToLower();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return value;
        }

        /// <summary>
        ///     Converts the specified path to an existing environment variable, if possible.
        /// </summary>
        /// <param name="path">
        ///     The path to convert.
        /// </param>
        public static string GetVariablePath(string path)
        {
            var variable = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                if (path.EqualsEx(GetVariableValue("curdir")))
                    variable = "CurDir";
                else
                    try
                    {
                        variable = Enum.GetValues(typeof(Environment.SpecialFolder))
                                       .Cast<Environment.SpecialFolder>()
                                       .First(s => path.EqualsEx(Environment.GetFolderPath(s)))
                                       .ToString();
                    }
                    catch
                    {
                        variable = Environment.GetEnvironmentVariables()
                                              .Cast<DictionaryEntry>()
                                              .First(x => path.EqualsEx(x.Value.ToString()))
                                              .Key.ToString();
                    }
                if (!string.IsNullOrWhiteSpace(variable))
                    variable = $"%{variable}%";
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return variable;
        }
    }
}
