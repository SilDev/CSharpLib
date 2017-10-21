#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Memory.cs
// Version:  2017-10-21 14:37
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Text;

    /// <inheritdoc/>
    /// <summary>
    ///     Provides a way to pin a managed object from unmanaged memory.
    /// </summary>
    public class MemoryPinner : IDisposable
    {
        private bool _disposed;
        private GCHandle _handle;

        /// <summary>
        ///     Initilazies a new instance of the <see cref="MemoryPinner"/> class with
        ///     the specified object to pin.
        /// </summary>
        /// <param name="value">
        ///     The object to pin.
        /// </param>
        public MemoryPinner(object value)
        {
            _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            Pointer = _handle.AddrOfPinnedObject();
        }

        /// <summary>
        ///     Returns the pointer to the pinned object.
        /// </summary>
        public IntPtr Pointer { get; private set; }

        /// <inheritdoc/>
        /// <summary>
        ///     Releases all resources used by this <see cref="MemoryPinner"/>.
        /// </summary>
        public void Dispose() =>
            Dispose(true);

        /// <summary>
        ///     Releases all resources used by this <see cref="MemoryPinner"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _handle.Free();
            Pointer = IntPtr.Zero;
            _disposed = true;
            if (!disposing)
                return;
            GC.SuppressFinalize(this);
        }

#pragma warning disable 1591
        ~MemoryPinner() =>
            Dispose(false);
#pragma warning restore 1591
    }

    /// <inheritdoc/>
    /// <summary>
    ///     Provides the functionality to manage data from an area of memory in a specified process.
    /// </summary>
    public class ProcessMemory : IDisposable
    {
        private readonly ArrayList _allocations = new ArrayList();
        private readonly IntPtr _hProcess;
        private bool _disposed;

        /// <summary>
        ///     Initilazies a new instance of the <see cref="ProcessMemory"/> class with
        ///     the specified window handle.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public ProcessMemory(IntPtr hWnd)
        {
            WinApi.NativeMethods.GetWindowThreadProcessId(hWnd, out var ownerProcessId);
            _hProcess = WinApi.NativeMethods.OpenProcess(WinApi.AccessRights.ProcessVmOperation | WinApi.AccessRights.ProcessVmRead | WinApi.AccessRights.ProcessVmWrite | WinApi.AccessRights.ProcessQueryInformation, false, ownerProcessId);
        }

        /// <inheritdoc/>
        /// <summary>
        ///     Releases all resources used by this <see cref="ProcessMemory"/>.
        /// </summary>
        public void Dispose() =>
            Dispose(true);

        /// <summary>
        ///     Releases all resources used by this <see cref="ProcessMemory"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || _hProcess == IntPtr.Zero)
                return;
            foreach (IntPtr ptr in _allocations)
                WinApi.NativeMethods.VirtualFreeEx(_hProcess, ptr, IntPtr.Zero, WinApi.MemFreeTypes.Release);
            WinApi.NativeMethods.CloseHandle(_hProcess);
            _disposed = true;
            if (!disposing)
                return;
            GC.SuppressFinalize(this);
        }

#pragma warning disable 1591
        ~ProcessMemory() =>
            Dispose(false);
