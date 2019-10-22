#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Memory.cs
// Version:  2019-10-22 16:00
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using Properties;

    /// <summary>
    ///     Provides a way to pin a managed object from unmanaged memory.
    /// </summary>
    public class MemoryPinner : IDisposable
    {
        private GCHandle _handle;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryPinner"/> class with
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

        /// <summary>
        ///     Releases all resources used by this <see cref="MemoryPinner"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases all resources used by this <see cref="MemoryPinner"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (_handle.IsAllocated)
                _handle.Free();
            Pointer = IntPtr.Zero;
        }
    }

    /// <summary>
    ///     Provides the functionality to manage data from an area of memory in a specified process.
    /// </summary>
    public class ProcessMemory : IDisposable
    {
        private readonly ArrayList _allocations = new ArrayList();
        private readonly IntPtr _hProcess;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProcessMemory"/> class with
        ///     the specified window handle.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        public ProcessMemory(IntPtr hWnd)
        {
            _ = WinApi.NativeMethods.GetWindowThreadProcessId(hWnd, out var ownerProcessId);
            _hProcess = WinApi.NativeMethods.OpenProcess(WinApi.AccessRights.ProcessVmOperation | WinApi.AccessRights.ProcessVmRead | WinApi.AccessRights.ProcessVmWrite | WinApi.AccessRights.ProcessQueryInformation, false, ownerProcessId);
        }

        /// <summary>
        ///     Releases all resources used by this <see cref="ProcessMemory"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases all resources used by this <see cref="ProcessMemory"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _hProcess == IntPtr.Zero)
                return;
            foreach (var ptr in _allocations.Cast<IntPtr>())
                _ = WinApi.NativeMethods.VirtualFreeEx(_hProcess, ptr, IntPtr.Zero, WinApi.MemFreeType.Release);
            WinApi.NativeMethods.CloseHandle(_hProcess);
        }

        ~ProcessMemory() =>
            Dispose(false);

        /// <summary>
        ///     Gets the file name of the process image.
        /// </summary>
        public string GetImageFileName()
        {
            var sb = new StringBuilder(short.MaxValue);
            return !WinApi.NativeMethods.GetProcessImageFileName(_hProcess, sb, sb.Capacity - 1) ? null : sb.ToString();
        }

        /// <summary>
        ///     Allocates a chunk of memory in the process.
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
                    throw new MemoryException(ExceptionMessages.BytesReadFailed + bytesRead);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
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
                throw new MemoryException(ExceptionMessages.BytesReadFailed + bytesRead);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
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
                    throw new MemoryException(ExceptionMessages.BytesWriteFailed + bytesWritten);
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
        }
    }
}
