#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: CounterInvestor.cs
// Version:  2020-01-24 20:58
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Investment
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     A base class that provides numerical values that are used as counters.
    /// </summary>
    /// <typeparam name="TCounter">
    ///     The type of the counters.
    /// </typeparam>
    public class CounterInvestor<TCounter> where TCounter : struct, IComparable, IFormattable, IConvertible, IComparable<TCounter>, IEquatable<TCounter>
    {
        private readonly Dictionary<int, TCounter> _counter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CounterInvestor{TNumber}"/>
        ///     class.
        ///     <para>
        ///         Allowed types: <see cref="char"/>, <see cref="sbyte"/>,
        ///         <see cref="byte"/>, <see cref="short"/>, <see cref="ushort"/>,
        ///         <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>,
        ///         <see cref="ulong"/>, and <see cref="decimal"/>.
        ///     </para>
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///     The generic type is not char, sbyte, byte, short, ushort, int, uint, long,
        ///     ulong, or decimal.
        /// </exception>
        public CounterInvestor()
        {
            var type = typeof(TCounter);
            if (!IsValid(type))
                throw new NotSupportedException($"The type '{type}' is not supported.");
            _counter = new Dictionary<int, TCounter>();
        }

        /// <summary>
        ///     Increases the value of the counter associated with the specified index.
        /// </summary>
        /// <param name="index">
        ///     The index of the counter.
        /// </param>
        public TCounter Increase(int index) =>
            Handler(index, true);

        /// <summary>
        ///     Resets the value of the counter associated with the specified index.
        /// </summary>
        /// <param name="index">
        ///     The index of the counter.
        /// </param>
        public TCounter Reset(int index) =>
            Handler(index, false);

        /// <summary>
        ///     Gets the value of the counter associated with the specified index.
        /// </summary>
        /// <param name="index">
        ///     The index of the counter.
        /// </param>
        public TCounter GetValue(int index) =>
            Handler(index, null);

        private static bool IsValid(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsAddable(TCounter source)
        {
            var type = typeof(TCounter);
            var maxValue = IsValid(type) ? (TCounter)type.GetField(nameof(int.MaxValue)).GetRawConstantValue() : default;
            return (dynamic)source < maxValue;
        }

        private TCounter Handler(int index, bool? state)
        {
            if (!_counter.ContainsKey(index))
                _counter.Add(index, default);
            switch (state)
            {
                case true:
                    if (IsAddable(_counter[index]))
                        _counter[index] = (dynamic)_counter[index] + 1;
                    break;
                case false:
                    _counter[index] = default;
                    break;
            }
            return _counter[index];
        }
    }
}
