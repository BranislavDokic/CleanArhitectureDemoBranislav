using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database
{
    public class RealDatabase : DbContext
    {
        public RealDatabase(DbContextOptions<RealDatabase> options) : base(options) { }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<LibraryModel> Libraries { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId);

            
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Library)
                .WithMany(l => l.Books)
                .HasForeignKey(b => b.LibraryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
