#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: CacheInvestor.cs
// Version:  2020-01-28 20:16
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Investment
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    ///     Provides simple global caching of elements.
    /// </summary>
    public static class CacheInvestor
    {
        private static volatile object _syncObject;

        private static object SyncObject
        {
            get
            {
                if (_syncObject != null)
                    return _syncObject;
                var obj = new object();
                Interlocked.CompareExchange<object>(ref _syncObject, obj, null);
                return _syncObject;
            }
        }

        /// <summary>
        ///     Gets the default reference identifier of the provided type.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        public static int GetId<TElement>()
        {
            var type = typeof(TElement);
            return type.FullName?.GetHashCode() ?? (type.Namespace + type.Name).GetHashCode();
        }

        /// <summary>
        ///     Adds a default instance for the provided type under the specified
        ///     identifier.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="id">
        ///     The reference identifier.
        /// </param>
        public static void AddDefault<TElement>(int id) where TElement : new()
        {
            if (CacheProvider<TElement>.Storage.ContainsKey(id))
                return;
            lock (SyncObject)
            {
                var item = new TElement();
                CacheProvider<TElement>.Storage.Add(id, item);
            }
        }

        /// <summary>
        ///     Adds a default instance for the provided type under its default identifier.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        public static void AddDefault<TElement>() where TElement : new() =>
            AddDefault<TElement>(GetId<TElement>());

        /// <summary>
        ///     Adds the specified element under the specified identifier.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="id">
        ///     The reference identifier.
        /// </param>
        /// <param name="element">
        ///     The element to be added.
        /// </param>
        public static void AddItem<TElement>(int id, TElement element)
        {
            if (CacheProvider<TElement>.Storage.ContainsKey(id))
                return;
            lock (SyncObject)
                CacheProvider<TElement>.Storage.Add(id, element);
        }

        /// <summary>
        ///     Adds the specified element under the default identifier of the provided
        ///     type.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="element">
        ///     The value to be added.
        /// </param>
        public static void AddItem<TElement>(TElement element) =>
            AddItem(GetId<TElement>(), element);

        /// <summary>
        ///     Adds or updates an element under the specified identifier.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="id">
        ///     The reference identifier.
        /// </param>
        /// <param name="element">
        ///     The element to be added or updated.
        /// </param>
        public static void AddOrUpdate<TElement>(int id, TElement element)
        {
            lock (SyncObject)
            {
                RemoveItem<TElement>(id);
                CacheProvider<TElement>.Storage.Add(id, element);
            }
        }

        /// <summary>
        ///     Adds or updates an element under the default identifier of the provided
        ///     type.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="element">
        ///     The element to be added or updated.
        /// </param>
        public static void AddOrUpdate<TElement>(TElement element) =>
            AddOrUpdate(GetId<TElement>(), element);

        /// <summary>
        ///     Updates an element under the specified identifier.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="id">
        ///     The reference identifier.
        /// </param>
        /// <param name="element">
        ///     The element to be updated.
        /// </param>
        public static void Update<TElement>(int id, TElement element)
        {
            if (!CacheProvider<TElement>.Storage.ContainsKey(id))
                return;
            lock (SyncObject)
            {
                if (CacheProvider<TElement>.Storage[id] is IDisposable disposable)
                    disposable.Dispose();
                CacheProvider<TElement>.Storage[id] = element;
            }
        }

        /// <summary>
        ///     Updates an element under the default identifier of the provided type.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="element">
        ///     The element to be updated.
        /// </param>
        public static void Update<TElement>(TElement element) where TElement : new() =>
            Update(GetId<TElement>(), element);

        /// <summary>
        ///     Gets the default element under the default identifier of the provided type.
        ///     <para>
        ///         If it does not exist, it will be created.
        ///     </para>
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        public static TElement GetDefault<TElement>() where TElement : new()
        {
            var id = GetId<TElement>();
            if (TryGetItem<TElement>(id, out var result))
                return result;
            AddDefault<TElement>(id);
            return GetItem<TElement>(id);
        }

        /// <summary>
        ///     Gets the element associated with the specified identifier.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="id">
        ///     The reference identifier.
        /// </param>
        public static TElement GetItem<TElement>(int id) =>
            CacheProvider<TElement>.Storage.ContainsKey(id) ? CacheProvider<TElement>.Storage[id] : default;

        /// <summary>
        ///     Gets the element associated with the default identifier of the provided
        ///     type.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        public static TElement GetItem<TElement>() where TElement : new() =>
            GetItem<TElement>(GetId<TElement>());

        /// <summary>
        ///     Gets the element associated with the specified identifier.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="id">
        ///     The reference identifier.
        /// </param>
        /// <param name="result">
        ///     When this method returns <see langword="true"/>, the value associated with
        ///     the specified identifier; otherwise, the default value for the provided
        ///     type.
        /// </param>
        public static bool TryGetItem<TElement>(int id, out TElement result) =>
            CacheProvider<TElement>.Storage.TryGetValue(id, out result);

        /// <summary>
        ///     Gets the element associated with the default identifier of the provided
        ///     type.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="result">
        ///     When this method returns <see langword="true"/>, the value associated with
        ///     the specified identifier; otherwise, the default value for the provided
        ///     type.
        /// </param>
        public static bool TryGetItem<TElement>(out TElement result) where TElement : new() =>
            TryGetItem(GetId<TElement>(), out result);

        /// <summary>
        ///     Removes the element associated with the specified identifier.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="id">
        ///     The reference identifier.
        /// </param>
        public static void RemoveItem<TElement>(int id)
        {
            if (!CacheProvider<TElement>.Storage.ContainsKey(id))
                return;
            lock (SyncObject)
            {
                var item = (object)CacheProvider<TElement>.Storage[id];
                CacheProvider<TElement>.Storage.Remove(id);
                CurrentMemory.Destroy(ref item);
            }
        }

        /// <summary>
        ///     Removes the element associated with the default identifier of the provided
        ///     type.
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        public static void RemoveItem<TElement>() =>
            RemoveItem<TElement>(GetId<TElement>());

        private static class CacheProvider<T>
        {
            internal static volatile IDictionary<int, T> Storage = new Dictionary<int, T>();
        }
    }
}
