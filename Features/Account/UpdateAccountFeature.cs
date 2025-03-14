using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Requests;
using Notebook.Models.Responses;

namespace Notebook.Features
{
    public class UpdateAccountFeature
    {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<User> _userManager;

        public UpdateAccountFeature(ApplicationDbContext context, UserManager<User> userManager)
        {
            _ctx = context;
            _userManager = userManager;
        }

        public async Task<Boolean> Execute(UpdateAccountRequest request, User user)
        {

            var account = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (account == null)
            {
                return false;
            }

            account.UserName = request.UserName;
            account.Email = request.Email;

            await _ctx.SaveChangesAsync();

            return true;

        }
    }
}