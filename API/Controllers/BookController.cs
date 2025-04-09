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
            return await HandleFeatureExecution((user) => _getBooksFeature.Execute(user));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, UpdateBookRequest book)
        {
            return await HandleFeatureExecution((user) => _updateBookFeature.Execute(id, book, user));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> GetBook(string id)
        {
            return await HandleFeatureExecution((user) => _getBookFeature.Execute(id, user));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateBookRequest book)
        {
            return await HandleFeatureExecution((user) => _createBookFeature.Execute(book, user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            return await HandleFeatureExecution((user) => _deleteBookFeature.Execute(id, user));
        }
    }
}