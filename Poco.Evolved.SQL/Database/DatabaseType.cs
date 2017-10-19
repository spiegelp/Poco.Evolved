using System;
using System.Collections.Generic;
using System.Text;

namespace Poco.Evolved.SQL.Database
{
    /// <summary>
    /// The type of the database to use an appropriate SQL dialect.
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// A generic dialect using ANSI SQL. This dialect may not work with every database.
        /// </summary>
        Generic,

        /// <summary>
        /// A dialect for Firebird.
        /// </summary>
        Firebird,

        /// <summary>
        /// A dialect for Microsoft SQL Server. (only generic support for now)
        /// </summary>
        MSSQLServer,

        /// <summary>
        /// A dialect for MySQL. (only generic support for now)
        /// </summary>
        MySQL,

        /// <summary>
        /// A dialect for Oracle. (only generic support for now)
        /// </summary>
        Oracle,

        /// <summary>
        /// A dialect for PostgreSQL. (only generic support for now)
        /// </summary>
        PostgreSQL,

        /// <summary>
        /// A dialect for SQLite.
        /// </summary>
        SQLite
    }
}
