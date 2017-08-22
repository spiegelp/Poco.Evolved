using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// Base controller for simple data migrations similar to Android's built in SQLite.
    /// </summary>
    public abstract class SimpleMigrationController : AbstractMigrationController
    {
        /// <summary>
        /// The current version number of the data model.
        /// </summary>
        public abstract int CurrentVersionNumber { get; }

        public SimpleMigrationController() { }

        /// <summary>
        /// Applies the open data migrations.
        /// </summary>
        public override void ApplyMigrations()
        {
            // find the latest version installed on the database
            List<InstalledVersion> installedVersions = GetInstalledVersionsSorted();

            int versionNumberOnDatabase = 0;

            if (installedVersions.Any())
            {
                versionNumberOnDatabase = (int)installedVersions.Last().VersionNumber;
            }

            // apply the open migrations
            for (int versionNumber = versionNumberOnDatabase; versionNumberOnDatabase <= CurrentVersionNumber; versionNumber++)
            {
                try
                {
                    ApplyMigration(versionNumber, out string description);

                    SaveInstalledVersion(new InstalledVersion() { VersionNumber = versionNumber, Description = description, Installed = DateTimeOffset.UtcNow });
                }
                catch (MigrationFailedException)
                {
                    throw;
                }
                catch (Exception exc)
                {
                    throw new MigrationFailedException("Migration to version number " + versionNumber + " failed. See inner exception for details.", exc);
                }
            }
        }

        /// <summary>
        /// Applies the data migration for the specified version number.
        /// </summary>
        /// <param name="versionNumber">The version number of the migration for applying</param>
        public abstract void ApplyMigration(int versionNumber, out string description);
    }
}
