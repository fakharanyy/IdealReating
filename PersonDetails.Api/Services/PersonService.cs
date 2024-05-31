using AutoMapper;
using PersonDetails.Data.Repos;
using PersonDetails.Models;

public class PersonService
{
    private readonly IEnumerable<IPersonRepository> _repositories;
    private readonly IMapper _mapper;

    public PersonService(IEnumerable<IPersonRepository> repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PersonResponseModel>> GetAllPersonsAsync(string? filter)
    {
        var tasks = _repositories.Select(repo => repo.GetPersonsAsync(filter));
        var results = await Task.WhenAll(tasks);
        var persons = results.SelectMany(r => r);

        return _mapper.Map<IEnumerable<PersonResponseModel>>(persons);
    }
}