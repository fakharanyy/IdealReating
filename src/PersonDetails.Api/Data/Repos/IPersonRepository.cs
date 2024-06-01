namespace PersonDetails.Data.Repos
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPersonsAsync(string? filter);

    }
}
