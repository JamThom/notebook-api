using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Notebook.Data;
using Notebook.Models;
using System.Collections.Concurrent;
using Notebook.Models.Requests;
using Notebook.Models.Responses;

namespace Notebook.Hubs
{
    public class PageHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PageHub> _logger;
        private static readonly ConcurrentDictionary<string, (DateTime lastUpdate, string content)> _pageUpdates = new();
        public PageHub(ApplicationDbContext context, UserManager<User> userManager, ILogger<PageHub> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task SendMessage(UpdatePageRequest page)
        {
            try
            {
                if (Context.User == null)
                {
                    await Clients.Caller.SendAsync("Error", "User not authenticated");
                    return;
                }
                var user = await _userManager.GetUserAsync(Context.User);
                if (user == null)
                {
                    await Clients.Caller.SendAsync("Error", "User not found");
                    return;
                }

                var existingPage = await _context.Pages.FindAsync(page.Id);
                var existingBook = await _context.Books.FindAsync(existingPage?.BookId);

                if (existingPage == null || existingBook?.UserId != user.Id)
                {
                    await Clients.Caller.SendAsync("Error", "Not authorized to update this page");
                    return;
                }

                _pageUpdates[page.Id] = (DateTime.UtcNow, page.Content);


                existingPage.Content = page.Content;
                _context.Pages.Update(existingPage);
                await _context.SaveChangesAsync();

                await Clients.All.SendAsync("UpdatePage", new PageResponse {
                    Id = existingPage.Id,
                    Content = existingPage.Content,
                    Index = existingPage.Index
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending message");
                await Clients.Caller.SendAsync("Error", ex.Message.ToString());
            }
        }

    }
}
