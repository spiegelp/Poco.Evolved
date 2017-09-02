using System;
using System.Collections.Generic;
using System.Text;

namespace Poco.Evolved.Core.Transactions
{
    /// <summary>
    /// Interface of a factory for creating a specific unit of work.
    /// </summary>
    /// <typeparam name="T">The specific type of the <see cref="IUnitOfWork"/></typeparam>
    public interface IUnitOfWorkFactory<T> where T : class, IUnitOfWork
    {
        /// <summary>
        /// Constructs a new unit of work.
        /// </summary>
        /// <returns></returns>
        T CreateUnitOfWork();
    }
}
