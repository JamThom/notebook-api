namespace Notebook.Models.Responses
{
    public class PageResponse
    {
        required public string Id { get; set; }
        required public int Index { get; set; }
        required public string Content { get; set; }
    }
}