using Microsoft.AspNetCore.Identity;
using Notebook.Models;
using Notebook.Models.Requests;

namespace Notebook.Features
{
    public class LoginFeature(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
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