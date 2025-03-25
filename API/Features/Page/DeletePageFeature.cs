using Notebook.Data;
using Notebook.Models;
using Microsoft.EntityFrameworkCore;
using Notebook.Models.Domain;

namespace Notebook.Features
{
    public class DeletePageFeature : BaseFeature
    {
        public DeletePageFeature(ApplicationDbContext ctx): base(ctx)
        {
        }

        public async Task<FeatureResult<bool>> Execute(string pageId, User user)
        {
            var page = await _ctx.Pages
                .Include(p => p.Book)
                .FirstOrDefaultAsync(p => p.Id == pageId && p.Book.UserId == user.Id);

            if (page == null)
            {
                return new FeatureResult<bool>
                {
                    Error = ErrorType.NotFound,
                    Response = false
                };
            }

            _ctx.Pages.Remove(page);
            await _ctx.SaveChangesAsync();

            return new FeatureResult<bool>
            {
                Response = true
            };
        }
    }
}