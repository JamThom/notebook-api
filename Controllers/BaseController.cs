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

        protected ActionResult ItemResponse<T>(T item) {
            return Ok(new GetItemResponse<T> { Item = item });
        }



        protected ActionResult ItemsResponse<T>(List<T> items) {
            return Ok(new GetItemsResponse<T> { Items = items });
        }

        protected ActionResult UpdatedResponse(string id, string message) {
            return Ok(new Updated { Id = id, Message = message });
        }

        private class Updated
        {
            required public string Id { get; set; }
            required public string Message { get; set; }
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