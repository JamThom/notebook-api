using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;

namespace Notebook.Features
{
    public class DeleteNotebookFeature
    {
        private readonly ApplicationDbContext _ctx;

        public DeleteNotebookFeature(ApplicationDbContext context)
        {
            _ctx = context;
        }

        public async Task<bool> Execute(string NotebookId, User user)
        {
            var Notebook = await _ctx.Books.FirstOrDefaultAsync(p => p.Id == NotebookId);
            if (Notebook == null || Notebook.UserId != user.Id)
            {
                return false;
            }

            _ctx.Books.Remove(Notebook);
            await _ctx.SaveChangesAsync();

            return true;
        }
    }
}