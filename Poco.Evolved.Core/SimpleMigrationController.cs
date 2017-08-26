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
        public abstract long CurrentVersionNumber { get; }

        public SimpleMigrationController() { }

        /// <summary>
        /// Applies the open data migrations.
        /// </summary>
        public override void ApplyMigrations()
        {
            // find the latest version installed on the database
            List<InstalledVersion> installedVersions = GetInstalledVersionsSorted();

            long versionNumberOnDatabase = 0;

            if (installedVersions.Any())
            {
                versionNumberOnDatabase = installedVersions.Last().VersionNumber;
            }

            // apply the open migrations
            for (long versionNumber = versionNumberOnDatabase; versionNumberOnDatabase <= CurrentVersionNumber; versionNumber++)
            {
                try
                {
                    DateTime start = DateTime.Now;
                    ApplyMigration(versionNumber, out string description, out string checksum);
                    long executionTime = (long)(DateTime.Now - start).TotalMilliseconds;

                    SaveInstalledVersion(new InstalledVersion()
                    {
                        VersionNumber = versionNumber,
                        Description = description,
                        Installed = DateTimeOffset.UtcNow,
                        ExecutionTime = executionTime,
                        Checksum = checksum
                    });
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
        /// <param name="description">An optional description for the data migration</param>
        /// <param name="checksum">An optional checksum for the data migration</param>
        public abstract void ApplyMigration(long versionNumber, out string description, out string checksum);
    }
}
