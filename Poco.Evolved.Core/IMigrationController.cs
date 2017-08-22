using System;
using System.Collections.Generic;
using System.Text;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// Interface for migration controllers.
    /// </summary>
    public interface IMigrationController
    {
        /// <summary>
        /// Applies the open data migrations.
        /// </summary>
        void ApplyMigrations();
    }
}
