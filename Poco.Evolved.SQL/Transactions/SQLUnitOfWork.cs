using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Poco.Evolved.Core.Transactions;

namespace Poco.Evolved.SQL.Transactions
{
    /// <summary>
    /// An unit of work (<see cref="IUnitOfWork" />) implemtended for SQL databases.
    /// </summary>
    public class SQLUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The connection for accessing the database.
        /// </summary>
        public IDbConnection Connection { get; protected set; }

        /// <summary>
        /// The database transaction for this unit of work.
        /// </summary>
        public IDbTransaction Transaction { get; protected set; }

        /// <summary>
        /// /// Constructs a new <see cref="SQLUnitOfWork" />
        /// <param name="connection">The connection for accessing the database</param>
        /// </summary>
        public SQLUnitOfWork(IDbConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Disposes the exclusive resources of this unit of work.
        /// </summary>
        public void Dispose()
        {
            DisposeTransaction();
        }

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        public virtual void BeginTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }

            Transaction = Connection.BeginTransaction();
        }

        /// <summary>
        /// Disposes the transaction to release the resources.
        /// </summary>
        public virtual void DisposeTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
            }
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public virtual void Commit()
        {
            Transaction?.Commit();
        }

        /// <summary>
        /// Reverts the transaction.
        /// </summary>
        public virtual void Rollback()
        {
            Transaction?.Rollback();
        }
    }
}
