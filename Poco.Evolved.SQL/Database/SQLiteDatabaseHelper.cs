using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core.Model;

namespace Poco.Evolved.SQL.Database
{
    /// <summary>
    /// Helper for database specific operations on a SQLite databases.
    /// </summary>
    public class SQLiteDatabaseHelper : SQLDatabaseHelper
    {
        /// <summary>
        /// true for adding WITHOUT ROWID to the create table statement
        /// </summary>
        public bool WithoutRowIdEnabled { get; set; }

        /// <summary>
        /// Constructs a new <see cref="SQLiteDatabaseHelper" />.
        /// </summary>
        /// <param name="installedVersionsTableName">Optional name of the table for saving the information about installed versions</param>
        /// <param name="withoutRowIdEnabled">true for adding WITHOUT ROWID to the create table statement</param>
        public SQLiteDatabaseHelper(string installedVersionsTableName = null, bool withoutRowIdEnabled = false)
            : base(installedVersionsTableName)
        {
            WithoutRowIdEnabled = withoutRowIdEnabled;
        }

        /// <summary>
        /// The SQL script to create the table for saving information on installed versions.
        /// </summary>
        /// <returns></returns>
        protected override string GetCreateInstalledVersionsTableScript()
        {
            return GetCreateInstalledVersionsTableScript(false);
        }

        private string GetCreateInstalledVersionsTableScript(bool addIfNotExists)
        {
            return string.Format(
                "CREATE TABLE {0} {1} ({2}, {3}, {4}, {5}, {6}) {7}",
                addIfNotExists ? "IF NOT EXISTS" : string.Empty,
                m_installedVersionsTableName,
                ConvertToNameOnDatabase(nameof(InstalledVersion.VersionNumber)) + " INTEGER NOT NULL PRIMARY KEY",
                ConvertToNameOnDatabase(nameof(InstalledVersion.Description)) + " TEXT",
                ConvertToNameOnDatabase(nameof(InstalledVersion.Installed)) + " TEXT NOT NULL",
                ConvertToNameOnDatabase(nameof(InstalledVersion.ExecutionTime)) + " INTEGER NOT NULL",
                ConvertToNameOnDatabase(nameof(InstalledVersion.Checksum)) + " TEXT",
                WithoutRowIdEnabled ? "WITHOUT ROWID" : string.Empty
            );
        }

        /// <summary>
        /// Optionally gets the CREATE TABLE script for saving information on installed versions using a check for an already existing table.
        /// </summary>
        /// <returns></returns>
        protected override string GetCreateInstalledVersionsTableIfNotExistsScript()
        {
            return GetCreateInstalledVersionsTableScript(true);
        }
    }
}
