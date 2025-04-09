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
    public class PageController(
        CreatePageFeature createPageFeature,
        UserManager<User> userManager,
        DeletePageFeature deletePageFeature
    ) : BaseController(userManager)
    {
        private readonly CreatePageFeature _createPageFeature = createPageFeature;
        private readonly DeletePageFeature _deletePageFeature = deletePageFeature;

        [HttpPost]
        public async Task<ActionResult<PageResponse>> Post(CreatePageRequest page)
        {
            return await HandleFeatureExecution((user) => _createPageFeature.Execute(page, user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePage(string id)
        {
            return await HandleFeatureExecution((user) => _deletePageFeature.Execute(id, user));
        }
    }
}