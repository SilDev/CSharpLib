#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PathEx.cs
// Version:  2017-05-04 21:50
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Provides static methods based on the <see cref="Path"/> class to perform operations on
    ///     <see cref="string"/> instances that contain file or directory path information.
    /// </summary>
    public static class PathEx
    {
        private static readonly char[] InvalidPathChars =
        {
            '\u0000', '\u0001', '\u0002', '\u0003',
            '\u0004', '\u0005', '\u0006', '\u0007',
            '\u0008', '\u0009', '\u000a', '\u000b',
            '\u000c', '\u000d', '\u000e', '\u000f',
            '\u0010', '\u0011', '\u0012', '\u0013',
            '\u0014', '\u0015', '\u0016', '\u0017',
            '\u0018', '\u0019', '\u001a', '\u001b',
            '\u001c', '\u001d', '\u001e', '\u001f',
            '\u0022', '\u002a', '\u003c', '\u003e',
            '\u003f', '\u007c'
        };

        /// <summary>
        ///     Provides enumerated values of PE (Portable Executable) headers.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Headers : ushort
        {
            Unknown = 0x0,
            AM33 = 0x1d3,
            AMD64 = 0x8664,
            ARM = 0x1c0,
            EBC = 0xebc,
            I386 = 0x14c,
            IA64 = 0x200,
            M32R = 0x9041,
            MIPS16 = 0x266,
            MIPSFPU = 0x366,
            MIPSFPU16 = 0x466,
            POWERPC = 0x1f0,
            POWERPCFP = 0x1f1,
            R4000 = 0x166,
            SH3 = 0x1a2,
            SH3DSP = 0x1a3,
            SH4 = 0x1a6,
            SH5 = 0x1a8,
            THUMB = 0x1c2,
            WCEMIPSV2 = 0x169
        }

        /// <summary>
        ///     Gets the full process executable path of the assembly based on
        ///     <see cref="Assembly.GetEntryAssembly()"/>.CodeBase.
        /// </summary>
        public static string LocalPath => Assembly.GetEntryAssembly().CodeBase.ToUri()?.LocalPath;

        /// <summary>
        ///     Gets the process executable located directory path of the assembly based on
        ///     <see cref="Assembly.GetEntryAssembly()"/>.CodeBase.
        /// </summary>
        public static string LocalDir => Path.GetDirectoryName(LocalPath)?.TrimEnd(Path.DirectorySeparatorChar);

        /// <summary>
        ///     Combines <see cref="Directory.Exists(string)"/> and <see cref="File.Exists(string)"/>
        ///     to determine whether the specified path element exists.
        /// </summary>
        /// <param name="path">
        ///     The file or directory to check.
        /// </param>
        public static bool DirOrFileExists(string path) =>
            Directory.Exists(path) || File.Exists(path);

        /// <summary>
        ///     Combines <see cref="Directory.Exists(string)"/> and <see cref="File.Exists(string)"/>
        ///     to determine whether the specified path elements exists.
        /// </summary>
        /// <param name="paths">
        ///     An array of files and directories to check.
        /// </param>
        public static bool DirsOrFilesExists(params string[] paths)
        {
            var exists = false;
            foreach (var path in paths)
            {
                exists = DirOrFileExists(path);
                if (!exists)
                    break;
            }
            return exists;
        }

        /// <summary>
        ///     <para>
        ///         Combines an array of strings, based on <see cref="Path.Combine(string[])"/>,
        ///         <see cref="Path.GetFullPath(string)"/>,
        ///         <see cref="Environment.GetFolderPath(Environment.SpecialFolder)"/>,
        ///         <see cref="Environment.GetEnvironmentVariable(string)"/> and
        ///         <see cref="Regex.Match(string, string, RegexOptions)"/>, into a path.
        ///     </para>
        ///     <para>
        ///         <c>
        ///             Hint:
        ///         </c>
        ///         Allows superordinate directory navigation and environment variables
        ///         based on <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>;
        ///         for example, write <code>"%Desktop%"</code>, cases are ignored as well.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        public static string Combine(char[] invalidPathChars, params string[] paths)
        {
            var path = string.Empty;
            try
            {
                if (paths.Length == 0 || paths.Count(string.IsNullOrWhiteSpace) == paths.Length)
                    throw new ArgumentNullException(nameof(paths));
                var levels = paths;
                var sepChar = Path.DirectorySeparatorChar;
                var sepStr = sepChar.ToString();
                for (var i = 0; i < levels.Length; i++)
                {
                    if (i > 0)
                        levels[i] = levels[i].RemoveChar(Path.VolumeSeparatorChar);
                    if (invalidPathChars != null)
                        levels[i] = levels[i].RemoveChar(invalidPathChars);
                    levels[i] = levels[i].Replace(Path.AltDirectorySeparatorChar, sepChar);
                    if (levels[i].Contains(sepChar))
                        levels[i] = levels[i].Split(sepChar).Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Trim()).Join(sepChar);
                    if (i == 0 && levels[i].EndsWith(Path.VolumeSeparatorChar.ToString()))
                        levels[i] += sepStr;
                }
                path = Path.Combine(levels);
                if (path.StartsWith("%") && (path.Contains("%" + sepStr) || path.EndsWith("%")))
                {
                    var variable = Regex.Match(path, "%(.+?)%", RegexOptions.IgnoreCase).Groups[1].Value;
                    var value = EnvironmentEx.GetVariableValue(variable);
                    if (!string.IsNullOrEmpty(variable) && !string.IsNullOrEmpty(value))
                        path = path.Replace("%" + variable + "%", value);
                }
                if (path.Contains(sepStr + ".."))
                    path = Path.GetFullPath(path);
                if (path.Contains('.'))
                    path = path.TrimEnd('.');
            }
            catch (ArgumentNullException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (ArgumentException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return path;
        }

        /// <summary>
        ///     <para>
        ///         Combines an array of strings, based on <see cref="Path.Combine(string[])"/>,
        ///         <see cref="Path.GetFullPath(string)"/>,
        ///         <see cref="Environment.GetFolderPath(Environment.SpecialFolder)"/>,
        ///         <see cref="Environment.GetEnvironmentVariable(string)"/> and
        ///         <see cref="Regex.Match(string, string, RegexOptions)"/>, into a path.
        ///     </para>
        ///     <para>
        ///         <c>
        ///             Hint:
        ///         </c>
        ///         Allows superordinate directory navigation and environment variables
        ///         based on <see cref="EnvironmentEx.GetVariableValue(string, bool)"/>;
        ///         for example, write <code>"%Desktop%"</code>, cases are ignored as well.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static string Combine(params string[] paths) =>
            Combine(InvalidPathChars, paths);

        /// <summary>
        ///     <para>
        ///         Combines an array of strings, based on <see cref="Combine(string[])"/>, into a
        ///         path.
        ///     </para>
        ///     <para>
        ///         <c>
        ///             Hint:
        ///         </c>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to seperate path levels.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        /// <param name="invalidPathChars">
        ///     A sequence of invalid chars used as a filter.
        /// </param>
        public static string AltCombine(char[] invalidPathChars, params string[] paths)
        {
            var path = Combine(invalidPathChars, paths);
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentNullException(nameof(path));
                path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var schemes = new[]
                {
                    "file:",
                    "ftp:",
                    "http:",
                    "https:"
                };
                for (var i = 0; i < schemes.Length; i++)
                {
                    var scheme = schemes[i] + Path.AltDirectorySeparatorChar;
                    if (!path.StartsWithEx(scheme))
                        continue;
                    path = path.Replace(scheme, scheme + new string(Path.AltDirectorySeparatorChar, i < 1 ? 2 : 1));
                    break;
                }
            }
            catch (ArgumentNullException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (ArgumentException ex)
            {
                if (Log.DebugMode > 1)
                    Log.Write(ex);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return path;
        }

        /// <summary>
        ///     <para>
        ///         Combines an array of strings, based on <see cref="Combine(string[])"/>, into a
        ///         path.
        ///     </para>
        ///     <para>
        ///         <c>
        ///             Hint:
        ///         </c>
        ///         <see cref="Path.AltDirectorySeparatorChar"/> is used to seperate path levels.
        ///     </para>
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static string AltCombine(params string[] paths) =>
            AltCombine(InvalidPathChars, paths);

        /// <summary>
        ///     Returns a uniquely directory name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="label">
        ///     This text is at the beginning of the name.
        /// </param>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempDirName(string label = "tmp", int len = 4)
        {
            var s = label;
            try
            {
                var g = new string(Guid.NewGuid().ToString().Where(char.IsLetterOrDigit).ToArray());
                s = s.ToLower() + g.Substring(0, len.IsBetween(4, 24) ? len : 4).ToUpper();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return s;
        }

        /// <summary>
        ///     Returns a uniquely directory name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempDirName(int len) =>
            GetTempDirName("tmp", len);

        /// <summary>
        ///     Returns a uniquely file name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="label">
        ///     This text is at the beginning of the name.
        /// </param>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempFileName(string label = "tmp", int len = 4) =>
            GetTempDirName(label, len) + ".tmp";

        /// <summary>
        ///     Returns a uniquely file name with a similar format as <see cref="Path.GetTempFileName()"/>.
        /// </summary>
        /// <param name="len">
        ///     The length of the hash. Valid values are 4 through 24.
        /// </param>
        public static string GetTempFileName(int len) =>
            GetTempFileName("tmp", len);

        /// <summary>
        ///     Determines the PE (Portable Executable) header of the specified file.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static Headers GetHeader(string path)
        {
            var pe = Headers.Unknown;
            try
            {
                using (var fs = new FileStream(Combine(path), FileMode.Open, FileAccess.Read))
                {
                    var br = new BinaryReader(fs);
                    fs.Seek(0x3c, SeekOrigin.Begin);
                    fs.Seek(br.ReadInt32(), SeekOrigin.Begin);
                    br.ReadUInt32();
                    pe = (Headers)br.ReadUInt16();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return pe;
        }

        /// <summary>
        ///     Determines whether the specified file was compiled for a 64-bit platform environments.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static bool FileIs64Bit(string path)
        {
            var pe = GetHeader(path);
            return pe == Headers.AMD64 || pe == Headers.IA64;
        }
    }

    /// <summary>
    ///     The exception that is thrown when an attempt to access a target that does not exist
    ///     fails.
    /// </summary>
    [Serializable]
    public class PathNotFoundException : Exception
    {
        /// <summary>
        ///     Create the exception.
        /// </summary>
        public PathNotFoundException() { }

        /// <summary>
        ///     Create the exception with path.
        /// </summary>
        /// <param name="target">
        ///     Exception target.
        /// </param>
        public PathNotFoundException(string target) : base(target)
        {
            Message = "Could not find target '" + target + "'.";
        }

        /// <summary>
        ///     Create the exception with path and inner cause.
        /// </summary>
        /// <param name="target">
        ///     Exception target.
        /// </param>
        /// <param name="innerException">
        ///     Exception inner cause.
        /// </param>
        public PathNotFoundException(string target, Exception innerException) : base(target, innerException)
        {
            Message = "Could not find target '" + target + "'.";
        }

        protected PathNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Gets the error message and the path, or only the exception message if no path
        ///     is set.
        /// </summary>
        public sealed override string Message { get; } = "Unable to find the target from the specified path.";
    }
}
