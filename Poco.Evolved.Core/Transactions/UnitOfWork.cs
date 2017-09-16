using System;
using System.Collections.Generic;
using System.Text;

namespace Poco.Evolved.Core.Transactions
{
    /// <summary>
    /// Base class for an unit of work to abstract a transaction.
    /// </summary>
    /// <typeparam name="T">The specific type of the abstracted transaction needed by the API of the database</typeparam>
    public abstract class UnitOfWork<T> : IUnitOfWork
    {
        /// <summary>
        /// The specific transaction of the database API.
        /// </summary>
        public T Transaction { get; protected set; }

        /// <summary>
        /// Constructs a new <see cref="UnitOfWork<T>"/>.
        /// </summary>
        public UnitOfWork() { }

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        public abstract void BeginTransaction();

        /// <summary>
        /// Disposes the transaction to release the resources.
        /// </summary>
        public abstract void DisposeTransaction();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public abstract void Commit();

        /// <summary>
        /// Reverts the transaction.
        /// </summary>
        public abstract void Rollback();

        /// <summary>
        /// Disposes this unit of work.
        /// </summary>
        public void Dispose()
        {
            DisposeTransaction();
        }
    }
}
