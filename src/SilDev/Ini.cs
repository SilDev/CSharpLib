#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Ini.cs
// Version:  2017-05-15 01:52
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
                var s = section?.Trim();
                if (string.IsNullOrWhiteSpace(section))
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
                if (!d.ContainsKey(s))
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
                var s = section?.Trim();
                if (string.IsNullOrWhiteSpace(section))
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
                if (!d.ContainsKey(s))
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
                    if (sorted)
                    {
                        var comparer = new Comparison.AlphanumericComparer();
                        var sort = output.OrderBy(x => x, comparer).ToList();
                        output = sort;
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
                var s = section?.Trim();
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
                    if (!d.ContainsKey(s))
                    {
                        var newSection = d.Keys.FirstOrDefault(x => x.EqualsEx(s));
                        if (!string.IsNullOrEmpty(newSection))
                            s = newSection;
                    }
                    if (d.ContainsKey(s) && d[s].Count > 0)
                    {
                        output = d[s].Keys.ToList();
                        if (sorted)
                        {
                            var comparer = new Comparison.AlphanumericComparer();
                            var sort = output.OrderBy(x => x, comparer).ToList();
                            output = sort;
                        }
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
                source = source.FormatNewLine()
                               .SplitNewLine()
                               .Select(s => s.Trim())
                               .Where(LineHasIniFormat)
                               .Join(Environment.NewLine);
                if (string.IsNullOrWhiteSpace(source))
                    throw new ArgumentNullException(nameof(source));
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
                    {
                        var comparer = new Comparison.AlphanumericComparer();
                        var sort = keys.OrderBy(d => d.Key, comparer).ToDictionary(d => d.Key, d => d.Value);
                        keys = sort;
                    }
                    sections[section] = keys;
                }
                if (sorted)
                {
                    var comparer = new Comparison.AlphanumericComparer();
                    var sort = sections.OrderBy(d => d.Key, comparer).ToDictionary(d => d.Key, d => d.Value);
                    sections = sort;
                }
                output = sections;
                if (output.Count > 0)
                {
                    if (CachedFiles == null)
                        CachedFiles = new Dictionary<int, Dictionary<string, Dictionary<string, List<string>>>>();
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
                var s = section?.Trim() ?? string.Empty;
                var path = PathEx.Combine(source);
                if (!File.Exists(path))
                    path = TmpFileGuid;
                var code = path.GetCode();
                if (code == -1)
                    throw new ArgumentOutOfRangeException(nameof(code));
                var d = !reread && CachedFiles?.ContainsKey(code) == true ? CachedFiles[code] : ReadAll(fileOrContent);
                if (d.Count > 0)
                {
                    if (!d.ContainsKey(s))
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
                dynamic d;
                var s = Read(section, key, fileOrContent);
                var t = typeof(TValue);
                if (string.IsNullOrEmpty(s))
                {
                    if (Log.DebugMode > 1)
                    {
                        var message = $"The value is not defined. (Section: '{(string.IsNullOrEmpty(section) ? "NULL" : section)}'; Key: '{(string.IsNullOrEmpty(key) ? "NULL" : key)}'; File: '{(string.IsNullOrEmpty(fileOrContent) ? GetFile() : PathEx.DirOrFileExists(fileOrContent) ? fileOrContent : "CONTENT" + fileOrContent.EncodeToBase85())}';)";
                        throw new WarningException(message);
                    }
                    d = defValue;
                }
                else if (t == typeof(string))
                    d = string.IsNullOrEmpty(s) && !string.IsNullOrEmpty(defValue as string) ? (dynamic)defValue : s;
                else if (t == typeof(string[]) ||
                         t == typeof(List<string>) ||
                         t == typeof(IEnumerable<string>))
                {
                    var tmp = s.FromHexStringToByteArray()?.TextFromZip()?
                               .Split('\0').Reverse().Skip(1).Reverse()
                               .Select(x => x?.FromHexString());
                    if (t == typeof(string[]))
                        d = tmp?.ToArray();
                    else if (t == typeof(List<string>))
                        d = tmp?.ToList();
                    else
                        d = tmp;
                }
                else if (t == typeof(byte[]))
                    d = s.FromHexStringToByteArray();
                else if (t == typeof(Bitmap) ||
                         t == typeof(Image))
                    d = s.FromHexStringToImage();
                else if (t == typeof(Icon))
                    d = s.FromHexStringToIcon();
                else if (t == typeof(Rectangle))
                    d = s.ToRectangle();
                else if (t == typeof(Point))
                    d = s.ToPoint();
                else if (t == typeof(Size))
                    d = s.ToSize();
                else if (t == typeof(Version))
                    d = Version.Parse(s);
                else if (!s.TryParse<TValue>(out d))
                    d = defValue;
                output = d;
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
                    var sort = source.OrderBy(d => d.Key, comparer)
                                     .ToDictionary(d => d.Key, d => d.Value
                                                                     .OrderBy(p => p.Key, comparer)
                                                                     .ToDictionary(p => p.Key, p => p.Value));
                    source = sort;
                }
                var sb = new StringBuilder();
                foreach (var dict in source)
                {
                    if (string.IsNullOrWhiteSpace(dict.Key) || dict.Value.Count == 0)
                        continue;
                    sb.Append('[');
                    sb.Append(dict.Key.Trim());
                    sb.Append(']');
                    sb.AppendLine();
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
                var s = section?.Trim();
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

                var sb = new StringBuilder();
                var v = string.Empty;
                if (value != null)
                {
                    // To allow the saving of a sequence of strings,
                    // it's required to bring it in a single string
                    var t = typeof(TValue);
                    if (t == typeof(string[]) ||
                        t == typeof(List<string>) ||
                        t == typeof(IEnumerable<string>))
                    {
                        var sa = t == typeof(string[]) ? value as string[] : (value as IEnumerable<string>)?.ToArray();
                        if (sa != null)
                        {
                            // Convert each part into hexadecimal to make sure there are no null
                            // terminated character in it; concatenates the result using the null
                            // terminated character as separator, in the next step
                            var str = sa.Select(x => x.ToHexString()).Join('\0');
                            if (!str.Contains('\0'))
                                str += '\0';
                            // To shorten it, zip the result and save it in hexadecimal
                            sb.Append(str.TextToZip().ToHexString());
                        }
                    }
                    // No special conversion for anything else
                    else if (t == typeof(Bitmap) ||
                             t == typeof(Image) ||
                             t == typeof(Icon) ||
                             t == typeof(byte[]))
                    {
                        byte[] ba;
                        if (t == typeof(Bitmap) ||
                            t == typeof(Image))
                            ba = (value as Bitmap)?.ToByteArray();
                        else if (t == typeof(Icon))
                            ba = (value as Icon)?.ToByteArray();
                        else
                            ba = value as byte[];
                        if (ba != null)
                            sb.Append(ba.ToHexString());
                    }
                    else
                        sb.Append(value);

                    if (sb.Length > 0)
                        v = sb.ToString();
                }

                if (!forceOverwrite || skipExistValue)
                {
                    var c = Read(section, key, fileOrContent);
                    if (!forceOverwrite && c == v || skipExistValue && !string.IsNullOrWhiteSpace(c))
                        return false;
                }

                var i = (int)index;
                var k = key?.Trim();
                if (CachedFiles?.ContainsKey(code) == true && CachedFiles[code].Count > 0)
                {
                    // To find the correct section
                    if (!CachedFiles[code].ContainsKey(s))
                    {
                        var newSection = CachedFiles[code].Keys.FirstOrDefault(x => x.EqualsEx(s));
                        if (!string.IsNullOrEmpty(newSection))
                            s = newSection;
                    }

                    // To remove key value pairs
                    if (string.IsNullOrEmpty(k) || string.IsNullOrEmpty(v))
                    {
                        if (!CachedFiles[code].ContainsKey(s))
                            return true;
                        if (string.IsNullOrEmpty(k))
                            CachedFiles[code][s].Clear();
                        else if (string.IsNullOrEmpty(v))
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

                    // To find the correct key
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
                if (string.IsNullOrEmpty(v))
                    throw new ArgumentNullException(nameof(value));

                // To write or overwrite the value to the cache
                if (CachedFiles == null)
                    CachedFiles = new Dictionary<int, Dictionary<string, Dictionary<string, List<string>>>>();
                if (!CachedFiles.ContainsKey(code))
                    CachedFiles.Add(code, new Dictionary<string, Dictionary<string, List<string>>>());
                if (!CachedFiles[code].ContainsKey(s))
                    CachedFiles[code].Add(s, new Dictionary<string, List<string>>());
                if (!CachedFiles[code][s].ContainsKey(k))
                    CachedFiles[code][s].Add(k, new List<string>());
                if (CachedFiles[code][s][k].Count > i)
                {
                    CachedFiles[code][s][k][i] = v;
                    return true;
                }
                if (CachedFiles[code][s][k].Count != i)
                    throw new ArgumentOutOfRangeException(nameof(index));
                CachedFiles[code][s][k].Add(v);
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
                    if (string.IsNullOrWhiteSpace(key) || value == null || !PathEx.IsValidPath(path))
                        throw new PathNotFoundException(path);
                    File.Create(path).Close();
                }
                var strValue = value?.ToString();
                if (forceOverwrite && !skipExistValue)
                    return WinApi.SafeNativeMethods.WritePrivateProfileString(section, key, strValue, path) != 0;
                var curValue = ReadDirect(section, key, path);
                if (!forceOverwrite && curValue.Equals(strValue) || skipExistValue && !string.IsNullOrWhiteSpace(curValue))
                    return false;
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
