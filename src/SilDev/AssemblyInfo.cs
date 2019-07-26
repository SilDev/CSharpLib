#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: AssemblyInfo.cs
// Version:  2019-07-26 05:03
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    /// <summary>
    ///     Provides information for the current assembly.
    /// </summary>
    public static class AssemblyInfo
    {
        /// <summary>
        ///     Gets company name information.
        /// </summary>
        public static string Company => GetAssembly<AssemblyCompanyAttribute>()?.Company;

        /// <summary>
        ///     Gets configuration information.
        /// </summary>
        public static string Configuration => GetAssembly<AssemblyDescriptionAttribute>()?.Description;

        /// <summary>
        ///     Gets copyright information.
        /// </summary>
        public static string Copyright => GetAssembly<AssemblyCopyrightAttribute>()?.Copyright;

        /// <summary>
        ///     Gets description information.
        /// </summary>
        public static string Description => GetAssembly<AssemblyDescriptionAttribute>()?.Description;

        /// <summary>
        ///     Gets file version information.
        /// </summary>
        public static string FileVersion => GetAssembly<AssemblyFileVersionAttribute>()?.Version;

        /// <summary>
        ///     Gets product information.
        /// </summary>
        public static string Product => GetAssembly<AssemblyProductAttribute>()?.Product;

        /// <summary>
        ///     Gets title information.
        /// </summary>
        public static string Title => GetAssembly<AssemblyTitleAttribute>()?.Title;

        /// <summary>
        ///     Gets trademark information.
        /// </summary>
        public static string Trademark => GetAssembly<AssemblyTrademarkAttribute>()?.Trademark;

        /// <summary>
        ///     Gets version information.
        /// </summary>
        public static string Version => Assembly.GetEntryAssembly()?.GetName().Version?.ToString();

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private static TSource GetAssembly<TSource>() where TSource : Attribute
        {
            try
            {
                var assembly = Attribute.GetCustomAttribute(Assembly.GetEntryAssembly(), typeof(TSource));
                return (TSource)assembly;
            }
            catch
            {
                return default(TSource);
            }
        }
    }
}
