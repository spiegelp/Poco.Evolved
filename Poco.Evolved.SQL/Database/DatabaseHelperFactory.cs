using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core.Database;

using Poco.Evolved.SQL.Transactions;

namespace Poco.Evolved.SQL.Database
{
    /// <summary>
    /// Factory for creating a <see cref="IDatabaseHelper&lt;SQLUnitOfWork&gt;" /> with a SQL dialect for a specific database.
    /// </summary>
    public abstract class DatabaseHelperFactory
    {
        /// <summary>
        /// Creates a new <see cref="IDatabaseHelper&lt;SQLUnitOfWork&gt;" /> for a specified database.
        /// </summary>
        /// <param name="databaseType">The database type for the <see cref="IDatabaseHelper&lt;SQLUnitOfWork&gt;" /></param>
        /// <param name="installedVersionsTableName">Optional name of the table for saving the information about installed versions</param>
        /// <param name="skipInitInstalledVersions">
        /// Optionally skip the CREATE TABLE statement for the table storing the information about installed versions (the table must be created manually beforehand)
        /// </param>
        /// <returns></returns>
        public static IDatabaseHelper<SQLUnitOfWork> CreateDatabaseHelper(DatabaseType databaseType = DatabaseType.Generic, string installedVersionsTableName = null,
            bool skipInitInstalledVersions = false)
        {
            switch (databaseType)
            {
                case DatabaseType.Firebird:
                    return new FirebirdDatabaseHelper(installedVersionsTableName, skipInitInstalledVersions);

                case DatabaseType.SQLite:
                    return new SQLiteDatabaseHelper(installedVersionsTableName, skipInitInstalledVersions);

                default:
                    return new SQLDatabaseHelper(installedVersionsTableName, skipInitInstalledVersions);
            }
        }
    }
}
