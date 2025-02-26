using Microsoft.AspNetCore.Identity;

namespace Notebook.Models
{
    public class User: IdentityUser
    {
        public string Id { get; set; }
        public ISet<Book> Books { get; set; }
    }
}