using Microsoft.AspNetCore.Identity;
using Notebook.Models;
using System.Threading.Tasks;

namespace Notebook.Features
{
    public class LogoutFeature: BaseAccountFeature
    {

        public LogoutFeature(SignInManager<User> signInManager): base(null, signInManager, null)
        {}

        public async Task Execute()
        {
            await _signInManager.SignOutAsync();
        }
    }
}