#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Service.cs
// Version:  2019-10-22 16:14
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Properties;

    /// <summary>
    ///     Provides enumerated values of service states.
    /// </summary>
    public enum ServiceState
    {
        /// <summary>
        ///     The service continue is pending.
        /// </summary>
        Continuing = 0x5,

        /// <summary>
        ///     The service pause is pending.
        /// </summary>
        Pausing = 0x6,

        /// <summary>
        ///     The service is paused.
        /// </summary>
        Paused = 0x7,

        /// <summary>
        ///     The service is running.
        /// </summary>
        Running = 0x4,

        /// <summary>
        ///     The service is starting.
        /// </summary>
        Starting = 0x2,

        /// <summary>
        ///     The service is stopping.
        /// </summary>
        Stopping = 0x3,

        /// <summary>
        ///     The service is not running.
        /// </summary>
        Stopped = 0x1,

        /// <summary>
        ///     The service could not be found.
        /// </summary>
        NotFound = 0x0,

        /// <summary>
        ///     The service state could not be determined.
        /// </summary>
        Unknown = -0x1
    }

    /// <summary>
    ///     Provides static methods to control service applications.
    /// </summary>
    public static class Service
    {
        private static IntPtr OpenServiceControlManager(WinApi.ServiceManagerAccessRights serviceRights)
        {
            var scman = WinApi.NativeMethods.OpenSCManager(null, null, serviceRights);
            try
            {
                if (scman == IntPtr.Zero)
                    throw new OperationCanceledException(ExceptionMessages.SCManagerConnectionCanceled);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return scman;
        }

        /// <summary>
        ///     Creates a service object.
        /// </summary>
        /// <param name="serviceName">
        ///     The name of the service to install.
        /// </param>
        /// <param name="displayName">
        ///     The display name to be used by user interface programs to identify the service.
        /// </param>
        /// <param name="path">
        ///     The fully qualified path to the service binary file.
        /// </param>
        /// <param name="args">
        ///     The command-line arguments for the service binary file.
        /// </param>
        public static void Install(string serviceName, string displayName, string path, string args = "")
        {
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.Connect | WinApi.ServiceManagerAccessRights.CreateService);
            try
            {
                if (path.Any(char.IsWhiteSpace))
                    path = $"\"{path}\"";
                if (!string.IsNullOrWhiteSpace(args))
                    args = " " + args;
                var service = WinApi.NativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.QueryStatus | WinApi.ServiceAccessRights.Start);
                if (service == IntPtr.Zero)
                    service = WinApi.NativeMethods.CreateService(scman, serviceName, displayName, WinApi.ServiceAccessRights.QueryStatus | WinApi.ServiceAccessRights.Start, WinApi.ServiceTypes.Win32OwnProcess, WinApi.ServiceBootFlags.AutoStart, WinApi.ServiceError.Normal, path + args, null, IntPtr.Zero, null, null, null);
                if (service == IntPtr.Zero)
                    throw new OperationCanceledException(ExceptionMessages.ServiceInstallationCanceled);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                _ = WinApi.NativeMethods.CloseServiceHandle(scman);
            }
        }

        /// <summary>
        ///     Creates a service object.
        /// </summary>
        /// <param name="serviceName">
        ///     The name of the service to install.
        /// </param>
        /// <param name="path">
        ///     The fully qualified path to the service binary file.
        /// </param>
        public static void Install(string serviceName, string path) =>
            Install(serviceName, serviceName, path, string.Empty);

        /// <summary>
        ///     Removes an existing service.
        /// </summary>
        /// <param name="serviceName">
        ///     The name of the service to uninstall.
        /// </param>
        public static void Uninstall(string serviceName)
        {
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.Connect);
            try
            {
                var service = WinApi.NativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.StandardRequired | WinApi.ServiceAccessRights.Stop | WinApi.ServiceAccessRights.QueryStatus);
                if (service == IntPtr.Zero)
                    throw new InvalidOperationException(ExceptionMessages.ServiceNotFound);
                try
                {
                    Stop(service);
                    var ret = WinApi.NativeMethods.DeleteService(service);
                    if (ret != 0)
                        return;
                    var error = Marshal.GetLastWin32Error();
                    throw new OperationCanceledException(ExceptionMessages.ServiceUninstallCanceled + error);
                }
                finally
                {
                    _ = WinApi.NativeMethods.CloseServiceHandle(service);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                _ = WinApi.NativeMethods.CloseServiceHandle(scman);
            }
        }

        /// <summary>
        ///     Determines whether the specified service exists.
        /// </summary>
        /// <param name="serviceName">
        ///     The name of the service to check.
        /// </param>
        public static bool Exists(string serviceName)
        {
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.Connect);
            try
            {
                var service = WinApi.NativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.QueryStatus);
                if (service == IntPtr.Zero)
                    return false;
                _ = WinApi.NativeMethods.CloseServiceHandle(service);
                return true;
            }
            finally
            {
                _ = WinApi.NativeMethods.CloseServiceHandle(scman);
            }
        }

        /// <summary>
        ///     Starts an existing service.
        /// </summary>
        /// <param name="serviceName">
        ///     The name of the service to start.
        /// </param>
        public static void Start(string serviceName)
        {
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.Connect);
            try
            {
                var hService = WinApi.NativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.QueryStatus | WinApi.ServiceAccessRights.Start);
                if (hService == IntPtr.Zero)
                    throw new OperationCanceledException(ExceptionMessages.ServiceStartCanceled);
                try
                {
                    Start(hService);
                }
                finally
                {
                    _ = WinApi.NativeMethods.CloseServiceHandle(hService);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                _ = WinApi.NativeMethods.CloseServiceHandle(scman);
            }
        }

        /// <summary>
        ///     Stops an existing service.
        /// </summary>
        /// <param name="serviceName">
        ///     The name of the service to stop.
        /// </param>
        public static void Stop(string serviceName)
        {
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.Connect);
            try
            {
                var hService = WinApi.NativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.QueryStatus | WinApi.ServiceAccessRights.Stop);
                if (hService == IntPtr.Zero)
                    throw new InvalidOperationException(ExceptionMessages.ServiceAccessFailed);
                try
                {
                    Stop(hService);
                }
                finally
                {
                    _ = WinApi.NativeMethods.CloseServiceHandle(hService);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                _ = WinApi.NativeMethods.CloseServiceHandle(scman);
            }
        }

        private static void Start(IntPtr hService)
        {
            _ = WinApi.NativeMethods.StartService(hService, 0, 0);
            WaitForStatus(hService, WinApi.ServiceStateTypes.StartPending, WinApi.ServiceStateTypes.Running);
        }

        private static void Stop(IntPtr hService)
        {
            var status = new WinApi.ServiceStatus();
            _ = WinApi.NativeMethods.ControlService(hService, WinApi.ServiceControlOptions.Stop, status);
            WaitForStatus(hService, WinApi.ServiceStateTypes.StopPending, WinApi.ServiceStateTypes.Stopped);
        }

        /// <summary>
        ///     Returns the current <see cref="ServiceState"/> of an existing service.
        /// </summary>
        /// <param name="serviceName">
        ///     The name of the service to check.
        /// </param>
        public static ServiceState GetStatus(string serviceName)
        {
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.Connect);
            try
            {
                var hService = WinApi.NativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.QueryStatus);
                if (hService == IntPtr.Zero)
                    return ServiceState.NotFound;
                try
                {
                    return GetStatus(hService);
                }
                finally
                {
                    _ = WinApi.NativeMethods.CloseServiceHandle(scman);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                _ = WinApi.NativeMethods.CloseServiceHandle(scman);
            }
            return ServiceState.NotFound;
        }

        private static ServiceState GetStatus(IntPtr hService)
        {
            var ssStatus = new WinApi.ServiceStatus();
            try
            {
                if (WinApi.NativeMethods.QueryServiceStatus(hService, ssStatus) == 0)
                    throw new OperationCanceledException(ExceptionMessages.ServiceStatusQueryFailed);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return (ServiceState)ssStatus.dwCurrentState;
        }

        private static void WaitForStatus(IntPtr hService, WinApi.ServiceStateTypes waitStatus, WinApi.ServiceStateTypes desiredStatus)
        {
            var status = new WinApi.ServiceStatus();
            try
            {
                _ = WinApi.NativeMethods.QueryServiceStatus(hService, status);
                if (status.dwCurrentState == desiredStatus)
                    return;
                var dwStartTickCount = Environment.TickCount;
                var dwOldCheckPoint = status.dwCheckPoint;
                while (status.dwCurrentState == waitStatus)
                {
                    var dwWaitTime = status.dwWaitHint / 10;
                    dwWaitTime = dwWaitTime < 1000 ? 1000 : dwWaitTime > 10000 ? 10000 : dwWaitTime;
                    Thread.Sleep(dwWaitTime);
                    if (WinApi.NativeMethods.QueryServiceStatus(hService, status) == 0)
                        break;
                    if (status.dwCheckPoint > dwOldCheckPoint)
                    {
                        dwStartTickCount = Environment.TickCount;
                        dwOldCheckPoint = status.dwCheckPoint;
                    }
                    else
                    {
                        if (Environment.TickCount - dwStartTickCount > status.dwWaitHint)
                            break;
                    }
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }
    }
}
