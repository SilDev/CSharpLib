#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IniDocument.cs
// Version:  2020-02-03 20:22
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Ini
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Web.Script.Serialization;

    /// <summary>
    ///     The file format of the INI document.
    /// </summary>
    public enum IniFileFormat
    {
        /// <summary>
        ///     Standard INI file format.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     Windows Registry Editor Version 4 file format.
        /// </summary>
        Regedit4 = 1,

        /// <summary>
        ///     Windows Registry Editor Version 5 file format.
        /// </summary>
        Regedit5 = 2
    }

    /// <summary>
    ///     Represents an INI document.
    /// </summary>
    [Serializable]
    public sealed class IniDocument : IEquatable<IniDocument>, ISerializable
    {
        [NonSerialized]
        private readonly object _identifier = new object();

        private IDictionary<string, IDictionary<string, IList<string>>> _document;

        /// <summary>
        ///     Gets the number of elements in this instance.
        /// </summary>
        [ScriptIgnore]
        public int Count => _document.Count;

        /// <summary>
        ///     Gets the raw object that is used to manage the complete data of this
        ///     instance.
        ///     <para>
        ///         Please use the indexer of this instance to update the content of this
        ///         <see cref="Document"/>.
        ///     </para>
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<string>>> Document =>
            _document.ToDictionary(p => p.Key, p => p.Value.ToDictionary(x => x.Key, x => (x.Value as List<string>)?.AsReadOnly() as IReadOnlyList<string>) as IReadOnlyDictionary<string, IReadOnlyList<string>>);

        /// <summary>
        ///     Gets the value that determines how sections and keys are compared
        ///     internally in this instance to allow duplicates if case-sensitive, or to
        ///     avoid if case-insensitive.
        /// </summary>
        [ScriptIgnore]
        public StringComparer Comparer { get; private set; }

        /// <summary>
        ///     Gets an <see cref="ICollection{T}"/> containing the sections of this
        ///     instance.
        /// </summary>
        [ScriptIgnore]
        public ICollection<string> Sections => _document.Keys;

        /// <summary>
        ///     Gets or sets the file format of this instance.
        /// </summary>
        public IniFileFormat FileFormat { get; set; }

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
        public string this[string section, string key, int index = 0]
        {
            get => GetValue(section, key, index);
            set => AddOrUpdate(section, key, index, value);
        }

        /// <summary>
        ///     Gets or sets the value under the specified index of the specified key in
        ///     the specified section of this instance.
        ///     <para>
        ///         If possible, the return value is converted to the specified type;
        ///         otherwise, <see langword="null"/> is returned.
        ///     </para>
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
        /// <param name="returnType">
        ///     The type to which the return value should be converted.
        /// </param>
        public object this[string section, string key, int index, Type returnType]
        {
            get
            {
                var value = this[section, key, index];
                return value.TryParse(out var result, returnType) ? result : null;
            }
            set
            {
                switch (value)
                {
                    case null:
                        this[section, key, index] = null;
                        return;
                    case string str:
                        this[section, key, index] = str;
                        return;
                    case IEnumerable<char> chars:
                        this[section, key, index] = new string(chars.ToArray());
                        return;
                    case IDictionary dictionary:
                        this[section, key, index] = Json.Serialize(dictionary);
                        return;
                    case IEnumerable<object> sequence:
                        this[section, key, index] = Json.Serialize(sequence);
                        return;
                }
                this[section, key, index] = value.ToString();
            }
        }

        /// <summary>
        ///     Gets or sets the first value of the specified key in the specified section
        ///     of this instance.
        ///     <para>
        ///         If possible, the return value is converted to the specified type;
        ///         otherwise, <see langword="null"/> is returned.
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to handle values of
        ///     a non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        /// <param name="returnType">
        ///     The type to which the return value should be converted.
        /// </param>
        public object this[string section, string key, Type returnType]
        {
            get => this[section, key, 0, returnType];
            set => this[section, key, 0, returnType] = value;
        }

        /// <summary>
        ///     Gets or sets the value under the specified index of the specified key in
        ///     the specified section of this instance.
        ///     <para>
        ///         If possible, the return value is converted to the type of the specified
        ///         default object; otherwise, <see langword="null"/> is returned.
        ///     </para>
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
            get
            {
                var type = defValue?.GetType();
                var value = this[section, key, index, type];
                return value ?? defValue;
            }
            set => this[section, key, index, defValue?.GetType()] = value;
        }

        /// <summary>
        ///     Gets or sets the first value of the specified key in the specified section
        ///     of this instance.
        ///     <para>
        ///         If possible, the return value is converted to the type of the specified
        ///         default object; otherwise, <see langword="null"/> is returned.
        ///     </para>
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
            get => this[section, key, 0, defValue];
            set => this[section, key, 0, defValue] = value;
        }

        /// <summary>
        ///     Gets key/value dictionary of a specific section from this instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> for access
        ///     non-section.
        /// </param>
        public IDictionary<string, IList<string>> this[string section] =>
            _document.TryGetValue(section ?? string.Empty, out var keyValuePairs) ? keyValuePairs : null;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IniDocument"/> class with the
        ///     specified parameters.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        /// <param name="ignoreCase">
        ///     <see langword="true"/> to ignore the case of sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public IniDocument(string fileOrContent, bool ignoreCase)
        {
            Comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            if (!string.IsNullOrEmpty(fileOrContent))
            {
                FilePath = FileEx.Exists(fileOrContent) ? fileOrContent : FileEx.GetUniqueTempPath("tmp", ".ini");
                if (IniReader.TryParse(fileOrContent, ignoreCase, out var document, out var fileFormat))
                {
                    _document = document;
                    FileFormat = fileFormat;
                    return;
                }
            }
            _document = new Dictionary<string, IDictionary<string, IList<string>>>(Comparer);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IniDocument"/> class with the
        ///     specified parameter.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The path or content of an INI file.
        /// </param>
        public IniDocument(string fileOrContent) : this(fileOrContent, true) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IniDocument"/> class.
        /// </summary>
        public IniDocument() : this(null, true) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IniDocument"/> class with the
        ///     specified parameter.
        /// </summary>
        /// <param name="dictionary">
        ///     The <see cref="IDictionary{TKey,TValue}"/> representation of an INI file.
        /// </param>
        internal IniDocument(IDictionary<string, IDictionary<string, IList<string>>> dictionary, IniFileFormat fileFormat) : this(null, true)
        {
            if (dictionary != null)
                _document = dictionary;
            FileFormat = fileFormat;
        }

        private IniDocument(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (Log.DebugMode > 1)
                Log.Write($"{nameof(IniDocument)}.ctor({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");
            _document = (IDictionary<string, IDictionary<string, IList<string>>>)info.GetValue(nameof(Document), typeof(IDictionary<string, IDictionary<string, IList<string>>>));
            Comparer = (StringComparer)info.GetValue(nameof(Comparer), typeof(StringComparer));
            FileFormat = (IniFileFormat)info.GetValue(nameof(FilePath), typeof(IniFileFormat));
            FilePath = info.GetString(nameof(FilePath));
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
        public void LoadFrom(string fileOrContent, bool ignoreCase = true)
        {
            Comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            if (!string.IsNullOrEmpty(fileOrContent))
            {
                FilePath = FileEx.Exists(fileOrContent) ? fileOrContent : null;
                if (IniReader.TryParse(fileOrContent, ignoreCase, out var document, out var fileFormat))
                {
                    _document?.Clear();
                    _document = document;
                    FileFormat = fileFormat;
                    return;
                }
            }
            _document?.Clear();
            _document = new Dictionary<string, IDictionary<string, IList<string>>>(Comparer);
        }

        /// <summary>
        ///     Loads the content of the file at <see cref="FilePath"/> into this instance.
        /// </summary>
        /// <param name="ignoreCase">
        ///     <see langword="true"/> to ignore the case of sections and keys; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public void Load(bool ignoreCase = true) =>
            LoadFrom(FilePath, ignoreCase);

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
        public void SaveTo(string path, bool setNewFilePath = false)
        {
            if (setNewFilePath)
                FilePath = path;
            this.WriteTo(path);
        }

        /// <summary>
        ///     Saves the string representation of this instance to <see cref="FilePath"/>.
        /// </summary>
        public void Save() =>
            this.WriteTo(FilePath);

        /// <summary>
        ///     Determines whether this instance contains a specific section.
        /// </summary>
        /// <param name="section">
        ///     The section to locate in this instance.
        /// </param>
        public bool ContainsSection(string section) =>
            !IniHelper.SectionIsInvalid(section ??= string.Empty) && _document.ContainsKey(section);

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
                Log.Write($"{nameof(IniDocument)}.get({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");
            info.AddValue(nameof(Document), _document);
            info.AddValue(nameof(Comparer), Comparer);
            info.AddValue(nameof(FileFormat), FileFormat);
            info.AddValue(nameof(FilePath), FilePath);
        }

        /// <summary>
        ///     Gets the value at the specified index of a specific key under a specific
        ///     section of this instance.
        /// </summary>
        /// <param name="section">
        ///     The name of the section. Must be <see langword="null"/> to retrieve the
        ///     values of a non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key in the section.
        /// </param>
        /// <param name="index">
        ///     The zero-based index whose the value is associated.
        /// </param>
        public string GetValue(string section, string key, int index) =>
            _document.ContainsKey(section ??= string.Empty) && _document[section].ContainsKey(key) && _document[section][key].Count > index ? _document[section][key][index] : null;

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
            if (!_document[section][key].Any() || _document[section][key].Count <= index)
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
        ///     Copies everything of the specified <see cref="IniDocument"/> instance to
        ///     this instance.
        ///     <para>
        ///         Please note that existing values will be overwritten.
        ///     </para>
        /// </summary>
        /// <param name="ini">
        ///     The <see cref="IniDocument"/> instance that is merged.
        /// </param>
        public void MergeWith(IniDocument ini)
        {
            if (ini == null)
                return;
            foreach (var section in ini.Sections)
            {
                if (ini[section]?.Any() != true)
                    continue;
                foreach (var key in ini[section].Keys)
                {
                    if (ini[section][key]?.Any() != true)
                        continue;
                    for (var i = 0; i < ini[section][key].Count; i++)
                        AddOrUpdate(section, key, i, ini[section][key][i]);
                }
            }
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
        ///     Removes all sections from this instance.
        /// </summary>
        public void Clear() =>
            _document?.Clear();

        /// <summary>
        ///     Sorts the sections and keys in this instance.
        /// </summary>
        /// <param name="topSections">
        ///     A sequence of section names that should always be on top.
        /// </param>
        public void Sort(params string[] topSections) =>
            _document = IniHelper.SortHelper(_document, topSections);

        /// <summary>
        ///     Sorts the sections and keys in this instance.
        /// </summary>
        public void Sort() =>
            _document = IniHelper.SortHelper(_document, null);

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode() =>
            _identifier.GetHashCode();

        /// <summary>
        ///     Determines whether this instance have same values as the specified
        ///     <see cref="IniDocument"/> instance.
        /// </summary>
        /// <param name="other">
        ///     The <see cref="IniDocument"/> instance to compare.
        /// </param>
        public bool Equals(IniDocument other)
        {
            if (other == null || Count != other.Count || Comparer != other.Comparer || FilePath != other.FilePath)
                return false;
            return _document.EncryptRaw() == other._document.EncryptRaw();
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
            if (!(other is IniDocument ini))
                return false;
            return Equals(ini);
        }

        /// <summary>
        ///     Returns a string that represents the current <see cref="IniDocument"/>
        ///     object.
        /// </summary>
        public override string ToString()
        {
            using var ms = new MemoryStream();
            this.WriteTo(ms);
            ms.Position = 0;
            return ms.ToArray().ToStringUtf8();
        }

        /// <summary>
        ///     Determines whether two specified <see cref="IniDocument"/> instances have
        ///     same values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="IniDocument"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="IniDocument"/> instance to compare.
        /// </param>
        public static bool operator ==(IniDocument left, IniDocument right) =>
            left?.Equals(right) ?? right is null;

        /// <summary>
        ///     Determines whether two specified <see cref="IniDocument"/> instances have
        ///     different values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="IniDocument"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="IniDocument"/> instance to compare.
        /// </param>
        public static bool operator !=(IniDocument left, IniDocument right) =>
            !(left == right);
    }
}
