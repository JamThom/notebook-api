using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Requests;

namespace Notebook.Features

{
    public class UpdateBookFeature: BaseFeature
    {
        public UpdateBookFeature(ApplicationDbContext bookRepository): base(bookRepository)
        {
        }

        public async Task<bool> Execute(string id, UpdateBookRequest request, User user)
        {
            var book = await _ctx.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);
            if (book == null)
            {
                return false;
            }

            book.Name = request.Name;

            _ctx.Books.Update(book);
            await _ctx.SaveChangesAsync();

            return true;
        }
    }
}