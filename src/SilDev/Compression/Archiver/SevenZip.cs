#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: SevenZip.cs
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
    ///     Provides basic features of the 7-Zip application.
    /// </summary>
    public sealed class SevenZip : ArchiverBase
    {
        private static SevenZip _defaultArchiver;

        /// <summary>
        ///     Gets or sets a static default <see cref="SevenZip"/> instance.
        /// </summary>
        public static SevenZip DefaultArchiver
        {
            get => _defaultArchiver ??= new SevenZip();
            set => _defaultArchiver = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SevenZip"/> class.
        /// </summary>
        public SevenZip() : base("7-Zip",
                                 "7z",
                                 null,
                                 new[] { "7zG.exe", "7z.exe" },
                                 null,
                                 new[] { "7z.dll" },
                                 $"a -t7z -mx9 -m0=lzma2 -md=128m -mfb=256 -mhe -ms -mmt{Environment.ProcessorCount} \"\"\"{{1}}\"\"\" \"\"\"{{0}}\\*\"\"\"",
                                 $"a -t7z -mx9 -m0=lzma2 -md=128m -mfb=256 -mhe -ms -mmt{Environment.ProcessorCount} \"\"\"{{1}}\"\"\" \"\"\"{{0}}\"\"\"",
                                 "x \"\"\"{0}\"\"\" -o\"\"\"{1}\"\"\" -y") { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SevenZip"/> class.
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
        ///             "a -t7z \"{1}\\*\" \"{0}\""
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
        ///             "a -t7z \"{1}\" \"{0}\""
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
        ///             "x \"{0}\" -o\"{1}\" -y"
        ///         </c>
        ///     </example>
        /// </param>
        public SevenZip(string location, string createDirArgs, string createFileArgs, string extractArgs) : this()
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
