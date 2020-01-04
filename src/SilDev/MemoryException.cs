#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: MemoryException.cs
// Version:  2020-01-04 14:01
// 
// Copyright (c) 2020, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Runtime.Serialization;
    using System.Security;
    using Properties;

    /// <summary>
    ///     The exception that is thrown when an attempt to access some data in memory fails.
    /// </summary>
    [Serializable]
    public class MemoryException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryException"/> class.
        /// </summary>
        public MemoryException() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryException"/> class
        ///     with a specified error message.
        /// </summary>
        /// <param name="message">
        ///     The message that describes the error.
        /// </param>
        public MemoryException(string message) : base(message) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryException"/> class
        ///     with a specified error message and the exception that is the cause of
        ///     this exception.
        /// </summary>
        /// <param name="message">
        ///     The message that describes the error.
        /// </param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference.
        /// </param>
        public MemoryException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArgumentInvalidException"/> class
        ///     with serialized data.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        protected MemoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Gets a message that describes the current exception.
        /// </summary>
        public sealed override string Message { get; } = ExceptionMessages.MemoryAccess;

        /// <summary>
        ///     Sets the <see cref="SerializationInfo"/> object with the additional exception
        ///     information.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        [SecurityCritical]
        public new virtual void GetObjectData(SerializationInfo info, StreamingContext context) =>
            base.GetObjectData(info, context);
    }
}
