using Microsoft.AspNetCore.Mvc;
using Notebook.Models;
using Notebook.Features;

namespace Notebook.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly RegisterFeature _registerFeature;
        private readonly LoginFeature _loginFeature;
        private readonly LogoutFeature _logoutFeature;

        public AccountController(RegisterFeature registerFeature, LoginFeature loginFeature, LogoutFeature logoutFeature)
        {
            _registerFeature = registerFeature;
            _loginFeature = loginFeature;
            _logoutFeature = logoutFeature;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try {
                var result = await _registerFeature.Execute(model);
                if (result.Succeeded)
                {
                    return Ok();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(result.Errors);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var result = await _loginFeature.Execute(model);
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
            await _logoutFeature.Execute();
            return Ok();
        }
    }
}