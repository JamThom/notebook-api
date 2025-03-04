using Microsoft.AspNetCore.Identity;

namespace Notebook.Models
{
    public class User: IdentityUser
    {
        public ISet<Book> Books { get; set; }
    }
}