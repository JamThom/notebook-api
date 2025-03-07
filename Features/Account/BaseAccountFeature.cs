using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notebook.Models;

namespace Notebook.Features
{
    public abstract class BaseAccountFeature: ControllerBase
    {
        protected readonly UserManager<User> _userManager;
        protected readonly SignInManager<User> _signInManager;

        protected BaseAccountFeature(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
    }
}