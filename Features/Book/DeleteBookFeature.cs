using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;

namespace Notebook.Features
{
    public class DeleteBookFeature: BaseFeature
    {
        public DeleteBookFeature(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> Execute(string bookId, User user)
        {
            var book = await _ctx.Books.FirstOrDefaultAsync(p => p.Id == bookId);
            if (book == null || book.UserId != user.Id)
            {
                return false;
            }

            _ctx.Books.Remove(book);
            await _ctx.SaveChangesAsync();

            return true;
        }
    }
}