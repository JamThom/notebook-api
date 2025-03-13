using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Notebook.Data;
using Notebook.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;

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

                _ = DebounceUpdatePage(page.Id, existingPage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending message");
                await Clients.Caller.SendAsync("Error", ex.Message.ToString());
            }
        }

        private async Task DebounceUpdatePage(string pageId, Page existingPage)
        {
            await Task.Delay(1000);

            if (_pageUpdates.TryGetValue(pageId, out var update) && (DateTime.UtcNow - update.lastUpdate).TotalMilliseconds >= 1000)
            {
                existingPage.Content = update.content;
                _context.Pages.Update(existingPage);
                await _context.SaveChangesAsync();

                await Clients.All.SendAsync("UpdatePage", new PageResponse { Id = existingPage.Id, Content = existingPage.Content });

                _pageUpdates.TryRemove(pageId, out _);
            }
        }
    }
}
