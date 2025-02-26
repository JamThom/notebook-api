namespace Notebook.Models
{
    public class Book
    {
        public required string Id { get; set; }
        required public string Name { get; set; }
        required public string Content { get; set; }
        required public string UserId { get; set; }
        required public User User { get; set; }
        public ISet<Page> Pages { get; set; }
    }
}