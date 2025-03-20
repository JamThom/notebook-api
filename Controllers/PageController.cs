using Microsoft.AspNetCore.Mvc;
using Notebook.Models.Requests;
using Notebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Notebook.Features;
using Notebook.Models.Responses;

namespace Notebook.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/pages")]
    public class PageController(GetPageFeature getPageFeature, CreatePageFeature createPageFeature, UserManager<User> userManager, DeletePageFeature deletePageFeature) : BaseController(userManager)
    {

        private readonly GetPageFeature _getPageFeature = getPageFeature;
        private readonly CreatePageFeature _createPageFeature = createPageFeature;
        private readonly DeletePageFeature _deletePageFeature = deletePageFeature;

        [HttpGet("{id}")]
        public async Task<ActionResult<PageResponse>> GetPage(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var pageResponse = await _getPageFeature.Execute(id, user);
            if (pageResponse == null)
            {
                return NotFound();
            }
            return ItemResponse(pageResponse);
        }

        [HttpPost]
        public async Task<ActionResult<PageResponse>> Post(CreatePageRequest page)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var pageResponse = await _createPageFeature.Execute(page, user);
            if (pageResponse == null)
            {
                return NotFound();
            }
            return UpdatedResponse(pageResponse.Id, "Page created");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePage(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var success = await _deletePageFeature.Execute(id, user);
            if (!success)
            {
                return NotFound();
            }
            return UpdatedResponse(id, "Page deleted");
        }
    }
}