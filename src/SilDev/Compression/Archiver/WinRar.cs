#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: WinRar.cs
// Version:  2020-01-19 15:33
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Compression.Archiver
{
    using System;

    /// <summary>
    ///     Provides basic features of the WinRAR applications.
    /// </summary>
    public sealed class WinRar : ArchiverBase
    {
        private static WinRar _defaultArchiver;

        /// <summary>
        ///     Gets or sets a static default <see cref="WinRar"/> instance.
        /// </summary>
        public static WinRar DefaultArchiver
        {
            get => _defaultArchiver ??= new WinRar();
            set => _defaultArchiver = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WinRar"/> class.
        /// </summary>
        public WinRar() : base("WinRAR",
                               "rar",
                               null,
                               new[] { "Rar.exe" },
                               new[] { "UnRAR.exe" },
                               null,
                               $"a -ed -ep1 -r -s -m5 -ma5 -md128m -ai -k -mt{Environment.ProcessorCount} \"{{1}}\\*\" \"{{0}}\"",
                               $"a -ed -ep1 -r -s -m5 -ma5 -md128m -ai -k -mt{Environment.ProcessorCount} \"{{1}}\" \"{{0}}\"",
                               "x -ad \"{0}\" \"{1}\"") { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WinRar"/> class.
        /// </summary>
        /// <param name="location">
        ///     The location of the executables.
        /// </param>
        /// <param name="createDirArgs">
        ///     The command line arguments format string that is used to create archives
        ///     from directories.
        ///     <para>
        ///         Please note that two formation keys must be defined; {0} is the source
        ///         directory path and {1} is the destination archive path.
        ///     </para>
        ///     <para/>
        ///     <example>
        ///         <strong>
        ///             Example:
        ///         </strong>
        ///         <c>
        ///             "a \"{1}\\*\" \"{0}\""
        ///         </c>
        ///     </example>
        /// </param>
        /// <param name="createFileArgs">
        ///     The command line arguments format string that is used to create archives
        ///     from files.
        ///     <para>
        ///         Please note that two formation keys must be defined; {0} is the source
        ///         file path and {1} is the destination archive path.
        ///     </para>
        ///     <para/>
        ///     <example>
        ///         <strong>
        ///             Example:
        ///         </strong>
        ///         <c>
        ///             "a \"{1}\" \"{0}\""
        ///         </c>
        ///     </example>
        /// </param>
        /// <param name="extractArgs">
        ///     The command line arguments format string that is used to extract archives.
        ///     <para>
        ///         Please note that two formation keys must be defined; {0} is the source
        ///         archive path and {1} is the destination directory path.
        ///     </para>
        ///     <para/>
        ///     <example>
        ///         <strong>
        ///             Example:
        ///         </strong>
        ///         <c>
        ///             "x -ad \"{0}\" \"{1}\""
        ///         </c>
        ///     </example>
        /// </param>
        public WinRar(string location, string createDirArgs, string createFileArgs, string extractArgs) : this()
        {
            Location = location;
            if (!string.IsNullOrWhiteSpace(createDirArgs))
                CreateDirArgs = createDirArgs;
            if (!string.IsNullOrWhiteSpace(createFileArgs))
                CreateFileArgs = createFileArgs;
            if (!string.IsNullOrWhiteSpace(extractArgs))
                ExtractArgs = extractArgs;
        }
    }
}
