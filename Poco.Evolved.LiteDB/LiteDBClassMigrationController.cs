using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Poco.Evolved.Core;
using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Model;
using Poco.Evolved.Core.Transactions;
using Poco.Evolved.LiteDB.Transactions;

namespace Poco.Evolved.LiteDB
{
    /// <summary>
    /// LiteDB specfific controller for class based data migrations.
    /// </summary>
    public class LiteDBClassMigrationController : ClassMigrationController<LiteDBUnitOfWork>
    {
        /// <summary>
        /// Constructs a new <see cref="LiteDBClassMigrationController" />.
        /// </summary>
        /// <param name="unitOfWorkFactory">The factory for the specific unit of work</param>
        /// <param name="databaseHelper">The helper for the specific database</param>
        /// <param name="assembly">The assembly to look for data migrations</param>
        public LiteDBClassMigrationController(IUnitOfWorkFactory<LiteDBUnitOfWork> unitOfWorkFactory, IDatabaseHelper<LiteDBUnitOfWork> databaseHelper, Assembly assembly = null)
            : base(unitOfWorkFactory, databaseHelper, assembly) { }
    }
}
