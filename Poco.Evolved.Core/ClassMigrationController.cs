﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Exceptions;
using Poco.Evolved.Core.Model;
using Poco.Evolved.Core.Transactions;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// Base controller for class based data migrations.
    /// </summary>
    /// <typeparam name="T">The type of the specific <see cref="IUnitOfWork"/></typeparam>
    public class ClassMigrationController<T> : AbstractMigrationController<T> where T : class, IUnitOfWork
    {
        /// <summary>
        /// The assembly to look for data migrations.
        /// </summary>
        protected readonly Assembly m_assembly;

        /// <summary>
        /// Constructs a new <see cref="ClassMigrationController&lt;T&gt;"/>.
        /// </summary>
        /// <param name="unitOfWorkFactory">The factory for the specific unit of work</param>
        /// <param name="databaseHelper">The helper for the specific database</param>
        /// <param name="assembly">The assembly to look for data migrations</param>
        public ClassMigrationController(IUnitOfWorkFactory<T> unitOfWorkFactory, IDatabaseHelper<T> databaseHelper, Assembly assembly = null)
            : base(unitOfWorkFactory, databaseHelper)
        {
            if (assembly != null)
            {
                m_assembly = assembly;
            }
            else
            {
#if NETSTANDARD1_3
                throw new ArgumentNullException(nameof(assembly) + " must not be null for .NET Standard 1.3");
#else
                // take the execution assembly, if no specific assembly to look for the data migrations is set
                m_assembly = Assembly.GetEntryAssembly();
#endif
            }
        }

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
            IEnumerable<IDataMigration<T>> migrations = GetMigrationsForAssembly()
                .Where(migration => migration.VersionNumber > versionNumberOnDatabase)
                .OrderBy(migration => migration.VersionNumber);

            foreach (IDataMigration<T> migration in migrations)
            {
                using (T unitOfWork = m_unitOfWorkFactory.CreateUnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();

                        Stopwatch stopwatch = Stopwatch.StartNew();

                        migration.UnitOfWork = unitOfWork;
                        migration.ApplyMigration();

                        stopwatch.Stop();

                        InstalledVersion installedVersion = new InstalledVersion()
                        {
                            VersionNumber = migration.VersionNumber,
                            Description = migration.Description,
                            Installed = DateTimeOffset.UtcNow,
                            ExecutionTime = stopwatch.ElapsedMilliseconds,
                            Checksum = migration.Checksum
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
                            throw new MigrationFailedException("Migration to version number " + migration.VersionNumber + " failed. See inner exception for details.", exc);
                        }
                    }
                    finally
                    {
                        migration.UnitOfWork = null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the data migrations inside the selected assembly for this controller.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<IDataMigration<T>> GetMigrationsForAssembly()
        {
            IEnumerable<IDataMigration<T>> types = null;

#if NETSTANDARD1_3
            types = m_assembly.DefinedTypes
                .Where(typeInfo => !typeInfo.IsInterface
                                    && !typeInfo.IsAbstract
                                    && typeInfo.ImplementedInterfaces.Contains(typeof(IDataMigration<T>))
                                    && typeInfo.GetCustomAttribute<MigrationAttribute>() != null
                                    && typeInfo.DeclaredConstructors.Where(ctor => ctor.GetParameters().Length == 0).Any())
                .Select(typeInfo => CreateDataMigrationForType(typeInfo));
#else
            types = m_assembly.GetTypes()
                .Where(type => !type.IsInterface
                                    && !type.IsAbstract
                                    && type.GetInterfaces().Contains(typeof(IDataMigration<T>))
                                    && type.GetCustomAttribute<MigrationAttribute>() != null
                                    && type.GetConstructor(Type.EmptyTypes) != null)
                .Select(type => CreateDataMigrationForType(type));
#endif

            return types.OrderBy(dataMigration => dataMigration.VersionNumber);
        }

#if NETSTANDARD1_3
        /// <summary>
        /// Creates the data migration object for the specified type.
        /// </summary>
        /// <param name="typeInfo">The type info of the data migration</param>
        /// <returns></returns>
        protected virtual IDataMigration<T> CreateDataMigrationForType(TypeInfo typeInfo)
        {
            return (IDataMigration<T>)typeInfo.DeclaredConstructors
                .First(ctor => ctor.GetParameters().Length == 0)
                .Invoke(new object[0]);
        }
#else
        /// <summary>
        /// Creates the data migration object for the specified type.
        /// </summary>
        /// <param name="type">The type of the data migration</param>
        /// <returns></returns>
        protected virtual IDataMigration<T> CreateDataMigrationForType(Type type)
        {
            return (IDataMigration<T>)Activator.CreateInstance(type);
        }
#endif
    }
}
