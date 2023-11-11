#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: CmdExec.cs
// Version:  2023-11-11 16:27
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Intern;

    /// <summary>
    ///     Provides functions for executing command prompt commands.
    /// </summary>
    public static class CmdExec
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class to execute
        ///     system commands using the system command prompt ("cmd.exe").
        ///     <para>
        ///         This can be useful for an unprivileged application as a simple way to
        ///         execute a command with the highest user permissions, for example.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to start the application with administrator
        ///     privileges; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="processWindowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if the process has been started;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static Process Send(string command, bool runAsAdmin = false, ProcessWindowStyle processWindowStyle = ProcessWindowStyle.Hidden, bool dispose = true)
        {
            try
            {
                var trimChars = new[]
                {
                    '\t', '\n', '\r', ' ', '&', '|'
                };
                var cmd = command?.Trim(trimChars) ?? throw new ArgumentNullException(nameof(command));
                if (processWindowStyle == ProcessWindowStyle.Hidden && cmd.StartsWithEx("/K "))
                    cmd = cmd.Substring(3).TrimStart();
                if (!cmd.StartsWithEx("/C ", "/K "))
                    cmd = $"/C {cmd}";
                if (cmd.Length < 16)
                    throw new ArgumentInvalidException(nameof(command));
#if any || x86
                var path = ComSpec.SysNativePath;
#else
                var path = ComSpec.DefaultPath;
#endif
                var anyLineSeparator = cmd.Any(TextEx.IsLineSeparator);
                if (anyLineSeparator || (path + cmd).Length + 4 > 8192)
                {
                    var sb = new StringBuilder();
                    var file = FileEx.GetUniqueTempPath("tmp", ".cmd");
                    var content = cmd.Substring(2).TrimStart(trimChars);
                    if (content.StartsWithEx("@ECHO ON", "@ECHO OFF"))
                    {
                        var start = content.StartsWithEx("@ECHO ON") ? 8 : 9;
                        content = content.Substring(start).TrimStart(trimChars);
                        sb.AppendLine(processWindowStyle == ProcessWindowStyle.Hidden || start == 9 ? "@ECHO OFF" : "@ECHO ON");
                    }
                    var loopVars = Regex.Matches(content, @"((\s|\""|\'|\=)(?<var>(%[A-Za-z]{1,32}))(\s|\""|\'|\)))").Cast<Match>().ToArray();
                    if (loopVars.Any())
                    {
                        var indicator = 0;
                        content = loopVars.Select(m => m.Groups["var"].Index + ++indicator)
                                          .Aggregate(content, (s, i) => $"{s.Substring(0, i)}%{s.Substring(i)}");
                    }
                    var exit = content.EndsWithEx("EXIT /B %ERRORLEVEL%",
                                                  "EXIT /B !ERRORLEVEL!",
                                                  "EXIT /B",
                                                  "EXIT %ERRORLEVEL%",
                                                  "EXIT !ERRORLEVEL!",
                                                  "EXIT");
                    if (exit)
                    {
                        if (content.EndsWithEx("%ERRORLEVEL%", "!ERRORLEVEL!"))
                            content = content.Substring(0, content.Length - 12).TrimEnd(trimChars);
                        content = content.Substring(0, content.Length - (content.EndsWithEx("EXIT") ? 4 : 7)).TrimEnd(trimChars);
                    }
                    sb.AppendLine(content);
                    sb.AppendFormatCurrent("DEL /F /Q \"{0}\"", file);
                    File.WriteAllText(file, sb.ToStringThenClear());
                    cmd = $"/C CALL \"{file}\" & EXIT";
                }
                var psi = new ProcessStartInfo
                {
                    Arguments = cmd,
                    FileName = path,
                    UseShellExecute = runAsAdmin,
                    Verb = runAsAdmin ? "runas" : string.Empty,
                    WindowStyle = processWindowStyle
                };
                var p = ProcessEx.Start(psi, dispose);
                if (Log.DebugMode > 1)
                    Log.Write($"COMMAND EXECUTED: {cmd.Substring(3)}");
                return p;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class to execute
        ///     system commands using the system command prompt ("cmd.exe").
        ///     <para>
        ///         This can be useful for an unprivileged application as a simple way to
        ///         execute a command with the highest user permissions, for example.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to start the application with administrator
        ///     privileges; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if the process has been started;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static Process Send(string command, bool runAsAdmin, bool dispose) =>
            Send(command, runAsAdmin, ProcessWindowStyle.Hidden, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class to execute
        ///     system commands using the system command prompt ("cmd.exe").
        ///     <para>
        ///         This can be useful for an unprivileged application as a simple way to
        ///         execute a command with the highest user permissions, for example.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="processWindowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if the process has been started;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static Process Send(string command, ProcessWindowStyle processWindowStyle, bool dispose = true) =>
            Send(command, false, processWindowStyle, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class to execute
        ///     system commands using the system command prompt ("cmd.exe") and stream its
        ///     output to a console, if available.
        /// </summary>
        /// <param name="commands">
        ///     The commands to execute.
        /// </param>
        public static bool SendEx(params string[] commands)
        {
            try
            {
                if (commands == null)
                    throw new ArgumentNullException(nameof(commands));
#if any || x86
                var path = ComSpec.SysNativePath;
#else
                var path = ComSpec.DefaultPath;
#endif
                using var p = new Process();
                p.StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = path,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    Verb = Elevation.IsAdministrator ? "runas" : string.Empty
                };
                p.Start();
                foreach (var s in commands)
                    p.StandardInput.WriteLine(s);
                p.StandardInput.Flush();
                p.StandardInput.Close();
                Console.WriteLine(p.StandardOutput.ReadToEnd());
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Copies an existing file or directory to a new location.
        /// </summary>
        /// <param name="srcPath">
        ///     The path to the file or directory to copy.
        /// </param>
        /// <param name="destPath">
        ///     The name of the destination file or directory.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="wait">
        ///     <see langword="true"/> to wait indefinitely for the associated process to
        ///     exit; otherwise, <see langword="false"/>.
        /// </param>
        public static bool Copy(string srcPath, string destPath, bool runAsAdmin = false, bool wait = true)
        {
            if (string.IsNullOrWhiteSpace(srcPath) || string.IsNullOrWhiteSpace(destPath))
                return false;
            var src = PathEx.Combine(srcPath);
            if (!PathEx.DirOrFileExists(src))
                return false;
            var dest = PathEx.Combine(destPath);
            using var p = Send($"COPY /Y \"{src}\" \"{dest}\"", runAsAdmin, false);
            if (!wait)
                return true;
            if (p?.HasExited ?? false)
                p.WaitForExit();
            return (p?.ExitCode ?? 1) == 0;
        }

        /// <summary>
        ///     Deletes an existing file or directory.
        /// </summary>
        /// <param name="path">
        ///     The path to the file or directory to be deleted.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="wait">
        ///     <see langword="true"/> to wait indefinitely for the associated process to
        ///     exit; otherwise, <see langword="false"/>.
        /// </param>
        public static bool Delete(string path, bool runAsAdmin = false, bool wait = true)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;
            var src = PathEx.Combine(path);
            if (!PathEx.DirOrFileExists(src))
                return true;
            using var p = Send((PathEx.IsDir(src) ? "RMDIR /S /Q \"{0}\"" : "DEL /F /Q \"{0}\"").FormatCurrent(src), runAsAdmin, false);
            if (!wait)
                return true;
            if (p?.HasExited ?? false)
                p.WaitForExit();
            return (p?.ExitCode ?? 1) == 0;
        }

        /// <summary>
        ///     Waits for the specified seconds to execute the specified command.
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="seconds">
        ///     The time to wait in seconds.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process WaitThenCmd(string command, int seconds = 5, bool runAsAdmin = false, bool dispose = true)
        {
            if (string.IsNullOrWhiteSpace(command))
                return null;
            var time = seconds < 1 ? 1 : seconds > 3600 ? 3600 : seconds;
            return Send($"PING LOCALHOST -n {time} > NUL && {command}", runAsAdmin, dispose);
        }

        /// <summary>
        ///     Waits for the specified seconds to execute the specified command.
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process WaitThenCmd(string command, bool runAsAdmin, bool dispose = true) =>
            WaitThenCmd(command, 5, runAsAdmin, dispose);

        /// <summary>
        ///     Waits for the specified seconds to delete the target at the specified path.
        /// </summary>
        /// <param name="path">
        ///     The path to the file or directory to be deleted.
        /// </param>
        /// <param name="seconds">
        ///     The time to wait in seconds.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process WaitThenDelete(string path, int seconds = 5, bool runAsAdmin = false, bool dispose = true)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;
            var src = PathEx.Combine(path);
            if (!PathEx.DirOrFileExists(src))
                return null;
            var time = seconds < 1 ? 1 : seconds > 3600 ? 3600 : seconds;
            var command = (PathEx.IsDir(src) ? "RMDIR /S /Q \"{0}\"" : "DEL /F /Q \"{0}\"").FormatCurrent(src);
            return WaitThenCmd(command, time, runAsAdmin, dispose);
        }

        /// <summary>
        ///     Waits for the specified seconds to delete the target at the specified path.
        /// </summary>
        /// <param name="path">
        ///     The path to the file or directory to be deleted.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process WaitThenDelete(string path, bool runAsAdmin, bool dispose = true) =>
            WaitThenDelete(path, 5, runAsAdmin, dispose);

        /// <summary>
        ///     Executes the specified command if there is no process running that is
        ///     matched with the specified process name.
        ///     <para>
        ///         If a matched process is still running, the task will wait until all
        ///         matched processes has been closed.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="processName">
        ///     The name of the process to be waited.
        /// </param>
        /// <param name="extension">
        ///     The file extension of the specified process.
        /// </param>
        /// <param name="seconds">
        ///     The amount of time, in seconds, to wait for the associated process to exit.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process WaitForExitThenCmd(string command, string processName, string extension, int seconds = 0, bool runAsAdmin = false, bool dispose = true)
        {
            if (string.IsNullOrWhiteSpace(command) || string.IsNullOrWhiteSpace(processName))
                return null;
            var name = processName;
            if (!string.IsNullOrEmpty(extension) && !name.EndsWithEx(extension))
                name += extension;
            return Send($"(FOR /L %X in (1,{(seconds < 1 ? "0,2" : $"1,{seconds}")}) DO (PING -n 2 LOCALHOST >NUL & TASKLIST | FIND /I \"{name}\" || ({command} & EXIT /B))) & EXIT /B", runAsAdmin, dispose);
        }

        /// <summary>
        ///     Executes the specified command if there is no process running that is
        ///     matched with the specified process name.
        ///     <para>
        ///         If a matched process is still running, the task will wait until all
        ///         matched processes has been closed.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="processName">
        ///     The name of the process to be waited.
        /// </param>
        /// <param name="seconds">
        ///     The amount of time, in seconds, to wait for the associated process to exit.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process WaitForExitThenCmd(string command, string processName, int seconds = 0, bool runAsAdmin = false, bool dispose = true) =>
            WaitForExitThenCmd(command, processName, ".exe", seconds, runAsAdmin, dispose);

        /// <summary>
        ///     Executes the specified command if there is no process running that is
        ///     matched with the specified process name.
        ///     <para>
        ///         If a matched process is still running, the task will wait until all
        ///         matched processes has been closed.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="processName">
        ///     The name of the process to be waited.
        /// </param>
        /// <param name="extension">
        ///     The file extension of the specified process.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process WaitForExitThenCmd(string command, string processName, string extension, bool runAsAdmin, bool dispose = true) =>
            WaitForExitThenCmd(command, processName, extension, 0, runAsAdmin, dispose);

        /// <summary>
        ///     Executes the specified command if there is no process running that is
        ///     matched with the specified process name.
        ///     <para>
        ///         If a matched process is still running, the task will wait until all
        ///         matched processes has been closed.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="processName">
        ///     The name of the process to be waited.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process WaitForExitThenCmd(string command, string processName, bool runAsAdmin, bool dispose = true) =>
            WaitForExitThenCmd(command, processName, ".exe", 0, runAsAdmin, dispose);

        /// <summary>
        ///     Deletes the target at the specified path if there is no process running
        ///     that is matched with the specified process name.
        ///     <para>
        ///         If a matched process is still running, the task will wait until all
        ///         matched processes has been closed.
        ///     </para>
        /// </summary>
        /// <param name="path">
        ///     The path to the file or directory to be deleted.
        /// </param>
        /// <param name="processName">
        ///     The name of the process to be waited.
        /// </param>
        /// <param name="extension">
        ///     The file extension of the specified process.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process WaitForExitThenDelete(string path, string processName, string extension, bool runAsAdmin = false, bool dispose = true)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(processName))
                return null;
            var src = PathEx.Combine(path);
            if (!PathEx.DirOrFileExists(src))
                return null;
            var command = (PathEx.IsDir(src) ? "RMDIR /S /Q \"{0}\"" : "DEL /F /Q \"{0}\"").FormatCurrent(src);
            return WaitForExitThenCmd(command, processName, extension, 0, runAsAdmin, dispose);
        }

        /// <summary>
        ///     Deletes the target at the specified path if there is no process running
        ///     that is matched with the specified process name.
        ///     <para>
        ///         If a matched process is still running, the task will wait until all
        ///         matched processes has been closed.
        ///     </para>
        /// </summary>
        /// <param name="path">
        ///     The path to the file or directory to be deleted.
        /// </param>
        /// <param name="processName">
        ///     The name of the process to be waited.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process WaitForExitThenDelete(string path, string processName, bool runAsAdmin = false, bool dispose = true) =>
            WaitForExitThenDelete(path, processName, ".exe", runAsAdmin, dispose);

        /// <summary>
        ///     Ends all processes matched by the specified process name.
        /// </summary>
        /// <param name="processName">
        ///     The name of the process to be killed.
        /// </param>
        /// <param name="extension">
        ///     The file extension of the specified process.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process KillTask(string processName, string extension, bool runAsAdmin = false, bool dispose = true)
        {
            if (string.IsNullOrWhiteSpace(processName))
                return null;
            var name = processName;
            if (!string.IsNullOrEmpty(extension) && !name.EndsWithEx(extension))
                name += extension;
            return Send($"TASKKILL /F /IM \"{name}\"", runAsAdmin, dispose);
        }

        /// <summary>
        ///     Ends all processes matched by the specified process name.
        /// </summary>
        /// <param name="processName">
        ///     The name of the process to be killed.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process KillTask(string processName, bool runAsAdmin = false, bool dispose = true) =>
            KillTask(processName, ".exe", runAsAdmin, dispose);

        /// <summary>
        ///     Ends all processes matched by all the specified process names.
        /// </summary>
        /// <param name="processNames">
        ///     A list of the process names to be killed.
        /// </param>
        /// <param name="extension">
        ///     The file extension of the specified processes.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process KillAllTasks(IEnumerable<string> processNames, string extension, bool runAsAdmin = false, bool dispose = true)
        {
            if (processNames == null)
                return null;
            var names = processNames.Where(Comparison.IsNotEmpty);
            if (!string.IsNullOrWhiteSpace(extension))
                names = names.Select(x => !x.EndsWithEx(extension) ? x + extension : x);
            var command = $"TASKKILL /F /IM \"{names.Join("\" && TASKKILL /F /IM \"")}\"";
            return Send(command, runAsAdmin, dispose);
        }

        /// <summary>
        ///     Ends all processes matched by all the specified process names.
        /// </summary>
        /// <param name="processNames">
        ///     A list of the process names to be killed.
        /// </param>
        /// <param name="runAsAdmin">
        ///     <see langword="true"/> to run this task with administrator privileges;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if this task has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process KillAllTasks(IEnumerable<string> processNames, bool runAsAdmin = false, bool dispose = true) =>
            KillAllTasks(processNames, ".exe", runAsAdmin, dispose);
    }
}
