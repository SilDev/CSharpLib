#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Reg.cs
// Version:  2016-10-28 08:29
// 
// Copyright (c) 2016, Si13n7 Developments (r)
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
    using System.Text.RegularExpressions;
    using Microsoft.Win32;

    /// <summary>
    ///     Provides functionality for the Windows registry database.
    /// </summary>
    public static class Reg
    {
        /// <summary>
        ///     Provides enumerated values which present the root keys of the Windows registry.
        /// </summary>
        public enum RegKey
        {
            /// <summary>
            ///     Defines the types (or classes) of documents and the properties associated withthose types.
            ///     This field reads the Windows registry base key HKEY_CLASSES_ROOT.
            /// </summary>
            ClassesRoot,

            /// <summary>
            ///     Contains configuration information pertaining to the hardware that is not specific to the
            ///     user. This field reads the Windows registry base key HKEY_CURRENT_CONFIG.
            /// </summary>
            CurrentConfig,

            /// <summary>
            ///     Contains information about the current user preferences. This field reads the Windows
            ///     registry base key HKEY_CURRENT_USER.
            /// </summary>
            CurrentUser,

            /// <summary>
            ///     Contains the configuration data for the local machine. This field reads the Windows
            ///     registry base key HKEY_LOCAL_MACHINE.
            /// </summary>
            LocalMachine,

            /// <summary>
            ///     Contains performance information for software components. This field reads the Windows
            ///     registry base key HKEY_PERFORMANCE_DATA.
            /// </summary>
            PerformanceData,

            /// <summary>
            ///     Contains information about the default user configuration. This field reads the Windows
            ///     registry base key HKEY_USERS.
            /// </summary>
            Users
        }

        /// <summary>
        ///     Specifies the data types to use when storing values in the registry, or identifies the
        ///     data type of a value in the registry.
        /// </summary>
        public enum RegValueKind
        {
            /// <summary>
            ///     No data type.
            /// </summary>
            None = RegistryValueKind.None,

            /// <summary>
            ///     A null-terminated string. This value is equivalent to the Win32 API registry data type
            ///     REG_SZ.
            /// </summary>
            String = RegistryValueKind.String,

            /// <summary>
            ///     Binary data in any form. This value is equivalent to the Win32 API registry data type
            ///     REG_BINARY.
            /// </summary>
            Binary = RegistryValueKind.Binary,

            /// <summary>
            ///     A 32-bit binary number. This value is equivalent to the Win32 API registry data type
            ///     REG_DWORD.
            /// </summary>
            DWord = RegistryValueKind.DWord,

            /// <summary>
            ///     A 64-bit binary number. This value is equivalent to the Win32 API registry data type
            ///     REG_QWORD.
            /// </summary>
            QWord = RegistryValueKind.QWord,

            /// <summary>
            ///     A null-terminated string that contains unexpanded references to environment variables,
            ///     such as %PATH%, that are expanded when the value is retrieved. This value is equivalent
            ///     to the Win32 API registry data type REG_EXPAND_SZ.
            /// </summary>
            ExpandString = RegistryValueKind.ExpandString,

            /// <summary>
            ///     An array of null-terminated strings, terminated by two null characters. This value is
            ///     equivalent to the Win32 API registry data type REG_MULTI_SZ.
            /// </summary>
            MultiString = RegistryValueKind.MultiString
        }

        [SuppressMessage("ReSharper", "CanBeReplacedWithTryCastAndCheckForNull")]
        private static RegistryKey AsRegistryKey(this object key)
        {
            try
            {
                if (key is RegistryKey)
                    return (RegistryKey)key;
                if (key is RegKey)
                    switch ((RegKey)key)
                    {
                        case RegKey.ClassesRoot:
                            return Registry.ClassesRoot;
                        case RegKey.CurrentConfig:
                            return Registry.CurrentConfig;
                        case RegKey.LocalMachine:
                            return Registry.LocalMachine;
                        case RegKey.PerformanceData:
                            return Registry.PerformanceData;
                        case RegKey.Users:
                            return Registry.Users;
                        default:
                            return Registry.CurrentUser;
                    }
                if (!(key is string))
                    throw new ArgumentException();
                switch (((string)key).ToUpper())
                {
                    case "HKEY_CLASSES_ROOT":
                    case "HKCR":
                        return Registry.ClassesRoot;
                    case "HKEY_CURRENT_CONFIG":
                    case "HKCC":
                        return Registry.CurrentConfig;
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
                        return Registry.CurrentUser;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return Registry.CurrentUser;
            }
        }

        private static RegistryKey GetKey(this string key) =>
            key.ContainsEx(Path.DirectorySeparatorChar) ? key.Split(Path.DirectorySeparatorChar)[0].AsRegistryKey() : key.AsRegistryKey();

        private static string GetSubKey(this string key) =>
            key.ContainsEx(Path.DirectorySeparatorChar) ? key.Split(Path.DirectorySeparatorChar).Skip(1).Join(Path.DirectorySeparatorChar) : string.Empty;

        private static RegistryValueKind AsRegistryValueKind(this object type)
        {
            try
            {
                if (type == null)
                    throw new ArgumentNullException();
                switch ((type is string ? (string)type : type.ToString()).ToUpper())
                {
                    case "STRING":
                    case "STR":
                        return RegistryValueKind.String;
                    case "BINARY":
                    case "BIN":
                        return RegistryValueKind.Binary;
                    case "DWORD":
                    case "DW":
                        return RegistryValueKind.DWord;
                    case "QWORD":
                    case "QW":
                        return RegistryValueKind.QWord;
                    case "EXPANDSTRING":
                    case "ESTR":
                        return RegistryValueKind.ExpandString;
                    case "MULTISTRING":
                    case "MSTR":
                        return RegistryValueKind.MultiString;
                    default:
                        return RegistryValueKind.None;
                }
            }
            catch
            {
                return RegistryValueKind.None;
            }
        }

        private static bool SubKeyExist(object key, string subKey)
        {
            try
            {
                var rKey = key.AsRegistryKey().OpenSubKey(subKey);
                return rKey != null;
            }
            catch (Exception ex)
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
        public static bool SubKeyExist(RegKey key, string subKey) =>
            SubKeyExist(key as object, subKey);

        /// <summary>
        ///     Determines whether the specified subkey exists.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to check.
        /// </param>
        public static bool SubKeyExist(string key, string subKey) =>
            SubKeyExist(key as object, subKey);

        /// <summary>
        ///     Determines whether the specified subkey exists.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to check.
        /// </param>
        public static bool SubKeyExist(string keyPath) =>
            SubKeyExist(keyPath.GetKey(), keyPath.GetSubKey());

        private static bool CreateNewSubKey(object key, string subKey)
        {
            try
            {
                if (!SubKeyExist(key, subKey))
                    key.AsRegistryKey().CreateSubKey(subKey);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
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
        public static bool CreateNewSubKey(RegKey key, string subKey) =>
            CreateNewSubKey(key as object, subKey);

        /// <summary>
        ///     Creates a new subkey.
        /// </summary>
        /// <param name="key">
        ///     The root key that receives the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to create.
        /// </param>
        public static bool CreateNewSubKey(string key, string subKey) =>
            CreateNewSubKey(key as object, subKey);

        /// <summary>
        ///     Creates a new subkey.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to create.
        /// </param>
        public static bool CreateNewSubKey(string keyPath) =>
            CreateNewSubKey(keyPath.GetKey(), keyPath.GetSubKey());

        private static bool RemoveExistSubKey(object key, string subKey)
        {
            try
            {
                if (SubKeyExist(key, subKey))
                    key.AsRegistryKey().DeleteSubKeyTree(subKey);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
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
        public static bool RemoveExistSubKey(RegKey key, string subKey) =>
            RemoveExistSubKey(key as object, subKey);

        /// <summary>
        ///     Removes an existing subkey.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to remove.
        /// </param>
        public static bool RemoveExistSubKey(string key, string subKey) =>
            RemoveExistSubKey(key as object, subKey);

        /// <summary>
        ///     Removes an existing subkey.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to remove.
        /// </param>
        public static bool RemoveExistSubKey(string keyPath) =>
            RemoveExistSubKey(keyPath.GetKey(), keyPath.GetSubKey());

        private static List<string> GetSubKeyTree(object key, string subKey)
        {
            try
            {
                var subKeys = GetSubKeys(key, subKey);
                if (subKeys.Count <= 0)
                    return subKeys.OrderBy(x => x).ToList();
                var count = subKeys.Count;
                for (var i = 0; i < count; i++)
                {
                    var subs = GetSubKeys(key, subKeys[i]);
                    if (subs.Count <= 0)
                        continue;
                    subKeys.AddRange(subs);
                    count = subKeys.Count;
                }
                return subKeys.OrderBy(x => x).ToList();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="List{T}"/> with the full subkey tree of
        ///     the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        public static List<string> GetSubKeyTree(RegKey key, string subKey) =>
            GetSubKeyTree(key as object, subKey);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="List{T}"/> with the full subkey tree of
        ///     the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        public static List<string> GetSubKeyTree(string key, string subKey) =>
            GetSubKeyTree(key as object, subKey);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="List{T}"/> with the full subkey tree of
        ///     the specified registry path.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to read.
        /// </param>
        public static List<string> GetSubKeyTree(string keyPath) =>
            GetSubKeyTree(keyPath.GetKey(), keyPath.GetSubKey());

        private static List<string> GetSubKeys(object key, string subKey)
        {
            try
            {
                var keys = new List<string>();
                if (!SubKeyExist(key, subKey))
                    return keys;
                using (var rKey = key.AsRegistryKey().OpenSubKey(subKey))
                {
                    var sKey = rKey?.GetSubKeyNames();
                    if (sKey == null)
                        throw new ArgumentNullException(nameof(sKey));
                    keys.AddRange(sKey.Select(e => $"{subKey}\\{e}"));
                }
                return keys;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="List{T}"/> with all subkeys of the
        ///     specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key which contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        public static List<string> GetSubKeys(RegKey key, string subKey) =>
            GetSubKeys(key as object, subKey);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="List{T}"/> with all subkeys of the
        ///     specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key which contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        public static List<string> GetSubKeys(string key, string subKey) =>
            GetSubKeys(key as object, subKey);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="List{T}"/> with all subkeys of the
        ///     specified registry path.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to read.
        /// </param>
        public static List<string> GetSubKeys(string keyPath) =>
            GetSubKeys(keyPath.GetKey(), keyPath.GetSubKey());

        private static bool MoveSubKey(object key, string subKey, string newSubKeyName)
        {
            if (!SubKeyExist(key, subKey) || SubKeyExist(key, newSubKeyName))
                return false;
            if (!CopyKey(key, subKey, newSubKeyName))
                return false;
            try
            {
                key.AsRegistryKey().DeleteSubKeyTree(subKey);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Moves an existing subkey to a new location.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to move.
        /// </param>
        /// <param name="newSubKeyName">
        ///     The destination subkey.
        /// </param>
        public static bool MoveSubKey(RegKey key, string subKey, string newSubKeyName) =>
            MoveSubKey(key as object, subKey, newSubKeyName);

        /// <summary>
        ///     Moves an existing subkey to a new location.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to move.
        /// </param>
        /// <param name="newSubKeyName">
        ///     The destination subkey.
        /// </param>
        public static bool MoveSubKey(string key, string subKey, string newSubKeyName) =>
            MoveSubKey(key as object, subKey, newSubKeyName);

        /// <summary>
        ///     Moves an existing subkey to a new location.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to move.
        /// </param>
        /// <param name="newSubKeyName">
        ///     The destination subkey.
        /// </param>
        public static bool MoveSubKey(string keyPath, string newSubKeyName) =>
            MoveSubKey(keyPath.GetKey(), keyPath.GetSubKey(), newSubKeyName);

        private static bool CopyKey(object key, string subKey, string newSubKeyName)
        {
            if (!SubKeyExist(key, subKey) || SubKeyExist(key, newSubKeyName))
                return false;
            try
            {
                var destKey = key.AsRegistryKey().CreateSubKey(newSubKeyName);
                var srcKey = key.AsRegistryKey().OpenSubKey(subKey);
                RecurseCopyKey(srcKey, destKey);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Copies an existing subkey to a new location.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to copy.
        /// </param>
        /// <param name="newSubKeyName">
        ///     The destination subkey.
        /// </param>
        public static bool CopyKey(RegKey key, string subKey, string newSubKeyName) =>
            CopyKey(key as object, subKey, newSubKeyName);

        /// <summary>
        ///     Copies an existing subkey to a new location.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to copy.
        /// </param>
        /// <param name="newSubKeyName">
        ///     The destination subkey.
        /// </param>
        public static bool CopyKey(string key, string subKey, string newSubKeyName) =>
            CopyKey(key as object, subKey, newSubKeyName);

        /// <summary>
        ///     Copies an existing subkey to a new location.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to move.
        /// </param>
        /// <param name="newSubKeyName">
        ///     The destination subkey.
        /// </param>
        public static bool CopyKey(string keyPath, string newSubKeyName) =>
            CopyKey(keyPath.GetKey(), keyPath.GetSubKey(), newSubKeyName);

        private static void RecurseCopyKey(RegistryKey srcKey, RegistryKey destKey)
        {
            try
            {
                foreach (var valueName in srcKey.GetValueNames())
                {
                    var obj = srcKey.GetValue(valueName);
                    var valKind = srcKey.GetValueKind(valueName);
                    destKey.SetValue(valueName, obj, valKind);
                }
                foreach (var srcSubKeyName in srcKey.GetSubKeyNames())
                {
                    var srcSubKey = srcKey.OpenSubKey(srcSubKeyName);
                    var destSubKey = destKey.CreateSubKey(srcSubKeyName);
                    RecurseCopyKey(srcSubKey, destSubKey);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private static bool ValueExist(object key, string subKey, string entry, object type = null)
        {
            try
            {
                var value = ReadStringValue(key, subKey, entry, type).ToLower();
                return !string.IsNullOrWhiteSpace(value) && value != "none";
            }
            catch (Exception ex)
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
        ///     The path of the subkey which contains the entry.
        /// </param>
        /// <param name="entry">
        ///     The entry to check.
        /// </param>
        /// <param name="type">
        ///     The value type.
        /// </param>
        public static bool ValueExist(RegKey key, string subKey, string entry, RegValueKind type = RegValueKind.None)
        {
            try
            {
                var value = ReadStringValue(key, subKey, entry, type).ToLower();
                return !string.IsNullOrWhiteSpace(value) && value != "none";
            }
            catch (Exception ex)
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
        ///     The path of the subkey which contains the entry.
        /// </param>
        /// <param name="entry">
        ///     The entry to check.
        /// </param>
        /// <param name="type">
        ///     The value type.
        /// </param>
        public static bool ValueExist(string key, string subKey, string entry, RegValueKind type = RegValueKind.None)
        {
            try
            {
                var value = ReadStringValue(key, subKey, entry, type).ToLower();
                return !string.IsNullOrWhiteSpace(value) && value != "none";
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Determines whether the specified subkey exists.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key which contains the entry.
        /// </param>
        /// <param name="entry">
        ///     The entry to check.
        /// </param>
        /// <param name="type">
        ///     The value type.
        /// </param>
        public static bool ValueExist(string keyPath, string entry, RegValueKind type = RegValueKind.None) =>
            ValueExist(keyPath.GetKey(), keyPath.GetSubKey(), entry, type);

        private static Dictionary<string, string> GetAllTreeValues(object key, string subKey)
        {
            var tree = new Dictionary<string, string>();
            foreach (var sKey in GetSubKeyTree(key, subKey))
            {
                if (string.IsNullOrEmpty(sKey))
                    continue;
                var entries = GetValues(key, sKey);
                if (entries.Keys.Count <= 0)
                    continue;
                if (!tree.ContainsKey(sKey))
                    tree.Add(sKey, sKey.EncryptToMd5());
                foreach (var entry in entries)
                    try
                    {
                        tree.Add(entry.Key, entry.Value);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                    }
            }
            return tree;
        }

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="Dictionary{T,T}"/> with all subkey entries
        ///     and values of the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        public static Dictionary<string, string> GetAllTreeValues(RegKey key, string subKey) =>
            GetAllTreeValues(key as object, subKey);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="Dictionary{T,T}"/> with all subkeys entries
        ///     and values of the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        public static Dictionary<string, string> GetAllTreeValues(string key, string subKey) =>
            GetAllTreeValues(key as object, subKey);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="Dictionary{T,T}"/> with all subkeys entries
        ///     and values of the specified registry path.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to read.
        /// </param>
        public static Dictionary<string, string> GetAllTreeValues(string keyPath) =>
            GetAllTreeValues(keyPath.GetKey(), keyPath.GetSubKey());

        private static Dictionary<string, string> GetValues(object key, string subKey)
        {
            var values = new Dictionary<string, string>();
            try
            {
                if (!SubKeyExist(key, subKey))
                    throw new ArgumentException();
                using (var rKey = key.AsRegistryKey().OpenSubKey(subKey))
                {
                    var vNames = rKey?.GetValueNames();
                    if (vNames == null)
                        throw new ArgumentNullException(nameof(vNames));
                    foreach (var ent in rKey.GetValueNames())
                        try
                        {
                            var value = ReadStringValue(key, subKey, ent);
                            if (string.IsNullOrWhiteSpace(value))
                                continue;
                            var type = rKey.GetValueKind(ent);
                            values.Add(ent, $"ValueKind_{type}::{(type == RegistryValueKind.MultiString ? value.ToHexString() : value)}");
                        }
                        catch (Exception ex)
                        {
                            Log.Write(ex);
                        }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return values;
        }

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="Dictionary{T,T}"/> with all entries
        ///     and values of the top subkey of the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        public static Dictionary<string, string> GetValues(RegKey key, string subKey) =>
            GetValues(key as object, subKey);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="Dictionary{T,T}"/> with all entries
        ///     and values of the top subkey of the specified registry path.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to read.
        /// </param>
        public static Dictionary<string, string> GetValues(string key, string subKey) =>
            GetValues(key as object, subKey);

        /// <summary>
        ///     Returns a <see cref="string"/> based <see cref="Dictionary{T,T}"/> with all entries
        ///     and values of the top subkey of the specified registry path.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to read.
        /// </param>
        public static Dictionary<string, string> GetValues(string keyPath) =>
            GetValues(keyPath.GetKey(), keyPath.GetSubKey());

        private static object ReadValue(object key, string subKey, string entry, object type = null)
        {
            if (!SubKeyExist(key, subKey))
                return null;
            try
            {
                object ent;
                using (var rKey = key.AsRegistryKey().OpenSubKey(subKey))
                    ent = rKey?.GetValue(entry, type.AsRegistryValueKind());
                return ent;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Retrives the value associated with the specified entry of the specified registry path.
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
        /// <param name="type">
        ///     The data type of the value.
        /// </param>
        public static object ReadValue(RegKey key, string subKey, string entry, RegValueKind type = RegValueKind.None) =>
            ReadValue(key as object, subKey, entry, type);

        /// <summary>
        ///     Retrives the value associated with the specified entry of the specified registry path.
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
        /// <param name="type">
        ///     The data type of the value.
        /// </param>
        public static object ReadValue(string key, string subKey, string entry, RegValueKind type = RegValueKind.None) =>
            ReadValue(key as object, subKey, entry, type);

        /// <summary>
        ///     Retrives the value associated with the specified entry of the specified registry path.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key that contains the entry to read.
        /// </param>
        /// <param name="entry">
        ///     The entry to read
        /// </param>
        /// <param name="type">
        ///     The data type of the value.
        /// </param>
        public static object ReadValue(string keyPath, string entry, RegValueKind type = RegValueKind.None) =>
            ReadValue(keyPath.GetKey(), keyPath.GetSubKey(), entry, type);

        private static string ReadStringValue(object key, string subKey, string entry, object type = null)
        {
            if (!SubKeyExist(key, subKey))
                return string.Empty;
            try
            {
                string value;
                using (var rKey = key.AsRegistryKey().OpenSubKey(subKey))
                {
                    var objValue = rKey?.GetValue(entry, type.AsRegistryValueKind());
                    if (objValue == null)
                        return string.Empty;
                    if (objValue is string[])
                        value = (objValue as string[]).Join(Environment.NewLine);
                    else if (objValue is byte[])
                        value = (objValue as byte[]).ToHexString();
                    else
                        value = objValue.ToString();
                }
                return value;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     <para>
        ///         Retrives the value associated with the specified entry of the specified registry path.
        ///     </para>
        ///     <para>
        ///         A non-string value is converted to <see cref="string"/> before returning it.
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
        /// <param name="type">
        ///     The data type of the value.
        /// </param>
        public static string ReadStringValue(RegKey key, string subKey, string entry, RegValueKind type = RegValueKind.None) =>
            ReadStringValue(key as object, subKey, entry, type);

        /// <summary>
        ///     <para>
        ///         Retrives the value associated with the specified entry of the specified registry path.
        ///     </para>
        ///     <para>
        ///         A non-string value is converted to <see cref="string"/> before returning it.
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
        /// <param name="type">
        ///     The data type of the value.
        /// </param>
        public static string ReadStringValue(string key, string subKey, string entry, RegValueKind type = RegValueKind.None) =>
            ReadStringValue(key as object, subKey, entry, type);

        /// <summary>
        ///     <para>
        ///         Retrives the value associated with the specified entry of the specified registry path.
        ///     </para>
        ///     <para>
        ///         A non-string value is converted to <see cref="string"/> before returning it.
        ///     </para>
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key that contains the entry to read.
        /// </param>
        /// <param name="entry">
        ///     The entry to read
        /// </param>
        /// <param name="type">
        ///     The data type of the value.
        /// </param>
        public static string ReadStringValue(string keyPath, string entry, RegValueKind type = RegValueKind.None) =>
            ReadStringValue(keyPath.GetKey(), keyPath.GetSubKey(), entry, type);

        private static void WriteValue<T>(object key, string subKey, string entry, T value, object type = null)
        {
            if (!SubKeyExist(key, subKey))
                CreateNewSubKey(key, subKey);
            if (!SubKeyExist(key, subKey))
                return;
            using (var rKey = key.AsRegistryKey().OpenSubKey(subKey, true))
                try
                {
                    if (value is string)
                        if ((value as string).StartsWith("ValueKind") && type.AsRegistryValueKind() == RegistryValueKind.None)
                        {
                            var valKind = Regex.Match(value.ToString(), "ValueKind_(.+?)::").Groups[1].Value;
                            var val = (value as string).RemoveText($"ValueKind_{valKind}::");
                            switch (valKind)
                            {
                                case "String":
                                    rKey?.SetValue(entry, val, RegistryValueKind.String);
                                    return;
                                case "Binary":
                                    rKey?.SetValue(entry, val.FromHexStringToByteArray(), RegistryValueKind.Binary);
                                    return;
                                case "DWord":
                                    rKey?.SetValue(entry, val, RegistryValueKind.DWord);
                                    return;
                                case "QWord":
                                    rKey?.SetValue(entry, val, RegistryValueKind.QWord);
                                    return;
                                case "ExpandString":
                                    rKey?.SetValue(entry, val, RegistryValueKind.ExpandString);
                                    return;
                                case "MultiString":
                                    rKey?.SetValue(entry, val.FromHexString().SplitNewLine(), RegistryValueKind.MultiString);
                                    return;
                                default:
                                    return;
                            }
                        }
                    if (type.AsRegistryValueKind() == RegistryValueKind.None)
                        if (value is string)
                            rKey?.SetValue(entry, value, RegistryValueKind.String);
                        else if (value is byte[] ||
                                 value is double ||
                                 value is float)
                            rKey?.SetValue(entry, value, RegistryValueKind.Binary);
                        else if (value is byte ||
                                 value is int ||
                                 value is short)
                            rKey?.SetValue(entry, value, RegistryValueKind.DWord);
                        else if (value is IntPtr ||
                                 value is long)
                            rKey?.SetValue(entry, value, RegistryValueKind.QWord);
                        else if (value is string[])
                            rKey?.SetValue(entry, value, RegistryValueKind.MultiString);
                        else
                            rKey?.SetValue(entry, value, RegistryValueKind.None);
                    else
                        rKey?.SetValue(entry, value, type.AsRegistryValueKind());
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    try
                    {
                        rKey?.SetValue(entry, value, RegistryValueKind.String);
                    }
                    catch (Exception exc)
                    {
                        Log.Write(exc);
                    }
                }
        }

        /// <summary>
        ///     Copies an object into the specified entry of the registry database.
        /// </summary>
        /// <typeparam name="T">
        ///     <para>
        ///         The data type of the value. If no <see cref="RegValueKind"/> is not definied the following types
        ///         are auto declared:
        ///     </para>
        ///     <para>
        ///         <see cref="bool"/> as <see cref="RegValueKind.String"/>
        ///     </para>
        ///     <para>
        ///         <see cref="byte"/> as <see cref="RegValueKind.DWord"/>
        ///     </para>
        ///     <para>
        ///         <see cref="byte"/>[] as <see cref="RegValueKind.Binary"/>
        ///     </para>
        ///     <para>
        ///         <see cref="double"/> as <see cref="RegValueKind.Binary"/>
        ///     </para>
        ///     <para>
        ///         <see cref="float"/> as <see cref="RegValueKind.Binary"/>
        ///     </para>
        ///     <para>
        ///         <see cref="IntPtr"/> as <see cref="RegValueKind.QWord"/>
        ///     </para>
        ///     <para>
        ///         <see cref="int"/> as <see cref="RegValueKind.DWord"/>
        ///     </para>
        ///     <para>
        ///         <see cref="long"/> as <see cref="RegValueKind.QWord"/>
        ///     </para>
        ///     <para>
        ///         <see cref="short"/> as <see cref="RegValueKind.DWord"/>
        ///     </para>
        ///     <para>
        ///         <see cref="string"/> as <see cref="RegValueKind.String"/>
        ///     </para>
        ///     <para>
        ///         <see cref="string"/>[] as <see cref="RegValueKind.MultiString"/>
        ///     </para>
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
        public static void WriteValue<T>(RegKey key, string subKey, string entry, T value, RegValueKind type = RegValueKind.None) =>
            WriteValue(key as object, subKey, entry, value, type);

        /// <summary>
        ///     Copies an object into the specified entry of the registry database.
        /// </summary>
        /// <typeparam name="T">
        ///     <para>
        ///         The data type of the value. If <see cref="RegValueKind"/> is not definied the following types
        ///         are auto declared:
        ///     </para>
        ///     <para>
        ///         <see cref="bool"/> as <see cref="RegValueKind.String"/>
        ///     </para>
        ///     <para>
        ///         <see cref="byte"/> as <see cref="RegValueKind.DWord"/>
        ///     </para>
        ///     <para>
        ///         <see cref="byte"/>[] as <see cref="RegValueKind.Binary"/>
        ///     </para>
        ///     <para>
        ///         <see cref="double"/> as <see cref="RegValueKind.Binary"/>
        ///     </para>
        ///     <para>
        ///         <see cref="float"/> as <see cref="RegValueKind.Binary"/>
        ///     </para>
        ///     <para>
        ///         <see cref="IntPtr"/> as <see cref="RegValueKind.QWord"/>
        ///     </para>
        ///     <para>
        ///         <see cref="int"/> as <see cref="RegValueKind.DWord"/>
        ///     </para>
        ///     <para>
        ///         <see cref="long"/> as <see cref="RegValueKind.QWord"/>
        ///     </para>
        ///     <para>
        ///         <see cref="short"/> as <see cref="RegValueKind.DWord"/>
        ///     </para>
        ///     <para>
        ///         <see cref="string"/> as <see cref="RegValueKind.String"/>
        ///     </para>
        ///     <para>
        ///         <see cref="string"/>[] as <see cref="RegValueKind.MultiString"/>
        ///     </para>
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
        public static void WriteValue<T>(string key, string subKey, string entry, T value, RegValueKind type = RegValueKind.None) =>
            WriteValue(key as object, subKey, entry, value, type);

        /// <summary>
        ///     Copies an object into the specified entry of the registry database.
        /// </summary>
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
        public static void WriteValue(RegKey key, string subKey, string entry, object value, RegValueKind type = RegValueKind.None) =>
            WriteValue(key as object, subKey, entry, value, type);

        /// <summary>
        ///     Copies an object into the specified entry of the registry database.
        /// </summary>
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
        public static void WriteValue(string keyPath, string entry, object value, RegValueKind type = RegValueKind.None) =>
            WriteValue(keyPath.GetKey(), keyPath.GetSubKey(), entry, value, type);

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
        public static void RemoveValue(object key, string subKey, string entry)
        {
            if (!SubKeyExist(key, subKey))
                return;
            try
            {
                using (var rKey = AsRegistryKey(key).OpenSubKey(subKey, true))
                    rKey?.DeleteValue(entry);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
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
        public static void RemoveValue(RegKey key, string subKey, string entry) =>
            RemoveValue(key as object, subKey, entry);

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
        public static void RemoveValue(string key, string subKey, string entry) =>
            RemoveValue(key as object, subKey, entry);

        /// <summary>
        ///     Removes the specified entry from the specified registry path.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key with the entry to remove.
        /// </param>
        /// <param name="entry">
        ///     The entry to remove.
        /// </param>
        public static void RemoveValue(string keyPath, string entry) =>
            RemoveValue(keyPath.GetKey(), keyPath.GetSubKey(), entry);

        /// <summary>
        ///     Imports the specified REG file to the registry.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to import.
        /// </param>
        /// <param name="elevated">
        ///     true to import with highest user permissions; otherwise, false.
        /// </param>
        public static bool ImportFile(string path, bool elevated = false)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                if (!File.Exists(path))
                    throw new FileNotFoundException();
                if (Path.GetExtension(path).EqualsEx(".ini"))
                {
                    using (var p = ProcessEx.Start("%system%\\reg.exe", $"IMPORT \"{path}\"", elevated, ProcessWindowStyle.Hidden, false))
                        if (!p?.HasExited == true)
                            p?.WaitForExit(1000);
                    return true;
                }
                var root = Ini.Read("Root", "Sections", path);
                if (!root.Contains(","))
                    return false;
                foreach (var section in root.Split(','))
                {
                    var rootKey = Ini.Read(section, $"{section}_RootKey", path);
                    var subKey = Ini.Read(section, $"{section}_SubKey", path);
                    var values = Ini.Read(section, $"{section}_Values", path);
                    if (string.IsNullOrWhiteSpace(rootKey) || string.IsNullOrWhiteSpace(subKey) || string.IsNullOrWhiteSpace(values))
                        continue;
                    foreach (var value in values.Split(','))
                    {
                        CreateNewSubKey(rootKey.AsRegistryKey(), subKey);
                        WriteValue(rootKey.AsRegistryKey(), subKey, value, Ini.Read(section, value, path));
                    }
                }
                return true;
            }
            catch (Exception ex)
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
        /// <param name="contents">
        ///     The full content of the file to import.
        /// </param>
        /// <param name="elevated">
        ///     true to import with highest user permissions; otherwise, false.
        /// </param>
        public static bool ImportFile(string path, string[] contents, bool elevated = false)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                File.WriteAllLines(path, contents);
                var imported = ImportFile(path, elevated);
                if (File.Exists(path))
                    File.Delete(path);
                return imported;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Creates a new REG file with the specified content, imports it into the registry, and then
        ///     deletes the file.
        /// </summary>
        /// <param name="contents">
        ///     The full content of the file to import.
        /// </param>
        /// <param name="elevated">
        ///     true to import with highest user permissions; otherwise, false.
        /// </param>
        public static bool ImportFile(string[] contents, bool elevated = false)
        {
            try
            {
                return ImportFile(Path.Combine(Path.GetTempPath(), $"{PathEx.GetTempDirName()}.reg"), contents, elevated);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        /// <summary>
        ///     Imports the content of an INI file, created by <see cref="ExportToIniFile(RegKey, string, string)"/>,
        ///     to the registry.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to import.
        /// </param>
        public static bool ImportFromIniFile(string path)
        {
            try
            {
                if (Path.GetExtension(path).EqualsEx("ini"))
                    return ImportFile(path);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return false;
        }

        private static void ExportToIniFile(object key, string subKey, string destIniPath = null)
        {
            try
            {
                if (string.IsNullOrEmpty(destIniPath))
                    destIniPath = PathEx.Combine(PathEx.LocalDir, $"{Process.GetCurrentProcess().ProcessName}.ini");
                var path = Path.GetDirectoryName(destIniPath);
                if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (File.Exists(destIniPath))
                    File.Delete(destIniPath);
                File.Create(destIniPath).Close();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return;
            }
            var section = string.Empty;
            foreach (var ent in GetAllTreeValues(key, subKey))
            {
                if (ent.Value == ent.Key.EncryptToMd5())
                {
                    var sections = Ini.Read("Root", "Sections", destIniPath);
                    section = ent.Value;
                    Ini.Write("Root", "Sections", $"{(!string.IsNullOrEmpty(sections) ? $"{sections}," : string.Empty)}{section}", destIniPath);
                    Ini.Write(section, $"{section}_RootKey", key.AsRegistryKey(), destIniPath);
                    Ini.Write(section, $"{section}_SubKey", ent.Key, destIniPath);
                    continue;
                }
                if (string.IsNullOrWhiteSpace(section))
                    continue;
                var values = Ini.Read(section, $"{section}_Values", destIniPath);
                Ini.Write(section, $"{section}_Values", $"{(!string.IsNullOrEmpty(values) ? $"{values}," : string.Empty)}{ent.Key}", destIniPath);
                Ini.Write(section, ent.Key, ent.Value, destIniPath);
            }
        }

        /// <summary>
        ///     Exports the full content of the specified registry path into an INI file.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey to export.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to to export.
        /// </param>
        /// <param name="destIniPath">
        ///     The full path of the file to create or override.
        /// </param>
        public static void ExportToIniFile(RegKey key, string subKey, string destIniPath = null) =>
            ExportToIniFile(key as object, subKey, destIniPath);

        /// <summary>
        ///     Exports the full content of the specified registry path into an INI file.
        /// </summary>
        /// <param name="key">
        ///     The root key that contains the subkey to export.
        /// </param>
        /// <param name="subKey">
        ///     The path of the subkey to to export.
        /// </param>
        /// <param name="destIniPath">
        ///     The full path of the file to create or override.
        /// </param>
        public static void ExportToIniFile(string key, string subKey, string destIniPath = null) =>
            ExportToIniFile(key as object, subKey, destIniPath);

        /// <summary>
        ///     Exports the full content of the specified registry path into an INI file.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to export.
        /// </param>
        public static void ExportToIniFile(string keyPath) =>
            ExportToIniFile(keyPath.GetKey(), keyPath.GetSubKey());

        /// <summary>
        ///     Exports the full content of the specified registry path into an REG file.
        /// </summary>
        /// <param name="keyPath">
        ///     The full path of the key to export.
        /// </param>
        /// <param name="destFilePath">
        ///     The full path of the file to create or override.
        /// </param>
        /// <param name="elevated">
        ///     true to export with highest user permissions; otherwise, false.
        /// </param>
        public static void ExportFile(string keyPath, string destFilePath, bool elevated = false)
        {
            var dir = Path.GetDirectoryName(destFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            ProcessEx.Start("%system32%\\reg.exe", $"EXPORT \"{keyPath}\" \"{destFilePath}\" /y", elevated, ProcessWindowStyle.Hidden);
        }
    }
}
