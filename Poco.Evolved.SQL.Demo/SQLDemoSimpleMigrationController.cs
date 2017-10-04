using System;
using System.Collections.Generic;
using System.Text;

using Poco.Evolved.Core.Database;
using Poco.Evolved.Core.Transactions;
using Poco.Evolved.SQL;
using Poco.Evolved.SQL.Transactions;

namespace Poco.Evolved.SQL.Demo
{
    public class SQLDemoSimpleMigrationController : SQLSimpleMigrationController
    {
        public override long CurrentVersionNumber
        {
            get
            {
                return 3;
            }
        }

        public SQLDemoSimpleMigrationController(IUnitOfWorkFactory<SQLUnitOfWork> unitOfWorkFactory, IDatabaseHelper<SQLUnitOfWork> databaseHelper)
            : base(unitOfWorkFactory, databaseHelper) { }

        protected override void ApplyMigration(SQLUnitOfWork unitOfWork, long versionNumber, out string description, out string checksum)
        {
            description = null;
            checksum = null;

            switch (versionNumber)
            {
                case 1:
                    // insert persons
                    PersonDao.CreateDatabase(unitOfWork.Connection);

                    description = "insert persons";

                    break;

                case 2:
                    // extend the names
                    PersonDao.GetAllPersons(unitOfWork.Connection).ForEach(person =>
                    {
                        person.Name = person.Name + " " + person.Name.ToUpper();

                        PersonDao.UpdatePerson(unitOfWork.Connection, person);
                    });

                    description = "extend name";

                    break;

                case 3:
                    // add age to the persons
                    PersonDao.GetAllPersons(unitOfWork.Connection).ForEach(person =>
                    {
                        person.Age = 20;

                        PersonDao.UpdatePerson(unitOfWork.Connection, person);
                    });

                    description = "add age";

                    break;
            }
        }
    }
}
