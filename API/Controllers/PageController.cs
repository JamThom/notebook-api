using Microsoft.AspNetCore.Mvc;
using Notebook.Models.Requests;
using Notebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Notebook.Features;
using Notebook.Models.Responses;
using Notebook.Models.Domain;

namespace Notebook.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/pages")]
    public class PageController(CreatePageFeature createPageFeature, UserManager<User> userManager, DeletePageFeature deletePageFeature) : BaseController(userManager)
    {
        private readonly CreatePageFeature _createPageFeature = createPageFeature;
        private readonly DeletePageFeature _deletePageFeature = deletePageFeature;

        [HttpPost]
        public async Task<ActionResult<PageResponse>> Post(CreatePageRequest page)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var result = await _createPageFeature.Execute(page, user);
            if (result.Error == ErrorType.NotFound)
            {
                return NotFound("Book not found");
            }
            if (result.Response == null)
            {
                return BadRequest(new ErrorResponse { Message = "Invalid request" });
            }
            return Ok(result.Response.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePage(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var result = await _deletePageFeature.Execute(id, user);
            if (result.Error == ErrorType.NotFound)
            {
                return NotFound();
            }
            if (result.Error != null)
            {
                return BadRequest(new ErrorResponse { Message = "Invalid request" });
            }
            return Ok();
        }
    }
}