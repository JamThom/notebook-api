using Microsoft.AspNetCore.SignalR;
using Notebook.Models;

namespace Notebook.Hubs
{
    public class PageHub: Hub
    {
        public async Task UpdatePage(Page page)
        {
            await Clients.All.SendAsync("UpdatePage", page);
        }
    }
}