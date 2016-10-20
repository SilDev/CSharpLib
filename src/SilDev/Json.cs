#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Json.cs
// Version:  2016-10-18 23:33
// 
// Copyright (c) 2016, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Script.Serialization;

    /// <summary>
    ///     Provides basic functionality for the JSON format.
    /// </summary>
    public static class Json
    {
        /// <summary>
        ///     Creates a new JSON file with the specified object data.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to read (environment variables are accepted).
        /// </param>
        /// <param name="obj">
        ///     The object to convert.
        /// </param>
        public static string Create(string path, object obj)
        {
            try
            {
                var s = PathEx.Combine(path);
                if (string.IsNullOrEmpty(s))
                    throw new ArgumentNullException();
                var json = new JavaScriptSerializer().Serialize(obj);
                File.WriteAllText(s, json);
                return json;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Retrieves the full content of the specified JSON file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to read (environment variables are accepted).
        /// </param>
        public static Dictionary<string, object> ReadAll(string path)
        {
            try
            {
                var s = PathEx.Combine(path);
                s = File.ReadAllText(s);
                dynamic json = new JavaScriptSerializer().DeserializeObject(s);
                return json;
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
        ///     The full path of the file to read (environment variables are accepted).
        /// </param>
        /// <param name="keys">
        ///     An array of keys to navigate to the exact position of the value.
        /// </param>
        public static string Read(string path, params string[] keys)
        {
            try
            {
                var s = PathEx.Combine(path);
                s = File.ReadAllText(s);
                dynamic json = new JavaScriptSerializer().DeserializeObject(s);
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var k in keys)
                    json = json[k];
                return json;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }
    }
}
