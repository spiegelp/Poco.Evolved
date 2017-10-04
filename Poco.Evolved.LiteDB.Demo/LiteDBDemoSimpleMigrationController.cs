using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Transactions;
using Poco.Evolved.LiteDB;
using Poco.Evolved.LiteDB.Transactions;

namespace Poco.Evolved.LiteDB.Demo
{
    public class LiteDBDemoSimpleMigrationController : LiteDBSimpleMigrationController
    {
        public override long CurrentVersionNumber
        {
            get
            {
                return 3;
            }
        }

        public LiteDBDemoSimpleMigrationController(IUnitOfWorkFactory<LiteDBUnitOfWork> unitOfWorkFactory, IDatabaseHelper<LiteDBUnitOfWork> databaseHelper)
            : base(unitOfWorkFactory, databaseHelper) { }

        protected override void ApplyMigration(LiteDBUnitOfWork unitOfWork, long versionNumber, out string description, out string checksum)
        {
            description = null;
            checksum = null;

            List<Person> persons = unitOfWork.LiteRepository.Fetch<Person>();

            switch (versionNumber)
            {
                case 1:
                    // extend the names
                    persons.ForEach(person =>
                    {
                        person.Name = person.Name + " " + person.Name.ToUpper();
                    });

                    unitOfWork.LiteRepository.Update<Person>(persons);

                    description = "extend name";

                    break;

                case 2:
                    // add age to the persons
                    persons.ForEach(person =>
                    {
                        person.Age = 20;
                    });

                    unitOfWork.LiteRepository.Update<Person>(persons);

                    description = "add age";

                    break;

                case 3:
                    // change the age of the persons
                    persons.ForEach(person =>
                    {
                        person.Age = person.Age + 2;
                    });

                    unitOfWork.LiteRepository.Update<Person>(persons);

                    description = "change age";

                    break;
            }
        }
    }
}
