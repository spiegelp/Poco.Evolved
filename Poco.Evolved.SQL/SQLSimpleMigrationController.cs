using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core;
using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Transactions;

using Poco.Evolved.SQL.Transactions;

namespace Poco.Evolved.SQL
{
    /// <summary>
    /// SQL database specific controller for simple data migrations similar to Android's built-in SQLite.
    /// </summary>
    public abstract class SQLSimpleMigrationController : SimpleMigrationController<SQLUnitOfWork>
    {
        /// <summary>
        /// Constructs a new <see cref="SQLSimpleMigrationController" />
        /// </summary>
        /// <param name="unitOfWorkFactory">The factory for the specific unit of work</param>
        /// <param name="databaseHelper">The helper for the specific database</param>
        public SQLSimpleMigrationController(IUnitOfWorkFactory<SQLUnitOfWork> unitOfWorkFactory, IDatabaseHelper<SQLUnitOfWork> databaseHelper)
            : base(unitOfWorkFactory, databaseHelper) { }
    }
}
