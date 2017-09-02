using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Exceptions;
using Poco.Evolved.Core.Model;
using Poco.Evolved.Core.Transactions;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// Controller with basic behavior and logic for data migrations.
    /// </summary>
    /// <typeparam name="T">The type of the specific <see cref="IUnitOfWork"/></typeparam>
    public abstract class AbstractMigrationController<T> : IMigrationController where T : class, IUnitOfWork
    {
        protected readonly IUnitOfWorkFactory<T> m_unitOfWorkFactory;
        protected readonly IDatabaseHelper<T> m_databaseHelper;

        /// <summary>
        /// Constructs a new <see cref="AbstractMigrationController"/>.
        /// </summary>
        /// <param name="unitOfWorkFactory">The factory for the specific unit of work</param>
        /// <param name="databaseHelper">The helper for the specific database</param>
        public AbstractMigrationController(IUnitOfWorkFactory<T> unitOfWorkFactory, IDatabaseHelper<T> databaseHelper)
        {
            m_unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory) + " must not be null");
            m_databaseHelper = databaseHelper ?? throw new ArgumentNullException(nameof(databaseHelper) + " must not be null");
        }

        /// <summary>
        /// Initializes the storage of installed versions.
        /// </summary>
        protected void InitInstalledVersions()
        {
            using (T unitOfWork = m_unitOfWorkFactory.CreateUnitOfWork())
            {
                try
                {
                    m_databaseHelper.InitInstalledVersions(unitOfWork);
                }
                catch (Exception exc)
                {
                    unitOfWork?.Rollback();

                    throw new InitializationFailedException("Error during initialization of Poco.Evolved for the database. See inner exception for details.", exc);
                }
            }
        }

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
    }
}
