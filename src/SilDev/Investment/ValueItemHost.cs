#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ValueItemHost.cs
// Version:  2019-12-12 21:43
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Investment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Text;

    /// <summary>
    ///     Defines a collection type for <see cref="ValueItem{TValue}"/> instances.
    /// </summary>
    /// <typeparam name="TKey">
    ///     The enumeration type of the key.
    ///     <para>
    ///         Please note that only the name of each key is stored. This means that each key
    ///         should have a different value to prevent an item from being misallocated.
    ///         Therefore, it is recommended that you do not add any value to the keys, because
    ///         the compiler does this automatically.
    ///     </para>
    /// </typeparam>
    [Serializable]
    public class ValueItemHost<TKey> : ISerializable where TKey : struct, Enum
    {
        private readonly Dictionary<string, object> _items;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueItemHost{TKey}"/>.
        /// </summary>
        public ValueItemHost() =>
            _items = new Dictionary<string, object>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueItemHost{TKey}"/> class with
        ///     serialized data.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        protected ValueItemHost(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (Log.DebugMode > 1)
                Log.Write($"{nameof(ValueItemHost<TKey>)}.ctor({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");
            _items = (Dictionary<string, object>)info.GetValue(nameof(_items), typeof(Dictionary<string, object>));
        }

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
                Log.Write($"{nameof(ValueItemHost<TKey>)}.get({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");
            info.AddValue(nameof(_items), _items);
        }

        /// <summary>
        ///     Deserializes the specified file into this object graph.
        /// </summary>
        /// <param name="path">
        ///     The file to deserialize.
        /// </param>
        /// <param name="defValue">
        ///     The default value.
        /// </param>
        public void Load(string path, ValueItemHost<TKey> defValue = default)
        {
            _items.Clear();
            var host = FileEx.Deserialize(path, true, defValue);
            if (host == null)
                return;
            var items = host.GetItems();
            if (!items.Any())
                return;
            var keys = Enum.GetNames(typeof(TKey));
            foreach (var pair in items)
            {
                var key = pair.Key;
                if (keys.All(x => x != key))
                    continue;
                _items.Add(key, pair.Value);
            }
        }

        /// <summary>
        ///     Creates a new file, writes this instance graph into to the file, and then
        ///     closes the file.
        /// </summary>
        /// <param name="path">
        ///     The file to create.
        /// </param>
        /// <param name="overwrite">
        ///     true to allow an existing file to be overwritten; otherwise, false.
        /// </param>
        public void Save(string path, bool overwrite = true) =>
            FileEx.Serialize(path, this, true, overwrite);

        /// <summary>
        ///     Adds the specified item to this instance that is addressed to the
        ///     specified key.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The type of values stored in the item.
        /// </typeparam>
        /// <param name="key">
        ///     The key of the element to add.
        /// </param>
        /// <param name="item">
        ///     The element to add.
        /// </param>
        public void AddItem<TValue>(TKey key, ValueItem<TValue> item) where TValue : IEquatable<TValue>
        {
            var name = GetKeyName(key);
            if (string.IsNullOrEmpty(name) || _items.ContainsKey(name))
                return;
            _items.Add(name, item);
        }

        /// <summary>
        ///     Adds to this instance a newly created item from the specified values that
        ///     is addressed to the specified key.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The type of values stored in the item.
        /// </typeparam>
        /// <param name="key">
        ///     The key of the element to add.
        /// </param>
        /// <param name="value">
        ///     The value to be set.
        /// </param>
        /// <param name="defValue">
        ///     The value used as default.
        /// </param>
        public void AddItem<TValue>(TKey key, TValue value, TValue defValue = default) where TValue : IEquatable<TValue>
        {
            var name = GetKeyName(key);
            if (string.IsNullOrEmpty(name) || _items.ContainsKey(name))
                return;
            _items.Add(name, new ValueItem<TValue>(value, defValue));
        }

        /// <summary>
        ///     Adds to this instance a newly created item from the specified values that
        ///     is addressed to the specified key.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The type of values stored in the item.
        /// </typeparam>
        /// <param name="key">
        ///     The key of the element to add.
        /// </param>
        /// <param name="value">
        ///     The value to be set.
        /// </param>
        /// <param name="defValue">
        ///     The value used as default.
        /// </param>
        /// <param name="minValue">
        ///     The minimum value. Must be smaller than the maximum value.
        /// </param>
        /// <param name="maxValue">
        ///     The maximum value. Must be larger than the minimum value.
        /// </param>
        public void AddItem<TValue>(TKey key, TValue value, TValue defValue, TValue minValue, TValue maxValue) where TValue : IEquatable<TValue>
        {
            var name = GetKeyName(key);
            if (string.IsNullOrEmpty(name) || _items.ContainsKey(name))
                return;
            _items.Add(name, new ValueItem<TValue>(value, defValue, minValue, maxValue));
        }

        /// <summary>
        ///     Removes all keys and values from this instance.
        /// </summary>
        /// <param name="removeOnlyInvalidElements">
        ///     true to remove only invalid elements; otherwise, false.
        ///     <para>
        ///         Please note that elements can become invalid if the associated key
        ///         has been deleted.
        ///     </para>
        /// </param>
        public void Clear(bool removeOnlyInvalidElements = false)
        {
            if (!removeOnlyInvalidElements)
            {
                _items.Clear();
                return;
            }
            var keys = Enum.GetNames(typeof(TKey));
            if (_items.Count > 0 || _items.Keys.All(x => keys.Any(y => y == x)))
                return;
            var items = _items;
            _items.Clear();
            foreach (var pair in items)
            {
                var key = pair.Key;
                if (keys.All(x => x != key))
                    continue;
                _items.Add(key, pair.Value);
            }
        }

        /// <summary>
        ///     Retrieves the name associated with the specified key.
        /// </summary>
        /// <param name="key">
        ///     The key associated with the name.
        /// </param>
        public string GetKeyName(TKey key) =>
            Enum.GetName(typeof(TKey), key);

        /// <summary>
        ///     Retrieves all valid key names.
        /// </summary>
        public IEnumerable<string> GetKeyNames() =>
            Enum.GetNames(typeof(TKey));

        /// <summary>
        ///     Retrieves all valid keys.
        /// </summary>
        public IEnumerable<TKey> GetKeys()
        {
            foreach (var name in GetKeyNames())
            {
                if (!Enum.TryParse(name, out TKey key))
                    continue;
                yield return key;
            }
        }

        /// <summary>
        ///     Retrieves the internal collection of keys and values.
        /// </summary>
        protected Dictionary<string, object> GetItems() =>
            _items;

        /// <summary>
        ///     Gets the element associated with the specified key.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The type of values stored in the element.
        /// </typeparam>
        /// <param name="key">
        ///     The key of the element to be retrieved.
        /// </param>
        public ValueItem<TValue> GetItem<TValue>(TKey key) where TValue : IEquatable<TValue>
        {
            var name = GetKeyName(key);
            if (string.IsNullOrEmpty(name) || !_items.TryGetValue(name, out var obj) || !(obj is ValueItem<TValue> item))
                return new ValueItem<TValue>(default);
            return item;
        }

        /// <summary>
        ///     Removes the element associated with the specified key from this instance.
        /// </summary>
        /// <param name="key">
        ///     The key of the element to be deleted.
        /// </param>
        public void RemoveItem(TKey key)
        {
            var name = GetKeyName(key);
            if (string.IsNullOrEmpty(name) || !_items.ContainsKey(name))
                return;
            _items.Remove(name);
        }

        /// <summary>
        ///     Gets the value of the element associated with the specified key.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The type of values stored in the element.
        /// </typeparam>
        /// <param name="key">
        ///     The key of the element to retrieve its value.
        /// </param>
        public TValue GetValue<TValue>(TKey key) where TValue : IEquatable<TValue> =>
            GetItem<TValue>(key).Value;

        /// <summary>
        ///     Sets the value of the element associated with the specified key.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The type of values stored in the element.
        /// </typeparam>
        /// <param name="key">
        ///     The key of the element whose value should be set.
        /// </param>
        /// <param name="value">
        ///     The value to be set.
        /// </param>
        public void SetValue<TValue>(TKey key, TValue value) where TValue : IEquatable<TValue>
        {
            var name = GetKeyName(key);
            var item = GetItem<TValue>(key);
            item.Value = value;
            if (_items.ContainsKey(name))
            {
                _items[name] = item;
                return;
            }
            _items.Add(name, item);
        }

        /// <summary>
        ///     Determines whether this instance has the same values as another.
        /// </summary>
        /// <param name="other">
        ///     The other <see cref="ValueItemHost{TKey}"/> instance to compare.
        /// </param>
        public bool Equals(ValueItemHost<TKey> other)
        {
            var obj = (object)other;
            if (obj == null)
                return false;
            return GetHashCode(true) == other.GetHashCode(true);
        }

        /// <summary>
        ///     Determines whether this instance has the same values as another.
        /// </summary>
        /// <param name="other">
        ///     The other <see cref="ValueItemHost{TKey}"/> instance object to compare.
        /// </param>
        public override bool Equals(object other) =>
            Equals(other as ValueItemHost<TKey>);

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <param name="nonReadOnly">
        ///     true to include the hashes of non-readonly properties; otherwise, false.
        /// </param>
        public int GetHashCode(bool nonReadOnly) =>
            Crypto.GetClassHashCode(this, nonReadOnly);

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode() =>
            Crypto.GetClassHashCode(this);

        /// <summary>
        ///     Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{");
            var first = false;
            foreach (var pair in _items)
            {
                var key = pair.Key;
                if (!Enum.TryParse(key, out TKey _))
                    continue;
                var item = (dynamic)pair.Value;
                if (first)
                    builder.Append(',');
                else
                    first = true;
                builder.AppendFormat(CultureConfig.GlobalCultureInfo, "Key={0},Item={1}", key, item);
            }
            builder.Append("}");
            return builder.ToString();
        }

        /// <summary>
        ///     Determines whether two specified <see cref="ValueItemHost{TKey}"/> instances
        ///     have same values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="ValueItemHost{TKey}"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="ValueItemHost{TKey}"/> instance to compare.
        /// </param>
        public static bool operator ==(ValueItemHost<TKey> left, ValueItemHost<TKey> right)
        {
            var obj = (object)left;
            if (obj != null)
                return left.Equals(right);
            obj = right;
            return obj == null;
        }

        /// <summary>
        ///     Determines whether two specified <see cref="ValueItemHost{TKey}"/> instances
        ///     have different values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="ValueItemHost{TKey}"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="ValueItemHost{TKey}"/> instance to compare.
        /// </param>
        public static bool operator !=(ValueItemHost<TKey> left, ValueItemHost<TKey> right) =>
            !(left == right);
    }
}
