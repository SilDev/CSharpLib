#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IrrKlangReference.cs
// Version:  2019-10-20 19:47
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Intern
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Properties;

    internal static class IrrKlangReference
    {
        private const string CoreFileName = "irrKlang.NET4.dll";
        private static Assembly _assembly;
        private static bool _assemblyChecked;
        private static readonly object Locker = new object();

        internal static Assembly Assembly
        {
            get
            {
                lock (Locker)
                {
                    if (_assemblyChecked)
                        return _assembly;

#if x86
                    var fileMap = FileMap32;
#elif x64
                    var fileMap = FileMap64;
#else
                    var fileMap = Environment.Is64BitProcess ? FileMap64 : FileMap32;
#endif

                    var refDirs = new[]
                    {
                        "%CurDir%",
                        "%CurDir%\\Bin",
                        "%CurDir%\\Binaries",
                        "%ApplicationData%\\References",
                        "%ApplicationData%\\References\\dotNet4\\irrKlang",
                        "%LocalApplicationData%\\References",
                        "%LocalApplicationData%\\References\\dotNet4\\irrKlang",
                        "%CommonApplicationData%\\References",
                        "%CommonApplicationData%\\References\\dotNet4\\irrKlang",
                        "%UserProfile%\\References",
                        "%UserProfile%\\References\\dotNet4\\irrKlang",
                        "%MyDocuments%\\References",
                        "%MyDocuments%\\References\\dotNet4\\irrKlang",
                        "%MyDocuments%\\Visual Studio\\References",
                        "%MyDocuments%\\Visual Studio\\References\\dotNet4\\irrKlang",
                        "%MyDocuments%\\Visual Studio 2015\\References",
                        "%MyDocuments%\\Visual Studio 2015\\References\\dotNet4\\irrKlang",
                        "%MyDocuments%\\Visual Studio 2017\\References",
                        "%MyDocuments%\\Visual Studio 2017\\References\\dotNet4\\irrKlang",
                        "%MyDocuments%\\Visual Studio 2019\\References",
                        "%MyDocuments%\\Visual Studio 2019\\References\\dotNet4\\irrKlang",
                        string.Format(CultureInfo.InvariantCulture, Resources.TempDirFormat, Math.Abs(PathEx.LocalPath.GetHashCode()))
                    };

                    var isValid = false;
                    foreach (var entry in refDirs.Select(PathEx.Combine))
                    {
                        var dirs = new[]
                        {
                            entry,
#if any || x86
                            Path.Combine(entry, "32"),
                            Path.Combine(entry, "x86"),
#endif
#if any || x64
                            Path.Combine(entry, "64"),
                            Path.Combine(entry, "x64")
#endif
                        };
                        foreach (var dir in dirs)
                        {
#if x86
                            var path = PathEx.Combine(dir, CoreFileName);
                            if (!File.Exists(path) || PortableExecutable.Is64Bit(path))
                                continue;
#elif x64
                            var path = PathEx.Combine(dir, CoreFileName);
                            if (!File.Exists(path) || !PortableExecutable.Is64Bit(path))
                                continue;
#else
                            if (Environment.Is64BitProcess && dir.EndsWithEx("32", "x86") || !Environment.Is64BitProcess && dir.EndsWithEx("64", "x64"))
                                continue;

                            var path = PathEx.Combine(dir, CoreFileName);
                            if (!File.Exists(path) || Environment.Is64BitProcess && !PortableExecutable.Is64Bit(path) || !Environment.Is64BitProcess && PortableExecutable.Is64Bit(path))
                                continue;
#endif

                            isValid = fileMap.Select(x => Tuple.Create(Path.Combine(dir, x.Key), x.Value.Item1, x.Value.Item2, x.Value.Item3))
                                             .All(x => !File.Exists(x.Item1) ||
                                                       x.Item1.EncryptFile(ChecksumAlgorithm.Adler32).Equals(x.Item2, StringComparison.Ordinal) &&
                                                       x.Item1.EncryptFile(ChecksumAlgorithm.Crc32).Equals(x.Item3, StringComparison.Ordinal) &&
                                                       x.Item1.EncryptFile(ChecksumAlgorithm.Sha256).Equals(x.Item4, StringComparison.Ordinal));

                            if (!isValid)
                                continue;

                            try
                            {
                                _assembly = Assembly.LoadFrom(path);
                                Location = dir;
                            }
                            catch (Exception ex)
                            {
                                Log.Write(ex);
                                _assembly = null;
                                Location = null;
                                isValid = false;
                                continue;
                            }
                            break;
                        }
                        if (isValid)
                            break;
                    }

                    _assemblyChecked = true;
                    return _assembly;
                }
            }
        }

        internal static string Location { get; private set; }

#if any || x86
        private static Dictionary<string, Tuple<string, string, string>> _fileMap32;

        private static Dictionary<string, Tuple<string, string, string>> FileMap32
        {
            get
            {
                if (_fileMap32 != default(Dictionary<string, Tuple<string, string, string>>))
                    return FileMap32;
                _fileMap32 = new Dictionary<string, Tuple<string, string, string>>
                {
                    {
                        "ikpFlac.dll",
                        Tuple.Create("afeb7c93", "82d93ea3", "201c520ab205ebc47aa1b8733253696311913a233bfe312d69244fa185ab92c6")
                    },
                    {
                        "ikpMP3.dll",
                        Tuple.Create("a6d1ee1b", "63f0f2bd", "46ba904fd7ec99a5a1dfffda57b968f21a36fa719059c03b0e54f7ba589a7cc3")
                    },
                    {
                        CoreFileName,
                        Tuple.Create("09ec1302", "c4726e21", "8e66bf235e9999983fae4b568bc2d0809b1c5b4682f16c62723faea41bc84c9d")
                    }
                };
                return _fileMap32;
            }
        }
#endif
#if any || x64
        private static Dictionary<string, Tuple<string, string, string>> _fileMap64;

        private static Dictionary<string, Tuple<string, string, string>> FileMap64
        {
            get
            {
                if (_fileMap64 != default(Dictionary<string, Tuple<string, string, string>>))
                    return _fileMap64;
                _fileMap64 = new Dictionary<string, Tuple<string, string, string>>
                {
                    {
                        "ikpFlac.dll",
                        Tuple.Create("3e2af648", "136b6659", "ba5ce54e067a942fb2b2fd1c97a72a4a822cc4f1808d7b170c56f5b4704e263f")
                    },
                    {
                        "ikpMP3.dll",
                        Tuple.Create("b226d6c4", "843f9b8a", "fb46f2455a803723576a4030d6c588887c620409c06a49a551b401e7df11d790")
                    },
                    {
                        CoreFileName,
                        Tuple.Create("fdd65a57", "44af99ee", "b056e30fdfd91f216f85387fa1098c520735b7d82f349da0a2f669d7c7c286d1")
                    }
                };
                return _fileMap64;
            }
        }
#endif
    }
}
