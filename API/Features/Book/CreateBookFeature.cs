using Notebook.Constants;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Domain;
using Notebook.Models.Requests;
using Notebook.Models.Responses;

namespace Notebook.Features
{
    public class CreateBookFeature : BaseFeature
    {
        public CreateBookFeature(ApplicationDbContext ctx) : base(ctx)
        {
        }

        public async Task<FeatureResult<string>> Execute(CreateBookRequest book, User user)
        {
            if (string.IsNullOrWhiteSpace(book.Name))
            {
                return new FeatureResult<string>
                {
                    ErrorMessage = ErrorMessages.BookNameRequired
                };
            }

            var existingBook = _ctx.Books.FirstOrDefault(b => b.Name == book.Name && b.UserId == user.Id);
            if (existingBook != null)
            {
                return new FeatureResult<string>
                {
                    ErrorMessage = ErrorMessages.BookNameAlreadyExists
                };
            }

            var createdBook = new Book
            {
                Id = Guid.NewGuid().ToString(),
                Name = book.Name,
                UserId = user.Id,
                Pages = new HashSet<Page>(),
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

            return new FeatureResult<string>
            {
                Response = createdBook.Id
            };
        }
    }
}