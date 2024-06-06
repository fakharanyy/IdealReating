using CsvHelper.Configuration;
using PersonDetails.Api.Data.Entities;

namespace PersonDetails.Api.Mappings
{

    public class PersonCsvMap : ClassMap<Person>
    {
        public PersonCsvMap()
        {
            Map(m => m.Name).Convert(row => $"{row.Row.GetField("First Name")} {row.Row.GetField("Last Name")}");
            Map(m => m.TelephoneNumber).Name("Number");
            Map(m => m.Address).Name("Full Address");
            Map(m => m.Country).Name("Country code");
        }
    }
}

