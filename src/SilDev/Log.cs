#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Log.cs
// Version:  2017-05-13 04:35
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
        private static volatile bool _conIsOpen, _firstCall, _firstEntry;
        private static volatile IntPtr _stdHandle = IntPtr.Zero;
        private static volatile SafeFileHandle _sfh;
        private static volatile FileStream _fs;
        private static volatile StreamWriter _sw;
        private static readonly AssemblyName AssemblyEntryName = Assembly.GetEntryAssembly().GetName();
        private static readonly string AssemblyName = AssemblyEntryName.Name;
        private static readonly Version AssemblyVersion = AssemblyEntryName.Version;
        private static readonly string ConsoleTitle = $"Debug Console ('{AssemblyName}')";

        /// <summary>
        ///     true to enable the catching of unhandled <see cref="Exception"/>'s; otherwise, false.
        /// </summary>
        public static bool CatchUnhandledExceptions { get; set; } = true;

        /// <summary>
        ///     Gets or sets the culture for the current thread.
        /// </summary>
        public static CultureInfo CurrentCulture { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        ///     Gets the current <see cref="DebugMode"/> option how <see cref="Exception"/>'s are handled. For
        ///     more informations see <see cref="ActivateLogging(int)"/>.
        /// </summary>
        public static int DebugMode { get; private set; }

        /// <summary>
        ///     Gets the name for the current log file.
        /// </summary>
        public static string FileName => $"{AssemblyName}_{DateTime.Now:yyyy-MM-dd}.log";

        /// <summary>
        ///     <para>
        ///         Gets or sets the location of the current log file.
        ///     </para>
        ///     <para>
        ///         If the specified path doesn't exist, it is created.
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
        public static string FilePath { get; private set; } = Path.Combine(FileDir, FileName);

        /// <summary>
        ///     <para>
        ///         Specifies the <see cref="DebugMode"/> for the handling of <see cref="Exception"/>'s.
        ///         The <see cref="DebugMode"/> can also specified over an command line argument or an
        ///         config parameter in combination with <see cref="AllowLogging(string, string, string)"/>.
        ///         The following <see cref="DebugMode"/> options are available.
        ///     </para>
        ///     <para>
        ///         0: Logging is disabled. If <see cref="CatchUnhandledExceptions"/> is enabled, unhandled
        ///         <see cref="Exception"/>'s are discarded. This can be useful for public releases to
        ///         prevent any kind of <see cref="Exception"/> notifications to the client. Please note
        ///         that this functions may have dangerous consequences if it is used incorrectly.
        ///     </para>
        ///     <para>
        ///         1: Logging is enabled and all <see cref="Exception"/>'s are logged.
        ///     </para>
        ///     <para>
        ///         2: Logging is enabled, all <see cref="Exception"/>'s are logged, and a new
        ///         <see cref="Console"/> window is allocated for the current process to display the current
        ///         log in real time. Please note that the console is first allocated after the first logging.
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
                AppDomain.CurrentDomain.UnhandledException += (s, e) => WriteUnhandled(
                    new ApplicationException("Error in the application. Sender object: '" + s + "'; Exception object: '" + e.ExceptionObject + "'"));
                AppDomain.CurrentDomain.ProcessExit += (s, e) => Close();
            }
            if (DebugMode <= 0)
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
        }

        /// <summary>
        ///     <para>
        ///         Checks the command line argument for a valid command, "/debug 2" - for example, or checks the
        ///         content of the specified configuration file to specify the current <see cref="DebugMode"/>.
        ///         For more informations see <see cref="ActivateLogging(int)"/>.
        ///     </para>
        /// </summary>
        /// <param name="configPath">
        ///     The full path of the configuration file.
        /// </param>
        /// <param name="pattern">
        ///     The regular expression pattern to match.
        /// </param>
        /// <param name="key">
        ///     The key of the configuration file which hold the value, to specify the current
        ///     <see cref="DebugMode"/>.
        /// </param>
        public static void AllowLogging(string configPath = null, string pattern = null, string key = nameof(DebugMode))
        {
            var mode = 0;
            var regex = new Regex("/debug [0-2]", RegexOptions.IgnoreCase);
            var cmdLine = Environment.CommandLine.RemoveChar('\"');
            if (regex.IsMatch(cmdLine))
            {
                int i;
                if (int.TryParse(regex.Match(cmdLine).Groups[1].ToString(), out i) && i > mode)
                    mode = i;
                if (mode > 0)
                    goto ACTIVATE;
            }
            var path = PathEx.Combine(configPath);
            if (!string.IsNullOrEmpty(pattern) && !string.IsNullOrEmpty(path) && File.Exists(path))
                try
                {
                    var lines = File.ReadAllLines(path);
                    foreach (var line in lines)
                    {
                        var match = Regex.Match(line, pattern, RegexOptions.IgnoreCase).Groups;
                        if (match.Count < 3)
                            continue;
                        if (!match[1].Value.EqualsEx(key))
                            continue;
                        int i;
                        if (!int.TryParse(match[2].Value, out i))
                            continue;
                        mode = i;
                        break;
                    }
                }
                catch
                {
                    // ignored
                }
            ACTIVATE:
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
            if (!_firstCall || DebugMode < 1 || string.IsNullOrEmpty(logMessage))
                return;
            var dat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff zzz") + Environment.NewLine;
            var log = dat;
            if (!_firstEntry)
            {
                _firstEntry = true;
                if (File.Exists(FilePath))
                    log = new string('-', 120) + Environment.NewLine + Environment.NewLine + dat;
                log += "***Logging has been started***" + Environment.NewLine;
                log += "   '" + Environment.OSVersion + "'; '" + AssemblyName + "'; '" + AssemblyVersion + "'; '" + FilePath + "'" + Environment.NewLine + Environment.NewLine;
                File.AppendAllText(FilePath, log);
                log = dat;
            }
            log += logMessage + Environment.NewLine + Environment.NewLine;
            File.AppendAllText(FilePath, log);
            if (DebugMode <= 1)
            {
                if (!exitProcess)
                    return;
                Environment.ExitCode = 1;
                Environment.Exit(Environment.ExitCode);
            }
            if (!_conIsOpen)
            {
                _conIsOpen = true;
                WinApi.SafeNativeMethods.AllocConsole();
                var hWnd = WinApi.SafeNativeMethods.GetConsoleWindow();
                if (hWnd != IntPtr.Zero)
                {
                    var hMenu = WinApi.UnsafeNativeMethods.GetSystemMenu(hWnd, false);
                    if (hMenu != IntPtr.Zero)
                        WinApi.UnsafeNativeMethods.DeleteMenu(hMenu, 0xf060, 0x0);
                }
                _stdHandle = WinApi.UnsafeNativeMethods.GetStdHandle(-11);
                _sfh = new SafeFileHandle(_stdHandle, true);
                _fs = new FileStream(_sfh, FileAccess.Write);
                if (Console.Title != ConsoleTitle)
                {
                    Console.Title = ConsoleTitle;
                    Console.BufferHeight = short.MaxValue - 1;
                    Console.BufferWidth = Console.WindowWidth;
                    Console.SetWindowSize(Math.Min(100, Console.LargestWindowWidth), Math.Min(40, Console.LargestWindowHeight));
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(log);
            Console.ResetColor();
            _sw = new StreamWriter(_fs, Encoding.ASCII) { AutoFlush = true };
            Console.SetOut(_sw);
            if (!exitProcess)
                return;
            Environment.ExitCode = 1;
            Environment.Exit(Environment.ExitCode);
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
            Write("Handled " + exception, exitProcess);
        }

        private static void WriteUnhandled(Exception exception)
        {
            DebugMode = 1;
            Write("Unhandled " + exception, true);
        }

        private static void Close()
        {
            if (_sfh != null && !_sfh.IsClosed)
                _sfh.Close();
            if (!Directory.Exists(FileDir))
                return;
            try
            {
                foreach (var file in Directory.GetFiles(FileDir, $"{AssemblyName}*.log", SearchOption.TopDirectoryOnly))
                {
                    if (FilePath.EqualsEx(file))
                        continue;
                    if ((DateTime.Now - new FileInfo(file).LastWriteTime).TotalDays >= 7d)
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
