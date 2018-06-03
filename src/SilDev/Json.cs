#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Json.cs
// Version:  2018-06-03 08:33
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web.Script.Serialization;

    /// <summary>
    ///     Provides basic functionality for the JSON format.
    /// </summary>
    public static class Json
    {
        /// <summary>
        ///     Retrieves the full content of the specified JSON file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to read.
        /// </param>
        public static Dictionary<string, object> ReadAll(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var s = PathEx.Combine(path);
                if (!File.Exists(s))
                    throw new PathNotFoundException(s);
                s = File.ReadAllText(s);
                var js = new JavaScriptSerializer();
                var json = js.DeserializeObject(s);
                return (Dictionary<string, object>)json;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Retrieves a value from the specified key in a JSON file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to read.
        /// </param>
        /// <param name="keys">
        ///     An array of keys to navigate to the exact position of the value.
        /// </param>
        public static string Read(string path, params string[] keys)
        {
            try
            {
                dynamic json = ReadAll(path);
                keys.ForEach(k => json = json[k]);
                return json;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     Converts an object to a JSON string.
        /// </summary>
        /// <param name="obj">
        ///     The object to convert.
        /// </param>
        /// <param name="maxLength">
        ///     Gets or sets the maximum length of JSON strings that are accepted by the
        ///     <see cref="JavaScriptSerializer"/> class.
        /// </param>
        /// <param name="recursionLimit">
        ///     Gets or sets the limit for constraining the number of object levels to process.
        /// </param>
        public static string String(object obj, int maxLength = 0x200000, int recursionLimit = 0x64)
        {
            try
            {
                if (obj == default(object))
                    throw new ArgumentNullException(nameof(obj));
                if (maxLength < short.MaxValue)
                    throw new ArgumentOutOfRangeException(nameof(maxLength));
                if (recursionLimit < 0x10)
                    throw new ArgumentOutOfRangeException(nameof(recursionLimit));
                var sb = new StringBuilder();
                var js = new JavaScriptSerializer();
                if (maxLength > 0)
                    js.MaxJsonLength = maxLength;
                if (recursionLimit > 0)
                    js.RecursionLimit = recursionLimit;
                js.Serialize(obj, sb);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///     Creates a new JSON file with the specified object data.
        /// </summary>
        /// <param name="obj">
        ///     The object to convert.
        /// </param>
        /// <param name="path">
        ///     The full path of the file to read.
        /// </param>
        /// <param name="maxLength">
        ///     Gets or sets the maximum length of JSON strings that are accepted by the
        ///     <see cref="JavaScriptSerializer"/> class.
        /// </param>
        /// <param name="recursionLimit">
        ///     Gets or sets the limit for constraining the number of object levels to process.
        /// </param>
        public static void Write(object obj, string path, int maxLength = 0x200000, int recursionLimit = 0x64)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
                var s1 = PathEx.Combine(path);
                if (!PathEx.IsValidPath(s1))
                    throw new ArgumentException();
                var s2 = String(obj, maxLength, recursionLimit);
                if (string.IsNullOrEmpty(s2))
                    throw new ArgumentNullException(nameof(s2));
                File.WriteAllText(s1, s2);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
    }
}
