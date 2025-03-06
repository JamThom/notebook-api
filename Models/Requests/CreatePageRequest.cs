namespace Notebook.Models
{
    public class CreatePageRequest
    {
        required public string Content { get; set; }
        required public string BookId { get; set; }
    }
}