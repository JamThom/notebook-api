namespace Notebook.Models
{
    public class BookResponse
    {
        required public string Id { get; set; }
        required public string Name { get; set; }
        required public ICollection<PageResponse> Pages { get; set; }
    }
}