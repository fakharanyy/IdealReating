using Microsoft.EntityFrameworkCore;

namespace PersonDetails.Data
{

    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }

}
