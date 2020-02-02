#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Log.cs
// Version:  2020-02-02 11:25
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
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

    /// <summary>
    ///     Provides functionality for the catching and logging of handled or unhandled
    ///     <see cref="Exception"/>'s.
    /// </summary>
    public static class Log
    {
        private const string DateTimeFormat = @"yyyy-MM-dd HH:mm:ss,fff zzz";

        private static volatile bool _catchUnhandled = true,
                                     _conIsAllocated,
                                     _firstCall,
                                     _firstEntry;

        private static volatile string _debugKey;
        private static volatile int _debugMode;

        private static volatile string _fileDir,
                                       _fileName,
                                       _filePath;

        private static volatile FileStream _fileStream;
        private static volatile StreamWriter _streamWriter;
        private static volatile object _syncObject;
        private static readonly AssemblyName AssemblyEntryName = Assembly.GetEntryAssembly()?.GetName();
        private static readonly string AssemblyName = AssemblyEntryName?.Name;
        private static readonly Version AssemblyVersion = AssemblyEntryName?.Version;

        /// <summary>
        ///     <see langword="true"/> to enable the catching of unhandled
        ///     <see cref="Exception"/>'s; otherwise, <see langword="false"/>.
        /// </summary>
        public static bool CatchUnhandled
        {
            get => _catchUnhandled;
            set
            {
                lock (SyncObject)
                    _catchUnhandled = value;
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
                    _streamWriter?.Dispose();
                    _fileStream?.Dispose();
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

        private static StreamWriter FileWriter
        {
            get
            {
                if (_fileStream != null && _streamWriter != null)
                    return _streamWriter;
                lock (SyncObject)
                {
                    _fileStream = File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    _streamWriter = new StreamWriter(_fileStream)
                    {
                        AutoFlush = true
                    };
                    return _streamWriter;
                }
            }
        }

        private static bool ConIsAllocated
        {
            get => _conIsAllocated;
            set
            {
                lock (SyncObject)
                    _conIsAllocated = value;
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
        ///         <see cref="CatchUnhandled"/> is enabled, unhandled
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
            if (CatchUnhandled)
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
                AppDomain.CurrentDomain.ProcessExit += (s, e) => OnProcessExit();
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
                if (DebugMode > 1 && !ConIsAllocated)
                {
                    ConIsAllocated = true;
                    ConsoleWindow.Allocate(true);
                }

                Build:
                if (FirstEntry)
                {
                    Append(ProcessEx.CurrentId.ToStringDefault());
                    Append(" ");
                    Append(DateTime.Now.ToStringDefault(DateTimeFormat));
                    Append(" | ");
                }
                else
                {
                    FirstEntry = true;

                    var separator = new string('=', 65);
                    var front = $"New Process {DateTime.Now.ToStringDefault(DateTimeFormat)} ";

                    Append(front);
                    AppendLine(separator);

                    Append(front);
                    Append(" Operating System: '");
                    Append(Environment.OSVersion.ToString());
                    AppendLine("'");

                    Append(front);
                    Append(" DotNET Runtime:   '");
                    Append(EnvironmentEx.Version.ToString());
                    AppendLine("'");

                    Append(front);
                    Append(" Process ID:       '");
                    Append(ProcessEx.CurrentId.ToStringDefault());
                    AppendLine("'");

                    Append(front);
                    Append(" Process Name:     '");
                    Append(ProcessEx.CurrentName);
                    AppendLine("'");

                    Append(front);
                    Append(" File Path:        '");
                    Append(PathEx.LocalPath);
                    AppendLine("'");

                    Append(front);
                    Append(" File Version:     '");
                    Append(AssemblyVersion.ToString());
                    AppendLine("'");

                    Append(front);
                    AppendLine(separator);

                    AppendLine();
                    if (!string.IsNullOrEmpty(logMessage))
                        goto Build;
                }

                if (!string.IsNullOrEmpty(logMessage))
                {
                    AppendLine(logMessage);
                    AppendLine();
                }

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
            if (DebugMode < 2 && (exception is ArgumentNullException ||
                                  exception is NullReferenceException ||
                                  exception is NotSupportedException ||
                                  exception is WarningException))
                return;
            Write($"Handled {exception}", exitProcess);
        }

        private static void WriteUnhandled(Exception exception)
        {
            DebugMode = 1;
            Write($"Unhandled {exception}", true);
        }

        private static void Append(string value)
        {
            if (value == null)
                return;
            if (ConIsAllocated)
                Console.Write(value);
            FileWriter.Write(value);
        }

        private static void AppendLine(string value = null)
        {
            if (value == null)
            {
                if (ConIsAllocated)
                    Console.WriteLine();
                FileWriter.WriteLine();
                return;
            }
            if (ConIsAllocated)
                Console.WriteLine(value);
            FileWriter.Write(value);
            FileWriter.WriteLine();
        }

        private static void OnProcessExit()
        {
            lock (SyncObject)
            {
                _streamWriter?.Dispose();
                _fileStream?.Dispose();
                if (DebugMode < 1 || !Directory.Exists(FileDir))
                    return;
                DirectoryEx.EnumerateFiles(FileDir, $"{AssemblyName}*.log").Where(file => !FilePath.EqualsEx(file) && (DateTime.Now - File.GetLastWriteTime(file)).TotalDays >= 7d).ForEach(File.Delete);
            }
        }
    }
}
