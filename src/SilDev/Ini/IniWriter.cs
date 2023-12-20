#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IniWriter.cs
// Version:  2023-12-20 00:28
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Ini
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    ///     Provides the functionality for writing INI documents.
    /// </summary>
    public static class IniWriter
    {
        /// <summary>
        ///     Writes the string representation of a specific <see cref="IniDocument"/>
        ///     object to the specified destination stream.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IniDocument"/> object to write.
        /// </param>
        /// <param name="destStream">
        ///     The destination stream to be written.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source or destStream is null.
        /// </exception>
        public static void WriteTo(this IniDocument source, Stream destStream)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (destStream == null)
                throw new ArgumentNullException(nameof(destStream));
            var fileFormat = source.FileFormat;
            using var sw = new StreamWriter(destStream, fileFormat == IniFileFormat.Regedit4 ? EncodingEx.Ansi : EncodingEx.Utf8NoBom, 8192, true);
            switch (fileFormat)
            {
                case IniFileFormat.Regedit4:
                    sw.WriteLine("REGEDIT4");
                    sw.WriteLine();
                    break;
                case IniFileFormat.Regedit5:
                    sw.WriteLine("Windows Registry Editor Version 5.00");
                    sw.WriteLine();
                    break;
            }
            foreach (var section in source.Sections)
            {
                if (IniHelper.SectionIsInvalid(section))
                    continue;
                var keyValueDict = source[section];
                var hasNoValue = keyValueDict?.Count is null or < 1;
                if (hasNoValue && !IniHelper.IsImportantSection(section))
                    continue;
                if (!string.IsNullOrEmpty(section))
                {
                    sw.Write('[');
                    sw.Write(section.Trim());
                    sw.WriteLine(']');
                }
                if (hasNoValue)
                {
                    sw.WriteLine();
                    continue;
                }
                foreach (var pair in keyValueDict)
                {
                    if (IniHelper.KeyIsInvalid(pair.Key) || pair.Value.Count < 1)
                        continue;
                    foreach (var value in pair.Value)
                    {
                        if (string.IsNullOrEmpty(value) || value.Any(TextEx.IsLineSeparator))
                            continue;
                        sw.Write(pair.Key.Trim());
                        sw.Write('=');
                        if (!IniHelper.IsHex(value) || value.Length < 76)
                            sw.WriteLine(value);
                        else
                        {
                            for (var i = 0; i < value.Length; i++)
                            {
                                sw.Write(value[i]);
                                if (i == 0 || i % 75 != 0)
                                    continue;
                                sw.WriteLine('\\');
                            }
                            sw.WriteLine();
                        }
                    }
                }
                sw.WriteLine();
            }
        }

        /// <summary>
        ///     Writes the string representation of a specific <see cref="IniDocument"/>
        ///     object to the specified destination file.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IniDocument"/> object to write.
        /// </param>
        /// <param name="destFile">
        ///     The destination file to be written.
        /// </param>
        public static bool WriteTo(this IniDocument source, string destFile)
        {
            try
            {
                if (source == null)
                    throw new ArgumentNullException(nameof(source));
                if (source.Count == 0)
                    throw new ArgumentInvalidException(nameof(source));
                var path = PathEx.Combine(destFile);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(destFile));
                if (!PathEx.IsValidPath(path))
                    throw new ArgumentInvalidException(nameof(destFile));
                using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                WriteTo(source, fs);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return false;
        }
    }
}
