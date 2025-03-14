namespace Notebook.Models.Requests
{
    public class UpdateNotebookRequest
    {
        required public string Id { get; set; }
        required public string Name { get; set; }
    }
}