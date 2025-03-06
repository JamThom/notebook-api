using Microsoft.AspNetCore.Mvc;
using Notebook.Data;
using Notebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ActionResult<IEnumerable<BooksResponse>>> GetBooks()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var books = _ctx.Books.Where(b => b.UserId == user.Id).ToList();

            var booksResponse = books.Select(b => new BooksResponse
            {
                Id = b.Id,
                Name = b.Name,
            }).ToList();
            return Ok(booksResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponse>> GetBook(string id)
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

            var bookResponse = new BookResponse
            {
                Id = book.Id,
                Name = book.Name,
                Pages = book.Pages?.Select(p => new PageResponse
                {
                    Id = p.Id,
                    Index = p.Index,
                    Content = p.Content
                }).ToList() ?? new List<PageResponse>()
            };

            return Ok(bookResponse);
        }

        [HttpGet("{id}/pages/{pageId}")]
        public async Task<ActionResult<PageResponse>> GetPage(string id, string pageId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var book = _ctx.Books.FirstOrDefault(b => b.Id == id && b.UserId == user.Id);
            if (book == null)
            {
                return NotFound();
            }

            var page = _ctx.Pages.FirstOrDefault(p => p.Id == pageId && p.BookId == book.Id);
            if (page == null)
            {
                return NotFound();
            }

            var pageResponse = new PageResponse
            {
                Id = page.Id,
                Index = page.Index,
                Content = page.Content
            };

            return Ok(pageResponse);
        }

        [HttpPost("{id}/pages")]
        public async Task<ActionResult<PageResponse>> PostPage(string id, CreatePageRequest page)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var book = _ctx.Books.FirstOrDefault(b => b.Id == id && b.UserId == user.Id);
            if (book == null)
            {
                return NotFound();
            }

            var createdPage = new Page
            {
                Id = Guid.NewGuid().ToString(),
                Index = book.Pages.Count,
                Content = page.Content,
                BookId = book.Id
            };

            _ctx.Pages.Add(createdPage);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPage), new { id = createdPage.Id }, createdPage);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateBookRequest book)
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

            var initialPage = new Page
            {
                Id = Guid.NewGuid().ToString(),
                Index = 0,
                Content = "Hello Worble",
                BookId = createdBook.Id
            };

            createdBook.Pages.Add(initialPage);

            _ctx.Books.Add(createdBook);
            _ctx.Pages.Add(initialPage);
            await _ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }
    }
}