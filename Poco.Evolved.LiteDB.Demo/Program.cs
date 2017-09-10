using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using LiteDB;

using Poco.Evolved.LiteDB;
using Poco.Evolved.LiteDB.Database;
using Poco.Evolved.LiteDB.Transactions;

namespace Poco.Evolved.LiteDB.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // simple migrations
                using (LiteRepository liteRepository = InitDatabase())
                {
                    Console.WriteLine("before migrations:");
                    PrintPersons(liteRepository);

                    LiteDBSimpleMigrationController controller = new LiteDBDemoSimpleMigrationController(
                        new LiteDBUnitOfWorkFactory(liteRepository),
                        new LiteDBDatabaseHelper()
                    );
                    controller.ApplyMigrations();

                    Console.WriteLine("\nafter migrations:");
                    PrintPersons(liteRepository);
                }

                Console.WriteLine("\nPress enter to exit...");
                Console.Read();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        private static LiteRepository InitDatabase()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            int index = assemblyLocation.LastIndexOf(@"\");
            string assemblyDirectory = assemblyLocation.Substring(0, index);
            string databaseFilename = assemblyDirectory + @"\database.litedb";

            // delete old test database
            if (File.Exists(databaseFilename))
            {
                File.Delete(databaseFilename);
            }

            // init mapping
            BsonMapper.Global.Entity<Person>()
                .Id(e => e.Id)
                .Field(e => e.Name, nameof(Person.Name))
                .Field(e => e.Age, nameof(Person.Age));

            // create new test database
            LiteRepository liteRepository = new LiteRepository("Filename=" + databaseFilename);

            List<Person> persons = new List<Person>
            {
                new Person() { Name = "Alice" },
                new Person() { Name = "Bob" },
                new Person() { Name = "Trudy" }
            };

            liteRepository.Insert<Person>(persons);

            return liteRepository;
        }

        private static void PrintPersons(LiteRepository liteRepository)
        {
            IEnumerable<Person> persons = liteRepository.Fetch<Person>()
                .OrderBy(person => person.Name);

            foreach (Person person in persons)
            {
                Console.WriteLine("{0}, {1} years old", person.Name, person.Age);
            }
        }
    }
}
