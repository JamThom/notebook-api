namespace Notebook.Models
{
    public class PageResponseModel
    {
        required public string Id { get; set; }
        required public int Index { get; set; }
        required public string Content { get; set; }
    }
}