using System;
using System.Collections.Generic;
using System.Text;

using LiteDB;

using Poco.Evolved.Core.Transactions;

namespace Poco.Evolved.LiteDB.Transactions
{
    /// <summary>
    /// An unit of work (<see cref="IUnitOfWork" />) implemtended for LiteDB.
    /// </summary>
    public class LiteDBUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The LiteRepository for accessing the database.
        /// </summary>
        public LiteRepository LiteRepository { get; protected set; }

        /// <summary>
        /// Constructs a new <see cref="LiteDBUnitOfWork" />
        /// </summary>
        /// <param name="liteRepository">The LiteRepository for accessing the database</param>
        public LiteDBUnitOfWork(LiteRepository liteRepository)
        {
            LiteRepository = liteRepository;
        }

        /// <summary>
        /// Disposes the exclusive resources of this unit of work.
        /// </summary>
        public void Dispose()
        {
            // nothing to do
        }

        /// <summary>
        /// Not implemented because LiteDb v4 does not have transactions.
        /// </summary>
        public void BeginTransaction() { }

        /// <summary>
        /// Not implemented because LiteDb v4 does not have transactions.
        /// </summary>
        public void DisposeTransaction() { }

        /// <summary>
        /// Not implemented because LiteDb v4 does not have transactions.
        /// </summary>
        public void Commit() { }

        /// <summary>
        /// Not implemented because LiteDb v4 does not have transactions.
        /// </summary>
        public void Rollback() { }
    }
}
