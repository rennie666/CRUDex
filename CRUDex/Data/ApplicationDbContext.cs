using CRUDex.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CRUDex.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            this.Books = this.Set<Book>();
            this.Authors = this.Set<Author>();
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
             .UseSqlServer(@"Server=RENI7440;Database=BookDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book()
                {
                    Id=1,
                    Title = "Something",
                    AuthorId = 1
                    
                });
            modelBuilder.Entity<Author>().HasData(
                new Author()
                {
                    Id = 1,
                    Name = "Someone"

                });
        }
    }
}
