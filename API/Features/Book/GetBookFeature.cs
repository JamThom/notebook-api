using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Domain;
using Notebook.Models.Responses;

namespace Notebook.Features
{
    public class GetBookFeature : BaseFeature
    {

        public GetBookFeature(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<FeatureResult<BookResponse>> Execute(string id, User user)
        {
            var book = await _ctx.Books
                .Include(b => b.Pages)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);

            var pages = await _ctx.Pages
                .Include(p => p.Book)
                .Where(p => p.Book.UserId == user.Id && p.BookId == id)
                .ToListAsync();

            if (book == null)
            {
                return new FeatureResult<BookResponse>
                {
                    Error = ErrorType.NotFound
                };
            }

            return new FeatureResult<BookResponse>
            {

                Response = new BookResponse
                {
                    Id = book.Id,
                    Name = book.Name,
                    Pages = book.Pages?.Select(p => new PageResponse
                    {
                        Id = p.Id,
                        Index = p.Index,
                        Content = p.Content
                    }).ToList() ?? new List<PageResponse>()
                }
            };
        }
    }
}