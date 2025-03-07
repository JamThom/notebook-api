using Microsoft.AspNetCore.Mvc;
using Notebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Notebook.Features;

namespace Notebook.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/notebooks")]
    public class NotebookController : BaseController
    {
        private readonly GetBooksFeature _getBooksFeature;
        private readonly GetBookFeature _getBookFeature;
        private readonly GetPageFeature _getPageFeature;
        private readonly CreatePageFeature _createPageFeature;
        private readonly CreateNotebookFeature _createNotebookFeature;

        public NotebookController(GetBooksFeature getBooksFeature, GetBookFeature getBookFeature, GetPageFeature getPageFeature, CreatePageFeature createPageFeature, CreateNotebookFeature createNotebookFeature, UserManager<User> userManager)
            : base(userManager)
        {
            _getBooksFeature = getBooksFeature;
            _getBookFeature = getBookFeature;
            _getPageFeature = getPageFeature;
            _createPageFeature = createPageFeature;
            _createNotebookFeature = createNotebookFeature;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<BooksResponse>>> GetBooks()
        {
            var user = await GetAuthenticatedUserAsync();
            var booksResponse = await _getBooksFeature.Execute(user);
            return Ok(booksResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> GetBook(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            var bookResponse = await _getBookFeature.Execute(id, user);
            if (bookResponse == null)
            {
                return NotFound();
            }
            return Ok(bookResponse);
        }

        [HttpGet("{id}/pages/{pageId}")]
        public async Task<ActionResult<PageResponse>> GetPage(string id, string pageId)
        {
            var user = await GetAuthenticatedUserAsync();
            var pageResponse = await _getPageFeature.Execute(pageId, user);
            if (pageResponse == null)
            {
                return NotFound();
            }
            return Ok(pageResponse);
        }

        [HttpPost("{id}/pages")]
        public async Task<ActionResult<PageResponse>> PostPage(string id, CreatePageRequest page)
        {
            var user = await GetAuthenticatedUserAsync();
            var pageResponse = await _createPageFeature.Execute(id, page, user);
            if (pageResponse == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetPage), new { id = pageResponse.Id }, pageResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateBookRequest book)
        {
            var user = await GetAuthenticatedUserAsync();
            var createdBook = await _createNotebookFeature.Execute(book, user);
            if (createdBook == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }
    }
}