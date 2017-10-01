using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Poco.Evolved.SQL
{
    /// <summary>
    /// Base class for SQL data migrations by reading the SQL script from an UTF-8 encoded file or stream.
    /// </summary>
    public abstract class SQLFileDataMigration : SQLDataMigration
    {
        /// <summary>
        /// Delegate for a factory which creates an UTF-8 encoded <see cref="StreamReader" /> with the SQL script.
        /// </summary>
        /// <returns></returns>
        public delegate StreamReader StreamReaderFactory();

        private StreamReaderFactory m_streamReaderFactory;
        private bool m_closeStream;

        /// <summary>
        /// Constructs a new <see cref="SQLFileDataMigration" />.
        /// <param name="fullFilename">The full filename of the file with the SQL script</param>
        /// </summary>
        public SQLFileDataMigration(string fullFilename)
            : this(() => new StreamReader(new FileStream(fullFilename, FileMode.Open), Encoding.UTF8), true) { }

        /// <summary>
        /// Constructs a new <see cref="SQLFileDataMigration" />.
        /// </summary>
        /// <param name="streamReaderFactory">The factory for an UTF-8 encoded <see cref="StreamReader" /> with the SQL script</param>
        /// <param name="closeStream">true for closing the stream after reading</param>
        public SQLFileDataMigration(StreamReaderFactory streamReaderFactory, bool closeStream)
            : base()
        {
            m_streamReaderFactory = streamReaderFactory;
            m_closeStream = closeStream;
        }

        /// <summary>
        /// Applies the data migration.
        /// </summary>
        public override void ApplyMigration()
        {
            using (IDbCommand command = UnitOfWork.Connection.CreateCommand())
            {
                StreamReader sr = m_streamReaderFactory();
                command.CommandText = sr.ReadToEnd();

                if (m_closeStream)
                {
                    sr.Close();
                }

                command.ExecuteNonQuery();
            }
        }
    }
}
