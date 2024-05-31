using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using PersonDetails.Data.Repos;
using PersonDetails.Mappings;

public class CsvPersonRepository : IPersonRepository
{
    private readonly string _csvFilePath;

    public CsvPersonRepository(string csvFilePath)
    {
        _csvFilePath = csvFilePath;
    }

    public async Task<IEnumerable<Person>> GetPersonsAsync(string filter)
    {
        using var reader = new StreamReader(_csvFilePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
        });
        csv.Context.RegisterClassMap<PersonCsvMap>();
        var records = csv.GetRecords<Person>().ToList();

        if (!string.IsNullOrEmpty(filter))
        {
            records = records.Where(p =>
                p.Name.Contains(filter) || p.TelephoneNumber.Contains(filter) || p.Address.Contains(filter) ||
                p.Country.Contains(filter)).ToList();
        }

        return await Task.FromResult(records);
    }
}