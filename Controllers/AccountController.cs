using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notebook.Models;
using System;
using System.Threading.Tasks;

namespace Notebook.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                Id = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok();
            }
            else if (result.IsLockedOut)
            {
                return BadRequest("User account locked out.");
            }
            else if (result.IsNotAllowed)
            {
                return BadRequest("User is not allowed to sign in.");
            }
            else if (result.RequiresTwoFactor)
            {
                return BadRequest("Two-factor authentication is required.");
            }
            else
            {
                return BadRequest("Invalid login attempt.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}