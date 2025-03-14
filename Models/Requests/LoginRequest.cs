namespace Notebook.Models.Requests
{
    public class LoginRequest
    {
        required public string Email { get; set; }
        required public string Password { get; set; }
    }
}