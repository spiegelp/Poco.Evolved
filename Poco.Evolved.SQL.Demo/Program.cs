using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Data.Sqlite;

using Poco.Evolved.SQL;
using Poco.Evolved.SQL.Transactions;
using Poco.Evolved.SQL.Database;

namespace Poco.Evolved.SQL.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                SQLitePCL.Batteries.Init();

                // initialize the helper for SQLite databases
                SQLiteDatabaseHelper databaseHelper = (SQLiteDatabaseHelper)DatabaseHelperFactory.CreateDatabaseHelper(DatabaseType.SQLite);
                databaseHelper.WithoutRowIdEnabled = true;
                
                using (IDbConnection connection = InitDatabase())
                {
                    // simple migrations (SQLDemoSimpleMigrationController.cs)
                    //     1. create database and insert persons with a name
                    //     2. append Name = Name + " " + ToUpper(Name)
                    //     3. set the age to 20
                    SQLDemoSimpleMigrationController simpleMigrationController = new SQLDemoSimpleMigrationController(
                        new SQLUnitOfWorkFactory(connection),
                        databaseHelper
                    );

                    simpleMigrationController.ApplyMigrations();

                    Console.WriteLine("simple migrations result:");
                    PrintPersons(connection);

                    // class migrations (Migration.cs)
                    //     1. Age = Age + 2 using code
                    //     2. Age = Age + 10 using a SQL statement out of a stream
                    SQLClassMigrationController classMigrationController = new SQLClassMigrationController(
                        new SQLUnitOfWorkFactory(connection),
                        databaseHelper
                    );

                    classMigrationController.ApplyMigrations();

                    Console.WriteLine("\nclass migrations result:");
                    PrintPersons(connection);

                    // SQL file migrations (files inside the 'migrations' directory)
                    string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                    int index = assemblyLocation.LastIndexOf(Path.DirectorySeparatorChar);
                    string assemblyDirectory = assemblyLocation.Substring(0, index);

                    SQLFileMigrationController sqlFileMigrationController = new SQLFileMigrationController(
                        new SQLUnitOfWorkFactory(connection),
                        databaseHelper,
                        Path.Combine(assemblyDirectory, "migrations")
                    )
                    {
                        SkipInitInstalledVersions = true
                    };

                    sqlFileMigrationController.ApplyMigrations();

                    Console.WriteLine("\nSQL file migrations result:");
                    PrintPersons(connection);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            Console.WriteLine("\nPress enter to exit...");
            Console.Read();
        }

        private static IDbConnection InitDatabase()
        {
            IDbConnection connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            return connection;
        }

        private static void PrintPersons(IDbConnection connection)
        {
            using (IDbTransaction transaction = connection.BeginTransaction())
            {
                IEnumerable<Person> persons = PersonDao.GetAllPersons(connection, transaction)
                    .OrderBy(person => person.Name);

                foreach (Person person in persons)
                {
                    Console.WriteLine("{0}, {1} years old", person.Name, person.Age);
                }
            }
        }
    }
}
