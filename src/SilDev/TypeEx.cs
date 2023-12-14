#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TypeEx.cs
// Version:  2023-12-14 16:00
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
    using System.Reflection;

    public static class TypeEx
    {
        /// <summary>
        ///     Returns all readable public properties of this <see cref="Type"/> where
        ///     none of the specified attributes are defined.
        /// </summary>
        /// <param name="type">
        ///     The type to retrieves the properties from.
        /// </param>
        /// <param name="excludeAttributes">
        ///     The attributes where none can be defined.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     type or excludeAttributes is null.
        /// </exception>
        public static IEnumerable<PropertyInfo> GetPropertiesEx(this Type type, params Type[] excludeAttributes)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (excludeAttributes == null)
                throw new ArgumentNullException(nameof(excludeAttributes));
            return type.GetProperties().Where(pi => pi.CanRead && !excludeAttributes.Any(a => Attribute.IsDefined(pi, a, false)));
        }

        /// <summary>
        ///     Returns all readable public properties of this <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        ///     The type to retrieves the properties from.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     type or excludeAttributes is null.
        /// </exception>
        public static IEnumerable<PropertyInfo> GetPropertiesEx(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            return type.GetProperties().Where(pi => pi.CanRead);
        }
    }
}
