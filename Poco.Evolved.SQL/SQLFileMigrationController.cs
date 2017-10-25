using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Poco.Evolved.Core;
using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Exceptions;
using Poco.Evolved.Core.Model;
using Poco.Evolved.Core.Transactions;

using Poco.Evolved.SQL.Transactions;

namespace Poco.Evolved.SQL
{
    /// <summary>
    /// Controller to apply data migrations via SQL files on the drive.
    /// </summary>
    public class SQLFileMigrationController : AbstractMigrationController<SQLUnitOfWork>
    {
        /// <summary>
        /// The directory containing the SQL files with the data migrations.
        /// </summary>
        private readonly string m_sqlFilesDirectory;

        /// <summary>
        /// Constructs a new <see cref="SQLFileMigrationController" />.
        /// </summary>
        /// <param name="unitOfWorkFactory">The factory for the specific unit of work</param>
        /// <param name="databaseHelper">The helper for the specific database</param>
        /// <param name="sqlFilesDirectory">The directory containing the SQL files with the data migrations</param>
        public SQLFileMigrationController(IUnitOfWorkFactory<SQLUnitOfWork> unitOfWorkFactory, IDatabaseHelper<SQLUnitOfWork> databaseHelper, string sqlFilesDirectory)
            : base(unitOfWorkFactory, databaseHelper)
        {
            if (string.IsNullOrWhiteSpace(sqlFilesDirectory))
            {
                throw new ArgumentException(nameof(sqlFilesDirectory) + " must not be null or empty");
            }

            if (!Directory.Exists(sqlFilesDirectory))
            {
                throw new ArgumentException("The directory " + sqlFilesDirectory + " does not exist");
            }

            if (!sqlFilesDirectory.EndsWith("/") && !sqlFilesDirectory.EndsWith(@"\"))
            {
                sqlFilesDirectory = sqlFilesDirectory + @"\";
            }

            m_sqlFilesDirectory = sqlFilesDirectory;
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
            IEnumerable<SQLFileMigration> migrations = GetSQLFileMigrations()
                .Where(migration => migration.VersionNumber > versionNumberOnDatabase)
                .OrderBy(migration => migration.VersionNumber);

            foreach (SQLFileMigration migration in migrations)
            {
                using (SQLUnitOfWork unitOfWork = m_unitOfWorkFactory.CreateUnitOfWork())
                {
                    try
                    {
                        long executionTime = 0;

                        unitOfWork.BeginTransaction();

                        using (IDbCommand command = unitOfWork.Connection.CreateCommand())
                        {
                            command.Transaction = unitOfWork.Transaction;

                            using (StreamReader sr = new StreamReader(new FileStream(migration.Filename, FileMode.Open), Encoding.UTF8))
                            {
                                command.CommandText = sr.ReadToEnd();
                            }

                            Stopwatch stopwatch = Stopwatch.StartNew();

                            command.ExecuteNonQuery();

                            stopwatch.Stop();
                            executionTime = stopwatch.ElapsedMilliseconds;
                        }

                        InstalledVersion installedVersion = new InstalledVersion()
                        {
                            VersionNumber = migration.VersionNumber,
                            Description = migration.Description,
                            Installed = DateTimeOffset.UtcNow,
                            ExecutionTime = executionTime,
                            Checksum = null
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
                }
            }
        }

        private IEnumerable<SQLFileMigration> GetSQLFileMigrations()
        {
            IEnumerable<string> sqlFilenames = GetSQLFilenames();

            return GetSQLFilenames()
                .Select(filename =>
                {
                    int underscoreIndex = filename.IndexOf('_');

                    long versionNumber = long.Parse(filename.Substring(1, underscoreIndex - 1));
                    string description = filename.Substring(underscoreIndex + 1, filename.Length - underscoreIndex - 5);

                    return new SQLFileMigration()
                    {
                        Filename = Path.Combine(m_sqlFilesDirectory, filename),
                        VersionNumber = versionNumber,
                        Description = description
                    };
                });
        }

        private IEnumerable<string> GetSQLFilenames()
        {
            Regex regex = new Regex(@"^[V][0-9]+_.+(\.sql)$", RegexOptions.IgnoreCase);

            return Directory.GetFileSystemEntries(m_sqlFilesDirectory)
                .Where(filename => regex.IsMatch(filename));
        }

        private class SQLFileMigration
        {
            public string Filename { get; set; }

            public long VersionNumber { get; set; }

            public string Description { get; set; }

            public SQLFileMigration() { }
        }
    }
}
