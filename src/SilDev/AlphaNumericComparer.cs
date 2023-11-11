#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: AlphaNumericComparer.cs
// Version:  2023-11-11 16:27
// 
// Copyright (c) 2023, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

// .NET Core version can be found at https://github.com/Roydl/AlphaNumericComparer
namespace SilDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    ///     Defines a method that performs the string transformation for the comparer.
    /// </summary>
    public interface IStringComparer
    {
        /// <summary>
        ///     When overridden in a derived class, performs the string transformation of
        ///     the object for the comparer.
        /// </summary>
        string GetString(object value);
    }

    /// <summary>
    ///     Provides a base class for alphanumeric comparison.
    /// </summary>
    [Serializable]
    public class AlphaNumericComparer : IComparer, IStringComparer
    {
        /// <summary>
        ///     Gets the value that determines whether the order is descended.
        /// </summary>
        protected bool Descendant { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer"/> class.
        ///     A parameter specifies whether the order is descended.
        /// </summary>
        /// <param name="descendant">
        ///     <see langword="true"/> to enable the descending order; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public AlphaNumericComparer(bool descendant) =>
            Descendant = descendant;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer"/> class.
        /// </summary>
        public AlphaNumericComparer() : this(false) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer"/> class
        ///     with serialized data.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        protected AlphaNumericComparer(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (Log.DebugMode > 1)
                Log.Write($"{nameof(AlphaNumericComparer)}.ctor({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");
            Descendant = info.GetBoolean(nameof(Descendant));
        }

        /// <summary>
        ///     Compare two specified objects and returns an integer that indicates their
        ///     relative position in the sort order.
        /// </summary>
        /// <param name="a">
        ///     The first object to compare.
        /// </param>
        /// <param name="b">
        ///     The second object to compare.
        /// </param>
        public int Compare(object a, object b)
        {
            var s1 = GetString(!Descendant ? a : b);
            if (s1 == null)
                return 0;
            var s2 = GetString(!Descendant ? b : a);
            if (s2 == null)
                return 0;
            try
            {
                var i1 = 0;
                var i2 = 0;
                while (i1 < s1.Length && i2 < s2.Length)
                {
                    var c1 = GetChunk(s1, ref i1);
                    var c2 = GetChunk(s2, ref i2);
                    int r;
                    if (!char.IsDigit(c1[0]) || !char.IsDigit(c2[0]))
                        r = string.Compare(c1, c2, StringComparison.CurrentCulture);
                    else
                    {
                        var n1 = int.Parse(c1, CultureInfo.CurrentCulture);
                        var n2 = int.Parse(c2, CultureInfo.CurrentCulture);
                        r = n1.CompareTo(n2);
                    }
                    if (r != 0)
                        return r;
                }
                return s1.Length - s2.Length;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                return string.Compare(s1, s2, StringComparison.CurrentCulture);
            }
        }

        /// <summary>
        ///     Sets the <see cref="SerializationInfo"/> object for this instance.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (Log.DebugMode > 1)
                Log.Write($"{nameof(AlphaNumericComparer)}.get({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");
            info.AddValue(nameof(Descendant), Descendant);
        }

        /// <summary>
        ///     Retrieves the string of the object that is used for comparison.
        /// </summary>
        /// <param name="value">
        ///     The object to compare.
        /// </param>
        public virtual string GetString(object value) =>
            value as string;

        /// <summary>
        ///     Determines whether this instance have same values as the specified
        ///     <see cref="object"/>.
        /// </summary>
        /// <param name="other">
        ///     The  <see cref="object"/> to compare.
        /// </param>
        public new virtual bool Equals(object other)
        {
            if (other is not AlphaNumericComparer comparer)
                return false;
            return Descendant != comparer.Descendant;
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        public new virtual int GetHashCode() =>
            nameof(AlphaNumericComparer).GetHashCode();

        private static string GetChunk(string str, ref int i)
        {
            var pos = 0;
            var len = str.Length;
            var ca = new char[len];
            do ca[pos++] = str[i];
            while (++i < len && char.IsDigit(str[i]) == char.IsDigit(ca[0]));
            return new string(ca);
        }
    }

    /// <summary>
    ///     Provides a base class for alphanumeric comparison.
    /// </summary>
    [Serializable]
    public class AlphaNumericComparer<T> : AlphaNumericComparer, IComparer<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer{T}"/>
        ///     class. A parameter specifies whether the order is descended.
        /// </summary>
        /// <param name="descendant">
        ///     <see langword="true"/> to enable the descending order; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public AlphaNumericComparer(bool descendant) : base(descendant) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer{T}"/>
        ///     class.
        /// </summary>
        public AlphaNumericComparer() : base(false) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaNumericComparer{T}"/>
        ///     class with serialized data.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        protected AlphaNumericComparer(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Compare two specified objects and returns an integer that indicates their
        ///     relative position in the sort order.
        /// </summary>
        /// <param name="a">
        ///     The first object to compare.
        /// </param>
        /// <param name="b">
        ///     The second object to compare.
        /// </param>
        public int Compare(T a, T b) => base.Compare(a, b);

        /// <summary>
        ///     Sets the <see cref="SerializationInfo"/> object for this instance.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        [SecurityCritical]
        public new virtual void GetObjectData(SerializationInfo info, StreamingContext context) => base.GetObjectData(info, context);

        /// <summary>
        ///     Determines whether this instance have same values as the specified
        ///     <see cref="object"/>.
        /// </summary>
        /// <param name="other">
        ///     The  <see cref="object"/> to compare.
        /// </param>
        public new virtual bool Equals(object other)
        {
            if (other is not AlphaNumericComparer<T> comparer)
                return false;
            return Descendant != comparer.Descendant;
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        public new virtual int GetHashCode() =>
            Crypto.CombineHashCodes(base.GetHashCode(), typeof(T).Name.GetHashCode());
    }
}
