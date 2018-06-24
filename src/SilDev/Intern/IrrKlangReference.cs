#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IrrKlangReference.cs
// Version:  2018-06-24 03:28
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Intern
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Properties;

    internal static class IrrKlangReference
    {
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

                    const string coreFileName = "irrKlang.NET4.dll";
                    var fileMap = new Dictionary<string, Tuple<string, string, string>>
                    {
#if x86
                        {
                            "ikpFlac.dll",
                            Tuple.Create("afeb7c93", "82d93ea3", "201c520ab205ebc47aa1b8733253696311913a233bfe312d69244fa185ab92c6")
                        },            
                        {
                            "ikpMP3.dll",
                            Tuple.Create("a6d1ee1b", "63f0f2bd", "46ba904fd7ec99a5a1dfffda57b968f21a36fa719059c03b0e54f7ba589a7cc3")
                        },            
                        {
                            coreFileName,
                            Tuple.Create("09ec1302", "c4726e21", "8e66bf235e9999983fae4b568bc2d0809b1c5b4682f16c62723faea41bc84c9d")
                        }
#else
                        {
                            "ikpFlac.dll",
                            Tuple.Create("3e2af648", "136b6659", "ba5ce54e067a942fb2b2fd1c97a72a4a822cc4f1808d7b170c56f5b4704e263f")
                        },
                        {
                            "ikpMP3.dll",
                            Tuple.Create("b226d6c4", "843f9b8a", "fb46f2455a803723576a4030d6c588887c620409c06a49a551b401e7df11d790")
                        },
                        {
                            coreFileName,
                            Tuple.Create("fdd65a57", "44af99ee", "b056e30fdfd91f216f85387fa1098c520735b7d82f349da0a2f669d7c7c286d1")
                        }
#endif
                    };

                    var refDirs = string.Format(Resources.ReferenceDirs, Math.Abs(PathEx.LocalPath.GetHashCode())).SplitNewLine();
                    var isValid = false;
                    foreach (var entry in refDirs.Select(PathEx.Combine))
                    {
                        var dirs = new[]
                        {
                            entry,
#if x86
                            Path.Combine(entry, "32"),
                            Path.Combine(entry, "x86"),
#else
                            Path.Combine(entry, "64"),
                            Path.Combine(entry, "x64")
#endif
                        };
                        foreach (var dir in dirs)
                        {
                            var path = PathEx.Combine(dir, coreFileName);
#if x86
                            if (!File.Exists(path) || PortableExecutable.Is64Bit(path))
                                continue;
#else
                            if (!File.Exists(path) || !PortableExecutable.Is64Bit(path))
                                continue;
#endif
                            isValid = fileMap.Select(x => Tuple.Create(Path.Combine(dir, x.Key), x.Value.Item1, x.Value.Item2, x.Value.Item3))
                                             .All(x => !File.Exists(x.Item1) ||
                                                       x.Item1.EncryptFile(ChecksumAlgorithms.Adler32).Equals(x.Item2) &&
                                                       x.Item1.EncryptFile(ChecksumAlgorithms.Crc32).Equals(x.Item3) &&
                                                       x.Item1.EncryptFile(ChecksumAlgorithms.Sha256).Equals(x.Item4));
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
    }
}
