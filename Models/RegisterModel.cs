namespace Notebook.Models
{
    public class RegisterModel
    {
        required public string Email { get; set; }
        required public string Password { get; set; }
        required public string ConfirmPassword { get; set; }
        required public string UserName { get; set; }
    }
}