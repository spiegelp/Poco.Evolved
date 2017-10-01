using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core.Database;

using Poco.Evolved.SQL.Transactions;

namespace Poco.Evolved.SQL.Database
{
    /// <summary>
    /// Factory for creating a <see cref="IDatabaseHelper<SQLUnitOfWork>" /> with a SQL dialect for a specific database.
    /// </summary>
    public abstract class DatabaseHelperFactory
    {
        /// <summary>
        /// Creates a new <see cref="IDatabaseHelper<SQLUnitOfWork>" /> for a specified database.
        /// </summary>
        /// <param name="databaseType">The database type for the <see cref="IDatabaseHelper<SQLUnitOfWork>" /></param>
        /// <param name="installedVersionsTableName">Optional name of the table for saving the information about installed versions</param>
        /// <returns></returns>
        public IDatabaseHelper<SQLUnitOfWork> CreateDatabaseHelper(DatabaseType databaseType = DatabaseType.Generic, string installedVersionsTableName = null)
        {
            switch (databaseType)
            {
                case DatabaseType.Firebird:
                    return new FirebirdDatabaseHelper(installedVersionsTableName);

                case DatabaseType.SQLite:
                    return new SQLiteDatabaseHelper(installedVersionsTableName);

                default:
                    return new SQLDatabaseHelper(installedVersionsTableName);
            }
        }
    }
}
