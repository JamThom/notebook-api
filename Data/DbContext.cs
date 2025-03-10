using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notebook.Models;

namespace Notebook.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Page> Pages { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=notebook.db");
                // optionsBuilder.UseSqlServer("Server=magicnotebook.database.windows.net;Database=notebook;User Id=your-notebookgod;Password=H20isWater;");
            }
        }
    }
}