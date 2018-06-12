#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Json.cs
// Version:  2018-06-12 23:23
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
        ///     Serializes the specified object graph into a string representation of an JSON
        ///     document.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source.
        /// </typeparam>
        /// <param name="source">
        ///     The object graph to serialize.
        /// </param>
        public static string Serialize<TSource>(TSource source)
        {
            try
            {
                if (source == null)
                    throw new ArgumentNullException(nameof(source));
                var js = new JavaScriptSerializer();
                var sb = new StringBuilder();
                js.Serialize(source, sb);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Creates a new JSON file, writes the specified object graph into to the JSON file,
        ///     and then closes the file.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source.
        /// </typeparam>
        /// <param name="path">
        ///     The JSON file to create.
        /// </param>
        /// <param name="source">
        ///     The object graph to write to the file.
        /// </param>
        /// <param name="overwrite">
        ///     true to allow an existing file to be overwritten; otherwise, false.
        /// </param>
        public static bool SerializeToFile<TSource>(string path, TSource source, bool overwrite = true)
        {
            try
            {
                if (path == null)
                    throw new ArgumentNullException(nameof(path));
                var output = Serialize(source);
                if (output == null)
                    throw new ArgumentNullException(nameof(output));
                var dest = PathEx.Combine(path);
                if (!overwrite && File.Exists(dest))
                    return false;
                File.WriteAllText(dest, output);
                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Deserializes a string representation of an JSON document into an object graph.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type of the result.
        /// </typeparam>
        /// <param name="source">
        ///     The string representation of an JSON document to deserialize.
        /// </param>
        /// <param name="defValue">
        ///     The default value.
        /// </param>
        public static TResult Deserialize<TResult>(string source, TResult defValue = default(TResult))
        {
            try
            {
                if (source == null)
                    throw new ArgumentNullException(nameof(source));
                var js = new JavaScriptSerializer();
                return js.Deserialize<TResult>(source);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return defValue;
            }
        }

        /// <summary>
        ///     Deserializes the specified JSON file into an object graph.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type of the result.
        /// </typeparam>
        /// <param name="path">
        ///     The JSON file to deserialize.
        /// </param>
        /// <param name="defValue">
        ///     The default value.
        /// </param>
        public static TResult DeserializeFile<TResult>(string path, TResult defValue = default(TResult))
        {
            try
            {
                if (path == null)
                    throw new ArgumentNullException(nameof(path));
                var src = PathEx.Combine(path);
                if (!File.Exists(src))
                    throw new PathNotFoundException(src);
                var input = File.ReadAllText(src);
                return Deserialize<TResult>(input);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return defValue;
            }
        }

        /// <summary>
        ///     Retrieves the full content of the specified JSON file.
        /// </summary>
        /// <param name="path">
        ///     The full path of the file to read.
        /// </param>
        public static Dictionary<string, object> ReadAll(string path) =>
            DeserializeFile<Dictionary<string, object>>(path);

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
    }
}
