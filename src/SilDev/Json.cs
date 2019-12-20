#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Json.cs
// Version:  2019-12-20 22:16
// 
// Copyright (c) 2019, Si13n7 Developments (r)
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
        private static JavaScriptSerializer _jsonSerializer;
        private static int _maxLength, _recursionLimit;

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
        ///     Gets or sets the limit for constraining the number of object levels to process.
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

        private static void WriteByte(this Stream stream, char value)
        {
            if (value <= byte.MaxValue)
            {
                stream?.WriteByte((byte)value);
                return;
            }
            var str = value.ToString(CultureConfig.GlobalCultureInfo);
            stream?.WriteBytes(str.ToBytes());
        }

        private static void WriteByte(this Stream stream, char value, int count)
        {
            if (stream == null || count < 1)
                return;
            for (var i = 0; i < count; i++)
                stream.WriteByte(value);
        }

        private static void FormatToStream(string inputString, Stream outputStream)
        {
            if (inputString == null)
                throw new ArgumentNullException(nameof(inputString));
            if (outputStream == null)
                throw new ArgumentNullException(nameof(outputStream));

            const int width = 3;
            var depth = 0;
            var isEscape = false;
            var isValue = false;
            var newLine = Environment.NewLine.ToBytes();

            var input = inputString.ToCharArray();
            var output = outputStream;
            foreach (var c in input)
            {
                if (isEscape)
                {
                    isEscape = false;
                    output.WriteByte(c);
                    continue;
                }
                switch (c)
                {
                    case '\\':
                        isEscape = true;
                        output.WriteByte(c);
                        continue;
                    case '"':
                        isValue = !isValue;
                        output.WriteByte(c);
                        continue;
                    default:
                        if (isValue)
                        {
                            output.WriteByte(c);
                            continue;
                        }
                        break;
                }
                switch (c)
                {
                    case ',':
                        output.WriteByte(c);
                        output.WriteBytes(newLine);
                        if (depth > 0)
                            output.WriteByte(' ', depth * width);
                        break;
                    case '[':
                    case '{':
                        output.WriteByte(c);
                        output.WriteBytes(newLine);
                        if (++depth > 0)
                            output.WriteByte(' ', depth * width);
                        break;
                    case ']':
                    case '}':
                        output.WriteBytes(newLine);
                        if (--depth > 0)
                            output.WriteByte(' ', depth * width);
                        output.WriteByte(c);
                        break;
                    case ':':
                        output.WriteByte(c);
                        output.WriteByte(' ');
                        break;
                    default:
                        if (!char.IsWhiteSpace(c))
                            output.WriteByte(c);
                        break;
                }
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
                FormatToStream(source, ms);
                ms.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(ms, TextEx.DefaultEncoding))
                {
                    ms = null;
                    return sr.ReadToEnd();
                }
            }
            finally
            {
                ms?.Dispose();
            }
        }

        /// <summary>
        ///     Serializes the specified object graph into a string representation of a JSON
        ///     document.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the source.
        /// </typeparam>
        /// <param name="source">
        ///     The object graph to serialize.
        /// </param>
        /// <param name="format">
        ///     true to format the string representation of the JSON document; otherwise, false.
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
                using (var fs = new FileStream(dest, overwrite ? FileMode.Create : FileMode.CreateNew))
                    FormatToStream(output, fs);
                return true;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }

        /// <summary>
        ///     Deserializes a string representation of a JSON document into an object graph.
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
    }
}
