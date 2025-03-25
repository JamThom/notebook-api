namespace Notebook.Models.Responses
{
    public class BooksResponse
    {
        required public string Id { get; set; }
        required public string Name { get; set; }
        required public List<PageResponse> Pages { get; set; }
    }
}