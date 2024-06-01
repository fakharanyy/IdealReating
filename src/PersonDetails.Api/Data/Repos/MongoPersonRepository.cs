using MongoDB.Driver;
using PersonDetails.Data.Repos;

public class MongoPersonRepository : IPersonRepository
{
    private readonly IMongoCollection<Person> _collection;

    public MongoPersonRepository(IMongoClient mongoClient, string databaseName, string collectionName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        _collection = database.GetCollection<Person>(collectionName);
    }

    public async Task<IEnumerable<Person>> GetPersonsAsync(string filter)
    {
        var persons = await _collection.Find(_ => true).ToListAsync();
        if (!string.IsNullOrEmpty(filter))
        {
            persons = persons.Where(p =>
                p.Name.Contains(filter) || p.TelephoneNumber.Contains(filter) || p.Address.Contains(filter) ||
                p.Country.Contains(filter)).ToList();
        }

        return persons;
    }
}