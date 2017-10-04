using System;
using System.Collections.Generic;
using System.Text;

namespace Poco.Evolved.SQL.Demo
{
    public class Person
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Person()
        {
            Id = Guid.NewGuid().ToString().ToLower();
            Name = null;
            Age = -1;
        }
    }
}
