namespace Notebook.Models.Requests
{
    public class UpdateBookRequest
    {
        required public string Id { get; set; }
        required public string Name { get; set; }
    }
}