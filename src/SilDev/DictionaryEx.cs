#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: DictionaryEx.cs
// Version:  2023-12-08 07:14
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System.Collections.Generic;

    public static class DictionaryEx
    {
        /// <summary>
        ///     Attempts to add  the specified key and value to this dictionary.
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
        ///     The <see cref="Dictionary{TKey, TValue}"/> to add the key/value pair.
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
        ///     The <see cref="Dictionary{TKey, TValue}"/> to set the key/value pair.
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
        /// </returns>
        public static TValue TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue defValue = default)
        {
            if (source == null || key == null || !source.ContainsKey(key))
                return defValue;
            return source[key];
        }
    }
}
