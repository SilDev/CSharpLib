#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: DictionaryEx.cs
// Version:  2023-12-20 12:39
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using Properties;

    public static class DictionaryEx
    {
        /// <summary>
        ///     Updates an element with the provided key and value of the specified
        ///     dictionary.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of the keys in the dictionary.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of the values in the dictionary.
        /// </typeparam>
        /// <param name="source">
        ///     The generic collection of key/value pairs.
        /// </param>
        /// <param name="key">
        ///     The key of the element to update.
        /// </param>
        /// <param name="value">
        ///     The new value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source or key is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     source is read-only.
        /// </exception>
        public static void Update<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value) where TValue : IComparable, IComparable<TValue>
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (source.IsReadOnly)
                throw new NotSupportedException(ExceptionMessages.ReadOnlyCollection);
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (source.ContainsKey(key))
            {
                if (value == null)
                {
                    source.Remove(key);
                    return;
                }
                source[key] = value;
                return;
            }
            source.Add(key, value);
        }

        /// <summary>
        ///     Attempts to add the specified key and value to this dictionary.
        ///     <para>
        ///         &#9888; Nothing happens if the <paramref name="key"/> already exists.
        ///     </para>
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of the key.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of the value.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="IDictionary{TKey, TValue}"/> to add the key/value pair.
        /// </param>
        /// <param name="key">
        ///     The key of the element to add.
        /// </param>
        /// <param name="value">
        ///     The value of the element to add. It can be <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the key/value pair was added successfully;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source == null || key == null || source.ContainsKey(key))
                return false;
            source.Add(key, value);
            return true;
        }

        /// <summary>
        ///     Attempts to add, set, or remove the specified key and value from this
        ///     dictionary.
        ///     <para>
        ///         &#9762; If the <paramref name="key"/> already exists, it will be
        ///         overwritten, and if the <paramref name="value"/> is also
        ///         <see langword="null"/> the <paramref name="key"/> is even completely
        ///         removed.
        ///     </para>
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of the key.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of the value.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="IDictionary{TKey, TValue}"/> to set the key/value pair.
        /// </param>
        /// <param name="key">
        ///     The key of the element to add, set or removed, depending on
        ///     <paramref name="value"/>.
        /// </param>
        /// <param name="value">
        ///     The value of the element to add. It can be <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the key/value pair was updated successfully;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TrySet<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source == null || key == null)
                return false;
            if (value == null)
                return source.ContainsKey(key) && source.Remove(key);
            if (source.ContainsKey(key))
            {
                source[key] = value;
                return true;
            }
            source.Add(key, value);
            return true;
        }

        /// <summary>
        ///     Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of the key.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of the value.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="Dictionary{TKey, TValue}"/> to get the value associated with
        ///     the specified key.
        /// </param>
        /// <param name="key">
        ///     The key of the value to get.
        /// </param>
        /// <param name="defValue">
        ///     The value returned if the key does not exist.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the object that implements the
        ///     <see cref="Dictionary{TKey, TValue}"/> interface contains an element that
        ///     has the specified key; otherwise, <see langword="false"/>.
        /// </returns>
        public static TValue TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue defValue = default)
        {
            if (source == null || key == null || !source.ContainsKey(key))
                return defValue;
            return source[key];
        }

        /// <summary>
        ///     Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of the key.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of the value.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="IDictionary{TKey, TValue}"/> to get the value associated
        ///     with the specified key.
        /// </param>
        /// <param name="key">
        ///     The key of the value to get.
        /// </param>
        /// <param name="defValue">
        ///     The value returned if the key does not exist.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the object that implements the
        ///     <see cref="IReadOnlyDictionary{TKey, TValue}"/> interface contains an
        ///     element that has the specified key; otherwise, <see langword="false"/>.
        /// </returns>
        public static TValue TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key, TValue defValue = default)
        {
            if (source == null || key == null || !source.ContainsKey(key))
                return defValue;
            return source[key];
        }
    }
}
