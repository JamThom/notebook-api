using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notebook.Models;
using Notebook.Models.Domain;

namespace Notebook.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly UserManager<User> _userManager;

        protected BaseController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected async Task<User?> GetAuthenticatedUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            
            return user;
        }

        protected async Task<ActionResult> HandleFeatureExecution<TResponse>(
            Func<User, Task<FeatureResult<TResponse>>> featureExecution)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var result = await featureExecution(user);
            if (result.Error != null) return ErrorResponse(result);
            return Ok(result.Response);
        }

        protected ActionResult AccountNotFound()
        {
            return NotFound(new { Message = "Account not found" });
        }

        private protected ActionResult ErrorResponse<T>(FeatureResult<T> result)
        {
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { Message = result.ErrorMessage });
            }
            if (result.Error == ErrorType.NotFound)
            {
                return NotFound();
            }
            return BadRequest(new { Message = "Invalid request" });
        }
    }
}