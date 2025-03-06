using Microsoft.AspNetCore.Mvc;
using Notebook.Data;
using Notebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/notebooks")]
    public class NotebookController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<User> _userManager;

        public NotebookController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _ctx = context;
            _userManager = userManager;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<BooksResponseModel>>> GetBooks()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var books = _ctx.Books.Where(b => b.UserId == user.Id).ToList();

            var booksResponse = books.Select(b => new BooksResponseModel
            {
                Id = b.Id,
                Name = b.Name,
            }).ToList();
            return Ok(booksResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponseModel>> GetBook(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var book = _ctx.Books.FirstOrDefault(b => b.Id == id && b.UserId == user.Id);
            if (book == null)
            {
                return NotFound();
            }

            var bookResponse = new BookResponseModel
            {
                Id = book.Id,
                Name = book.Name,
                Pages = book.Pages?.Select(p => new PageResponseModel
                {
                    Id = p.Id,
                    Index = p.Index,
                    Content = p.Content
                }).ToHashSet() ?? new HashSet<PageResponseModel>()
            };

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateBookRequestModel book)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var createdBook = new Book
            {
                Id = Guid.NewGuid().ToString(),
                Name = book.Name,
                UserId = user.Id,
                Pages = new HashSet<Page> {},
                User = user
            };

            _ctx.Books.Add(createdBook);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }
    }
}