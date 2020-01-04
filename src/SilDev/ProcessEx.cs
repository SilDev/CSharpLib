#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ProcessEx.cs
// Version:  2020-01-04 12:49
// 
// Copyright (c) 2020, Si13n7 Developments (r)
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
    using System.Management;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using Intern;
    using Microsoft.Win32.SafeHandles;
    using Properties;

    /// <summary>
    ///     Provides static methods based on the <see cref="Process"/> class to enable you to start
    ///     local system processes.
    /// </summary>
    public static class ProcessEx
    {
        private static Process _current;

        /// <summary>
        ///     Gets the currently active process.
        /// </summary>
        public static Process Current
        {
            get
            {
                if (_current != default)
                    return _current;
                _current = Process.GetCurrentProcess();
                return _current;
            }
        }

        /// <summary>
        ///     Gets the handle of the current process instance.
        /// </summary>
        public static IntPtr CurrentHandle =>
            Current.Handle;

        /// <summary>
        ///     Gets the unique identifier of the current process instance.
        /// </summary>
        public static int CurrentId =>
            Current.Id;

        /// <summary>
        ///     Gets the name of the current process instance.
        /// </summary>
        public static string CurrentName =>
            Current.ProcessName;

        /// <summary>
        ///     Gets the parent process of the current process instance.
        /// </summary>
        public static Process CurrentParent =>
            Current.GetParent();

        /// <summary>
        ///     Gets the parent process of this <see cref="Process"/>.
        /// </summary>
        /// <param name="process">
        ///     The <see cref="Process"/> component.
        /// </param>
        public static Process GetParent(this Process process)
        {
            try
            {
                if (process == null)
                    throw new ArgumentNullException(nameof(process));
                var childId = process.Id;
                var childName = Process.GetProcessById(childId).ProcessName;
                var processes = Process.GetProcessesByName(childName);
                string parentName = null;
                for (var i = 0; i < processes.Length; i++)
                {
                    parentName = i == 0 ? childName : $"{childName}#{i}";
                    using (var tmpId = new PerformanceCounter("Process", "ID Process", parentName))
                        if ((int)tmpId.NextValue() == childId)
                            break;
                }
                using (var parentId = new PerformanceCounter("Process", "Creating Process ID", parentName))
                    return Process.GetProcessById((int)parentId.NextValue());
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Sets the parent process of this <see cref="Process"/>.
        /// </summary>
        /// <param name="process">
        ///     The child <see cref="Process"/> component.
        /// </param>
        /// <param name="newParent">
        ///     The new parent <see cref="Process"/> component.
        /// </param>
        public static bool SetParent(this Process process, Process newParent)
        {
            try
            {
                if (process == null)
                    throw new ArgumentNullException(nameof(process));
                var hWndChild = process.MainWindowHandle;
                if (hWndChild == IntPtr.Zero)
                    hWndChild = process.Handle;
                var hWndNewParent = IntPtr.Zero;
                if (newParent != null)
                {
                    hWndNewParent = newParent.MainWindowHandle;
                    if (hWndNewParent == IntPtr.Zero)
                        hWndNewParent = newParent.Handle;
                }
                if (WinApi.NativeMethods.SetParent(hWndChild, hWndNewParent) != IntPtr.Zero)
                    return true;
                WinApi.ThrowLastError();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Gets all active instances associated with the specified application. If the
        ///     specified name/path is null, all running processes are returned.
        /// </summary>
        /// <param name="nameOrPath">
        ///     The filename or the full path to the application to check.
        /// </param>
        /// <param name="doubleTap">
        ///     true to try to get firstly by the path, then by name; otherwise, false.
        ///     <para>
        ///         Please note that this option has no effect if the first parameter contains
        ///         only a name.
        ///     </para>
        /// </param>
        public static IEnumerable<Process> GetInstances(string nameOrPath, bool doubleTap = false)
        {
            if (nameOrPath != null && string.IsNullOrWhiteSpace(nameOrPath))
                yield break;
            var path = nameOrPath;
            if (path?.StartsWith("\\", StringComparison.Ordinal) ?? false)
                path = path.TrimStart('\t', ' ', '?', '\\');
            path = PathEx.Combine(path);
            var isPath = path.ContainsEx(Path.DirectorySeparatorChar);
            if (isPath && !File.Exists(path))
                yield break;
            if (!isPath && !doubleTap)
                doubleTap = true;
            var name = nameOrPath;
            if (!string.IsNullOrEmpty(name))
                name = path.EndsWithEx(".com", ".exe", ".scr") ? Path.GetFileNameWithoutExtension(path) : Path.GetFileName(path);
            foreach (var p in Process.GetProcesses())
            {
                if (name == null)
                {
                    yield return p;
                    continue;
                }
                if (!p.ProcessName.EqualsEx(name))
                    continue;
                var mPath = default(string);
                if (isPath)
                    try
                    {
                        mPath = p.MainModule?.FileName;
                    }
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        if (!(ex is ArgumentException))
                            Log.Write(ex);
                    }
                if (mPath?.EqualsEx(path) ?? isPath && doubleTap || doubleTap)
                    yield return p;
            }
        }

        /// <summary>
        ///     Returns the number of all active instances associated with the specified
        ///     application. If the specified name/path is null, the number of all running
        ///     processes is returned.
        /// </summary>
        /// <param name="nameOrPath">
        ///     The filename or the full path to the application to check.
        /// </param>
        /// <param name="doubleTap">
        ///     true to try to check firstly by the path, then by name; otherwise, false.
        ///     <para>
        ///         Please note that this option has no effect if the first parameter contains
        ///         only a name.
        ///     </para>
        /// </param>
        public static int InstancesCount(string nameOrPath, bool doubleTap = false) =>
            GetInstances(nameOrPath, doubleTap).Count();

        /// <summary>
        ///     Determines whether the specified file is matched with a running process.
        /// </summary>
        /// <param name="nameOrPath">
        ///     The filename or the full path to the application to check.
        /// </param>
        /// <param name="doubleTap">
        ///     true to try to check firstly by the path, then by name; otherwise, false.
        ///     <para>
        ///         Please note that this option has no effect if the first parameter contains
        ///         only a name.
        ///     </para>
        /// </param>
        public static bool IsRunning(string nameOrPath, bool doubleTap = false) =>
            !string.IsNullOrWhiteSpace(nameOrPath) && InstancesCount(nameOrPath, doubleTap) > 0;

        /// <summary>
        ///     Determines whether this <see cref="Process"/> is running in a sandbox
        ///     environment.
        ///     <para>
        ///         Hint: This function supports only the program Sandboxie.
        ///     </para>
        /// </summary>
        /// <param name="process">
        ///     The <see cref="Process"/> to check.
        /// </param>
        public static bool IsSandboxed(this Process process)
        {
            try
            {
                if (process == null)
                    throw new ArgumentNullException(nameof(process));
                var modules = process.Modules.Cast<ProcessModule>().ToList();
                var path = modules.First(m => Path.GetFileName(m.FileName).EqualsEx("SbieDll.dll"))?.FileName;
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    return false;
                var info = FileVersionInfo.GetVersionInfo(path);
                return info.FileDescription.EqualsEx("Sandboxie User Mode DLL");
            }
            catch (Exception ex) when (ex.IsCaught())
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
                if (process == null)
                    throw new ArgumentNullException(nameof(process));
                var list = new List<string>();
                var query = $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}";
                using (var objs = new ManagementObjectSearcher(query))
                    list.AddRange(objs.Get().Cast<ManagementBaseObject>().Select(obj => obj["CommandLine"].ToString()));
                return list.ToArray();
            }
            catch (Exception ex) when (ex.IsCaught())
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
        ///     Starts (or reuses) the process resource that is specified by the current
        ///     <see cref="Process"/>.StartInfo property of this <see cref="Process"/> and associates
        ///     it with the component.
        ///     <para>
        ///         If the <see cref="Process"/>.StartInfo.WorkingDirectory parameter is undefined,
        ///         it is created by <see cref="Process"/>.StartInfo.FileName parameter.
        ///     </para>
        /// </summary>
        /// <param name="process">
        ///     The <see cref="Process"/> component to start.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Process"/> component, if the
        ///     process has been started; otherwise, false.
        /// </param>
        public static Process Start(Process process, bool dispose = true)
        {
            try
            {
                if (process == null)
                    throw new ArgumentNullException(nameof(process));
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
                        var workingDirectory = Path.GetDirectoryName(process.StartInfo.FileName);
                        if (string.IsNullOrEmpty(workingDirectory))
                            throw new ArgumentNullException(nameof(workingDirectory));
                        process.StartInfo.WorkingDirectory = workingDirectory;
                    }
                    if (!process.StartInfo.UseShellExecute && !process.StartInfo.CreateNoWindow && process.StartInfo.WindowStyle == ProcessWindowStyle.Hidden)
                        process.StartInfo.CreateNoWindow = true;
                    var processStarted = false;
                    if (process.StartInfo.Verb.EqualsEx("RunNotAs"))
                    {
                        process.StartInfo.Verb = string.Empty;
                        var processId = 0;
                        try
                        {
                            if (!Elevation.IsAdministrator)
                                throw new NotSupportedException();
                            var tokenHandle = IntPtr.Zero;
                            try
                            {
                                var pseudoHandle = WinApi.NativeMethods.GetCurrentProcess();
                                if (pseudoHandle == IntPtr.Zero)
                                    WinApi.ThrowLastError("The pseudo handle could not be retrieved.");
                                if (!WinApi.NativeMethods.OpenProcessToken(pseudoHandle, 0x20, out tokenHandle))
                                    WinApi.ThrowLastError("Unable to open the token for the pseudo handle.");
                                var newState = new WinApi.TokenPrivileges
                                {
                                    PrivilegeCount = 1,
                                    Privileges = new WinApi.LuIdAndAttributes[1]
                                };
                                if (!WinApi.NativeMethods.LookupPrivilegeValue(null, "SeIncreaseQuotaPrivilege", ref newState.Privileges[0].Luid))
                                    WinApi.ThrowLastError("Privilege value could not be retrieved.");
                                newState.Privileges[0].Attributes = 0x2;
                                if (!WinApi.NativeHelper.AdjustTokenPrivileges(tokenHandle, false, ref newState))
                                    WinApi.ThrowLastError("Unable to adjust the token privileges.");
                            }
                            finally
                            {
                                WinApi.NativeMethods.CloseHandle(tokenHandle);
                            }
                            var shellWindow = WinApi.NativeMethods.GetShellWindow();
                            if (shellWindow == IntPtr.Zero)
                                throw new ArgumentNullException(nameof(shellWindow));
                            var shellHandle = IntPtr.Zero;
                            var shellToken = IntPtr.Zero;
                            var primaryToken = IntPtr.Zero;
                            try
                            {
                                if (WinApi.NativeMethods.GetWindowThreadProcessId(shellWindow, out var pid) <= 0)
                                    WinApi.ThrowLastError("Unable to identifier the shell process.");
                                shellHandle = WinApi.NativeMethods.OpenProcess(WinApi.AccessRights.ProcessQueryInformation, false, pid);
                                if (shellHandle == IntPtr.Zero)
                                    WinApi.ThrowLastError("Unable to open the shell process object.");
                                if (!WinApi.NativeMethods.OpenProcessToken(shellHandle, 0x2, out shellToken))
                                    WinApi.ThrowLastError("Unable to open the shell process token.");
                                if (!WinApi.NativeMethods.DuplicateTokenEx(shellToken, 0x18bu, IntPtr.Zero, WinApi.SecurityImpersonationLevels.SecurityImpersonation, WinApi.TokenTypes.TokenPrimary, out primaryToken))
                                    WinApi.ThrowLastError("Unable to duplicate the shell process token.");
                                var startupInfo = new WinApi.StartupInfo();
                                if (!WinApi.NativeMethods.CreateProcessWithTokenW(primaryToken, 0, process.StartInfo.FileName, process.StartInfo.Arguments, 0, IntPtr.Zero, process.StartInfo.WorkingDirectory, ref startupInfo, out var processInformation))
                                    WinApi.ThrowLastError("Unable to create a new process with the duplicated token.");
                                processId = processInformation.dwProcessId;
                            }
                            finally
                            {
                                WinApi.NativeMethods.CloseHandle(primaryToken);
                                WinApi.NativeMethods.CloseHandle(shellToken);
                                WinApi.NativeMethods.CloseHandle(shellHandle);
                            }
                            processStarted = true;
                        }
                        catch (Exception ex) when (ex.IsCaught())
                        {
                            Log.Write(ex);
                        }
                        if (processStarted && !dispose)
                            return Process.GetProcessById(processId);
                    }
                    if (!processStarted)
                        process.Start();
                }
                return process;
            }
            catch (Exception ex) when (ex.IsCaught())
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
        ///     Initializes a new instance of the <see cref="Process"/> class from the specified
        ///     <see cref="ProcessStartInfo"/> and starts (or reuses) the process component.
        ///     <para>
        ///         If WorkingDirectory parameter is undefined, it is created by the FileName parameter.
        ///     </para>
        /// </summary>
        /// <param name="processStartInfo">
        ///     The <see cref="ProcessStartInfo"/> component to initialize a new <see cref="Process"/>.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Process"/> component, if the
        ///     process has been started; otherwise, false.
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
        ///     true to release all resources used by the <see cref="Process"/> component, if the
        ///     process has been started; otherwise, false.
        /// </param>
        public static Process Start(string fileName, string workingDirectory, string arguments, bool verbRunAs = false, ProcessWindowStyle processWindowStyle = ProcessWindowStyle.Normal, bool dispose = true)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = arguments,
                    FileName = fileName,
                    Verb = verbRunAs ? "RunAs" : string.Empty,
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
        ///     true to release all resources used by the <see cref="Process"/> component, if the
        ///     process has been started; otherwise, false.
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
        ///     true to release all resources used by the <see cref="Process"/> component if the
        ///     process has been started; otherwise, false.
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
        ///     true to release all resources used by the <see cref="Process"/> component, if the
        ///     process has been started; otherwise, false.
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
        ///     true to release all resources used by the <see cref="Process"/> component, if the
        ///     process has been started; otherwise, false.
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
        ///     true to release all resources used by the <see cref="Process"/> component, if the
        ///     process has been started; otherwise, false.
        /// </param>
        public static Process Start(string fileName, bool verbRunAs, bool dispose) =>
            Start(fileName, null, null, verbRunAs, dispose);

        /// <summary>
        ///     Retrieves all thread handles of the specified process.
        /// </summary>
        /// <param name="process">
        ///     The <see cref="Process"/> to get all thread handles.
        /// </param>
        public static IEnumerable<IntPtr> ThreadHandles(this Process process)
        {
            if (process == null)
                return default;
            var handles = new List<IntPtr>();
            var threads = new WinApi.EnumThreadWndProc((hWnd, lParam) =>
            {
                handles.Add(hWnd);
                return true;
            });
            foreach (ProcessThread thread in process.Threads)
                WinApi.NativeMethods.EnumThreadWindows((uint)thread.Id, threads, IntPtr.Zero);
            return handles;
        }

        /// <summary>
        ///     Immediately closes all threads of the specified processes.
        /// </summary>
        /// <param name="processes">
        ///     The <see cref="Process"/>/es to close.
        /// </param>
        /// <param name="waitOnHandle">
        ///     Wait on the handle.
        /// </param>
        public static bool Close(IEnumerable<Process> processes, bool waitOnHandle = true)
        {
            if (processes == null)
                return false;
            var count = 0;
            foreach (var p in processes)
                try
                {
                    using (p)
                    {
                        foreach (var h in p.ThreadHandles())
                        {
                            WinApi.NativeHelper.PostMessage(h, 0x10, IntPtr.Zero, IntPtr.Zero);
                            if (!waitOnHandle)
                                continue;
                            using (var wh = new ManualResetEvent(false) { SafeWaitHandle = new SafeWaitHandle(h, false) })
                                wh.WaitOne(100);
                        }
                        if (p?.HasExited ?? false)
                            count++;
                    }
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
            return count > 0;
        }

        /// <summary>
        ///     Immediately closes all threads of the specified processes.
        /// </summary>
        /// <param name="processes">
        ///     The <see cref="Process"/>/es to close.
        /// </param>
        public static bool Close(params Process[] processes) =>
            Close(processes.ToList());

        /// <summary>
        ///     Immediately stops all specified processes.
        ///     <para>
        ///         If the current process doesn't have enough privileges to stop a specified process
        ///         it starts an invisible elevated instance of the command prompt to run taskkill.
        ///     </para>
        /// </summary>
        /// <param name="processes">
        ///     The <see cref="Process"/>/es to kill.
        /// </param>
        public static bool Terminate(IEnumerable<Process> processes)
        {
            if (processes == null)
                return false;
            var count = 0;
            var list = new List<string>();
            foreach (var p in processes)
            {
                string name = null;
                try
                {
                    using (p)
                    {
                        name = p.ProcessName;
                        if (!p.HasExited)
                        {
                            p.Kill();
                            count++;
                        }
                        if (p.HasExited)
                            continue;
                    }
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                if (string.IsNullOrEmpty(name) || list.ContainsEx(name))
                    continue;
                list.Add(name);
            }
            if (list.Count == 0)
                return count > 0;
            using (var p = SendHelper.KillAllTasks(list, true, false))
                if (p?.HasExited ?? false)
                    p.WaitForExit();
            Tray.RefreshAsync(16);
            return count > 0;
        }

        /// <summary>
        ///     Immediately stops all specified processes.
        ///     <para>
        ///         If the current process doesn't have enough privileges to stop a specified process
        ///         it starts an invisible elevated instance of the command prompt to run taskkill.
        ///     </para>
        /// </summary>
        /// <param name="processes">
        ///     The collection of processes to kill.
        /// </param>
        public static bool Terminate(params Process[] processes) =>
            Terminate(processes.ToList());

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class to execute system
        ///     commands using the system command prompt ("cmd.exe").
        ///     <para>
        ///         This can be useful for an unprivileged application as a simple way to execute a
        ///         command with the highest user permissions, for example.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="runAsAdmin">
        ///     true to start the application with administrator privileges; otherwise, false.
        /// </param>
        /// <param name="processWindowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Process"/> component, if the
        ///     process has been started; otherwise, false.
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
                    throw new ArgumentNullException(nameof(cmd));
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
                    sb.AppendFormat(CultureConfig.GlobalCultureInfo, "DEL /F /Q \"{0}\"", file);
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
                var p = Start(psi, dispose);
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
        ///     Initializes a new instance of the <see cref="Process"/> class to execute system
        ///     commands using the system command prompt ("cmd.exe").
        ///     <para>
        ///         This can be useful for an unprivileged application as a simple way to execute a
        ///         command with the highest user permissions, for example.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="runAsAdmin">
        ///     true to start the application with administrator privileges; otherwise, false.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Process"/> component, if the
        ///     process has been started; otherwise, false.
        /// </param>
        public static Process Send(string command, bool runAsAdmin, bool dispose) =>
            Send(command, runAsAdmin, ProcessWindowStyle.Hidden, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class to execute system
        ///     commands using the system command prompt ("cmd.exe").
        ///     <para>
        ///         This can be useful for an unprivileged application as a simple way to execute a
        ///         command with the highest user permissions, for example.
        ///     </para>
        /// </summary>
        /// <param name="command">
        ///     The command to execute.
        /// </param>
        /// <param name="processWindowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     true to release all resources used by the <see cref="Process"/> component, if the
        ///     process has been started; otherwise, false.
        /// </param>
        public static Process Send(string command, ProcessWindowStyle processWindowStyle, bool dispose = true) =>
            Send(command, false, processWindowStyle, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class to execute system commands
        ///     using the system command prompt ("cmd.exe") and stream its output to a console, if
        ///     available.
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
                using (var p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        FileName = path,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        Verb = Elevation.IsAdministrator ? "runas" : string.Empty
                    }
                })
                {
                    p.Start();
                    foreach (var s in commands)
                        p.StandardInput.WriteLine(s);
                    p.StandardInput.Flush();
                    p.StandardInput.Close();
                    Console.WriteLine(p.StandardOutput.ReadToEnd());
                }
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Provides the functionality to handle the current principal name.
        /// </summary>
        public static class CurrentPrincipal
        {
            /// <summary>
            ///     Gets the original name of the current principal.
            ///     <para>
            ///         This variable is only set if <see cref="GetOriginalName"/> was
            ///         previously called.
            ///     </para>
            /// </summary>
            public static string Name { get; private set; }

            private static void GetPointers(out IntPtr offset, out IntPtr buffer)
            {
                var curHandle = Process.GetCurrentProcess().Handle;
                var pebBaseAddress = WinApi.NativeHelper.GetProcessBasicInformation(curHandle).PebBaseAddress;
                var processParameters = Marshal.ReadIntPtr(pebBaseAddress, 4 * IntPtr.Size);
                var unicodeSize = IntPtr.Size * 2;
                offset = processParameters.Increment(new IntPtr(4 * 4 + 5 * IntPtr.Size + unicodeSize + IntPtr.Size + unicodeSize));
                buffer = Marshal.ReadIntPtr(offset, IntPtr.Size);
            }

            /// <summary>
            ///     Retrieves the original name of the current principal.
            /// </summary>
            public static string GetOriginalName()
            {
                if (!string.IsNullOrEmpty(Name))
                    return Name;
                try
                {
                    GetPointers(out var offset, out var buffer);
                    var len = Marshal.ReadInt16(offset);
                    if (string.IsNullOrEmpty(Name))
                        Name = Marshal.PtrToStringUni(buffer, len / 2);
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
                return Name;
            }

            /// <summary>
            ///     Changes the name of the current principal.
            /// </summary>
            /// <param name="newName">
            ///     The new name for the current principal, which cannot be longer than
            ///     the original one.
            /// </param>
            public static void ChangeName(string newName)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(newName))
                        throw new ArgumentNullException(nameof(newName));
                    GetPointers(out var offset, out var buffer);
                    var len = Marshal.ReadInt16(offset);
                    if (string.IsNullOrEmpty(Name))
                        Name = Marshal.PtrToStringUni(buffer, len / 2);
                    var principalDir = Path.GetDirectoryName(Name);
                    if (string.IsNullOrEmpty(principalDir))
                        throw new PathNotFoundException(principalDir);
                    var newPrincipalName = Path.Combine(principalDir, newName);
                    if (newPrincipalName.Length > Name.Length)
                        throw new ArgumentException(ExceptionMessages.NewPrincipalNameTooLong);
                    var ptr = buffer;
                    foreach (var c in newPrincipalName)
                    {
                        Marshal.WriteInt16(ptr, c);
                        ptr = ptr.Increment(new IntPtr(2));
                    }
                    Marshal.WriteInt16(ptr, 0);
                    Marshal.WriteInt16(offset, (short)(newPrincipalName.Length * 2));
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
            }

            /// <summary>
            ///     Restores the name of the current principal.
            /// </summary>
            public static void RestoreName()
            {
                try
                {
                    if (string.IsNullOrEmpty(Name))
                        throw new InvalidOperationException();
                    GetPointers(out var offset, out var buffer);
                    foreach (var c in Name)
                    {
                        Marshal.WriteInt16(buffer, c);
                        buffer = buffer.Increment(new IntPtr(2));
                    }
                    Marshal.WriteInt16(buffer, 0);
                    Marshal.WriteInt16(offset, (short)(Name.Length * 2));
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                }
            }
        }

        /// <summary>
        ///     Provides basic functionality based on <see cref="Send(string,bool,bool)"/>.
        /// </summary>
        public static class SendHelper
        {
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="wait">
            ///     true to wait indefinitely for the associated process to exit; otherwise, false.
            /// </param>
            public static bool Copy(string srcPath, string destPath, bool runAsAdmin = false, bool wait = true)
            {
                if (string.IsNullOrWhiteSpace(srcPath) || string.IsNullOrWhiteSpace(destPath))
                    return false;
                var src = PathEx.Combine(srcPath);
                if (!PathEx.DirOrFileExists(src))
                    return false;
                var dest = PathEx.Combine(destPath);
                int exitCode;
                using (var p = Send($"COPY /Y \"{src}\" \"{dest}\"", runAsAdmin, false))
                {
                    if (!wait)
                        return true;
                    if (p?.HasExited ?? false)
                        p.WaitForExit();
                    exitCode = p?.ExitCode ?? 1;
                }
                return exitCode == 0;
            }

            /// <summary>
            ///     Deletes an existing file or directory.
            /// </summary>
            /// <param name="path">
            ///     The path to the file or directory to be deleted.
            /// </param>
            /// <param name="runAsAdmin">
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="wait">
            ///     true to wait indefinitely for the associated process to exit; otherwise, false.
            /// </param>
            public static bool Delete(string path, bool runAsAdmin = false, bool wait = true)
            {
                if (string.IsNullOrWhiteSpace(path))
                    return false;
                var src = PathEx.Combine(path);
                if (!PathEx.DirOrFileExists(src))
                    return true;
                int exitCode;
                using (var p = Send(string.Format(CultureConfig.GlobalCultureInfo, PathEx.IsDir(src) ? "RMDIR /S /Q \"{0}\"" : "DEL /F /Q \"{0}\"", src), runAsAdmin, false))
                {
                    if (!wait)
                        return true;
                    if (p?.HasExited ?? false)
                        p.WaitForExit();
                    exitCode = p?.ExitCode ?? 1;
                }
                return exitCode == 0;
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
            /// </param>
            public static Process WaitThenDelete(string path, int seconds = 5, bool runAsAdmin = false, bool dispose = true)
            {
                if (string.IsNullOrWhiteSpace(path))
                    return null;
                var src = PathEx.Combine(path);
                if (!PathEx.DirOrFileExists(src))
                    return null;
                var time = seconds < 1 ? 1 : seconds > 3600 ? 3600 : seconds;
                var command = string.Format(CultureConfig.GlobalCultureInfo, PathEx.IsDir(src) ? "RMDIR /S /Q \"{0}\"" : "DEL /F /Q \"{0}\"", src);
                return WaitThenCmd(command, time, runAsAdmin, dispose);
            }

            /// <summary>
            ///     Waits for the specified seconds to delete the target at the specified path.
            /// </summary>
            /// <param name="path">
            ///     The path to the file or directory to be deleted.
            /// </param>
            /// <param name="runAsAdmin">
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
            /// </param>
            public static Process WaitThenDelete(string path, bool runAsAdmin, bool dispose = true) =>
                WaitThenDelete(path, 5, runAsAdmin, dispose);

            /// <summary>
            ///     Executes the specified command if there is no process running that is matched with the
            ///     specified process name.
            ///     <para>
            ///         If a matched process is still running, the task will wait until all matched
            ///         processes has been closed.
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
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
            ///     Executes the specified command if there is no process running that is matched with the
            ///     specified process name.
            ///     <para>
            ///         If a matched process is still running, the task will wait until all matched
            ///         processes has been closed.
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
            /// </param>
            public static Process WaitForExitThenCmd(string command, string processName, int seconds = 0, bool runAsAdmin = false, bool dispose = true) =>
                WaitForExitThenCmd(command, processName, ".exe", seconds, runAsAdmin, dispose);

            /// <summary>
            ///     Executes the specified command if there is no process running that is matched with the
            ///     specified process name.
            ///     <para>
            ///         If a matched process is still running, the task will wait until all matched
            ///         processes has been closed.
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
            /// </param>
            public static Process WaitForExitThenCmd(string command, string processName, string extension, bool runAsAdmin, bool dispose = true) =>
                WaitForExitThenCmd(command, processName, extension, 0, runAsAdmin, dispose);

            /// <summary>
            ///     Executes the specified command if there is no process running that is matched with the
            ///     specified process name.
            ///     <para>
            ///         If a matched process is still running, the task will wait until all matched
            ///         processes has been closed.
            ///     </para>
            /// </summary>
            /// <param name="command">
            ///     The command to execute.
            /// </param>
            /// <param name="processName">
            ///     The name of the process to be waited.
            /// </param>
            /// <param name="runAsAdmin">
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
            /// </param>
            public static Process WaitForExitThenCmd(string command, string processName, bool runAsAdmin, bool dispose = true) =>
                WaitForExitThenCmd(command, processName, ".exe", 0, runAsAdmin, dispose);

            /// <summary>
            ///     Deletes the target at the specified path if there is no process running that is
            ///     matched with the specified process name.
            ///     <para>
            ///         If a matched process is still running, the task will wait until all matched
            ///         processes has been closed.
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
            /// </param>
            public static Process WaitForExitThenDelete(string path, string processName, string extension, bool runAsAdmin = false, bool dispose = true)
            {
                if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(processName))
                    return null;
                var src = PathEx.Combine(path);
                if (!PathEx.DirOrFileExists(src))
                    return null;
                var command = string.Format(CultureConfig.GlobalCultureInfo, PathEx.IsDir(src) ? "RMDIR /S /Q \"{0}\"" : "DEL /F /Q \"{0}\"", src);
                return WaitForExitThenCmd(command, processName, extension, 0, runAsAdmin, dispose);
            }

            /// <summary>
            ///     Deletes the target at the specified path if there is no process running that is
            ///     matched with the specified process name.
            ///     <para>
            ///         If a matched process is still running, the task will wait until all matched
            ///         processes has been closed.
            ///     </para>
            /// </summary>
            /// <param name="path">
            ///     The path to the file or directory to be deleted.
            /// </param>
            /// <param name="processName">
            ///     The name of the process to be waited.
            /// </param>
            /// <param name="runAsAdmin">
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
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
            ///     true to run this task with administrator privileges; otherwise, false.
            /// </param>
            /// <param name="dispose">
            ///     true to release all resources used by the <see cref="Process"/> component, if this
            ///     task has been started; otherwise, false.
            /// </param>
            public static Process KillAllTasks(IEnumerable<string> processNames, bool runAsAdmin = false, bool dispose = true) =>
                KillAllTasks(processNames, ".exe", runAsAdmin, dispose);
        }
    }
}
