using Microsoft.AspNetCore.Identity;

namespace Notebook.Models
{
    public class User: IdentityUser
    {
        required public ISet<Book> Books { get; set; }
    }
}