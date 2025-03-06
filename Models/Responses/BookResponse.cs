namespace Notebook.Models
{
    public class BookResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<PageResponse> Pages { get; set; }
    }
}