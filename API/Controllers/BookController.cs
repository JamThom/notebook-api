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
    public class BooksController(GetBooksFeature getBooksFeature, GetBookFeature getBookFeature, CreateBookFeature createBookFeature, UserManager<User> userManager, DeleteBookFeature deleteBookFeature, UpdateBookFeature updateBookFeature) : BaseController(userManager)
    {


        [HttpGet()]
        public async Task<ActionResult<List<BooksResponse>>> GetBooks()
        {
            return await HandleFeatureExecution((user) => getBooksFeature.Execute(user));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, UpdateBookRequest book)
        {
            return await HandleFeatureExecution((user) => updateBookFeature.Execute(id, book, user));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> GetBook(string id)
        {
            return await HandleFeatureExecution((user) => getBookFeature.Execute(id, user));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateBookRequest book)
        {
            return await HandleFeatureExecution((user) => createBookFeature.Execute(book, user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            return await HandleFeatureExecution((user) => deleteBookFeature.Execute(id, user));
        }
    }
}