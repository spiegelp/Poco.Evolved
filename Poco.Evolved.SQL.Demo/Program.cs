using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Data.Sqlite;

using Poco.Evolved.Core.Database;
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

                SQLiteDatabaseHelper databaseHelper = (SQLiteDatabaseHelper)DatabaseHelperFactory.CreateDatabaseHelper(DatabaseType.SQLite);
                databaseHelper.WithoutRowIdEnabled = true;

                // simple migrations
                using (IDbConnection connection = InitDatabase())
                {
                    SQLDemoSimpleMigrationController controller = new SQLDemoSimpleMigrationController(
                        new SQLUnitOfWorkFactory(connection),
                        databaseHelper
                    );

                    controller.ApplyMigrations();

                    Console.WriteLine("simple migrations result:");
                    PrintPersons(connection);
                }

                // class migrations
                /*using (LiteRepository liteRepository = InitDatabase())
                {
                    Console.WriteLine("\n\n-----------------------------------\n\n");
                    Console.WriteLine("before class migrations:");
                    PrintPersons(liteRepository);

                    LiteDBClassMigrationController controller = new LiteDBClassMigrationController(
                        new LiteDBUnitOfWorkFactory(liteRepository),
                        databaseHelper
                    );

                    controller.ApplyMigrations();

                    Console.WriteLine("\nafter class migrations:");
                    PrintPersons(liteRepository);
                }*/
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
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            int index = assemblyLocation.LastIndexOf(@"\");
            string assemblyDirectory = assemblyLocation.Substring(0, index);
            string databaseFilename = assemblyDirectory + @"\database.sqlite";

            // delete old test database
            if (File.Exists(databaseFilename))
            {
                File.Delete(databaseFilename);
            }

            // create new test database
            File.Create(databaseFilename).Close();

            IDbConnection connection = new SqliteConnection("Data Source=" + databaseFilename);
            connection.Open();

            return connection;
        }

        private static void PrintPersons(IDbConnection connection)
        {
            IEnumerable<Person> persons = PersonDao.GetAllPersons(connection)
                .OrderBy(person => person.Name);

            foreach (Person person in persons)
            {
                Console.WriteLine("{0}, {1} years old", person.Name, person.Age);
            }
        }
    }
}
