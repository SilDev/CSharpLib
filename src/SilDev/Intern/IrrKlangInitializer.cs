#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IrrKlangInitializer.cs
// Version:  2017-05-16 09:46
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Intern
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Properties;

    internal static class IrrKlangInitializer
    {
        internal static string AssemblyDirectory { get; private set; }
        internal static bool AssemblyLoaded { get; private set; }

        internal static bool LoadAssembly()
        {
            var hash = Math.Abs(PathEx.LocalPath.GetHashCode());
            var dirs = string.Format(Resources.ReferenceDirs, hash);
            var list = dirs.SplitNewLine().ToList();
            list.Add(PathEx.LocalDir);
            const string reqName = "msvcr120.dll";
            var reqPath = PathEx.Combine("%system%", reqName);
            var exist = File.Exists(reqPath);
            const string libName = "irrKlang.NET4.dll";
            foreach (var entry in list)
            {
#if x86
                var dir = PathEx.Combine(entry, "x86");
#else
                var dir = PathEx.Combine(entry, "x64");
#endif
                if (!exist)
                {
                    reqPath = PathEx.Combine(dir, reqName);
                    if (!File.Exists(reqPath))
                        continue;
                }
                var libPath = PathEx.Combine(dir, libName);
                if (!File.Exists(libPath))
                    continue;
                AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
                        Assembly.LoadFrom(libPath);
                AssemblyDirectory = dir;
                AssemblyLoaded = true;
                break;
            }
            return AssemblyLoaded;
        }
    }
}
