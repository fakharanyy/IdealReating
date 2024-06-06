using Microsoft.EntityFrameworkCore;
using PersonDetails.Api.Data.Entities;

namespace PersonDetails.Api.Data.Repos;

public class SqlPersonRepository : IPersonRepository
{
    private readonly ApplicationContext _context;

    public SqlPersonRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Person>> GetPersonsAsync(string filter)
    {
        var query = _context.Persons.AsQueryable();
        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(p =>
                p.Name.Contains(filter) || p.TelephoneNumber.Contains(filter) || p.Address.Contains(filter) ||
                p.Country.Contains(filter));
        }

        return await query.ToListAsync();
    }
}