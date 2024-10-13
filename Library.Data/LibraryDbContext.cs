using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    public class LibraryDbContext : DbContext
    {
        public const string CONNECTION_STRING = "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Database=Library";
        
        public virtual DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookInstance> BookInstances { get; set; }

        public LibraryDbContext() {}

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CONNECTION_STRING);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Books)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            modelBuilder.Entity<Author>()
                .HasMany(x => x.Books)
                .WithOne(x => x.BookAuthor)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .HasMany(x => x.BookInstances)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            modelBuilder.Entity<Book>()
                .HasMany(x => x.Instances)
                .WithOne(x => x.Book)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder); 
        }
    }
}
