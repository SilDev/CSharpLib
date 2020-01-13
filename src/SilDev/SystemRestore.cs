#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: SystemRestore.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.ComponentModel;
    using System.Management;
    using Microsoft.Win32;
    using Properties;

    /// <summary>
    ///     The type of event. For more information, see
    ///     <see cref="SystemRestore.Create"/>.
    /// </summary>
    public enum RestoreEventType
    {
        /// <summary>
        ///     A system change has begun. A subsequent nested call does not create a new
        ///     restore point.
        ///     <para>
        ///         Subsequent calls must use <see cref="EndNestedSystemChange"/>, not
        ///         <see cref="EndSystemChange"/>.
        ///     </para>
        /// </summary>
        BeginNestedSystemChange = 0x66,

        /// <summary>
        ///     A system change has begun.
        /// </summary>
        BeginSystemChange = 0x64,

        /// <summary>
        ///     A system change has ended.
        /// </summary>
        EndNestedSystemChange = 0x67,

        /// <summary>
        ///     A system change has ended.
        /// </summary>
        EndSystemChange = 0x65
    }

    /// <summary>
    ///     The type of restore point. For more information, see
    ///     <see cref="SystemRestore.Create"/>.
    /// </summary>
    public enum RestorePointType
    {
        /// <summary>
        ///     An application has been installed.
        /// </summary>
        ApplicationInstall = 0x0,

        /// <summary>
        ///     An application has been uninstalled.
        /// </summary>
        ApplicationUninstall = 0x1,

        /// <summary>
        ///     An application needs to delete the restore point it created. For example,
        ///     an application would use this flag when a user cancels an installation.
        /// </summary>
        CancelledOperation = 0xd,

        /// <summary>
        ///     A device driver has been installed.
        /// </summary>
        DeviceDriverInstall = 0xa,

        /// <summary>
        ///     An application has had features added or removed.
        /// </summary>
        ModifySettings = 0xc
    }

    /// <summary>
    ///     Provides functionality to handle system restore points.
    /// </summary>
    public static class SystemRestore
    {
        /// <summary>
        ///     Determines whether the system restoring is enabled.
        /// </summary>
        public static bool IsEnabled =>
            Reg.Read(Registry.LocalMachine, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\SystemRestore", "RPSessionInterval", 0) > 0;

        /// <summary>
        ///     Creates a restore point on the local system.
        /// </summary>
        /// <param name="description">
        ///     The description to be displayed so the user can easily identify a restore
        ///     point.
        /// </param>
        /// <param name="eventType">
        ///     The type of event.
        /// </param>
        /// <param name="restorePointType">
        ///     The type of restore point.
        /// </param>
        public static void Create(string description, RestoreEventType eventType, RestorePointType restorePointType)
        {
            try
            {
                if (!IsEnabled)
                    throw new WarningException(ExceptionMessages.SystemRestoringIsDisabled);
                var mScope = new ManagementScope("\\\\localhost\\root\\default");
                var mPath = new ManagementPath("SystemRestore");
                var options = new ObjectGetOptions();
                using var mClass = new ManagementClass(mScope, mPath, options);
                using var parameters = mClass.GetMethodParameters("CreateRestorePoint");
                parameters["Description"] = description;
                parameters["EventType"] = (int)eventType;
                parameters["RestorePointType"] = (int)restorePointType;
                mClass.InvokeMethod("CreateRestorePoint", parameters, null);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }
    }
}
