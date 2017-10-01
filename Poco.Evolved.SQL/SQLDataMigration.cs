using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core;

using Poco.Evolved.SQL.Transactions;

namespace Poco.Evolved.SQL
{
    /// <summary>
    /// Base class for data migrations on a SQL database.
    /// </summary>
    public abstract class SQLDataMigration : AbstractDataMigration<SQLUnitOfWork>
    {
        /// <summary>
        /// Constructs a new <see cref="SQLDataMigration" />.
        /// </summary>
        public SQLDataMigration() : base() { }
    }
}
