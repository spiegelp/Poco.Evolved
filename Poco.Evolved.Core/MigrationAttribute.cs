using System;
using System.Collections.Generic;
using System.Text;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// An attribute to mark a class to be considered as a data migration.
    /// The class must implement <see cref="IDataMigration"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MigrationAttribute : Attribute
    {
        /// <summary>
        /// The version number of the data migration.
        /// </summary>
        public long VersionNumber { get; private set; }

        /// <summary>
        /// An optional description for the data migration.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// An optional checksum of the data migration.
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// Marks a class to be considered as a data migration.
        /// </summary>
        /// <param name="versionNumber">The version number of the data migration</param>
        public MigrationAttribute(long versionNumber)
        {
            VersionNumber = versionNumber;
            Description = null;
            Checksum = null;
        }
    }
}
