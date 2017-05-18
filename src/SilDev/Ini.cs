#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Ini.cs
// Version:  2017-05-18 14:21
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
    using Properties;

    /// <summary>
    ///     Provides static methods for the handling of initialization files.
    /// </summary>
    public static class Ini
    {
        private static volatile Dictionary<int, Dictionary<string, Dictionary<string, List<string>>>> _cachedFiles;

        private static Dictionary<int, Dictionary<string, Dictionary<string, List<string>>>> CachedFiles
        {
            get { return _cachedFiles; }
            set { _cachedFiles = value; }
        }

        private const int MaxCacheSize = 8;

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

        private static Dictionary<string, Dictionary<string, List<string>>> SortHelper(this Dictionary<string, Dictionary<string, List<string>>> source)
        {
            var comparer = new Comparison.AlphanumericComparer();
            var sorted = source.OrderBy(x => !x.Key.Equals(Resources.Ini_NonSection)).ThenBy(x => x.Key, comparer)
                               .ToDictionary(x => x.Key, x => x.Value.OrderBy(y => y.Key, comparer)
                                                               .ToDictionary(y => y.Key, y => y.Value));
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

        private static volatile string _tmpFileGuid;

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

        private static volatile string _filePath;

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
                    if (!Directory.Exists(fileDir))
                    {
                        if (string.IsNullOrEmpty(fileDir))
                            return;
                        Directory.CreateDirectory(fileDir);
                    }
                    File.Create(_filePath).Close();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }
        }

        private static int GetCode(this string str) =>
            str?.ToLower().GetHashCode() ?? -1;

        /// <summary>
        ///     Gets the full path of the default INI file.
        /// </summary>
        public static string GetFile() =>
            FilePath;

        /// <summary>
        ///     Specifies an INI file to use as default.
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path (environment variables are accepted).
        /// </param>
        public static bool SetFile(params string[] paths) =>
            File.Exists(FilePath = PathEx.Combine(paths));

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
                var source = fileOrContent ?? GetFile();
                if (string.IsNullOrWhiteSpace(source))
                    throw new ArgumentNullException(nameof(source));
                var path = PathEx.Combine(source);
                if (!File.Exists(path))
                    path = TmpFileGuid;
                var code = path.GetCode();
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

        /// <summary>
        ///     Removes the full specified section of an INI file or an INI file formatted
        ///     string value.
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
                var source = fileOrContent ?? GetFile();
                if (string.IsNullOrEmpty(source))
                    throw new ArgumentNullException(nameof(source));
                var s = section?.Trim() ?? Resources.Ini_NonSection;
                if (string.IsNullOrEmpty(section))
                    throw new ArgumentNullException(nameof(section));
                var path = PathEx.Combine(source);
                if (!File.Exists(path))
                    path = TmpFileGuid;
                var code = path.GetCode();
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                var d = CachedFiles?.ContainsKey(code) == true ? CachedFiles[code] : ReadAll(fileOrContent);
                if (CachedFiles?.ContainsKey(code) != true || d.Count == 0)
                    return true;
                if (!s.Equals(Resources.Ini_NonSection) && !d.ContainsKey(s))
                {
                    var newSection = d.Keys.FirstOrDefault(x => x.EqualsEx(s));
                    if (!string.IsNullOrEmpty(newSection))
                        s = newSection;
                }
                if (!d.ContainsKey(s))
                    return true;
                d.Remove(s);
                CachedFiles[code] = d;
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
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
                var source = fileOrContent ?? GetFile();
                if (string.IsNullOrEmpty(source))
                    throw new ArgumentNullException(nameof(source));
                var s = section?.Trim() ?? Resources.Ini_NonSection;
                if (string.IsNullOrEmpty(section))
                    throw new ArgumentNullException(nameof(section));
                var k = key?.Trim();
                if (string.IsNullOrEmpty(k))
                    throw new ArgumentNullException(nameof(key));
                var path = PathEx.Combine(source);
                if (!File.Exists(path))
                    path = TmpFileGuid;
                var code = path.GetCode();
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                var d = CachedFiles?.ContainsKey(code) == true ? CachedFiles[code] : ReadAll(fileOrContent);
                if (CachedFiles?.ContainsKey(code) != true || d.Count == 0)
                    return true;
                if (!s.Equals(Resources.Ini_NonSection) && !d.ContainsKey(s))
                {
                    var newSection = d.Keys.FirstOrDefault(x => x.EqualsEx(s));
                    if (!string.IsNullOrEmpty(newSection))
                        s = newSection;
                }
                if (d.ContainsKey(s) && !d[s].ContainsKey(k))
                {
                    var newKey = d[s].Keys.FirstOrDefault(x => x.EqualsEx(k));
                    if (!string.IsNullOrEmpty(newKey))
                        k = newKey;
                }
                if (!d.ContainsKey(s) || !d[s].ContainsKey(k))
                    return true;
                d[s].Remove(k);
                CachedFiles[code] = d;
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

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
            var output = new List<string>();
            try
            {
                var source = fileOrContent ?? GetFile();
                if (string.IsNullOrEmpty(source))
                    throw new ArgumentNullException(nameof(source));
                var path = PathEx.Combine(source);
                if (!File.Exists(path))
                    path = TmpFileGuid;
                var code = path.GetCode();
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                var d = CachedFiles?.ContainsKey(code) == true ? CachedFiles[code] : ReadAll(fileOrContent);
                if (d.Count > 0)
                {
                    output = d.Keys.ToList();
                    if (output.Contains(Resources.Ini_NonSection))
                        output.Remove(Resources.Ini_NonSection);
                    if (sorted)
                        output = output.SortHelper();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
        }

        /// <summary>
        ///     Retrieves all section names of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="sorted">
        ///     true to sort the sections; otherwise, false.
        /// </param>
        public static List<string> GetSections(bool sorted) =>
            GetSections(null, sorted);

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
            var output = new List<string>();
            try
            {
                var source = fileOrContent ?? GetFile();
                if (string.IsNullOrEmpty(source))
                    throw new ArgumentNullException(nameof(source));
                var s = section?.Trim() ?? Resources.Ini_NonSection;
                if (string.IsNullOrEmpty(s))
                    throw new ArgumentNullException(nameof(section));
                var path = PathEx.Combine(source);
                if (!File.Exists(path))
                    path = TmpFileGuid;
                var code = path.GetCode();
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                var d = CachedFiles?.ContainsKey(code) == true ? CachedFiles[code] : ReadAll(fileOrContent);
                if (d.Count > 0)
                {
                    if (!s.Equals(Resources.Ini_NonSection) && !d.ContainsKey(s))
                    {
                        var newSection = d.Keys.FirstOrDefault(x => x.EqualsEx(s));
                        if (!string.IsNullOrEmpty(newSection))
                            s = newSection;
                    }
                    if (d.ContainsKey(s) && d[s].Count > 0)
                    {
                        output = d[s].Keys.ToList();
                        if (sorted)
                            output = output.SortHelper();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
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
                var code = path.GetCode();
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
                    source = $"[{Resources.Ini_NonSection}]{Environment.NewLine}{source}";
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
                    if (sorted)
                        keys = keys.SortHelper();
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
                        var defCode = FilePath.GetCode();
                        CachedFiles.Remove(CachedFiles.Keys.First(x => x != defCode));
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
            if (s.StartsWith("[") && s.EndsWith("]") && s.Count(c => c == '[') == 1 && s.Count(c => c == ']') == 1 && s.Count(char.IsLetterOrDigit) > 0)
                return true;
            return !s.StartsWith("=") && s.Contains('=') && s.IndexOf('=') + 1 < s.Length;
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
        public static string Read(string section, string key, string fileOrContent = null, bool reread = false)
        {
            var output = string.Empty;
            try
            {
                var source = fileOrContent ?? GetFile();
                if (string.IsNullOrEmpty(source))
                    throw new ArgumentNullException(nameof(source));
                var k = key?.Trim();
                if (string.IsNullOrEmpty(k))
                    throw new ArgumentNullException(nameof(key));
                var s = section?.Trim() ?? Resources.Ini_NonSection;
                var path = PathEx.Combine(source);
                if (!File.Exists(path))
                    path = TmpFileGuid;
                var code = path.GetCode();
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                var d = !reread && CachedFiles?.ContainsKey(code) == true ? CachedFiles[code] : ReadAll(fileOrContent);
                if (d.Count > 0)
                {
                    if (!s.Equals(Resources.Ini_NonSection) && !d.ContainsKey(s))
                    {
                        var newSection = d.Keys.FirstOrDefault(x => x.EqualsEx(s));
                        if (!string.IsNullOrEmpty(newSection))
                            s = newSection;
                    }
                    if (d.ContainsKey(s))
                    {
                        if (!d[s].ContainsKey(k))
                        {
                            var newKey = d[s].Keys.FirstOrDefault(x => x.EqualsEx(k));
                            if (!string.IsNullOrEmpty(newKey))
                                k = newKey;
                        }
                        if (d[s].ContainsKey(k) && d[s][k].Count > 0)
                            output = d[s][k].FirstOrDefault() ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
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
        public static TValue Read<TValue>(string section, string key, TValue defValue = default(TValue), string fileOrContent = null)
        {
            dynamic output = defValue;
            try
            {
                dynamic newValue;
                var strValue = Read(section, key, fileOrContent);
                var type = typeof(TValue);
                if (string.IsNullOrEmpty(strValue))
                {
                    if (Log.DebugMode > 1)
                    {
                        var message = $"The value is not defined. (Section: '{section}'; Key: '{key}'; File: '{(fileOrContent ?? GetFile())?.EncodeToBase85()}';)";
                        throw new WarningException(message);
                    }
                    newValue = defValue;
                }
                else if (type == typeof(string))
                    newValue = string.IsNullOrEmpty(strValue) && !string.IsNullOrEmpty(defValue as string) ? (dynamic)defValue : strValue;
                else if (type == typeof(string[]) || type == typeof(List<string>) || type == typeof(IEnumerable<string>))
                {
                    var sequence = strValue.FromHexStringToByteArray()?.TextFromZip()?.Split('\0')
                                           .Reverse().Skip(1).Reverse().Select(x => x?.FromHexString());
                    if (type == typeof(string[]))
                        newValue = sequence?.ToArray();
                    else if (type == typeof(List<string>))
                        newValue = sequence?.ToList();
                    else
                        newValue = sequence;
                }
                else if (type == typeof(byte[]))
                    newValue = strValue.FromHexStringToByteArray();
                else if (type == typeof(Bitmap) || type == typeof(Image))
                    newValue = strValue.FromHexStringToImage();
                else if (type == typeof(Icon))
                    newValue = strValue.FromHexStringToIcon();
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
                output = newValue;
            }
            catch (FormatException)
            {
                // ignored
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
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
        public static TValue ReadOnly<TValue>(string section, string key, TValue defValue = default(TValue), string fileOrContent = null)
        {
            var output = Read(section, key, defValue, fileOrContent);
            Detach(fileOrContent);
            return output;
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
        public static string ReadOnly(string section, string key, string fileOrContent = null)
        {
            var output = Read(section, key, fileOrContent);
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
        ///     The full file path of an INI file.
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
                var source = content ?? new Dictionary<string, Dictionary<string, List<string>>>();
                var path = PathEx.Combine(file ?? GetFile());
                var code = path.GetCode();
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                if (source.Count == 0 && CachedFiles?.ContainsKey(code) == true)
                    source = CachedFiles[code];
                if (source.Count == 0 && File.Exists(path))
                    source = ReadAll(path);
                if (source.Count == 0 || source.Values.Count == 0)
                    throw new ArgumentNullException(nameof(source));
                if (!File.Exists(path))
                    File.Create(path).Close();
                if (sorted)
                {
                    var comparer = new Comparison.AlphanumericComparer();
                    var sort = source.OrderBy(d => !d.Key.Equals(Resources.Ini_NonSection))
                                     .ThenBy(d => d.Key, comparer)
                                     .ToDictionary(d => d.Key, d => d.Value
                                                                     .OrderBy(p => p.Key, comparer)
                                                                     .ToDictionary(p => p.Key, p => p.Value));
                    source = sort;
                }

                var hash = new Crypto.Md5().EncryptFile(path);
                var temp = path + ".new";
                File.CreateText(temp).Close();
                using (var sw = new StreamWriter(temp, true))
                    foreach (var dict in source)
                    {
                        if (string.IsNullOrWhiteSpace(dict.Key) || dict.Value.Count == 0)
                            continue;
                        if (!dict.Key.Equals(Resources.Ini_NonSection))
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
                            var key = pair.Key.Trim();
                            foreach (var value in pair.Value)
                            {
                                if (string.IsNullOrWhiteSpace(value))
                                    continue;
                                sw.Write(key);
                                sw.Write('=');
                                sw.Write(value.Trim());
                                sw.WriteLine();
                            }
                        }
                        sw.WriteLine();
                    }
                if (hash.Equals(new Crypto.Md5().EncryptFile(temp)))
                {
                    File.Delete(temp);
                    return true;
                }
                File.Delete(path);
                File.Move(temp, path);
                Data.SetAttributes(path, FileAttributes.Normal);

                /*
                var sb = new StringBuilder();
                foreach (var dict in source)
                {
                    if (string.IsNullOrWhiteSpace(dict.Key) || dict.Value.Count == 0)
                        continue;
                    if (!dict.Key.Equals(NonSectionGuid))
                    {
                        sb.Append('[');
                        sb.Append(dict.Key.Trim());
                        sb.Append(']');
                        sb.AppendLine();
                    }
                    foreach (var pair in dict.Value)
                    {
                        if (string.IsNullOrWhiteSpace(pair.Key) || pair.Value.Count == 0)
                            continue;
                        var key = pair.Key.Trim();
                        foreach (var value in pair.Value)
                        {
                            if (string.IsNullOrWhiteSpace(value))
                                continue;
                            sb.Append(key);
                            sb.Append('=');
                            sb.Append(value.Trim());
                            sb.AppendLine();
                        }
                    }
                    sb.AppendLine();
                }
                File.WriteAllText(path, sb.ToString());
                */

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
        ///     The full file path of an INI file.
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
        public static bool Write<TValue>(string section, string key, TValue value, string fileOrContent = null, bool forceOverwrite = true, bool skipExistValue = false, uint index = 0)
        {
            try
            {
                var s = section?.Trim() ?? Resources.Ini_NonSection;
                if (string.IsNullOrEmpty(s))
                    throw new ArgumentNullException(nameof(section));

                int code;
                var path = PathEx.Combine(fileOrContent ?? GetFile());
                if (File.Exists(path))
                {
                    code = path.GetCode();
                    if (code == -1)
                        throw new ArgumentOutOfRangeException(nameof(code));
                    if (CachedFiles?.ContainsKey(code) != true)
                        ReadAll(fileOrContent);
                }
                else
                {
                    path = TmpFileGuid;
                    code = path.GetCode();
                    if (code == -1)
                        throw new ArgumentOutOfRangeException(nameof(code));
                }

                var newValue = string.Empty;
                if (value != null)
                {
                    // To allow the saving of a sequence of strings,
                    // it's required to bring it in a single string
                    var type = typeof(TValue);
                    if (type == typeof(string[]) || type == typeof(List<string>) || type == typeof(IEnumerable<string>))
                    {
                        var sa = type == typeof(string[]) ? value as string[] : (value as IEnumerable<string>)?.ToArray();
                        if (sa != null)
                        {
                            // Convert each part into hexadecimal to make sure there are no null
                            // terminated character in it; concatenates the result using the null
                            // terminated character as separator, in the next step
                            var str = sa.Select(x => x.ToHexString()).Join('\0');
                            if (!str.Contains('\0'))
                                str += '\0';
                            // To shorten it, zip the result and save it in hexadecimal
                            newValue = str.TextToZip().ToHexString();
                        }
                    }
                    // No special conversion for anything else
                    else if (type == typeof(Bitmap) || type == typeof(Image) || type == typeof(Icon) || type == typeof(byte[]))
                    {
                        byte[] ba;
                        if (type == typeof(Bitmap) || type == typeof(Image))
                            ba = (value as Bitmap)?.ToByteArray();
                        else if (type == typeof(Icon))
                            ba = (value as Icon)?.ToByteArray();
                        else
                            ba = value as byte[];
                        if (ba != null)
                            newValue = ba.ToHexString();
                    }
                    else
                        newValue = value.ToString();
                }

                if (!forceOverwrite || skipExistValue)
                {
                    var c = Read(section, key, fileOrContent);
                    if (!forceOverwrite && c == newValue || skipExistValue && !string.IsNullOrWhiteSpace(c))
                        return false;
                }

                var i = (int)index;
                var k = key?.Trim();
                if (CachedFiles?.ContainsKey(code) == true && CachedFiles[code].Count > 0)
                {
                    // Find the correct section
                    if (!s.Equals(Resources.Ini_NonSection) && !CachedFiles[code].ContainsKey(s))
                    {
                        var newSection = CachedFiles[code].Keys.FirstOrDefault(x => x.EqualsEx(s));
                        if (!string.IsNullOrEmpty(newSection))
                            s = newSection;
                    }

                    // Remove key value pairs
                    if (string.IsNullOrEmpty(k) || string.IsNullOrEmpty(newValue))
                    {
                        if (!CachedFiles[code].ContainsKey(s))
                            return true;
                        if (string.IsNullOrEmpty(k))
                            CachedFiles[code][s].Clear();
                        else if (string.IsNullOrEmpty(newValue))
                        {
                            if (!CachedFiles[code][s].ContainsKey(k))
                                return true;
                            if (CachedFiles[code][s][k].Count > 1 && CachedFiles[code][s][k].Count > i)
                                CachedFiles[code][s][k] = CachedFiles[code][s][k].Skip(i).ToList();
                            else
                                CachedFiles[code][s].Remove(k);
                        }
                        return true;
                    }

                    // Find the correct key
                    if (CachedFiles[code].ContainsKey(s))
                        if (!CachedFiles[code][s].ContainsKey(k))
                        {
                            var newKey = CachedFiles[code][s].Keys.FirstOrDefault(x => x.EqualsEx(k));
                            if (!string.IsNullOrEmpty(newKey))
                                k = newKey;
                        }
                }

                if (string.IsNullOrEmpty(k))
                    throw new ArgumentNullException(nameof(key));
                if (string.IsNullOrEmpty(newValue))
                    throw new ArgumentNullException(nameof(value));

                // Finally write the value in cache
                InitializeCache(code, s, k);
                if (CachedFiles[code][s][k].Count > i)
                {
                    CachedFiles[code][s][k][i] = newValue;
                    return true;
                }
                if (CachedFiles[code][s][k].Count != i)
                    throw new ArgumentOutOfRangeException(nameof(index));
                CachedFiles[code][s][k].Add(newValue);
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
        public static bool Write<TValue>(string section, string key, TValue value, bool forceOverwrite, bool skipExistValue = false, uint index = 0) =>
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
                if (forceOverwrite && !skipExistValue)
                {
                    if (string.Concat(section, key, value).All(TextEx.IsAscii))
                        goto Write;
                    var encoding = TextEx.GetEncoding(path);
                    if (encoding.Equals(Encoding.Unicode) || encoding.Equals(Encoding.BigEndianUnicode))
                        goto Write;
                    TextEx.ChangeEncoding(path, Encoding.Unicode);
                    goto Write;
                }
                var curValue = ReadDirect(section, key, path);
                if (!forceOverwrite && curValue.Equals(strValue) || skipExistValue && !string.IsNullOrWhiteSpace(curValue))
                    return false;
                Write:
                return WinApi.SafeNativeMethods.WritePrivateProfileString(section, key, strValue, file) != 0;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }
    }
}
