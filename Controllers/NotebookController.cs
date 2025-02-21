using Microsoft.AspNetCore.Mvc;
using Notebook.Data;
using Notebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Notebook.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class BookController : ControllerBase
{

    private readonly ILogger<BookController> _logger;
    private readonly DbContext _ctx;
    private readonly UserManager<User> _userManager;

    public BookController(ILogger<BookController> logger, DbContext context, UserManager<User> userManager)
    {
        _ctx = context;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet(Name = "GetBooks")]
    public async Task<ActionResult<IEnumerable<Book>>> Get()
    {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var books = _ctx.Books.Where(b => b.UserId == user.Id).ToList();
            return Ok(books);
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
