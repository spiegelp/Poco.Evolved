using System;
using System.Collections.Generic;
using System.Text;

#if NETSTANDARD2_0 || NET45
using System.Runtime.Serialization;
#endif

namespace Poco.Evolved.Core.Exceptions
{
    /// <summary>
    /// An exception to inidcate errors during the initialization of Poco.Evolved for the database.
    /// </summary>
    public class InitializationFailedException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="InitializationFailedException"/>.
        /// </summary>
        public InitializationFailedException() : base() { }

        /// <summary>
        /// Constructs a new <see cref="InitializationFailedException"/>.
        /// </summary>
        /// <param name="message">The exception message</param>
        public InitializationFailedException(string message) : base(message) { }

        /// <summary>
        /// Constructs a new <see cref="InitializationFailedException"/>.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">Optional inner exception for the cause</param>
        public InitializationFailedException(string message, Exception innerException) : base(message, innerException) { }

#if NETSTANDARD2_0 || NET45
        /// <summary>
        /// Constructs a new <see cref="InitializationFailedException"/>.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public InitializationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
