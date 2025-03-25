using Notebook.Data;
using Notebook.Models;
using Microsoft.EntityFrameworkCore;
using Notebook.Models.Requests;
using Notebook.Models.Responses;
using Notebook.Models.Domain;

namespace Notebook.Features
{
    public class CreatePageFeature : BaseFeature
    {
        public CreatePageFeature(ApplicationDbContext ctx) : base(ctx)
        {
        }

        public async Task<FeatureResult<PageResponse>> Execute(CreatePageRequest page, User user)
        {
            var book = await _ctx.Books.Include(b => b.Pages).FirstOrDefaultAsync(b => b.Id == page.BookId && b.UserId == user.Id);

            if (book == null)
            {
                return new FeatureResult<PageResponse>
                {
                    Error = ErrorType.NotFound
                };
            }

            if (book.Pages == null)
            {
                book.Pages = new HashSet<Page>();
            }

            var createdPage = new Page
            {
                Id = Guid.NewGuid().ToString(),
                Index = book.Pages.Count,
                Content = page.Content,
                BookId = book.Id,
                Book = book
            };

            book.Pages.Add(createdPage);

            _ctx.Pages.Add(createdPage);
            await _ctx.SaveChangesAsync();

            return new FeatureResult<PageResponse>
            {
                Response = new PageResponse
                {
                    Id = createdPage.Id,
                    Index = createdPage.Index,
                    Content = createdPage.Content
                }
            };
        }
    }
}