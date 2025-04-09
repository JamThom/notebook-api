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

        protected ActionResult ItemResponse<T>(T item) where T : notnull
        {
            return Ok(new GetItemResponse<T> { Item = item });
        }

        protected ActionResult ListResponse<T>(List<T> items) where T : notnull {
            return Ok(new GetItemsResponse<T> { Items = items });
        }

        protected ActionResult CreatedResponse(string id)
        {
            return Ok(id);
        }

        protected ActionResult ErrorResponse<T>(FeatureResult<T> result)
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

        private class GetItemResponse<T>
        {
            public required T Item { get; set; }
        }

        private class GetItemsResponse<T>
        {
            public required List<T> Items { get; set; }
        }
    }
}