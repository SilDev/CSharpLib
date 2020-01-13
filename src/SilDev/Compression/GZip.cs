#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: GZip.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Compression
{
    using System;
    using System.IO;
    using System.IO.Compression;

    public static class GZip
    {
        internal static byte[] Header =
        {
            0x1f,
            0x8b,
            0x08
        };

        /// <summary>
        ///     Compresses the specified input stream to the specified output stream.
        /// </summary>
        /// <param name="inputStream">
        ///     The input stream.
        /// </param>
        /// <param name="outputStream">
        ///     The output stream.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to dispose the the input stream; otherwise,
        ///     <see langword="false"/>;
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     inputStream or outputStream is null.
        /// </exception>
        public static void Compress(Stream inputStream, Stream outputStream, bool dispose)
        {
            if (inputStream == null)
                throw new ArgumentNullException(nameof(inputStream));
            if (outputStream == null)
                throw new ArgumentNullException(nameof(outputStream));
            var rs = inputStream;
            var ws = outputStream;
            try
            {
                using var gs = new GZipStream(ws, CompressionMode.Compress, true);
                rs.CopyTo(gs);
            }
            finally
            {
                if (dispose)
                    rs.Dispose();
            }
        }

        /// <summary>
        ///     Compresses the specified sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to compress.
        /// </param>
        public static byte[] Compress(byte[] bytes)
        {
            if (bytes == null)
                return null;
            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            Compress(msi, mso, false);
            return mso.ToArray();
        }

        /// <summary>
        ///     Compresses the specified <see cref="string"/> value.
        /// </summary>
        /// <param name="text">
        ///     The string to compress.
        /// </param>
        public static byte[] CompressText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            var ba = TextEx.DefaultEncoding.GetBytes(text);
            return Compress(ba);
        }

        /// <summary>
        ///     Decompresses the specified input stream to the specified output stream.
        /// </summary>
        /// <param name="inputStream">
        ///     The input stream.
        /// </param>
        /// <param name="outputStream">
        ///     The output stream.
        /// </param>
        /// <param name="dispose">
        ///     <see langword="true"/> to dispose the the input stream; otherwise,
        ///     <see langword="false"/>;
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     inputStream or outputStream is null.
        /// </exception>
        public static void Decompress(Stream inputStream, Stream outputStream, bool dispose)
        {
            if (inputStream == null)
                throw new ArgumentNullException(nameof(inputStream));
            if (outputStream == null)
                throw new ArgumentNullException(nameof(outputStream));
            var rs = inputStream;
            var ws = outputStream;
            try
            {
                using var gs = new GZipStream(rs, CompressionMode.Decompress, true);
                gs.CopyTo(ws);
            }
            finally
            {
                if (dispose)
                    rs.Dispose();
            }
        }

        /// <summary>
        ///     Decompresses a compressed sequence of bytes.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to decompress.
        /// </param>
        public static byte[] Decompress(byte[] bytes)
        {
            if (bytes == null)
                return null;
            try
            {
                using var msi = new MemoryStream(bytes);
                using var mso = new MemoryStream();
                Decompress(msi, mso, false);
                return mso.ToArray();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Decompresses a compressed sequence of bytes back to a <see cref="string"/>
        ///     value.
        /// </summary>
        /// <param name="bytes">
        ///     The sequence of bytes to decompress.
        /// </param>
        public static string DecompressText(byte[] bytes)
        {
            var ba = Decompress(bytes);
            return ba == null ? null : TextEx.DefaultEncoding.GetString(ba);
        }
    }
}
