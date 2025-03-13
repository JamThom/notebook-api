using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;

namespace Notebook.Features
{
    public class GetBooksFeature
    {
        private readonly ApplicationDbContext _ctx;

        public GetBooksFeature(ApplicationDbContext context)
        {
            _ctx = context;
        }

        public async Task<IEnumerable<BooksResponse>> Execute(User user)
        {
            var books = await _ctx.Books.Where(b => b.UserId == user.Id).ToListAsync();

            return books.Select(b => new BooksResponse
            {
                Id = b.Id,
                Name = b.Name,
                Pages = b.Pages.Select(p => new PageResponse
                {
                    Id = p.Id,
                    Index = p.Index,
                    Content = p.Content
                }).ToList()
            }).ToList();
        }
    }
}