using Microsoft.AspNetCore.SignalR;
using Notebook.Data;
using Notebook.Models;

namespace Notebook.Hubs
{
    public class PageHub: Hub
    {
        private readonly DbContext _context;
        public PageHub(DbContext context)
        {
            _context = context;
        }
        public async Task UpdatePage(Page page)
        {
            _context.Pages.Update(page);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("UpdatePage", page);
        }
    }
}