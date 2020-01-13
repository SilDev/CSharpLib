#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: AssemblyInfo.cs
// Version:  2020-01-13 13:02
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Reflection;

    /// <summary>
    ///     Provides information for the current assembly.
    /// </summary>
    public static class AssemblyInfo
    {
        /// <summary>
        ///     Gets company name information of the current process.
        /// </summary>
        public static string Company =>
            GetCompanyFrom(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets configuration information of the current process.
        /// </summary>
        public static string Configuration =>
            GetConfigurationFrom(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets copyright information of the current process.
        /// </summary>
        public static string Copyright =>
            GetCopyrightFrom(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets description information of the current process.
        /// </summary>
        public static string Description =>
            GetDescriptionFrom(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets file version information of the current process.
        /// </summary>
        public static string FileVersion =>
            GetFileVersionFrom(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets product information of the current process.
        /// </summary>
        public static string Product =>
            GetProductFrom(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets title information of the current process.
        /// </summary>
        public static string Title =>
            GetTitleFrom(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets trademark information of the current process.
        /// </summary>
        public static string Trademark =>
            GetTrademarkFrom(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets version information of the current process.
        /// </summary>
        public static string Version =>
            GetVersionFrom(Assembly.GetEntryAssembly());

        /// <summary>
        ///     Gets company name information from specified assembly, if available.
        /// </summary>
        /// <param name="element">
        ///     The assembly element to get the information.
        /// </param>
        public static string GetCompanyFrom(Assembly element) =>
            GetAssembly<AssemblyCompanyAttribute>(element)?.Company;

        /// <summary>
        ///     Gets configuration information from specified assembly, if available.
        /// </summary>
        /// <param name="element">
        ///     The assembly element to get the information.
        /// </param>
        public static string GetConfigurationFrom(Assembly element) =>
            GetAssembly<AssemblyDescriptionAttribute>(element)?.Description;

        /// <summary>
        ///     Gets copyright information from specified assembly, if available.
        /// </summary>
        /// <param name="element">
        ///     The assembly element to get the information.
        /// </param>
        public static string GetCopyrightFrom(Assembly element) =>
            GetAssembly<AssemblyCopyrightAttribute>(element)?.Copyright;

        /// <summary>
        ///     Gets description information from specified assembly, if available.
        /// </summary>
        /// <param name="element">
        ///     The assembly element to get the information.
        /// </param>
        public static string GetDescriptionFrom(Assembly element) =>
            GetAssembly<AssemblyDescriptionAttribute>(element)?.Description;

        /// <summary>
        ///     Gets file version information from specified assembly, if available.
        /// </summary>
        /// <param name="element">
        ///     The assembly element to get the information.
        /// </param>
        public static string GetFileVersionFrom(Assembly element) =>
            GetAssembly<AssemblyFileVersionAttribute>(element)?.Version;

        /// <summary>
        ///     Gets product information from specified assembly, if available.
        /// </summary>
        /// <param name="element">
        ///     The assembly element to get the information.
        /// </param>
        public static string GetProductFrom(Assembly element) =>
            GetAssembly<AssemblyProductAttribute>(element)?.Product;

        /// <summary>
        ///     Gets title information from specified assembly, if available.
        /// </summary>
        /// <param name="element">
        ///     The assembly element to get the information.
        /// </param>
        public static string GetTitleFrom(Assembly element) =>
            GetAssembly<AssemblyTitleAttribute>(element)?.Title;

        /// <summary>
        ///     Gets trademark information.
        /// </summary>
        public static string GetTrademarkFrom(Assembly element) =>
            GetAssembly<AssemblyTrademarkAttribute>(element)?.Trademark;

        /// <summary>
        ///     Gets version information from specified assembly, if available.
        /// </summary>
        /// <param name="element">
        ///     The assembly element to get the information.
        /// </param>
        public static string GetVersionFrom(Assembly element) =>
            element?.GetName().Version?.ToString();

        private static TSource GetAssembly<TSource>(Assembly element) where TSource : Attribute
        {
            try
            {
                if (element == null)
                    throw new ArgumentNullException(nameof(element));
                return (TSource)Attribute.GetCustomAttribute(element, typeof(TSource));
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return default;
            }
        }
    }
}
