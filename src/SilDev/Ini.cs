#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Ini.cs
// Version:  2017-05-22 19:05
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Provides the functionality to handle the INI format.
    /// </summary>
    public static class Ini
    {
        private const string NonSectionId = "\0\u0002(NON-SECTION)\u0003\0";

        private static Dictionary<int, Dictionary<string, Dictionary<string, List<string>>>> CachedFiles { get; set; }

        private static void InitializeCache(int code, string section = null, string key = null)
        {
            if (CachedFiles == null)
                CachedFiles = new Dictionary<int, Dictionary<string, Dictionary<string, List<string>>>>();

            if (!CachedFiles.ContainsKey(code))
                CachedFiles[code] = new Dictionary<string, Dictionary<string, List<string>>>();

            if (string.IsNullOrEmpty(section))
                return;

            if (!CachedFiles[code].ContainsKey(section))
                CachedFiles[code][section] = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(key))
                return;

            if (!CachedFiles[code][section].ContainsKey(key))
                CachedFiles[code][section][key] = new List<string>();
        }

        /// <summary>
        ///     Save the cached data to the specified file.
        /// </summary>
        /// <param name="cacheFilePath">
        ///     The full file path of the cache file to create.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file. If this parameter is NULL, all
        ///     cached data are saved.
        /// </param>
        public static void SaveCache(string cacheFilePath, string fileOrContent)
        {
            if (CachedFiles?.Any() != true)
                return;
            var cache = CachedFiles;
            var file = fileOrContent ?? GetFile();
            if (!string.IsNullOrEmpty(file))
            {
                var code = GetCode(file);
                if (!CodeExists(code))
                    return;
                foreach (var data in CachedFiles)
                {
                    if (data.Key != code)
                        continue;
                    cache = new Dictionary<int, Dictionary<string, Dictionary<string, List<string>>>> { { data.Key, data.Value } };
                    break;
                }
            }
            var bytes = cache?.SerializeObject();
            if (bytes == null)
                return;
            var path = PathEx.Combine(cacheFilePath);
            if (!PathEx.IsValidPath(path))
                return;
            try
            {
                File.WriteAllBytes(path, bytes.Zip());
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     <para>
        ///         Loads the data of a cache file into memory.
        ///     </para>
        ///     <para>
        ///         Please note that <see cref="MaxCacheSize"/> is ignored in this case.
        ///     </para>
        /// </summary>
        /// <param name="cacheFilePath">
        ///     The full path of a cache file.
        /// </param>
        public static void LoadCache(string cacheFilePath)
        {
            var path = PathEx.Combine(cacheFilePath);
            if (!File.Exists(path))
                return;
            try
            {
                var cache = File.ReadAllBytes(path).Unzip()?.DeserializeObject<Dictionary<int, Dictionary<string, Dictionary<string, List<string>>>>>();
                if (cache == null)
                    return;
                foreach (var data in cache)
                {
                    var code = data.Key;
                    if (code == 0)
                        return;
                    InitializeCache(code);
                    CachedFiles[code] = data.Value;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Gets or sets the maximum number of cached files.
        /// </summary>
        public static int MaxCacheSize { get; set; } = 8;

        /// <summary>
        ///     Specifies a sequence of section names to be sorted first.
        /// </summary>
        public static IEnumerable<string> SortBySections { get; set; }

        private static Dictionary<string, Dictionary<string, List<string>>> SortHelper(this Dictionary<string, Dictionary<string, List<string>>> source)
        {
            var comparer = new Comparison.AlphanumericComparer();
            var sorted = source.OrderBy(x => !x.Key.Equals(NonSectionId))
                               .ThenBy(x => !SortBySections.ContainsEx(x.Key))
                               .ThenBy(x => x.Key, comparer).ToDictionary(x => x.Key, x => x.Value.SortHelper());
            return sorted;
        }

        private static Dictionary<string, List<string>> SortHelper(this Dictionary<string, List<string>> source)
        {
            var comparer = new Comparison.AlphanumericComparer();
            var sorted = source.OrderBy(d => d.Key, comparer).ToDictionary(d => d.Key, d => d.Value);
            return sorted;
        }

        private static List<string> SortHelper(this IEnumerable<string> source)
        {
            var comparer = new Comparison.AlphanumericComparer();
            var sorted = source.OrderBy(x => x, comparer).ToList();
            return sorted;
        }

        private static string _tmpFileGuid;

        private static string TmpFileGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(_tmpFileGuid))
                    return _tmpFileGuid;
                _tmpFileGuid = Guid.NewGuid().ToString();
                return _tmpFileGuid;
            }
        }

        private static string _filePath;

        /// <summary>
        ///     Gets or sets a default INI file.
        /// </summary>
        public static string FilePath
        {
            get { return _filePath ?? string.Empty; }
            set
            {
                _filePath = PathEx.Combine(value);
                if (File.Exists(_filePath))
                    return;
                try
                {
                    var fileDir = Path.GetDirectoryName(_filePath);
                    if (string.IsNullOrEmpty(fileDir))
                        return;
                    if (!Directory.Exists(fileDir))
                        Directory.CreateDirectory(fileDir);
                    File.Create(_filePath).Close();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }
        }

        /// <summary>
        ///     Gets the full path of the default INI file.
        /// </summary>
        public static string GetFile() =>
            FilePath;

        /// <summary>
        ///     Specifies an INI file to use as default.
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path.
        /// </param>
        public static bool SetFile(params string[] paths) =>
            File.Exists(FilePath = PathEx.Combine(paths));

        private static bool CodeExists(int code) =>
            code != -1 && CachedFiles?.ContainsKey(code) == true && CachedFiles[code]?.Any() == true;

        private static int GetCode(string fileOrContent)
        {
            var source = fileOrContent ?? GetFile();
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));
            var path = PathEx.Combine(source);
            if (!File.Exists(path))
                path = TmpFileGuid;
            var code = path?.ToLower().GetHashCode() ?? -1;
            return code;
        }

        /// <summary>
        ///     Removes the read content of an INI file from cache.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static bool Detach(string fileOrContent = null)
        {
            try
            {
                var code = GetCode(fileOrContent);
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                if (CachedFiles?.ContainsKey(code) == true)
                    CachedFiles.Remove(code);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        private static string FindSection(int code, string section)
        {
            var sct = section?.Trim();
            if (string.IsNullOrEmpty(sct))
                return NonSectionId;
            if (!CodeExists(code) || CachedFiles[code].ContainsKey(sct))
                return sct;
            var newSct = CachedFiles[code].Keys.FirstOrDefault(x => x.EqualsEx(sct));
            if (!string.IsNullOrEmpty(newSct) && !sct.Equals(newSct))
                return newSct;
            return sct;
        }

        private static bool SectionExists(int code, string section) =>
            !string.IsNullOrEmpty(section) && CodeExists(code) && CachedFiles[code]?.ContainsKey(section) == true && CachedFiles[code][section]?.Any() == true;

        /// <summary>
        ///     Retrieves all section names of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="sorted">
        ///     true to sort the sections; otherwise, false.
        /// </param>
        public static List<string> GetSections(string fileOrContent = null, bool sorted = true)
        {
            try
            {
                var code = GetCode(fileOrContent);
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                if (!CodeExists(code))
                    ReadAll(fileOrContent);
                if (CodeExists(code))
                {
                    var output = CachedFiles[code].Keys.ToList();
                    if (output.Contains(NonSectionId))
                        output.Remove(NonSectionId);
                    if (sorted)
                        output = output.SortHelper();
                    return output;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return new List<string>();
        }

        /// <summary>
        ///     Retrieves all section names of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="sorted">
        ///     true to sort the sections; otherwise, false.
        /// </param>
        public static List<string> GetSections(bool sorted) =>
            GetSections(null, sorted);

        private static bool RemoveSection(int code, string section)
        {
            if (!CodeExists(code))
                return true;
            var sct = FindSection(code, section);
            if (!CachedFiles[code].ContainsKey(sct))
                return true;
            CachedFiles[code].Remove(sct);
            return true;
        }

        /// <summary>
        ///     Removes the specified section including all associated keys of an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section to remove.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static bool RemoveSection(string section, string fileOrContent = null)
        {
            try
            {
                var code = GetCode(fileOrContent);
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                if (!CodeExists(code))
                    ReadAll(fileOrContent);
                return RemoveSection(code, section);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        private static string FindKey(int code, string section, string key)
        {
            var sct = FindSection(code, section);
            var ke = key?.Trim();
            if (string.IsNullOrEmpty(ke) || !SectionExists(code, sct) || CachedFiles[code][sct].ContainsKey(ke))
                return ke;
            var newKey = CachedFiles[code][sct].Keys.FirstOrDefault(x => x.EqualsEx(ke));
            if (!string.IsNullOrEmpty(newKey) && !ke.Equals(newKey))
                return newKey;
            return ke;
        }

        private static bool KeyExists(int code, string section, string key) =>
            !string.IsNullOrEmpty(key) && SectionExists(code, section) && CachedFiles[code][section].ContainsKey(key) && CachedFiles[code][section][key]?.Any() == true;

        /// <summary>
        ///     Retrieves all key names of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section to get the key names. The value must be NULL to get all the
        ///     key names of the specified fileOrContent parameter.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="sorted">
        ///     true to sort keys; otherwise, false.
        /// </param>
        public static List<string> GetKeys(string section, string fileOrContent = null, bool sorted = true)
        {
            try
            {
                var code = GetCode(fileOrContent);
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                if (!CodeExists(code))
                    ReadAll(fileOrContent);
                var sct = FindSection(code, section);
                if (SectionExists(code, sct))
                {
                    var output = CachedFiles[code][sct].Keys.ToList();
                    if (output.Contains(NonSectionId))
                        output.Remove(NonSectionId);
                    if (sorted)
                        output = output.SortHelper();
                    return output;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return new List<string>();
        }

        /// <summary>
        ///     Retrieves all key names of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section to get the key names. The value must be NULL to get all the
        ///     key names of the specified fileOrContent parameter.
        /// </param>
        /// <param name="sorted">
        ///     true to sort keys; otherwise, false.
        /// </param>
        public static List<string> GetKeys(string section, bool sorted) =>
            GetKeys(section, null, sorted);

        private static bool RemoveKey(int code, string section, string key)
        {
            var sct = FindSection(code, section);
            var ke = FindKey(code, sct, key);
            if (!KeyExists(code, sct, ke))
                return true;
            CachedFiles[code][sct].Remove(ke);
            return true;
        }

        /// <summary>
        ///     Removes the specified key from the specified section, of an INI file or an INI
        ///     file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key to remove.
        /// </param>
        /// <param name="key">
        ///     The name of the key to remove.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static bool RemoveKey(string section, string key, string fileOrContent = null)
        {
            try
            {
                var code = GetCode(fileOrContent);
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                if (CachedFiles?.ContainsKey(code) != true)
                    ReadAll(fileOrContent);
                return RemoveKey(code, section, key);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Retrieves the full content of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="sorted">
        ///     true to sort the sections and keys; otherwise, false.
        /// </param>
        public static Dictionary<string, Dictionary<string, List<string>>> ReadAll(string fileOrContent = null, bool sorted = false)
        {
            var output = new Dictionary<string, Dictionary<string, List<string>>>();
            try
            {
                var source = fileOrContent ?? GetFile();
                if (string.IsNullOrEmpty(source))
                    throw new ArgumentNullException(nameof(source));
                var path = PathEx.Combine(source);
                if (File.Exists(path))
                    source = File.ReadAllText(path);
                else
                    path = TmpFileGuid;
                var code = path?.ToLower().GetHashCode() ?? -1;
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));

                // Enforce INI format rules
                var lines = TextEx.FormatNewLine(source).SplitNewLine();
                for (var i = 0; i < lines.Length; i++)
                {
                    var line = lines[i].Trim();
                    if (line.StartsWith("[") && !line.EndsWith("]") && line.Contains("]") && line.IndexOf(']') > 1)
                        lines[i] = line.Substring(0, line.IndexOf(']') + 1);
                    else
                        lines[i] = line;
                }
                source = lines.Where(LineHasIniFormat).Join(Environment.NewLine);
                if (string.IsNullOrWhiteSpace(source))
                    throw new ArgumentNullException(nameof(source));
                if (!source.StartsWith("["))
                    source = $"[{NonSectionId}]{Environment.NewLine}{source}";
                if (!source.EndsWith(Environment.NewLine))
                    source += Environment.NewLine;

                var matches = Regex.Matches(source,
                    @"^                        # Beginning of the line
                      ((?:\[)                  # Section Start
                           (?<Section>[^\]]*)  # Actual Section text into Section Group
                       (?:\])                  # Section End then EOL/EOB
                       (?:[\r\n]{0,}|\Z))      # Match but don't capture the CRLF or EOB
                      (                        # Begin capture groups (Key Value Pairs)
                       (?!\[)                  # Stop capture groups if a [ is found; new section
                       (?<Key>[^=]*?)          # Any text before the =, matched few as possible
                       (?:=)                   # Get the = now
                       (?<Value>[^\r\n]*)      # Get everything that is not an Line Changes
                       (?:[\r\n]{0,4})         # MBDC \r\n
                      )*                       # End Capture groups",
                    RegexOptions.IgnoreCase |
                    RegexOptions.Multiline |
                    RegexOptions.IgnorePatternWhitespace);

                var sections = new Dictionary<string, Dictionary<string, List<string>>>();
                foreach (Match match in matches)
                {
                    var section = match.Groups["Section"]?.Value.Trim();
                    if (string.IsNullOrEmpty(section))
                        continue;
                    if (!sections.ContainsKey(section))
                    {
                        if (sections.Count > 0)
                        {
                            var newSection = sections.Keys.FirstOrDefault(x => x.EqualsEx(section));
                            if (!string.IsNullOrEmpty(newSection))
                                section = newSection;
                        }
                        if (!sections.ContainsKey(section))
                            sections.Add(section, new Dictionary<string, List<string>>());
                    }
                    var keys = new Dictionary<string, List<string>>();
                    for (var i = 0; i < match.Groups["Key"].Captures.Count; i++)
                    {
                        var key = match.Groups["Key"]?.Captures[i].Value.Trim();
                        if (string.IsNullOrEmpty(key))
                            continue;
                        var value = match.Groups["Value"]?.Captures[i].Value.Trim();
                        if (string.IsNullOrEmpty(value))
                            continue;
                        if (!keys.ContainsKey(key))
                        {
                            if (keys.Count > 0)
                            {
                                var newKey = keys.Keys.FirstOrDefault(x => x.EqualsEx(key));
                                if (!string.IsNullOrEmpty(newKey))
                                    key = newKey;
                            }
                            if (!keys.ContainsKey(key))
                                keys.Add(key, new List<string>());
                        }
                        keys[key].Add(value);
                    }
                    sections[section] = keys;
                }
                if (sorted)
                    sections = sections.SortHelper();
                output = sections;
                if (output.Count > 0)
                {
                    InitializeCache(code);
                    if (CachedFiles.Count >= MaxCacheSize)
                    {
                        var defCode = FilePath?.ToLower().GetHashCode() ?? -1;
                        var delCode = CachedFiles.Keys.First(x => x != defCode);
                        if (CodeExists(delCode))
                            CachedFiles.Remove(delCode);
                    }
                    CachedFiles[code] = output;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
        }

        private static bool LineHasIniFormat(this string str)
        {
            var s = str;
            if (string.IsNullOrWhiteSpace(s))
                return false;
            if (s.StartsWith("[") && s.EndsWith("]") && s.Count(x => x == '[') == 1 && s.Count(x => x == ']') == 1 && s.Any(char.IsLetterOrDigit))
                return true;
            var c = s.First();
            if (!char.IsLetterOrDigit(c) && !c.IsBetween('$', '/') && !c.IsBetween('<', '@') && !c.IsBetween('{', '~') && c != '!' && c != '"' && c != ':' && c != '^' && c != '_')
                return false;
            var i = s.IndexOf('=');
            return i > 0 && s.Substring(0, i).Any(char.IsLetterOrDigit) && s.Contains('=') && i + 1 < s.Length;
        }

        /// <summary>
        ///     Retrieves the full content of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="sorted">
        ///     true to sort the sections and keys; otherwise, false.
        /// </param>
        public static Dictionary<string, Dictionary<string, List<string>>> ReadAll(bool sorted) =>
            ReadAll(null, sorted);

        /// <summary>
        ///     Retrieves a <see cref="string"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="reread">
        ///     true to reread the INI file; otherwise, false.
        /// </param>
        /// <param name="index">
        ///     The value index used to handle multiple key value pairs.
        /// </param>
        public static string Read(string section, string key, string fileOrContent = null, bool reread = false, int index = 0)
        {
            try
            {
                var code = GetCode(fileOrContent);
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                if (reread || !CodeExists(code))
                    ReadAll(fileOrContent);
                if (!CodeExists(code))
                    throw new ArgumentNullException(nameof(fileOrContent));
                var sct = FindSection(code, section);
                if (string.IsNullOrEmpty(sct))
                    throw new ArgumentNullException(nameof(section));
                var ke = FindKey(code, sct, key);
                if (string.IsNullOrEmpty(ke))
                    throw new ArgumentNullException(nameof(key));
                if (KeyExists(code, sct, ke))
                {
                    var i = Math.Abs(index);
                    if (CachedFiles[code][sct][ke].Count > i)
                        return CachedFiles[code][sct][ke][i] ?? string.Empty;
                    return CachedFiles[code][sct][ke]?.FirstOrDefault() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return string.Empty;
        }

        /// <summary>
        ///     Retrieves a <see cref="string"/> value from the specified section in an INI file
        ///     or an INI file formatted string value and release all cached resources used by
        ///     the specified INI file or the INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="index">
        ///     The value index used to handle multiple key value pairs.
        /// </param>
        public static string ReadOnly(string section, string key, string fileOrContent, int index = 0)
        {
            var output = Read(section, key, fileOrContent, false, index);
            Detach(fileOrContent);
            return output;
        }

        /// <summary>
        ///     Retrieves a value from the specified section in an INI file or an INI file
        ///     formatted string value.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="section">
        ///     The name of the section containing the key name.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The value that is used as default.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="reread">
        ///     true to reread the INI file; otherwise, false.
        /// </param>
        public static TValue Read<TValue>(string section, string key, TValue defValue = default(TValue), string fileOrContent = null, bool reread = false)
        {
            try
            {
                object newValue;
                var strValue = Read(section, key, fileOrContent, reread);
                var type = typeof(TValue);
                if (string.IsNullOrEmpty(strValue))
                {
                    if (Log.DebugMode > 1)
                    {
                        var message = $"The value is not defined. (Section: '{section}'; Key: '{key}'; File: '{(fileOrContent ?? GetFile())?.EncodeToBase85()}';)";
                        throw new WarningException(message);
                    }
                    newValue = (object)defValue ?? string.Empty;
                }
                else if (strValue.StartsWith("\u0001Object\u0002") && strValue.EndsWith("\u0003"))
                {
                    var bytes = strValue.Substring(8).TrimEnd('\u0003').DecodeBytesFromBase85(null, null);
                    var unzipped = bytes?.Unzip();
                    if (unzipped != null)
                        bytes = unzipped;
                    newValue = bytes?.DeserializeObject<object>() ?? defValue;
                }
                else
                {
                    if (type == typeof(string))
                        newValue = strValue;
                    else if (type == typeof(Rectangle))
                        newValue = strValue.ToRectangle();
                    else if (type == typeof(Point))
                        newValue = strValue.ToPoint();
                    else if (type == typeof(Size))
                        newValue = strValue.ToSize();
                    else if (type == typeof(Version))
                        newValue = Version.Parse(strValue);
                    else if (!strValue.TryParse<TValue>(out newValue))
                        newValue = defValue;
                }
                return (TValue)newValue;
            }
            catch (FormatException)
            {
                // ignored
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return defValue;
        }

        /// <summary>
        ///     Retrieves a value from the specified section in an INI file or an INI file
        ///     formatted string value and release all cached resources used by the
        ///     specified INI file or the INI file formatted string value.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="section">
        ///     The name of the section containing the key name.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The value that is used as default.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="index">
        ///     The value index used to handle multiple key value pairs.
        /// </param>
        public static TValue ReadOnly<TValue>(string section, string key, TValue defValue, string fileOrContent, int index = 0)
        {
            var output = Read(section, key, defValue, fileOrContent);
            Detach(fileOrContent);
            return output;
        }

        /// <summary>
        ///     <para>
        ///         Retrieves a <see cref="string"/> value from the specified section in an INI
        ///         file.
        ///     </para>
        ///     <para>
        ///         The Win32-API without file caching is used for reading in this case.
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="file">
        ///     The full file path of an INI file.
        /// </param>
        public static string ReadDirect(string section, string key, string file = null)
        {
            var output = string.Empty;
            try
            {
                var path = PathEx.Combine(file ?? GetFile());
                if (!File.Exists(path))
                    throw new PathNotFoundException(path);
                var sb = new StringBuilder(short.MaxValue);
                if (WinApi.SafeNativeMethods.GetPrivateProfileString(section, key, string.Empty, sb, sb.Capacity, path) != 0)
                    output = sb.ToString();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
        }

        /// <summary>
        ///     Writes the specifed content to an INI file on the disk.
        /// </summary>
        /// <param name="content">
        ///     <para>
        ///         The content based on <see cref="ReadAll(string,bool)"/>.
        ///     </para>
        ///     <para>
        ///         If this parameter is NULL, the function writes all the cached data from the
        ///         specified INI file to the disk.
        ///     </para>
        /// </param>
        /// <param name="file">
        ///     <para>
        ///         The full file path of an INI file.
        ///     </para>
        ///     <para>
        ///         If this parameter is NULL, the default INI file is used.
        ///     </para>
        /// </param>
        /// <param name="sorted">
        ///     true to sort the sections and keys; otherwise, false.
        /// </param>
        /// <param name="detach">
        ///     true to release all cached resources used by the specified INI file; otherwise,
        ///     false.
        /// </param>
        public static bool WriteAll(Dictionary<string, Dictionary<string, List<string>>> content = null, string file = null, bool sorted = true, bool detach = false)
        {
            try
            {
                var path = PathEx.Combine(file ?? GetFile());
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));

                var code = path.ToLower().GetHashCode();
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));

                var source = content ?? new Dictionary<string, Dictionary<string, List<string>>>();
                if (source.Count == 0 && CodeExists(code))
                    source = CachedFiles[code];
                if (source.Count == 0 && File.Exists(path))
                    source = ReadAll(path);
                if (source.Count == 0 || source.Values.Count == 0)
                    throw new ArgumentNullException(nameof(source));
                if (sorted)
                    source = source.SortHelper();

                if (!File.Exists(path) && !PathEx.IsValidPath(path))
                    throw new ArgumentException(nameof(path));

                var hash = File.Exists(path) ? new Crypto.Md5().EncryptFile(path) : null;
                var temp = Path.GetRandomFileName();
                using (var sw = new StreamWriter(temp, true))
                    foreach (var dict in source)
                    {
                        if (string.IsNullOrWhiteSpace(dict.Key) || dict.Value.Count == 0)
                            continue;
                        if (!dict.Key.Equals(NonSectionId))
                        {
                            sw.Write('[');
                            sw.Write(dict.Key.Trim());
                            sw.Write(']');
                            sw.WriteLine();
                        }
                        foreach (var pair in dict.Value)
                        {
                            if (string.IsNullOrWhiteSpace(pair.Key) || pair.Value.Count == 0)
                                continue;
                            foreach (var value in pair.Value)
                            {
                                if (string.IsNullOrWhiteSpace(value))
                                    continue;
                                sw.Write(pair.Key.Trim());
                                sw.Write('=');
                                sw.Write(value.Trim());
                                sw.WriteLine();
                            }
                        }
                        sw.WriteLine();
                    }
                if (hash?.Equals(new Crypto.Md5().EncryptFile(temp)) == true)
                {
                    File.Delete(temp);
                    return true;
                }
                File.Delete(path);
                File.Move(temp, path);

                if (detach)
                    Detach(path);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Writes all the cached data from the specified INI file to the disk.
        /// </summary>
        /// <param name="file">
        ///     <para>
        ///         The full file path of an INI file.
        ///     </para>
        ///     <para>
        ///         If this parameter is NULL, the default INI file is used.
        ///     </para>
        /// </param>
        /// <param name="sorted">
        ///     true to sort the sections and keys; otherwise, false.
        /// </param>
        /// <param name="detach">
        ///     true to release all cached resources used by the specified INI file; otherwise,
        ///     false.
        /// </param>
        public static bool WriteAll(string file, bool sorted = true, bool detach = false) =>
            WriteAll(null, file, sorted);

        /// <summary>
        ///     <para>
        ///         Copies the specified value into the specified section of an INI file.
        ///     </para>
        ///     <para>
        ///         This function updates only the cache and has no effect on the file until
        ///         <see cref="WriteAll(string,bool,bool)"/> is called.
        ///     </para>
        /// </summary>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="section">
        ///     The name of the section to which the value will be copied.
        /// </param>
        /// <param name="key">
        ///     <para>
        ///         The name of the key to be associated with a value.
        ///     </para>
        ///     <para>
        ///         If this parameter is NULL, the entire section, including all entries within the
        ///         section, is deleted.
        ///     </para>
        /// </param>
        /// <param name="value">
        ///     <para>
        ///         The value to be written to the file.
        ///     </para>
        ///     <para>
        ///         If this parameter is NULL, the key pointed to by the key parameter is deleted.
        ///     </para>
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="forceOverwrite">
        ///     true to enable overwriting of a key with the same value as specified; otherwise,
        ///     false.
        /// </param>
        /// <param name="skipExistValue">
        ///     true to skip an existing value, even it is not the same value as specified;
        ///     otherwise, false.
        /// </param>
        /// <param name="index">
        ///     The value index used to handle multiple key value pairs.
        /// </param>
        public static bool Write<TValue>(string section, string key, TValue value, string fileOrContent = null, bool forceOverwrite = true, bool skipExistValue = false, int index = 0)
        {
            try
            {
                var code = GetCode(fileOrContent);
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));

                if (!CodeExists(code))
                    ReadAll(fileOrContent);

                var sct = FindSection(code, section);
                if (string.IsNullOrEmpty(sct))
                    throw new ArgumentNullException(nameof(section));
                if (sct.Any(TextEx.IsLineSeparator))
                    throw new ArgumentOutOfRangeException(nameof(section));

                var ke = FindKey(code, sct, key);
                if (ke.Any(TextEx.IsLineSeparator))
                    throw new ArgumentOutOfRangeException(nameof(key));

                var val = string.Empty;
                if (!string.IsNullOrEmpty(sct) && !string.IsNullOrEmpty(ke) && Comparison.IsNotEmpty(value))
                {
                    var str = value.ToString();
                    var type = typeof(TValue);
                    if (type.IsSerializable && (type.ToString() == str || $"({type.Name})" == str || str.Any(TextEx.IsLineSeparator)))
                    {
                        var bytes = value.SerializeObject();
                        var zipped = bytes?.Zip();
                        if (zipped?.Length < bytes?.Length)
                            bytes = zipped;
                        val = "\u0001Object\u0002" + bytes.EncodeToBase85(null, null) + "\u0003";
                    }
                    else
                        val = str;
                }

                if (!forceOverwrite || skipExistValue)
                {
                    var c = Read(section, key, fileOrContent, false, index);
                    if (!forceOverwrite && c == val || skipExistValue && !string.IsNullOrWhiteSpace(c))
                        return false;
                }

                var i = Math.Abs(index);
                if (string.IsNullOrEmpty(ke) || string.IsNullOrEmpty(val))
                {
                    if (!SectionExists(code, sct))
                        return true;
                    if (string.IsNullOrEmpty(ke))
                    {
                        RemoveSection(code, sct);
                        return true;
                    }
                    if (!KeyExists(code, sct, ke))
                        return true;
                    if (CachedFiles?[code][sct][ke].Count > i)
                    {
                        CachedFiles[code][sct][ke].RemoveAt(i);
                        return true;
                    }
                    RemoveKey(code, sct, ke);
                    return true;
                }

                InitializeCache(code, sct, ke);
                if (KeyExists(code, sct, ke))
                {
                    if (CachedFiles?[code][sct][ke].Count > i)
                    {
                        CachedFiles[code][sct][ke][i] = val;
                        return true;
                    }
                    if (CachedFiles?[code][sct][ke].Count != i)
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
                CachedFiles?[code][sct][ke].Add(val);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     <para>
        ///         Copies the specified value into the specified section of an INI file.
        ///     </para>
        ///     <para>
        ///         This function updates only the cache and has no effect on the file until
        ///         <see cref="WriteAll(string,bool,bool)"/> is called.
        ///     </para>
        /// </summary>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="section">
        ///     The name of the section to which the value will be copied.
        /// </param>
        /// <param name="key">
        ///     <para>
        ///         The name of the key to be associated with a value.
        ///     </para>
        ///     <para>
        ///         If this parameter is NULL, the entire section, including all entries within the
        ///         section, is deleted.
        ///     </para>
        /// </param>
        /// <param name="value">
        ///     <para>
        ///         The value to be written to the file.
        ///     </para>
        ///     <para>
        ///         If this parameter is NULL, the key pointed to by the key parameter is deleted.
        ///     </para>
        /// </param>
        /// <param name="forceOverwrite">
        ///     true to enable overwriting of a key with the same value as specified; otherwise,
        ///     false.
        /// </param>
        /// <param name="skipExistValue">
        ///     true to skip an existing value, even it is not the same value as specified;
        ///     otherwise, false.
        /// </param>
        /// <param name="index">
        ///     The value index used to handle multiple key value pairs.
        /// </param>
        public static bool Write<TValue>(string section, string key, TValue value, bool forceOverwrite, bool skipExistValue = false, int index = 0) =>
            Write(section, key, value, null, forceOverwrite, skipExistValue);

        /// <summary>
        ///     <para>
        ///         Copies the <see cref="string"/> representation of the specified <see cref="object"/>
        ///         value into the specified section of an INI file. If the file does not exist, it is
        ///         created.
        ///     </para>
        ///     <para>
        ///         The Win32-API is used for writing in this case. Please note that this function
        ///         writes the changes directly on the disk. This causes a lot of write accesses if
        ///         it is used incorrectly.
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section to which the value will be copied.
        /// </param>
        /// <param name="key">
        ///     <para>
        ///         The name of the key to be associated with a value.
        ///     </para>
        ///     <para>
        ///         If this parameter is NULL, the entire section, including all entries within the
        ///         section, is deleted.
        ///     </para>
        /// </param>
        /// <param name="value">
        ///     <para>
        ///         The value to be written to the file.
        ///     </para>
        ///     <para>
        ///         If this parameter is NULL, the key pointed to by the key parameter is deleted.
        ///     </para>
        /// </param>
        /// <param name="file">
        ///     The full path of an INI file.
        /// </param>
        /// <param name="forceOverwrite">
        ///     true to enable overwriting of a key with the same value as specified; otherwise,
        ///     false.
        /// </param>
        /// <param name="skipExistValue">
        ///     true to skip an existing value, even it is not the same value as specified;
        ///     otherwise, false.
        /// </param>
        public static bool WriteDirect(string section, string key, object value, string file = null, bool forceOverwrite = true, bool skipExistValue = false)
        {
            try
            {
                var path = PathEx.Combine(file ?? GetFile());
                if (string.IsNullOrWhiteSpace(section))
                    throw new ArgumentNullException(nameof(section));
                if (!File.Exists(path))
                {
                    if (string.IsNullOrWhiteSpace(key) || value == null || !Path.HasExtension(path) || !PathEx.IsValidPath(path))
                        throw new PathNotFoundException(path);
                    var dir = Path.GetDirectoryName(path);
                    if (string.IsNullOrWhiteSpace(dir))
                        throw new ArgumentNullException(nameof(dir));
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    File.Create(path).Close();
                }
                var strValue = value?.ToString();
                if (!forceOverwrite || skipExistValue)
                {
                    var curValue = ReadDirect(section, key, path);
                    if (!forceOverwrite && curValue.Equals(strValue) || skipExistValue && !string.IsNullOrWhiteSpace(curValue))
                        return false;
                }
                if (string.Concat(section, key, value).All(TextEx.IsAscii))
                    goto Write;
                var encoding = TextEx.GetEncoding(path);
                if (!encoding.Equals(Encoding.Unicode) && !encoding.Equals(Encoding.BigEndianUnicode))
                    TextEx.ChangeEncoding(path, Encoding.Unicode);
                Write:
                return WinApi.SafeNativeMethods.WritePrivateProfileString(section, key, strValue, path) != 0;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }
    }
}
