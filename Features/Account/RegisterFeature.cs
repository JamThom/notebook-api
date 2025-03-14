using Microsoft.AspNetCore.Identity;
using Notebook.Models;
using Notebook.Models.Requests;

namespace Notebook.Features
{
    public class RegisterFeature: BaseAccountFeature
    {

        public RegisterFeature(UserManager<User> userManager, SignInManager<User> signInManager): base(userManager, signInManager, null)
        {
        }

        public async Task<User> Execute(RegisterRequest model)
        {
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                Id = Guid.NewGuid().ToString(),
                Books = new HashSet<Book> {}
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create user");
            }
            return user;
        }
    }
}