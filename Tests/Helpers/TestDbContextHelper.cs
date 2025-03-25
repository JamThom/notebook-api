using Microsoft.EntityFrameworkCore;
using Notebook.Data;

namespace Tests.Helpers
{
    public static class TestDbContextHelper
    {
        public static ApplicationDbContext GetDbContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
