using Notebook.Data;
using Microsoft.AspNetCore.Mvc;

namespace Notebook.Features
{
    public abstract class BaseFeature : ControllerBase
    {
        protected readonly ApplicationDbContext _ctx;

        protected BaseFeature(ApplicationDbContext context)
        {
            _ctx = context;
        }
    }
}