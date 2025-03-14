using Microsoft.AspNetCore.Identity;
using Notebook.Models;
using Notebook.Models.Requests;

namespace Notebook.Features
{
    public class LoginFeature: BaseAccountFeature
    {

        public LoginFeature(UserManager<User> userManager, SignInManager<User> signInManager): base(userManager, signInManager, null)
        {
        }

        public async Task<SignInResult> Execute(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            return await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
        }
    }
}