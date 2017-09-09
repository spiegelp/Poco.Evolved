using System;
using System.Collections.Generic;
using System.Text;

using LiteDB;

using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Model;

using Poco.Evolved.LiteDB.Transactions;

namespace Poco.Evolved.LiteDB.Database
{
    /// <summary>
    /// Helper for database specific operations on a LiteDb database.
    /// </summary>
    public class LiteDBDatabaseHelper : IDatabaseHelper<LiteDBUnitOfWork>
    {
        /// <summary>
        /// The default name of the collection for saving the information about installed versions.
        /// </summary>
        private const string DefaultInstalledVersionsCollectionName = "InstalledVersions";

        private readonly string m_installedVersionsCollectionName;

        /// <summary>
        /// Constructs a new <see cref="LiteDBDatabaseHelper"/>.
        /// </summary>
        /// <param name="installedVersionsCollectionName">Optional name of the collection for saving the information about installed versions</param>
        public LiteDBDatabaseHelper(string installedVersionsCollectionName = null)
        {
            m_installedVersionsCollectionName = string.IsNullOrWhiteSpace(installedVersionsCollectionName)
                ? installedVersionsCollectionName : DefaultInstalledVersionsCollectionName;

            BsonMapper.Global.Entity<InstalledVersion>()
                .Field(v => v.VersionNumber, nameof(InstalledVersion.VersionNumber))
                .Id(v => v.VersionNumber, false)
                .Field(v => v.Description, nameof(InstalledVersion.Description))
                .Ignore(v => v.Installed)
                .Field(v => v.InstalledString, nameof(InstalledVersion.Installed))
                .Field(v => v.ExecutionTime, nameof(InstalledVersion.ExecutionTime))
                .Field(v => v.Checksum, nameof(InstalledVersion.Checksum));
        }

        /// <summary>
        /// Not implemented because LiteDB will initialize the collection on the first access.
        /// </summary>
        /// <param name="unitOfWork">The unit of work to work with</param>
        public void InitInstalledVersions(LiteDBUnitOfWork unitOfWork) { }

        /// <summary>
        /// Saves the information about an installed data migration.
        /// </summary>
        /// <param name="installedVersion">The unit of work to work with</param>
        public void SaveInstalledVersion(LiteDBUnitOfWork unitOfWork, InstalledVersion installedVersion)
        {
            unitOfWork.LiteRepository.Insert(installedVersion, m_installedVersionsCollectionName);
        }

        /// <summary>
        /// Gets all installed versions of data migrations installed on the database.
        /// </summary>
        /// <param name="unitOfWork">The unit of work to work with</param>
        /// <returns></returns>
        public IEnumerable<InstalledVersion> GetInstalledVersions(LiteDBUnitOfWork unitOfWork)
        {
            return unitOfWork.LiteRepository.Fetch<InstalledVersion>(collectionName: m_installedVersionsCollectionName);
        }
    }
}
