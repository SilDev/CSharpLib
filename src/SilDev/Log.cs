#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Log.cs
// Version:  2018-03-25 00:26
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
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
        internal static string DebugKey = @"Debug";
        private const string DateTimeFormat = @"yyyy-MM-dd HH:mm:ss,fff zzz";
        private static bool _conIsOpen, _fileIsValid, _firstCall, _firstEntry = true;
        private static string _fileDir, _filePath;
        private static FileStream _fs;
        private static SafeFileHandle _sfh;
        private static IntPtr _stdHandle = IntPtr.Zero;
        private static StreamWriter _sw;
        private static readonly AssemblyName AssemblyEntryName = Assembly.GetEntryAssembly().GetName();
        private static readonly string AssemblyName = AssemblyEntryName.Name;
        private static readonly Version AssemblyVersion = AssemblyEntryName.Version;
        private static readonly StringBuilder Builder = new StringBuilder();

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
        public static string FileName { get; } = $"{AssemblyName}_{DateTime.Now:yyyy-MM-dd}.log";

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
        public static string FileDir
        {
            get
            {
                if (_fileDir == default(string))
                    FileDir = Path.GetTempPath();
                return _fileDir;
            }
            set
            {
                var dir = PathEx.Combine(value);
                if (!PathEx.IsValidPath(dir) || !DirectoryEx.Create(dir))
                    dir = Path.GetTempPath();
                _fileDir = dir;
                _fileIsValid = false;
            }
        }

        /// <summary>
        ///     Gets the full path of the current LOG file.
        /// </summary>
        public static string FilePath
        {
            get
            {
                if (_fileIsValid)
                    return _filePath;
                _filePath = Path.Combine(FileDir, FileName);
                _fileIsValid = true;
                return _filePath;
            }
        }

        /// <summary>
        ///     <para>
        ///         Specifies the <see cref="DebugMode"/> for the handling of <see cref="Exception"/>'s.
        ///         The <see cref="DebugMode"/> can also specified over an command line argument or an
        ///         config parameter in combination with <see cref="AllowLogging(string, string, Regex)"/>.
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
                {
                    var builder = new StringBuilder();
                    builder.Append("Error in the application. Sender object: '");
                    builder.Append(s);
                    builder.Append("'; Exception object: '");
                    builder.Append(e.ExceptionObject);
                    builder.Append(';');
                    WriteUnhandled(new ApplicationException(builder.ToString()));
                };
                AppDomain.CurrentDomain.ProcessExit += (s, e) => Close();
            }
            if (DebugMode < 1)
                return;
            Thread.CurrentThread.CurrentCulture = CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = CurrentCulture;
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
        /// <param name="regex">
        ///     The regular expression to match.
        /// </param>
        public static void AllowLogging(string configPath = null, string key = "Debug", Regex regex = null)
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
                if (!int.TryParse(option, out var i))
                    mode = 1;
                if (i > mode)
                    mode = i;
                if (mode > 0)
                    goto Finalize;
            }
            if (string.IsNullOrEmpty(configPath) || regex == null)
                goto Finalize;
            var path = PathEx.Combine(configPath);
            if (!File.Exists(path))
                goto Finalize;
            var groupNames = regex.GetGroupNames();
            if (!groupNames.Contains("Key") || !groupNames.Contains("Value"))
                goto Finalize;
            string source;
            try
            {
                source = File.ReadAllText(path);
                if (string.IsNullOrEmpty(source))
                    throw new ArgumentNullException(nameof(source));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                goto Finalize;
            }
            foreach (var match in regex.Matches(source).Cast<Match>())
            {
                var keys = match.Groups["Key"].Captures.Cast<Capture>().GetEnumerator();
                var vals = match.Groups["Value"].Captures.Cast<Capture>().GetEnumerator();
                try
                {
                    while (keys.MoveNext() && vals.MoveNext())
                    {
                        var keyName = keys.Current?.Value.Trim();
                        if (!keyName.EqualsEx(key))
                            continue;
                        var strValue = vals.Current?.Value.Trim();
                        if (int.TryParse(strValue, out var value))
                            mode = value;
                        goto Finalize;
                    }
                }
                finally
                {
                    keys.Dispose();
                    vals.Dispose();
                }
            }
            Finalize:
            ActivateLogging(mode);
        }

        /// <summary>
        ///     Writes the specified information into a LOG file.
        /// </summary>
        /// <param name="logMessage">
        ///     The message text to write.
        /// </param>
        /// <param name="exitProcess">
        ///     true to terminate this process after logging; otherwise, false.
        /// </param>
        public static void Write(string logMessage, bool exitProcess = false)
        {
            lock (Builder)
            {
                if (!_firstCall || DebugMode < 1 || !_firstEntry && string.IsNullOrEmpty(logMessage))
                    return;
                if (!_firstEntry)
                    Builder.AppendLine(DateTime.Now.ToString(DateTimeFormat));
                else
                {
                    _firstEntry = false;
                    if (Directory.Exists(FileDir) && File.Exists(FilePath))
                    {
                        Builder.AppendLine();
                        Builder.AppendLine(new string('-', 120));
                        Builder.AppendLine();
                        AppendToFile();
                    }
                    Builder.AppendLine(DateTime.Now.ToString(DateTimeFormat));
                    Builder.Append("System: '");
                    Builder.Append(Environment.OSVersion);
                    Builder.Append("'; Runtime: '");
                    Builder.Append(EnvironmentEx.Version);
                    Builder.Append("'; Assembly: '");
                    Builder.Append(AssemblyName);
                    Builder.Append("'; Version: '");
                    Builder.Append(AssemblyVersion);
                    Builder.AppendLine("';");
                    Builder.AppendLine();
                    if (!string.IsNullOrEmpty(logMessage))
                        Builder.AppendLine(DateTime.Now.ToString(DateTimeFormat));
                }
                if (!string.IsNullOrEmpty(logMessage))
                {
                    Builder.AppendLine(logMessage);
                    Builder.AppendLine();
                }
                var content = AppendToFile();
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
                    _stdHandle = WinApi.NativeMethods.GetStdHandle(-0xb);
                    _sfh = new SafeFileHandle(_stdHandle, true);
                    _fs = new FileStream(_sfh, FileAccess.Write);
                    var title = $"Debug Console ('{AssemblyName}')";
                    if (Console.Title != title)
                    {
                        Console.Title = title;
                        var parentName = ProcessEx.CurrentParent?.ProcessName;
                        if (!parentName.EqualsEx("bash", "cmd", "powershell", "powershell_ise"))
                        {
                            Console.SetBufferSize(short.MaxValue - 1, Console.WindowWidth);
                            Console.SetWindowSize(Math.Min(120, Console.LargestWindowWidth), Math.Min(55, Console.LargestWindowHeight));
                        }
                    }
                }
                Console.Write(content);
                _sw = new StreamWriter(_fs, Encoding.ASCII) { AutoFlush = true };
                Console.SetOut(_sw);
                if (!exitProcess)
                    return;
                Environment.ExitCode = 1;
                Environment.Exit(Environment.ExitCode);
            }
        }

        /// <summary>
        ///     Writes all <see cref="Exception"/> information into a LOG file.
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

        private static string AppendToFile()
        {
            lock (Builder)
            {
                var content = Builder.Length > 0 ? Builder.ToString() : string.Empty;
                if (Directory.Exists(FileDir))
                    File.AppendAllText(FilePath, content);
                Builder.Clear();
                return content;
            }
        }

        private static void Close()
        {
            if (_sfh?.IsClosed == false)
                _sfh.Close();
            if (DebugMode < 1 || !Directory.Exists(FileDir))
                return;
            AppendToFile();
            foreach (var file in Directory.EnumerateFiles(FileDir, AssemblyName + "*.log", SearchOption.TopDirectoryOnly))
            {
                if (FilePath.EqualsEx(file))
                    continue;
                if ((DateTime.Now - File.GetLastWriteTime(file)).TotalDays >= 7d)
                    File.Delete(file);
            }
        }
    }
}
