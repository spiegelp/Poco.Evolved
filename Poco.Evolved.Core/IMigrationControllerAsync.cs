using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Poco.Evolved.Core
{
    /// <summary>
    /// Interface with async methods for migration controllers.
    /// </summary>
    public interface IMigrationControllerAsync
    {
        /// <summary>
        /// Applies the open data migrations.
        /// </summary>
        Task ApplyMigrationsAsync();
    }
}
