namespace Notebook.Models.Requests
{
    public class UpdatePageRequest
    {
        required public string Id { get; set; }
        required public string Content { get; set; }
    }
}