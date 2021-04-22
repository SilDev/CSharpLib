#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ProcessEx.cs
// Version:  2021-04-22 19:46
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
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
    using System.Threading;
    using Microsoft.Win32.SafeHandles;
    using Properties;

    /// <summary>
    ///     Provides static methods based on the <see cref="Process"/> class to enable
    ///     you to start local system processes.
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
                    using var tmpId = new PerformanceCounter("Process", "ID Process", parentName);
                    if ((int)tmpId.NextValue() == childId)
                        break;
                }
                using var parentId = new PerformanceCounter("Process", "Creating Process ID", parentName);
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
        ///     specified name/path is <see langword="null"/>, all running processes are
        ///     returned.
        /// </summary>
        /// <param name="nameOrPath">
        ///     The filename or the full path to the application to check.
        /// </param>
        /// <param name="doubleTap">
        ///     <see langword="true"/> to try to get firstly by the path, then by name;
        ///     otherwise, <see langword="false"/>.
        ///     <para>
        ///         Please note that this option has no effect if the first parameter
        ///         contains only a name.
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
        ///     application. If the specified name/path is <see langword="null"/>, the
        ///     number of all running processes is returned.
        /// </summary>
        /// <param name="nameOrPath">
        ///     The filename or the full path to the application to check.
        /// </param>
        /// <param name="doubleTap">
        ///     <see langword="true"/> to try to check firstly by the path, then by name;
        ///     otherwise, <see langword="false"/>.
        ///     <para>
        ///         Please note that this option has no effect if the first parameter
        ///         contains only a name.
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
        ///     <see langword="true"/> to try to check firstly by the path, then by name;
        ///     otherwise, <see langword="false"/>.
        ///     <para>
        ///         Please note that this option has no effect if the first parameter
        ///         contains only a name.
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
                var path = modules.First(m => Path.GetFileName(m.FileName).EqualsEx("SbieDll.dll")).FileName;
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
                using var objs = new ManagementObjectSearcher(query);
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
        ///     <see cref="Process"/>.StartInfo property of this <see cref="Process"/> and
        ///     associates it with the component.
        ///     <para>
        ///         If the <see cref="Process"/>.StartInfo.WorkingDirectory parameter is
        ///         undefined, it is created by <see cref="Process"/>.StartInfo.FileName
        ///         parameter.
        ///     </para>
        /// </summary>
        /// <param name="process">
        ///     The <see cref="Process"/> component to start.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if the process has been started;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static Process Start(Process process, bool dispose = true)
        {
            try
            {
                if (process == null)
                    throw new ArgumentNullException(nameof(process));
                process.StartInfo.FileName = PathEx.Combine(process.StartInfo.FileName);
                if (string.IsNullOrEmpty(process.StartInfo.FileName))
                    throw new NullReferenceException();
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
                            throw new NullReferenceException();
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
                                    WinApi.ThrowLastError(ExceptionMessages.PseudoHandleNotFound);
                                if (!WinApi.NativeMethods.OpenProcessToken(pseudoHandle, 0x20, out tokenHandle))
                                    WinApi.ThrowLastError(ExceptionMessages.PseudoHandleTokenAccess);
                                var newLuId = new WinApi.LuId();
                                if (!WinApi.NativeMethods.LookupPrivilegeValue(null, "SeIncreaseQuotaPrivilege", ref newLuId))
                                    WinApi.ThrowLastError(ExceptionMessages.PrivilegeValueAccess);
                                var newState = new WinApi.TokenPrivileges
                                {
                                    Privileges = new[]
                                    {
                                        new WinApi.LuIdAndAttributes
                                        {
                                            Attributes = 0x2,
                                            Luid = newLuId
                                        }
                                    }
                                };
                                if (!WinApi.NativeHelper.AdjustTokenPrivileges(tokenHandle, false, ref newState))
                                    WinApi.ThrowLastError(ExceptionMessages.TokenPrivilegesAdjustment);
                            }
                            finally
                            {
                                WinApi.NativeMethods.CloseHandle(tokenHandle);
                            }
                            var shellWindow = WinApi.NativeMethods.GetShellWindow();
                            if (shellWindow == IntPtr.Zero)
                                throw new NullReferenceException();
                            var shellHandle = IntPtr.Zero;
                            var shellToken = IntPtr.Zero;
                            var primaryToken = IntPtr.Zero;
                            try
                            {
                                if (WinApi.NativeMethods.GetWindowThreadProcessId(shellWindow, out var pid) <= 0)
                                    WinApi.ThrowLastError(ExceptionMessages.ShellPidNotFound);
                                shellHandle = WinApi.NativeMethods.OpenProcess(WinApi.AccessRights.ProcessQueryInformation, false, pid);
                                if (shellHandle == IntPtr.Zero)
                                    WinApi.ThrowLastError(ExceptionMessages.ShellProcessAccess);
                                if (!WinApi.NativeMethods.OpenProcessToken(shellHandle, 0x2, out shellToken))
                                    WinApi.ThrowLastError(ExceptionMessages.ShellProcessTokenAccess);
                                if (!WinApi.NativeMethods.DuplicateTokenEx(shellToken, 0x18bu, IntPtr.Zero, WinApi.SecurityImpersonationLevels.SecurityImpersonation, WinApi.TokenTypes.TokenPrimary, out primaryToken))
                                    WinApi.ThrowLastError(ExceptionMessages.ShellProcessTokenDuplication);
                                var startupInfo = new WinApi.StartupInfo();
                                if (!WinApi.NativeMethods.CreateProcessWithTokenW(primaryToken, 0, process.StartInfo.FileName, process.StartInfo.Arguments, 0, IntPtr.Zero, process.StartInfo.WorkingDirectory, ref startupInfo, out var processInformation))
                                    WinApi.ThrowLastError(ExceptionMessages.NoProcessWithDuplicatedToken);
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
        ///     Initializes a new instance of the <see cref="Process"/> class from the
        ///     specified <see cref="ProcessStartInfo"/> and starts (or reuses) the process
        ///     component.
        ///     <para>
        ///         If WorkingDirectory parameter is undefined, it is created by the
        ///         FileName parameter.
        ///     </para>
        /// </summary>
        /// <param name="processStartInfo">
        ///     The <see cref="ProcessStartInfo"/> component to initialize a new
        ///     <see cref="Process"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if the process has been started;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static Process Start(ProcessStartInfo processStartInfo, bool dispose = true)
        {
            var process = new Process { StartInfo = processStartInfo };
            return Start(process, dispose);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the
        ///     specified parameters and starts (or reuses) the process component.
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
        ///     Initializes a new instance of the <see cref="Process"/> class from the
        ///     specified parameters and starts (or reuses) the process component.
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
        ///     <see langword="true"/> to start the application with administrator
        ///     privileges; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if the process has been started;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static Process Start(string fileName, string workingDirectory, string arguments, bool verbRunAs, bool dispose) =>
            Start(fileName, workingDirectory, arguments, verbRunAs, ProcessWindowStyle.Normal, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the
        ///     specified parameters and starts (or reuses) the process component.
        /// </summary>
        /// <param name="fileName">
        ///     The application to start.
        /// </param>
        /// <param name="arguments">
        ///     The command-line arguments to use when starting the application.
        /// </param>
        /// <param name="verbRunAs">
        ///     <see langword="true"/> to start the application with administrator
        ///     privileges; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="processWindowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component if the process has been started; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Process Start(string fileName, string arguments = null, bool verbRunAs = false, ProcessWindowStyle processWindowStyle = ProcessWindowStyle.Normal, bool dispose = true) =>
            Start(fileName, null, arguments, verbRunAs, processWindowStyle, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the
        ///     specified parameters and starts (or reuses) the process component.
        /// </summary>
        /// <param name="fileName">
        ///     The application to start.
        /// </param>
        /// <param name="arguments">
        ///     The command-line arguments to use when starting the application.
        /// </param>
        /// <param name="verbRunAs">
        ///     <see langword="true"/> to start the application with administrator
        ///     privileges; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if the process has been started;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public static Process Start(string fileName, string arguments, bool verbRunAs, bool dispose) =>
            Start(fileName, null, arguments, verbRunAs, ProcessWindowStyle.Normal, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the
        ///     specified parameters and starts (or reuses) the process component.
        /// </summary>
        /// <param name="fileName">
        ///     The application to start.
        /// </param>
        /// <param name="verbRunAs">
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
        public static Process Start(string fileName, bool verbRunAs, ProcessWindowStyle processWindowStyle = ProcessWindowStyle.Normal, bool dispose = true) =>
            Start(fileName, null, null, verbRunAs, processWindowStyle, dispose);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Process"/> class from the
        ///     specified parameters to starts (or reuses) the process component.
        /// </summary>
        /// <param name="fileName">
        ///     The application to start.
        /// </param>
        /// <param name="verbRunAs">
        ///     <see langword="true"/> to start the application with administrator
        ///     privileges; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component, if the process has been started;
        ///     otherwise, <see langword="false"/>.
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
            var handles = new HashSet<IntPtr>();
            var threads = new WinApi.EnumThreadWndProc((hWnd, _) =>
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
                            using var wh = new ManualResetEvent(false)
                            {
                                SafeWaitHandle = new SafeWaitHandle(h, false)
                            };
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
        ///         If the current process doesn't have enough privileges to stop a
        ///         specified process it starts an invisible elevated instance of the
        ///         command prompt to run taskkill.
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
            var items = new HashSet<string>();
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
                if (string.IsNullOrEmpty(name))
                    continue;
                items.Add(name);
            }
            if (!items.Any())
                return count > 0;
            using (var p = CmdExec.KillAllTasks(items, true, false))
                if (p?.HasExited ?? false)
                    p.WaitForExit();
            Tray.RefreshAsync(16);
            return count > 0;
        }

        /// <summary>
        ///     Immediately stops all specified processes.
        ///     <para>
        ///         If the current process doesn't have enough privileges to stop a
        ///         specified process it starts an invisible elevated instance of the
        ///         command prompt to run taskkill.
        ///     </para>
        /// </summary>
        /// <param name="processes">
        ///     The collection of processes to kill.
        /// </param>
        public static bool Terminate(params Process[] processes) =>
            Terminate(processes.ToList());
    }
}
