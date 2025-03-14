using Microsoft.AspNetCore.Identity;
using Notebook.Models;
using Notebook.Models.Requests;

namespace Notebook.Features
{
    public class RegisterFeature(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;

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