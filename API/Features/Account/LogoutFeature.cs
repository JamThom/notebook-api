using Microsoft.AspNetCore.Identity;
using Notebook.Models;
using System.Threading.Tasks;

namespace Notebook.Features
{
    public class LogoutFeature(SignInManager<User> signInManager)
    {

        private readonly SignInManager<User> _signInManager = signInManager;

        public async Task Execute()
        {
            await _signInManager.SignOutAsync();
        }
    }
}