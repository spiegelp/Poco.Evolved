using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core.Transactions;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// Interface which all class based data migrations must implement.
    /// </summary>
    /// <typeparam name="T">The type of the specific <see cref="IUnitOfWork"/></typeparam>
    public interface IDataMigration<T> where T : class, IUnitOfWork
    {
        /// <summary>
        /// The version number of the data migration.
        /// </summary>
        long VersionNumber { get; }

        /// <summary>
        /// An optional description for the data migration.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// An optional checksum of the data migration.
        /// </summary>
        string Checksum { get; }

        /// <summary>
        /// The unit of work to work with.
        /// </summary>
        T UnitOfWork { get; set; }

        /// <summary>
        /// Applies the data migration.
        /// </summary>
        void ApplyMigration();
    }
}
