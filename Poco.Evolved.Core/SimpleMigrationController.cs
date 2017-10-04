using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Exceptions;
using Poco.Evolved.Core.Model;
using Poco.Evolved.Core.Transactions;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// Base controller for simple data migrations similar to Android's built-in SQLite.
    /// </summary>
    /// <typeparam name="T">The type of the specific <see cref="IUnitOfWork"/></typeparam>
    public abstract class SimpleMigrationController<T> : AbstractMigrationController<T> where T : class, IUnitOfWork
    {
        /// <summary>
        /// The current version number of the data model.
        /// </summary>
        public abstract long CurrentVersionNumber { get; }

        /// <summary>
        /// Constructs a new <see cref="SimpleMigrationController<T>"/>.
        /// </summary>
        /// <param name="unitOfWorkFactory">The factory for the specific unit of work</param>
        /// <param name="databaseHelper">The helper for the specific database</param>
        public SimpleMigrationController(IUnitOfWorkFactory<T> unitOfWorkFactory, IDatabaseHelper<T> databaseHelper) : base(unitOfWorkFactory, databaseHelper) { }

        /// <summary>
        /// Applies the open data migrations.
        /// </summary>
        public override void ApplyMigrations()
        {
            // initialize the storage of installed versions
            InitInstalledVersions();

            // find the latest version installed on the database
            List<InstalledVersion> installedVersions = GetInstalledVersionsSorted();

            long versionNumberOnDatabase = 0;

            if (installedVersions.Any())
            {
                versionNumberOnDatabase = installedVersions.Last().VersionNumber;
            }

            // apply the open migrations
            for (long versionNumber = versionNumberOnDatabase + 1; versionNumber <= CurrentVersionNumber; versionNumber++)
            {
                using (T unitOfWork = m_unitOfWorkFactory.CreateUnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        
                        Stopwatch stopwatch = Stopwatch.StartNew();

                        ApplyMigration(unitOfWork, versionNumber, out string description, out string checksum);

                        stopwatch.Stop();

                        InstalledVersion installedVersion = new InstalledVersion()
                        {
                            VersionNumber = versionNumber,
                            Description = description,
                            Installed = DateTimeOffset.UtcNow,
                            ExecutionTime = stopwatch.ElapsedMilliseconds,
                            Checksum = checksum
                        };

                        m_databaseHelper.SaveInstalledVersion(unitOfWork, installedVersion);

                        unitOfWork.Commit();
                    }
                    catch (Exception exc)
                    {
                        unitOfWork?.Rollback();

                        if (exc is MigrationFailedException)
                        {
                            throw;
                        }
                        else
                        {
                            throw new MigrationFailedException("Migration to version number " + versionNumber + " failed. See inner exception for details.", exc);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies the data migration for the specified version number.
        /// </summary>
        /// <param name="unitOfWork">The unit of work encapsulating a transaction</param>
        /// <param name="versionNumber">The version number of the migration for applying</param>
        /// <param name="description">An optional description for the data migration</param>
        /// <param name="checksum">An optional checksum for the data migration</param>
        protected abstract void ApplyMigration(T unitOfWork, long versionNumber, out string description, out string checksum);
    }
}
