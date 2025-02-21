namespace Notebook.Models
{
    public class Page
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}