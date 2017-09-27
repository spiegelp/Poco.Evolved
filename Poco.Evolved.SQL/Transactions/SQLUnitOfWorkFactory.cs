using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Poco.Evolved.Core.Transactions;

namespace Poco.Evolved.SQL.Transactions
{
    /// <summary>
    /// Factory for creating a unit of work for SQL databases.
    /// </summary>
    public class SQLUnitOfWorkFactory : IUnitOfWorkFactory<SQLUnitOfWork>
    {
        private readonly IDbConnection m_connection;

        /// <summary>
        /// Constructs a new <see cref="SQLUnitOfWorkFactory" />.
        /// <param name="connection">The connection for accessing the database</param>
        /// </summary>
        public SQLUnitOfWorkFactory(IDbConnection connection)
        {
            m_connection = connection;
        }

        /// <summary>
        /// Constructs a new unit of work.
        /// </summary>
        /// <returns></returns>
        public SQLUnitOfWork CreateUnitOfWork()
        {
            return new SQLUnitOfWork(m_connection);
        }
    }
}
