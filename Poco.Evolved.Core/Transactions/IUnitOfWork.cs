using System;
using System.Collections.Generic;
using System.Text;

namespace Poco.Evolved.Core.Transactions
{
    /// <summary>
    /// Interface for an unit of work to abstract a transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Disposes the transaction to release the resources.
        /// </summary>
        void DisposeTransaction();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Reverts the transaction.
        /// </summary>
        void Rollback();
    }
}
