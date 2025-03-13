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
        private readonly CreateNotebookFeature _createNotebookFeature;
        private readonly DeleteNotebookFeature _deleteBookFeature;

        public NotebookController(GetBooksFeature getBooksFeature, GetBookFeature getBookFeature, CreateNotebookFeature createNotebookFeature, UserManager<User> userManager, DeleteNotebookFeature deleteBookFeature)
            : base(userManager)
        {
            _getBooksFeature = getBooksFeature;
            _getBookFeature = getBookFeature;
            _createNotebookFeature = createNotebookFeature;
            _deleteBookFeature = deleteBookFeature;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<BooksResponse>>> GetBooks()
        {
            var user = await GetAuthenticatedUserAsync();
            var booksResponse = await _getBooksFeature.Execute(user);

            if (booksResponse == null)
            {
                return NotFound();
            }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            var book = await _deleteBookFeature.Execute(id, user);
            if (book == false)
            {
                return NotFound();
            }
            await _deleteBookFeature.Execute(id, user);
            return NoContent();
        }
    }
}