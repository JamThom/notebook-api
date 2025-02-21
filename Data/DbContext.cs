using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notebook.Models;

namespace Notebook.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ISet<Page> Pages { get; set; }
        public ISet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Book>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notebooks)
                .HasForeignKey(n => n.UserId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=notebook.db");
        }
    }
}