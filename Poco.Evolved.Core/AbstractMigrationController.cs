using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// Controller with basic behavior and logic for data migrations.
    /// </summary>
    public abstract class AbstractMigrationController : IMigrationController
    {
        public AbstractMigrationController() { }

        /// <summary>
        /// Applies the open data migrations.
        /// </summary>
        public abstract void ApplyMigrations();

        /// <summary>
        /// Gets all installed versions of data migrations installed on the database.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<InstalledVersion> GetInstalledVersions();

        /// <summary>
        /// Gets all installed versions of data migrations installed on the database ordered ascendingly by the version number.
        /// </summary>
        /// <returns></returns>
        protected List<InstalledVersion> GetInstalledVersionsSorted()
        {
            return GetInstalledVersions().OrderBy(installedVersion => installedVersion.VersionNumber).ToList();
        }

        /// <summary>
        /// Save the information about an installed data migration.
        /// </summary>
        /// <param name="installedVersion"></param>
        protected abstract void SaveInstalledVersion(InstalledVersion installedVersion);
    }
}
