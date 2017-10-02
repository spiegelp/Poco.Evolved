using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Model;

using Poco.Evolved.SQL.Transactions;

namespace Poco.Evolved.SQL.Database
{
    /// <summary>
    /// Helper for database specific operations on a SQL databases.
    /// </summary>
    public class SQLDatabaseHelper : IDatabaseHelper<SQLUnitOfWork>
    {
        /// <summary>
        /// The maximum table name length, because some databases limit the length of names.
        /// </summary>
        protected const int MaxInstalledVersionsTableNameLength = 30;

        /// <summary>
        /// The default table name for saving the information about installed versions.
        /// </summary>
        protected const string DefaultInstalledVersionsTableName = nameof(InstalledVersion) + "s";

        protected readonly string m_installedVersionsTableName;

        /// <summary>
        /// Constructs a new <see cref="SQLDatabaseHelper" />.
        /// </summary>
        /// <param name="installedVersionsTableName">Optional name of the table for saving the information about installed versions</param>
        public SQLDatabaseHelper(string installedVersionsTableName = null)
        {
            if (!string.IsNullOrWhiteSpace(installedVersionsTableName)
                && installedVersionsTableName.Length > MaxInstalledVersionsTableNameLength)
            {
                throw new ArgumentNullException(string.Format(
                    "The maximum allowed length for {0} is {1}, because some databases limit the length of names.",
                    nameof(installedVersionsTableName),
                    MaxInstalledVersionsTableNameLength
                ));
            }

            m_installedVersionsTableName = !string.IsNullOrWhiteSpace(installedVersionsTableName)
                ? installedVersionsTableName : ConvertToNameOnDatabase(DefaultInstalledVersionsTableName);
        }

        /// <summary>
        /// Converts a name from the code into a typical SQL database name (upper case and underscores).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string ConvertToNameOnDatabase(string name)
        {
            String nameUpper = name.ToUpper();
            StringBuilder databaseName = new StringBuilder();

            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                char cUpper = nameUpper[i];

                if (i > 0 && c == cUpper)
                {
                    char cPrev = name[i - 1];
                    char cPrevUpper = nameUpper[i - 1];

                    if (cPrev != cPrevUpper)
                    {
                        databaseName.Append('_');
                    }
                }

                databaseName.Append(cUpper);
            }

            return databaseName.ToString();
        }

        /// <summary>
        /// Gets the SQL script to create the table for saving information on installed versions.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCreateInstalledVersionsTableScript()
        {
            return string.Format(
                "CREATE TABLE {0} ({1}, {2}, {3}, {4}, {5}, {6})",
                m_installedVersionsTableName,
                ConvertToNameOnDatabase(nameof(InstalledVersion.VersionNumber)) + " DECIMAL(18, 0) NOT NULL",
                ConvertToNameOnDatabase(nameof(InstalledVersion.Description)) + " VARCHAR(1024)",
                ConvertToNameOnDatabase(nameof(InstalledVersion.Installed)) + " VARCHAR(38) NOT NULL",
                ConvertToNameOnDatabase(nameof(InstalledVersion.ExecutionTime)) + " DECIMAL(18, 0) NOT NULL",
                ConvertToNameOnDatabase(nameof(InstalledVersion.Checksum)) + " VARCHAR(1024)",
                "PRIMARY KEY(" + ConvertToNameOnDatabase(nameof(InstalledVersion.VersionNumber)) + ")"
            );
        }

        /// <summary>
        /// Optionally gets the CREATE TABLE script for saving information on installed versions using a check for an already existing table.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCreateInstalledVersionsTableIfNotExistsScript()
        {
            return null;
        }

