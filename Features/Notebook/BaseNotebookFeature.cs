using Notebook.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notebook.Models;

namespace Notebook.Features
{
    public abstract class BaseNotebookFeature: ControllerBase
    {
        protected readonly UserManager<User> _userManager;
        protected readonly ApplicationDbContext _ctx;

        protected BaseNotebookFeature(UserManager<User> userManager, ApplicationDbContext ctx)
        {
            _userManager = userManager;
            _ctx = ctx;
        }
    }
}