#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: CounterInvestor.cs
// Version:  2018-06-14 22:14
// 
// Copyright (c) 2018, Si13n7 Developments (r)
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
        ///     Initializes a new instance of the <see cref="CounterInvestor{TNumber}"/> class.
        ///     <para>
        ///         Allowed types: <see cref="sbyte"/>, <see cref="byte"/>, <see cref="char"/>,
        ///         <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/>, <see cref="uint"/>,
        ///         <see cref="long"/>, <see cref="ulong"/>, and <see cref="decimal"/>.
        ///     </para>
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///     The generic type is not sbyte, byte, char, short, ushort, int, uint, long, ulong,
        ///     or decimal.
        /// </exception>
        public CounterInvestor()
        {
            var type = typeof(TCounter);
            if (type != typeof(sbyte) &&
                type != typeof(byte) &&
                type != typeof(char) &&
                type != typeof(short) &&
                type != typeof(ushort) &&
                type != typeof(int) &&
                type != typeof(uint) &&
                type != typeof(long) &&
                type != typeof(ulong) &&
                type != typeof(decimal))
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

        private TCounter Handler(int index, bool? state)
        {
            if (!_counter.ContainsKey(index))
                _counter.Add(index, default(TCounter));
            switch (state)
            {
                case true:
                    if (IsAddable(_counter[index]))
                        _counter[index] = (dynamic)_counter[index] + 1;
                    break;
                case false:
                    _counter[index] = default(TCounter);
                    break;
            }
            return _counter[index];
        }

        private static bool IsAddable(TCounter source)
        {
            object maxValue;
            switch (Type.GetTypeCode(typeof(TCounter)))
            {
                case TypeCode.SByte:
                    maxValue = sbyte.MaxValue;
                    break;
                case TypeCode.Byte:
                    maxValue = byte.MaxValue;
                    break;
                case TypeCode.Char:
                    maxValue = char.MaxValue;
                    break;
                case TypeCode.Int16:
                    maxValue = short.MaxValue;
                    break;
                case TypeCode.UInt16:
                    maxValue = ushort.MaxValue;
                    break;
                case TypeCode.Int32:
                    maxValue = int.MaxValue;
                    break;
                case TypeCode.UInt32:
                    maxValue = uint.MaxValue;
                    break;
                case TypeCode.Int64:
                    maxValue = long.MaxValue;
                    break;
                case TypeCode.UInt64:
                    maxValue = ulong.MaxValue;
                    break;
                case TypeCode.Decimal:
                    maxValue = decimal.MaxValue;
                    break;
                default:
                    maxValue = default(TCounter);
                    break;
            }
            return (dynamic)source < (TCounter)maxValue;
        }
    }
}
