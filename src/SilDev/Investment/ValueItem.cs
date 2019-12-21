#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ValueItem.cs
// Version:  2019-12-21 22:49
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Investment
{
    using System;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Text;

    /// <summary>
    ///     Defines a value type that can store and retrieve data.
    /// </summary>
    /// <typeparam name="TValue">
    ///     The type of the value.
    /// </typeparam>
    [Serializable]
    public class ValueItem<TValue> : ISerializable where TValue : IEquatable<TValue>
    {
        [NonSerialized]
        private TValue _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueItem{TValue}"/> with the specified
        ///     value, default value, minimum value and maximum value.
        /// </summary>
        /// <param name="value">
        ///     The value to be set.
        /// </param>
        /// <param name="defValue">
        ///     The value used as default.
        /// </param>
        /// <param name="minValue">
        ///     The minimum value. Must be smaller than the maximum value.
        /// </param>
        /// <param name="maxValue">
        ///     The maximum value. Must be larger than the minimum value.
        /// </param>
        /// <param name="getValidationFunc">
        ///     The method that is called when <see cref="Value"/> is get.
        ///     <para>
        ///         Please note that <see cref="Func{T, TResult}"/> methods cannot be serialized.
        ///     </para>
        /// </param>
        /// <param name="setValidationFunc">
        ///     The method that is called when <see cref="Value"/> is set.
        ///     <para>
        ///         Please note that <see cref="Func{T, TResult}"/> methods cannot be serialized.
        ///     </para>
        /// </param>
        public ValueItem(TValue value, TValue defValue, TValue minValue, TValue maxValue, Func<TValue, TValue> getValidationFunc = default, Func<TValue, TValue> setValidationFunc = default)
        {
            ValueGetValidationFunc = getValidationFunc;
            ValueSetValidationFunc = setValidationFunc;
            if (IsMinMaxValidationType(typeof(TValue)))
            {
                var minVal = (dynamic)minValue;
                var maxVal = (dynamic)maxValue;
                if (minVal == maxVal)
                {
                    MinValue = GetMinValue(typeof(TValue));
                    MaxValue = GetMaxValue(typeof(TValue));
                }
                else
                {
                    MinMaxValidation = true;
                    MinValue = minVal < maxVal ? minVal : maxVal;
                    MaxValue = minVal > maxVal ? minVal : maxVal;
                }
            }
            DefValue = defValue;
            Value = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueItem{TValue}"/> with the specified
        ///     value and default value.
        /// </summary>
        /// <param name="value">
        ///     The value to be set.
        /// </param>
        /// <param name="defValue">
        ///     The value used as default.
        /// </param>
        /// <param name="getValidationFunc">
        ///     The method that is called when <see cref="Value"/> is get.
        ///     <para>
        ///         Please note that <see cref="Func{T, TResult}"/> methods cannot be serialized.
        ///     </para>
        /// </param>
        /// <param name="setValidationFunc">
        ///     The method that is called when <see cref="Value"/> is set.
        ///     <para>
        ///         Please note that <see cref="Func{T, TResult}"/> methods cannot be serialized.
        ///     </para>
        /// </param>
        public ValueItem(TValue value, TValue defValue = default, Func<TValue, TValue> getValidationFunc = default, Func<TValue, TValue> setValidationFunc = default) : this(value, defValue, default, default, getValidationFunc, setValidationFunc) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueItem{TValue}"/> class with
        ///     serialized data.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        protected ValueItem(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (Log.DebugMode > 1)
                Log.Write($"{nameof(ValueItem<TValue>)}.ctor({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");

            MinMaxValidation = info.GetBoolean(nameof(MinMaxValidation));
            MinValue = (TValue)info.GetValue(nameof(MinValue), typeof(TValue));
            MaxValue = (TValue)info.GetValue(nameof(MaxValue), typeof(TValue));
            DefValue = (TValue)info.GetValue(nameof(DefValue), typeof(TValue));
            Value = (TValue)info.GetValue(nameof(Value), typeof(TValue));
        }

        /// <summary>
        ///     Gets the value that determines whether <see cref="Value"/> is validated by
        ///     <see cref="MinValue"/> and <see cref="MaxValue"/>.
        /// </summary>
        protected bool MinMaxValidation { get; }

        /// <summary>
        ///     Gets the minimum value, if available; otherwise, the default value is returned.
        /// </summary>
        public TValue MinValue { get; }

        /// <summary>
        ///     Gets the maximum value, if available; otherwise, the default value is returned.
        /// </summary>
        public TValue MaxValue { get; }

        /// <summary>
        ///     Gets the default value.
        /// </summary>
        public TValue DefValue { get; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public TValue Value
        {
            get => ValidateGetValue(_value);
            set => _value = ValidateSetValue(value);
        }

        /// <summary>
        ///     Gets the method that is called when <see cref="Value"/> is get, if available;
        ///     otherwise, the default value is returned.
        ///     <para>
        ///         Please note that <see cref="Func{T, TResult}"/> methods cannot be serialized.
        ///     </para>
        /// </summary>
        public Func<TValue, TValue> ValueGetValidationFunc { get; set; }

        /// <summary>
        ///     Gets the method that is called when <see cref="Value"/> is set, if available;
        ///     otherwise, the default value is returned.
        ///     <para>
        ///         Please note that <see cref="Func{T, TResult}"/> methods cannot be serialized.
        ///     </para>
        /// </summary>
        public Func<TValue, TValue> ValueSetValidationFunc { get; set; }

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
                Log.Write($"{nameof(ValueItem<TValue>)}.get({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");

            info.AddValue(nameof(MinMaxValidation), MinMaxValidation);
            info.AddValue(nameof(MinValue), MinValue);
            info.AddValue(nameof(MaxValue), MaxValue);
            info.AddValue(nameof(DefValue), DefValue);
            info.AddValue(nameof(Value), Value);
        }

        private static bool IsMinMaxValidationType(Type type) =>
            ((int)Type.GetTypeCode(type)).IsBetween(4, 16);

        private static TValue GetMinValue(Type type)
        {
            if (!IsMinMaxValidationType(type))
                return default;
            if (Type.GetTypeCode(type) == TypeCode.DateTime)
                return (TValue)(object)DateTime.MinValue;
            return (TValue)type.GetField(nameof(MinValue)).GetRawConstantValue();
        }

        private static TValue GetMaxValue(Type type)
        {
            if (!IsMinMaxValidationType(type))
                return default;
            if (Type.GetTypeCode(type) == TypeCode.DateTime)
                return (TValue)(object)DateTime.MaxValue;
            return (TValue)type.GetField(nameof(MaxValue)).GetRawConstantValue();
        }

        /// <summary>
        ///     Ensures that the specified value is valid. If <see cref="MinValue"/> and
        ///     <see cref="MaxValue"/> have been set and are valid, the specified value
        ///     is calibrated if it is not between <see cref="MinValue"/> and
        ///     <see cref="MaxValue"/>. If the value is default, it also ensures that
        ///     <see cref="DefValue"/> is returned instead.
        ///     <para>
        ///         Please note that this method is automatically used for the
        ///         <see cref="Value"/> setter and overwriting can lead to an unexpected
        ///         result when setting values.
        ///     </para>
        /// </summary>
        /// <param name="value">
        ///     The value to validate.
        /// </param>
        protected virtual TValue ValidateGetValue(TValue value)
        {
            var newValue = value;
            if (ValueGetValidationFunc != default)
                newValue = ValueGetValidationFunc(newValue);
            if ((dynamic)newValue == default(TValue))
                newValue = DefValue;
            return newValue;
        }

        /// <summary>
        ///     Ensures that the specified value is valid. If <see cref="MinValue"/> and
        ///     <see cref="MaxValue"/> have been set and are valid, the specified value
        ///     is calibrated if it is not between <see cref="MinValue"/> and
        ///     <see cref="MaxValue"/>. If the value is default, it also ensures that
        ///     <see cref="DefValue"/> is returned instead.
        ///     <para>
        ///         Please note that this method is automatically used for the
        ///         <see cref="Value"/> setter and overwriting can lead to an unexpected
        ///         result when setting values.
        ///     </para>
        /// </summary>
        /// <param name="value">
        ///     The value to validate.
        /// </param>
        protected virtual TValue ValidateSetValue(TValue value)
        {
            var newValue = value;
            if (MinMaxValidation)
            {
                if ((dynamic)newValue < MinValue)
                    newValue = MinValue;
                if ((dynamic)newValue > MaxValue)
                    newValue = MaxValue;
            }
            if (ValueSetValidationFunc != default)
                newValue = ValueSetValidationFunc(newValue);
            if ((dynamic)newValue == default(TValue))
                newValue = DefValue;
            return newValue;
        }

        /// <summary>
        ///     Determines whether this instance has the same values as another.
        /// </summary>
        /// <param name="other">
        ///     The other <see cref="ValueItem{TValue}"/> instance to compare.
        /// </param>
        public bool Equals(ValueItem<TValue> other)
        {
            var obj = (object)other;
            if (obj == null)
                return false;
            return GetHashCode(true) == other.GetHashCode(true);
        }

        /// <summary>
        ///     Determines whether this instance has the same values as another.
        /// </summary>
        /// <param name="other">
        ///     The other <see cref="ValueItem{TValue}"/> instance object to compare.
        /// </param>
        public override bool Equals(object other) =>
            Equals(other as ValueItem<TValue>);

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <param name="nonReadOnly">
        ///     true to include the hashes of non-readonly properties; otherwise, false.
        /// </param>
        public int GetHashCode(bool nonReadOnly) =>
            Crypto.GetClassHashCode(this, nonReadOnly);

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode() =>
            Crypto.GetClassHashCode(this);

        /// <summary>
        ///     Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{");
            var current = this;
            var properties = current.GetType().GetProperties();
            var first = false;
            foreach (var pi in properties)
            {
                var name = pi.Name;
                if (!MinMaxValidation && (name == nameof(MinValue) || name == nameof(MaxValue)))
                    continue;
                var value = pi.GetValue(current);
                if (value == null)
                    continue;
                if (first)
                    builder.Append(',');
                else
                    first = true;
                builder.AppendFormat(CultureConfig.GlobalCultureInfo, "{0}={1}", name, value);
            }
            builder.Append("}");
            return builder.ToStringThenClear();
        }

        /// <summary>
        ///     Determines whether two specified <see cref="ValueItem{TValue}"/> instances
        ///     have same values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="ValueItem{TValue}"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="ValueItem{TValue}"/> instance to compare.
        /// </param>
        public static bool operator ==(ValueItem<TValue> left, ValueItem<TValue> right)
        {
            var obj = (object)left;
            if (obj != null)
                return left.Equals(right);
            obj = right;
            return obj == null;
        }

        /// <summary>
        ///     Determines whether two specified <see cref="ValueItem{TValue}"/> instances
        ///     have different values.
        /// </summary>
        /// <param name="left">
        ///     The first <see cref="ValueItem{TValue}"/> instance to compare.
        /// </param>
        /// <param name="right">
        ///     The second <see cref="ValueItem{TValue}"/> instance to compare.
        /// </param>
        public static bool operator !=(ValueItem<TValue> left, ValueItem<TValue> right) =>
            !(left == right);
    }
}