#pragma warning restore 1591

        /// <summary>
        ///     Gets the file name of the process image.
        /// </summary>
        public string GetImageFileName()
        {
            var sb = new StringBuilder(short.MaxValue);
            return !WinApi.NativeMethods.GetProcessImageFileName(_hProcess, sb, sb.Capacity - 1) ? null : sb.ToString();
        }

        /// <summary>
        ///     Allocates a chunck of memory in the process.
        /// </summary>
        /// <param name="value">
        ///     The structure to be allocated.
        /// </param>
        public IntPtr Allocate(object value)
        {
            var size = new IntPtr(Marshal.SizeOf(value));
            var ptr = WinApi.NativeMethods.VirtualAllocEx(_hProcess, IntPtr.Zero, size, WinApi.MemAllocTypes.Commit, WinApi.MemProtectFlags.PageReadWrite);
            if (ptr != IntPtr.Zero)
                _allocations.Add(ptr);
            return ptr;
        }

        /// <summary>
        ///     Reads data from an area of memory in a specified process.
        /// </summary>
        /// <param name="value">
        ///     The structure to be allocated.
        /// </param>
        /// <param name="address">
        ///     A pointer to the base address in the specified process from which to read.
        /// </param>
        public void Read(object value, IntPtr address)
        {
            try
            {
                using (var pin = new MemoryPinner(value))
                {
                    var bytesRead = IntPtr.Zero;
                    var size = new IntPtr(Marshal.SizeOf(value));
                    if (WinApi.NativeMethods.ReadProcessMemory(_hProcess, address, pin.Pointer, size, ref bytesRead))
                        return;
                    throw new MemoryException($"Read failed (bytesRead={bytesRead}).");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                try
                {
                    WinApi.NativeHelper.ThrowLastError();
                }
                catch (Exception exc)
                {
                    Log.Write(exc);
                }
            }
        }

        /// <summary>
        ///     Reads a string from an area of memory in a specified process.
        /// </summary>
        /// <param name="size">
        ///     The number of bytes to be read from the specified process.
        /// </param>
        /// <param name="address">
        ///     A pointer to the base address in the specified process from which to read.
        /// </param>
        public string ReadString(int size, IntPtr address)
        {
            try
            {
                var sb = new StringBuilder(short.MaxValue);
                var bytesRead = IntPtr.Zero;
                if (WinApi.NativeMethods.ReadProcessMemory(_hProcess, address, sb, new IntPtr(size), ref bytesRead))
                    return sb.ToString();
                throw new MemoryException($"Read failed (bytesRead={bytesRead}).");
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                try
                {
                    WinApi.NativeHelper.ThrowLastError();
                }
                catch (Exception exc)
                {
                    Log.Write(exc);
                }
                return null;
            }
        }

        /// <summary>
        ///     Writes data to an area of memory in a specified process.
        /// </summary>
        /// <param name="value">
        ///     The structure to be allocated.
        /// </param>
        /// <param name="size">
        ///     The number of bytes to be written to the specified process.
        /// </param>
        /// <param name="buffer">
        ///     A pointer to the buffer that contains data to be written in the address
        ///     space of the specified process.
        /// </param>
        public void Write(object value, int size, IntPtr buffer)
        {
            try
            {
                using (var pin = new MemoryPinner(value))
                {
                    if (WinApi.NativeMethods.WriteProcessMemory(_hProcess, buffer, pin.Pointer, size, out var bytesWritten))
                        return;
                    throw new MemoryException($"Write failed (bytesWritten={bytesWritten}).");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                try
                {
                    WinApi.NativeHelper.ThrowLastError();
                }
                catch (Exception exc)
                {
                    Log.Write(exc);
                }
            }
        }
    }

    /// <inheritdoc/>
    /// <summary>
    ///     The exception that is thrown when an attempt to access some data in memory.
    /// </summary>
    [Serializable]
    public class MemoryException : Exception
    {
        /// <inheritdoc/>
        /// <summary>
        ///     Create the exception.
        /// </summary>
        public MemoryException() { }

        /// <inheritdoc/>
        /// <summary>
        ///     Create the exception with a specified error message.
        /// </summary>
        /// <param name="message">
        ///     Exception message.
        /// </param>
        public MemoryException(string message) : base(message) { }

        /// <inheritdoc/>
        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryException"/> class with serialized data.
        /// </summary>
        protected MemoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <inheritdoc/>
        /// <summary>
        ///     Gets the error message.
        /// </summary>
        public sealed override string Message { get; } = "Unable to access to the specified area of the memory.";
    }
}
