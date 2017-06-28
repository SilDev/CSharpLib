#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Log.cs
// Version:  2017-06-27 16:30
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    ///     Provides functionality for the catching and logging of handled or unhandled
    ///     <see cref="Exception"/>'s.
    /// </summary>
    public static class Log
    {
        internal static string DebugKey = "Debug";
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss,fff zzz";
        private static bool _conIsOpen, _firstCall, _firstEntry = true;
        private static FileStream _fs;
        private static SafeFileHandle _sfh;
        private static IntPtr _stdHandle = IntPtr.Zero;
        private static StreamWriter _sw;
        private static readonly AssemblyName AssemblyEntryName = Assembly.GetEntryAssembly().GetName();
        private static readonly string AssemblyName = AssemblyEntryName.Name;
        private static readonly Version AssemblyVersion = AssemblyEntryName.Version;
        private static readonly string RuntimeSeparator = new string('-', 120);
        private static readonly object WriteLocker = new object();

        /// <summary>
        ///     true to enable the catching of unhandled <see cref="Exception"/>'s; otherwise, false.
        /// </summary>
        public static bool CatchUnhandledExceptions { get; set; } = true;

        /// <summary>
        ///     Gets or sets the culture for the current thread.
        /// </summary>
        public static CultureInfo CurrentCulture { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        ///     Gets the current <see cref="DebugMode"/> value that determines how <see cref="Exception"/>'s
        ///     are handled. For more informations see <see cref="ActivateLogging(int)"/>.
        /// </summary>
        public static int DebugMode { get; private set; }

        /// <summary>
        ///     Gets the name of the current LOG file.
        /// </summary>
        public static string FileName => $"{AssemblyName}_{DateTime.Now:yyyy-MM-dd}.log";

        /// <summary>
        ///     <para>
        ///         Gets or sets the location of the current LOG file.
        ///     </para>
        ///     <para>
        ///         If the specified path doesn't exists, it is created.
        ///     </para>
        ///     <para>
        ///         If the specified path is invalid or this process doesn't have the necessary permissions
        ///         to write to this location, the location is changed to the Windows specified folder for
        ///         temporary files.
        ///     </para>
        /// </summary>
        public static string FileDir { get; set; } = Path.GetTempPath();

        /// <summary>
        ///     Gets the full path of the current log file.
        /// </summary>
        public static string FilePath { get; private set; } = PathEx.Combine(FileDir, FileName);

        /// <summary>
        ///     <para>
        ///         Specifies the <see cref="DebugMode"/> for the handling of <see cref="Exception"/>'s.
        ///         The <see cref="DebugMode"/> can also specified over an command line argument or an
        ///         config parameter in combination with <see cref="AllowLogging(string, string, string)"/>.
        ///         The following <see cref="DebugMode"/> options are available.
        ///     </para>
        ///     <para>
        ///         0: Logging is disabled. If <see cref="CatchUnhandledExceptions"/> is enabled, unhandled
        ///         <see cref="Exception"/>'s are discarded as well. This can be useful for public releases
        ///         to prevent any kind of <see cref="Exception"/> notifications to the client. Please note
        ///         that these functions may have dangerous consequences if used incorrectly.
        ///     </para>
        ///     <para>
        ///         1: Logging is enabled and all <see cref="Exception"/>'s are logged.
        ///     </para>
        ///     <para>
        ///         2: Logging is enabled, all <see cref="Exception"/>'s are logged, and a new
        ///         <see cref="Console"/> window is allocated for the current process to display the current
        ///         log in real time.
        ///     </para>
        /// </summary>
        public static void ActivateLogging(int mode = 1)
        {
            DebugMode = mode;
            if (_firstCall)
                return;
            _firstCall = true;
            if (CatchUnhandledExceptions)
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += (s, e) => Write(e.Exception, true, true);
                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                    WriteUnhandled
                    (
                        new ApplicationException
                        (
                            string.Concat
                            (
                                "Error in the application. Sender object: '",
                                s,
                                "'; Exception object: '",
                                e.ExceptionObject,
                                "';"
                            )
                        )
                    );
                AppDomain.CurrentDomain.ProcessExit += (s, e) => Close();
            }
            if (DebugMode < 1)
                return;
            Thread.CurrentThread.CurrentCulture = CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = CurrentCulture;
            try
            {
                FileDir = Path.GetFullPath(FileDir);
                if (!Directory.Exists(FileDir))
                    Directory.CreateDirectory(FileDir);
            }
            catch
            {
                FileDir = Path.GetTempPath();
            }
            finally
            {
                FilePath = Path.Combine(FileDir, FileName);
            }
            if (DebugMode < 2)
                return;
            Write(string.Empty);
        }

        /// <summary>
        ///     Allows you to enable logging by command line arguments or a specified configuration file. For
        ///     more informations see <see cref="ActivateLogging(int)"/>.
        /// </summary>
        /// <param name="configPath">
        ///     The full path of the configuration file.
        /// </param>
        /// <param name="key">
        ///     The key used to specify the <see cref="DebugMode"/>.
        /// </param>
        /// <param name="pattern">
        ///     <para>
        ///         The regular expression pattern to match.
        ///     </para>
        ///     <para>
        ///         Please note that the default pattern is optimized to search within INI formatted files. The
        ///         &lt;Key&gt; and &lt;Value&gt; tags are required in all search pattern.
        ///     </para>
        /// </param>
        public static void AllowLogging(string configPath = null, string key = "Debug", string pattern = @"^((?:\[)(?<Section>[^\]]*)(?:\])(?:[\r\n]{0,}|\Z))((?!\[)(?<Key>[^=]*?)(?:=)(?<Value>[^\r\n]*)(?:[\r\n]{0,4}))+")
        {
            var args = Environment.GetCommandLineArgs();
            if (!DebugKey.EqualsEx(key))
                DebugKey = key;
            var arg = string.Concat('/', key);
            var mode = 0;
            if (args.ContainsEx(arg))
            {
                string option;
                try
                {
                    args = args.SkipWhile(x => !x.EqualsEx(arg)).ToArray();
                    if (args.Length <= 1)
                    {
                        mode = 1;
                        goto Finalize;
                    }
                    option = args.Skip(1).FirstOrDefault();
                }
                catch
                {
                    goto Finalize;
                }
                if (!int.TryParse(option, out int i))
                    mode = 1;
                if (i > mode)
                    mode = i;
                if (mode > 0)
                    goto Finalize;
            }
            if (string.IsNullOrEmpty(configPath) || string.IsNullOrEmpty(pattern))
                goto Finalize;
            var path = PathEx.Combine(configPath);
            if (!File.Exists(path))
                goto Finalize;
            try
            {
                var source = File.ReadAllText(path);
                var matches = Regex.Matches(source, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                foreach (Match match in matches)
                    for (var i = 0; i < match.Groups["Key"].Captures.Count; i++)
                    {
                        var mKey = match.Groups["Key"]?.Captures[i].Value.Trim();
                        if (string.IsNullOrEmpty(mKey) || !mKey.EqualsEx(key))
                            continue;
                        var mVal = match.Groups["Value"]?.Captures[i].Value.Trim();
                        if (string.IsNullOrEmpty(mVal))
                            continue;
                        if (!int.TryParse(mVal, out int num))
                            goto Finalize;
                        mode = num;
                        goto Finalize;
                    }
            }
            catch
            {
                // ignored
            }
            Finalize:
            ActivateLogging(mode);
        }

        /// <summary>
        ///     Writes the specified information into a log file.
        /// </summary>
        /// <param name="logMessage">
        ///     The message text to write.
        /// </param>
        /// <param name="exitProcess">
        ///     true to terminate this process after logging; otherwise, false.
        /// </param>
        public static void Write(string logMessage, bool exitProcess = false)
        {
            lock (WriteLocker)
            {
                if (!_firstCall || DebugMode < 1 || !_firstEntry && string.IsNullOrEmpty(logMessage))
                    return;
                var log = string.Empty;
                if (!_firstEntry)
                    log = string.Concat
                    (
                        DateTime.Now.ToString(DateTimeFormat),
                        Environment.NewLine
                    );
                else
                {
                    _firstEntry = false;
                    if (File.Exists(FilePath))
                        log = string.Concat
                        (
                            Environment.NewLine,
                            RuntimeSeparator,
                            Environment.NewLine,
                            Environment.NewLine
                        );
                    log = string.Concat
                    (
                        log,
                        DateTime.Now.ToString(DateTimeFormat),
                        Environment.NewLine,
                        "System: '",
                        Environment.OSVersion,
                        "'; Runtime: '",
                        EnvironmentEx.Version,
                        "'; Assembly: '",
                        AssemblyName,
                        "'; Version: '",
                        AssemblyVersion,
                        "';",
                        Environment.NewLine,
                        Environment.NewLine
                    );
                    if (!string.IsNullOrEmpty(logMessage))
                        log += string.Concat
                        (
                            DateTime.Now.ToString(DateTimeFormat),
                            Environment.NewLine
                        );
                }
                if (!string.IsNullOrEmpty(logMessage))
                    log = string.Concat(log, logMessage, Environment.NewLine, Environment.NewLine);
                File.AppendAllText(FilePath, log);
                if (DebugMode < 2)
                {
                    if (!exitProcess)
                        return;
                    Environment.ExitCode = 1;
                    Environment.Exit(Environment.ExitCode);
                }
                if (!_conIsOpen)
                {
                    _conIsOpen = true;
                    WinApi.NativeMethods.AllocConsole();
                    var hWnd = WinApi.NativeMethods.GetConsoleWindow();
                    if (hWnd != IntPtr.Zero)
                    {
                        var hMenu = WinApi.NativeMethods.GetSystemMenu(hWnd, false);
                        if (hMenu != IntPtr.Zero)
                            WinApi.NativeMethods.DeleteMenu(hMenu, 0xf060, 0x0);
                    }
                    _stdHandle = WinApi.NativeMethods.GetStdHandle(-11);
                    _sfh = new SafeFileHandle(_stdHandle, true);
                    _fs = new FileStream(_sfh, FileAccess.Write);
                    var title = $"Debug Console ('{AssemblyName}')";
                    if (Console.Title != title)
                    {
                        Console.Title = title;
                        Console.BufferHeight = short.MaxValue - 1;
                        Console.BufferWidth = Console.WindowWidth;
                        Console.SetWindowSize(Math.Min(100, Console.LargestWindowWidth), Math.Min(40, Console.LargestWindowHeight));
                    }
                }
                if (log.Contains(RuntimeSeparator))
                    log = log.TrimStart().Substring(RuntimeSeparator.Length).TrimStart();
                Console.Write(log);
                _sw = new StreamWriter(_fs, Encoding.ASCII) { AutoFlush = true };
                Console.SetOut(_sw);
                if (!exitProcess)
                    return;
                Environment.ExitCode = 1;
                Environment.Exit(Environment.ExitCode);
            }
        }

        /// <summary>
        ///     Writes all <see cref="Exception"/> information into a log file.
        /// </summary>
        /// <param name="exception">
        ///     The handled <see cref="Exception"/> to write.
        /// </param>
        /// <param name="forceLogging">
        ///     true to enforce that <see cref="DebugMode"/> is enabled; otherwise, false.
        /// </param>
        /// <param name="exitProcess">
        ///     true to terminate this process after logging; otherwise, false.
        /// </param>
        public static void Write(Exception exception, bool forceLogging = false, bool exitProcess = false)
        {
            if (DebugMode < 1)
            {
                if (!forceLogging)
                    return;
                DebugMode = 1;
            }
            if (DebugMode < 2 && (exception is ArgumentNullException || exception is WarningException))
                return;
            Write($"Handled {exception}", exitProcess);
        }

        private static void WriteUnhandled(Exception exception)
        {
            DebugMode = 1;
            Write($"Unhandled {exception}", true);
        }

        private static void Close()
        {
            if (_sfh?.IsClosed == false)
                _sfh.Close();
            if (!Directory.Exists(FileDir))
                return;
            try
            {
                foreach (var file in Directory.EnumerateFiles(FileDir, AssemblyName + "*.log", SearchOption.TopDirectoryOnly))
                {
                    if (FilePath.EqualsEx(file))
                        continue;
                    if ((DateTime.Now - File.GetLastWriteTime(file)).TotalDays >= 7d)
                        File.Delete(file);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
