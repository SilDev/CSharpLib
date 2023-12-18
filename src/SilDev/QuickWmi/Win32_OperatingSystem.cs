#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Win32_OperatingSystem.cs
// Version:  2023-12-18 22:31
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.QuickWmi
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Management;

    /// <summary>
    ///     Provides quick access to <see cref="Win32_OperatingSystem"/> WMI class
    ///     properties.
    /// </summary>
    /// ReSharper disable CommentTypo InconsistentNaming
     public static class Win32_OperatingSystem
    {
        /// <summary>
        ///     Gets the name of the disk drive from which the Windows operating system
        ///     starts.
        ///     <para>
        ///         Example: "\\Device\Harddisk0"
        ///     </para>
        /// </summary>
        public static string BootDevice => GetValue<string>(nameof(BootDevice));

        /// <summary>
        ///     Gets the build number of an operating system. It can be used for more
        ///     precise version information than product release version numbers.
        ///     <para>
        ///         Example: "1381".
        ///     </para>
        /// </summary>
        public static string BuildNumber => GetValue<string>(nameof(BuildNumber));

        /// <summary>
        ///     Gets the type of build used for an operating system.
        ///     <para>
        ///         Examples: "retail build", "checked build"
        ///     </para>
        /// </summary>
        public static string BuildType => GetValue<string>(nameof(BuildType));

        /// <summary>
        ///     Gets the operating system version.
        ///     <para>
        ///         Example: "Microsoft Windows 7 Enterprise"
        ///     </para>
        /// </summary>
        public static string Caption => GetValue<string>(nameof(Caption));

        /// <summary>
        ///     Gets the code page value an operating system uses. A code page contains a
        ///     character table that an operating system uses to translate strings for
        ///     different languages. The American National Standards Institute (ANSI) lists
        ///     values that represent defined code pages. If an operating system does not
        ///     use an ANSI code page, this member is set to 0 (zero). The CodeSet string
        ///     can use a maximum of six characters to define the code page value.
        /// </summary>
        public static string CodeSet => GetValue<string>(nameof(CodeSet));

        /// <summary>
        ///     Gets the code for the country/region that an operating system uses. Values
        ///     are based on international phone dialing prefixes-also referred to as IBM
        ///     country/region codes. This property can use a maximum of six characters to
        ///     define the country/region code value.
        ///     <para>
        ///         Example: "1" (United States)
        ///     </para>
        /// </summary>
        public static string CountryCode => GetValue<string>(nameof(CountryCode));

        /// <summary>
        ///     Gets the name of the first concrete class that appears in the inheritance
        ///     chain used in the creation of an instance. When used with other key
        ///     properties of the class, this property allows all instances of this class
        ///     and its subclasses to be identified uniquely.
        /// </summary>
        public static string CreationClassName => GetValue<string>(nameof(CreationClassName));

        /// <summary>
        ///     Gets the creation class name of the scoping computer system.
        /// </summary>
        public static string CSCreationClassName => GetValue<string>(nameof(CSCreationClassName));

        /// <summary>
        ///     Gets a <see langword="null"/>-terminated string that indicates the latest
        ///     service pack installed on a computer. If no service pack is installed, the
        ///     string is <see langword="null"/>.
        /// </summary>
        public static string CSDVersion => GetValue<string>(nameof(CSDVersion));

        /// <summary>
        ///     Gets the name of the scoping computer system.
        /// </summary>
        public static string CSName => GetValue<string>(nameof(CSName));

        /// <summary>
        ///     Gets the number, in minutes, an operating system is offset from Greenwich
        ///     mean time (GMT). The number is positive, negative, or zero.
        /// </summary>
        public static short CurrentTimeZone => GetValue<short>(nameof(CurrentTimeZone));

        /// <summary>
        ///     Gets the Data Execution Prevention. When the Data Execution Prevention
        ///     hardware feature is available, this property indicates that the feature is
        ///     set to work for 32-bit applications if <see langword="true"/>. On 64-bit
        ///     computers, the Data Execution Prevention feature is configured in the Boot
        ///     Configuration Data (BCD) store and the properties in
        ///     <see cref="Win32_OperatingSystem"/> are set accordingly.
        /// </summary>
        public static bool DataExecutionPrevention_32BitApplications => GetValue<bool>(nameof(DataExecutionPrevention_32BitApplications));

        /// <summary>
        ///     Gets the Data Execution Prevention that is a hardware feature to prevent
        ///     buffer overrun attacks by stopping the execution of code on data-type
        ///     memory pages. If <see langword="true"/>, then this feature is available. On
        ///     64-bit computers, the Data Execution Prevention feature is configured in
        ///     the BCD store and the properties in <see cref="Win32_OperatingSystem"/> are
        ///     set accordingly.
        /// </summary>
        public static bool DataExecutionPrevention_Available => GetValue<bool>(nameof(DataExecutionPrevention_Available));

        /// <summary>
        ///     Gets the Data Execution Prevention. When the Data Execution Prevention
        ///     hardware feature is available, this property indicates that the feature is
        ///     set to work for drivers if <see langword="true"/>. On 64-bit computers, the
        ///     Data Execution Prevention feature is configured in the BCD store and the
        ///     properties in <see cref="Win32_OperatingSystem"/> are set accordingly.
        /// </summary>
        public static bool DataExecutionPrevention_Drivers => GetValue<bool>(nameof(DataExecutionPrevention_Drivers));

        /// <summary>
        ///     Gets the value indicating which Data Execution Prevention (DEP) setting is
        ///     applied. The DEP setting specifies the extent to which DEP applies to
        ///     32-bit applications on the system. DEP is always applied to the Windows
        ///     kernel.
        ///     <para>
        ///         Always Off (0): DEP is turned off for all 32-bit applications on the
        ///         computer with no exceptions. This setting is not available for the user
        ///         interface.
        ///     </para>
        ///     <para>
        ///         Always On (1): DEP is enabled for all 32-bit applications on the
        ///         computer. This setting is not available for the user interface.
        ///     </para>
        ///     <para>
        ///         Opt In (2): DEP is enabled for a limited number of binaries, the
        ///         kernel, and all Windows-based services. However, it is off by default
        ///         for all 32-bit applications. A user or administrator must explicitly
        ///         choose either the Always On or the Opt Out setting before DEP can be
        ///         applied to 32-bit applications.
        ///     </para>
        ///     <para>
        ///         Opt Out (3): DEP is enabled by default for all 32-bit applications. A
        ///         user or administrator can explicitly remove support for a 32-bit
        ///         application by adding the application to an exceptions list.
        ///     </para>
        /// </summary>
        public static byte DataExecutionPrevention_SupportPolicy => GetValue<byte>(nameof(DataExecutionPrevention_SupportPolicy));

        /// <summary>
        ///     Gets the value indicating that the operating system is a checked (debug)
        ///     build. If <see langword="true"/>, the debugging version is installed.
        ///     Checked builds provide error checking, argument verification, and system
        ///     debugging code. Additional code in a checked binary generates a kernel
        ///     debugger error message and breaks into the debugger. This helps immediately
        ///     determine the cause and location of the error. Performance may be affected
        ///     in a checked build due to the additional code that is executed.
        /// </summary>
        public static bool Debug => GetValue<bool>(nameof(Debug));

        /// <summary>
        ///     Gets the description of the Windows operating system. Some user interfaces
        ///     for example, those that allow editing of this description, limit its length
        ///     to 48 characters.
        /// </summary>
        public static string Description => GetValue<string>(nameof(Description));

        /// <summary>
        ///     Gets the value that specifies if the operating system is distributed across
        ///     several computer system nodes. If so, these nodes should be grouped as a
        ///     cluster.
        /// </summary>
        public static bool Distributed => GetValue<bool>(nameof(Distributed));

        /// <summary>
        ///     Gets the encryption level for secure transactions: 40-bit, 128-bit, or
        ///     n-bit.
        /// </summary>
        public static uint EncryptionLevel => GetValue<uint>(nameof(EncryptionLevel));

        /// <summary>
        ///     Gets the value that increases in priority is given to the foreground
        ///     application. Application boost is implemented by giving an application more
        ///     execution time slices (quantum lengths).
        /// </summary>
        public static byte ForegroundApplicationBoost => GetValue<byte>(nameof(ForegroundApplicationBoost));

        /// <summary>
        ///     Gets the number, in kilobytes, of physical memory currently unused and
        ///     available.
        /// </summary>
        public static ulong FreePhysicalMemory => GetValue<ulong>(nameof(FreePhysicalMemory));

        /// <summary>
        ///     Gets the number, in kilobytes, that can be mapped into the operating system
        ///     paging files without causing any other pages to be swapped out.
        /// </summary>
        public static ulong FreeSpaceInPagingFiles => GetValue<ulong>(nameof(FreeSpaceInPagingFiles));

        /// <summary>
        ///     Gets the number, in kilobytes, of virtual memory currently unused and
        ///     available.
        /// </summary>
        public static ulong FreeVirtualMemory => GetValue<ulong>(nameof(FreeVirtualMemory));

        /// <summary>
        ///     Gets the install date of the operating system.
        /// </summary>
        public static DateTime InstallDate => GetValue<DateTime>(nameof(InstallDate));

        /// <summary>
        ///     Gets the operating system cache optimize option.
        ///     <para>
        ///         0: Optimize memory for applications.
        ///     </para>
        ///     <para>
        ///         1: Optimize memory for system performance.
        ///     </para>
        /// </summary>
        public static uint LargeSystemCache => GetValue<uint>(nameof(LargeSystemCache));

        /// <summary>
        ///     Gets the date and time the operating system was last restarted.
        /// </summary>
        public static DateTime LastBootUpTime => GetValue<DateTime>(nameof(LastBootUpTime));

        /// <summary>
        ///     Gets the operating system version of the local date and time-of-day.
        /// </summary>
        public static DateTime LocalDateTime => GetValue<DateTime>(nameof(LocalDateTime));

        /// <summary>
        ///     Gets the language identifier used by the operating system. A language
        ///     identifier is a standard international numeric abbreviation for a
        ///     country/region. Each language has a unique language identifier (LANGID), a
        ///     16-bit value that consists of a primary language identifier and a secondary
        ///     language identifier.
        /// </summary>
        public static string Locale => GetValue<string>(nameof(Locale));

        /// <summary>
        ///     Gets the name of the operating system manufacturer. For Windows-based
        ///     systems, this value is "Microsoft Corporation".
        /// </summary>
        public static string Manufacturer => GetValue<string>(nameof(Manufacturer));

        /// <summary>
        ///     Gets the maximum number of process contexts the operating system can
        ///     support. The default value set by the provider is 4294967295 (0xFFFFFFFF).
        ///     If there is no fixed maximum, the value should be 0 (zero). On systems that
        ///     have a fixed maximum, this object can help diagnose failures that occur
        ///     when the maximum is reached-if unknown, enter 4294967295 (0xFFFFFFFF).
        /// </summary>
        public static uint MaxNumberOfProcesses => GetValue<uint>(nameof(MaxNumberOfProcesses));

        /// <summary>
        ///     Gets the maximum number, in kilobytes, of memory that can be allocated to a
        ///     process. For operating systems with no virtual memory, typically this value
        ///     is equal to the total amount of physical memory minus the memory used by
        ///     the BIOS and the operating system. For some operating systems, this value
        ///     may be infinity, in which case 0 (zero) should be entered. In other cases,
        ///     this value could be a constant, for example, 2G or 4G.
        /// </summary>
        public static ulong MaxProcessMemorySize => GetValue<ulong>(nameof(MaxProcessMemorySize));

        /// <summary>
        ///     Gets the Multilingual User Interface Pack (MUI Pack) languages installed on
        ///     the computer. For example, "en-us". MUI Pack languages are resource files
        ///     that can be installed on the English version of the operating system. When
        ///     an MUI Pack is installed, you can can change the user interface language to
        ///     one of 33 supported languages.
        /// </summary>
        public static ReadOnlyCollection<string> MUILanguages => GetValue<ReadOnlyCollection<string>>(nameof(MUILanguages));

        /// <summary>
        ///     Gets the name of the operating system instance within a computer system.
        /// </summary>
        public static string Name => GetValue<string>(nameof(Name));

        /// <summary>
        ///     Gets the number of user licenses for the operating system. If unlimited,
        ///     enter 0 (zero). If unknown, enter -1.
        /// </summary>
        public static uint NumberOfLicensedUsers => GetValue<uint>(nameof(NumberOfLicensedUsers));

        /// <summary>
        ///     Gets the number of process contexts currently loaded or running on the
        ///     operating system.
        /// </summary>
        public static uint NumberOfProcesses => GetValue<uint>(nameof(NumberOfProcesses));

        /// <summary>
        ///     Gets the number of user sessions for which the operating system is storing
        ///     state information currently.
        /// </summary>
        public static uint NumberOfUsers => GetValue<uint>(nameof(NumberOfUsers));

        /// <summary>
        ///     Gets the Stock Keeping Unit (SKU) number for the operating system.
        /// </summary>
        public static uint OperatingSystemSKU => GetValue<uint>(nameof(OperatingSystemSKU));

        /// <summary>
        ///     Gets the company name for the registered user of the operating system.
        /// </summary>
        public static string Organization => GetValue<string>(nameof(Organization));

        /// <summary>
        ///     Gets the architecture of the operating system, as opposed to the processor.
        ///     This property can be localized.
        /// </summary>
        public static string OSArchitecture => GetValue<string>(nameof(OSArchitecture));

        /// <summary>
        ///     Gets the language version of the operating system installed. The following
        ///     list lists the possible values. Example: 0x0807 (German, Switzerland).
        /// </summary>
        public static uint OSLanguage => GetValue<uint>(nameof(OSLanguage));

        /// <summary>
        ///     Gets the installed and licensed system product additions to the operating
        ///     system. For example, the value of 146 (0x92) for OSProductSuite indicates
        ///     Enterprise, Terminal Services, and Data Center (bits one, four, and seven
        ///     set). The following list lists possible values.
        /// </summary>
        public static uint OSProductSuite => GetValue<uint>(nameof(OSProductSuite));

        /// <summary>
        ///     Gets the type of operating system.
        /// </summary>
        public static ushort OSType => GetValue<ushort>(nameof(OSType));

        /// <summary>
        ///     Gets the additional description for the current operating system version.
        /// </summary>
        public static string OtherTypeDescription => GetValue<string>(nameof(OtherTypeDescription));

        /// <summary>
        ///     Gets the value that specifies whether the physical address extensions (PAE)
        ///     are enabled by the operating system running on Intel processors. PAE allows
        ///     applications to address more than 4 GB of physical memory. When PAE is
        ///     enabled, the operating system uses three-level linear address translation
        ///     rather than two-level. Providing more physical memory to an application
        ///     reduces the need to swap memory to the page file and increases performance.
        /// </summary>
        public static bool PAEEnabled => GetValue<bool>(nameof(PAEEnabled));

        /// <summary>
        ///     Gets the value that specifies whether the operating system booted from an
        ///     external USB device. If <see langword="true"/>, the operating system has
        ///     detected it is booting on a supported locally connected storage device.
        /// </summary>
        public static bool PortableOperatingSystem => GetValue<bool>(nameof(PortableOperatingSystem));

        /// <summary>
        ///     Gets the value that specifies whether this is the primary operating system.
        /// </summary>
        public static bool Primary => GetValue<bool>(nameof(Primary));

        /// <summary>
        ///     Gets additional system information.
        /// </summary>
        public static uint ProductType => GetValue<uint>(nameof(ProductType));

        /// <summary>
        ///     Gets the name of the registered user of the operating system.
        ///     <para>
        ///         Example: "Ben Smith"
        ///     </para>
        /// </summary>
        public static string RegisteredUser => GetValue<string>(nameof(RegisteredUser));

        /// <summary>
        ///     Gets the operating system product serial identification number.
        ///     <para>
        ///         Example: "10497-OEM-0031416-71674"
        ///     </para>
        /// </summary>
        public static string SerialNumber => GetValue<string>(nameof(SerialNumber));

        /// <summary>
        ///     Gets the major version number of the service pack installed on the computer
        ///     system. If no service pack has been installed, the value is 0 (zero).
        /// </summary>
        public static ushort ServicePackMajorVersion => GetValue<ushort>(nameof(ServicePackMajorVersion));

        /// <summary>
        ///     Gets the minor version number of the service pack installed on the computer
        ///     system. If no service pack has been installed, the value is 0 (zero).
        /// </summary>
        public static ushort ServicePackMinorVersion => GetValue<ushort>(nameof(ServicePackMinorVersion));

        /// <summary>
        ///     Gets the total number of kilobytes that can be stored in the operating
        ///     system paging files-0 (zero) indicates that there are no paging files. Be
        ///     aware that this number does not represent the actual physical size of the
        ///     paging file on disk.
        /// </summary>
        public static ulong SizeStoredInPagingFiles => GetValue<ulong>(nameof(SizeStoredInPagingFiles));

        /// <summary>
        ///     Gets the current status of the object. Various operational and
        ///     nonoperational statuses can be defined. Operational statuses include: "OK",
        ///     "Degraded", and "Pred Fail" (an element, such as a SMART-enabled hard disk
        ///     drive may function properly, but predicts a failure in the near future).
        ///     Nonoperational statuses include: "Error", "Starting", "Stopping", and
        ///     "Service". The Service status applies to administrative work, such as
        ///     mirror-resilvering of a disk, reload of a user permissions list, or other
        ///     administrative work. Not all such work is online, but the managed element
        ///     is neither "OK" nor in one of the other states.
        /// </summary>
        public static string Status => GetValue<string>(nameof(Status));

        /// <summary>
        ///     Gets the bit flags that identify the product suites available on the
        ///     system.
        /// </summary>
        public static uint SuiteMask => GetValue<uint>(nameof(SuiteMask));

        /// <summary>
        ///     Gets the physical disk partition on which the operating system is
        ///     installed.
        /// </summary>
        public static string SystemDevice => GetValue<string>(nameof(SystemDevice));

        /// <summary>
        ///     Gets the system directory of the operating system.
        ///     <para>
        ///         Example: "C:\WINDOWS\SYSTEM32"
        ///     </para>
        /// </summary>
        public static string SystemDirectory => GetValue<string>(nameof(SystemDirectory));

        /// <summary>
        ///     Gets the letter of the disk drive on which the operating system resides.
        ///     <para>
        ///         Example: "C:"
        ///     </para>
        /// </summary>
        public static string SystemDrive => GetValue<string>(nameof(SystemDrive));

        /// <summary>
        ///     Gets the total swap space in kilobytes. This value may be NULL
        ///     (unspecified) if the swap space is not distinguished from page files.
        ///     However, some operating systems distinguish these concepts. For example, in
        ///     UNIX, whole processes can be swapped out when the free page list falls and
        ///     remains below a specified amount.
        /// </summary>
        public static ulong TotalSwapSpaceSize => GetValue<ulong>(nameof(TotalSwapSpaceSize));

        /// <summary>
        ///     Gets the number, in kilobytes, of virtual memory. For example, this may be
        ///     calculated by adding the amount of total RAM to the amount of paging space,
        ///     that is, adding the amount of memory in or aggregated by the computer
        ///     system to the property, <see cref="SizeStoredInPagingFiles"/>.
        /// </summary>
        public static ulong TotalVirtualMemorySize => GetValue<ulong>(nameof(TotalVirtualMemorySize));

        /// <summary>
        ///     Gets the total amount, in kilobytes, of physical memory available to the
        ///     operating system. This value does not necessarily indicate the
        ///     <see langword="true"/> amount of physical memory, but what is reported to
        ///     the operating system as available to it.
        /// </summary>
        public static ulong TotalVisibleMemorySize => GetValue<ulong>(nameof(TotalVisibleMemorySize));

        /// <summary>
        ///     Gets the version number of the operating system.
        /// </summary>
        public static Version Version => GetValue<Version>(nameof(Version));

        /// <summary>
        ///     Gets the Windows directory of the operating system.
        ///     <para>
        ///         Example: "C:\WINDOWS"
        ///     </para>
        /// </summary>
        public static string WindowsDirectory => GetValue<string>(nameof(WindowsDirectory));

        private static object GetValue(string name)
        {
            using var mo = new ManagementObject($"{nameof(Win32_OperatingSystem)}=@");
            return mo[name];
        }

        private static T GetValue<T>(string name)
        {
            try
            {
                var value = GetValue(name);
                var type = typeof(T);
                switch (value)
                {
                    case T item:
                        return item;
                    case string item when type == typeof(DateTime):
                        return (T)(object)ManagementDateTimeConverter.ToDateTime(item);
                    case string item when type == typeof(Version):
                        while (item.Count(c => c == '.') < 3)
                            item += ".0";
                        return (T)(object)Version.Parse(item);
                    case string[] item when type == typeof(ReadOnlyCollection<string>):
                        return (T)(object)new ReadOnlyCollection<string>(item);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return default;
        }
    }
}
