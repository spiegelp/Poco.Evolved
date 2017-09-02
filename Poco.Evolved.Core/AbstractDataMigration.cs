using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Poco.Evolved.Core.Transactions;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// Base class for data migrations providing Getters for properties set in the <see cref="MigrationAttribute"/> attribute.
    /// </summary>
    /// <typeparam name="T">The type of the specific <see cref="IUnitOfWork"/></typeparam>
    public abstract class AbstractDataMigration<T> : IDataMigration<T> where T : class, IUnitOfWork
    {
        /// <summary>
        /// The version number of the data migration.
        /// </summary>
        public long VersionNumber
        {
            get
            {
                return GetMigrationAttributeProperty(attribute => attribute.VersionNumber);
            }
        }

        /// <summary>
        /// An optional description for the data migration.
        /// </summary>
        public string Description
        {
            get
            {
                return GetMigrationAttributeProperty(attribute => attribute.Description);
            }
        }

        /// <summary>
        /// An optional checksum of the data migration.
        /// </summary>
        public virtual string Checksum
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// The unit of work encapsulating a transaction to work with.
        /// </summary>
        T IDataMigration<T>.UnitOfWork { get; set; }

        /// <summary>
        /// Applies the data migration.
        /// </summary>
        public abstract void ApplyMigration();

        private P GetMigrationAttributeProperty<P>(Func<MigrationAttribute, P> func)
        {
            return func.Invoke(GetType().GetCustomAttribute<MigrationAttribute>());
        }
    }
}
