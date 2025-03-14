using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notebook.Models;

namespace Notebook.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Page> Pages { get; set; }
        public DbSet<Book> Books { get; set; }
        public new DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .HasOne(b => b.User)
                .WithMany(u => u.Books)
                .HasForeignKey(b => b.UserId);

            builder.Entity<Page>()
                .HasOne(p => p.Book)
                .WithMany(b => b.Pages)
                .HasForeignKey(p => p.BookId);
        }
    }
}