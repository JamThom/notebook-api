using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Domain;

namespace Notebook.Features
{
    public class DeleteBookFeature: BaseFeature
    {
        public DeleteBookFeature(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<FeatureResult<bool>> Execute(string bookId, User user)
        {
            var book = await _ctx.Books.FirstOrDefaultAsync(p => p.Id == bookId);
            if (book == null || book.UserId != user.Id)
            {
                return new FeatureResult<bool>
                {
                    Error = ErrorType.NotFound
                };
            }

            _ctx.Books.Remove(book);
            await _ctx.SaveChangesAsync();

            return new FeatureResult<bool>
            {
                Response = true
            };
        }
    }
}