using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core;

using Poco.Evolved.LiteDB;

namespace Poco.Evolved.LiteDB.Demo
{
    [Migration(1, Description = "extend name")]
    public class ExtendNameMigration : LiteDBDataMigration
    {
        public ExtendNameMigration() : base() { }

        public override void ApplyMigration()
        {
            List<Person> persons = UnitOfWork.LiteRepository.Fetch<Person>();

            persons.ForEach(person =>
            {
                person.Name = person.Name + " " + person.Name.ToUpper();
            });

            UnitOfWork.LiteRepository.Update<Person>(persons);
        }
    }

    [Migration(2, Description = "add age")]
    public class AddAgeMigration : LiteDBDataMigration
    {
        public AddAgeMigration() : base() { }

        public override void ApplyMigration()
        {
            List<Person> persons = UnitOfWork.LiteRepository.Fetch<Person>();

            persons.ForEach(person =>
            {
                person.Age = 20;
            });

            UnitOfWork.LiteRepository.Update<Person>(persons);
        }
    }

    [Migration(3, Description = "change age")]
    public class ChangeAgeMigration : LiteDBDataMigration
    {
        public ChangeAgeMigration() : base() { }

        public override void ApplyMigration()
        {
            List<Person> persons = UnitOfWork.LiteRepository.Fetch<Person>();

            persons.ForEach(person =>
            {
                person.Age = person.Age + 2;
            });

            UnitOfWork.LiteRepository.Update<Person>(persons);
        }
    }
}
