using Notebook.Data;
using Notebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Notebook.Features
{
    public class CreatePageFeature : BaseNotebookFeature
    {
        public CreatePageFeature(UserManager<User> userManager, ApplicationDbContext ctx): base(userManager, ctx)
        {
        }

        public async Task<PageResponse> Execute(string bookId, CreatePageRequest page, User user)
        {
            var book = await _ctx.Books.FirstOrDefaultAsync(b => b.Id == bookId && b.UserId == user.Id);
            if (book == null)
            {
                return null;
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

            return new PageResponse
            {
                Id = createdPage.Id,
                Index = createdPage.Index,
                Content = createdPage.Content
            };
        }
    }
}