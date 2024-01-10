#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: TypeEx.cs
// Version:  2024-01-10 17:28
// 
// Copyright (c) 2024, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Expands the functionality for the <see cref="Type"/> class.
    /// </summary>
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
        ///     The attributes that determine that all properties that one of these
        ///     attributes has are excluded.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     type is null.
        /// </exception>
        public static IEnumerable<PropertyInfo> GetPropertiesEx(this Type type, params Type[] excludeAttributes)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            return excludeAttributes == null
                ? type.GetProperties().Where(pi => pi.CanRead)
                : type.GetProperties().Where(pi => pi.CanRead && !excludeAttributes.Any(t => Attribute.IsDefined(pi, t, false)));
        }

        /// <summary>
        ///     Returns all readable public properties of this <see cref="Type"/>.
        /// </summary>
        /// <inheritdoc cref="GetPropertiesEx(Type,Type[])"/>
        public static IEnumerable<PropertyInfo> GetPropertiesEx(this Type type) =>
            GetPropertiesEx(type, null);

        /// <summary>
        ///     Determines whether a readable property with the specified name exists in
        ///     this <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        ///     The <see cref="Type"/> to check.
        /// </param>
        /// <param name="name">
        ///     The property name.
        /// </param>
        /// <param name="comparisonType">
        ///     Specifies how the <paramref name="name"/> will be compared.
        /// </param>
        /// <param name="excludeAttributes">
        ///     The attributes that determine that all properties that one of these
        ///     attributes has are excluded from the search.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     type or name is null.
        /// </exception>
        public static bool HasProperty(this Type type, string name, StringComparison comparisonType, params Type[] excludeAttributes)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            return type.GetPropertiesEx(excludeAttributes).Any(pi => pi.Name.Equals(name, comparisonType));
        }

        /// <inheritdoc cref="HasProperty(Type,string,StringComparison,Type[])"/>
        public static bool HasProperty(this Type type, string name, params Type[] excludeAttributes) =>
            HasProperty(type, name, StringComparison.Ordinal, excludeAttributes);

        /// <summary>
        ///     Returns the property associated with the specified name of this
        ///     <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        ///     The <see cref="Type"/> to get the property value from.
        /// </param>
        /// <param name="name">
        ///     The property name.
        /// </param>
        /// <param name="comparisonType">
        ///     Specifies how the <paramref name="name"/> will be compared.
        /// </param>
        /// <param name="excludeAttributes">
        ///     The attributes that determine that all properties that one of these
        ///     attributes has are excluded from the search.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     type or name is null.
        /// </exception>
        public static PropertyInfo GetProperty(this Type type, string name, StringComparison comparisonType, params Type[] excludeAttributes)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            return type.GetPropertiesEx(excludeAttributes)?.FirstOrDefault(pi => pi.Name.Equals(name, comparisonType));
        }

        /// <inheritdoc cref="GetProperty(Type,string,StringComparison,Type[])"/>
        public static PropertyInfo GetProperty(this Type type, string name, params Type[] excludeAttributes) =>
            GetProperty(type, name, StringComparison.Ordinal, excludeAttributes);

        /// <summary>
        ///     Returns the value of the specified property of this instance.
        /// </summary>
        /// <typeparam name="TInstance">
        ///     The instance type.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="instance">
        ///     The instance to get the property value from.
        /// </param>
        /// <param name="name">
        ///     The property name.
        /// </param>
        /// <param name="comparisonType">
        ///     Specifies how the <paramref name="name"/> will be compared.
        /// </param>
        /// <param name="defValue">
        ///     The value returned if no property value was found.
        /// </param>
        /// <param name="excludeAttributes">
        ///     The attributes that determine that all properties that one of these
        ///     attributes has are excluded from the search.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     type or name is null.
        /// </exception>
        public static TValue GetPropertyValue<TInstance, TValue>(this TInstance instance, string name, StringComparison comparisonType, TValue defValue = default, params Type[] excludeAttributes)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (instance.GetType().GetProperty(name, comparisonType, excludeAttributes)?.GetValue(instance) is TValue value)
                return value;
            return defValue;
        }

        /// <inheritdoc cref="GetPropertyValue{TInstance,TValue}(TInstance,string,StringComparison,TValue,Type[])"/>
        public static TValue GetPropertyValue<TInstance, TValue>(this TInstance instance, string name, TValue defValue = default, params Type[] excludeAttributes) =>
            GetPropertyValue(instance, name, StringComparison.Ordinal, defValue, excludeAttributes);

        /// <summary>
        ///     Sets the specified value of the specified property of this instance.
        /// </summary>
        /// <typeparam name="TInstance">
        ///     The instance type.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The value type.
        /// </typeparam>
        /// <param name="instance">
        ///     The instance to get the property value from.
        /// </param>
        /// <param name="name">
        ///     The property name.
        /// </param>
        /// <param name="comparisonType">
        ///     Specifies how the <paramref name="name"/> will be compared.
        /// </param>
        /// <param name="value">
        ///     The value to be set.
        /// </param>
        /// <param name="excludeAttributes">
        ///     The attributes that determine that all properties that one of these
        ///     attributes has are excluded from the search.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     type or name is null.
        /// </exception>
        public static bool SetPropertyValue<TInstance, TValue>(this TInstance instance, string name, StringComparison comparisonType, TValue value, params Type[] excludeAttributes)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (instance.GetType().GetProperty(name, comparisonType, excludeAttributes) is not { } pi)
                return false;
            pi.SetValue(instance, value);
            return true;
        }

        /// <inheritdoc cref="SetPropertyValue{TInstance,TValue}(TInstance,string,StringComparison,TValue,Type[])"/>
        public static bool SetPropertyValue<TInstance, TValue>(this TInstance instance, string name, TValue value, params Type[] excludeAttributes) =>
            SetPropertyValue(instance, name, StringComparison.Ordinal, value, excludeAttributes);
    }
}
