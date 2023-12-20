#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: EnumEx.cs
// Version:  2023-12-20 11:31
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Provides static methods based on the <see cref="Enum"/> class.
    /// </summary>
    public static class EnumEx
    {
        /// <summary>
        ///     Adds a flag to specified enum.
        /// </summary>
        /// <typeparam name="TEnum">
        ///     The type of source.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="Enum"/> to change.
        /// </param>
        public static TEnum AddFlag<TEnum>(this TEnum source, TEnum value) where TEnum : Enum =>
            (TEnum)Enum.ToObject(typeof(TEnum), Convert.ToInt32(source) | Convert.ToInt32(value));

        /// <summary>
        ///     Removes a flag of the specified enum.
        /// </summary>
        /// <typeparam name="TEnum">
        ///     The type of source.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="Enum"/> to change.
        /// </param>
        public static TEnum RemoveFlag<TEnum>(this TEnum source, TEnum value) where TEnum : Enum =>
            (TEnum)Enum.ToObject(typeof(TEnum), Convert.ToInt32(source) & ~Convert.ToInt32(value));

        /// <summary>
        ///     Extracts every single value of the specified flags.
        /// </summary>
        /// <typeparam name="TEnum">
        ///     The type of source.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="Enum"/> flags to extract.
        /// </param>
        public static IEnumerable<TEnum> Extract<TEnum>(this TEnum source) where TEnum : Enum =>
            Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Where(value => source.HasFlag(value));

        /// <summary>
        ///     Extracts every single value of the specified sequence of flags.
        /// </summary>
        /// <typeparam name="TEnum">
        ///     The type of the elements of source.
        /// </typeparam>
        /// <param name="source">
        ///     The <see cref="Enum"/> sequence of flags to extract.
        /// </param>
        public static IEnumerable<TEnum> Extract<TEnum>(this IEnumerable<TEnum> source) where TEnum : Enum =>
            source.SelectMany(flag => flag.Extract());
    }
}
