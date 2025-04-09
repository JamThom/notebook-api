using Microsoft.EntityFrameworkCore;
using Notebook.Constants;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Domain;
using Notebook.Models.Requests;

namespace Notebook.Features

{
    public class UpdateBookFeature : BaseFeature
    {
        public UpdateBookFeature(ApplicationDbContext bookRepository) : base(bookRepository)
        {
        }

        public async Task<FeatureResult<bool>> Execute(string id, UpdateBookRequest request, User user)
        {

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new FeatureResult<bool>
                {
                    ErrorMessage = ErrorMessages.BookNameRequired
                };
            }

            var book = await _ctx.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);
            if (book == null)
            {
                return new FeatureResult<bool>
                {
                    Error = ErrorType.NotFound
                };
            }

            var sameNameBook = await _ctx.Books.FirstOrDefaultAsync(b => b.Name == request.Name && b.UserId == user.Id);
            if (sameNameBook != null && sameNameBook.Id != book.Id)
            {
                return new FeatureResult<bool>
                {
                    ErrorMessage = ErrorMessages.BookNameAlreadyExists
                };
            }

            book.Name = request.Name;

            _ctx.Books.Update(book);
            await _ctx.SaveChangesAsync();

            return new FeatureResult<bool>
            {
                Response = true
            };
        }
    }
}