        /// <summary>
        /// Truncates the string to the specified lenght if it is longer.
        /// </summary>
        /// <param name="str">The string to truncate</param>
        /// <param name="length">The maximum length for the string</param>
        /// <returns></returns>
        protected string TruncateStringToLength(string str, int length)
        {
            if (str != null && length > 0 && str.Length > length)
            {
                return str.Substring(0, length);
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// Creates the table for saving information on installed versions if necessary.
        /// </summary>
        /// <param name="unitOfWork">The unit of work to work with</param>
        public virtual void InitInstalledVersions(SQLUnitOfWork unitOfWork)
        {
            string createTableScript = GetCreateInstalledVersionsTableIfNotExistsScript();

            if (string.IsNullOrWhiteSpace(createTableScript))
            {
                // very rude way to check if a table exists:
                //     there is no standard way to check if a table exists, therefore try to select from it and see if the command throws an exception
                bool tableExists = false;

                using (IDbCommand command = unitOfWork.Connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM " + m_installedVersionsTableName;
                    command.Transaction = unitOfWork.Transaction;

                    try
                    {
                        command.ExecuteScalar();

                        // the table exists, because the SELECT did not fail
                        tableExists = true;
                    }
                    catch (Exception)
                    {
                        // the SELECT fails so the table likely does not exist
                        tableExists = false;
                    }
                }

                if (!tableExists)
                {
                    createTableScript = GetCreateInstalledVersionsTableScript();
                }
            }

            if (!string.IsNullOrWhiteSpace(createTableScript))
            {
                using (IDbCommand command = unitOfWork.Connection.CreateCommand())
                {
                    command.CommandText = createTableScript;
                    command.Transaction = unitOfWork.Transaction;

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Saves the information about an installed data migration.
        /// </summary>
        /// <param name="unitOfWork">The unit of work to work with</param>
        /// <param name="installedVersion">The information of a version to save</param>
        public virtual void SaveInstalledVersion(SQLUnitOfWork unitOfWork, InstalledVersion installedVersion)
        {
            string insertScript = string.Format(
                "INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}) VALUES (@VersionNumber, @Description, @Installed, @ExecutionTime, @Checksum)",
                m_installedVersionsTableName,
                ConvertToNameOnDatabase(nameof(InstalledVersion.VersionNumber)),
                ConvertToNameOnDatabase(nameof(InstalledVersion.Description)),
                ConvertToNameOnDatabase(nameof(InstalledVersion.Installed)),
                ConvertToNameOnDatabase(nameof(InstalledVersion.ExecutionTime)),
                ConvertToNameOnDatabase(nameof(InstalledVersion.Checksum))
            );

            using (IDbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = insertScript;
                command.Transaction = unitOfWork.Transaction;

                IDbDataParameter parameter = command.CreateParameter();
                parameter.ParameterName = "VersionNumber";
                parameter.DbType = DbType.Int64;
                parameter.Value = installedVersion.VersionNumber;
                command.Parameters.Add(parameter);

                parameter = command.CreateParameter();
                parameter.ParameterName = "Description";
                parameter.DbType = DbType.String;
                parameter.Value = (object)TruncateStringToLength(installedVersion.Description, 1024) ?? DBNull.Value;
                command.Parameters.Add(parameter);

                parameter = command.CreateParameter();
                parameter.ParameterName = "Installed";
                parameter.DbType = DbType.String;
                parameter.Value = installedVersion.InstalledString;
                command.Parameters.Add(parameter);

                parameter = command.CreateParameter();
                parameter.ParameterName = "ExecutionTime";
                parameter.DbType = DbType.Int64;
                parameter.Value = installedVersion.ExecutionTime;
                command.Parameters.Add(parameter);

                parameter = command.CreateParameter();
                parameter.ParameterName = "Checksum";
                parameter.DbType = DbType.String;
                parameter.Value = (object)installedVersion.Checksum ?? DBNull.Value;
                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets all installed versions of data migrations installed on the database.
        /// </summary>
        /// <param name="unitOfWork">The unit of work to work with</param>
        /// <returns></returns>
        public virtual IEnumerable<InstalledVersion> GetInstalledVersions(SQLUnitOfWork unitOfWork)
        {
            string selectScript = string.Format(
                "SELECT {0}, {1}, {2}, {3}, {4} FROM {5}",
                ConvertToNameOnDatabase(nameof(InstalledVersion.VersionNumber)),
                ConvertToNameOnDatabase(nameof(InstalledVersion.Description)),
                ConvertToNameOnDatabase(nameof(InstalledVersion.Installed)),
                ConvertToNameOnDatabase(nameof(InstalledVersion.ExecutionTime)),
                ConvertToNameOnDatabase(nameof(InstalledVersion.Checksum)),
                m_installedVersionsTableName
            );

            using (IDbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = selectScript;
                command.Transaction = unitOfWork.Transaction;

                using (IDataReader reader = command.ExecuteReader())
                {
                    List<InstalledVersion> installedVersions = new List<InstalledVersion>();

                    while (reader.Read())
                    {
                        installedVersions.Add(new InstalledVersion()
                        {
                            VersionNumber = reader.GetInt64(0),
                            Description = reader.GetString(1),
                            InstalledString = reader.GetString(2),
                            ExecutionTime = reader.GetInt64(3),
                            Checksum = reader.GetString(4)
                        });
                    }

                    return installedVersions;
                }
            }
        }
    }
}
