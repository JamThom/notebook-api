using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Responses;

namespace Notebook.Features
{
    public class GetAccountFeature
    {
        private readonly ApplicationDbContext _ctx;

        public GetAccountFeature(ApplicationDbContext context)
        {
            _ctx = context;
        }

        public async Task<AccountResponse> Execute(User user)
        {
            var account = await _ctx.Users.FirstOrDefaultAsync(a => a.Id == user.Id);

            if (account == null)
            {
                return null;
            }

            return new AccountResponse
            {
                Id = account.Id,
                UserName = account.UserName,
                Email = account.Email,
            };
        }
    }
}