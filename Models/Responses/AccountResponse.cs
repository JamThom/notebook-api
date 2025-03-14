namespace Notebook.Models.Responses
{
    public class AccountResponse
    {
        required public string Id { get; set; }
        required public string UserName { get; set; }
        required public string Email { get; set; }
    }
}