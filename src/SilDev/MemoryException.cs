#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: MemoryException.cs
// Version:  2019-01-30 10:22
// 
// Copyright (c) 2019, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    ///     The exception that is thrown when an attempt to access some data in memory fails.
    /// </summary>
    [Serializable]
    public class MemoryException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ArgumentInvalidException"/> class.
        /// </summary>
        public MemoryException() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArgumentInvalidException"/> class
        ///     with a specified error message.
        /// </summary>
        /// <param name="message">
        ///     The message that describes the error.
        /// </param>
        public MemoryException(string message) : base(message) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArgumentInvalidException"/> class
        ///     with serialized data.
        /// </summary>
        protected MemoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        ///     Gets a message that describes the current exception.
        /// </summary>
        public sealed override string Message { get; } = "Unable to access to the specified area of the memory.";

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
