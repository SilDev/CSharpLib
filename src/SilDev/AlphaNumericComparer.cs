#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: AlphaNumericComparer.cs
// Version:  2020-01-24 20:58
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    ///     Provides a base class for comparison.
    /// </summary>
    [Serializable]
    public class AlphaNumericComparer : IComparer, IComparer<object>
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
                    var c1 = s1[i1];
                    var ca1 = new char[s1.Length];
                    var l1 = 0;
                    do
                    {
                        ca1[l1++] = c1;
                        i1++;
                        if (i1 >= s1.Length)
                            break;
                        c1 = s1[i1];
                    }
                    while (char.IsDigit(c1) == char.IsDigit(ca1[0]));
                    var c2 = s2[i2];
                    var ca2 = new char[s2.Length];
                    var l2 = 0;
                    do
                    {
                        ca2[l2++] = c2;
                        i2++;
                        if (i2 >= s2.Length)
                            break;
                        c2 = s2[i2];
                    }
                    while (char.IsDigit(c2) == char.IsDigit(ca2[0]));
                    var str1 = new string(ca1);
                    var str2 = new string(ca2);
                    int r;
                    if (char.IsDigit(ca1[0]) && char.IsDigit(ca2[0]))
                    {
                        var ch1 = int.Parse(str1, CultureConfig.GlobalCultureInfo);
                        var ch2 = int.Parse(str2, CultureConfig.GlobalCultureInfo);
                        r = ch1.CompareTo(ch2);
                    }
                    else
                        r = string.Compare(str1, str2, StringComparison.InvariantCulture);
                    if (r != 0)
                        return r;
                }
                return s1.Length - s2.Length;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                return string.Compare(s1, s2, StringComparison.InvariantCulture);
            }
        }

        /// <summary>
        ///     Gets the string of the object that is used for comparison.
        /// </summary>
        /// <param name="value">
        ///     The object to compare.
        /// </param>
        protected virtual string GetString(object value) =>
            value as string;
    }
}
