#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Win32_OperatingSystem.cs
// Version:  2016-10-31 17:36
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.QuickWmi
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Management;

    /// <summary>
    ///     ***This is an undocumented class and can be changed or removed in the future
    ///     without futher notice.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Win32_OperatingSystem
    {
        public static string BootDevice => GetValue(nameof(BootDevice));
        public static string BuildNumber => GetValue(nameof(BuildNumber));
        public static string BuildType => GetValue(nameof(BuildType));
        public static string Caption => GetValue(nameof(Caption));
        public static string CodeSet => GetValue(nameof(CodeSet));
        public static string CountryCode => GetValue(nameof(CountryCode));
        public static string CreationClassName => GetValue(nameof(CreationClassName));
        public static string CSCreationClassName => GetValue(nameof(CSCreationClassName));
        public static string CSDVersion => GetValue(nameof(CSDVersion));
        public static string CSName => GetValue(nameof(CSName));
        public static short? CurrentTimeZone => GetValue(nameof(CurrentTimeZone));
        public static bool? DataExecutionPrevention_Available => GetValue(nameof(DataExecutionPrevention_Available));
        public static bool? DataExecutionPrevention_32BitApplications => GetValue(nameof(DataExecutionPrevention_32BitApplications));
        public static bool? DataExecutionPrevention_Drivers => GetValue(nameof(DataExecutionPrevention_Drivers));
        public static byte? DataExecutionPrevention_SupportPolicy => GetValue(nameof(DataExecutionPrevention_SupportPolicy));
        public static bool? Debug => GetValue(nameof(Debug));
        public static string Description => GetValue(nameof(Description));
        public static bool? Distributed => GetValue(nameof(Distributed));
        public static uint? EncryptionLevel => GetValue(nameof(EncryptionLevel));
        public static byte? ForegroundApplicationBoost => GetValue(nameof(ForegroundApplicationBoost));
        public static ulong? FreePhysicalMemory => GetValue(nameof(FreePhysicalMemory));
        public static ulong? FreeSpaceInPagingFiles => GetValue(nameof(FreeSpaceInPagingFiles));
        public static ulong? FreeVirtualMemory => GetValue(nameof(FreeVirtualMemory));
        public static DateTime? InstallDate => GetValue(nameof(InstallDate), typeof(DateTime));
        public static uint? LargeSystemCache => GetValue(nameof(LargeSystemCache));
        public static DateTime? LastBootUpTime => GetValue(nameof(LastBootUpTime), typeof(DateTime));
        public static DateTime? LocalDateTime => GetValue(nameof(LocalDateTime), typeof(DateTime));
        public static string Locale => GetValue(nameof(Locale));
        public static string Manufacturer => GetValue(nameof(Manufacturer));
        public static uint? MaxNumberOfProcesses => GetValue(nameof(MaxNumberOfProcesses));
        public static ulong? MaxProcessMemorySize => GetValue(nameof(MaxProcessMemorySize));
        public static string[] MUILanguages => GetValue(nameof(MUILanguages));
        public static string Name => GetValue(nameof(Name));
        public static uint? NumberOfLicensedUsers => GetValue(nameof(NumberOfLicensedUsers));
        public static uint? NumberOfProcesses => GetValue(nameof(NumberOfProcesses));
        public static uint? NumberOfUsers => GetValue(nameof(NumberOfUsers));
        public static uint? OperatingSystemSKU => GetValue(nameof(OperatingSystemSKU));
        public static string Organization => GetValue(nameof(Organization));
        public static string OSArchitecture => GetValue(nameof(OSArchitecture));
        public static uint? OSLanguage => GetValue(nameof(OSLanguage));
        public static uint? OSProductSuite => GetValue(nameof(OSProductSuite));
        public static ushort? OSType => GetValue(nameof(OSType));
        public static string OtherTypeDescription => GetValue(nameof(OtherTypeDescription));
        public static bool? PAEEnabled => GetValue(nameof(PAEEnabled));
        public static string PlusProductID => GetValue(nameof(PlusProductID));
        public static string PlusVersionNumber => GetValue(nameof(PlusVersionNumber));
        public static bool? PortableOperatingSystem => GetValue(nameof(PortableOperatingSystem));
        public static bool? Primary => GetValue(nameof(Primary));
        public static uint? ProductType => GetValue(nameof(ProductType));
        public static string RegisteredUser => GetValue(nameof(RegisteredUser));
        public static string SerialNumber => GetValue(nameof(SerialNumber));
        public static ushort? ServicePackMajorVersion => GetValue(nameof(ServicePackMajorVersion));
        public static ushort? ServicePackMinorVersion => GetValue(nameof(ServicePackMinorVersion));
        public static ulong? SizeStoredInPagingFiles => GetValue(nameof(SizeStoredInPagingFiles));
        public static string Status => GetValue(nameof(Status));
        public static uint? SuiteMask => GetValue(nameof(SuiteMask));
        public static string SystemDevice => GetValue(nameof(SystemDevice));
        public static string SystemDirectory => GetValue(nameof(SystemDirectory));
        public static string SystemDrive => GetValue(nameof(SystemDrive));
        public static ulong? TotalSwapSpaceSize => GetValue(nameof(TotalSwapSpaceSize));
        public static ulong? TotalVirtualMemorySize => GetValue(nameof(TotalVirtualMemorySize));
        public static ulong? TotalVisibleMemorySize => GetValue(nameof(TotalVisibleMemorySize));
        public static Version Version => GetValue(nameof(Version), typeof(Version));
        public static string WindowsDirectory => GetValue(nameof(WindowsDirectory));
        public static byte? QuantumLength => GetValue(nameof(QuantumLength));
        public static byte? QuantumType => GetValue(nameof(QuantumType));

        private static dynamic GetValue(string name, Type type)
        {
            try
            {
                dynamic d = GetValue(name).ToString();
                if (type == typeof(Version))
                    d = System.Version.Parse(d);
                else if (type == typeof(DateTime))
                    d = ManagementDateTimeConverter.ToDateTime(d);
                return d;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        private static dynamic GetValue(string name)
        {
            try
            {
                dynamic d;
                using (var obj = new ManagementObject(nameof(Win32_OperatingSystem) + "=@"))
                    d = obj[name];
                return d;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }
    }
}
