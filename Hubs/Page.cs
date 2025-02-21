using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Notebook.Data;
using Notebook.Models;

namespace Notebook.Hubs
{
    public class PageHub : Hub
    {
        private readonly DbContext _context;
        private readonly UserManager<User> _userManager;

        public PageHub(DbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task UpdatePage(Page page)
        {
            var user = await _userManager.GetUserAsync(Context.User);
            if (user == null)
            {
                await Clients.Caller.SendAsync("Error", "User not found");
                return;
            }

            var existingPage = await _context.Pages.FindAsync(page.Id);
            if (existingPage == null || existingPage.Book.UserId != user.Id)
            {
                await Clients.Caller.SendAsync("Error", "Not authorized to update this page");
                return;
            }

            existingPage.Content = page.Content;
            _context.Pages.Update(existingPage);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("UpdatePage", existingPage);
        }
    }
}