#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Reg.cs
// Version:  2019-10-31 22:30
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.Win32;
#if !x64
    using Intern;

#endif

    /// <summary>
    ///     Provides functionality for the Windows registry database.
    /// </summary>
    public static class Reg
    {
        private static Dictionary<int, string> CachedKeyFilters { get; set; }

        private const int MaxCacheSize = 16;
        private const int MaxPathLength = 255;

        private static RegistryKey AsRegistryKey(this string key, bool nullable = false)
        {
            switch (key?.ToUpperInvariant())
            {
                case "HKEY_CLASSES_ROOT":
                case "HKCR":
                    return Registry.ClassesRoot;
                case "HKEY_CURRENT_CONFIG":
                case "HKCC":
                    return Registry.CurrentConfig;
                case "HKEY_CURRENT_USER":
                case "HKCU":
                    return Registry.CurrentUser;
                case "HKEY_LOCAL_MACHINE":
                case "HKLM":
                    return Registry.LocalMachine;
                case "HKEY_PERFORMANCE_DATA":
                case "HKPD":
                    return Registry.PerformanceData;
                case "HKEY_USERS":
                case "HKU":
                    return Registry.Users;
                default:
                    return !nullable ? Registry.CurrentUser : null;
            }
        }

        private static RegistryKey GetKey(this string path)
        {
            var key = path.KeyFilter();
            if (key?.ContainsEx(Path.DirectorySeparatorChar) ?? false)
                key = key.Split(Path.DirectorySeparatorChar).First();
            return key.AsRegistryKey();
        }

        private static string GetSubKeyName(this string path)
        {
            var subKey = path.KeyFilter();
            if (!subKey?.ContainsEx(Path.DirectorySeparatorChar) ?? true)
                return subKey?.AsRegistryKey(true) == null ? subKey : null;
            var parts = subKey.Split(Path.DirectorySeparatorChar);
            var first = parts.FirstOrDefault().AsRegistryKey(true);
            if (first != null)
                subKey = parts.Skip(1).Join(Path.DirectorySeparatorChar);
            return subKey;
        }

        private static string KeyFilter(this string subKey)
        {
            try
            {
                var hashCode = subKey.GetHashCode();
                if (CachedKeyFilters?.ContainsKey(hashCode) ?? false)
                    return CachedKeyFilters[hashCode];
                var newSubKey = subKey;
                if (newSubKey.Contains(Path.DirectorySeparatorChar))
                    newSubKey = newSubKey.Split(Path.DirectorySeparatorChar).Where(s => !string.IsNullOrEmpty(s))
                                         .Select(s => s.Trim()).Join(Path.DirectorySeparatorChar);
                if (CachedKeyFilters == null)
                    CachedKeyFilters = new Dictionary<int, string>();
                if (CachedKeyFilters.Count > MaxCacheSize)
                    CachedKeyFilters.Remove(CachedKeyFilters.Keys.First());
                CachedKeyFilters[hashCode] = newSubKey;
                return newSubKey;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                return null;
            }
        }

#if any || x86
        private static string _regLowEnvPath,
                              _regNatEnvPath;

        private static string RegLowEnvPath =>
            _regLowEnvPath ?? (_regLowEnvPath = Path.Combine(ComSpec.LowestEnvDir, "reg.exe"));

        private static string RegNatEnvPath =>
            _regNatEnvPath ?? (_regNatEnvPath = Path.Combine(ComSpec.SysNativeEnvDir, "reg.exe"));
#else
        private const string RegLowEnvPath = "%SystemRoot%\\SysWOW64\\reg.exe";
        private const string RegNatEnvPath = "%SystemRoot%\\System32\\reg.exe";
#endif

        /// <summary>
        ///     Determines whether the specified subkey exists.
        /// </summary>
        /// <param name="key">
        ///     The root <see cref="Registry"/> key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to check.
        /// </param>
        public static bool SubKeyExists(RegistryKey key, string subKey)
        {
            try
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (subKey == null)
                    throw new ArgumentNullException(nameof(subKey));
                bool exists;
                using (var rKey = key.OpenSubKey(subKey.KeyFilter()))
                    exists = rKey != null;
                return exists;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified subkey exists.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to check.
        /// </param>
        public static bool SubKeyExists(string key, string subKey) =>
            SubKeyExists(key.AsRegistryKey(), subKey);

        /// <summary>
        ///     Determines whether the specified subkey exists.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to check.
        /// </param>
        public static bool SubKeyExists(string keyPath) =>
            SubKeyExists(keyPath.GetKey(), keyPath.GetSubKeyName());

        /// <summary>
        ///     Creates a new subkey.
        /// </summary>
        /// <param name="key">
        ///     The root <see cref="Registry"/> key that receives the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to create.
        /// </param>
        /// <param name="overwrite">
        ///     true to remove an existing target before creating; otherwise, false.
        /// </param>
        public static bool CreateNewSubKey(RegistryKey key, string subKey, bool overwrite = false)
        {
            var rKey = default(RegistryKey);
            try
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (subKey == null)
                    throw new ArgumentNullException(nameof(subKey));
                var path = string.Concat(key, Path.DirectorySeparatorChar, subKey);
                if (path.Length > MaxPathLength)
                    throw new ArgumentOutOfRangeException(nameof(path));
                if (overwrite && SubKeyExists(key, subKey))
                    RemoveSubKey(key, subKey);
                rKey = key.CreateSubKey(subKey.KeyFilter());
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            finally
            {
                rKey?.Dispose();
            }
            return SubKeyExists(key, subKey);
        }

        /// <summary>
        ///     Creates a new subkey.
        /// </summary>
        /// <param name="key">
        ///     The root key that receives the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to create.
        /// </param>
        /// <param name="overwrite">
        ///     true to remove an existing target before creating; otherwise, false.
        /// </param>
        public static bool CreateNewSubKey(string key, string subKey, bool overwrite = false) =>
            CreateNewSubKey(key.AsRegistryKey(), subKey, overwrite);

        /// <summary>
        ///     Creates a new subkey.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to create.
        /// </param>
        /// <param name="overwrite">
        ///     true to remove an existing target before creating; otherwise, false.
        /// </param>
        public static bool CreateNewSubKey(string keyPath, bool overwrite = false) =>
            CreateNewSubKey(keyPath.GetKey(), keyPath.GetSubKeyName(), overwrite);

        /// <summary>
        ///     Removes an existing subkey.
        /// </summary>
        /// <param name="key">
        ///     The root <see cref="Registry"/> key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to remove.
        /// </param>
        public static bool RemoveSubKey(RegistryKey key, string subKey)
        {
            try
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (subKey == null)
                    throw new ArgumentNullException(nameof(subKey));
                if (SubKeyExists(key, subKey))
                    key.DeleteSubKeyTree(subKey.KeyFilter());
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return !SubKeyExists(key, subKey);
        }

        /// <summary>
        ///     Removes an existing subkey.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to remove.
        /// </param>
        public static bool RemoveSubKey(string key, string subKey) =>
            RemoveSubKey(key.AsRegistryKey(), subKey);

        /// <summary>
        ///     Removes an existing subkey.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to remove.
        /// </param>
        public static bool RemoveSubKey(string keyPath) =>
            RemoveSubKey(keyPath.GetKey(), keyPath.GetSubKeyName());

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="IEnumerable{T}"/> with all subkeys of
        ///     the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root <see cref="Registry"/> key which contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        public static IEnumerable<string> GetSubKeys(RegistryKey key, string subKey)
        {
            try
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (string.IsNullOrEmpty(subKey))
                    return key.GetSubKeyNames();
                var sKey = subKey.KeyFilter();
                if (string.IsNullOrEmpty(sKey))
                    throw new ArgumentNullException(nameof(sKey));
                if (!SubKeyExists(key, subKey))
                    throw new PathNotFoundException(string.Concat(key, Path.DirectorySeparatorChar, subKey));
                using (var rKey = key.OpenSubKey(sKey))
                {
                    var sKeys = rKey?.GetSubKeyNames();
                    return sKeys?.Select(e => string.Concat(sKey, Path.DirectorySeparatorChar, e));
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="IEnumerable{T}"/> with all subkeys of
        ///     the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key which contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        public static IEnumerable<string> GetSubKeys(string key, string subKey) =>
            GetSubKeys(key.AsRegistryKey(), subKey);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="IEnumerable{T}"/> with all subkeys of
        ///     the specified registry path.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to read.
        /// </param>
        public static IEnumerable<string> GetSubKeys(string keyPath) =>
            GetSubKeys(keyPath.GetKey(), keyPath.GetSubKeyName());

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="IEnumerable{T}"/> with the full subkey
        ///     tree of the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root <see cref="Registry"/> key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        /// <param name="timelimit">
        ///     The time limit in milliseconds.
        /// </param>
        public static IEnumerable<string> GetSubKeyTree(RegistryKey key, string subKey, int timelimit = 30000) =>
            GetSubKeys(key, subKey)?.RecursiveSelect(e => GetSubKeys(key, e), timelimit);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="IEnumerable{T}"/> with the full subkey
        ///     tree of the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        /// <param name="timelimit">
        ///     The time limit in milliseconds.
        /// </param>
        public static IEnumerable<string> GetSubKeyTree(string key, string subKey, int timelimit = 30000) =>
            GetSubKeyTree(key.AsRegistryKey(), subKey, timelimit);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="IEnumerable{T}"/> with the full subkey
        ///     tree of the specified registry path.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to read.
        /// </param>
        /// <param name="timelimit">
        ///     The time limit in milliseconds.
        /// </param>
        public static IEnumerable<string> GetSubKeyTree(string keyPath, int timelimit = 30000) =>
            GetSubKeyTree(keyPath.GetKey(), keyPath.GetSubKeyName(), timelimit);

        /// <summary>
        ///     Copies an existing subkey to a new location.
        /// </summary>
        /// <param name="srcKey">
        ///     The root <see cref="Registry"/> key that contains the source subkey.
        /// </param>
        /// <param name="srcSubKey">
        ///     The name of the subkey to copy.
        /// </param>
        /// <param name="destKey">
        ///     The root <see cref="Registry"/> key that should contain the new subkey.
        /// </param>
        /// <param name="destSubKey">
        ///     The new path and name of the destination subkey.
        /// </param>
        /// <param name="overwrite">
        ///     true to remove an existing target before copying; otherwise, false.
        /// </param>
        public static bool CopySubKey(RegistryKey srcKey, string srcSubKey, RegistryKey destKey, string destSubKey, bool overwrite = false)
        {
            try
            {
                if (srcKey == null)
                    throw new ArgumentNullException(nameof(srcKey));
                if (srcSubKey == null)
                    throw new ArgumentNullException(nameof(srcSubKey));
                if (destKey == null)
                    throw new ArgumentNullException(nameof(destKey));
                if (destSubKey == null)
                    throw new ArgumentNullException(nameof(destSubKey));
                if (!SubKeyExists(srcKey, srcSubKey))
                    throw new PathNotFoundException(string.Concat(srcKey, Path.DirectorySeparatorChar, srcSubKey));
                var destPath = string.Concat(destKey, Path.DirectorySeparatorChar, destSubKey);
                if (destPath.Length > MaxPathLength)
                    throw new ArgumentOutOfRangeException(nameof(destPath));
                if (overwrite && SubKeyExists(destKey, destSubKey))
                    RemoveSubKey(destKey, destSubKey);
                var releaseQueue = new Queue<RegistryKey>();
                var srcItem = srcKey.OpenSubKey(srcSubKey.KeyFilter());
                releaseQueue.Enqueue(srcItem);
                var destItem = destKey.CreateSubKey(destSubKey.KeyFilter());
                releaseQueue.Enqueue(destItem);
                var copyQueue = new Queue<(RegistryKey, RegistryKey)>();
                copyQueue.Enqueue((srcItem, destItem));
                try
                {
                    do
                    {
                        var (item1, item2) = copyQueue.Dequeue();
                        srcItem = item1;
                        destItem = item2;
                        foreach (var entry in srcItem.GetValueNames())
                        {
                            var value = srcItem.GetValue(entry);
                            var kind = srcItem.GetValueKind(entry);
                            destItem.SetValue(entry, value, kind);
                        }
                        foreach (var name in srcItem.GetSubKeyNames())
                        {
                            var srcSubItem = srcItem.OpenSubKey(name);
                            releaseQueue.Enqueue(srcSubItem);
                            var destSubItem = destItem.CreateSubKey(name);
                            releaseQueue.Enqueue(destSubItem);
                            copyQueue.Enqueue((srcSubItem, destSubItem));
                        }
                    }
                    while (copyQueue.Any());
                }
                finally
                {
                    do
                    {
                        var item = releaseQueue.Dequeue();
                        item?.Dispose();
                    }
                    while (releaseQueue.Any());
                }
                return SubKeyExists(destKey, destSubKey);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Copies an existing subkey to a new location.
        /// </summary>
        /// <param name="srcKey">
        ///     The root key that contains the source subkey.
        /// </param>
        /// <param name="srcSubKey">
        ///     The name of the subkey to copy.
        /// </param>
        /// <param name="destKey">
        ///     The root key that should contain the new subkey.
        /// </param>
        /// <param name="destSubKey">
        ///     The new path and name of the destination subkey.
        /// </param>
        /// <param name="overwrite">
        ///     true to remove an existing target before copying; otherwise, false.
        /// </param>
        public static bool CopySubKey(string srcKey, string srcSubKey, string destKey, string destSubKey, bool overwrite = false) =>
            CopySubKey(srcKey.AsRegistryKey(), srcSubKey, destKey.AsRegistryKey(), destSubKey, overwrite);

        /// <summary>
        ///     Copies an existing subkey to a new location.
        /// </summary>
        /// <param name="srcKeyPath">
        ///     The full path of the source key to copy.
        /// </param>
        /// <param name="destKeyPath">
        ///     The full path of the destination key.
        /// </param>
        /// <param name="overwrite">
        ///     true to remove an existing target before copying; otherwise, false.
        /// </param>
        public static bool CopySubKey(string srcKeyPath, string destKeyPath, bool overwrite = false) =>
            CopySubKey(srcKeyPath.GetKey(), srcKeyPath.GetSubKeyName(), destKeyPath.GetKey(), destKeyPath.GetSubKeyName(), overwrite);

        /// <summary>
        ///     Moves an existing subkey to a new location.
        /// </summary>
        /// <param name="oldKey">
        ///     The root <see cref="Registry"/> key that contains the source subkey.
        /// </param>
        /// <param name="oldSubKey">
        ///     The name of the subkey to move.
        /// </param>
        /// <param name="newKey">
        ///     The root <see cref="Registry"/> key that should contain the new subkey.
        /// </param>
        /// <param name="newSubKey">
        ///     The new path and name of the subkey.
        /// </param>
        /// <param name="overwrite">
        ///     true to remove an existing target before moving; otherwise, false.
        /// </param>
        public static bool MoveSubKey(RegistryKey oldKey, string oldSubKey, RegistryKey newKey, string newSubKey, bool overwrite = false)
        {
            try
            {
                if (!SubKeyExists(oldKey, oldSubKey))
                    throw new PathNotFoundException(string.Concat(oldKey, Path.DirectorySeparatorChar, oldSubKey));
                if (overwrite && SubKeyExists(newKey, newSubKey))
                    RemoveSubKey(newKey, newSubKey);
                if (!CopySubKey(oldKey, oldSubKey, newKey, newSubKey) || !RemoveSubKey(oldKey, oldSubKey))
                    return false;
                return !SubKeyExists(oldKey, oldSubKey) && SubKeyExists(newKey, newSubKey);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Moves an existing subkey to a new location.
        /// </summary>
        /// <param name="oldKey">
        ///     The root key that contains the source subkey.
        /// </param>
        /// <param name="oldSubKey">
        ///     The name of the subkey to move.
        /// </param>
        /// <param name="newKey">
        ///     The root key that should contain the new subkey.
        /// </param>
        /// <param name="newSubKey">
        ///     The new path and name of the subkey.
        /// </param>
        /// <param name="overwrite">
        ///     true to remove an existing target before moving; otherwise, false.
        /// </param>
        public static bool MoveSubKey(string oldKey, string oldSubKey, string newKey, string newSubKey, bool overwrite = false) =>
            MoveSubKey(oldKey.AsRegistryKey(), oldSubKey, newKey.AsRegistryKey(), newSubKey, overwrite);

        /// <summary>
        ///     Moves an existing subkey to a new location.
        /// </summary>
        /// <param name="oldKeyPath">
        ///     The full path of the source key to copy.
        /// </param>
        /// <param name="newKeyPath">
        ///     The full path of the destination key.
        /// </param>
        /// <param name="overwrite">
        ///     true to remove an existing target before moving; otherwise, false.
        /// </param>
        public static bool MoveSubKey(string oldKeyPath, string newKeyPath, bool overwrite = false) =>
            MoveSubKey(oldKeyPath.GetKey(), oldKeyPath.GetSubKeyName(), newKeyPath.GetKey(), newKeyPath.GetSubKeyName(), overwrite);

        /// <summary>
        ///     Determines whether the specified entry exists.
        /// </summary>
        /// <param name="key">
        ///     The root <see cref="Registry"/> key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey which contains the entry.
        /// </param>
        /// <param name="entry">
        ///     The entry to check.
        /// </param>
        public static bool EntryExists(RegistryKey key, string subKey, string entry)
        {
            var value = Read<object>(key, subKey, entry);
            return value != null;
        }

        /// <summary>
        ///     Determines whether the specified entry exists.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey which contains the entry.
        /// </param>
        /// <param name="entry">
        ///     The entry to check.
        /// </param>
        public static bool EntryExists(string key, string subKey, string entry) =>
            EntryExists(key.AsRegistryKey(), subKey, entry);

        /// <summary>
        ///     Determines whether the specified entry exists.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key which contains the entry.
        /// </param>
        /// <param name="entry">
        ///     The entry to check.
        /// </param>
        public static bool EntryExists(string keyPath, string entry) =>
            EntryExists(keyPath.GetKey(), keyPath.GetSubKeyName(), entry);

        /// <summary>
        ///     Retrieves the value associated with the specified entry of the specified registry path.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="key">
        ///     The root <see cref="Registry"/> key that contains the subkey to open.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey that contains the entry to read.
        /// </param>
        /// <param name="entry">
        ///     The entry to read
        /// </param>
        /// <param name="defValue">
        ///     The value that is used as default.
        /// </param>
        public static TValue Read<TValue>(RegistryKey key, string subKey, string entry, TValue defValue = default)
        {
            var value = defValue;
            try
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (subKey == null)
                    throw new ArgumentNullException(nameof(subKey));
                if (SubKeyExists(key, subKey))
                {
                    object objValue;
                    using (var rKey = key.OpenSubKey(subKey.KeyFilter()))
                        objValue = rKey?.GetValue(entry, value);
                    if (objValue != null)
                    {
                        var valueType = typeof(TValue);
                        var objType = objValue.GetType();
                        if (valueType != objType && valueType.IsSerializable && objType == typeof(byte[]))
                        {
                            var bytes = objValue as byte[];
                            var obj = bytes?.DeserializeObject<object>();
                            if (obj != null)
                                objValue = obj;
                        }
                        value = (TValue)objValue;
                    }
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return value;
        }

        /// <summary>
        ///     Retrieves the value associated with the specified entry of the specified registry path.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="key">
        ///     The root key that contains the subkey to open.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey that contains the entry to read.
        /// </param>
        /// <param name="entry">
        ///     The entry to read
        /// </param>
        /// <param name="defValue">
        ///     The value that is used as default.
        /// </param>
        public static TValue Read<TValue>(string key, string subKey, string entry, TValue defValue = default) =>
            Read(key.AsRegistryKey(), subKey, entry, defValue);

        /// <summary>
        ///     Retrieves the value associated with the specified entry of the specified registry path.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="keyPath">
        ///     The full path of the key that contains the entry to read.
        /// </param>
        /// <param name="entry">
        ///     The entry to read
        /// </param>
        /// <param name="defValue">
        ///     The value that is used as default.
        /// </param>
        public static TValue Read<TValue>(string keyPath, string entry, TValue defValue = default) =>
            Read(keyPath.GetKey(), keyPath.GetSubKeyName(), entry, defValue);

        /// <summary>
        ///     Retrieves the value associated with the specified entry of the specified registry path.
        ///     <para>
        ///         A non-string value is converted to a valid <see cref="string"/>.
        ///     </para>
        /// </summary>
        /// <param name="key">
        ///     The root <see cref="Registry"/> key that contains the subkey to open.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey that contains the entry to read.
        /// </param>
        /// <param name="entry">
        ///     The entry to read
        /// </param>
        /// <param name="defValue">
        ///     The value that is used as default.
        /// </param>
        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        public static string ReadString(RegistryKey key, string subKey, string entry, string defValue = "")
        {
            var value = defValue;
            try
            {
                var objValue = Read<object>(key, subKey, entry, defValue);
                if (objValue == null)
                    throw new ArgumentNullException(nameof(objValue));
                if (objValue is string[] strs)
                    value = strs.Join(Environment.NewLine);
                else if (objValue is byte[] bytes)
                    value = bytes.Encode(BinaryToTextEncoding.Base16);
                else
                    value = objValue.ToString();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return value;
        }

        /// <summary>
        ///     Retrieves the value associated with the specified entry of the specified registry path.
        ///     <para>
        ///         A non-string value is converted to a valid <see cref="string"/>.
        ///     </para>
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey to open.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey that contains the entry to read.
        /// </param>
        /// <param name="entry">
        ///     The entry to read
        /// </param>
        /// <param name="defValue">
        ///     The value that is used as default.
        /// </param>
        public static string ReadString(string key, string subKey, string entry, string defValue) =>
            ReadString(key.AsRegistryKey(), subKey, entry, defValue);

        /// <summary>
        ///     Retrieves the value associated with the specified entry of the specified registry path.
        ///     <para>
        ///         A non-string value is converted to a valid <see cref="string"/>.
        ///     </para>
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key that contains the entry to read.
        /// </param>
        /// <param name="entry">
        ///     The entry to read
        /// </param>
        /// <param name="defValue">
        ///     The value that is used as default.
        /// </param>
        public static string ReadString(string keyPath, string entry, string defValue = "") =>
            ReadString(keyPath.GetKey(), keyPath.GetSubKeyName(), entry, defValue);

        /// <summary>
        ///     Copies an object into the specified entry of the registry database.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="key">
        ///     The root <see cref="Registry"/> key that contains the subkey to create or override.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to create or override.
        /// </param>
        /// <param name="entry">
        ///     The entry to create or override.
        /// </param>
        /// <param name="value">
        ///     The <see cref="object"/> to be written.
        /// </param>
        /// <param name="type">
        ///     The data type of the value to write.
        /// </param>
        public static bool Write<TValue>(RegistryKey key, string subKey, string entry, TValue value, RegistryValueKind type = RegistryValueKind.None)
        {
            try
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (subKey == null)
                    throw new ArgumentNullException(nameof(subKey));
                if (!SubKeyExists(key, subKey) && !CreateNewSubKey(key, subKey))
                    throw new PathNotFoundException(string.Concat(key, Path.DirectorySeparatorChar, subKey));
                using (var rKey = key.OpenSubKey(subKey.KeyFilter(), true))
                    try
                    {
                        object newValue = value;
                        var valueType = typeof(TValue);
                        if (type == RegistryValueKind.None || type == RegistryValueKind.Binary)
                        {
                            if (type == RegistryValueKind.None && valueType == typeof(string))
                            {
                                rKey?.SetValue(entry, newValue, RegistryValueKind.String);
                                return true;
                            }
                            if (valueType.IsSerializable && valueType != typeof(byte[]))
                            {
                                newValue = value.SerializeObject();
                                rKey?.SetValue(entry, newValue, RegistryValueKind.Binary);
                                return true;
                            }
                        }
                        rKey?.SetValue(entry, newValue, type);
                        return true;
                    }
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        Log.Write(ex);
                        return false;
                    }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Copies an object into the specified entry of the registry database.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="key">
        ///     The root key that contains the subkey to create or override.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to create or override.
        /// </param>
        /// <param name="entry">
        ///     The entry to create or override.
        /// </param>
        /// <param name="value">
        ///     The <see cref="object"/> to be written.
        /// </param>
        /// <param name="type">
        ///     The data type of the value to write.
        /// </param>
        public static bool Write<TValue>(string key, string subKey, string entry, TValue value, RegistryValueKind type = RegistryValueKind.None) =>
            Write(key.AsRegistryKey(), subKey, entry, value, type);

        /// <summary>
        ///     Copies an object into the specified entry of the registry database.
        /// </summary>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="keyPath">
        ///     The full path of the key to create or override.
        /// </param>
        /// <param name="entry">
        ///     The entry to create or override.
        /// </param>
        /// <param name="value">
        ///     The <see cref="object"/> to be written.
        /// </param>
        /// <param name="type">
        ///     The data type of the value to write.
        /// </param>
        public static bool Write<TValue>(string keyPath, string entry, TValue value, RegistryValueKind type = RegistryValueKind.None) =>
            Write(keyPath.GetKey(), keyPath.GetSubKeyName(), entry, value, type);

        /// <summary>
        ///     Removes the specified entry from the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root <see cref="Registry"/> key that contains the subkey with the entry to remove.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey with the entry to remove.
        /// </param>
        /// <param name="entry">
        ///     The entry to remove.
        /// </param>
        public static bool RemoveEntry(RegistryKey key, string subKey, string entry)
        {
            try
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (subKey == null)
                    throw new ArgumentNullException(nameof(subKey));
                if (SubKeyExists(key, subKey))
                    using (var rKey = key.OpenSubKey(subKey.KeyFilter(), true))
                        rKey?.DeleteValue(entry);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return !EntryExists(key, subKey, entry);
        }

        /// <summary>
        ///     Removes the specified entry from the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey with the entry to remove.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey with the entry to remove.
        /// </param>
        /// <param name="entry">
        ///     The entry to remove.
        /// </param>
        public static bool RemoveEntry(string key, string subKey, string entry) =>
            RemoveEntry(key.AsRegistryKey(), subKey, entry);

        /// <summary>
        ///     Removes the specified entry from the specified <see cref="Registry"/> path.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key with the entry to remove.
        /// </param>
        /// <param name="entry">
        ///     The entry to remove.
        /// </param>
        public static bool RemoveEntry(string keyPath, string entry) =>
            RemoveEntry(keyPath.GetKey(), keyPath.GetSubKeyName(), entry);

        /// <summary>
        ///     Imports the specified REG file to the registry.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to import.
        /// </param>
        /// <param name="elevated">
        ///     true to import with highest user permissions; otherwise, false.
        /// </param>
        /// <param name="native">
        ///     true to import with the system native architecture; otherwise, false.
        /// </param>
#if any || x86
            public static bool ImportFile(string path, bool elevated = false, bool native = false)
#else
            public static bool ImportFile(string path, bool elevated = false, bool native = true)
#endif
        {
            try
            {
                var filePath = PathEx.Combine(path);
                if (string.IsNullOrEmpty(filePath))
                    throw new ArgumentNullException(nameof(path));
                if (!File.Exists(filePath))
                    throw new PathNotFoundException(filePath);
                if (Log.DebugMode > 1)
                    Log.Write($"IMPORT: \"{filePath}\"");
                using (var p = ProcessEx.Start(native ? RegNatEnvPath : RegLowEnvPath, $"IMPORT \"{filePath}\"", elevated, ProcessWindowStyle.Hidden, false))
                    if (p?.HasExited ?? false)
                        p.WaitForExit(3000);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Creates a new REG file with the specified content, imports it into the registry, and then
        ///     deletes the file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to import.
        /// </param>
        /// <param name="content">
        ///     The full content of the file to import.
        /// </param>
        /// <param name="elevated">
        ///     true to import with highest user permissions; otherwise, false.
        /// </param>
        /// <param name="native">
        ///     true to import with the system native architecture; otherwise, false.
        /// </param>
#if any || x86
            public static bool ImportFile(string path, string[] content, bool elevated = false, bool native = false)
#else
            public static bool ImportFile(string path, string[] content, bool elevated = false, bool native = true)
#endif
        {
            try
            {
                if (path == null)
                    throw new ArgumentNullException(nameof(path));
                var filePath = PathEx.Combine(path);
                if (string.IsNullOrEmpty(filePath))
                    throw new ArgumentNullException(nameof(filePath));
                if (content == null)
                    throw new ArgumentNullException(nameof(content));
                if (content.Length == 0)
                    throw new ArgumentOutOfRangeException(nameof(content));
                if (File.Exists(filePath))
                    File.Delete(filePath);
                using (var sw = new StreamWriter(filePath, true, Encoding.GetEncoding(1252)))
                {
                    var head = false;
                    foreach (var line in content.Where(Comparison.IsNotEmpty))
                    {
                        if (!head)
                        {
                            head = true;
                            const string header = "Windows Registry Editor Version 5.00";
                            if (!line.StartsWithEx("REGEDIT4", header))
                                sw.WriteLine(header);
                        }
                        if (line.StartsWith("[", StringComparison.Ordinal))
                            sw.WriteLine();
                        sw.WriteLine(line);
                    }
                    sw.WriteLine();
                    sw.WriteLine();
                }
                var imported = ImportFile(filePath, elevated, native);
                if (File.Exists(filePath))
                    File.Delete(filePath);
                return imported;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Creates a new REG file with the specified content, imports it into the registry, and then
        ///     deletes the file.
        /// </summary>
        /// <param name="content">
        ///     The full content of the file to import.
        /// </param>
        /// <param name="elevated">
        ///     true to import with highest user permissions; otherwise, false.
        /// </param>
        /// <param name="native">
        ///     true to import with the system native architecture; otherwise, false.
        /// </param>
#if any || x86
            public static bool ImportFile(string[] content, bool elevated = false, bool native = false) =>
#else
            public static bool ImportFile(string[] content, bool elevated = false, bool native = true) =>
#endif
                ImportFile(FileEx.GetUniqueTempPath("tmp", ".reg"), content, elevated, native);

        /// <summary>
        ///     Exports the full content of the specified registry paths into an REG file.
        /// </summary>
        /// <param name="destPath">
        ///     The full path of the file to create or override.
        /// </param>
        /// <param name="elevated">
        ///     true to export with highest user permissions; otherwise, false.
        /// </param>
        /// <param name="keyPaths">
        ///     The full paths of the keys to export.
        /// </param>
        /// <param name="native">
        ///     true to import with the system native architecture; otherwise, false.
        /// </param>
        public static bool ExportKeys(string destPath, bool elevated, bool native, params string[] keyPaths)
        {
            try
            {
                if (destPath == null)
                    throw new ArgumentNullException(nameof(destPath));
                if (keyPaths == null)
                    throw new ArgumentNullException(nameof(keyPaths));
                if (keyPaths.Length == 0)
                    throw new ArgumentOutOfRangeException(nameof(keyPaths));
                var filePath = PathEx.Combine(destPath);
                if (string.IsNullOrEmpty(filePath))
                    throw new ArgumentInvalidException(nameof(destPath));
                var destDir = Path.GetDirectoryName(filePath);
                if (string.IsNullOrEmpty(destDir))
                    throw new ArgumentNullException(nameof(destDir));
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);
                const string header = "Windows Registry Editor Version 5.00";
                var encoding = Encoding.GetEncoding(1252);
                File.WriteAllText(filePath, $@"{header}{Environment.NewLine}", encoding);
                var count = 0;
                foreach (var key in keyPaths)
                {
                    var path = FileEx.GetUniqueTempPath("reg", null, 8);
                    if (Log.DebugMode > 1)
                        Log.Write($"EXPORT: \"{key}\" TO \"{path}\"");
                    using (var p = ProcessEx.Start(native ? RegNatEnvPath : RegLowEnvPath, $"EXPORT \"{key}\" \"{path}\" /y", elevated, ProcessWindowStyle.Hidden, false))
                        if (p?.HasExited ?? false)
                            p.WaitForExit(5000);
                    var lines = File.ReadAllLines(path).Skip(1);
                    File.AppendAllText(filePath, lines.Join(Environment.NewLine), encoding);
                    FileEx.TryDelete(path);
                    count++;
                }
                return count == keyPaths.Length;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Exports the full content of the specified registry paths into an REG file.
        /// </summary>
        /// <param name="destPath">
        ///     The full path of the file to create or override.
        /// </param>
        /// <param name="elevated">
        ///     true to export with highest user permissions; otherwise, false.
        /// </param>
        /// <param name="keyPaths">
        ///     The full paths of the keys to export.
        /// </param>
        public static bool ExportKeys(string destPath, bool elevated, params string[] keyPaths) =>
#if any || x86
            ExportKeys(destPath, elevated, false, keyPaths);
#else
            ExportKeys(destPath, elevated, true, keyPaths);
#endif

        /// <summary>
        ///     Exports the full content of the specified registry paths into an REG file.
        /// </summary>
        /// <param name="destPath">
        ///     The full path of the file to create or override.
        /// </param>
        /// <param name="keyPaths">
        ///     The full paths of the keys to export.
        /// </param>
        public static bool ExportKeys(string destPath, params string[] keyPaths) =>
#if any || x86
            ExportKeys(destPath, false, false, keyPaths);
#else
            ExportKeys(destPath, false, true, keyPaths);
#endif
    }
}
