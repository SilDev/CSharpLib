#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: CultureConfig.cs
// Version:  2019-10-31 21:53
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    ///     Provides information about a specific culture (called a locale for unmanaged
    ///     code development) that is used in all related library functions.
    /// </summary>
    public static class CultureConfig
    {
        private static volatile CultureInfo _globalCultureInfo = CultureInfo.InvariantCulture;
        private static volatile StringComparison _globalStringComparison = StringComparison.Ordinal;
        private static volatile StringComparison _globalStringComparisonIgnoreCase = StringComparison.OrdinalIgnoreCase;
        private static volatile object _syncObject;

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
                    switch (value)
                    {
                        case StringComparison.InvariantCulture:
                        case StringComparison.InvariantCultureIgnoreCase:
                            _globalStringComparison = StringComparison.InvariantCulture;
                            break;
                        case StringComparison.Ordinal:
                        case StringComparison.OrdinalIgnoreCase:
                            _globalStringComparison = StringComparison.Ordinal;
                            break;
                        default:
                            _globalStringComparison = StringComparison.CurrentCulture;
                            break;
                    }
            }
        }

        /// <summary>
        ///     Gets or sets the default <see cref="StringComparison"/> value. (Please note that
        ///     the "ignore case" statement is always used.)
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
                    switch (value)
                    {
                        case StringComparison.InvariantCulture:
                        case StringComparison.InvariantCultureIgnoreCase:
                            _globalStringComparisonIgnoreCase = StringComparison.InvariantCultureIgnoreCase;
                            break;
                        case StringComparison.Ordinal:
                        case StringComparison.OrdinalIgnoreCase:
                            _globalStringComparisonIgnoreCase = StringComparison.OrdinalIgnoreCase;
                            break;
                        default:
                            _globalStringComparisonIgnoreCase = StringComparison.CurrentCultureIgnoreCase;
                            break;
                    }
            }
        }
    }
}
