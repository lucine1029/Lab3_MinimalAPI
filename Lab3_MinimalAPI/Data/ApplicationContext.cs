using Lab3_MinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab3_MinimalAPI.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Person> Person { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<InterestLink> InterestLinks { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) { }
    }
}
