using Microsoft.EntityFrameworkCore;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Domain;
using Notebook.Models.Responses;

namespace Notebook.Features
{
    public class GetAccountFeature(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _ctx = context;

        public async Task<FeatureResult<AccountResponse>> Execute(User user)
        {
            var account = await _ctx.Users.FirstOrDefaultAsync(a => a.Id == user.Id);

            if (account == null)
            {
                return new FeatureResult<AccountResponse>
                {
                    Error = ErrorType.NotFound
                };
            }

            return new FeatureResult<AccountResponse>
            {
                Response = new AccountResponse
                {
                    Id = account.Id,
                    UserName = account.UserName,
                    Email = account.Email,
                }
            };
        }
    }
}