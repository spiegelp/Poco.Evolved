using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Poco.Evolved.SQL.Demo
{
    public class PersonDao
    {
        public static void CreateDatabase(IDbConnection connection)
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE PERSONS (ID TEXT PRIMARY KEY NOT NULL, NAME TEXT, AGE INTEGER) WITHOUT ROWID";
                command.ExecuteNonQuery();

                Person person = new Person() { Name = "Alice" };
                command.CommandText = string.Format(
                    "INSERT INTO PERSONS (ID, NAME, AGE) VALUES ('{0}', '{1}', {2})",
                    person.Id,
                    person.Name,
                    person.Age
                );
                command.ExecuteNonQuery();

                person = new Person() { Name = "Bob" };
                command.CommandText = string.Format(
                    "INSERT INTO PERSONS (ID, NAME, AGE) VALUES ('{0}', '{1}', {2})",
                    person.Id,
                    person.Name,
                    person.Age
                );
                command.ExecuteNonQuery();

                person = new Person() { Name = "Trudy" };
                command.CommandText = string.Format(
                    "INSERT INTO PERSONS (ID, NAME, AGE) VALUES ('{0}', '{1}', {2})",
                    person.Id,
                    person.Name,
                    person.Age
                );
                command.ExecuteNonQuery();
            }
        }

        public static List<Person> GetAllPersons(IDbConnection connection)
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ID, NAME, AGE FROM PERSONS";
                List<Person> persons = new List<Person>();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        persons.Add(
                            new Person()
                            {
                                Id = reader.GetString(0),
                                Name = reader.GetString(1),
                                Age = reader.GetInt32(2)
                            }
                        );
                    }
                }

                return persons;
            }
        }

        public static void UpdatePerson(IDbConnection connection, Person person)
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = string.Format(
                    "UPDATE PERSONS SET NAME = '{0}', AGE = {1} WHERE ID = '{2}'",
                    person.Name,
                    person.Age,
                    person.Id
                );

                command.ExecuteNonQuery();
            }
        }
    }
}
