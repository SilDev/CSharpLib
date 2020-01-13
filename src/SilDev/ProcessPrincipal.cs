#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ProcessPrincipal.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using Properties;

    /// <summary>
    ///     Provides the functionality to handle the current principal name.
    /// </summary>
    public static class ProcessPrincipal
    {
        /// <summary>
        ///     Gets the original name of the current principal.
        ///     <para>
        ///         This variable is only set if <see cref="GetOriginalName"/> was
        ///         previously called.
        ///     </para>
        /// </summary>
        public static string Name { get; private set; }

        private static void GetPointers(out IntPtr offset, out IntPtr buffer)
        {
            var curHandle = Process.GetCurrentProcess().Handle;
            var pebBaseAddress = WinApi.NativeHelper.GetProcessBasicInformation(curHandle).PebBaseAddress;
            var processParameters = Marshal.ReadIntPtr(pebBaseAddress, 4 * IntPtr.Size);
            var unicodeSize = IntPtr.Size * 2;
            offset = processParameters.Increment(new IntPtr(4 * 4 + 5 * IntPtr.Size + unicodeSize + IntPtr.Size + unicodeSize));
            buffer = Marshal.ReadIntPtr(offset, IntPtr.Size);
        }

        /// <summary>
        ///     Retrieves the original name of the current principal.
        /// </summary>
        public static string GetOriginalName()
        {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            try
            {
                GetPointers(out var offset, out var buffer);
                var len = Marshal.ReadInt16(offset);
                if (string.IsNullOrEmpty(Name))
                    Name = Marshal.PtrToStringUni(buffer, len / 2);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return Name;
        }

        /// <summary>
        ///     Changes the name of the current principal.
        /// </summary>
        /// <param name="newName">
        ///     The new name for the current principal, which cannot be longer than the
        ///     original one.
        /// </param>
        public static void ChangeName(string newName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newName))
                    throw new ArgumentNullException(nameof(newName));
                GetPointers(out var offset, out var buffer);
                var len = Marshal.ReadInt16(offset);
                if (string.IsNullOrEmpty(Name))
                    Name = Marshal.PtrToStringUni(buffer, len / 2);
                var principalDir = Path.GetDirectoryName(Name);
                if (string.IsNullOrEmpty(principalDir))
                    throw new PathNotFoundException(principalDir);
                var newPrincipalName = Path.Combine(principalDir, newName);
                if (newPrincipalName.Length > Name.Length)
                    throw new ArgumentException(ExceptionMessages.NewPrincipalNameTooLong);
                var ptr = buffer;
                foreach (var c in newPrincipalName)
                {
                    Marshal.WriteInt16(ptr, c);
                    ptr = ptr.Increment(new IntPtr(2));
                }
                Marshal.WriteInt16(ptr, 0);
                Marshal.WriteInt16(offset, (short)(newPrincipalName.Length * 2));
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        ///     Restores the name of the current principal.
        /// </summary>
        public static void RestoreName()
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                    throw new InvalidOperationException();
                GetPointers(out var offset, out var buffer);
                foreach (var c in Name)
                {
                    Marshal.WriteInt16(buffer, c);
                    buffer = buffer.Increment(new IntPtr(2));
                }
                Marshal.WriteInt16(buffer, 0);
                Marshal.WriteInt16(offset, (short)(Name.Length * 2));
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }
    }
}
