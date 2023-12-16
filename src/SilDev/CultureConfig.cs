#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: CultureConfig.cs
// Version:  2023-12-16 18:12
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    /// <summary>
    ///     Provides information about a specific culture (called a locale for
    ///     unmanaged code development) that is used in all related library functions.
    /// </summary>
    public static class CultureConfig
    {
        private static volatile object _syncObject;
        private static volatile CultureInfo _globalCultureInfo = CultureInfo.InvariantCulture;
        private static volatile StringComparison _globalStringComparison = StringComparison.Ordinal;
        private static volatile StringComparison _globalStringComparisonIgnoreCase = StringComparison.OrdinalIgnoreCase;

        /// <summary>
        ///     Gets or sets the default <see cref="CultureInfo"/> object value.
        ///     <para>
        ///         Default: <see cref="CultureInfo.InvariantCulture"/>
        ///     </para>
        /// </summary>
        public static CultureInfo GlobalCultureInfo
        {
            get => _globalCultureInfo;
            set
            {
                lock (SyncObject)
                    _globalCultureInfo = value ?? CultureInfo.InvariantCulture;
            }
        }

        /// <summary>
        ///     Gets or sets the default <see cref="StringComparison"/> value. (Please note
        ///     that the "Ignore case" statement will be removed.)
        ///     <para>
        ///         Default: <see cref="StringComparison.Ordinal"/>
        ///     </para>
        /// </summary>
        public static StringComparison GlobalStringComparison
        {
            get => _globalStringComparison;
            set
            {
                lock (SyncObject)
                    _globalStringComparison = value switch
                    {
                        StringComparison.InvariantCulture => StringComparison.InvariantCulture,
                        StringComparison.InvariantCultureIgnoreCase => StringComparison.InvariantCulture,
                        StringComparison.Ordinal => StringComparison.Ordinal,
                        StringComparison.OrdinalIgnoreCase => StringComparison.Ordinal,
                        _ => StringComparison.CurrentCulture
                    };
            }
        }

        /// <summary>
        ///     Gets or sets the default <see cref="StringComparison"/> value. (Please note
        ///     that the "ignore case" statement is always used.)
        ///     <para>
        ///         Default: <see cref="StringComparison.OrdinalIgnoreCase"/>
        ///     </para>
        /// </summary>
        public static StringComparison GlobalStringComparisonIgnoreCase
        {
            get => _globalStringComparisonIgnoreCase;
            set
            {
                lock (SyncObject)
                    _globalStringComparisonIgnoreCase = value switch
                    {
                        StringComparison.InvariantCulture => StringComparison.InvariantCultureIgnoreCase,
                        StringComparison.InvariantCultureIgnoreCase => StringComparison.InvariantCultureIgnoreCase,
                        StringComparison.Ordinal => StringComparison.OrdinalIgnoreCase,
                        StringComparison.OrdinalIgnoreCase => StringComparison.OrdinalIgnoreCase,
                        _ => StringComparison.CurrentCultureIgnoreCase
                    };
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

        /// <summary>
        ///     Gets the <see cref="CultureInfo"/> object of the specified language.
        /// </summary>
        /// <param name="englishName">
        ///     The culture name in English.
        /// </param>
        public static CultureInfo GetCultureInfo(string englishName)
        {
            if (string.IsNullOrWhiteSpace(englishName))
                return CultureInfo.CurrentCulture;
            englishName = englishName switch
            {
                "EnglishGB" => "English (United Kingdom)",
                "PortugueseBR" => "Portuguese (Brazil)",
                "Portuguese" => "Portuguese (Portugal)",
                "SerbianLatin" => "Serbian (Latin)",
                "SpanishInternational" => "Spanish (Spain, International Sort)",
                "Spanish" => "Spanish (Spain, International Sort)",
                "Farsi" => "Persian",
                "SimpChinese" => "Chinese (Simplified, China)",
                "TradChinese" => "Chinese (Traditional)",
                _ => englishName
            };
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            return cultures.FirstOrDefault(ci => ci.EnglishName.EqualsEx(englishName)) ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        ///     Gets the <see cref="CultureInfo"/> object of the specified language
        ///     identify code.
        /// </summary>
        /// <param name="code">
        ///     A number that identifies the culture.
        /// </param>
        public static CultureInfo GetCultureInfo(int code)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            return cultures.FirstOrDefault(ci => ci.TextInfo.LCID == code) ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        ///     Gets the identify code of the specified language.
        /// </summary>
        /// <param name="englishName">
        ///     The culture name in English.
        /// </param>
        public static int GetCultureId(string englishName) =>
            GetCultureInfo(englishName).TextInfo.LCID;
    }
}
