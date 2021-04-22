#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ValueItemHost.cs
// Version:  2021-04-22 19:46
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
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
    ///         It is strongly recommended to avoid using multiple keys with the same
    ///         values.
    ///     </para>
    /// </typeparam>
    [Serializable]
    public class ValueItemHost<TKey> : ISerializable where TKey : struct, Enum
    {
        [NonSerialized]
        private IDictionary<string, object> _itemDictionary;

        /// <summary>
        ///     Gets the collection of keys and values.
        /// </summary>
        public IReadOnlyDictionary<string, object> ItemDictionary
        {
            get
            {
                _itemDictionary ??= new Dictionary<string, object>(StringComparer.InvariantCulture);
                return (IReadOnlyDictionary<string, object>)_itemDictionary;
            }
            private set => _itemDictionary = value as Dictionary<string, object>;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueItemHost{TKey}"/>.
        /// </summary>
        public ValueItemHost() =>
            ItemDictionary = new Dictionary<string, object>(StringComparer.InvariantCulture);

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueItemHost{TKey}"/> class
        ///     with serialized data.
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

            ItemDictionary = (Dictionary<string, object>)info.GetValue(nameof(ItemDictionary), typeof(Dictionary<string, object>));
        }

        /// <summary>
        ///     Deserializes the specified file into this instance graph.
        /// </summary>
        /// <param name="path">
        ///     The file to deserialize.
        /// </param>
        /// <param name="defValue">
        ///     The default value.
        /// </param>
        /// <param name="merge">
        ///     <see langword="true"/> to merge the current elements with the new ones;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public void Load(string path, ValueItemHost<TKey> defValue = default, bool merge = true)
        {
            if (!merge && ItemDictionary.Any())
                _itemDictionary.Clear();
            var host = FileEx.Deserialize(path, defValue);
            if (host == default)
                return;
            var hostItemDict = host.ItemDictionary;
            if (!hostItemDict.Any())
                return;
            var keys = GetKeyNames(true);
            foreach (var key in keys)
            {
                if (!hostItemDict.ContainsKey(key))
                    continue;
                var value = hostItemDict[key];
                if (!merge || !ItemDictionary.ContainsKey(key))
                {
                    _itemDictionary.Add(key, value);
                    continue;
                }
                var item = (dynamic)_itemDictionary[key];
                _itemDictionary[key] = value;
                if (item.ValueGetValidationFunc != null)
                    ((dynamic)_itemDictionary[key]).ValueGetValidationFunc = item.ValueGetValidationFunc;
                if (item.ValueSetValidationFunc != null)
                    ((dynamic)_itemDictionary[key]).ValueSetValidationFunc = item.ValueSetValidationFunc;
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
        ///     <see langword="true"/> to allow an existing file to be overwritten;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public void Save(string path, bool overwrite = true) =>
            FileEx.Serialize(path, this, true, overwrite);

        /// <summary>
        ///     Adds the specified item to this instance that is addressed to the specified
        ///     key.
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
            if (string.IsNullOrEmpty(name) || ItemDictionary.ContainsKey(name))
                return;
            _itemDictionary.Add(name, item);
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
        /// <param name="getValidationFunc">
        ///     The method that is called when <see cref="ValueItem{TValue}.Value"/> is
        ///     get.
        ///     <para>
        ///         Please note that <see cref="Func{T, TResult}"/> methods cannot be
        ///         serialized.
        ///     </para>
        /// </param>
        /// <param name="setValidationFunc">
        ///     The method that is called when <see cref="ValueItem{TValue}.Value"/> is
        ///     set.
        ///     <para>
        ///         Please note that <see cref="Func{T, TResult}"/> methods cannot be
        ///         serialized.
        ///     </para>
        /// </param>
        public void AddItem<TValue>(TKey key, TValue value, TValue defValue, TValue minValue, TValue maxValue, Func<TValue, TValue> getValidationFunc = default, Func<TValue, TValue> setValidationFunc = default) where TValue : IEquatable<TValue>
        {
            var name = GetKeyName(key);
            if (string.IsNullOrEmpty(name) || ItemDictionary.ContainsKey(name))
                return;
            _itemDictionary.Add(name, new ValueItem<TValue>(value, defValue, minValue, maxValue, getValidationFunc, setValidationFunc));
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
        /// <param name="getValidationFunc">
        ///     The method that is called when <see cref="ValueItem{TValue}.Value"/> is
        ///     get.
        ///     <para>
        ///         Please note that <see cref="Func{T, TResult}"/> methods cannot be
        ///         serialized.
        ///     </para>
        /// </param>
        /// <param name="setValidationFunc">
        ///     The method that is called when <see cref="ValueItem{TValue}.Value"/> is
        ///     set.
        ///     <para>
        ///         Please note that <see cref="Func{T, TResult}"/> methods cannot be
        ///         serialized.
        ///     </para>
        /// </param>
        public void AddItem<TValue>(TKey key, TValue value, TValue defValue = default, Func<TValue, TValue> getValidationFunc = default, Func<TValue, TValue> setValidationFunc = default) where TValue : IEquatable<TValue> =>
            AddItem(key, value, defValue, default, default, getValidationFunc, setValidationFunc);

        /// <summary>
        ///     Removes all keys and values from this instance.
        /// </summary>
        /// <param name="removeOnlyInvalidElements">
        ///     <see langword="true"/> to remove only invalid elements; otherwise,
        ///     <see langword="false"/>.
        ///     <para>
        ///         Please note that elements can become invalid if the associated key has
        ///         been deleted.
        ///     </para>
        /// </param>
        public void Clear(bool removeOnlyInvalidElements = false)
        {
            if (!removeOnlyInvalidElements)
            {
                if (ItemDictionary.Any())
                    _itemDictionary.Clear();
                return;
            }
            if (!ItemDictionary.Any())
                return;
            var keys = GetKeyNames(true);
            if (ItemDictionary.Keys.All(x => keys.Any(y => y == x)))
                return;
            var itemDict = ItemDictionary;
            _itemDictionary.Clear();
            foreach (var key in keys)
            {
                if (!itemDict.ContainsKey(key))
                    continue;
                var value = itemDict[key];
                _itemDictionary.Add(key, value);
            }
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
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (Log.DebugMode > 1)
                Log.Write($"{nameof(ValueItemHost<TKey>)}.get({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");

            info.AddValue(nameof(ItemDictionary), ItemDictionary);
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
        public IEnumerable<string> GetKeyNames(bool sorted = false)
        {
            var names = Enum.GetNames(typeof(TKey)).AsEnumerable();
            if (sorted)
                names = names.OrderBy(x => x, CacheInvestor.GetDefault<AlphaNumericComparer<string>>());
            return names;
        }

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
            if (string.IsNullOrEmpty(name) || !ItemDictionary.TryGetValue(name, out var obj) || !(obj is ValueItem<TValue> item))
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
            if (string.IsNullOrEmpty(name) || !ItemDictionary.ContainsKey(name))
                return;
            _itemDictionary.Remove(name);
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
            if (ItemDictionary.ContainsKey(name))
            {
                _itemDictionary[name] = item;
                return;
            }
            _itemDictionary.Add(name, item);
        }

        /// <summary>
        ///     Sorts the elements in the entire <see cref="ItemDictionary"/> using the
        ///     <see cref="AlphaNumericComparer"/> comparer.
        /// </summary>
        public void Sort()
        {
            var itemDict = ItemDictionary;
            if (itemDict.Count > 1)
                ItemDictionary = itemDict.OrderBy(x => x.Key, CacheInvestor.GetDefault<AlphaNumericComparer<string>>()).ToDictionary(x => x.Key, x => x.Value);
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
            return this.EncryptRaw() == other.EncryptRaw();
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
        public override int GetHashCode() =>
            ItemDictionary.GetHashCode();

        /// <summary>
        ///     Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{");
            var first = false;
            foreach (var pair in ItemDictionary)
            {
                var key = pair.Key;
                if (!Enum.TryParse(key, out TKey _))
                    continue;
                var item = (dynamic)pair.Value;
                if (first)
                    builder.Append(',');
                else
                    first = true;
                builder.AppendFormatCurrent("Key={0},Item={1}", key, (object)item);
            }
            builder.Append("}");
            return builder.ToStringThenClear();
        }

        /// <summary>
        ///     Determines whether two specified <see cref="ValueItemHost{TKey}"/>
        ///     instances have same values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="ValueItemHost{TKey}"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="ValueItemHost{TKey}"/> instance to compare.
        /// </param>
        public static bool operator ==(ValueItemHost<TKey> left, ValueItemHost<TKey> right) =>
            left?.Equals(right) ?? right is null;

        /// <summary>
        ///     Determines whether two specified <see cref="ValueItemHost{TKey}"/>
        ///     instances have different values.
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
