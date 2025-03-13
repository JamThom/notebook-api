using Microsoft.AspNetCore.Mvc;
using Notebook.Data;
using Notebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Notebook.Features;

namespace Notebook.Features
{
    public class CreateNotebookFeature : BaseNotebookFeature
    {
        public CreateNotebookFeature(UserManager<User> userManager, ApplicationDbContext ctx): base(userManager, ctx)
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

            createdBook.Pages.Add(initialPage);

            _ctx.Books.Add(createdBook);
            _ctx.Pages.Add(initialPage);
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