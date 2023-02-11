using Microsoft.EntityFrameworkCore;
using MinimalBookAPI.DbModels;

namespace MinimalBookAPI.Context
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(BookDataForSeed());
            base.OnModelCreating(modelBuilder);
        }

        private static List<Book> BookDataForSeed()
        {
            return new List<Book>
            {
                new ()
                {
                     Id = 1,
                     Name = "Test first name",
                     Page = 100
                },
                new ()
                {
                     Id = 2,
                     Name = "Test seconf name",
                     Page = 200
                },
                new ()
                {
                     Id = 3,
                     Name = "Test fifth name",
                     Page = 500
                },
                new ()
                {
                     Id = 4,
                     Name = "Test sixth name",
                     Page = 345
                },
                new ()
                {
                     Id = 5,
                     Name = "Test seventh name",
                     Page = 700
                }
            };
        }
    }
}
