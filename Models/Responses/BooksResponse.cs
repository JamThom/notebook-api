using Domain.Base;

namespace Notebook.Models
{
    public class BooksResponse
    {
        required public string Id { get; set; }
        required public string Name { get; set; }
    }
}