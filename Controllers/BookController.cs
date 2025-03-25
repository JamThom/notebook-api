using Microsoft.AspNetCore.Mvc;
using Notebook.Models;
using Notebook.Models.Requests;
using Notebook.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Notebook.Features;
using Notebook.Models.Domain;

namespace Notebook.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/books")]
    public class BooksController : BaseController
    {
        private readonly GetBooksFeature _getBooksFeature;
        private readonly GetBookFeature _getBookFeature;
        private readonly CreateBookFeature _createBookFeature;
        private readonly DeleteBookFeature _deleteBookFeature;
        private readonly UpdateBookFeature _updateBookFeature;

        public BooksController(GetBooksFeature getBooksFeature, GetBookFeature getBookFeature, CreateBookFeature createBookFeature, UserManager<User> userManager, DeleteBookFeature deleteBookFeature, UpdateBookFeature updateBookFeature)
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
            var result = await _getBooksFeature.Execute(user);
            
            if (result.Error == ErrorType.NotFound)
            {
                return NotFound();
            }
            if (result.Response == null)
            {
                return BadRequest(new ErrorResponse { Message = "Invalid request" });
            }

            return ListResponse(result.Response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, UpdateBookRequest book)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var result = await _updateBookFeature.Execute(id, book, user);
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

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> GetBook(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var result = await _getBookFeature.Execute(id, user);
            if (result.Error == ErrorType.NotFound)
            {
                return NotFound();
            }
            if (result.Response == null)
            {
                return BadRequest(new ErrorResponse { Message = "Invalid request" });
            }
            return ItemResponse<BookResponse>(result.Response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateBookRequest book)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var result = await _createBookFeature.Execute(book, user);
            if (result.Error == ErrorType.NameEmpty)
            {
                return BadRequest(new ErrorResponse { Message = "Name cannot be empty" });
            }
            if (result.Error == ErrorType.DuplicateName)
            {
                return BadRequest(new ErrorResponse { Message = "Name already exists" });
            }
            if (result.Response == null)
            {
                return BadRequest(new ErrorResponse { Message = "Invalid request" });
            }
            return CreatedResponse(result.Response.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var user = await GetAuthenticatedUserAsync();
            if (user == null) return AccountNotFound();
            var result = await _deleteBookFeature.Execute(id, user);
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