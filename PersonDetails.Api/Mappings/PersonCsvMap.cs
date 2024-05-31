using CsvHelper.Configuration;

namespace PersonDetails.Mappings
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

