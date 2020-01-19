#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IrrKlangReference.cs
// Version:  2020-01-19 15:32
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
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
    using System.Threading;

    internal static class IrrKlangReference
    {
        private const string CoreModuleName = "irrKlang.NET4.dll";
        private const string PluginModule1Name = "ikpFlac.dll";
        private const string PluginModule2Name = "ikpMP3.dll";
        private static volatile Assembly _assembly;
        private static volatile bool _assemblyChecked;
        private static volatile string _location;
        private static volatile object _syncObject;

        internal static Assembly Assembly
        {
            get
            {
                lock (SyncObject)
                {
                    if (_assemblyChecked)
                        return _assembly;

                    var workingDir = Directory.GetCurrentDirectory();
                    var refDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    {
                        PathEx.Combine(PathEx.LocalDir),
                        PathEx.Combine(PathEx.LocalDir, "bin"),
                        PathEx.Combine(PathEx.LocalDir, "binaries"),
                        PathEx.Combine(PathEx.LibraryDir),
                        PathEx.Combine(PathEx.LibraryDir, "bin"),
                        PathEx.Combine(PathEx.LibraryDir, "binaries"),
                        PathEx.Combine(workingDir),
                        PathEx.Combine(workingDir, "bin"),
                        PathEx.Combine(workingDir, "binaries")
                    };

                    var isValid = false;
                    foreach (var entry in refDirs)
                    {
                        var dirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                        {
                            entry,
#if x86
                            Path.Combine(entry, "32"),
                            Path.Combine(entry, "x86"),
#elif x64
                            Path.Combine(entry, "64"),
                            Path.Combine(entry, "x64")
#endif
                        };

#if any
                        if (Environment.Is64BitProcess)
                        {
                            dirs.Add(Path.Combine(entry, "64"));
                            dirs.Add(Path.Combine(entry, "x64"));
                        }
                        else
                        {
                            dirs.Add(Path.Combine(entry, "32"));
                            dirs.Add(Path.Combine(entry, "x86"));
                        }
#endif

                        var files = new[]
                        {
                            CoreModuleName,
                            PluginModule1Name,
                            PluginModule2Name
                        };

                        foreach (var dir in dirs)
                        {
                            if (files.Where((file, i) => !FileIsValid(dir, file, GetHashData(file, Environment.Is64BitProcess), i == 0)).Any())
                                continue;
                            try
                            {
                                var path = Path.Combine(dir, CoreModuleName);
                                _assembly = Assembly.LoadFrom(path);
                                Location = dir;
                                isValid = true;
                                break;
                            }
                            catch (Exception ex) when (ex.IsCaught())
                            {
                                Log.Write(ex);
                                _assembly = null;
                                Location = null;
                            }
                        }
                        if (isValid)
                            break;
                    }

                    _assemblyChecked = true;
                    return _assembly;
                }
            }
        }

        internal static string Location
        {
            get => _location;
            private set
            {
                lock (SyncObject)
                    _location = value;
            }
        }

        private static object SyncObject
        {
            get
            {
                if (_syncObject != null)
                    return _syncObject;
                var obj = new object();
                Interlocked.CompareExchange<object>(ref _syncObject, obj, null);
                return _syncObject;
            }
        }

        private static bool FileIsValid(string dir, string file, IEnumerable<(ChecksumAlgorithm, string)> hashData, bool isCore)
        {
            var path = PathEx.Combine(dir, file);
            if (!File.Exists(path))
                return !isCore;
            if (hashData == null)
                return true;
            foreach (var (algo, hash) in hashData)
            {
                if (path.EncryptFile(algo) == hash)
                    continue;
                return false;
            }
            return true;
        }

        private static IEnumerable<(ChecksumAlgorithm, string)> GetHashData(string file, bool is64Bit)
        {
            if (file == null)
                yield break;
            switch (file)
            {
                case CoreModuleName:
                    if (is64Bit)
                    {
                        yield return (ChecksumAlgorithm.Adler32, "fdd65a57");
                        yield return (ChecksumAlgorithm.Crc32, "44af99ee");
                        yield return (ChecksumAlgorithm.Sha256, "b056e30fdfd91f216f85387fa1098c520735b7d82f349da0a2f669d7c7c286d1");
                    }
                    else
                    {
                        yield return (ChecksumAlgorithm.Adler32, "09ec1302");
                        yield return (ChecksumAlgorithm.Crc32, "c4726e21");
                        yield return (ChecksumAlgorithm.Sha256, "8e66bf235e9999983fae4b568bc2d0809b1c5b4682f16c62723faea41bc84c9d");
                    }
                    break;
                case PluginModule1Name:
                    if (is64Bit)
                    {
                        yield return (ChecksumAlgorithm.Adler32, "3e2af648");
                        yield return (ChecksumAlgorithm.Crc32, "136b6659");
                        yield return (ChecksumAlgorithm.Sha256, "ba5ce54e067a942fb2b2fd1c97a72a4a822cc4f1808d7b170c56f5b4704e263f");
                    }
                    else
                    {
                        yield return (ChecksumAlgorithm.Adler32, "afeb7c93");
                        yield return (ChecksumAlgorithm.Crc32, "82d93ea3");
                        yield return (ChecksumAlgorithm.Sha256, "201c520ab205ebc47aa1b8733253696311913a233bfe312d69244fa185ab92c6");
                    }
                    break;
                case PluginModule2Name:
                    if (is64Bit)
                    {
                        yield return (ChecksumAlgorithm.Adler32, "b226d6c4");
                        yield return (ChecksumAlgorithm.Crc32, "843f9b8a");
                        yield return (ChecksumAlgorithm.Sha256, "fb46f2455a803723576a4030d6c588887c620409c06a49a551b401e7df11d790");
                    }
                    else
                    {
                        yield return (ChecksumAlgorithm.Adler32, "a6d1ee1b");
                        yield return (ChecksumAlgorithm.Crc32, "63f0f2bd");
                        yield return (ChecksumAlgorithm.Sha256, "46ba904fd7ec99a5a1dfffda57b968f21a36fa719059c03b0e54f7ba589a7cc3");
                    }
                    break;
            }
        }
    }
}
