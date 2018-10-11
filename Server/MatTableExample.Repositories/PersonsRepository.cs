using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MatTableExample.Entities;
using MatTableExample.Entities.Infra;

namespace MatTableExample.Repositories
{
    public class PersonsRepository : RepositoryBase
    {
        public PersonsRepository(){}
        public PersonsRepository(string connectionString): base(connectionString) { }

        public PaginationResult<Person> LoadPagedAll(string lastNameFilter, int pageIndex, int pageSize, string sortColumn = "LastName", string sortDirection = "Asc")
        {
            string filter = "";
              if (!string.IsNullOrWhiteSpace(lastNameFilter))
            {
                lastNameFilter = $"%{lastNameFilter}%";
                filter = "WHERE LastName LIKE @lastNameFilter";
            }          
            var sql = $"SELECT Count(*) FROM Persons {filter};" ;
            sql += $"SELECT * FROM Persons {filter}";


            sql += GetPaginationQuery(pageIndex, pageSize, sortColumn, sortDirection);

            using (var connection = new SqlConnection(ConnectionString))
            {

                using (var multi = connection.QueryMultiple(sql, new {lastNameFilter}))
                {
                    var numbers = multi.Read<int>().First();
                    var persons = multi.Read<Person>();
                    return new PaginationResult<Person>(numbers, persons);
                }
            }
        }

        public Person LoadById(int id)
        {
            var sql = "SELECT * FROM Persons WHERE Id = @Id";
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.QueryFirstOrDefault<Person>(sql, new {id});
            }
        }

        public void Create(Person person)
        {
            var sql = @"
                INSERT INTO Persons (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age);
                SELECT CAST(SCOPE_IDENTITY() AS int);";
            using (var connection = new SqlConnection(ConnectionString))
            {
                person.Id = connection.QueryFirst<int>(sql, person);
            }
        }

        public int Update(Person person)
        {
            var sql = "UPDATE Persons SET FirstName = @FirstName, LastName = @LastName, Age = @Age WHERE Id = @Id";
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Execute(sql, person);
            }
        }

        public int Delete(int id)
        {
            var sql = "DELETE From Persons WHERE id = @id";
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Execute(sql, new{id});
            }
        }
        protected override IEnumerable<string> AllowedSortColumns => new[] {"FIRSTNAME", "LASTNAME", "AGE"};
    }
}