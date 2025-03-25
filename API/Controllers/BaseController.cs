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

        protected async Task<User?> GetAuthenticatedUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            
            return user;
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

        public class ErrorResponse
        {
            required public string Message { get; set; }
            public string? Error { get; set; }
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