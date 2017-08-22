using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Poco.Evolved.Core
{
    public class MigrationFailedException : Exception
    {
        public MigrationFailedException() : base() { }

        public MigrationFailedException(string message) : base(message) { }

        public MigrationFailedException(string message, Exception innerException) : base(message, innerException) { }

        public MigrationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
