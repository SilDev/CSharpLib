#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Service.cs
// Version:  2023-12-05 13:51
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
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
    using static WinApi;

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
        /// <summary>
        ///     Creates a service object.
        /// </summary>
        /// <param name="serviceName">
        ///     The name of the service to install.
        /// </param>
        /// <param name="displayName">
        ///     The display name to be used by user interface programs to identify the
        ///     service.
        /// </param>
        /// <param name="path">
        ///     The fully qualified path to the service binary file.
        /// </param>
        /// <param name="args">
        ///     The command-line arguments for the service binary file.
        /// </param>
        public static void Install(string serviceName, string displayName, string path, string args = "")
        {
            var scman = OpenServiceControlManager(ServiceManagerAccessRights.Connect | ServiceManagerAccessRights.CreateService);
            try
            {
                if (path.Any(char.IsWhiteSpace))
                    path = $"\"{path}\"";
                if (!string.IsNullOrWhiteSpace(args))
                    args = " " + args;
                var service = NativeMethods.OpenService(scman, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Start);
                if (service == IntPtr.Zero)
                    service = NativeMethods.CreateService(scman, serviceName, displayName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Start, ServiceTypes.Win32OwnProcess, ServiceBootFlags.AutoStart, ServiceError.Normal, path + args, null, IntPtr.Zero, null, null, null);
                if (service == IntPtr.Zero)
                    throw new OperationCanceledException(ExceptionMessages.ServiceInstallationCanceled);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                _ = NativeMethods.CloseServiceHandle(scman);
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
            var scman = OpenServiceControlManager(ServiceManagerAccessRights.Connect);
            try
            {
                var service = NativeMethods.OpenService(scman, serviceName, ServiceAccessRights.StandardRequired | ServiceAccessRights.Stop | ServiceAccessRights.QueryStatus);
                if (service == IntPtr.Zero)
                    throw new InvalidOperationException(ExceptionMessages.ServiceNotFound);
                try
                {
                    Stop(service);
                    var ret = NativeMethods.DeleteService(service);
                    if (ret != 0)
                        return;
                    var error = Marshal.GetLastWin32Error();
                    throw new OperationCanceledException(ExceptionMessages.ServiceUninstallCanceled + error);
                }
                finally
                {
                    _ = NativeMethods.CloseServiceHandle(service);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                _ = NativeMethods.CloseServiceHandle(scman);
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
            var scman = OpenServiceControlManager(ServiceManagerAccessRights.Connect);
            try
            {
                var service = NativeMethods.OpenService(scman, serviceName, ServiceAccessRights.QueryStatus);
                if (service == IntPtr.Zero)
                    return false;
                _ = NativeMethods.CloseServiceHandle(service);
                return true;
            }
            finally
            {
                _ = NativeMethods.CloseServiceHandle(scman);
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
            var scman = OpenServiceControlManager(ServiceManagerAccessRights.Connect);
            try
            {
                var hService = NativeMethods.OpenService(scman, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Start);
                if (hService == IntPtr.Zero)
                    throw new OperationCanceledException(ExceptionMessages.ServiceStartCanceled);
                try
                {
                    Start(hService);
                }
                finally
                {
                    _ = NativeMethods.CloseServiceHandle(hService);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                _ = NativeMethods.CloseServiceHandle(scman);
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
            var scman = OpenServiceControlManager(ServiceManagerAccessRights.Connect);
            try
            {
                var hService = NativeMethods.OpenService(scman, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Stop);
                if (hService == IntPtr.Zero)
                    throw new InvalidOperationException(ExceptionMessages.ServiceAccessFailed);
                try
                {
                    Stop(hService);
                }
                finally
                {
                    _ = NativeMethods.CloseServiceHandle(hService);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                _ = NativeMethods.CloseServiceHandle(scman);
            }
        }

        /// <summary>
        ///     Returns the current <see cref="ServiceState"/> of an existing service.
        /// </summary>
        /// <param name="serviceName">
        ///     The name of the service to check.
        /// </param>
        public static ServiceState GetStatus(string serviceName)
        {
            var scman = OpenServiceControlManager(ServiceManagerAccessRights.Connect);
            try
            {
                var hService = NativeMethods.OpenService(scman, serviceName, ServiceAccessRights.QueryStatus);
                if (hService == IntPtr.Zero)
                    return ServiceState.NotFound;
                try
                {
                    return GetStatus(hService);
                }
                finally
                {
                    _ = NativeMethods.CloseServiceHandle(scman);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                _ = NativeMethods.CloseServiceHandle(scman);
            }
            return ServiceState.NotFound;
        }

        private static IntPtr OpenServiceControlManager(ServiceManagerAccessRights serviceRights)
        {
            var scman = NativeMethods.OpenSCManager(null, null, serviceRights);
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

        private static void Start(IntPtr hService)
        {
            _ = NativeMethods.StartService(hService, 0, 0);
            WaitForStatus(hService, ServiceStateTypes.StartPending, ServiceStateTypes.Running);
        }

        private static void Stop(IntPtr hService)
        {
            var status = new ServiceStatus();
            _ = NativeMethods.ControlService(hService, ServiceControlOptions.Stop, status);
            WaitForStatus(hService, ServiceStateTypes.StopPending, ServiceStateTypes.Stopped);
        }

        private static ServiceState GetStatus(IntPtr hService)
        {
            var ssStatus = new ServiceStatus();
            try
            {
                if (NativeMethods.QueryServiceStatus(hService, ssStatus) == 0)
                    throw new OperationCanceledException(ExceptionMessages.ServiceStatusQueryFailed);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return (ServiceState)ssStatus.dwCurrentState;
        }

        private static void WaitForStatus(IntPtr hService, ServiceStateTypes waitStatus, ServiceStateTypes desiredStatus)
        {
            var status = new ServiceStatus();
            try
            {
                _ = NativeMethods.QueryServiceStatus(hService, status);
                if (status.dwCurrentState == desiredStatus)
                    return;
                var dwStartTickCount = Environment.TickCount;
                var dwOldCheckPoint = status.dwCheckPoint;
                while (status.dwCurrentState == waitStatus)
                {
                    var dwWaitTime = status.dwWaitHint / 10;
                    dwWaitTime = dwWaitTime < 1000 ? 1000 : dwWaitTime > 10000 ? 10000 : dwWaitTime;
                    Thread.Sleep(dwWaitTime);
                    if (NativeMethods.QueryServiceStatus(hService, status) == 0)
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
