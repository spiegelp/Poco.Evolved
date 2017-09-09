using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core;
using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Model;
using Poco.Evolved.Core.Transactions;
using Poco.Evolved.LiteDB.Transactions;

namespace Poco.Evolved.LiteDB
{
    /// <summary>
    /// LiteDB specific controller for simple data migrations similar to Android's built in SQLite.
    /// </summary>
    public abstract class LiteDBSimpleMigrationController : SimpleMigrationController<LiteDBUnitOfWork>
    {
        /// <summary>
        /// Constructs a new <see cref="LiteDBSimpleMigrationController" />
        /// </summary>
        /// <param name="unitOfWorkFactory">The factory for the specific unit of work</param>
        /// <param name="databaseHelper">The helper for the specific database</param>
        public LiteDBSimpleMigrationController(IUnitOfWorkFactory<LiteDBUnitOfWork> unitOfWorkFactory, IDatabaseHelper<LiteDBUnitOfWork> databaseHelper)
            : base(unitOfWorkFactory, databaseHelper) { }
    }
}
