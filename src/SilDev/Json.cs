#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Json.cs
// Version:  2020-02-02 11:33
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;

    /// <summary>
    ///     Provides basic functionality for the JSON format.
    /// </summary>
    public static class Json
    {
        private static JavaScriptSerializer _jsonSerializer;
        private static int _maxLength, _recursionLimit;

        /// <summary>
        ///     Gets or sets the maximum length of JSON strings that are accepted by the
        ///     <see cref="JavaScriptSerializer"/> object.
        /// </summary>
        public static int MaxLength
        {
            get
            {
                if (_maxLength > 0)
                    return _maxLength;
                _maxLength = 0x4000000;
                return _maxLength;
            }
            set
            {
                _maxLength = value == 0 ? int.MaxValue : value;
                _maxLength = default;
            }
        }

        /// <summary>
        ///     Gets or sets the limit for constraining the number of object levels to
        ///     process.
        /// </summary>
        public static int RecursionLimit
        {
            get
            {
                if (_recursionLimit > 0)
                    return _recursionLimit;
                _recursionLimit = 0x4000;
                return _recursionLimit;
            }
            set
            {
                _recursionLimit = value == 0 ? int.MaxValue : value;
                _jsonSerializer = default;
            }
        }

        private static JavaScriptSerializer JsonSerializer
        {
            get
            {
                if (_jsonSerializer != default)
                    return _jsonSerializer;
                _jsonSerializer = new JavaScriptSerializer
                {
                    MaxJsonLength = MaxLength,
                    RecursionLimit = RecursionLimit
                };
                return _jsonSerializer;
            }
        }

        /// <summary>
        ///     Formats the specified string representation of a JSON document.
        /// </summary>
        /// <param name="source">
        ///     The string representation of an JSON document to format.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     source is null.
        /// </exception>
        public static string Format(string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            var ms = new MemoryStream();
            try
            {
                Format(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                using var sr = new StreamReader(ms, EncodingEx.Utf8NoBom);
                ms = null;
                return sr.ReadToEnd();
            }
            finally
            {
                ms?.Dispose();
            }
        }

        /// <summary>
        ///     Formats the specified JSON file and overwrites it if necessary.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     path is null.
        /// </exception>
        /// <exception cref="IOException">
        ///     path is invalid.
        /// </exception>
        public static bool FormatFile(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            var srcFile = PathEx.Combine(path);
            if (!PathEx.IsValidPath(srcFile))
                throw new IOException();
            var srcDir = Path.GetDirectoryName(srcFile);
            var newFile = PathEx.GetUniquePath(srcDir, "tmp", ".json");
            using (var sr = new StreamReader(srcFile))
            {
                using var fs = new FileStream(newFile, FileMode.Create);
                int count;
                var ca = new char[4096];
                var depth = 0;
                var isEscape = false;
                var isValue = false;
                while ((count = sr.Read(ca, 0, ca.Length)) > 0)
                    Format(fs, ca, count, ' ', ref depth, ref isEscape, ref isValue);
            }
            if (!FileEx.ContentIsEqual(srcFile, newFile))
                return FileEx.Move(newFile, srcFile, true);
            FileEx.TryDelete(newFile);
            return false;
        }

        /// <summary>
        ///     Serializes the specified object graph into a string representation of a
        ///     JSON document.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source.
        /// </typeparam>
        /// <param name="source">
        ///     The object graph to serialize.
        /// </param>
        /// <param name="format">
        ///     <see langword="true"/> to format the string representation of the JSON
        ///     document; otherwise, <see langword="false"/>.
        /// </param>
        public static string Serialize<TSource>(TSource source, bool format = false)
        {
            try
            {
                if (source == null)
                    throw new ArgumentNullException(nameof(source));
                var sb = new StringBuilder();
                JsonSerializer.Serialize(source, sb);
                var s = sb.ToStringThenClear();
                return format ? Format(s) : s;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Creates a new JSON file, writes the specified object graph into to the JSON
        ///     file, and then closes the file.
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
        ///     <see langword="true"/> to allow an existing file to be overwritten;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="formatted">
        ///     <see langword="true"/> to save the JSON document formatted; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static bool SerializeToFile<TSource>(string path, TSource source, bool overwrite = true, bool formatted = true)
        {
            try
            {
                if (path == null)
                    throw new ArgumentNullException(nameof(path));
                var output = Serialize(source);
                if (output == null)
                    throw new NullReferenceException();
                var dest = PathEx.Combine(path);
                using (var fs = new FileStream(dest, overwrite ? FileMode.Create : FileMode.CreateNew))
                    if (!formatted)
                        fs.WriteBytes(output);
                    else
                        Format(fs, output);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Deserializes a string representation of a JSON document into an object
        ///     graph.
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
        public static TResult Deserialize<TResult>(string source, TResult defValue = default)
        {
            try
            {
                if (source == null)
                    throw new ArgumentNullException(nameof(source));
                return JsonSerializer.Deserialize<TResult>(source);
            }
            catch (Exception ex) when (ex.IsCaught())
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
        public static TResult DeserializeFile<TResult>(string path, TResult defValue = default)
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
            catch (Exception ex) when (ex.IsCaught())
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
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return string.Empty;
            }
        }

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        private static void Format(Stream stream, char[] buffer, int count, char spacer, ref int depth, ref bool isEscape, ref bool isValue)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (count < 0 || count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count));
            int width;
            switch (spacer)
            {
                case '\t':
                    width = 1;
                    break;
                case ' ':
                    width = 3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spacer));
            }
            for (var i = 0; i < count; i++)
            {
                var c = buffer[i];
                if (isEscape)
                {
                    isEscape = false;
                    stream.WriteByte(c);
                    continue;
                }
                switch (c)
                {
                    case '\\':
                        isEscape = true;
                        stream.WriteByte(c);
                        continue;
                    case '"':
                        isValue = !isValue;
                        stream.WriteByte(c);
                        continue;
                    default:
                        if (isValue)
                        {
                            stream.WriteByte(c);
                            continue;
                        }
                        break;
                }
                switch (c)
                {
                    case ',':
                        stream.WriteByte(c);
                        stream.WriteBytes(Environment.NewLine);
                        if (depth > 0)
                            stream.WriteByte(spacer, depth * width);
                        break;
                    case '[':
                    case '{':
                    {
                        stream.WriteByte(c);
                        var ni = i + 1;
                        var hasValue = ni >= count;
                        if (!hasValue)
                        {
                            var nc = buffer[ni];
                            hasValue = nc != ']' && nc != '}';
                        }
                        if (hasValue)
                            stream.WriteBytes(Environment.NewLine);
                        if (++depth > 0 && hasValue)
                            stream.WriteByte(spacer, depth * width);
                        break;
                    }
                    case ']':
                    case '}':
                    {
                        var pi = i - 1;
                        var hasValue = pi < 0;
                        if (!hasValue)
                        {
                            var pc = buffer[pi];
                            hasValue = pc != '[' && pc != '{';
                        }
                        if (hasValue)
                            stream.WriteBytes(Environment.NewLine);
                        if (--depth > 0 && hasValue)
                            stream.WriteByte(spacer, depth * width);
                        stream.WriteByte(c);
                        break;
                    }
                    case ':':
                        stream.WriteByte(c);
                        stream.WriteByte(' ');
                        break;
                    default:
                        if (!char.IsWhiteSpace(c))
                            stream.WriteByte(c);
                        break;
                }
            }
        }

        private static void Format(Stream stream, IEnumerable<char> source)
        {
            if (stream == null || source == null)
                return;
            var depth = 0;
            var isEscape = false;
            var isValue = false;
            var buffer = source.ToArray();
            Format(stream, buffer, buffer.Length, ' ', ref depth, ref isEscape, ref isValue);
        }
    }
}
