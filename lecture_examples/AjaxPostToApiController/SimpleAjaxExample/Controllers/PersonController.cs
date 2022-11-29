using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleAjaxExample.DAL;
using SimpleAjaxExample.Models;

// See: https://learn.microsoft.com/en-us/aspnet/core/web-api/?WT.mc_id=dotnet-35129-website&view=aspnetcore-6.0

namespace SimpleAjaxExample.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IRepository<Person> _personRepository;

        public PersonController(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        // GET: api/person/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Person> GetPerson(int id)
        {
            Person person = _personRepository.FindById(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // POST: api/person
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person> PostPerson([Bind("Age,Name,Anniversary")] Person person)
        {
            if (ModelState.IsValid)
            {
                // prevent overposting of the Id
                person.Id = 0;
                // If using a DTO can proceed.  If using an Entity Framework model, set all nav properties to null
                Person p = _personRepository.AddOrUpdate(person);
                return CreatedAtAction("GetPerson", "Person", p);
            }

            return BadRequest(ModelState);
        }
    }
}
