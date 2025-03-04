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
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var books = _ctx.Books.Where(b => b.UserId == user.Id).ToList();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(string id)
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

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book book)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            book.UserId = user.Id;
            _ctx.Books.Add(book);
            await _ctx.SaveChangesAsync();
            return CreatedAtRoute("GetBooks", new { id = book.Id }, book);
        }
    }
}