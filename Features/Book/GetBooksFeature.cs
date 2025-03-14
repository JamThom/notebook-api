using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Responses;

namespace Notebook.Features
{
    public class GetBooksFeature: BaseFeature
    {

        public GetBooksFeature(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<BooksResponse>> Execute(User user)
        {
            var books = await _ctx.Books.Where(b => b.UserId == user.Id).ToListAsync();

            if (books == null)
            {
                return null;
            }

            var pages = await _ctx.Pages
                .Include(p => p.Book)
                .Where(p => p.Book.UserId == user.Id)
                .ToListAsync();

            return books.Select(b => new BooksResponse
            {
                Id = b.Id,
                Name = b.Name,
                Pages = pages?.Where(p => p.BookId == b.Id).Select(p => new PageResponse
                {
                    Id = p.Id,
                    Index = p.Index,
                    Content = p.Content
                }).ToList() ?? new List<PageResponse>()
            }).ToList();
        }
    }
}