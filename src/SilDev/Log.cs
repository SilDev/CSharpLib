#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Log.cs
// Version:  2020-01-19 15:31
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
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
        private const string DateTimeFormat = @"yyyy-MM-dd HH:mm:ss,fff zzz";
        private static volatile AssemblyName _assemblyEntryName;
        private static volatile StringBuilder _builder;

        private static volatile bool _catchUnhandledExceptions = true,
                                     _conIsOpen,
                                     _firstCall,
                                     _firstEntry;

        private static volatile string _debugKey;
        private static volatile int _debugMode;

        private static volatile string _fileDir,
                                       _fileName,
                                       _filePath;

        private static volatile FileStream _fs;
        private static volatile SafeFileHandle _sfh;
        private static volatile IntPtr _stdHandle = IntPtr.Zero;
        private static volatile StreamWriter _sw;
        private static volatile object _syncObject;

        /// <summary>
        ///     <see langword="true"/> to enable the catching of unhandled
        ///     <see cref="Exception"/>'s; otherwise, <see langword="false"/>.
        /// </summary>
        public static bool CatchUnhandledExceptions
        {
            get => _catchUnhandledExceptions;
            set
            {
                lock (SyncObject)
                    _catchUnhandledExceptions = value;
            }
        }

        /// <summary>
        ///     Gets or sets the culture for the current thread.
        /// </summary>
        public static CultureInfo CurrentCulture => CultureConfig.GlobalCultureInfo;

        /// <summary>
        ///     Gets the current <see cref="DebugMode"/> value that determines how
        ///     <see cref="Exception"/>'s are handled. For more information see
        ///     <see cref="ActivateLogging(int)"/>.
        /// </summary>
        public static int DebugMode
        {
            get => _debugMode;
            set
            {
                lock (SyncObject)
                    _debugMode = value;
            }
        }

        /// <summary>
        ///     Gets or sets the location of the current LOG file.
        ///     <para>
        ///         If the specified path doesn't exists, it is created.
        ///     </para>
        ///     <para>
        ///         If the specified path is invalid or this process doesn't have the
        ///         necessary permissions to write to this location, the location is
        ///         changed to the Windows specified folder for temporary files.
        ///     </para>
        /// </summary>
        public static string FileDir
        {
            get
            {
                if (_fileDir != null)
                    return _fileDir;
                lock (SyncObject)
                {
                    _fileDir = Environment.GetEnvironmentVariable("TEMP");
                    return _fileDir;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    _fileDir = value;
                    _filePath = null;
                }
            }
        }

        /// <summary>
        ///     Gets the name of the current LOG file.
        /// </summary>
        public static string FileName
        {
            get
            {
                if (_fileName != null)
                    return _fileName;
                lock (SyncObject)
                {
                    _fileName = $"{AssemblyName}_{DateTime.Now:yyyy-MM-dd}.log";
                    return _fileName;
                }
            }
        }

        /// <summary>
        ///     Gets the full path of the current LOG file.
        /// </summary>
        public static string FilePath
        {
            get
            {
                if (_filePath != null)
                    return _filePath;
                lock (SyncObject)
                {
                    _filePath = Path.Combine(FileDir, FileName);
                    return _filePath;
                }
            }
        }

        internal static string DebugKey
        {
            get
            {
                if (_debugKey != null)
                    return _debugKey;
                lock (SyncObject)
                {
                    _debugKey = @"Debug";
                    return _debugKey;
                }
            }
            set
            {
                lock (SyncObject)
                    _debugKey = value;
            }
        }

        private static object SyncObject
        {
            get
            {
                if (_syncObject != null)
                    return _syncObject;
                var obj = new object();
                Interlocked.CompareExchange<object>(ref _syncObject, obj, null);
                return _syncObject;
            }
        }

        private static AssemblyName AssemblyEntryName
        {
            get
            {
                if (_assemblyEntryName != null)
                    return _assemblyEntryName;
                lock (SyncObject)
                {
                    _assemblyEntryName = Assembly.GetEntryAssembly()?.GetName();
                    return _assemblyEntryName;
                }
            }
        }

        private static string AssemblyName => AssemblyEntryName.Name;

        private static Version AssemblyVersion => AssemblyEntryName.Version;

        private static StringBuilder Builder
        {
            get
            {
                if (_builder != null)
                    return _builder;
                lock (SyncObject)
                {
                    _builder = new StringBuilder();
                    return _builder;
                }
            }
        }

        private static bool ConIsOpen
        {
            get => _conIsOpen;
            set
            {
                lock (SyncObject)
                    _conIsOpen = value;
            }
        }

        private static bool FirstCall
        {
            get => _firstCall;
            set
            {
                lock (SyncObject)
                    _firstCall = value;
            }
        }

        private static bool FirstEntry
        {
            get => _firstEntry;
            set
            {
                lock (SyncObject)
                    _firstEntry = value;
            }
        }

        /// <summary>
        ///     Specifies the <see cref="DebugMode"/> for the handling of
        ///     <see cref="Exception"/>'s. The <see cref="DebugMode"/> can also specified
        ///     over an command line argument or an config parameter in combination with
        ///     <see cref="AllowLogging(string, string, Regex)"/>. The following
        ///     <see cref="DebugMode"/> options are available.
        ///     <para>
        ///         0: Logging is disabled. <see cref="Exception"/>'s are caught, and if
        ///         <see cref="CatchUnhandledExceptions"/> is enabled, unhandled
        ///         <see cref="Exception"/>'s are also discarded. This can be useful for
        ///         public releases to prevent any kind of <see cref="Exception"/>
        ///         notifications to the client. Please note that these functions may have
        ///         dangerous consequences if used incorrectly.
        ///     </para>
        ///     <para>
        ///         1: Logging is enabled, <see cref="Exception"/>'s are caught, and all
        ///         <see cref="Exception"/>'s are logged.
        ///     </para>
        ///     <para>
        ///         2: Logging is enabled, <see cref="Exception"/>'s are caught, all
        ///         <see cref="Exception"/>'s are logged, and a new <see cref="Console"/>
        ///         window is allocated for the current process to display the current log
        ///         in real time.
        ///     </para>
        ///     <para>
        ///         3: Logging is enabled, but <see cref="Exception"/>'s are thrown.
        ///     </para>
        /// </summary>
        public static void ActivateLogging(int mode = 1)
        {
            DebugMode = mode;
            if (FirstCall)
                return;
            FirstCall = true;
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
                    WriteUnhandled(new ApplicationException(builder.ToStringThenClear()));
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
        ///     Allows you to enable logging by command line arguments or a specified
        ///     configuration file. For more information see
        ///     <see cref="ActivateLogging(int)"/>.
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
            lock (SyncObject)
            {
                var args = Environment.GetCommandLineArgs();
                if (!DebugKey.EqualsEx(key))
                    DebugKey = key;
                var arg = string.Concat('/', key);
                var mode = 0;
                if (args.ContainsItem(arg))
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
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        Debug.WriteLine(ex);
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
                        throw new WarningException();
                }
                catch (Exception ex) when (ex.IsCaught())
                {
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
        }

        /// <summary>
        ///     Determines whether this <see cref="Exception"/> should be caught or thrown.
        ///     For more information see <see cref="ActivateLogging(int)"/>.
        /// </summary>
        /// <param name="exception">
        ///     The <see cref="Exception"/> to be checked.
        /// </param>
        /// <param name="exTypes">
        ///     A sequence of <see cref="Exception"/> types to catch.
        /// </param>
        public static bool IsCaught(this Exception exception, params Type[] exTypes)
        {
            if (DebugMode > 2)
            {
                Write($"Thrown {exception}");
                return false;
            }
            if (exception == null || exTypes == null || !exTypes.Any())
                return true;
            var current = exception.GetType();
            return exTypes.Any(type => type == current);
        }

        /// <summary>
        ///     Writes the specified information into a LOG file.
        /// </summary>
        /// <param name="logMessage">
        ///     The message text to write.
        /// </param>
        /// <param name="exitProcess">
        ///     <see langword="true"/> to terminate this process after logging; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static void Write(string logMessage, bool exitProcess = false)
        {
            if (!FirstCall || DebugMode < 1 || FirstEntry && string.IsNullOrEmpty(logMessage))
                return;
            lock (SyncObject)
            {
                Build:
                if (FirstEntry)
                {
                    Builder.Append(ProcessEx.CurrentId);
                    Builder.Append(" ");
                    Builder.Append(DateTime.Now.ToStringDefault(DateTimeFormat));
                    Builder.Append(" | ");
                }
                else
                {
                    FirstEntry = true;
                    var separator = new string('=', 65);

                    Builder.Append("New Process ");
                    Builder.Append(DateTime.Now.ToStringDefault(DateTimeFormat));
                    Builder.Append(" ");

                    var front = Builder.ToString();
                    Builder.AppendLine(separator);

                    Builder.Append(front);
                    Builder.Append(" Operating System: '");
                    Builder.Append(Environment.OSVersion);
                    Builder.AppendLine("'");

                    Builder.Append(front);
                    Builder.Append(" DotNET Runtime:   '");
                    Builder.Append(EnvironmentEx.Version);
                    Builder.AppendLine("'");

                    Builder.Append(front);
                    Builder.Append(" Process ID:       '");
                    Builder.Append(ProcessEx.CurrentId);
                    Builder.AppendLine("'");

                    Builder.Append(front);
                    Builder.Append(" Process Name:     '");
                    Builder.Append(ProcessEx.CurrentName);
                    Builder.AppendLine("'");

                    Builder.Append(front);
                    Builder.Append(" File Path:        '");
                    Builder.Append(PathEx.LocalPath);
                    Builder.AppendLine("'");

                    Builder.Append(front);
                    Builder.Append(" File Version:     '");
                    Builder.Append(AssemblyVersion);
                    Builder.AppendLine("'");

                    Builder.Append(front);
                    Builder.AppendLine(separator);

                    Builder.AppendLine();
                    if (!string.IsNullOrEmpty(logMessage))
                        goto Build;
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

                if (!ConIsOpen)
                {
                    ConIsOpen = true;

                    _ = WinApi.NativeMethods.AllocConsole();
                    var hWnd = WinApi.NativeMethods.GetConsoleWindow();
                    if (hWnd != IntPtr.Zero)
                    {
                        var hMenu = WinApi.NativeMethods.GetSystemMenu(hWnd, false);
                        if (hMenu != IntPtr.Zero)
                            _ = WinApi.NativeMethods.DeleteMenu(hMenu, 0xf060, 0x0);
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

                _sw = new StreamWriter(_fs, Encoding.ASCII)
                {
                    AutoFlush = true
                };
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
        ///     <see langword="true"/> to enforce that <see cref="DebugMode"/> is enabled;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="exitProcess">
        ///     <see langword="true"/> to terminate this process after logging; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static void Write(Exception exception, bool forceLogging = false, bool exitProcess = false)
        {
            if (DebugMode < 1)
            {
                if (!forceLogging)
                    return;
                DebugMode = 1;
            }
            if (DebugMode < 2 && (exception is ArgumentNullException || exception is NotSupportedException || exception is WarningException))
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
            if (Builder.Length <= 0)
                return string.Empty;
            var content = Builder.ToStringThenClear();
            if (Directory.Exists(FileDir))
                File.AppendAllText(FilePath, content);
            return content;
        }

        private static void Close()
        {
            lock (SyncObject)
            {
                if (_sfh?.IsClosed ?? false)
                    _sfh.Close();
                if (DebugMode < 1 || !Directory.Exists(FileDir))
                    return;
                AppendToFile();
                Directory.EnumerateFiles(FileDir, $"{AssemblyName}*.log", SearchOption.TopDirectoryOnly)
                         .Where(file => !FilePath.EqualsEx(file) && (DateTime.Now - File.GetLastWriteTime(file)).TotalDays >= 7d)
                         .ForEach(File.Delete);
            }
        }
    }
}
