#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IniDirect.cs
// Version:  2023-12-05 13:51
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Ini
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using static WinApi;

    /// <summary>
    ///     Provides static functions for accessing INI files using the Win32 API.
    /// </summary>
    public static class IniDirect
    {
        /// <summary>
        ///     Retrieves the value from the specified section of an INI file.
        ///     <para>
        ///         The Win32-API without file caching is used for reading in this case.
        ///         Please note that empty sections are not permitted.
        ///     </para>
        /// </summary>
        /// <param name="file">
        ///     The file path of the INI file to read.
        /// </param>
        /// <param name="section">
        ///     The name of the section containing the key name.
        /// </param>
        /// <param name="key">
        ///     The name of the key whose associated value is to be retrieved.
        /// </param>
        public static string Read(string file, string section, string key)
        {
            var output = string.Empty;
            try
            {
                var path = PathEx.Combine(file);
                if (!File.Exists(path))
                    throw new PathNotFoundException(path);
                var sb = new StringBuilder(short.MaxValue);
                if (NativeMethods.GetPrivateProfileString(section, key, string.Empty, sb, sb.Capacity, path) != 0)
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
        ///         directly on the disk, which means that the entire file is rewritten
        ///         every time. This causes many write accesses when used incorrectly.
        ///     </para>
        /// </summary>
        /// <param name="file">
        ///     The path of the INI file to write.
        /// </param>
        /// <param name="section">
        ///     The name of the section to which the value will be copied.
        /// </param>
        /// <param name="key">
        ///     The name of the key to be associated with a value.
        ///     <para>
        ///         If this parameter is <see langword="null"/>, the entire section,
        ///         including all entries within the section, is deleted.
        ///     </para>
        /// </param>
        /// <param name="value">
        ///     The value to be written to the file.
        ///     <para>
        ///         If this parameter is <see langword="null"/>, the key pointed to by the
        ///         key parameter is deleted.
        ///     </para>
        /// </param>
        /// <param name="forceOverwrite">
        ///     <see langword="true"/> to enable overwriting of a key with the same value
        ///     as specified; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="skipExistValue">
        ///     <see langword="true"/> to skip an existing value, even it is not the same
        ///     value as specified; otherwise, <see langword="false"/>.
        /// </param>
        public static bool Write(string file, string section, string key, object value, bool forceOverwrite = true, bool skipExistValue = false)
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
                    var curValue = Read(section, key, path);
                    if ((!forceOverwrite && curValue.Equals(strValue, StringComparison.Ordinal)) || (skipExistValue && !string.IsNullOrWhiteSpace(curValue)))
                        return false;
                }
                if (string.Concat(section, key, value).All(TextEx.IsAscii))
                    goto Write;
                var encoding = EncodingEx.GetEncoding(path);
                if (!encoding.Equals(Encoding.Unicode) && !encoding.Equals(Encoding.BigEndianUnicode))
                    EncodingEx.ChangeEncoding(path, Encoding.Unicode);
                Write:
                return NativeMethods.WritePrivateProfileString(section, key, strValue, path) != 0;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }
    }
}
