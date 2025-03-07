using Microsoft.AspNetCore.Mvc;
using Notebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Notebook.Features;

namespace Notebook.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/pages")]
    public class PageController : BaseController
    {

        private readonly GetPageFeature _getPageFeature;
        private readonly CreatePageFeature _createPageFeature;
        private readonly DeletePageFeature _deletePageFeature;

        public PageController(GetPageFeature getPageFeature, CreatePageFeature createPageFeature, UserManager<User> userManager, DeletePageFeature deletePageFeature)
            : base(userManager)
        {
            _getPageFeature = getPageFeature;
            _createPageFeature = createPageFeature;
            _deletePageFeature = deletePageFeature;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PageResponse>> GetPage(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            var pageResponse = await _getPageFeature.Execute(id, user);
            if (pageResponse == null)
            {
                return NotFound();
            }
            return Ok(pageResponse);
        }

        [HttpPost]
        public async Task<ActionResult<PageResponse>> Post(CreatePageRequest page)
        {
            var user = await GetAuthenticatedUserAsync();
            var pageResponse = await _createPageFeature.Execute(page, user);
            if (pageResponse == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetPage), new { id = pageResponse.Id }, pageResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePage(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            var success = await _deletePageFeature.Execute(id, user);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}