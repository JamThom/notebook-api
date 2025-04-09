using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Notebook.Constants;
using Notebook.Data;
using Notebook.Models;
using Notebook.Models.Domain;
using Notebook.Models.Requests;
using Xunit.Sdk;

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

        public async Task<FeatureResult<bool>> Execute(UpdateAccountRequest request, User user)
        {

            var account = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (account == null)
            {
                return new FeatureResult<bool>
                {
                    ErrorMessage = ErrorMessages.UserNotFound,
                };
            }

            if (string.IsNullOrEmpty(request.UserName)) {
                return new FeatureResult<bool>
                {
                    ErrorMessage = ErrorMessages.UserNameRequired,
                };
            }

            if (!string.IsNullOrEmpty(request.UserName))
            {
                var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
                if (userWithSameUserName != null && userWithSameUserName.Id != user.Id)
                {
                    return new FeatureResult<bool>
                    {
                        ErrorMessage = ErrorMessages.UserNameAlreadyExists,
                    };
                }
            }

            account.UserName = request.UserName;

            await _ctx.SaveChangesAsync();

            return new FeatureResult<bool>
            {
                Response = true
            };

        }
    }
}