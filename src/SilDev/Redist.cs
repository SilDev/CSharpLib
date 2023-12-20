#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: Redist.cs
// Version:  2023-12-20 11:51
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
        VC2005X86 = 1 << 00,

        /// <summary>
        ///     Microsoft Visual C++ 2008 Redistributable Package (x86).
        /// </summary>
        VC2008X86 = 1 << 02,

        /// <summary>
        ///     Microsoft Visual C++ 2010 Redistributable Package (x86).
        /// </summary>
        VC2010X86 = 1 << 04,

        /// <summary>
        ///     Microsoft Visual C++ 2012 Redistributable Package (x86).
        /// </summary>
        VC2012X86 = 1 << 06,

        /// <summary>
        ///     Microsoft Visual C++ 2013 Redistributable Package (x86).
        /// </summary>
        VC2013X86 = 1 << 08,

        /// <summary>
        ///     Microsoft Visual C++ 2015 Redistributable Package (x86).
        /// </summary>
        VC2015X86 = 1 << 10,

        /// <summary>
        ///     Microsoft Visual C++ 2017 Redistributable Package (x86).
        /// </summary>
        VC2017X86 = 1 << 12,

        /// <summary>
        ///     Microsoft Visual C++ 2019 Redistributable Package (x86).
        /// </summary>
        VC2019X86 = 1 << 14,

        /// <summary>
        ///     Microsoft Visual C++ 2022 Redistributable Package (x86).
        /// </summary>
        VC2022X86 = 1 << 16,

#if any || x64
        /// <summary>
        ///     Microsoft Visual C++ 2005 Redistributable Package (x64).
        /// </summary>
        VC2005X64 = 1 << 01,

        /// <summary>
        ///     Microsoft Visual C++ 2008 Redistributable Package (x64).
        /// </summary>
        VC2008X64 = 1 << 03,

        /// <summary>
        ///     Microsoft Visual C++ 2010 Redistributable Package (x64).
        /// </summary>
        VC2010X64 = 1 << 05,

        /// <summary>
        ///     Microsoft Visual C++ 2012 Redistributable Package (x64).
        /// </summary>
        VC2012X64 = 1 << 07,

        /// <summary>
        ///     Microsoft Visual C++ 2013 Redistributable Package (x64).
        /// </summary>
        VC2013X64 = 1 << 09,

        /// <summary>
        ///     Microsoft Visual C++ 2015 Redistributable Package (x64).
        /// </summary>
        VC2015X64 = 1 << 11,

        /// <summary>
        ///     Microsoft Visual C++ 2017 Redistributable Package (x64).
        /// </summary>
        VC2017X64 = 1 << 13,

        /// <summary>
        ///     Microsoft Visual C++ 2019 Redistributable Package (x64).
        /// </summary>
        VC2019X64 = 1 << 15,

        /// <summary>
        ///     Microsoft Visual C++ 2022 Redistributable Package (x64).
        /// </summary>
        VC2022X64 = 1 << 17
#endif
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
        ///     <para>
        ///         &#9888; If <paramref name="keys"/> is undefined, it only determines
        ///         whether VC++ 2022 is installed. The architecture differs depending on
        ///         the process and library architecture. It should be noted that a 64-bit
        ///         process using the Any-compiled library will only be
        ///         <see langword="true"/> if both x86 and x64 are installed.
        ///     </para>
        /// </summary>
        /// <param name="keys">
        ///     The redistributable package keys to check.
        /// </param>
        public static bool IsInstalled(params RedistFlags[] keys)
        {
            try
            {
                if (keys?.Length is null or < 1)
#if any
                    keys = Environment.Is64BitProcess ? new[] { RedistFlags.VC2022X64, RedistFlags.VC2022X86 } : new[] { RedistFlags.VC2022X86 };
#elif x64
                    keys = new[] { RedistFlags.VC2022X64, RedistFlags.VC2022X86 };
#elif x86
                    keys = new[] { RedistFlags.VC2022X86 };
#endif
                var result = false;
                var names = GetDisplayNames();
                foreach (var key in keys.Extract().Select(x => x.ToString()))
                {
                    var year = key.Substring(2, 4);
                    var arch = key.Substring(6);
                    while (true)
                    {
                        result = year switch
                        {
                            "2005" => names.Any(x => x.Contains(year) && ((arch.EqualsEx("x64") && x.ContainsEx(arch)) || !x.ContainsEx("x64"))),
                            _ => names.Any(x => x.Contains(year) && x.ContainsEx(arch)),
                        };
                        if (result)
                            break;
                        switch (year)
                        {
                            // No re-check needed.
                            case "2005":
                            case "2008":
                            case "2010":
                            case "2012":
                            case "2013":
                                break;

                            // 2015-2019 must be checked again with the year changed, otherwise
                            // it will not be recognized whether the specified version is older
                            // than the version installed on the system.
                            case "2015":
                                year = "2017";
                                continue;
                            case "2017":
                                year = "2019";
                                continue;
                            case "2019":
                                year = "2022";
                                continue;
                        }
                        break;
                    }
                    if (Log.DebugMode > 1)
                        Log.Write($"Microsoft Visual C++ {year} Redistributable Package ({arch}) is {(result ? "installed" : "not installed")}.");
                    if (result)
                        continue;
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
