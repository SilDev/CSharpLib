﻿#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Memory.cs
// Version:  2023-12-05 13:51
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
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
    using static WinApi;

    /// <summary>
    ///     Provides static methods to reduce the memory usage of the current process.
    /// </summary>
    public static class CurrentMemory
    {
        /// <summary>
        ///     Removes the specified element from current process memory.
        ///     <para>
        ///         Note that the element is only removed if you set all references to
        ///         <see langword="null"/>, except for the one with which you call this
        ///         method; otherwise the element will remain in memory even if it appears
        ///         to be removed.
        ///     </para>
        /// </summary>
        /// <typeparam name="TElement">
        ///     The type of the element.
        /// </typeparam>
        /// <param name="element">
        ///     The element to be removed.
        /// </param>
        public static void Destroy<TElement>(ref TElement element) where TElement : class
        {
            if (element == null)
                return;
            var isCollection = false;
            switch (element)
            {
                case ICollection:
                    isCollection = element is not Array;
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
            var generation = GC.GetGeneration(element);
            element = null;
            GC.Collect(generation, GCCollectionMode.Forced);
            if (isCollection)
                GC.Collect();
        }
    }

    /// <summary>
    ///     Provides a way to pin a managed object from unmanaged memory.
    /// </summary>
    public class MemoryPinner : IDisposable
    {
        private GCHandle _handle;

        /// <summary>
        ///     Returns the pointer to the pinned object.
        /// </summary>
        public IntPtr PointerHandle { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryPinner"/> class with the
        ///     specified object to pin.
        /// </summary>
        /// <param name="value">
        ///     The object to pin.
        /// </param>
        public MemoryPinner(object value)
        {
            _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            PointerHandle = _handle.AddrOfPinnedObject();
        }

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
            PointerHandle = IntPtr.Zero;
        }
    }

    /// <summary>
    ///     Provides the functionality to manage data from an area of memory in a
    ///     specified process.
    /// </summary>
    public class ProcessMemory : IDisposable
    {
        private readonly ArrayList _allocations = new();
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
            _ = NativeMethods.GetWindowThreadProcessId(hWnd, out var ownerProcessId);
            _hProcess = NativeMethods.OpenProcess(AccessRights.ProcessVmOperation | AccessRights.ProcessVmRead | AccessRights.ProcessVmWrite | AccessRights.ProcessQueryInformation, false, ownerProcessId);
        }

        ~ProcessMemory() =>
            Dispose(false);

        /// <summary>
        ///     Gets the file name of the process image.
        /// </summary>
        public string GetImageFileName()
        {
            var sb = new StringBuilder(short.MaxValue);
            return !NativeMethods.GetProcessImageFileName(_hProcess, sb, sb.Capacity - 1) ? null : sb.ToStringThenClear();
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
            var ptr = NativeMethods.VirtualAllocEx(_hProcess, IntPtr.Zero, size, MemAllocTypes.Commit, MemProtectFlags.PageReadWrite);
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
                using var pin = new MemoryPinner(value);
                var bytesRead = IntPtr.Zero;
                var size = new IntPtr(Marshal.SizeOf(value));
                if (NativeMethods.ReadProcessMemory(_hProcess, address, pin.PointerHandle, size, ref bytesRead))
                    return;
                throw new MemoryException(ExceptionMessages.BytesReadFailed + bytesRead);
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
                if (NativeMethods.ReadProcessMemory(_hProcess, address, sb, new IntPtr(size), ref bytesRead))
                    return sb.ToStringThenClear();
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
                using var pin = new MemoryPinner(value);
                if (NativeMethods.WriteProcessMemory(_hProcess, buffer, pin.PointerHandle, size, out var bytesWritten))
                    return;
                throw new MemoryException(ExceptionMessages.BytesWriteFailed + bytesWritten);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
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

        /// <summary>
        ///     Releases all resources used by this <see cref="ProcessMemory"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _hProcess == IntPtr.Zero)
                return;
            foreach (var ptr in _allocations.Cast<IntPtr>())
                _ = NativeMethods.VirtualFreeEx(_hProcess, ptr, IntPtr.Zero, MemFreeType.Release);
            NativeMethods.CloseHandle(_hProcess);
        }
    }
}
