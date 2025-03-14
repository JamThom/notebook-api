using Microsoft.AspNetCore.Mvc;
using Notebook.Models;
using Notebook.Models.Requests;
using Notebook.Models.Responses;
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
        private readonly UpdateNotebookFeature _updateBookFeature;

        public NotebookController(GetBooksFeature getBooksFeature, GetBookFeature getBookFeature, CreateNotebookFeature createNotebookFeature, UserManager<User> userManager, DeleteNotebookFeature deleteBookFeature, UpdateNotebookFeature updateBookFeature)
            : base(userManager)
        {
            _updateBookFeature = updateBookFeature;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, UpdateNotebookRequest book)
        {
            var user = await GetAuthenticatedUserAsync();
            var updatedBook = await _updateBookFeature.Execute(id, book, user);
            if (updatedBook == false)
            {
                return NotFound();
            }
            return Ok(updatedBook);
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