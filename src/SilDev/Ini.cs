#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Ini.cs
// Version:  2016-10-20 12:09
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///     Provides static methods for the handling of initialization files.
    /// </summary>
    public static class Ini
    {
        private static string _iniFile;

        /// <summary>
        ///     Specifies an initialization file to use as the default file if no other file is
        ///     specified for a called <see cref="Ini"/> method.
        /// </summary>
        /// <param name="paths">
        ///     An array of parts of the path (environment variables are accepted).
        /// </param>
        public static bool File(params string[] paths)
        {
            try
            {
                _iniFile = PathEx.Combine(paths);
                if (System.IO.File.Exists(_iniFile))
                    return true;
                var iniDir = Path.GetDirectoryName(_iniFile);
                if (!Directory.Exists(iniDir))
                {
                    if (string.IsNullOrEmpty(iniDir))
                        return false;
                    Directory.CreateDirectory(iniDir);
                }
                System.IO.File.Create(_iniFile).Close();
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Gets the full path of the default initialization file.
        /// </summary>
        public static string File() =>
            _iniFile ?? string.Empty;

        /// <summary>
        ///     Removes the full specified section of an INI file or an INI file formatted
        ///     string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section to remove.
        /// </param>
        /// <param name="file">
        ///     The full file path of an INI file.
        /// </param>
        public static bool RemoveSection(string section, string file = null)
        {
            try
            {
                var path = !string.IsNullOrEmpty(file) ? PathEx.Combine(file) : _iniFile;
                if (!System.IO.File.Exists(path))
                    throw new FileNotFoundException();
                return WinApi.SafeNativeMethods.WritePrivateProfileSection(section, null, path) != 0;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Removes the specified key from the specified section, of an INI file or an INI
        ///     file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key to remove.
        /// </param>
        /// <param name="key">
        ///     The name of the key to remove.
        /// </param>
        /// <param name="file">
        ///     The full file path of an INI file.
        /// </param>
        public static bool RemoveKey(string section, string key, string file = null)
        {
            try
            {
                var path = !string.IsNullOrEmpty(file) ? PathEx.Combine(file) : _iniFile;
                if (!System.IO.File.Exists(path))
                    throw new FileNotFoundException();
                return WinApi.SafeNativeMethods.WritePrivateProfileString(section, key, null, path) != 0;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Retrieves all section names of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="sorted">
        ///     true to sort the sections; otherwise, false.
        /// </param>
        public static List<string> GetSections(string fileOrContent = null, bool sorted = true)
        {
            var output = new List<string>();
            try
            {
                var dest = fileOrContent ?? _iniFile;
                if (string.IsNullOrWhiteSpace(dest))
                    throw new ArgumentNullException();
                var path = PathEx.Combine(dest);
                if (System.IO.File.Exists(path))
                {
                    var buffer = new byte[short.MaxValue];
                    if (WinApi.SafeNativeMethods.GetPrivateProfileSectionNames(buffer, short.MaxValue, path) != 0)
                        output = Encoding.ASCII.GetString(buffer).Trim('\0').Split('\0').ToList();
                }
                else
                {
                    path = Path.GetTempFileName();
                    System.IO.File.WriteAllText(path, dest);
                    if (System.IO.File.Exists(path))
                    {
                        output = GetSections(path, false);
                        System.IO.File.Delete(path);
                    }
                }
                if (sorted)
                    output = output.OrderBy(x => x, new Comparison.AlphanumericComparer()).ToList();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
        }

        /// <summary>
        ///     Retrieves all section names of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="sorted">
        ///     true to sort the sections; otherwise, false.
        /// </param>
        public static List<string> GetSections(bool sorted) =>
            GetSections(_iniFile, sorted);

        /// <summary>
        ///     Retrieves all key names of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section to get the key names. The value must be NULL to get all the
        ///     key names of the specified fileOrContent parameter.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="sorted">
        ///     true to sort keys; otherwise, false.
        /// </param>
        public static List<string> GetKeys(string section, string fileOrContent = null, bool sorted = true)
        {
            var output = new List<string>();
            try
            {
                var dest = fileOrContent ?? _iniFile;
                var path = PathEx.Combine(dest);
                if (System.IO.File.Exists(dest))
                {
                    var tmp = new string(' ', short.MaxValue);
                    if (WinApi.SafeNativeMethods.GetPrivateProfileString(section, null, string.Empty, tmp, short.MaxValue, path) != 0)
                    {
                        output = new List<string>(tmp.Split('\0'));
                        output.RemoveRange(output.Count - 2, 2);
                    }
                }
                else
                {
                    path = Path.GetTempFileName();
                    System.IO.File.WriteAllText(path, dest);
                    if (System.IO.File.Exists(path))
                    {
                        output = GetKeys(section, path, false);
                        System.IO.File.Delete(path);
                    }
                }
                if (sorted)
                    output = output.OrderBy(x => x, new Comparison.AlphanumericComparer()).ToList();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
        }

        /// <summary>
        ///     Retrieves all key names of an INI file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section to get the key names. The value must be NULL to get all the
        ///     key names of the specified fileOrContent parameter.
        /// </param>
        /// <param name="sorted">
        ///     true to sort keys; otherwise, false.
        /// </param>
        public static List<string> GetKeys(string section, bool sorted) =>
            GetKeys(section, _iniFile, sorted);

        /// <summary>
        ///     <para>
        ///         Retrieves the full content of an INI file or an INI file formatted string value.
        ///         The result is saved in an <see cref="Dictionary{TKey,TValue}"/>.
        ///     </para>
        ///     <para>
        ///         Please note that values are stored as <see cref="string"/>.
        ///     </para>
        /// </summary>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        /// <param name="sorted">
        ///     true to sort the sections and keys; otherwise, false.
        /// </param>
        public static Dictionary<string, Dictionary<string, string>> ReadAll(string fileOrContent = null, bool sorted = true)
        {
            var output = new Dictionary<string, Dictionary<string, string>>();
            try
            {
                var isContent = false;
                var source = fileOrContent ?? _iniFile;
                var path = PathEx.Combine(source);
                if (!System.IO.File.Exists(path))
                {
                    isContent = true;
                    path = Path.GetTempFileName();
                    System.IO.File.WriteAllText(path, source);
                }
                if (!System.IO.File.Exists(path))
                    throw new FileNotFoundException();
                var sections = GetSections(path, sorted);
                if (sections.Count == 0)
                    throw new ArgumentNullException();
                foreach (var section in sections)
                {
                    var keys = GetKeys(section, path, sorted);
                    if (keys.Count == 0)
                        continue;
                    var values = new Dictionary<string, string>();
                    foreach (var key in keys)
                    {
                        var value = Read(section, key, path);
                        if (string.IsNullOrWhiteSpace(value))
                            continue;
                        values.Add(key, value);
                    }
                    if (values.Count == 0)
                        continue;
                    if (!output.ContainsKey(section))
                        output.Add(section, values);
                }
                if (isContent)
                    System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
        }

        /// <summary>
        ///     <para>
        ///         Retrieves the full content of an INI file or an INI file formatted string value.
        ///         The result is saved in an <see cref="Dictionary{TKey,TValue}"/>.
        ///     </para>
        ///     <para>
        ///         Please note that values are stored as <see cref="string"/>.
        ///     </para>
        /// </summary>
        /// <param name="sorted">
        ///     true to sort the sections and keys; otherwise, false.
        /// </param>
        public static Dictionary<string, Dictionary<string, string>> ReadAll(bool sorted) =>
            ReadAll(_iniFile, sorted);

        /// <summary>
        ///     Determines whether the specified section, of an INI file or an INI file formatted
        ///     string value, contains a key with a valid value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static bool ValueExists(string section, string key, string fileOrContent = null) =>
            !string.IsNullOrWhiteSpace(Read(section, key, fileOrContent ?? _iniFile));

        /// <summary>
        ///     Retrieves a <see cref="string"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static string Read(string section, string key, string fileOrContent = null)
        {
            var output = string.Empty;
            try
            {
                var source = fileOrContent ?? _iniFile;
                var path = PathEx.Combine(source);
                if (System.IO.File.Exists(source))
                {
                    var tmp = new StringBuilder(short.MaxValue);
                    if (WinApi.SafeNativeMethods.GetPrivateProfileString(section, key, string.Empty, tmp, tmp.Capacity, path) != 0)
                        output = tmp.ToString();
                }
                else
                {
                    path = Path.GetTempFileName();
                    System.IO.File.WriteAllText(path, source);
                    if (System.IO.File.Exists(path))
                    {
                        output = Read(section, key, path);
                        System.IO.File.Delete(path);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return output;
        }

        [SuppressMessage("ReSharper", "TryCastAlwaysSucceeds")]
        private static object ReadObject(string section, string key, object defValue, IniValueKind valkind, string fileOrContent)
        {
            object output = null;
            var value = Read(section, key, fileOrContent);
            switch (valkind)
            {
                case IniValueKind.Boolean:
                    bool boolParser;
                    if (bool.TryParse(Read(section, key, fileOrContent), out boolParser))
                        output = boolParser;
                    break;
                case IniValueKind.Byte:
                    byte byteParser;
                    if (byte.TryParse(Read(section, key, fileOrContent), out byteParser))
                        output = byteParser;
                    break;
                case IniValueKind.ByteArray:
                    var bytesParser = value.FromHexStringToByteArray();
                    if (bytesParser.Length > 0)
                        output = bytesParser;
                    break;
                case IniValueKind.DateTime:
                    DateTime dateTimeParser;
                    if (DateTime.TryParse(Read(section, key, fileOrContent), out dateTimeParser))
                        output = dateTimeParser;
                    break;
                case IniValueKind.Double:
                    double doubleParser;
                    if (double.TryParse(Read(section, key, fileOrContent), out doubleParser))
                        output = doubleParser;
                    break;
                case IniValueKind.Float:
                    float floatParser;
                    if (float.TryParse(Read(section, key, fileOrContent), out floatParser))
                        output = floatParser;
                    break;
                case IniValueKind.Image:
                    var imageParser = value.FromHexStringToImage();
                    if (imageParser != null)
                        output = imageParser;
                    break;
                case IniValueKind.Integer:
                    int intParser;
                    if (int.TryParse(Read(section, key, fileOrContent), out intParser))
                        output = intParser;
                    break;
                case IniValueKind.Long:
                    long longParser;
                    if (long.TryParse(Read(section, key, fileOrContent), out longParser))
                        output = longParser;
                    break;
                case IniValueKind.Point:
                    var pointParser = Read(section, key, fileOrContent).ToPoint();
                    if (pointParser != new Point(int.MinValue, int.MinValue))
                        output = pointParser;
                    break;
                case IniValueKind.Rectangle:
                    var rectParser = Read(section, key, fileOrContent).ToRectangle();
                    if (rectParser != Rectangle.Empty)
                        output = rectParser;
                    break;
                case IniValueKind.Short:
                    short shortParser;
                    if (short.TryParse(Read(section, key, fileOrContent), out shortParser))
                        output = shortParser;
                    break;
                case IniValueKind.Size:
                    var sizeParser = Read(section, key, fileOrContent).ToSize();
                    if (sizeParser != Size.Empty)
                        output = sizeParser;
                    break;
                case IniValueKind.StringArray:
                    var stringsParser = value.FromHexString().Split(new string(Enumerable.Range(0, 8).Select(i => (char)i).ToArray()).Reverse());
                    if (stringsParser.Length > 0)
                        output = stringsParser;
                    break;
                case IniValueKind.Version:
                    Version versionParser;
                    if (Version.TryParse(Read(section, key, fileOrContent), out versionParser))
                        output = versionParser;
                    break;
                default:
                    output = Read(section, key, fileOrContent);
                    if (string.IsNullOrWhiteSpace(output as string))
                        output = null;
                    break;
            }
            return output ?? defValue;
        }

        /// <summary>
        ///     Retrieves a <see cref="bool"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static bool ReadBoolean(string section, string key, bool defValue = false, string fileOrContent = null) =>
            Convert.ToBoolean(ReadObject(section, key, defValue, IniValueKind.Boolean, fileOrContent ?? _iniFile));

        /// <summary>
        ///     Retrieves a <see cref="bool"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated string is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static bool ReadBoolean(string section, string key, string fileOrContent) =>
            ReadBoolean(section, key, false, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="byte"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static byte ReadByte(string section, string key, byte defValue = 0x0, string fileOrContent = null) =>
            Convert.ToByte(ReadObject(section, key, defValue, IniValueKind.Byte, fileOrContent ?? _iniFile));

        /// <summary>
        ///     Retrieves a <see cref="byte"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static byte ReadByte(string section, string key, string fileOrContent) =>
            ReadByte(section, key, 0x0, fileOrContent);

        /// <summary>
        ///     <para>
        ///         Retrieves a sequence of bytes from the specified section in an INI file or
        ///         an INI file formatted string value.
        ///     </para>
        ///     <para>
        ///         Please note that the associated key value must be a hexadecimal string of a
        ///         sequence of bytes.
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static byte[] ReadByteArray(string section, string key, byte[] defValue = null, string fileOrContent = null) =>
            ReadObject(section, key, defValue, IniValueKind.ByteArray, fileOrContent ?? _iniFile) as byte[];

        /// <summary>
        ///     <para>
        ///         Retrieves a sequence of bytes from the specified section in an INI file or
        ///         an INI file formatted string value.
        ///     </para>
        ///     <para>
        ///         Please note that the associated key value must be a hexadecimal string of a
        ///         sequence of bytes.
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static byte[] ReadByteArray(string section, string key, string fileOrContent) =>
            ReadByteArray(section, key, null, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="DateTime"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static DateTime ReadDateTime(string section, string key, DateTime defValue, string fileOrContent = null) =>
            Convert.ToDateTime(ReadObject(section, key, defValue, IniValueKind.DateTime, fileOrContent ?? _iniFile));

        /// <summary>
        ///     Retrieves a <see cref="DateTime"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static DateTime ReadDateTime(string section, string key, string fileOrContent) =>
            ReadDateTime(section, key, DateTime.Now, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="DateTime"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        public static DateTime ReadDateTime(string section, string key) =>
            ReadDateTime(section, key, DateTime.Now, _iniFile);

        /// <summary>
        ///     Retrieves a <see cref="double"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static double ReadDouble(string section, string key, double defValue = 0d, string fileOrContent = null) =>
            Convert.ToDouble(ReadObject(section, key, defValue, IniValueKind.Double, fileOrContent ?? _iniFile));

        /// <summary>
        ///     Retrieves a <see cref="double"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static double ReadDouble(string section, string key, string fileOrContent) =>
            ReadDouble(section, key, 0d, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="float"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static float ReadFloat(string section, string key, float defValue = 0f, string fileOrContent = null) =>
            Convert.ToSingle(ReadObject(section, key, defValue, IniValueKind.Float, fileOrContent ?? _iniFile));

        /// <summary>
        ///     Retrieves a <see cref="float"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static float ReadFloat(string section, string key, string fileOrContent) =>
            ReadFloat(section, key, 0f, fileOrContent);

        /// <summary>
        ///     <para>
        ///         Retrieves an <see cref="Image"/> from the specified section in an INI file
        ///         or an INI file formatted string value.
        ///     </para>
        ///     <para>
        ///         Please note that the associated key value must be a hexadecimal string of a
        ///         sequence of bytes which represent a valid picture.
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static Image ReadImage(string section, string key, Image defValue = null, string fileOrContent = null) =>
            ReadObject(section, key, null, IniValueKind.Image, fileOrContent ?? _iniFile) as Image;

        /// <summary>
        ///     <para>
        ///         Retrieves an <see cref="Image"/> from the specified section in an INI file
        ///         or an INI file formatted string value.
        ///     </para>
        ///     <para>
        ///         Please note that the associated key value must be a hexadecimal string of a
        ///         sequence of bytes which represent a valid picture.
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static Image ReadImage(string section, string key, string fileOrContent) =>
            ReadImage(section, key, null, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="int"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static int ReadInteger(string section, string key, int defValue = 0, string fileOrContent = null) =>
            Convert.ToInt32(ReadObject(section, key, defValue, IniValueKind.Integer, fileOrContent ?? _iniFile));

        /// <summary>
        ///     Retrieves a <see cref="int"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static int ReadInteger(string section, string key, string fileOrContent) =>
            ReadInteger(section, key, 0, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="long"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static long ReadLong(string section, string key, long defValue = 0, string fileOrContent = null) =>
            Convert.ToInt64(ReadObject(section, key, defValue, IniValueKind.Long, fileOrContent ?? _iniFile));

        /// <summary>
        ///     Retrieves a <see cref="long"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static long ReadLong(string section, string key, string fileOrContent) =>
            ReadLong(section, key, 0, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="Point"/> value pair from the specified section in an INI
        ///     file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static Point ReadPoint(string section, string key, Point defValue, string fileOrContent = null) =>
            ReadObject(section, key, defValue, IniValueKind.Point, fileOrContent ?? _iniFile).ToString().ToPoint();

        /// <summary>
        ///     Retrieves a <see cref="Point"/> value pair from the specified section in an INI
        ///     file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static Point ReadPoint(string section, string key, string fileOrContent) =>
            ReadPoint(section, key, Point.Empty, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="Point"/> value pair from the specified section in an INI
        ///     file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        public static Point ReadPoint(string section, string key) =>
            ReadPoint(section, key, Point.Empty, _iniFile);

        /// <summary>
        ///     Retrieves a <see cref="Rectangle"/> from the specified section in an INI file or
        ///     an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static Rectangle ReadRectangle(string section, string key, Rectangle defValue, string fileOrContent = null) =>
            ReadObject(section, key, defValue, IniValueKind.Rectangle, fileOrContent ?? _iniFile).ToString().ToRectangle();

        /// <summary>
        ///     Retrieves a <see cref="Rectangle"/> from the specified section in an INI file or
        ///     an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static Rectangle ReadRectangle(string section, string key, string fileOrContent) =>
            ReadRectangle(section, key, Rectangle.Empty, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="Rectangle"/> from the specified section in an INI file or
        ///     an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        public static Rectangle ReadRectangle(string section, string key) =>
            ReadRectangle(section, key, Rectangle.Empty, _iniFile);

        /// <summary>
        ///     Retrieves a <see cref="short"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static short ReadShort(string section, string key, short defValue = 0, string fileOrContent = null) =>
            Convert.ToInt16(ReadObject(section, key, defValue, IniValueKind.Short, fileOrContent ?? _iniFile));

        /// <summary>
        ///     Retrieves a <see cref="short"/> value from the specified section in an INI file
        ///     or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static short ReadShort(string section, string key, string fileOrContent) =>
            ReadShort(section, key, 0, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="Size"/> value pair from the specified section in an INI
        ///     file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static Size ReadSize(string section, string key, Size defValue, string fileOrContent = null) =>
            ReadObject(section, key, defValue, IniValueKind.Size, fileOrContent ?? _iniFile).ToString().ToSize();

        /// <summary>
        ///     Retrieves a <see cref="Size"/> value pair from the specified section in an INI
        ///     file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static Size ReadSize(string section, string key, string fileOrContent) =>
            ReadSize(section, key, Size.Empty, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="Size"/> value pair from the specified section in an INI
        ///     file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        public static Size ReadSize(string section, string key) =>
            ReadSize(section, key, Size.Empty, _iniFile);

        /// <summary>
        ///     Retrieves a <see cref="string"/> value from the specified section in an INI
        ///     file or an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static string ReadString(string section, string key, string defValue = "", string fileOrContent = null) =>
            Convert.ToString(ReadObject(section, key, defValue, IniValueKind.String, fileOrContent));

        /// <summary>
        ///     <para>
        ///         Retrieves an <see cref="string"/> array from the specified section in an INI
        ///         file or an INI file formatted string value.
        ///     </para>
        ///     <para>
        ///         Please note that the associated key value must be a hexadecimal string of an
        ///         <see cref="string"/> array.
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static string[] ReadStringArray(string section, string key, string[] defValue = null, string fileOrContent = null) =>
            ReadObject(section, key, defValue, IniValueKind.StringArray, fileOrContent ?? _iniFile) as string[];

        /// <summary>
        ///     <para>
        ///         Retrieves an <see cref="string"/> array from the specified section in an INI
        ///         file or an INI file formatted string value.
        ///     </para>
        ///     <para>
        ///         Please note that the associated key value must be a hexadecimal string of an
        ///         <see cref="string"/> array.
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static string[] ReadStringArray(string section, string key, string fileOrContent) =>
            ReadStringArray(section, key, null, fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="Version"/> from the specified section in an INI file or
        ///     an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="defValue">
        ///     The default value which returns if there is no valid value to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static Version ReadVersion(string section, string key, Version defValue, string fileOrContent = null) =>
            Version.Parse(ReadObject(section, key, defValue, IniValueKind.Version, fileOrContent ?? _iniFile).ToString());

        /// <summary>
        ///     Retrieves a <see cref="Version"/> from the specified section in an INI file or
        ///     an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        /// <param name="fileOrContent">
        ///     The full file path or content of an INI file.
        /// </param>
        public static Version ReadVersion(string section, string key, string fileOrContent) =>
            ReadVersion(section, key, Version.Parse("0.0.0.0"), fileOrContent);

        /// <summary>
        ///     Retrieves a <see cref="Version"/> from the specified section in an INI file or
        ///     an INI file formatted string value.
        /// </summary>
        /// <param name="section">
        ///     The name of the section containing the key name. The value must be NULL for a
        ///     non-section key.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        public static Version ReadVersion(string section, string key) =>
            ReadVersion(section, key, Version.Parse("0.0.0.0"), _iniFile);

        /// <summary>
        ///     <para>
        ///         Copies an <see cref="object"/> into the specified section of an initialization
        ///         file. The following objects are valid:
        ///     </para>
        ///     <para>
        ///         <see cref="bool"/>,
        ///         <see cref="byte"/>,
        ///         <see cref="byte"/>[],
        ///         <see cref="DateTime"/>,
        ///         <see cref="double"/>,
        ///         <see cref="float"/>,
        ///         <see cref="Image"/>,
        ///         <see cref="int"/>,
        ///         <see cref="long"/>,
        ///         <see cref="Point"/>,
        ///         <see cref="Rectangle"/>,
        ///         <see cref="short"/>,
        ///         <see cref="Size"/>,
        ///         <see cref="string"/>,
        ///         <see cref="string"/>[], and
        ///         <see cref="Version"/>
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section to which the string will be copied. If the section does
        ///     not exist, it is created. The name of the section is case-independent; the
        ///     string can be any combination of uppercase and lowercase letters.
        /// </param>
        /// <param name="key">
        ///     The name of the key to be associated with a string. If the key does not exist in
        ///     the specified section, it is created. If this parameter is NULL, the entire
        ///     section, including all entries within the section, is deleted.
        /// </param>
        /// <param name="value">
        ///     The <see cref="object"/> to be written to the file. If this parameter is NULL,
        ///     the key pointed to by the key parameter is deleted.
        /// </param>
        /// <param name="file">
        ///     <para>
        ///         The name of the initialization file.
        ///     </para>
        ///     <para>
        ///         If the file was created using Unicode characters, the function writes Unicode
        ///         characters to the file. Otherwise, the function writes ANSI characters.
        ///     </para>
        /// </param>
        /// <param name="forceOverwrite">
        ///     true to enable overwriting of a key with the same value as specified; otherwise,
        ///     false.
        /// </param>
        /// <param name="skipExistValue">
        ///     true to skip a existing value, even it is not the same value as specified;
        ///     otherwise, false.
        /// </param>
        [SuppressMessage("ReSharper", "CanBeReplacedWithTryCastAndCheckForNull")]
        public static bool Write(string section, string key, object value, string file = null, bool forceOverwrite = true, bool skipExistValue = false)
        {
            try
            {
                var path = !string.IsNullOrEmpty(file) ? PathEx.Combine(file) : _iniFile;
                if (!System.IO.File.Exists(path))
                    throw new FileNotFoundException();
                if (value == null)
                    return RemoveKey(section, key, path);
                if (value is Image)
                {
                    var img = (Image)value;
                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, ImageFormat.Png);
                        value = ms.ToArray();
                    }
                }
                var newValue = value.ToString();
                if (value is byte[])
                    newValue = ((byte[])value).ToHexString();
                if (value is string[])
                {
                    var separator = new string(Enumerable.Range(0, 8).Select(i => (char)i).ToArray()).Reverse();
                    newValue = ((string[])value).Join(separator);
                    if (!newValue.Contains(separator))
                        newValue += separator;
                    newValue = newValue.ToHexString();
                }
                if (forceOverwrite && !skipExistValue)
                    return WinApi.SafeNativeMethods.WritePrivateProfileString(section, key, newValue, path) != 0;
                var curValue = Read(section, key, path);
                if (!forceOverwrite && curValue == newValue || skipExistValue && !string.IsNullOrWhiteSpace(curValue))
                    return false;
                return WinApi.SafeNativeMethods.WritePrivateProfileString(section, key, newValue, path) != 0;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     <para>
        ///         Copies an <see cref="object"/> into the specified section of an initialization
        ///         file. The following objects are valid:
        ///     </para>
        ///     <para>
        ///         <see cref="bool"/>,
        ///         <see cref="byte"/>,
        ///         <see cref="byte"/>[],
        ///         <see cref="DateTime"/>,
        ///         <see cref="double"/>,
        ///         <see cref="float"/>,
        ///         <see cref="Image"/>,
        ///         <see cref="int"/>,
        ///         <see cref="long"/>,
        ///         <see cref="Point"/>,
        ///         <see cref="Rectangle"/>,
        ///         <see cref="short"/>,
        ///         <see cref="Size"/>,
        ///         <see cref="string"/>,
        ///         <see cref="string"/>[], and
        ///         <see cref="Version"/>
        ///     </para>
        /// </summary>
        /// <param name="section">
        ///     The name of the section to which the string will be copied. If the section does
        ///     not exist, it is created. The name of the section is case-independent; the
        ///     string can be any combination of uppercase and lowercase letters.
        /// </param>
        /// <param name="key">
        ///     The name of the key to be associated with a string. If the key does not exist in
        ///     the specified section, it is created. If this parameter is NULL, the entire
        ///     section, including all entries within the section, is deleted.
        /// </param>
        /// <param name="value">
        ///     The <see cref="object"/> to be written to the file. If this parameter is NULL,
        ///     the key pointed to by the key parameter is deleted.
        /// </param>
        /// <param name="forceOverwrite">
        ///     true to enable overwriting of a key with the same value as specified; otherwise,
        ///     false.
        /// </param>
        /// <param name="skipExistValue">
        ///     true to skip a existing value, even it is not the same value as specified;
        ///     otherwise, false.
        /// </param>
        public static bool Write(string section, string key, object value, bool forceOverwrite, bool skipExistValue = false) =>
            Write(section, key, value, _iniFile, forceOverwrite, skipExistValue);

        private enum IniValueKind
        {
            Boolean,
            Byte,
            ByteArray,
            DateTime,
            Double,
            Float,
            Image,
            Integer,
            Long,
            Point,
            Rectangle,
            Short,
            Size,
            String,
            StringArray,
            Version
        }
    }
}
