#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Log.cs
// Version:  2023-12-20 00:28
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
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
    using QuickWmi;

    /// <summary>
    ///     Provides functionality for the catching and logging of handled or unhandled
    ///     <see cref="Exception"/>'s.
    ///     <para>
    ///         This class is not intended to replace the Visual Studio debugging
    ///         tools. It was primarily designed for final releases in which debugging
    ///         is disabled. Exceptions can be caught entirely so that the end user
    ///         doesn't get confused unnecessarily. All exceptions are logged and can
    ///         still reliably help developers.
    ///     </para>
    /// </summary>
    public static class Log
    {
        private const string DateTimeFormat = @"yyyy-MM-dd HH:mm:ss,fff zzz";
        private static volatile object _syncObject;
        private static volatile string _debugKey;

        private static volatile int _debugMode,
                                    _doNotCatchUnhandled,
                                    _conIsAllocated,
                                    _firstCall,
                                    _firstEntry;

        private static volatile string _fileDir,
                                       _fileName,
                                       _filePath;

        private static volatile FileStream _fileStream;
        private static volatile StreamWriter _streamWriter;
        private static readonly AssemblyName AssemblyEntryName = Assembly.GetEntryAssembly()?.GetName();
        private static readonly string AssemblyName = AssemblyEntryName?.Name;
        private static readonly Version AssemblyVersion = AssemblyEntryName?.Version;

        /// <summary>
        ///     <see langword="true"/> to enable the catching of unhandled
        ///     <see cref="Exception"/>'s; otherwise, <see langword="false"/>.
        /// </summary>
        public static bool CatchUnhandled
        {
            get => _doNotCatchUnhandled == 0;
            set => Interlocked.Exchange(ref _doNotCatchUnhandled, value ? 1 : 0);
        }

        /// <summary>
        ///     Gets or sets the culture for the current thread.
        /// </summary>
        public static CultureInfo CurrentCulture => CultureConfig.GlobalCultureInfo;

        /// <summary>
        ///     Gets the current <see cref="DebugMode"/> value that determines how
        ///     <see cref="Exception"/>'s are caught and logged. For more information see
        ///     <see cref="ActivateLogging(int)"/>.
        /// </summary>
        public static int DebugMode
        {
            get => _debugMode;
            set => Interlocked.Exchange(ref _debugMode, value);
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
                var dir = Environment.GetEnvironmentVariable("TEMP");
                Interlocked.CompareExchange(ref _fileDir, dir, null);
                return _fileDir;
            }
            set
            {
                lock (SyncObject)
                {
                    _fileDir = value;
                    _filePath = null;
                    _streamWriter?.Dispose();
                    _streamWriter = null;
                    _fileStream?.Dispose();
                    _fileStream = null;
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
                var name = $"{AssemblyName}_{DateTime.Now:yyyy-MM-dd}.log";
                Interlocked.CompareExchange(ref _fileName, name, null);
                return _fileName;
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
                var path = Path.Combine(FileDir, FileName);
                Interlocked.CompareExchange(ref _filePath, path, null);
                return _filePath;
            }
        }

        /// <summary>
        ///     Gets or sets the key that is used to allow logging by command line
        ///     arguments or a configuration file. For more information see
        ///     <see cref="AllowLogging(string, string)"/>.
        ///     <para>
        ///         <strong>
        ///             Default:
        ///         </strong>
        ///         'DebugMode'
        ///     </para>
        /// </summary>
        public static string DebugKey
        {
            get
            {
                if (_debugKey != null)
                    return _debugKey;
                Interlocked.CompareExchange(ref _debugKey, @"DebugMode", null);
                return _debugKey;
            }
            set => Interlocked.Exchange(ref _debugKey, value);
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
                var fs = File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                var sw = new StreamWriter(fs)
                {
                    AutoFlush = true
                };
                Interlocked.CompareExchange(ref _fileStream, fs, null);
                Interlocked.CompareExchange(ref _streamWriter, sw, null);
                return _streamWriter;
            }
        }

        private static bool ConIsAllocated
        {
            get => _conIsAllocated == 1;
            set => Interlocked.Exchange(ref _conIsAllocated, value ? 1 : 0);
        }

        private static bool FirstCall
        {
            get => _firstCall == 1;
            set => Interlocked.Exchange(ref _firstCall, value ? 1 : 0);
        }

        private static bool FirstEntry
        {
            get => _firstEntry == 1;
            set => Interlocked.Exchange(ref _firstEntry, value ? 1 : 0);
        }

        /// <summary>
        ///     Specifies the <see cref="DebugMode"/> for the handling of exceptions. This
        ///     variable can also be specified over an command line argument using
        ///     <see cref="AllowLogging(string, string)"/> method.
        ///     <para>
        ///         <strong>
        ///             The following modes are available:
        ///         </strong>
        ///     </para>
        ///     <list type="number">
        ///         <listheader>
        ///             <term>
        ///                 Logging is disabled
        ///             </term>
        ///             <description>
        ///                 Exceptions are still caught.
        ///                 <para>
        ///                     If <see cref="CatchUnhandled"/> is <see langword="true"/>,
        ///                     unhandled exceptions are also discarded. This can be useful
        ///                     for public releases to prevent any kind of exception
        ///                     notifications to the end user. But the consequences are
        ///                     dangerous if used incorrectly because all exceptions are
        ///                     completely suppressed.
        ///                 </para>
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 Logging is enabled
        ///             </term>
        ///             <description>
        ///                 Exceptions are caught and logged.
        ///                 <i>
        ///                     (Recommended)
        ///                 </i>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 Logging is enabled
        ///             </term>
        ///             <description>
        ///                 Exceptions are caught and logged. A console window is allocated
        ///                 to display logging in real time.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 Logging is enabled
        ///             </term>
        ///             <description>
        ///                 All exceptions are thrown.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="mode">
        ///     The logging mode to be set.
        /// </param>
        public static void ActivateLogging(int mode = 1)
        {
            DebugMode = mode;
            if (FirstCall)
                return;
            FirstCall = true;
            if (CatchUnhandled)
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += (_, e) => Write(e.Exception, true, true);
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
                AppDomain.CurrentDomain.ProcessExit += (_, _) => OnProcessExit();
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
        ///     configuration file using the specified regular expression pattern. For more
        ///     information see <see cref="ActivateLogging(int)"/>.
        ///     <list type="bullet">
        ///         <listheader>
        ///             <para>
        ///                 <strong>
        ///                     There are several ways to make the regular expression
        ///                     pattern work:
        ///                 </strong>
        ///             </para>
        ///             <list type="number">
        ///                 <item>
        ///                     <description>
        ///                         Use of the Key and Value groups, that are used to
        ///                         search for the correct key within the file and sets its
        ///                         value as logging mode.
        ///                     </description>
        ///                 </item>
        ///                 <item>
        ///                     <description>
        ///                         It's also possible to define the Value group alone, or
        ///                         even to use no groups at all. That will only check the
        ///                         first matching value.
        ///                     </description>
        ///                 </item>
        ///             </list>
        ///         </listheader>
        ///         <item>
        ///             <para>
        ///                 <strong>
        ///                     Available regular expression groups:
        ///                 </strong>
        ///             </para>
        ///             <list type="number">
        ///                 <item>
        ///                     <term>
        ///                         Key
        ///                     </term>
        ///                     <description>
        ///                         Represents the key within the configuration file that
        ///                         contains the value. Must be the value of the
        ///                         <see cref="DebugKey"/> property.
        ///                     </description>
        ///                 </item>
        ///                 <item>
        ///                     <term>
        ///                         Value
        ///                     </term>
        ///                     <description>
        ///                         Represents the value passed to the
        ///                         <see cref="ActivateLogging(int)"/> method. Must be
        ///                         convertible to <see cref="int"/>.
        ///                     </description>
        ///                 </item>
        ///             </list>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="configPath">
        ///     The path of the text file to look for the value that determines the logging
        ///     mode.
        /// </param>
        /// <param name="pattern">
        ///     The regular expression pattern to match in the specified configuration
        ///     file.
        ///     <para>
        ///         <i>
        ///             (The standard pattern has the INI file format, which searches for
        ///             the appropriate key in all sections and uses it's value to
        ///             determine the logging mode.)
        ///         </i>
        ///     </para>
        /// </param>
        public static void AllowLogging(string configPath, string pattern = @"(?<Key>(.*?))(?:=)(?<Value>(.*?))(?:\s)")
        {
            lock (SyncObject)
            {
                var mode = 0;

                // Look for the correct command line switch
                var seek = $"/{DebugKey}";
                foreach (var arg in Environment.GetCommandLineArgs().Skip(1))
                {
                    if ((mode < 1 && !arg.EqualsEx(seek)) || mode++ < 1)
                        continue;
                    mode = arg.EqualsEx("True") ? 1 : arg.ToInt32();
                    break;
                }

                // Look for a regular expression in the specified configuration file
                if (mode < 1 && !string.IsNullOrEmpty(pattern) && FileEx.Exists(configPath))
                {
                    var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    var content = FileEx.ReadAllText(configPath);
                    var matches = regex.Matches(content).Cast<Match>().ToArray();
                    if (matches.Length > 0)
                    {
                        if (pattern.Contains("?<Key>") && pattern.Contains("?<Value>"))
                            foreach (var match in matches)
                            {
                                var keys = match.Groups["Key"].Captures;
                                var values = match.Groups["Value"].Captures;
                                if (keys.Count == 0 || values.Count == 0)
                                    continue;
                                for (var i = 0; i < Math.Min(keys.Count, values.Count); i++)
                                {
                                    if (keys[i].Value.EqualsEx(DebugKey))
                                        continue;
                                    var value = values[i].Value;
                                    if (string.IsNullOrEmpty(value))
                                        continue;
                                    if (value.EqualsEx("True"))
                                    {
                                        mode = 1;
                                        break;
                                    }
                                    mode = value.ToInt32();
                                    break;
                                }
                                if (mode > 0)
                                    break;
                            }
                        else
                        {
                            string value;
                            if (pattern.Contains("?<Value>"))
                                value = matches.SelectMany(m => m.Groups["Value"].Captures.Cast<Capture>())
                                               .Select(c => c.Value)
                                               .FirstOrDefault();
                            else
                                value = matches.SelectMany(m => m.Groups.Cast<Group>())
                                               .SelectMany(g => g.Captures.Cast<Capture>())
                                               .Select(c => c.Value)
                                               .FirstOrDefault();
                            if (!string.IsNullOrEmpty(value))
                                mode = value.EqualsEx("True") ? 1 : value.ToInt32();
                        }
                    }
                }

                ActivateLogging(mode);
            }
        }

        /// <summary>
        ///     Allows you to enable logging by command line arguments. For more
        ///     information see <see cref="ActivateLogging(int)"/>.
        /// </summary>
        public static void AllowLogging() =>
            AllowLogging(null, null);

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
            if (exception == null || exTypes?.Length is null or < 1)
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
            if (!FirstCall || DebugMode < 1 || (FirstEntry && string.IsNullOrEmpty(logMessage)))
                return;

            lock (SyncObject)
            {
                if (DebugMode > 1 && !ConIsAllocated)
                {
                    ConIsAllocated = true;
                    ConsoleWindow.Allocate(true);
                }

                if (!FirstEntry)
                {
                    FirstEntry = true;

                    var separator = new string('=', 65);
                    var front = $"New Process {DateTime.Now.ToStringDefault(DateTimeFormat)} ";

                    Append(front);
                    AppendLine(separator);

                    Append(front);
                    Append(" Operating System: '");
                    Append(Win32_OperatingSystem.Caption);
                    Append(" (");
                    Append(Win32_OperatingSystem.Version.ToString());
                    Append(")");
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
                }

                if (!string.IsNullOrEmpty(logMessage))
                {
                    Append(ProcessEx.CurrentId.ToStringDefault());
                    Append(" ");
                    Append(DateTime.Now.ToStringDefault(DateTimeFormat));
                    Append(" | ");
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
            if (DebugMode < 2 && exception is ArgumentNullException or NullReferenceException or NotSupportedException or WarningException)
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
