using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Poco.Evolved.Core;

using Poco.Evolved.SQL;

namespace Poco.Evolved.SQL.Demo
{
    [Migration(4, Description = "change age")]
    public class ChangeAgeMigration : SQLDataMigration
    {
        public ChangeAgeMigration() : base() { }

        public override void ApplyMigration()
        {
            PersonDao.GetAllPersons(UnitOfWork.Connection, UnitOfWork.Transaction).ForEach(person =>
            {
                person.Age = person.Age + 2;

                PersonDao.UpdatePerson(UnitOfWork.Connection, UnitOfWork.Transaction, person);
            });
        }
    }

    [Migration(5, Description = "another change age")]
    public class AnotherChangeAgeMigration : SQLFileDataMigration
    {
        public AnotherChangeAgeMigration()
            : base(
                  // use a MemoryStream for the example instead of a FileStream
                  () => new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes("UPDATE PERSONS SET AGE = AGE + 10"))),
                  true
            ) { }
    }
}
