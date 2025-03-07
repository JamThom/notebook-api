namespace Notebook.Models
{
    public class Page
    {
        required public string Id { get; set; }
        required public int Index { get; set; }
        required public string Content { get; set; }
        required public string BookId { get; set; }
        required public Book Book { get; set; }
    }
}