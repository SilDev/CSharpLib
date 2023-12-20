#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: StreamEx.cs
// Version:  2023-12-20 12:04
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.IO;

    public static class StreamEx
    {
        /// <summary>
        ///     Reads the bytes from the specified stream and writes them to another
        ///     stream.
        /// </summary>
        /// <param name="srcStream">
        ///     The <see cref="Stream"/> to copy.
        /// </param>
        /// <param name="destStream">
        ///     The <see cref="Stream"/> to override.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stream or bytes is null.
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O error occurred, such as the specified file cannot be found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The stream does not support writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     stream was closed while the bytes were being written.
        /// </exception>
        public static void CopyTo(this Stream srcStream, Stream destStream)
        {
            if (srcStream == null)
                throw new ArgumentNullException(nameof(srcStream));
            if (destStream == null)
                throw new ArgumentNullException(nameof(destStream));
            int i;
            var ba = new byte[4096];
            while ((i = srcStream.Read(ba, 0, ba.Length)) > 0)
                destStream.Write(ba, 0, i);
        }

        /// <summary>
        ///     Writes a character to the this stream and advances the current position
        ///     within this stream by the number of bytes written.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="Stream"/> to write.
        /// </param>
        /// <param name="chr">
        ///     The character to write.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stream is null.
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O error occurred, such as the specified file cannot be found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The stream does not support writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     stream was closed while the bytes were being written.
        /// </exception>
        public static void WriteByte(this Stream stream, char chr)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (chr <= byte.MaxValue)
            {
                stream.WriteByte((byte)chr);
                return;
            }
            var str = chr.ToStringDefault();
            stream.WriteBytes(str.ToBytesUtf8());
        }

        /// <summary>
        ///     Writes a character repeated a specified number of times to the this stream
        ///     and advances the current position within this stream by the number of bytes
        ///     written.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="Stream"/> to write.
        /// </param>
        /// <param name="chr">
        ///     The character to write.
        /// </param>
        /// <param name="count">
        ///     The number of times chr occurs.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stream is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     count is less than zero.
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O error occurred, such as the specified file cannot be found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The stream does not support writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     stream was closed while the bytes were being written.
        /// </exception>
        public static void WriteByte(this Stream stream, char chr, int count)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0)
                return;
            for (var i = 0; i < count; i++)
                stream.WriteByte(chr);
        }

        /// <summary>
        ///     Writes a string to the this stream and advances the current position within
        ///     this stream by the number of bytes written.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="Stream"/> to write.
        /// </param>
        /// <param name="str">
        ///     The string to write.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stream or str is null.
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O error occurred, such as the specified file cannot be found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The stream does not support writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     stream was closed while the bytes were being written.
        /// </exception>
        public static void WriteBytes(this Stream stream, string str)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (string.IsNullOrEmpty(str))
                return;
            foreach (var c in str.ToCharArray())
                stream.WriteByte(c);
        }

        /// <summary>
        ///     Writes a sequence of bytes to the this stream and advances the current
        ///     position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="stream">
        ///     The <see cref="Stream"/> to write.
        /// </param>
        /// <param name="buffer">
        ///     An array of bytes to write.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     stream or bytes is null.
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O error occurred, such as the specified file cannot be found.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The stream does not support writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        ///     stream was closed while the bytes were being written.
        /// </exception>
        public static void WriteBytes(this Stream stream, byte[] buffer)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            var ba = buffer;
            stream.Write(ba, 0, ba.Length);
        }
    }
}
