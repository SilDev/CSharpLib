#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IniReader.cs
// Version:  2020-02-03 20:22
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Ini
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///     Provides the functionality for parsing INI documents.
    /// </summary>
    public static class IniReader
    {
        /// <summary>
        ///     Converts the content of an INI file or an INI file formatted string value
        ///     to an equivalent <see cref="IniDocument"/> object.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        public static IniDocument Parse(string fileOrContent)
        {
            var dict = Parse(fileOrContent, true, out var fileFormat);
            return new IniDocument(dict, fileFormat);
        }

        /// <summary>
        ///     Converts the content of an INI file or an INI file formatted string value
        ///     to an equivalent <see cref="IniDocument"/> object.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        /// <param name="result">
        ///     The result <see cref="IniDocument"/> object.
        /// </param>
        public static bool TryParse(string fileOrContent, out IniDocument result)
        {
            try
            {
                result = Parse(fileOrContent);
                return result != null;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                result = default;
                Log.Write(ex);
            }
            return false;
        }

        internal static IDictionary<string, IDictionary<string, IList<string>>> Parse(string fileOrContent, bool ignoreCase, out IniFileFormat fileFormat)
        {
            fileFormat = IniFileFormat.Default;
            var source = fileOrContent;
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(fileOrContent));
            var path = PathEx.Combine(source);
            if (File.Exists(path))
                source = File.ReadAllText(path, EncodingEx.Utf8NoBom);
            var lines = source.Split(TextSeparatorChar.AllNewLineChars, StringSplitOptions.RemoveEmptyEntries);
            var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            var content = new Dictionary<string, IDictionary<string, IList<string>>>(comparer);
            var section = string.Empty;
            var key = string.Empty;
            var index = 0;
            var isHex = false;
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (i == 0)
                {
                    if (line.Equals("Windows Registry Editor Version 5.00", StringComparison.Ordinal))
                        fileFormat = IniFileFormat.Regedit5;
                    else if (line.Equals("REGEDIT4", StringComparison.Ordinal))
                        fileFormat = IniFileFormat.Regedit4;
                }
                if (isHex && (isHex = IniHelper.IsHex(line)))
                {
                    content[section][key][index] += line.TrimEnd('\\');
                    continue;
                }
                var item = line.Trim();
                if (item.Length < 3 || item[0] == ';' || item[0] == '#')
                    continue;
                int length;
                if (item[0] == '[' && (length = item.LastIndexOf(']') - 1) > 0)
                {
                    section = item.Substring(1, length);
                    if (IniHelper.IsImportantSection(section) && !content.ContainsKey(section))
                        content.Add(section, default);
                    continue;
                }
                if ((length = item.IndexOf('=')) < 1 || length > item.Length - 2)
                    continue;
                key = item.Substring(0, length);
                var value = item.Substring(length + 1);
                if (IniHelper.HasHexPrefix(value) && IniHelper.IsHex(value) && value.EndsWith("\\", StringComparison.Ordinal))
                {
                    isHex = true;
                    value = value.TrimEnd('\\');
                }
                if (!content.ContainsKey(section))
                    content.Add(section, new Dictionary<string, IList<string>>(comparer));
                if (!content[section].ContainsKey(key))
                    content[section].Add(key, new List<string>());
                content[section][key].Add(value);
                if (isHex)
                    index = content[section][key].Count - 1;
            }
            return content;
        }

        internal static bool TryParse(string fileOrContent, bool ignoreCase, out IDictionary<string, IDictionary<string, IList<string>>> result, out IniFileFormat fileFormat)
        {
            fileFormat = IniFileFormat.Default;
            try
            {
                result = Parse(fileOrContent, ignoreCase, out fileFormat);
                return result != null;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                result = default;
                Log.Write(ex);
            }
            return false;
        }
    }
}
