using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// The information about an installed data migration.
    /// </summary>
    public class InstalledVersion
    {
        /// <summary>
        /// The version number of the data migration.
        /// </summary>
        public long VersionNumber { get; set; }

        /// <summary>
        /// An optional description for the data migration.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The timestamp of the installation.
        /// </summary>
        public DateTimeOffset Installed { get; set; }

        /// <summary>
        /// A string version for <code>Installed</code>.
        /// This property is used for persistency because the different databases support date and time differently.
        /// Saving an ISO-compliant string bypasses an eventual lack in the database implementation.
        /// </summary>
        public string InstalledString
        {
            get
            {
                return Installed.ToString("O");
            }

            set
            {
                Installed = DateTimeOffset.ParseExact(value, "O", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// The execution time of the data migration
        /// </summary>
        public long ExecutionTime { get; set; }

        /// <summary>
        /// The checksum of the data migration.
        /// </summary>
        public string Checksum { get; set; }

        public InstalledVersion()
        {
            VersionNumber = -1;
            Description = null;
            Installed = DateTimeOffset.UtcNow;
            ExecutionTime = 0;
            Checksum = null;
        }
    }
}
