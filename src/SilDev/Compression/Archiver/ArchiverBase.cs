#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ArchiverBase.cs
// Version:  2020-01-19 15:33
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Compression.Archiver
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    /// <summary>
    ///     Provides a base class to handle file archiver.
    /// </summary>
    public abstract class ArchiverBase
    {
        private string _location;

        /// <summary>
        ///     The location of the executables.
        /// </summary>
        public string Location
        {
            get
            {
                if (_location != default)
                    return _location;
                foreach (var location in GetDefaultLocations())
                {
                    var dir = location;
                    if (!ValidateFilePaths(ref dir, out var path1, out var path2))
                        continue;
                    _location = dir;
                    CreateExePath = path1;
                    ExtractExePath = path2;
                    return _location;
                }
                return _location ??= string.Empty;
            }
            set
            {
                var dir = value;
                if (!ValidateFilePaths(ref dir, out var path1, out var path2))
                    return;
                _location = dir;
                CreateExePath = path1;
                ExtractExePath = path2;
            }
        }

        /// <summary>
        ///     The path of the executable used to create archives.
        /// </summary>
        public string CreateExePath { get; private set; }

        /// <summary>
        ///     The path of the executable used to extract archives.
        /// </summary>
        public string ExtractExePath { get; private set; }

        /// <summary>
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
        /// </summary>
        protected string CreateDirArgs { get; set; }

        /// <summary>
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
        /// </summary>
        protected string CreateFileArgs { get; set; }

        /// <summary>
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
        /// </summary>
        protected string ExtractArgs { get; set; }

        private string AppName { get; }

        private string AltAppName { get; }

        private IReadOnlyList<string> CreateExeNames { get; }

        private IReadOnlyList<string> ExtractExeNames { get; }

        private IReadOnlyList<string> Depencies { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArchiverBase"/> class.
        /// </summary>
        /// <param name="appName">
        ///     The name of the application; should be the name of the default installation
        ///     directory.
        ///     <para/>
        ///     <example>
        ///         <strong>
        ///             Example:
        ///         </strong>
        ///         "7-Zip"
        ///     </example>
        /// </param>
        /// <param name="altAppName">
        ///     The alternate name of the application.
        ///     <para/>
        ///     <example>
        ///         <strong>
        ///             Example:
        ///         </strong>
        ///         "7z"
        ///     </example>
        /// </param>
        /// <param name="location">
        ///     The location of the executables.
        /// </param>
        /// <param name="createExeNames">
        ///     A sequence of execution file names that can be used to create archives.
        ///     <para>
        ///         Please note that only the file found first is used. All other defined
        ///         files are only alternatives.
        ///     </para>
        ///     <para/>
        ///     <example>
        ///         <strong>
        ///             Example:
        ///         </strong>
        ///         <code>
        ///             new string[] { "7zG.exe", "7z.exe", ... }
        ///         </code>
        ///     </example>
        /// </param>
        /// <param name="extractExeNames">
        ///     A sequence of execution file names that can be used to extract archives.
        ///     The rules are the same as for the <paramref name="createExeNames"/>
        ///     parameter.
        ///     <para>
        ///         Can be <see langword="null"/> if the same files are used to create and
        ///         extract.
        ///     </para>
        /// </param>
        /// <param name="depencies">
        ///     A sequence of files that must exist to allow the archiver applications to
        ///     run.
        ///     <para/>
        ///     <example>
        ///         <strong>
        ///             Example:
        ///         </strong>
        ///         <code>
        ///             new string[] { "7z.dll", "Lang\\en.ttt", ... }
        ///         </code>
        ///     </example>
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
        protected ArchiverBase(string appName,
                               string altAppName,
                               string location,
                               IReadOnlyList<string> createExeNames,
                               IReadOnlyList<string> extractExeNames,
                               IReadOnlyList<string> depencies,
                               string createDirArgs,
                               string createFileArgs,
                               string extractArgs)
        {
            AppName = appName;
            AltAppName = altAppName;
            Location = location;
            CreateExeNames = createExeNames;
            ExtractExeNames = extractExeNames ?? createExeNames;
            Depencies = depencies;
            CreateDirArgs = createDirArgs;
            CreateFileArgs = createFileArgs;
            ExtractArgs = extractArgs;
        }

        /// <summary>
        ///     Compress the specified file, or all files of the specified directory, to
        ///     the specified file on the file system.
        /// </summary>
        /// <param name="srcDirOrFile">
        ///     The path of the file or directory to compress.
        /// </param>
        /// <param name="destFile">
        ///     The path of the archive.
        /// </param>
        /// <param name="windowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component; otherwise, <see langword="false"/>.
        /// </param>
        public Process Create(string srcDirOrFile, string destFile, ProcessWindowStyle windowStyle = ProcessWindowStyle.Minimized, bool dispose = false)
        {
            if (!File.Exists(CreateExePath))
                return null;
            var src = PathEx.Combine(srcDirOrFile);
            var dest = PathEx.Combine(destFile);
            var args = PathEx.IsDir(src) ? CreateDirArgs : CreateFileArgs;
            return ProcessEx.Start(CreateExePath, args.FormatCurrent(src, dest), false, windowStyle, dispose);
        }

        /// <summary>
        ///     Extracts all the files in the specified archive to the specified directory
        ///     on the file system.
        /// </summary>
        /// <param name="srcFile">
        ///     The path of the archive to extract.
        /// </param>
        /// <param name="destDir">
        ///     The path to the directory to place the extracted files in.
        /// </param>
        /// <param name="windowStyle">
        ///     The window state to use when the process is started.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to release all resources used by the
        ///     <see cref="Process"/> component; otherwise, <see langword="false"/>.
        /// </param>
        public Process Extract(string srcFile, string destDir, ProcessWindowStyle windowStyle = ProcessWindowStyle.Minimized, bool dispose = false) =>
            !File.Exists(ExtractExePath) ? null : ProcessEx.Start(ExtractExePath, ExtractArgs.FormatCurrent(srcFile, destDir), false, windowStyle, dispose);

        private IEnumerable<string> GetDefaultLocations()
        {
            yield return $"%ProgramFiles%\\{AppName}";
            yield return "%CurDir%";
#if any || x64
#if any
            if (Environment.Is64BitOperatingSystem)
            {
#endif
                yield return $"%CurDir%\\bin\\{AltAppName}\\x64";
                yield return $"%CurDir%\\bin\\helper\\{AltAppName}\\x64";
                yield return $"%CurDir%\\binaries\\{AltAppName}\\x64";
                yield return $"%CurDir%\\binaries\\helper\\{AltAppName}\\x64";
                yield return $"%CurDir%\\helper\\{AltAppName}\\x64";
                yield return $"%ProgramFiles(x86)%\\{AppName}";
#if any
            }
#endif
#endif
            yield return $"%CurDir%\\bin\\{AltAppName}";
            yield return $"%CurDir%\\bin\\helper\\{AltAppName}";
            yield return $"%CurDir%\\binaries\\{AltAppName}";
            yield return $"%CurDir%\\binaries\\helper\\{AltAppName}";
            yield return $"%CurDir%\\helper\\{AltAppName}";
        }

        private bool ValidateFilePaths(ref string dir, out string path1, out string path2)
        {
            dir = PathEx.Combine(dir);
            if (!Directory.Exists(dir))
                goto Invalid;

            var names = Depencies?.ToArray();
            if (names?.Any() ?? false)
                foreach (var name in names)
                {
                    var path = Path.Combine(dir, name);
                    if (File.Exists(path))
                        continue;
                    goto Invalid;
                }

            path1 = string.Empty;
            path2 = string.Empty;
            for (var i = 0; i < 2; i++)
            {
                names = (i == 0 ? CreateExeNames : ExtractExeNames)?.ToArray();
                if (!names?.Any() ?? true)
                    goto Invalid;
                foreach (var name in names)
                {
                    var path = Path.Combine(dir, name);
                    if (!File.Exists(path))
                        continue;
                    if (i == 0)
                    {
                        path1 = path;
                        break;
                    }
                    path2 = path;
                    break;
                }
            }
            if (File.Exists(path1) && File.Exists(path2))
                return true;

            Invalid:
            dir = string.Empty;
            path1 = string.Empty;
            path2 = string.Empty;
            return false;
        }
    }
}
