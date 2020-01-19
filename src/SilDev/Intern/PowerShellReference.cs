#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: PowerShellReference.cs
// Version:  2020-01-19 15:32
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Intern
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    internal static class PowerShellReference
    {
        private static volatile Assembly _assembly;
        private static volatile bool _assemblyChecked;
        private static volatile object _syncObject;

        internal static Assembly Assembly
        {
            get
            {
                lock (SyncObject)
                {
                    if (_assemblyChecked)
                        return _assembly;
                    var dir = "%WinDir%\\Microsoft.NET\\assembly\\GAC_MSIL\\System.Management.Automation";
                    var path = DirectoryEx.EnumerateFiles(dir, "System.Management.Automation.dll", SearchOption.AllDirectories)?.FirstOrDefault();
                    try
                    {
                        if (string.IsNullOrEmpty(path))
                            throw new NullReferenceException();
                        if (!File.Exists(path))
                            throw new FileNotFoundException();
                        dir = Path.GetDirectoryName(path);
                        if (string.IsNullOrEmpty(dir))
                            throw new NullReferenceException();
                        _assembly = Assembly.LoadFrom(path);
                        Location = dir;
                    }
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        Log.Write(ex);
                        _assembly = null;
                        Location = null;
                    }
                    _assemblyChecked = true;
                    return _assembly;
                }
            }
        }

        internal static string Location { get; private set; }

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
    }
}
