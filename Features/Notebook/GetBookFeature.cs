using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Responses;

namespace Notebook.Features
{
    public class GetBookFeature
    {
        private readonly ApplicationDbContext _ctx;

        public GetBookFeature(ApplicationDbContext context)
        {
            _ctx = context;
        }

        public async Task<BookResponse> Execute(string id, User user)
        {
            var book = await _ctx.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);

            var pages = await _ctx.Pages
                .Include(p => p.Book)
                .Where(p => p.Book.UserId == user.Id).ToListAsync();

            if (book == null)
            {
                return null;
            }

            return new BookResponse
            {
                Id = book.Id,
                Name = book.Name,
                Pages = pages?.Select(p => new PageResponse
                {
                    Id = p.Id,
                    Index = p.Index,
                    Content = p.Content
                }).ToList() ?? new List<PageResponse>()
            };
        }
    }
}