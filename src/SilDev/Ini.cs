#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Ini.cs
// Version:  2020-01-24 21:29
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Script.Serialization;
    using IDocumentDictionary = System.Collections.Generic.IDictionary<string, System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<string>>>;
    using IKeyValueDictionary = System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<string>>;
    using IReadOnlyDocumentDictionary = System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IReadOnlyList<string>>>;
    using IReadOnlyKeyValueDictionary = System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IReadOnlyList<string>>;

    /// <summary>
    ///     Provides the functionality to handle the INI format.
    /// </summary>
    [Serializable]
    public sealed class Ini : ISerializable, IEquatable<Ini>
    {
        /// <summary>
        ///     The default pattern for parsing an INI document that allows blank sections.
        /// </summary>
        [ScriptIgnore]
        public const string ParsePatternDefault = @"^((?:\[)(?<Section>[^\]]*)(?:\])(?:[\r\n]{0,}|\Z))((?!\[)(?<Key>[^=]*?)(?:=)(?<Value>[^\r\n]*)(?:[\r\n]{0,4}))*";

        /// <summary>
        ///     The pattern for parsing an INI document that does not allow blank sections.
        /// </summary>
        [ScriptIgnore]
        public const string ParsePatternStrict = @"^((?:\[)(?<Section>[^\]]*)(?:\])(?:[\r\n]{0,}|\Z))((?!\[)(?<Key>[^=]*?)(?:=)(?<Value>[^\r\n]*)(?:[\r\n]{0,4}))+";

        [ScriptIgnore]
        private const string EncodeTagOpen = "<base64 reason=\"LineSeparator\">",
                             EncodeTagClose = "</base64>";

        private readonly object _identifier = new object();
        private IDocumentDictionary _document;

        /// <summary>
        ///     Gets the raw object that is used to manage the complete data of this
        ///     instance.
        ///     <para>
        ///         Please use the indexer of this instance to update the content of this
        ///         <see cref="Document"/>.
        ///     </para>
        /// </summary>
        public IReadOnlyDocumentDictionary Document =>
            _document.ToDictionary(p => p.Key, p => p.Value.ToDictionary(x => x.Key, x => (x.Value as List<string>)?.AsReadOnly() as IReadOnlyList<string>) as IReadOnlyKeyValueDictionary);

        /// <summary>
        ///     Gets the value that determines how sections and keys are compared
        ///     internally in this instance to allow duplicates if case-sensitive, or to
        ///     avoid if case-insensitive.
        /// </summary>
        [ScriptIgnore]
        public StringComparer Comparer { get; private set; }

        /// <summary>
        ///     Gets the value that determines whether this instance is saved with ordered
        ///     sections and keys.
        /// </summary>
        public bool Sorted { get; private set; }

        /// <summary>
        ///     Gets or sets the file path of this instance.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///     Gets or sets the value under the specified index of the specified key in
        ///     the specified section of this instance.
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
        ///     specified section of this instance.
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

        /// <summary>
        ///     Gets or sets the object value under the specified index of the specified
        ///     key in the specified section of this instance.
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
        /// <param name="defValue">
        ///     The default value that is returned if no value is found.
        ///     <para>
        ///         Should be <see langword="null"/> if a new object value is to be set.
        ///     </para>
        /// </param>
        public object this[string section, string key, int index, object defValue]
        {
            get => GetValue(section, key, index) ?? defValue;
            set => AddOrUpdate(section, key, index, value?.ToString());
        }

        /// <summary>
        ///     Gets or sets the object value under the first index of the specified key in
        ///     the specified section of this instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to handle values of
        ///     a non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        /// <param name="defValue">
        ///     The default value that is returned if no value is found.
        ///     <para>
        ///         Should be <see langword="null"/> if a new object value is to be set.
        ///     </para>
        /// </param>
        public object this[string section, string key, object defValue]
        {
            get => GetValue(section, key) ?? defValue;
            set => AddOrUpdate(section, key, value?.ToString());
        }

        private static string NonSectionKey { get; } = Guid.NewGuid().Encrypt();

        private static AlphaNumericComparer SortComparer { get; } = new AlphaNumericComparer();

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

        private Ini(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (Log.DebugMode > 1)
                Log.Write($"{nameof(Ini)}.ctor({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");

            _document = (IDocumentDictionary)info.GetValue(nameof(Document), typeof(IDocumentDictionary));
            Comparer = (StringComparer)info.GetValue(nameof(Comparer), typeof(StringComparer));
            Sorted = info.GetBoolean(nameof(Sorted));
            FilePath = info.GetString(nameof(FilePath));
        }

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
        ///     to a <see cref="IDictionary{TKey, TValue}"/> object.
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
        public static IDocumentDictionary Parse(string fileOrContent, bool ignoreCase = true, bool sorted = true)
        {
            var source = fileOrContent;
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException(nameof(fileOrContent));
            var path = PathEx.Combine(source);
            if (File.Exists(path))
                source = File.ReadAllText(path, TextEx.DefaultEncoding);
            source = ForceFormat(source);

            var comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            var content = new Dictionary<string, IKeyValueDictionary>(comparer);
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
        ///     to a <see cref="IDictionary{TKey, TValue}"/> object.
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
        public static bool TryParse(string fileOrContent, bool ignoreCase, bool sorted, out IDocumentDictionary value)
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
        ///     to a <see cref="IDictionary{TKey, TValue}"/> object.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        /// <param name="ignoreCase">
        ///     <see langword="true"/> to ignore the case of sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static bool TryParse(string fileOrContent, bool ignoreCase, out IDocumentDictionary value) =>
            TryParse(fileOrContent, ignoreCase, true, out value);

        /// <summary>
        ///     Converts the content of an INI file or an INI file formatted string value
        ///     to a <see cref="IDictionary{TKey, TValue}"/> object.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        public static bool TryParse(string fileOrContent, out IDocumentDictionary value) =>
            TryParse(fileOrContent, true, true, out value);

        /// <summary>
        ///     Writes the specified dictionary representation of an INI file to the
        ///     specified stream.
        /// </summary>
        /// <param name="source">
        ///     The dictionary representation of an INI file.
        /// </param>
        /// <param name="destination">
        ///     The stream to write.
        /// </param>
        /// <param name="sorted">
        ///     <see langword="true"/> to sort the sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source or destination is null.
        /// </exception>
        public static void WriteToStream(IDocumentDictionary source, Stream destination, bool sorted = true)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            var document = source;
            if (sorted)
                document = SortHelper(document);
            using var sw = new StreamWriter(destination, new UTF8Encoding(false, true), 4096, true);
            foreach (var dict in document)
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
        }

        /// <summary>
        ///     Writes the specified dictionary representation of an INI file to the
        ///     specified destination file.
        /// </summary>
        /// <param name="source">
        ///     The dictionary representation of an INI file.
        /// </param>
        /// <param name="destination">
        ///     The file to write.
        /// </param>
        /// <param name="sorted">
        ///     <see langword="true"/> to sort the sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static bool WriteToFile(IDocumentDictionary source, string destination, bool sorted = true)
        {
            try
            {
                if (source == null)
                    throw new ArgumentNullException(nameof(source));
                if (!source.Any() || !source.Values.Any() || source.Values.All(x => x?.Any() != true))
                    throw new ArgumentInvalidException(nameof(source));
                var path = PathEx.Combine(destination);
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(destination));
                if (!PathEx.IsValidPath(path))
                    throw new ArgumentInvalidException(nameof(destination));
                var hash = File.Exists(path) ? path.EncryptFile(ChecksumAlgorithm.Crc32) : null;
                var temp = FileEx.GetUniqueTempPath("tmp", ".ini");
                using (var fs = new FileStream(temp, FileMode.Create))
                    WriteToStream(source, fs, sorted);
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
        ///     Writes the specified read-only dictionary representation of an INI file to
        ///     the specified file.
        /// </summary>
        /// <param name="source">
        ///     The read-only dictionary representation of an INI file.
        /// </param>
        /// <param name="destination">
        ///     The file path of an INI file.
        /// </param>
        /// <param name="sorted">
        ///     <see langword="true"/> to sort the sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static bool WriteToFile(IReadOnlyDocumentDictionary source, string destination, bool sorted = true)
        {
            var document = source?.ToDictionary(d => d.Key, d => d.Value?.ToDictionary(p => p.Key, p => p.Value?.ToList() as IList<string>) as IKeyValueDictionary);
            return WriteToFile(document, destination, sorted);
        }

        /// <summary>
        ///     Retrieves the value from the specified section in an INI file.
        ///     <para>
        ///         The Win32-API without file caching is used for reading in this case.
        ///         Please note that empty sections are not permitted.
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

        /// <summary>
        ///     Loads the full content of an INI file or an INI file formatted string value
        ///     into this instance.
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
                FilePath = FileEx.Exists(fileOrContent) ? fileOrContent : FileEx.GetUniqueTempPath("tmp", ".ini");
                Comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
                Sorted = sorted;
                if (TryParse(fileOrContent, ignoreCase, sorted, out var document))
                {
                    _document = document;
                    return;
                }
            }
            _document = new Dictionary<string, IKeyValueDictionary>(Comparer);
        }

        /// <summary>
        ///     Loads the content of the file at <see cref="FilePath"/> into this instance.
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
        ///     Saves the string representation of this instance to the specified path.
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
            WriteToFile(_document, path, Sorted);
        }

        /// <summary>
        ///     Saves the string representation of this instance to <see cref="FilePath"/>.
        /// </summary>
        public void Save() =>
            WriteToFile(_document, FilePath, Sorted);

        /// <summary>
        ///     Sets the <see cref="SerializationInfo"/> object for this instance.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        [SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (Log.DebugMode > 1)
                Log.Write($"{nameof(Ini)}.get({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");

            info.AddValue(nameof(Document), _document);
            info.AddValue(nameof(Comparer), Comparer);
            info.AddValue(nameof(Sorted), Sorted);
            info.AddValue(nameof(FilePath), FilePath);
        }

        /// <summary>
        ///     Gets a collection of all sections of this instance.
        /// </summary>
        public ICollection<string> GetSections()
        {
            var sections = _document.Keys;
            if (Sorted && sections.Count > 1)
                return sections.OrderBy(s => !string.IsNullOrEmpty(s)).ThenBy(s => s, SortComparer).ToList();
            return sections;
        }

        /// <summary>
        ///     Gets a collection of all keys under the specified section of this instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section to get the keys. Must be <see langword="null"/> to
        ///     retrieve the non-section keys.
        /// </param>
        public ICollection<string> GetKeys(string section)
        {
            var keys = _document.ContainsKey(section ??= string.Empty) ? _document[section].Keys : new Collection<string>();
            if (Sorted && keys.Count > 1)
                return keys.OrderBy(s => s, SortComparer).ToList();
            return keys;
        }

        /// <summary>
        ///     Gets a list of all values under the specified key of the specified section
        ///     of this instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key. Must be <see langword="null"/>
        ///     to retrieve the values of a non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        public IList<string> GetValues(string section, string key) =>
            _document.ContainsKey(section ??= string.Empty) && _document[section].ContainsKey(key) ? _document[section][key] : new Collection<string>();

        /// <summary>
        ///     Gets the value under the specified index of the specified key in the
        ///     specified section of this instance.
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
            _document.ContainsKey(section ??= string.Empty) && _document[section].ContainsKey(key) && _document[section][key].Count > index ? _document[section][key][index] : null;

        /// <summary>
        ///     Copies everything of the specified <see cref="Ini"/> instance to this
        ///     instance.
        ///     <para>
        ///         Please note that existing values will be overwritten.
        ///     </para>
        /// </summary>
        /// <param name="ini">
        ///     The <see cref="Ini"/> instance that is merged.
        /// </param>
        public void AddOrUpdate(Ini ini)
        {
            if (ini == null)
                return;
            foreach (var section in ini.GetSections())
            {
                foreach (var key in ini.GetKeys(section))
                {
                    var values = ini.GetValues(section, key);
                    if (values?.Any() != true)
                        continue;
                    for (var i = 0; i < values.Count; i++)
                        AddOrUpdate(section, key, i, values[0]);
                }
            }
        }

        /// <summary>
        ///     Sets the specified value under the index for the specified key in the
        ///     specified section of this instance.
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
        ///         If higher than available, a new value is added to the list of values.
        ///     </para>
        /// </param>
        /// <param name="value">
        ///     The value to be set.
        ///     <para>
        ///         If <see langword="null"/>, the entire key is removed.
        ///     </para>
        /// </param>
        /// <exception cref="ArgumentInvalidException">
        ///     section or key contains invalid characters.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     index is below zero.
        /// </exception>
        public void AddOrUpdate(string section, string key, int index, string value)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (key?.Any(TextEx.IsLineSeparator) == true)
                throw new ArgumentInvalidException(nameof(key));
            if ((section ??= string.Empty).Any(TextEx.IsLineSeparator))
                throw new ArgumentInvalidException(nameof(section));
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
            if (!_document.ContainsKey(section))
                _document.Add(section, new Dictionary<string, IList<string>>(Comparer));
            if (!_document[section].ContainsKey(key))
                _document[section].Add(key, new List<string>());
            if (!_document[section][key].Any() || _document[section][key].Count < index)
            {
                _document[section][key].Add(value);
                return;
            }
            _document[section][key][index] = value;
        }

        /// <summary>
        ///     Sets the specified value under the first index for the specified key in the
        ///     specified section of this instance.
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
        /// <param name="value">
        ///     The value to be set.
        ///     <para>
        ///         If <see langword="null"/>, the entire key is removed.
        ///     </para>
        /// </param>
        /// <exception cref="ArgumentInvalidException">
        ///     section or key contains invalid characters.
        /// </exception>
        public void AddOrUpdate(string section, string key, string value) =>
            AddOrUpdate(section, key, 0, value);

        /// <summary>
        ///     Removes the value at the specified index under the specified key in the
        ///     specified section of this instance.
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
            if (_document == default || index < 0 || !_document.ContainsKey(section ??= string.Empty) || !_document[section].ContainsKey(key) || _document[section][key].Count <= index)
                return;
            _document[section][key].RemoveAt(index);
        }

        /// <summary>
        ///     Removes all values from the specified key in the specified section of this
        ///     instance.
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
            if (!_document.ContainsKey(section ??= string.Empty) || !_document[section].ContainsKey(key))
                return;
            _document[section].Remove(key);
        }

        /// <summary>
        ///     Removes all keys and their associated values from the specified section of
        ///     this instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to handle
        ///     non-section keys.
        /// </param>
        public void Remove(string section)
        {
            if (!_document.ContainsKey(section ??= string.Empty))
                return;
            _document.Remove(section);
        }

        /// <summary>
        ///     Removes all sections from this instance.
        /// </summary>
        public void Clear() =>
            _document?.Clear();

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode() =>
            _identifier.GetHashCode();

        /// <summary>
        ///     Determines whether this instance have same values as the specified
        ///     <see cref="Ini"/> instance.
        /// </summary>
        /// <param name="other">
        ///     The <see cref="Ini"/> instance to compare.
        /// </param>
        public bool Equals(Ini other)
        {
            if (other == null || Comparer != other.Comparer || Sorted != other.Sorted || FilePath != other.FilePath || _document.Count != other._document.Count)
                return false;
            return this.EncryptRaw() == other.EncryptRaw();
        }

        /// <summary>
        ///     Determines whether this instance have same values as the specified
        ///     <see cref="object"/>.
        /// </summary>
        /// <param name="other">
        ///     The  <see cref="object"/> to compare.
        /// </param>
        public override bool Equals(object other)
        {
            if (!(other is Ini ini))
                return false;
            return Equals(ini);
        }

        /// <summary>
        ///     Returns a string that represents the current <see cref="Ini"/> object.
        /// </summary>
        public override string ToString()
        {
            using var ms = new MemoryStream();
            WriteToStream(_document, ms, Sorted);
            ms.Position = 0;
            return ms.ToArray().ToStringDefault();
        }

        private static IDocumentDictionary SortHelper(IDocumentDictionary source) =>
            source.OrderBy(p => !string.IsNullOrEmpty(p.Key)).ThenBy(p => p.Key, SortComparer).ToDictionary(p => p.Key, p => SortHelper(p.Value));

        private static IKeyValueDictionary SortHelper(IKeyValueDictionary source) =>
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

        /// <summary>
        ///     Determines whether two specified <see cref="Ini"/> instances have same
        ///     values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="Ini"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="Ini"/> instance to compare.
        /// </param>
        public static bool operator ==(Ini left, Ini right) =>
            left?.Equals(right) ?? right is null;

        /// <summary>
        ///     Determines whether two specified <see cref="Ini"/> instances have different
        ///     values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="Ini"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="Ini"/> instance to compare.
        /// </param>
        public static bool operator !=(Ini left, Ini right) =>
            !(left == right);
    }
}
