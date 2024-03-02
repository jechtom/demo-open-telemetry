using Microsoft.EntityFrameworkCore;

namespace OTelDemo.Web.Services
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = "John Doe", Age = 30 },
                new Person { Id = 2, Name = "Jane Doe", Age = 25 },
                new Person { Id = 3, Name = "Joe Bloggs", Age = 40 },
                new Person { Id = 4, Name = "Jill Bloggs", Age = 35 },
                new Person { Id = 5, Name = "Jack Smith", Age = 50 },
                new Person { Id = 6, Name = "Jill Smith", Age = 45 },
                new Person { Id = 7, Name = "John Johnson", Age = 60 },
                new Person { Id = 8, Name = "Jane Johnson", Age = 55 },
                new Person { Id = 9, Name = "John Citizen", Age = 80 },
                new Person { Id = 10, Name = "Jane Citizen", Age = 75 }
            );
        }

        public class Person
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int Age { get; set; }
        }
    }
}
