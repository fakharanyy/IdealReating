using Microsoft.AspNetCore.Mvc;
using PersonDetails.Api.Models;
using PersonDetails.Api.Services;

namespace PersonDetails.Api.Controllers;

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
    public async Task<ActionResult<IEnumerable<PersonResponseModel>>> Get([FromQuery] string? filter = null)
    {
        var persons = await _personService.GetAllPersonsAsync(filter);
        return Ok(persons);
    }
}