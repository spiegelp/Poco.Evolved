using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core.Model;
using Poco.Evolved.Core.Transactions;

namespace Poco.Evolved.Core.Database
{
    /// <summary>
    /// Helper for database specific operations such as setting up tables or saving information on installed versions.
    /// </summary>
    /// <typeparam name="T">The type of the specific <see cref="IUnitOfWork"/></typeparam>
    public interface IDatabaseHelper<T> where T : IUnitOfWork
    {
        /// <summary>
        /// Initializes the storage (e.g. table in RDMS) for saving information on installed versions.
        /// </summary>
        /// <param name="unitOfWork">The unit of work to work with</param>
        void InitInstalledVersions(T unitOfWork);

        /// <summary>
        /// Saves the information about an installed data migration.
        /// </summary>
        /// <param name="installedVersion">The unit of work to work with</param>
        void SaveInstalledVersion(T unitOfWork, InstalledVersion installedVersion);

        /// <summary>
        /// Gets all installed versions of data migrations installed on the database.
        /// </summary>
        /// <param name="unitOfWork">The unit of work to work with</param>
        /// <returns></returns>
        IEnumerable<InstalledVersion> GetInstalledVersions(T unitOfWork);
    }
}
