using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core;

using Poco.Evolved.LiteDB.Transactions;

namespace Poco.Evolved.LiteDB
{
    /// <summary>
    /// Base class for data migrations on a LiteDB database.
    /// </summary>
    public abstract class LiteDBDataMigration : AbstractDataMigration<LiteDBUnitOfWork>
    {
        /// <summary>
        /// Constructs a new <see cref="LiteDBDataMigration" />
        /// </summary>
        public LiteDBDataMigration() : base() { }
    }
}
