using Microsoft.AspNetCore.Mvc;
using Notebook.Models;
using Notebook.Features;
using Notebook.Models.Requests;
using Notebook.Models.Responses;

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
                    return Ok(new { Message = "Registration successful" });
                }
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { Message = "Registration failed", Errors = errors });
            } catch (Exception e) {
                return BadRequest(new { Message = "An error occurred during registration", Error = e.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var result = await _loginFeature.Execute(model);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Login successful" });
            }
            else if (result.IsLockedOut)
            {
                return BadRequest(new { Message = "User account locked out" });
            }
            else if (result.IsNotAllowed)
            {
                return BadRequest(new { Message = "User is not allowed to sign in" });
            }
            else if (result.RequiresTwoFactor)
            {
                return BadRequest(new { Message = "Two-factor authentication is required" });
            }
            else
            {
                return BadRequest(new { Message = "Invalid login attempt" });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _logoutFeature.Execute();
            return Ok(new { Message = "Logout successful" });
        }
    }
}