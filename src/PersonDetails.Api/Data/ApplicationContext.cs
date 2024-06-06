using Microsoft.EntityFrameworkCore;
using PersonDetails.Api.Data.Entities;

namespace PersonDetails.Api.Data
{

    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }

}
