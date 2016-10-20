#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Service.cs
// Version:  2016-10-18 23:33
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting;
    using System.Threading;

    /// <summary>
    ///     Provides static methods to control service applications.
    /// </summary>
    public static class Service
    {
        /// <summary>
        ///     Provides enumerated values of service states.
        /// </summary>
        public enum State
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

        private static IntPtr OpenServiceControlManager(WinApi.ServiceManagerAccessRights serviceRights)
        {
            var scman = WinApi.SafeNativeMethods.OpenSCManager(null, null, serviceRights);
            try
            {
                if (scman == IntPtr.Zero)
                    throw new Exception("Could not connect to service control manager.");
            }
            catch (Exception ex)
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
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.SC_MANAGER_CONNECT | WinApi.ServiceManagerAccessRights.SC_MANAGER_CREATE_SERVICE);
            try
            {
                if (path.Any(char.IsWhiteSpace))
                    path = $"\"{path}\"";
                if (!string.IsNullOrWhiteSpace(args))
                    args = " " + args;
                var service = WinApi.SafeNativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.SERVICE_QUERY_STATUS | WinApi.ServiceAccessRights.SERVICE_START);
                if (service == IntPtr.Zero)
                    service = WinApi.SafeNativeMethods.CreateService(scman, serviceName, displayName, WinApi.ServiceAccessRights.SERVICE_QUERY_STATUS | WinApi.ServiceAccessRights.SERVICE_START, WinApi.ServiceTypes.SERVICE_WIN32_OWN_PROCESS, WinApi.ServiceBootFlag.SERVICE_AUTO_START, WinApi.ServiceError.SERVICE_ERROR_NORMAL, path + args, null, IntPtr.Zero, null, null, null);
                if (service == IntPtr.Zero)
                    throw new Exception("Failed to install service.");
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                WinApi.SafeNativeMethods.CloseServiceHandle(scman);
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
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.SC_MANAGER_CONNECT);
            try
            {
                var service = WinApi.SafeNativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.SERVICE_STANDARD_REQUIRED | WinApi.ServiceAccessRights.SERVICE_STOP | WinApi.ServiceAccessRights.SERVICE_QUERY_STATUS);
                if (service == IntPtr.Zero)
                    throw new Exception("Service not installed.");
                try
                {
                    Stop(service);
                    var ret = WinApi.SafeNativeMethods.DeleteService(service);
                    if (ret != 0)
                        return;
                    var error = Marshal.GetLastWin32Error();
                    throw new Exception("Could not delete service: '" + error + "'");
                }
                finally
                {
                    WinApi.SafeNativeMethods.CloseServiceHandle(service);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                WinApi.SafeNativeMethods.CloseServiceHandle(scman);
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
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.SC_MANAGER_CONNECT);
            try
            {
                var service = WinApi.SafeNativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.SERVICE_QUERY_STATUS);
                if (service == IntPtr.Zero)
                    return false;
                WinApi.SafeNativeMethods.CloseServiceHandle(service);
                return true;
            }
            finally
            {
                WinApi.SafeNativeMethods.CloseServiceHandle(scman);
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
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.SC_MANAGER_CONNECT);
            try
            {
                var hService = WinApi.SafeNativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.SERVICE_QUERY_STATUS | WinApi.ServiceAccessRights.SERVICE_START);
                if (hService == IntPtr.Zero)
                    throw new Exception("Could not open service.");
                try
                {
                    Start(hService);
                }
                finally
                {
                    WinApi.SafeNativeMethods.CloseServiceHandle(hService);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                WinApi.SafeNativeMethods.CloseServiceHandle(scman);
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
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.SC_MANAGER_CONNECT);
            try
            {
                var hService = WinApi.SafeNativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.SERVICE_QUERY_STATUS | WinApi.ServiceAccessRights.SERVICE_STOP);
                if (hService == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");
                try
                {
                    Stop(hService);
                }
                finally
                {
                    WinApi.SafeNativeMethods.CloseServiceHandle(hService);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                WinApi.SafeNativeMethods.CloseServiceHandle(scman);
            }
        }

        private static void Start(IntPtr hService)
        {
            WinApi.SafeNativeMethods.StartService(hService, 0, 0);
            WaitForStatus(hService, WinApi.ServiceState.SERVICE_START_PENDING, WinApi.ServiceState.SERVICE_RUNNING);
        }

        private static void Stop(IntPtr hService)
        {
            var status = new WinApi.SERVICE_STATUS();
            WinApi.SafeNativeMethods.ControlService(hService, WinApi.ControlServiceFunc.SERVICE_CONTROL_STOP, status);
            WaitForStatus(hService, WinApi.ServiceState.SERVICE_STOP_PENDING, WinApi.ServiceState.SERVICE_STOPPED);
        }

        /// <summary>
        ///     Returns the current <see cref="State"/> of an existing service.
        /// </summary>
        /// <param name="serviceName">
        ///     The name of the service to check.
        /// </param>
        public static State GetStatus(string serviceName)
        {
            var scman = OpenServiceControlManager(WinApi.ServiceManagerAccessRights.SC_MANAGER_CONNECT);
            try
            {
                var hService = WinApi.SafeNativeMethods.OpenService(scman, serviceName, WinApi.ServiceAccessRights.SERVICE_QUERY_STATUS);
                if (hService == IntPtr.Zero)
                    return State.NotFound;
                try
                {
                    return GetStatus(hService);
                }
                finally
                {
                    WinApi.SafeNativeMethods.CloseServiceHandle(scman);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                WinApi.SafeNativeMethods.CloseServiceHandle(scman);
            }
            return State.NotFound;
        }

        private static State GetStatus(IntPtr hService)
        {
            var ssStatus = new WinApi.SERVICE_STATUS();
            try
            {
                if (WinApi.SafeNativeMethods.QueryServiceStatus(hService, ssStatus) == 0)
                    throw new ServerException("Failed to query service status.");
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return (State)ssStatus.dwCurrentState;
        }

        private static void WaitForStatus(IntPtr hService, WinApi.ServiceState waitStatus, WinApi.ServiceState desiredStatus)
        {
            var status = new WinApi.SERVICE_STATUS();
            try
            {
                WinApi.SafeNativeMethods.QueryServiceStatus(hService, status);
                if (status.dwCurrentState == desiredStatus)
                    return;
                var dwStartTickCount = Environment.TickCount;
                var dwOldCheckPoint = status.dwCheckPoint;
                while (status.dwCurrentState == waitStatus)
                {
                    var dwWaitTime = status.dwWaitHint / 10;
                    dwWaitTime = dwWaitTime < 1000 ? 1000 : dwWaitTime > 10000 ? 10000 : dwWaitTime;
                    Thread.Sleep(dwWaitTime);
                    if (WinApi.SafeNativeMethods.QueryServiceStatus(hService, status) == 0)
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
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }
}
