#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Redist.cs
// Version:  2023-11-11 16:27
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Linq;
    using Investment;
    using Microsoft.Win32;

    /// <summary>
    ///     Provides identity flags of redistributable packages. For more information,
    ///     see <see cref="Redist.IsInstalled(RedistFlags[])"/>.
    /// </summary>
    /// ReSharper disable InconsistentNaming
    [Flags]
    public enum RedistFlags
    {
        /// <summary>
        ///     Microsoft Visual C++ 2005 Redistributable Package (x86).
        /// </summary>
        VC2005X86 = 0x1,

        /// <summary>
        ///     Microsoft Visual C++ 2005 Redistributable Package (x64).
        /// </summary>
        VC2005X64 = 0x2,

        /// <summary>
        ///     Microsoft Visual C++ 2008 Redistributable Package (x86).
        /// </summary>
        VC2008X86 = 0x4,

        /// <summary>
        ///     Microsoft Visual C++ 2008 Redistributable Package (x64).
        /// </summary>
        VC2008X64 = 0x8,

        /// <summary>
        ///     Microsoft Visual C++ 2010 Redistributable Package (x86).
        /// </summary>
        VC2010X86 = 0x10,

        /// <summary>
        ///     Microsoft Visual C++ 2010 Redistributable Package (x64).
        /// </summary>
        VC2010X64 = 0x20,

        /// <summary>
        ///     Microsoft Visual C++ 2012 Redistributable Package (x86).
        /// </summary>
        VC2012X86 = 0x40,

        /// <summary>
        ///     Microsoft Visual C++ 2012 Redistributable Package (x64).
        /// </summary>
        VC2012X64 = 0x80,

        /// <summary>
        ///     Microsoft Visual C++ 2013 Redistributable Package (x86).
        /// </summary>
        VC2013X86 = 0x100,

        /// <summary>
        ///     Microsoft Visual C++ 2013 Redistributable Package (x64).
        /// </summary>
        VC2013X64 = 0x200,

        /// <summary>
        ///     Microsoft Visual C++ 2015 Redistributable Package (x86).
        /// </summary>
        VC2015X86 = 0x400,

        /// <summary>
        ///     Microsoft Visual C++ 2015 Redistributable Package (x64).
        /// </summary>
        VC2015X64 = 0x800,

        /// <summary>
        ///     Microsoft Visual C++ 2017 Redistributable Package (x86).
        /// </summary>
        VC2017X86 = 0x1000,

        /// <summary>
        ///     Microsoft Visual C++ 2017 Redistributable Package (x64).
        /// </summary>
        VC2017X64 = 0x2000,

        /// <summary>
        ///     Microsoft Visual C++ 2019 Redistributable Package (x86).
        /// </summary>
        VC2019X86 = 0x4000,

        /// <summary>
        ///     Microsoft Visual C++ 2019 Redistributable Package (x64).
        /// </summary>
        VC2019X64 = 0x8000
    }

    /// <summary>
    ///     Provides functionality to verify the installation of redistributable
    ///     packages.
    /// </summary>
    public static class Redist
    {
        private static string[] _displayNames;

        /// <summary>
        ///     Returns the display names of all installed Microsoft Visual C++
        ///     redistributable packages.
        /// </summary>
        /// <param name="refresh">
        ///     <see langword="true"/> to refresh all names; otherwise,
        ///     <see langword="false"/> to get the cached names from previous call.
        ///     <para>
        ///         Please note that this parameter is always <see langword="true"/> if
        ///         this function has never been called before.
        ///     </para>
        /// </param>
        public static string[] GetDisplayNames(bool refresh = false)
        {
            try
            {
                if (!refresh && _displayNames != null)
                    return _displayNames;
                var comparer = CacheInvestor.GetDefault<AlphaNumericComparer<string>>();
                var entries = new[]
                {
                    "DisplayName",
                    "ProductName"
                };
                var names = Reg.GetSubKeyTree(Registry.LocalMachine, "SOFTWARE\\Classes\\Installer", 3000)
                               .SelectMany(_ => entries, (x, y) => Reg.ReadString(Registry.LocalMachine, x, y))
                               .Where(x => x.StartsWithEx("Microsoft Visual C++"))
                               .OrderBy(x => x, comparer);
                _displayNames = names.ToArray();
                return _displayNames;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return null;
            }
        }

        /// <summary>
        ///     Determines whether the specified redistributable package is installed.
        /// </summary>
        /// <param name="keys">
        ///     The redistributable package keys to check.
        /// </param>
        public static bool IsInstalled(params RedistFlags[] keys)
        {
            try
            {
                keys = Enum.GetValues(typeof(RedistFlags)).Cast<RedistFlags>()
                           .SelectMany(_ => keys, (item, key) => (item, key))
                           .Where(type => (type.item & type.key) != 0)
                           .Select(type => type.item).ToArray();
                var result = false;
                var names = GetDisplayNames();
                foreach (var key in keys.Select(x => x.ToString()))
                {
                    var year = key.Substring(2, 4);
                    var arch = key.Substring(6);
                    Recheck:
                    result = year switch
                    {
                        "2005" => names.Any(x => x.Contains(year) && ((arch.EqualsEx("x64") && x.ContainsEx(arch)) || !x.ContainsEx("x64"))),
                        _ => names.Any(x => x.Contains(year) && x.ContainsEx(arch)),
                    };
                    if (result)
                        continue;
                    switch (year)
                    {
                        case "2005":
                        case "2008":
                        case "2010":
                        case "2012":
                        case "2013":
                            break;

                        // I know, I know... GOTO is evil shit! 
                        case "2015":
                            year = "2017";
                            goto Recheck;
                        case "2017":
                            year = "2019";
                            goto Recheck;
                    }
                    break;
                }
                return result;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
        }
    }
}
