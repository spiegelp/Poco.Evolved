using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Poco.Evolved.Core;
using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Transactions;

using Poco.Evolved.SQL.Transactions;

namespace Poco.Evolved.SQL
{
    /// <summary>
    /// SQL database specfific controller for class based data migrations.
    /// </summary>
    public class SQLClassMigrationController : ClassMigrationController<SQLUnitOfWork>
    {
        /// <summary>
        /// Constructs a new <see cref="SQLClassMigrationController" />.
        /// </summary>
        /// <param name="unitOfWorkFactory">The factory for the specific unit of work</param>
        /// <param name="databaseHelper">The helper for the specific database</param>
        /// <param name="assembly">The assembly to look for data migrations</param>
        public SQLClassMigrationController(IUnitOfWorkFactory<SQLUnitOfWork> unitOfWorkFactory, IDatabaseHelper<SQLUnitOfWork> databaseHelper, Assembly assembly = null)
            : base(unitOfWorkFactory, databaseHelper, assembly) { }
    }
}
