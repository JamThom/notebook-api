using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Requests;
using Notebook.Models.Responses;

namespace Notebook.Features
{
    public class CreateBookFeature : BaseFeature
    {
        public CreateBookFeature(ApplicationDbContext ctx): base(ctx)
        {
        }
        public async Task<BookResponse> Execute(CreateBookRequest book, User user)
        {
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
                Content = "",
                BookId = createdBook.Id,
                Book = createdBook
            };


            var createdPage = _ctx.Pages.Add(initialPage).Entity;

            createdBook.Pages.Add(createdPage);

            _ctx.Books.Add(createdBook);

            await _ctx.SaveChangesAsync();

            return new BookResponse {
                Id = createdBook.Id,
                Name = createdBook.Name,
                Pages = createdBook.Pages.Select(p => new PageResponse {
                    Id = p.Id,
                    Index = p.Index,
                    Content = p.Content
                }).ToList()
            };
        }
    }
}