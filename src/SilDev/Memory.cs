#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Memory.cs
// Version:  2017-05-04 16:37
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

    public class MemoryPinner : IDisposable
    {
        private GCHandle _handle;
        private bool _disposed;

        /// <summary>
        ///     Returns the pointer to the pinned object.
        /// </summary>
        public IntPtr Pointer { get; private set; }

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

        ~MemoryPinner()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Releases all resources used by this <see cref="MemoryPinner"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || !_disposed)
                return;
            _handle.Free();
            Pointer = IntPtr.Zero;
            _disposed = true;
        }
    }

    public class ProcessMemory : IDisposable
    {
        private readonly IntPtr _hProcess;
        private readonly ArrayList _allocations = new ArrayList();
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
            uint ownerProcessId;
            WinApi.UnsafeNativeMethods.GetWindowThreadProcessId(hWnd, out ownerProcessId);
            _hProcess = WinApi.UnsafeNativeMethods.OpenProcess((uint)(WinApi.AccessRights.PROCESS_VM_OPERATION | WinApi.AccessRights.PROCESS_VM_READ | WinApi.AccessRights.PROCESS_VM_WRITE | WinApi.AccessRights.PROCESS_QUERY_INFORMATION), false, ownerProcessId);
        }

        ~ProcessMemory()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Gets the file name of the process image.
        /// </summary>
        public string GetImageFileName()
        {
            var sb = new StringBuilder(short.MaxValue);
            return !WinApi.UnsafeNativeMethods.GetProcessImageFileName(_hProcess, sb, sb.Capacity - 1) ? null : sb.ToString();
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
            var ptr = WinApi.UnsafeNativeMethods.VirtualAllocEx(_hProcess, IntPtr.Zero, size, WinApi.AllocationTypes.MEM_COMMIT, WinApi.VirtualAllocFuncMemProtect.PAGE_READWRITE);
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
                    if (WinApi.UnsafeNativeMethods.ReadProcessMemory(_hProcess, address, pin.Pointer, size, ref bytesRead))
                        return;
                    throw new MemoryException("Read failed (bytesRead=" + bytesRead + ").");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                try
                {
                    WinApi.ThrowLastError();
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
                if (WinApi.UnsafeNativeMethods.ReadProcessMemory(_hProcess, address, sb, new IntPtr(size), ref bytesRead))
                    return sb.ToString();
                throw new MemoryException("Read failed (bytesRead=" + bytesRead + ").");
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                try
                {
                    WinApi.ThrowLastError();
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
                    IntPtr bytesWritten;
                    if (WinApi.UnsafeNativeMethods.WriteProcessMemory(_hProcess, buffer, pin.Pointer, size, out bytesWritten))
                        return;
                    throw new MemoryException("Write failed (bytesWritten=" + bytesWritten + ").");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                try
                {
                    WinApi.ThrowLastError();
                }
                catch (Exception exc)
                {
                    Log.Write(exc);
                }
            }
        }

        /// <summary>
        ///     Releases all resources used by this <see cref="ProcessMemory"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed || !disposing)
                return;
            if (_hProcess != IntPtr.Zero)
            {
                foreach (IntPtr ptr in _allocations)
                    WinApi.UnsafeNativeMethods.VirtualFreeEx(_hProcess, ptr, IntPtr.Zero, WinApi.MemFreeTypes.MEM_RELEASE);
                WinApi.UnsafeNativeMethods.CloseHandle(_hProcess);
            }
            _disposed = true;
        }
    }

    /// <summary>
    ///     The exception that is thrown when an attempt to access some data in memory.
    /// </summary>
    [Serializable]
    public class MemoryException : Exception
    {
        /// <summary>
        ///     Create the exception.
        /// </summary>
        public MemoryException() { }

        /// <summary>
        ///     Create the exception with a specified error message.
        /// </summary>
        /// <param name="message">
        ///     Exception message.
        /// </param>
        public MemoryException(string message) : base(message) { }

        protected MemoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Gets the error message.
        /// </summary>
        public sealed override string Message { get; } = "Unable to access to the specified area of the memory.";
    }
}
