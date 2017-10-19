using System;
using System.Collections.Generic;
using System.Text;

#if NETSTANDARD2_0 || NET45
using System.Runtime.Serialization;
#endif

namespace Poco.Evolved.Core.Exceptions
{
    /// <summary>
    /// An exception to inidcate errors during applying of data migrations.
    /// </summary>
    public class MigrationFailedException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="MigrationFailedException"/>.
        /// </summary>
        public MigrationFailedException() : base() { }

        /// <summary>
        /// Constructs a new <see cref="MigrationFailedException"/>.
        /// </summary>
        /// <param name="message">The exception message</param>
        public MigrationFailedException(string message) : base(message) { }

        /// <summary>
        /// Constructs a new <see cref="MigrationFailedException"/>.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">Optional inner exception for the cause</param>
        public MigrationFailedException(string message, Exception innerException) : base(message, innerException) { }

#if NETSTANDARD2_0 || NET45
        /// <summary>
        /// Constructs a new <see cref="MigrationFailedException"/>.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MigrationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
