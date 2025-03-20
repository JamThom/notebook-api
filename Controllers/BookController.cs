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
    [Route("api/books")]
    public class BookController : BaseController
    {
        private readonly GetBooksFeature _getBooksFeature;
        private readonly GetBookFeature _getBookFeature;
        private readonly CreateBookFeature _createBookFeature;
        private readonly DeleteBookFeature _deleteBookFeature;
        private readonly UpdateBookFeature _updateBookFeature;

        public BookController(GetBooksFeature getBooksFeature, GetBookFeature getBookFeature, CreateBookFeature createBookFeature, UserManager<User> userManager, DeleteBookFeature deleteBookFeature, UpdateBookFeature updateBookFeature)
            : base(userManager)
        {
            _getBooksFeature = getBooksFeature;
            _getBookFeature = getBookFeature;
            _createBookFeature = createBookFeature;
            _deleteBookFeature = deleteBookFeature;
            _updateBookFeature = updateBookFeature;
        }

        [HttpGet()]
        public async Task<ActionResult<List<BooksResponse>>> GetBooks()
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return NotFound(new { Message = "Account not found" });
            var booksResponse = await _getBooksFeature.Execute(user);

            if (booksResponse == null)
            {
                return NotFound();
            }

            return ListResponse(booksResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, UpdateBookRequest book)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var updatedBook = await _updateBookFeature.Execute(id, book, user);
            if (updatedBook == false)
            {
                return NotFound();
            }
            return UpdatedResponse(id, "Book updated");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> GetBook(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var bookResponse = await _getBookFeature.Execute(id, user);
            if (bookResponse == null)
            {
                return NotFound();
            }
            return ItemResponse(bookResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateBookRequest book)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var createdBook = await _createBookFeature.Execute(book, user);
            if (createdBook == null)
            {
                return BadRequest();
            }
            return UpdatedResponse(createdBook.Id, "Book created");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var book = await _deleteBookFeature.Execute(id, user);
            if (book == false)
            {
                return NotFound();
            }
            await _deleteBookFeature.Execute(id, user);
            return UpdatedResponse(id, "Book deleted");
        }
    }
}