using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Responses;

namespace Notebook.Features
{
    public class UpdateAccountFeature
    {
        private readonly ApplicationDbContext _ctx;

        public UpdateAccountFeature(ApplicationDbContext context)
        {
            _ctx = context;
        }

        public async Task<AccountResponse> Execute(User user)
        {
            var account = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (account == null)
            {
                return null;
            }

            return new AccountResponse
            {
                Id = account.Id,
                UserName = account.UserName,
                Email = account.Email
            };
        }
    }
}