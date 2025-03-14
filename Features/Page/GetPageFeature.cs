using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Responses;

namespace Notebook.Features
{
    public class GetPageFeature
    {
        private readonly ApplicationDbContext _ctx;

        public GetPageFeature(ApplicationDbContext context)
        {
            _ctx = context;
        }

        public async Task<PageResponse> Execute(string pageId, User user)
        {
            var page = await _ctx.Pages.FirstOrDefaultAsync(p => p.Id == pageId);
            if (page == null || page.Book.UserId != user.Id)
            {
                return null;
            }

            return new PageResponse
            {
                Id = page.Id,
                Index = page.Index,
                Content = page.Content
            };
        }
    }
}