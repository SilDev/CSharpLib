#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Ini.cs
// Version:  2020-01-21 20:23
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Provides the functionality to handle the INI format.
    /// </summary>
    public sealed class Ini
    {
        /// <summary>
        ///     The default pattern for parsing an INI document that allows blank sections.
        /// </summary>
        public const string ParsePatternDefault = @"^((?:\[)(?<Section>[^\]]*)(?:\])(?:[\r\n]{0,}|\Z))((?!\[)(?<Key>[^=]*?)(?:=)(?<Value>[^\r\n]*)(?:[\r\n]{0,4}))*";

        /// <summary>
        ///     The pattern for parsing an INI document that does not allow blank sections.
        /// </summary>
        public const string ParsePatternStrict = @"^((?:\[)(?<Section>[^\]]*)(?:\])(?:[\r\n]{0,}|\Z))((?!\[)(?<Key>[^=]*?)(?:=)(?<Value>[^\r\n]*)(?:[\r\n]{0,4}))+";

        private const string EncodeTagOpen = "<base64 reason=\"LineSeparator\">",
                             EncodeTagClose = "</base64>";

        /// <summary>
        ///     Gets the raw object that is used to manage the complete data of this
        ///     <see cref="Ini"/> instance.
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<string>>> ReadOnlyDocument =>
            Document.ToDictionary(p => p.Key, p => (IReadOnlyDictionary<string, IReadOnlyList<string>>)p.Value.ToDictionary(x => x.Key, x => (IReadOnlyList<string>)x.Value));

        /// <summary>
        ///     Gets the value that determines how sections and keys are compared
        ///     internally in this <see cref="Ini"/> instance to allow duplicates if
        ///     case-sensitive, or to avoid if case-insensitive.
        /// </summary>
        public StringComparer Comparer { get; private set; }

        /// <summary>
        ///     Gets the value that determines whether this <see cref="Ini"/> instance is
        ///     saved with ordered sections and keys.
        /// </summary>
        public bool Sorted { get; private set; }

        /// <summary>
        ///     Gets or sets the file path of this <see cref="Ini"/> instance.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///     Gets or sets the value under the specified index of the specified key in
        ///     the specified section of this <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to handle values of
        ///     a non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        /// <param name="index">
        ///     The index of the key whose the value is associated.
        /// </param>
        public string this[string section, string key, int index]
        {
            get => GetValue(section, key, index);
            set => AddOrUpdate(section, key, index, value);
        }

        /// <summary>
        ///     Gets or sets the value under the first index of the specified key in the
        ///     specified section of this <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to handle values of
        ///     a non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        public string this[string section, string key]
        {
            get => GetValue(section, key);
            set => AddOrUpdate(section, key, value);
        }

        private static string NonSectionKey { get; } = Guid.NewGuid().Encrypt();

        private static AlphaNumericComparer SortComparer { get; } = new AlphaNumericComparer();

        private IDictionary<string, IDictionary<string, IList<string>>> Document { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Ini"/> class with the
        ///     specified parameters.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        /// <param name="ignoreCase">
        ///     <see langword="true"/> to ignore the case of sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="sorted">
        ///     <see langword="true"/> to sort the sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public Ini(string fileOrContent, bool ignoreCase = true, bool sorted = true) =>
            Load(fileOrContent, ignoreCase, sorted);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Ini"/> class.
        /// </summary>
        public Ini() : this(null) { }

        /// <summary>
        ///     Loads the full content of an INI file or an INI file formatted string value
        ///     into this <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        /// <param name="ignoreCase">
        ///     <see langword="true"/> to ignore the case of sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="sorted">
        ///     <see langword="true"/> to sort the sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public void Load(string fileOrContent, bool ignoreCase = true, bool sorted = true)
        {
            if (!string.IsNullOrEmpty(fileOrContent))
            {
                if (FileEx.Exists(fileOrContent))
                    FilePath = fileOrContent;
                Comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
                Sorted = sorted;
                if (TryParse(fileOrContent, ignoreCase, sorted, out var content))
                {
                    Document = content;
                    return;
                }
            }
            Document = new Dictionary<string, IDictionary<string, IList<string>>>(Comparer);
        }

        /// <summary>
        ///     Loads the content of the file at <see cref="FilePath"/> into this
        ///     <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="ignoreCase">
        ///     <see langword="true"/> to ignore the case of sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="sorted">
        ///     <see langword="true"/> to sort the sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public void Load(bool ignoreCase = true, bool sorted = true) =>
            Load(FilePath, ignoreCase, sorted);

        /// <summary>
        ///     Saves content of this <see cref="Ini"/> instance to the specified path.
        /// </summary>
        /// <param name="path">
        ///     The path to file to be written.
        /// </param>
        /// <param name="setNewFilePath">
        ///     <see langword="true"/> to set a new value of the <see cref="FilePath"/>
        ///     property; otherwise, <see langword="false"/>.
        /// </param>
        public void Save(string path, bool setNewFilePath = false)
        {
            if (setNewFilePath)
                FilePath = path;
            WriteAll(Document, path, Sorted);
        }

        /// <summary>
        ///     Saves content of this <see cref="Ini"/> instance to the path at
        ///     <see cref="FilePath"/>.
        /// </summary>
        public void Save() =>
            WriteAll(Document, FilePath, Sorted);

        /// <summary>
        ///     Gets a collection of all sections of this <see cref="Ini"/> instance.
        /// </summary>
        public ICollection<string> GetSections() =>
            Document.Keys;

        /// <summary>
        ///     Gets a collection of all keys under the specified section of this
        ///     <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section to get the keys. Must be <see langword="null"/> to
        ///     retrieve the non-section keys.
        /// </param>
        public ICollection<string> GetKeys(string section) =>
            Document?.ContainsKey(section ??= string.Empty) == true ? Document[section].Keys : Array.Empty<string>();

        /// <summary>
        ///     Gets a list of all values under the specified key of the specified section
        ///     of this <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key. Must be <see langword="null"/>
        ///     to retrieve the values of a non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        public IList<string> GetValues(string section, string key) =>
            Document.ContainsKey(section ??= string.Empty) && Document[section].ContainsKey(key) ? Document[section][key] : Array.Empty<string>();

        /// <summary>
        ///     Gets the value under the specified index of the specified key in the
        ///     specified section of this <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to retrieve the
        ///     values of a non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        /// <param name="index">
        ///     The zero-based index of the value.
        /// </param>
        public string GetValue(string section, string key, int index = 0) =>
            Document.ContainsKey(section ??= string.Empty) && Document[section].ContainsKey(key) && Document[section][key].Count > index ? Document[section][key][index] : string.Empty;

        /// <summary>
        ///     Sets the specified value under the index for the specified key in the
        ///     specified section of this <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to retrieve the
        ///     values of a non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        ///     <para>
        ///         If <see langword="null"/>, the entire section is removed.
        ///     </para>
        /// </param>
        /// <param name="index">
        ///     The zero-based index of the value.
        ///     <para>
        ///         If <see langword="null"/>, the entire key is removed.
        ///     </para>
        ///     <para>
        ///         If out of range, a new value is added to the list of values.
        ///     </para>
        /// </param>
        /// <param name="value">
        ///     The value to be set.
        /// </param>
        /// <exception cref="ArgumentInvalidException">
        ///     section or key contains invalid characters.
        /// </exception>
        public void AddOrUpdate(string section, string key, int index, string value)
        {
            if ((section ??= string.Empty).Any(TextEx.IsLineSeparator))
                throw new ArgumentInvalidException(nameof(section));
            if (key?.Any(TextEx.IsLineSeparator) == true)
                throw new ArgumentInvalidException(nameof(key));
            if (string.IsNullOrEmpty(key))
            {
                Remove(section);
                return;
            }
            if (string.IsNullOrEmpty(value))
            {
                RemoveAt(section, key, index);
                return;
            }
            if (!Document.ContainsKey(section))
                Document.Add(section, new Dictionary<string, IList<string>>(Comparer));
            if (!Document[section].ContainsKey(key))
                Document[section].Add(key, new List<string>());
            if (!Document[section][key].Any() || Document[section][key].Count < index)
            {
                Document[section][key].Add(value);
                return;
            }
            Document[section][key][index] = value;
        }

        /// <summary>
        ///     Sets the specified value under the first index for the specified key in the
        ///     specified section of this <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to retrieve the
        ///     values of a non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        /// <param name="value">
        ///     The value to be set.
        /// </param>
        public void AddOrUpdate(string section, string key, string value) =>
            AddOrUpdate(section, key, 0, value);

        /// <summary>
        ///     Removes the value at the specified index under the specified key in the
        ///     specified section of this <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to handle
        ///     non-section keys.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        /// <param name="index">
        ///     The zero-based index of the value to remove.
        /// </param>
        public void RemoveAt(string section, string key, int index)
        {
            if (Document == default || !Document.ContainsKey(section ??= string.Empty) || !Document[section].ContainsKey(key) || Document[section][key].Count <= index)
                return;
            Document[section][key].RemoveAt(index);
        }

        /// <summary>
        ///     Removes all values from the specified key in the specified section of this
        ///     <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to handle
        ///     non-section keys.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        public void Remove(string section, string key)
        {
            if (!Document.ContainsKey(section ??= string.Empty) || !Document[section].ContainsKey(key))
                return;
            Document[section].Remove(key);
        }

        /// <summary>
        ///     Removes all keys and their associated values from the specified section of
        ///     this <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to handle
        ///     non-section keys.
        /// </param>
        public void Remove(string section)
        {
            if (!Document.ContainsKey(section ??= string.Empty))
                return;
            Document.Remove(section);
        }

        /// <summary>
        ///     Removes all sections from this <see cref="Ini"/> instance.
        /// </summary>
        public void Clear() =>
            Document?.Clear();

        /// <summary>
        ///     Gets the regular expression used to parse the INI document in an accessible
        ///     format.
        /// </summary>
        /// <param name="allowEmptySection">
        ///     <see langword="true"/> to allow key value pairs without section; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static Regex GetParseRegex(bool allowEmptySection = true, bool ignoreCases = true)
        {
            var options = (ignoreCases ? RegexOptions.IgnoreCase : RegexOptions.None) | RegexOptions.Multiline;
            return new Regex(allowEmptySection ? ParsePatternDefault : ParsePatternStrict, options);
        }

        /// <summary>
        ///     Converts the content of an INI file or an INI file formatted string value
        ///     to <see cref="IDictionary{TKey, TValue}"/> object.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        /// <param name="ignoreCase">
        ///     <see langword="true"/> to ignore the case of sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="sorted">
        ///     <see langword="true"/> to sort the sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static IDictionary<string, IDictionary<string, IList<string>>> Parse(string fileOrContent, bool ignoreCase = true, bool sorted = true)
        {
            var source = fileOrContent;
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(fileOrContent));
            var path = PathEx.Combine(source);
            if (File.Exists(path))
                source = File.ReadAllText(path, TextEx.DefaultEncoding);
            source = ForceFormat(source);

            var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            var content = new Dictionary<string, IDictionary<string, IList<string>>>(comparer);
            foreach (var match in GetParseRegex(true, ignoreCase).Matches(source).Cast<Match>())
            {
                var section = match.Groups["Section"]?.Value.Trim();
                if (string.IsNullOrEmpty(section))
                    continue;
                if (section == NonSectionKey)
                    section = string.Empty;
                if (!content.ContainsKey(section))
                    content.Add(section, new Dictionary<string, IList<string>>(comparer));
                var keys = new Dictionary<string, IList<string>>(comparer);
                for (var i = 0; i < match.Groups["Key"].Captures.Count; i++)
                {
                    var key = match.Groups["Key"]?.Captures[i].Value.Trim();
                    if (string.IsNullOrEmpty(key))
                        continue;
                    var value = match.Groups["Value"]?.Captures[i].Value.Trim();
                    if (string.IsNullOrEmpty(value))
                        continue;
                    if (!keys.ContainsKey(key))
                        keys.Add(key, new List<string>());
                    if (value.Length > EncodeTagOpen.Length + EncodeTagClose.Length && value.StartsWithEx(EncodeTagOpen) && value.EndsWithEx(EncodeTagClose))
                        value = value.Substring(EncodeTagOpen.Length, value.Length - EncodeTagClose.Length).DecodeString();
                    keys[key].Add(value);
                }
                if (keys.Values.All(x => x?.Any() != true))
                    continue;
                content[section] = keys;
            }

            return sorted ? SortHelper(content) : content;
        }

        /// <summary>
        ///     Converts the content of an INI file or an INI file formatted string value
        ///     to <see cref="IDictionary{TKey, TValue}"/> object.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        /// <param name="ignoreCase">
        ///     <see langword="true"/> to ignore the case of sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="sorted">
        ///     <see langword="true"/> to sort the sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static bool TryParse(string fileOrContent, bool ignoreCase, bool sorted, out IDictionary<string, IDictionary<string, IList<string>>> value)
        {
            try
            {
                value = Parse(fileOrContent, ignoreCase, sorted);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                value = default;
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Converts the content of an INI file or an INI file formatted string value
        ///     to <see cref="IDictionary{TKey, TValue}"/> object.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        /// <param name="ignoreCase">
        ///     <see langword="true"/> to ignore the case of sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static bool TryParse(string fileOrContent, bool ignoreCase, out IDictionary<string, IDictionary<string, IList<string>>> value) =>
            TryParse(fileOrContent, ignoreCase, true, out value);

        /// <summary>
        ///     Converts the content of an INI file or an INI file formatted string value
        ///     to <see cref="IDictionary{TKey, TValue}"/> object.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        public static bool TryParse(string fileOrContent, out IDictionary<string, IDictionary<string, IList<string>>> value) =>
            TryParse(fileOrContent, true, true, out value);

        /// <summary>
        ///     Retrieves the value from the specified section in an INI file.
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
        ///     The file path of an INI file.
        /// </param>
        public static string ReadDirect(string section, string key, string file)
        {
            var output = string.Empty;
            try
            {
                var path = PathEx.Combine(file);
                if (!File.Exists(path))
                    throw new PathNotFoundException(path);
                var sb = new StringBuilder(short.MaxValue);
                if (WinApi.NativeMethods.GetPrivateProfileString(section, key, string.Empty, sb, sb.Capacity, path) != 0)
                    output = sb.ToStringThenClear();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return output;
        }

        /// <summary>
        ///     Writes the specified content to an INI file on the disk.
        /// </summary>
        /// <param name="content">
        ///     The content based on <see cref="Parse(string, bool, bool)"/>.
        /// </param>
        /// <param name="file">
        ///     The file path of an INI file.
        /// </param>
        /// <param name="sorted">
        ///     <see langword="true"/> to sort the sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static bool WriteAll(IDictionary<string, IDictionary<string, IList<string>>> content, string file, bool sorted = true)
        {
            try
            {
                if (content == null)
                    throw new ArgumentNullException(nameof(content));
                if (!content.Any() || !content.Values.Any() || content.Values.All(x => x?.Any() != true))
                    throw new ArgumentInvalidException(nameof(content));

                var path = PathEx.Combine(file);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(file));
                if (!PathEx.IsValidPath(path))
                    throw new ArgumentInvalidException(nameof(file));

                var source = content;
                if (sorted)
                    source = SortHelper(source);

                var hash = File.Exists(path) ? path.EncryptFile(ChecksumAlgorithm.Crc32) : null;
                var temp = FileEx.GetUniqueTempPath("tmp", ".ini");
                using (var sw = new StreamWriter(temp, true))
                    foreach (var dict in source)
                    {
                        if (!dict.Value.Any() || dict.Key.Any(TextEx.IsLineSeparator))
                            continue;
                        if (!string.IsNullOrEmpty(dict.Key))
                        {
                            sw.Write('[');
                            sw.Write(dict.Key.Trim());
                            sw.Write(']');
                            sw.WriteLine();
                        }
                        foreach (var pair in dict.Value)
                        {
                            if (string.IsNullOrWhiteSpace(pair.Key) || pair.Key.Any(TextEx.IsLineSeparator) || !pair.Value.Any())
                                continue;
                            foreach (var value in pair.Value)
                            {
                                if (string.IsNullOrWhiteSpace(value))
                                    continue;
                                sw.Write(pair.Key.Trim());
                                sw.Write('=');
                                if (!value.Any(TextEx.IsLineSeparator))
                                    sw.Write(value.Trim());
                                else
                                {
                                    sw.Write(EncodeTagOpen);
                                    sw.Write(value.Encode());
                                    sw.Write(EncodeTagClose);
                                }
                                sw.WriteLine();
                            }
                        }
                        sw.WriteLine();
                    }

                if (hash == temp.EncryptFile(ChecksumAlgorithm.Crc32))
                {
                    File.Delete(temp);
                    return true;
                }
                if (File.Exists(path))
                    File.Delete(path);
                File.Move(temp, path);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Copies the <see cref="string"/> representation of the specified
        ///     <see cref="object"/> value into the specified section of an INI file. If
        ///     the file does not exist, it is created.
        ///     <para>
        ///         The Win32-API is used for writing in this case. Please note that empty
        ///         sections are not permitted and that this function writes all changes
        ///         directly on the disk. This causes many write accesses when used
        ///         incorrectly.
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section to which the value will be copied.
        /// </param>
        /// <param name="key">
        ///     The name of the key to be associated with a value.
        ///     <para>
        ///         If this parameter is NULL, the entire section, including all entries
        ///         within the section, is deleted.
        ///     </para>
        /// </param>
        /// <param name="value">
        ///     The value to be written to the file.
        ///     <para>
        ///         If this parameter is <see langword="null"/>, the key pointed to by the
        ///         key parameter is deleted.
        ///     </para>
        /// </param>
        /// <param name="file">
        ///     The path of the INI file to write.
        /// </param>
        /// <param name="forceOverwrite">
        ///     <see langword="true"/> to enable overwriting of a key with the same value
        ///     as specified; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="skipExistValue">
        ///     <see langword="true"/> to skip an existing value, even it is not the same
        ///     value as specified; otherwise, <see langword="false"/>.
        /// </param>
        public static bool WriteDirect(string section, string key, object value, string file = null, bool forceOverwrite = true, bool skipExistValue = false)
        {
            try
            {
                var path = PathEx.Combine(file);
                if (string.IsNullOrWhiteSpace(section))
                    throw new ArgumentNullException(nameof(section));
                if (!File.Exists(path))
                {
                    if (string.IsNullOrWhiteSpace(key) || value == null || !Path.HasExtension(path) || !PathEx.IsValidPath(path))
                        throw new PathNotFoundException(path);
                    var dir = Path.GetDirectoryName(path);
                    if (string.IsNullOrWhiteSpace(dir))
                        throw new ArgumentInvalidException(nameof(file));
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    File.Create(path).Close();
                }
                var strValue = value?.ToString();
                if (!forceOverwrite || skipExistValue)
                {
                    var curValue = ReadDirect(section, key, path);
                    if (!forceOverwrite && curValue.Equals(strValue, StringComparison.Ordinal) || skipExistValue && !string.IsNullOrWhiteSpace(curValue))
                        return false;
                }
                if (string.Concat(section, key, value).All(TextEx.IsAscii))
                    goto Write;
                var encoding = TextEx.GetEncoding(path);
                if (!encoding.Equals(Encoding.Unicode) && !encoding.Equals(Encoding.BigEndianUnicode))
                    TextEx.ChangeEncoding(path, Encoding.Unicode);
                Write:
                return WinApi.NativeMethods.WritePrivateProfileString(section, key, strValue, path) != 0;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        private static IDictionary<string, IDictionary<string, IList<string>>> SortHelper(IDictionary<string, IDictionary<string, IList<string>>> source) =>
            source.OrderBy(p => !string.IsNullOrEmpty(p.Key)).ThenBy(p => p.Key, SortComparer).ToDictionary(p => p.Key, p => SortHelper(p.Value));

        private static IDictionary<string, IList<string>> SortHelper(IDictionary<string, IList<string>> source) =>
            source.OrderBy(p => p.Key, SortComparer).ToDictionary(p => p.Key, p => p.Value);

        private static bool LineIsValid(string str)
        {
            var s = str;
            if (string.IsNullOrWhiteSpace(s) || s.Length < 3)
                return false;
            if (s.StartsWith("[", StringComparison.Ordinal) && s.EndsWith("]", StringComparison.Ordinal) && s.Count(x => x == '[') == 1 && s.Count(x => x == ']') == 1 && s.Any(char.IsLetterOrDigit))
                return true;
            var c = s.First();
            if (!char.IsLetterOrDigit(c) && !c.IsBetween('$', '/') && !c.IsBetween('<', '@') && !c.IsBetween('{', '~') && c != '!' && c != '"' && c != ':' && c != '^' && c != '_')
                return false;
            var i = s.IndexOf('=');
            return i > 0 && s.Substring(0, i).Any(char.IsLetterOrDigit) && i + 1 < s.Length;
        }

        private static string ForceFormat(string str)
        {
            var builder = new StringBuilder();
            foreach (var text in str.TrimStart().Split(StringNewLineFormats.All))
            {
                var line = text.Trim();
                if (line.StartsWith("[", StringComparison.Ordinal) && !line.EndsWith("]", StringComparison.Ordinal) && line.Contains(']') && line.IndexOf(']') > 1)
                    line = line.Substring(0, line.IndexOf(']') + 1);
                if (LineIsValid(line))
                    builder.AppendLine(line);
            }
            if (builder.Length < 1)
                throw new NullReferenceException();
            var first = builder.ToString(0, 1).First();
            if (first.Equals('['))
                return builder.ToString();
            builder.Insert(0, Environment.NewLine);
            builder.Insert(0, ']');
            builder.Insert(0, NonSectionKey);
            builder.Insert(0, '[');
            return builder.ToStringThenClear();
        }
    }
}
