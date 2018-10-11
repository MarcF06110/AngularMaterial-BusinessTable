using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MatTableExample.Repositories.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private string connectionString = "Server=(local);Database=TestDb;Trusted_Connection=True;";

        [TestMethod]
        public void TestMethod1()
        {
            var repository = new PersonsRepository(connectionString);
            var result = repository.LoadPagedAll(null, 0, 10, "FirstName", "ASC");
            Assert.IsNotNull(result);

            Console.WriteLine($"{result.Count}");
            foreach (var person in result.Items)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var repository = new PersonsRepository(connectionString);
            var result = repository.LoadPagedAll("cl", 0, 5, "FirstName", "ASC");
            Assert.IsNotNull(result);

            Console.WriteLine($"{result.Count}");
            foreach (var person in result.Items)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
            }
        }
    }
}
