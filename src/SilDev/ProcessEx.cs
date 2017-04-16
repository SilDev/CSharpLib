#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ProcessEx.cs
// Version:  2017-04-16 18:01
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Management;

    /// <summary>
    ///     Provides static methods based on the <see cref="Process"/> class to enable you to start
    ///     local system processes.
    /// </summary>
    public static class ProcessEx
    {
        /// <summary>
        ///     Determines whether the specified file is matched with a running process.
        /// </summary>
        /// <param name="nameOrPath">
        ///     The filename without extension or the path to the file to check.
        /// </param>
        public static bool IsRunning(string nameOrPath)
        {
            try
            {
                bool isRunning;
                var path = nameOrPath;
                var name = Path.GetFileNameWithoutExtension(path);
                if (path.Contains("\\") && File.Exists(path))
                    isRunning = Process.GetProcesses().Any(p => p.ProcessName.EqualsEx(name) && p.MainModule.FileName.EqualsEx(path));
                else
                    isRunning = Process.GetProcessesByName(name).Length > 0;
                return isRunning;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     <para>
        ///         Determines whether this <see cref="Process"/> is running in a sandboxed
        ///         environment.
        ///     </para>
        ///     <para>
        ///         Hint: This function supports only Sandboxie.
        ///     </para>
        /// </summary>
        /// <param name="process">
        ///     The <see cref="Process"/> to check.
        /// </param>
        public static bool IsSandboxed(this Process process)
        {
            try
            {
                var modules = process.Modules.Cast<ProcessModule>().ToList();
                var path = modules.First(m => Path.GetFileName(m.FileName).EqualsEx("SbieDll.dll"))?.FileName;
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    return false;
                var info = FileVersionInfo.GetVersionInfo(path);
                return info.FileDescription.EqualsEx("Sandboxie User Mode DLL");
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Returns a string array containing the command-line arguments for this
        ///     <see cref="Process"/>.
        /// </summary>
        /// <param name="process">
        ///     The <see cref="Process"/> component.
        /// </param>
        public static string[] GetCommandLineArgs(this Process process)
        {
            try
            {
                var list = new List<string>();
                var query = "SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id;
                using (var objs = new ManagementObjectSearcher(query))
                    list.AddRange(objs.Get().Cast<ManagementBaseObject>().Select(obj => obj["CommandLine"].ToString()));
                return list.ToArray();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Returns the command-line arguments for this process.
        /// </summary>
        /// <param name="process">
        ///     The <see cref="Process"/> component.
        /// </param>
        public static string GetCommandLine(this Process process) =>
            process.GetCommandLineArgs().Join(' ');

        /// <summary>
        ///     <para>
        ///         Starts (or reuses) the process resource that is specified by the current
        ///         <see cref="Process"/>.StartInfo property of this <see cref="Process"/> and
        ///         associates it with the component.
        ///     </para>
        ///     <para>
        ///         If the <see cref="Process"/>.StartInfo.WorkingDirectory parameter is undefined,
        ///         it is created by <see cref="Process"/>.StartInfo.FileName parameter.
        ///     </para>
        /// </summary>
        /// <param name="process">
        ///     The <see cref="Process"/> component to start.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Start(Process process, bool dispose = true)
        {
            try
            {
                process.StartInfo.FileName = PathEx.Combine(process.StartInfo.FileName);
                if (string.IsNullOrEmpty(process.StartInfo.FileName))
                    throw new ArgumentNullException(nameof(process.StartInfo.FileName));
                if (!File.Exists(process.StartInfo.FileName))
                    throw new PathNotFoundException(process.StartInfo.FileName);
                if (process.StartInfo.FileName.EndsWithEx(".lnk"))
                    process.Start();
                else
                {
                    process.StartInfo.WorkingDirectory = PathEx.Combine(process.StartInfo.WorkingDirectory);
                    if (!Directory.Exists(process.StartInfo.WorkingDirectory))
                    {
                        var path = Path.GetDirectoryName(process.StartInfo.FileName);
                        if (string.IsNullOrEmpty(path))
                            throw new ArgumentNullException(nameof(path));
                        process.StartInfo.WorkingDirectory = path;
                    }
                    if (!process.StartInfo.UseShellExecute && !process.StartInfo.CreateNoWindow && process.StartInfo.WindowStyle == ProcessWindowStyle.Hidden)
                        process.StartInfo.CreateNoWindow = true;
                    process.Start();
                }
                return process;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
            finally
            {
                if (dispose)
                    process?.Dispose();
            }
        }

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of the <see cref="Process"/> class from the specified
        ///         <see cref="ProcessStartInfo"/> and starts (or reuses) the process component.
        ///     </para>
        ///     <para>
        ///         If WorkingDirectory parameter is undefined, it is created by the FileName parameter.
        ///     </para>
        /// </summary>
        /// <param name="processStartInfo">
        ///     The <see cref="ProcessStartInfo"/> component to initialize a new <see cref="Process"/>.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Start(ProcessStartInfo processStartInfo, bool dispose = true)
        {
            var process = new Process { StartInfo = processStartInfo };
            return Start(process, dispose);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the specified
        ///     parameters and starts (or reuses) the process component.
        /// </summary>
        /// <param name="fileName">
        ///     The application to start.
        /// </param>
        /// <param name="workingDirectory">
        ///     The working directory for the process to be started.
        /// </param>
        /// <param name="arguments">
        ///     The command-line arguments to use when starting the application.
        /// </param>
        /// <param name="verbRunAs">
        ///     true to start the application with administrator privileges; otherwise, false.
        /// </param>
        /// <param name="processWindowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Start(string fileName, string workingDirectory, string arguments, bool verbRunAs = false, ProcessWindowStyle processWindowStyle = ProcessWindowStyle.Normal, bool dispose = true)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = arguments,
                    FileName = fileName,
                    Verb = verbRunAs ? "runas" : string.Empty,
                    WindowStyle = processWindowStyle,
                    WorkingDirectory = workingDirectory
                }
            };
            return Start(process, dispose);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the specified
        ///     parameters and starts (or reuses) the process component.
        /// </summary>
        /// <param name="fileName">
        ///     The application to start.
        /// </param>
        /// <param name="workingDirectory">
        ///     The working directory for the process to be started.
        /// </param>
        /// <param name="arguments">
        ///     The command-line arguments to use when starting the application.
        /// </param>
        /// <param name="verbRunAs">
        ///     true to start the application with administrator privileges; otherwise, false.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Start(string fileName, string workingDirectory, string arguments, bool verbRunAs, bool dispose) =>
            Start(fileName, workingDirectory, arguments, verbRunAs, ProcessWindowStyle.Normal, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the specified
        ///     parameters and starts (or reuses) the process component.
        /// </summary>
        /// <param name="fileName">
        ///     The application to start.
        /// </param>
        /// <param name="arguments">
        ///     The command-line arguments to use when starting the application.
        /// </param>
        /// <param name="verbRunAs">
        ///     true to start the application with administrator privileges; otherwise, false.
        /// </param>
        /// <param name="processWindowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Start(string fileName, string arguments = null, bool verbRunAs = false, ProcessWindowStyle processWindowStyle = ProcessWindowStyle.Normal, bool dispose = true) =>
            Start(fileName, null, arguments, verbRunAs, processWindowStyle, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the specified
        ///     parameters and starts (or reuses) the process component.
        /// </summary>
        /// <param name="fileName">
        ///     The application to start.
        /// </param>
        /// <param name="arguments">
        ///     The command-line arguments to use when starting the application.
        /// </param>
        /// <param name="verbRunAs">
        ///     true to start the application with administrator privileges; otherwise, false.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Start(string fileName, string arguments, bool verbRunAs, bool dispose) =>
            Start(fileName, null, arguments, verbRunAs, ProcessWindowStyle.Normal, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the specified
        ///     parameters and starts (or reuses) the process component.
        /// </summary>
        /// <param name="fileName">
        ///     The application to start.
        /// </param>
        /// <param name="verbRunAs">
        ///     true to start the application with administrator privileges; otherwise, false.
        /// </param>
        /// <param name="processWindowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Start(string fileName, bool verbRunAs, ProcessWindowStyle processWindowStyle = ProcessWindowStyle.Normal, bool dispose = true) =>
            Start(fileName, null, null, verbRunAs, processWindowStyle, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the specified
        ///     parameters to starts (or reuses) the process component.
        /// </summary>
        /// <param name="fileName">
        ///     The application to start.
        /// </param>
        /// <param name="verbRunAs">
        ///     true to start the application with administrator privileges; otherwise, false.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Start(string fileName, bool verbRunAs, bool dispose) =>
            Start(fileName, null, null, verbRunAs, dispose);

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of the <see cref="Process"/> class to execute system
        ///         commands using the system command prompt ("cmd.exe").
        ///     </para>
        ///     <para>
        ///         This can be useful for an unprivileged application as a simple way to execute a
        ///         command with the highest user permissions, for example.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The application to start.
        /// </param>
        /// <param name="runAsAdmin">
        ///     true to start the application with administrator privileges; otherwise, false.
        /// </param>
        /// <param name="processWindowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Send(string command, bool runAsAdmin = false, ProcessWindowStyle processWindowStyle = ProcessWindowStyle.Hidden, bool dispose = true)
        {
            try
            {
                var cmd = command.Trim();
                if (cmd.StartsWithEx("/K"))
                    cmd = cmd.Substring(2)
                             .TrimStart();
                if (!cmd.StartsWithEx("/C"))
                    cmd = $"/C {cmd}";
                if (cmd.Length <= 3)
                    throw new ArgumentNullException(nameof(cmd));
                var psi = new ProcessStartInfo
                {
                    Arguments = cmd,
                    FileName = "%System%\\cmd.exe",
                    UseShellExecute = runAsAdmin,
                    Verb = runAsAdmin ? "runas" : string.Empty,
                    WindowStyle = processWindowStyle
                };
                var p = Start(psi, dispose);
                if (Log.DebugMode > 0)
                    Log.Write($"COMMAND EXECUTED: {cmd.Substring(3)}");
                return p;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of the <see cref="Process"/> class to execute system
        ///         commands using the system command prompt ("cmd.exe").
        ///     </para>
        ///     <para>
        ///         This can be useful for an unprivileged application as a simple way to execute a
        ///         command with the highest user permissions, for example.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The application to start.
        /// </param>
        /// <param name="runAsAdmin">
        ///     true to start the application with administrator privileges; otherwise, false.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Send(string command, bool runAsAdmin, bool dispose) =>
            Send(command, runAsAdmin, ProcessWindowStyle.Hidden, dispose);

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of the <see cref="Process"/> class to execute system
        ///         commands using the system command prompt ("cmd.exe").
        ///     </para>
        ///     <para>
        ///         This can be useful for an unprivileged application as a simple way to execute a
        ///         command with the highest user permissions, for example.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The application to start.
        /// </param>
        /// <param name="processWindowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Component.Dispose()"/>
        ///     if the process has been started; otherwise, false.
        /// </param>
        public static Process Send(string command, ProcessWindowStyle processWindowStyle, bool dispose = true) =>
            Send(command, false, processWindowStyle, dispose);

        /// <summary>
        ///     <para>
        ///         Immediately stops all specified processes.
        ///     </para>
        ///     <para>
        ///         If the current process doesn't have enough privileges to stop a specified process
        ///         it starts an invisible elevated instance of the command prompt to run taskkill.
        ///     </para>
        /// </summary>
        /// <param name="processes">
        ///     The <see cref="Process"/>/es to kill.
        /// </param>
        public static bool Terminate(params Process[] processes)
        {
            var count = 0;
            var list = new List<string>();
            foreach (var p in processes)
            {
                try
                {
                    if (!p.HasExited)
                    {
                        count++;
                        p.Kill();
                    }
                    if (p.HasExited)
                        continue;
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
                var s = p.ProcessName + ".exe";
                if (!list.ContainsEx(s))
                    list.Add(s);
            }
            if (list.Count == 0)
                return count > 0;
            using (var p = Send($"TASKKILL /F /IM \"{list.Join("\" && TASKKILL /F /IM \"")}\"", true, false))
                if (p != null && !p.HasExited)
                    p.WaitForExit();
            return count > 0;
        }

        /// <summary>
        ///     <para>
        ///         Immediately stops all specified processes.
        ///     </para>
        ///     <para>
        ///         If the current process doesn't have enough privileges to stop a specified process
        ///         it starts an invisible elevated instance of the command prompt to run taskkill.
        ///     </para>
        /// </summary>
        /// <param name="processes">
        ///     The collection of processes to kill.
        /// </param>
        public static bool Terminate(IEnumerable<Process> processes) =>
            Terminate(processes.ToArray());
    }
}
