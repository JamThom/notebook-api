namespace Notebook.Models
{
    public class Page
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public string Content { get; set; }
        public string BookId { get; set; }
        public Book Book { get; set; }
    }
}