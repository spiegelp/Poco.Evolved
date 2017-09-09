using System;
using System.Collections.Generic;
using System.Text;

using LiteDB;

using Poco.Evolved.Core.Transactions;

namespace Poco.Evolved.LiteDB.Transactions
{
    /// <summary>
    /// Factory for creating a unit of work for LiteDB.
    /// </summary>
    public class LiteDBUnitOfWorkFactory : IUnitOfWorkFactory<LiteDBUnitOfWork>
    {
        private readonly LiteRepository m_liteRepository;

        /// <summary>
        /// Constructs a new <see cref="LiteDBUnitOfWorkFactory" />.
        /// </summary>
        /// <param name="liteRepository">The LiteRepository for accessing the database</param>
        public LiteDBUnitOfWorkFactory(LiteRepository liteRepository)
        {
            m_liteRepository = liteRepository;
        }

        /// <summary>
        /// Constructs a new unit of work.
        /// </summary>
        /// <returns></returns>
        public LiteDBUnitOfWork CreateUnitOfWork()
        {
            return new LiteDBUnitOfWork(m_liteRepository);
        }
    }
}
