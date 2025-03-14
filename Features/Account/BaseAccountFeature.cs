using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notebook.Data;
using Notebook.Models;

namespace Notebook.Features
{
    public abstract class BaseAccountFeature: ControllerBase
    {
        protected readonly UserManager<User> _userManager;
        protected readonly SignInManager<User> _signInManager;
        protected readonly ApplicationDbContext _ctx;

        protected BaseAccountFeature(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ctx = context;
        }
    }
}