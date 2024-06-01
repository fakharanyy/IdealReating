using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly PersonService _personService;

    public PersonsController(PersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> Get([FromQuery] string? filter = null)
    {
        var persons = await _personService.GetAllPersonsAsync(filter);
        return Ok(persons);
    }
}