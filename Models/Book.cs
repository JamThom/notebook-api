namespace Notebook.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ISet<Page> Pages { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}