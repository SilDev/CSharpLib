#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PortableExecutable.cs
// Version:  2019-10-21 15:08
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    /// <summary>
    ///     Provides enumerated values of the machine field values that specifies its CPU type.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum MachineType
    {
        /// <summary>
        ///     The contents of this field are assumed to be applicable to any machine type.
        /// </summary>
        Unknown = 0x0,

        /// <summary>
        ///     Matsushita AM33.
        /// </summary>
        AM33 = 0x1d3,

        /// <summary>
        ///     AMD64 (x64).
        /// </summary>
        AMD64 = 0x8664,

        /// <summary>
        ///     ARM little endian.
        /// </summary>
        ARM = 0x1c0,

        /// <summary>
        ///     ARM64 little endian.
        /// </summary>
        ARM64 = 0xaa64,

        /// <summary>
        ///     ARM Thumb-2 little endian.
        /// </summary>
        ARMNT = 0x1c4,

        /// <summary>
        ///     EFI byte code.
        /// </summary>
        EBC = 0xebc,

        /// <summary>
        ///     Intel 386 or later processors and compatible processors.
        /// </summary>
        I386 = 0x14c,

        /// <summary>
        ///     Intel Itanium processor family.
        /// </summary>
        IA64 = 0x200,

        /// <summary>
        ///     Mitsubishi M32R little endian.
        /// </summary>
        M32R = 0x9041,

        /// <summary>
        ///     MIPS16.
        /// </summary>
        MIPS16 = 0x266,

        /// <summary>
        ///     MIPS with FPU.
        /// </summary>
        MIPSFPU = 0x366,

        /// <summary>
        ///     MIPS16 with FPU.
        /// </summary>
        MIPSFPU16 = 0x466,

        /// <summary>
        ///     Power PC little endian.
        /// </summary>
        POWERPC = 0x1f0,

        /// <summary>
        ///     Power PC with floating point support.
        /// </summary>
        POWERPCFP = 0x1f1,

        /// <summary>
        ///     MIPS little endian.
        /// </summary>
        R4000 = 0x166,

        /// <summary>
        ///     RISC-V 32-bit address space.
        /// </summary>
        RISCV32 = 0x5032,

        /// <summary>
        ///     RISC-V 64-bit address space.
        /// </summary>
        RISCV64 = 0x5064,

        /// <summary>
        ///     RISC-V 128-bit address space.
        /// </summary>
        RISCV128 = 0x5128,

        /// <summary>
        ///     Hitachi SH3.
        /// </summary>
        SH3 = 0x1a2,

        /// <summary>
        ///     Hitachi SH3 DSP.
        /// </summary>
        SH3DSP = 0x1a3,

        /// <summary>
        ///     Hitachi SH4.
        /// </summary>
        SH4 = 0x1a6,

        /// <summary>
        ///     Hitachi SH5.
        /// </summary>
        SH5 = 0x1a8,

        /// <summary>
        ///     ARM Thumb.
        /// </summary>
        THUMB = 0x1c2,

        /// <summary>
        ///     MIPS little-endian WCE v2.
        /// </summary>
        WCEMIPSV2 = 0x169
    }

    /// <summary>
    ///     Provides basic functionality for reading PE (Portable Executable) header information.
    /// </summary>
    public static class PortableExecutable
    {
        /// <summary>
        ///     Determines the CPU type of the specified file.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static MachineType GetMachineTypes(string path)
        {
            var pe = MachineType.Unknown;
            try
            {
                var file = PathEx.Combine(path);
                if (!PathEx.IsValidPath(file))
                    throw new ArgumentException();
                if (!File.Exists(file))
                    throw new PathNotFoundException(file);
                var fs = default(FileStream);
                try
                {
                    fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    using (var br = new BinaryReader(fs))
                    {
                        var i = fs;
                        fs = null;
                        i.Seek(0x3c, SeekOrigin.Begin);
                        i.Seek(br.ReadInt32(), SeekOrigin.Begin);
                        br.ReadUInt32();
                        pe = (MachineType)br.ReadUInt16();
                    }
                }
                finally
                {
                    fs?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            return pe;
        }

        /// <summary>
        ///     Determines whether the specified file was compiled for 64-bit platform environments.
        /// </summary>
        /// <param name="path">
        ///     The file to check.
        /// </param>
        public static bool Is64Bit(string path)
        {
            var pe = GetMachineTypes(path);
            return pe == MachineType.AMD64 || pe == MachineType.IA64;
        }
    }
}
