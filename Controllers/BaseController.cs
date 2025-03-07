using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notebook.Models;

namespace Notebook.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly UserManager<User> _userManager;

        protected BaseController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected async Task<User> GetAuthenticatedUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }
            return user;
        }
    }
}