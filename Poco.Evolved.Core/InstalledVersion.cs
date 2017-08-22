using System;
using System.Collections.Generic;
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

        public InstalledVersion()
        {
            VersionNumber = -1;
            Description = null;
        }
    }
}
