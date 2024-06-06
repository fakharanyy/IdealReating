using PersonDetails.Api.Data.Entities;

namespace PersonDetails.Api.Data.Repos
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPersonsAsync(string? filter);

    }
}
