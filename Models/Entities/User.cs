using Microsoft.AspNetCore.Identity;

namespace Notebook.Models
{
    public class User: IdentityUser
    {
        required public override string UserName { get; set; }
        required public override string Email { get; set; }
        required public ISet<Book> Books { get; set; }
    }
}