using System.Web.Http;
using MatTableExample.Entities;
using MatTableExample.Entities.Infra;
using MatTableExample.Repositories;

namespace MatTableExample.Controllers
{
    public class PersonsController : ApiController
    {
        private readonly PersonsRepository _personsRepositoy = new PersonsRepository();
        // GET api/values
        public PaginationResult<Person> Get(string lastNameFilter, int pageIndex, int pageSize, string sortColumn = "LastName", string sortDirection = "Asc")
        {
            return _personsRepositoy.LoadPagedAll(lastNameFilter, pageIndex, pageSize, sortColumn, sortDirection);
        }

        // POST api/values
        public Person Post([FromBody]Person person)
        {
            _personsRepositoy.Create(person);
            return person;
        }

        // PUT api/values/5
        public Person Put(int id, [FromBody]Person value)
        {
            var result = _personsRepositoy.Update(value);
            return _personsRepositoy.LoadById(value.Id);
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            _personsRepositoy.Delete(id);
        }
    }
}
