using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Domain;
using Notebook.Models.Requests;

namespace Notebook.Features

{
    public class UpdateBookFeature: BaseFeature
    {
        public UpdateBookFeature(ApplicationDbContext bookRepository): base(bookRepository)
        {
        }

        public async Task<FeatureResult<bool>> Execute(string id, UpdateBookRequest request, User user)
        {
            var book = await _ctx.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);
            if (book == null)
            {
                return new FeatureResult<bool>
                {
                    Error = ErrorType.NotFound
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