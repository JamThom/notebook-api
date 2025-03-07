using Microsoft.AspNetCore.Identity;
using Notebook.Models;
using System.Threading.Tasks;

namespace Notebook.Features
{
    public class LogoutFeature
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutFeature(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task Execute()
        {
            await _signInManager.SignOutAsync();
        }
    }
}